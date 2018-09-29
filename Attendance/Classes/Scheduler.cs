using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Threading;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Server;
using Quartz.Impl.Matchers;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Diagnostics;

namespace Attendance.Classes
{
    public class Scheduler
    {
        public static IScheduler scheduler;
        
        public static IMqttServer mqts;
        public static IMqttClient mqtc;
        
        private static string Errfilepath = Utils.Helper.GetErrLogFilePath();
        private static string Loginfopath = Utils.Helper.GetInfoLogFilePath();

        public static bool _StatusAutoTimeSet = false;
        public static bool _StatusAutoDownload = false;
        public static bool _StatusAutoProcess = false;
        public static bool _StatusAutoArrival = false;
        public static bool _StatusWorker = false;
        public static bool _ShutDown = false;

        public void Start()
        {
            if (!scheduler.IsStarted)
            {

                //attach job listener if required...
                if (Globals.G_JobNotificationFlg && !string.IsNullOrEmpty(Globals.G_JobNotificationEmail))
                {
                    scheduler.ListenerManager.AddJobListener(new DummyJobListener(), GroupMatcher<JobKey>.GroupStartsWith("Job_"));
                }
                
                scheduler.Start();  
              
                _StatusAutoTimeSet = false;
                _StatusAutoDownload = false;
                _StatusAutoProcess = false;
                _StatusAutoArrival = false;
                _ShutDown = false;
            }

        }

        public IScheduler GetScheduler()
        {
            return scheduler;
        }

        public static void Publish(ServerMsg tMsg)
        {
            string sendstr = tMsg.ToString();

            var message = new MqttApplicationMessageBuilder()
                .WithTopic("Server/Status")
                .WithPayload(sendstr)
                .WithExactlyOnceQoS()
                .WithRetainFlag()
                .Build();

            mqtc.PublishAsync(message);
        }

        public void Stop()
        {
            if (!scheduler.IsShutdown)
            {
                scheduler.Clear();
                _ShutDown = true;
                scheduler.Shutdown(false);
                mqtc.DisconnectAsync();               
                mqts.StopAsync();
                
            }

        }

        public void StartMQTTClient()
        {
           
            var clientoptions = new MqttClientOptionsBuilder()
            .WithTcpServer(Globals.G_ServerWorkerIP, 1884) // Port is optional
            .Build();

            mqtc = new MqttFactory().CreateMqttClient();
            mqtc.ConnectAsync(clientoptions);

            mqtc.Disconnected += async (s, evtdisconnected) =>
            {
                if (_ShutDown)
                    return;

                await Task.Delay(TimeSpan.FromSeconds(5));
                try
                {
                    await mqtc.ConnectAsync(clientoptions);
                }
                catch
                {
                
                }
            };

        }

        public void StartMQTTServer()
        {

            System.Net.IPAddress serverip = System.Net.IPAddress.Parse(Globals.G_ServerWorkerIP);

            // Configure MQTT server.
            var serveroptionsBuilder = new MqttServerOptionsBuilder()
                .WithConnectionBacklog(100)
                .WithDefaultEndpointPort(1884)
                .WithDefaultEndpointBoundIPAddress(serverip)
                .Build();
            mqts = new MqttFactory().CreateMqttServer();
            mqts.StartAsync(serveroptionsBuilder);

            //mqts.ClientConnected += (s, ect) =>
            //{
            //    SetText("### CONNECTED Client ###" + ect.Client.ClientId + Environment.NewLine);
            //};

        }

        public Scheduler()
        {   
           var properties = new System.Collections.Specialized.NameValueCollection();
           properties["quartz.threadPool.threadCount"] = "20";
           properties["quartz.threadPool.type"] = "Quartz.Simpl.SimpleThreadPool, Quartz";
           properties["quartz.scheduler.instanceName"] = "AttendanceScheduler";

            StdSchedulerFactory schedulerFactory = new StdSchedulerFactory(properties); //getting the scheduler factory
            scheduler = schedulerFactory.GetScheduler();//getting the instance
           
           StartMQTTServer();
           StartMQTTClient();
        }

        public void Restart()
        {
            if (!scheduler.IsShutdown)
            {
                scheduler.Clear();
                _ShutDown = true;                
            }


            //this is required for take new changes in sceduler
            Globals.GetGlobalVars();

            RegSchedule_AutoTimeSet();
            RegSchedule_WorkerProcess();
            RegSchedule_AutoArrival();
            RegSchedule_AutoProcess();
            RegSchedule_DownloadPunch();
            RegSchedule_BlockUnBlockProcess();
            RegSchedule_GatePassPunchProcess();
            _ShutDown = false;  
        }

        public void RegSchedule_DownloadPunch()
        {
            bool hasrow = Globals.G_DsAutoLog.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);
            if (hasrow)
            {
                foreach (DataRow dr in Globals.G_DsAutoLog.Tables[0].Rows)
                {
                    TimeSpan tTime = (TimeSpan)dr["SchTime"];
                    string jobid = "Job_AutoDownload_" + tTime.Hours.ToString() + tTime.Minutes.ToString();
                    string triggerid = "Trigger_AutoDownload_" + tTime.Hours.ToString() + tTime.Minutes.ToString();
                    // define the job and tie it to our HelloJob class
                    IJobDetail job = JobBuilder.Create<AutoDownLoad>()
                         .WithDescription("Auto Download Attendance Log from All Machine")
                        .WithIdentity(jobid, "Job_AutoDownload")
                        .Build();

                    // Trigger the job to run now, and then repeat every 10 seconds
                    ITrigger trigger = TriggerBuilder.Create()
                        .WithIdentity(triggerid, "TRG_AutoDownload")
                        .StartNow()
                        .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(tTime.Hours, tTime.Minutes).WithMisfireHandlingInstructionFireAndProceed())
                        .Build();

                    // Tell quartz to schedule the job using our trigger
                    scheduler.ScheduleJob(job, trigger);
                    ServerMsg tMsg = new ServerMsg();
                    tMsg.MsgType = "Job Building";
                    tMsg.MsgTime = DateTime.Now;
                    tMsg.Message = string.Format("Building Job Job ID : {0} And Trigger ID : {1}", jobid, triggerid);
                    Publish(tMsg);
                    
                }
            }
        }

        public void RegSchedule_AutoTimeSet()
        {
            bool hasrow = Globals.G_DsAutoTime.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);
            if (hasrow)
            {
                foreach (DataRow dr in Globals.G_DsAutoTime.Tables[0].Rows)
                {
                    TimeSpan tTime = (TimeSpan)dr["SchTime"];
                    string jobid = "Job_TimeSet_" + tTime.Hours.ToString() + tTime.Minutes.ToString();
                    string triggerid = "Trigger_TimeSet_" + tTime.Hours.ToString() + tTime.Minutes.ToString();
                    // define the job and tie it to our HelloJob class
                    IJobDetail job = JobBuilder.Create<AutoTimeSet>()
                         .WithDescription("Auto Set ServerTime to All Machine")
                        .WithIdentity(jobid, "Job_AutoTimeSet")
                        .Build();

                    // Trigger the job to run now, and then repeat every 10 seconds
                    ITrigger trigger = TriggerBuilder.Create()
                        .WithIdentity(triggerid, "TRG_AutoTimeSet")                        
                        .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(tTime.Hours, tTime.Minutes))
                        .StartNow()
                        .Build();

                    // Tell quartz to schedule the job using our trigger
                    scheduler.ScheduleJob(job, trigger);
                    ServerMsg tMsg = new ServerMsg();
                    tMsg.MsgType = "Job Building";
                    tMsg.MsgTime = DateTime.Now;
                    tMsg.Message = string.Format("Building Job Job ID : {0} And Trigger ID : {1}", jobid, triggerid);
                    Scheduler.Publish(tMsg);
                    
                }
            }
        }

        public void RegSchedule_AutoProcess()
        {
            if (Globals.G_AutoProcess)
            {
                TimeSpan tTime = Globals.G_AutoProcessTime;
                if (tTime.Hours == 0 && tTime.Minutes == 0)
                {
                    ServerMsg tMsg = new ServerMsg();
                    tMsg.MsgType = "Job Building";
                    tMsg.MsgTime = DateTime.Now;
                    tMsg.Message = string.Format("Auto Process : did not get time");
                    Publish(tMsg);
                    return;
                }

                string[] tWrkGrp = Globals.G_AutoProcessWrkGrp.Split(',');
                if (tWrkGrp.Count() <= 0)
                {
                    ServerMsg tMsg = new ServerMsg();
                    tMsg.MsgType = "Job Building";
                    tMsg.MsgTime = DateTime.Now;
                    tMsg.Message = string.Format("Auto Process : did not get wrkgrps");
                    Publish(tMsg);
                    return;
                }
                int t1 = -5;
                foreach (string wrk in tWrkGrp)
                {
                    t1 += 1;
                    string jobid = "Job_AutoProcess_" + wrk.Replace("'", "");
                    string triggerid = "Trigger_AutoProcess_" + wrk.Replace("'", "");

                    // define the job and tie it to our HelloJob class
                    IJobDetail job = JobBuilder.Create<AutoProcess>()
                        .WithDescription("Auto Process Attendance Data")
                        .WithIdentity(jobid, "Job_AutoProcess")
                        .UsingJobData("WrkGrp", wrk.Replace("'", ""))                        
                        .Build();
                    
                    // Trigger the job to run now, and then repeat every 10 seconds
                    ITrigger trigger = TriggerBuilder.Create()
                        .WithIdentity(triggerid, "TRG_AutoProcess")
                        .StartNow()
                        .WithSchedule(
                            CronScheduleBuilder.DailyAtHourAndMinute(tTime.Hours, tTime.Minutes + t1)
                            .WithMisfireHandlingInstructionFireAndProceed()
                            )
                        .Build();

                    // Tell quartz to schedule the job using our trigger
                    scheduler.ScheduleJob(job, trigger);

                    ServerMsg tMsg = new ServerMsg();
                    tMsg.MsgType = "Job Building";
                    tMsg.MsgTime = DateTime.Now;
                    tMsg.Message = string.Format("Building Job Job ID : {0} And Trigger ID : {1}", jobid, triggerid);
                    Publish(tMsg);

                }
            }
        }

        public void RegSchedule_GatePassPunchProcess()
        {

            string jobid = "GatePassPunch_Process";
            string triggerid = "Trigger_GatePassPunch_Process";

            // define the job and tie it to our HelloJob class
            IJobDetail job = JobBuilder.Create<AutoGatePassPunch_Process>()
                    .WithDescription("GatePass Punch Segrigate based on ESS GatePass")
                .WithIdentity(jobid, "GatePassPunch_Process")
                .Build();

            // Trigger the job to run every 5 minute
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity(triggerid, "TRG_GatePassPunch_Process")
                .StartNow()
                 .WithSchedule(CronScheduleBuilder.CronSchedule("0 0/5 * * * ?").WithMisfireHandlingInstructionFireAndProceed())
                .Build();

            // Tell quartz to schedule the job using our trigger
            scheduler.ScheduleJob(job, trigger);

            ServerMsg tMsg = new ServerMsg();
            tMsg.MsgType = "Job Building";
            tMsg.MsgTime = DateTime.Now;
            tMsg.Message = string.Format("Building Job Job ID : {0} And Trigger ID : {1}", jobid, triggerid);
            Scheduler.Publish(tMsg);

            
        }

        public void RegSchedule_BlockUnBlockProcess()
        {
            string jobid = "BlockUnBlockProcess";
            string triggerid = "Trigger_BlockUnBlock";

            // define the job and tie it to our HelloJob class
            IJobDetail job = JobBuilder.Create<BlockUnBlockOperation>()
                    .WithDescription("Blocking/UnBlocking of Employee")
                .WithIdentity(jobid, "BlockUnBlockProcess")
                .Build();

            // Trigger the job to run every 3 minute
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity(triggerid, "TRG_BlockUnBlock")
                .StartNow()
                .WithSchedule(CronScheduleBuilder.CronSchedule("0 0/2 * * * ?").WithMisfireHandlingInstructionFireAndProceed())
                .Build();

            // Tell quartz to schedule the job using our trigger
            scheduler.ScheduleJob(job, trigger);

            ServerMsg tMsg = new ServerMsg();
            tMsg.MsgType = "Job Building";
            tMsg.MsgTime = DateTime.Now;
            tMsg.Message = string.Format("Building Job Job ID : {0} And Trigger ID : {1}", jobid, triggerid);
            Scheduler.Publish(tMsg);

           
        }


        public void RegSchedule_WorkerProcess()
        {
            string jobid = "WorkerProcess";
            string triggerid = "Trigger_WorkerProcess";

            // define the job and tie it to our HelloJob class
            IJobDetail job = JobBuilder.Create<WorkerProcess>()
                    .WithDescription("Heartbeat And Pending backlog Data Process")
                .WithIdentity(jobid, "WorkerProcess")
                .Build();

            // Trigger the job to run every 3 minute
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity(triggerid, "TRG_WorkerProcess")
                .StartNow()
                .WithSchedule(CronScheduleBuilder.CronSchedule("0 0/2 * * * ?").WithMisfireHandlingInstructionFireAndProceed())                
                .Build();

            //.WithMisfireHandlingInstructionFireAndProceed()

            // Tell quartz to schedule the job using our trigger
            scheduler.ScheduleJob(job, trigger);
            
            ServerMsg tMsg = new ServerMsg();
            tMsg.MsgType = "Job Building";
            tMsg.MsgTime = DateTime.Now;
            tMsg.Message = string.Format("Building Job Job ID : {0} And Trigger ID : {1}", jobid, triggerid);
            Scheduler.Publish(tMsg);
            
            if (Globals.G_AutoDelEmp)
            {
                
                string jobid2 = "Job_AutoDeleteLeftEmp";
                string triggerid2 = "Trigger_AutoDeleteLeftEmp";

                // define the job and tie it to our HelloJob class
                IJobDetail job2 = JobBuilder.Create<AutoDeleteLeftEmp>()
                     .WithDescription("Auto Delete Left Employee")
                    .WithIdentity(jobid2, "Job_DEL_LeftEmp")
                    .Build();

                DayOfWeek[] onSunday = new DayOfWeek[] { DayOfWeek.Sunday};

                // Trigger the job to run 
                ITrigger trigger2 = TriggerBuilder.Create()
                    .WithIdentity(triggerid2, "TRG_DEL_LeftEmp")
                    .StartNow()
                    .WithSchedule(CronScheduleBuilder.AtHourAndMinuteOnGivenDaysOfWeek(Globals.G_AutoDelEmpTime.Hours, Globals.G_AutoDelEmpTime.Minutes, onSunday).WithMisfireHandlingInstructionFireAndProceed())
                    .Build();

                // Tell quartz to schedule the job using our trigger
                scheduler.ScheduleJob(job2, trigger2);

                tMsg = new ServerMsg();
                tMsg.MsgType = "Job Building";
                tMsg.MsgTime = DateTime.Now;
                tMsg.Message = string.Format("Building Job Job ID : {0} And Trigger ID : {1}", jobid2, triggerid2);
                Scheduler.Publish(tMsg);
            }

            #region AutoDelExpEmp
            if (Globals.G_AutoDelExpEmp)
            {
                string jobid3 = "Job_AutoDeleteExpireValidityEmp";
                string triggerid3 = "Trigger_AutoDeleteExpireValidityEmp";

                // define the job and tie it to our HelloJob class
                IJobDetail job3 = JobBuilder.Create<AutoDeleteExpireValidityEmp>()
                     .WithDescription("Auto Delete Expired Validity Employee")
                    .WithIdentity(jobid3, "Job_DELExpEmp")
                    .Build();

                // Trigger the job to run 
                ITrigger trigger3 = TriggerBuilder.Create()
                    .WithIdentity(triggerid3, "TRG_DELExpEmp")
                    .StartNow()
                    .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(Globals.G_AutoDelExpEmpTime.Hours, Globals.G_AutoDelExpEmpTime.Minutes))
                    .Build();

                // Tell quartz to schedule the job using our trigger
                scheduler.ScheduleJob(job3, trigger3);

                tMsg = new ServerMsg();
                tMsg.MsgType = "Job Building";
                tMsg.MsgTime = DateTime.Now;
                tMsg.Message = string.Format("Building Job Job ID : {0} And Trigger ID : {1}", jobid3, triggerid3);
                Scheduler.Publish(tMsg);
            }
            #endregion
        }
        
        public void RegSchedule_AutoArrival()
        {
            
            
            bool hasrow = Globals.G_DsAutoArrival.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);
            if (hasrow)
            {
                foreach (DataRow dr in Globals.G_DsAutoArrival.Tables[0].Rows)
                {
                    TimeSpan tTime = (TimeSpan)dr["SchTime"];
                    TimeSpan FromTime = (TimeSpan)dr["FromTime"];
                    TimeSpan ToTime = (TimeSpan)dr["ToTime"];
                     
                    
                    string jobid4 = "Job_Arrival_" + tTime.Hours.ToString() + tTime.Minutes.ToString();
                    string triggerid4 = "Trigger_Arrival_" + tTime.Hours.ToString() + tTime.Minutes.ToString();
                    // define the job and tie it to our HelloJob class
                    IJobDetail job4 = JobBuilder.Create<AutoArrival>()
                        .WithIdentity(jobid4, "Job_Arrival")
                        .WithDescription("Auto Process Shift wise Arrival Report For " + FromTime.ToString() + " TO " + ToTime.ToString())
                        .UsingJobData("FromTime", FromTime.ToString())
                        .UsingJobData("ToTime", ToTime.ToString())    
                        .Build();

                    // Trigger the job to run now
                    ITrigger trigger4 = TriggerBuilder.Create()
                        .WithIdentity(triggerid4, "TRG_Arrival")
                        .StartNow()
                        .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(tTime.Hours, tTime.Minutes).WithMisfireHandlingInstructionFireAndProceed())                                          
                        .Build();

                    // Tell quartz to schedule the job using our trigger
                    scheduler.ScheduleJob(job4, trigger4);
                    ServerMsg tMsg = new ServerMsg();
                    tMsg.MsgType = "Job Building";
                    tMsg.MsgTime = DateTime.Now;
                    tMsg.Message = string.Format("Building Job Job ID : {0} And Trigger ID : {1}", jobid4, triggerid4);
                    Scheduler.Publish(tMsg);
                    
                }
            }
        }

        public class AutoDeleteLeftEmp : IJob
        {
            public void Execute(IJobExecutionContext context)
            {
                if (_ShutDown)
                {
                    return;
                }

                if (_StatusAutoArrival == false &&
                   _StatusAutoDownload == false &&
                   _StatusAutoProcess == false &&
                   _StatusAutoTimeSet == false &&
                   _StatusWorker == false)
                {


                    bool hasrow = Globals.G_DsMachine.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);

                    if (hasrow)
                    {
                       

                        string filenm = "AutoDeleteEmp_" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
                        foreach (DataRow dr in Globals.G_DsMachine.Tables[0].Rows)
                        {

                            if (_ShutDown)
                            {
                                _StatusWorker = false;
                                return;
                            }

                            _StatusWorker = true;

                            string ip = dr["MachineIP"].ToString();

                            try
                            {
                                ServerMsg tMsg = new ServerMsg();
                                tMsg.MsgTime = DateTime.Now;
                                tMsg.MsgType = "Auto Delete Left Employee";
                                tMsg.Message = ip;
                                Scheduler.Publish(tMsg);

                                string ioflg = dr["IOFLG"].ToString();
                                string err = string.Empty;

                                clsMachine m = new clsMachine(ip, ioflg);
                                m.Connect(out err);
                                if (!string.IsNullOrEmpty(err))
                                {

                                    string fullpath = Path.Combine(Errfilepath, filenm);
                                    //write primary errors
                                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(fullpath, true))
                                    {
                                        file.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "-AutoDelete-[" + ip + "]-" + err);
                                    }

                                    tMsg.MsgTime = DateTime.Now;
                                    tMsg.MsgType = "Auto Delete Left Employee";
                                    tMsg.Message = ip;
                                    Scheduler.Publish(tMsg);
                                    continue;
                                }
                                err = string.Empty;

                                m.DeleteLeftEmp_NEW(out err);

                                if (!string.IsNullOrEmpty(err))
                                {
                                    string fullpath = Path.Combine(Errfilepath, filenm);
                                    //write errlog
                                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(fullpath, true))
                                    {
                                        file.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "-Auto Delete Left Employee-[" + ip + "]-" + err);
                                    }

                                }

                                string fullpath2 = Path.Combine(Loginfopath, filenm);
                                using (System.IO.StreamWriter file = new System.IO.StreamWriter(fullpath2, true))
                                {
                                    file.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "-Auto Delete Left Employee-[" + ip + "]-Completed");
                                }
                                m.RefreshData();
                                m.DisConnect(out err);
                            }
                            catch (Exception ex)
                            {
                                string fullpath = Path.Combine(Errfilepath, filenm);
                                using (System.IO.StreamWriter file = new System.IO.StreamWriter(fullpath, true))
                                {
                                    file.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "-Auto Delete Left Employee-[" + ip + "]-" + ex.ToString());
                                }
                            }
                        }

                        _StatusWorker = false;
                    }
                }
            }
        }
                
        public class AutoDownLoad : IJob
        {
            public void Execute(IJobExecutionContext context)
            {
                if (_ShutDown)
                {
                    return;
                }
                
                bool hasrow = Globals.G_DsMachine.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);
                if (hasrow)
                {
                    
                    
                    _StatusAutoDownload = true;


                    foreach (DataRow dr in Globals.G_DsMachine.Tables[0].Rows)
                    {
                        string ip = dr["MachineIP"].ToString();

                        try
                        {


                            ServerMsg tMsg = new ServerMsg();
                            tMsg.MsgTime = DateTime.Now;
                            tMsg.MsgType = "Auto Download";
                            tMsg.Message = ip;
                            Scheduler.Publish(tMsg);

                            string ioflg = dr["IOFLG"].ToString();
                            string err = string.Empty;
                            string filenm = "AutoErrLog_" + DateTime.Now.ToString("yyyyMMdd_HHmm") + ".txt";
                            string fullpath = Path.Combine(Errfilepath, filenm);

                            clsMachine m = new clsMachine(ip, ioflg);
                            m.Connect(out err);
                            if (!string.IsNullOrEmpty(err))
                            {
                                //write primary errors
                                using (System.IO.StreamWriter file = new System.IO.StreamWriter(fullpath, true))
                                {
                                    file.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "-AutoDownload-[" + ip + "]-" + err);
                                }

                                tMsg.MsgTime = DateTime.Now;
                                tMsg.MsgType = "Auto Download";
                                tMsg.Message = ip;
                                Scheduler.Publish(tMsg);
                                continue;
                            }
                            err = string.Empty;

                            List<AttdLog> tempattd = new List<AttdLog>();
                            m.GetAttdRec(out tempattd, out err);
                            if (!string.IsNullOrEmpty(err))
                            {
                                //write errlog
                                using (System.IO.StreamWriter file = new System.IO.StreamWriter(fullpath, true))
                                {
                                    file.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "-AutoDownload-[" + ip + "]-" + err);
                                }

                                tMsg.MsgTime = DateTime.Now;
                                tMsg.MsgType = "Auto Download";
                                tMsg.Message = ip + "->Error :" + err;
                                Scheduler.Publish(tMsg);
                                continue;
                            }



                            filenm = "AutoDownload_Log_" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
                            fullpath = Path.Combine(Loginfopath, filenm);
                            using (System.IO.StreamWriter file = new System.IO.StreamWriter(fullpath, true))
                            {
                                file.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "-AutoDownload-[" + ip + "]-Completed");
                            }

                            m.DisConnect(out err);
                        }
                        catch (Exception ex)
                        {
                            string filenm = "AutoErrLog_" + DateTime.Now.ToString("yyyyMMdd_HHmm") + ".txt";
                            string fullpath = Path.Combine(Errfilepath, filenm);
                            using (System.IO.StreamWriter file = new System.IO.StreamWriter(fullpath, true))
                            {
                                file.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "-AutoDownload-[" + ip + "]-" + ex.ToString());
                            }
                        }
                    }
                    
                    _StatusAutoDownload = false;
                }
            }
        }

        public class AutoGatePassPunch_Process : IJob
        {
            public void Execute(IJobExecutionContext context)
            {

                ServerMsg tMsg = new ServerMsg();
                tMsg.MsgTime = DateTime.Now;
                tMsg.MsgType = "GatePass Punch Process";
                tMsg.Message = "Started" + ":" + _ShutDown.ToString() ;
                Scheduler.Publish(tMsg);
                
                if (_ShutDown)
                {
                    return;
                }

                string err;
                string essConStr = "Server=172.16.12.14;Database=ESS;User Id=sa;Password=testomonials@123;";

                string sql = "Select Top 20 ID,EmpUnqID,AddDateTime,GateOutDateTime,GateInDateTime,GatePassStatus,Mode,GatePassDate " +
                    " From GatePasses " +
                    " Where " +
                    " GateOutUser is null " +
                    " Order By ID  " ;

                DataSet dsGPass = Utils.Helper.GetData(sql,essConStr,out err);
                if (!string.IsNullOrEmpty(err))
                {
                    tMsg = new ServerMsg();
                    tMsg.MsgTime = DateTime.Now;
                    tMsg.MsgType = "GatePass Punch Process";
                    tMsg.Message = err;
                    Scheduler.Publish(tMsg);
                    return;
                }

                bool hasrow = dsGPass.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);
                
                
                if (hasrow)
                {
                    tMsg = new ServerMsg();
                    tMsg.MsgTime = DateTime.Now;
                    tMsg.MsgType = "GatePass Punch Process";
                    tMsg.Message = dsGPass.Tables[0].Rows.Count.ToString();
                    Scheduler.Publish(tMsg);

                    foreach (DataRow dr in dsGPass.Tables[0].Rows)
                    {
                        if (_ShutDown)
                        {
                            return;
                        }                       

                        string gpstatus = dr["GatePassStatus"].ToString();
                        string id = dr["ID"].ToString();
                        DateTime gpdate = Convert.ToDateTime(dr["AddDateTime"]).AddMinutes(-10);
                        string EmpUnqID = dr["EmpUnqID"].ToString();

                        if (gpstatus == "F")
                        {
                            using (SqlConnection esscn = new SqlConnection(essConStr))
                            {
                                try
                                {
                                    esscn.Open();
                                    using (SqlCommand cmd = new SqlCommand())
                                    {
                                        cmd.Connection = esscn;
                                        cmd.CommandText = "Update GatePasses Set GateOutUser = 1 where ID ='" + id + "'";
                                        cmd.ExecuteNonQuery();

                                        tMsg = new ServerMsg();
                                        tMsg.MsgTime = DateTime.Now;
                                        tMsg.MsgType = "GatePass Punch Process";
                                        tMsg.Message = "Forced Out : " + id;
                                        Scheduler.Publish(tMsg);
                                    }
                                }
                                catch(Exception ex)
                                {
                                    tMsg = new ServerMsg();
                                    tMsg.MsgTime = DateTime.Now;
                                    tMsg.MsgType = "GatePass Punch Process";
                                    tMsg.Message = id + " : " + ex.Message;
                                    Scheduler.Publish(tMsg);

                                }
                                
                            }

                            //skip further process
                            continue;
                        }

                        if (gpstatus == "N")
                        {
                            continue;
                        }

                        bool outprocess = false;
                        bool inprocess = false;


                        
                        if (dr["GateOutDateTime"] != DBNull.Value)
                        {

                            DateTime outtime = Convert.ToDateTime(dr["GateOutDateTime"]).AddMinutes(30);

                            //find the nearby punch from tripod log if found, Addinto GatePassInOut and Remove from lunchinout also TripodLog 

                            string tsql = "Select top 1 * From TripodLog where EmpUnqID ='" + EmpUnqID + "' " +
                                " And PunchDate BETWEEN '" + gpdate.ToString("yyyy-MM-dd HH:mm:ss") + "' And '" + outtime.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                                " And IOFLG = 'O' and tYear = '" + gpdate.ToString("yyyy") + "' " +
                                " And tYearMt = '" + gpdate.ToString("yyyyMM") + "' Order by PunchDate ";

                            DataSet ds = Utils.Helper.GetData(tsql, Utils.Helper.constr);
                            hasrow = ds.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);


                            tMsg = new ServerMsg();
                            tMsg.MsgTime = DateTime.Now;
                            tMsg.MsgType = "GatePass Punch Process";
                            tMsg.Message = id + ":" + dr["EmpUnqID"].ToString();
                            Scheduler.Publish(tMsg);


                            if (hasrow)
                            {
                                DataRow row = ds.Tables[0].Rows[0];
                                string PunchDate = Convert.ToDateTime(row["PunchDate"]).ToString("yyyy-MM-dd HH:mm:ss");
                                string IOFLG = row["IOFLG"].ToString();
                                string t1Date = Convert.ToDateTime(row["t1Date"]).ToString("yyyy-MM-dd");
                                string tYearmt = row["tYearMt"].ToString();
                                string tYear = row["tYear"].ToString();
                                string MachineIP = row["MachineIP"].ToString();

                                #region convert_out_punch
                                using (SqlConnection cn = new SqlConnection(Utils.Helper.constr))
                                {
                                    try
                                    {
                                        cn.Open();
                                    }
                                    catch (Exception ex)
                                    {
                                        tMsg = new ServerMsg();
                                        tMsg.MsgTime = DateTime.Now;
                                        tMsg.MsgType = "GatePass Punch Process";
                                        tMsg.Message = id + " : " + ex.Message;
                                        Scheduler.Publish(tMsg);
                                        
                                        
                                        continue;
                                    }

                                    SqlTransaction tr = cn.BeginTransaction();
                                    try
                                    {
                                        DateTime tDate = Convert.ToDateTime(PunchDate);

                                        sql = "Delete From AttdLunchGate Where EmpUnqID ='" + EmpUnqID + "' and PunchDate between '" + PunchDate + "' And '" + Convert.ToDateTime(row["PunchDate"]).AddMinutes(3).ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                                            " And MachineIP ='" + MachineIP + "' And IOFLG = '" + IOFLG + "' " +
                                            " And tYear ='" + tYear + "' And tYearMt = '" + tYearmt + "' and t1date = '" + t1Date + "'";
                                        SqlCommand cmd = new SqlCommand(sql, cn);
                                        cmd.Transaction = tr;
                                        int t = (int)cmd.ExecuteNonQuery();

                                        if (t > 0)
                                        {
                                            sql = "Insert into AttdLunchIODelete (EmpUnqID,PunchDate,IOFLG,MachineIP,AddDt,AddId,tYear,tYearMt,t1date) Values (" +
                                            " '" + EmpUnqID + "','" + PunchDate + "'," +
                                            " '" + IOFLG + "','" + MachineIP + "',GetDate(),'Conv-Server','" + tYear + "','" + tYearmt + "','" + t1Date + "' )";

                                            cmd = new SqlCommand(sql, cn);
                                            cmd.Transaction = tr;
                                            cmd.ExecuteNonQuery();
                                        }


                                        sql = "Insert into AttdGatePassInOut (EmpUnqID,PunchDate,IOFLG,MachineIP,AddDt,AddId,tYear,tYearMt,t1date) Values (" +
                                        " '" + EmpUnqID + "','" + PunchDate + "'," +
                                        " '" + IOFLG + "','" + MachineIP + "',GetDate(),'TRIPOD-Conv-Server','" + tYear + "','" + tYearmt + "','" + t1Date + "' )";
                                        cmd = new SqlCommand(sql, cn);
                                        cmd.Transaction = tr;
                                        cmd.ExecuteNonQuery();

                                        sql = "Delete From TripodLog Where EmpUnqID ='" + EmpUnqID + "' and PunchDate between '" + PunchDate + "' And '" + Convert.ToDateTime(row["PunchDate"]).AddMinutes(3).ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                                            " And MachineIP ='" + MachineIP + "' And IOFLG = '" + IOFLG + "' and tYear ='" + tYear + "' and tYearMt = '" + tYearmt + "'";

                                        cmd = new SqlCommand(sql, cn);
                                        cmd.Transaction = tr;
                                        cmd.ExecuteNonQuery();

                                        tr.Commit();

                                        outprocess = true;
                                        outtime = Convert.ToDateTime(row["PunchDate"]);

                                        
                                    }
                                    catch (Exception ex)
                                    {
                                        tr.Rollback();

                                        tMsg = new ServerMsg();
                                        tMsg.MsgTime = DateTime.Now;
                                        tMsg.MsgType = "GatePass Punch Process";
                                        tMsg.Message = id + " : " + ex.Message;
                                        Scheduler.Publish(tMsg);

                                        continue;
                                    }
                                }//using localcn 




                                #endregion
                            }//if tripod out punch found...
                            else
                            {
                                //check if already converted by manually
                                tsql = "Select top 1 * From AttdGatePassInOut where EmpUnqID ='" + EmpUnqID + "' " +
                                " And PunchDate BETWEEN '" + gpdate.ToString("yyyy-MM-dd HH:mm:ss") + "' And '" + outtime.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                                " And IOFLG = 'O' and tYear = '" + gpdate.ToString("yyyy") + "' " +
                                " And tYearMt = '" + gpdate.ToString("yyyyMM") + "' and AddId <> 'TRIPOD-Conv-Server' Order by PunchDate  ";

                                DataSet tds = Utils.Helper.GetData(tsql, Utils.Helper.constr);
                                hasrow = tds.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);
                                
                                bool manualin = false;
                                bool manualout = false;

                                DateTime manualouttime, manualintime ;
                                
                                if (hasrow)
                                {
                                    manualout = true;
                                    manualouttime = Convert.ToDateTime(tds.Tables[0].Rows[0]["PunchDate"]);
                                    
                                    //check if already converted by manually
                                    tsql = "Select top 1 * From AttdGatePassInOut where EmpUnqID ='" + EmpUnqID + "' " +
                                    " And PunchDate >= '" + manualouttime.ToString("yyyy-MM-dd HH:mm:ss") + "'  " +
                                    " And IOFLG = 'I' and tYear = '" + gpdate.ToString("yyyy") + "' " +
                                    " And tYearMt = '" + gpdate.ToString("yyyyMM") + "' and AddId <> 'TRIPOD-Conv-Server'  Order by PunchDate  ";

                                    tds = Utils.Helper.GetData(tsql, Utils.Helper.constr);
                                    hasrow = tds.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);
                                    if (hasrow)
                                    {
                                        manualin = true;
                                        manualintime = Convert.ToDateTime(tds.Tables[0].Rows[0]["PunchDate"]);
                                    }

                                    if (manualin && manualout)
                                    {

                                        using (SqlConnection esscn = new SqlConnection(essConStr))
                                        {
                                            try
                                            {
                                                esscn.Open();
                                                using (SqlCommand cmd = new SqlCommand())
                                                {
                                                    cmd.Connection = esscn;
                                                    cmd.CommandText = "Update GatePasses Set GateOutUser = 1 where ID ='" + id + "'";
                                                    cmd.ExecuteNonQuery();

                                                    tMsg = new ServerMsg();
                                                    tMsg.MsgTime = DateTime.Now;
                                                    tMsg.MsgType = "GatePass Punch Process";
                                                    tMsg.Message = "Manual Out/In : " + id;
                                                    Scheduler.Publish(tMsg);
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                tMsg = new ServerMsg();
                                                tMsg.MsgTime = DateTime.Now;
                                                tMsg.MsgType = "GatePass Punch Process";
                                                tMsg.Message = id + " : " + ex.Message;
                                                Scheduler.Publish(tMsg);

                                            }

                                        }
                                       
                                    }
                                }
                            }

                            //if mode == "D"->"Duty Off"
                            if (dr["Mode"].ToString() == "D")
                            {
                                using (SqlConnection esscn = new SqlConnection(essConStr))
                                {
                                    try
                                    {
                                        esscn.Open();
                                        using (SqlCommand cmd = new SqlCommand())
                                        {
                                            cmd.Connection = esscn;
                                            cmd.CommandText = "Update GatePasses Set GateOutUser = 1 where ID ='" + id + "'";
                                            cmd.ExecuteNonQuery();
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        tMsg = new ServerMsg();
                                        tMsg.MsgTime = DateTime.Now;
                                        tMsg.MsgType = "GatePass Punch Process";
                                        tMsg.Message = id.ToString() + ":" + ex.Message;
                                        Scheduler.Publish(tMsg);
                                    }

                                }//using essconnection

                                continue;
                            }
                            


                            #region essgp_inpunch
                            if (dr["GateINDateTime"] != DBNull.Value)
                            {

                                //find the nearby punch from tripod log if found, Addinto GatePassInOut and Remove from lunchinout also TripodLog 

                                DateTime InTime = Convert.ToDateTime(dr["GateInDateTime"]).AddMinutes(3);

                                tsql = "Select top 1 * From TripodLog where EmpUnqID ='" + EmpUnqID + "' " +
                                " And PunchDate BETWEEN '" + outtime.ToString("yyyy-MM-dd HH:mm:ss") + "' And '" + InTime.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                                " And IOFLG = 'I' and tYear = '" + gpdate.ToString("yyyy") + "' " +
                                " And tYearMt = '" + gpdate.ToString("yyyyMM") + "' Order by PunchDate ";

                                ds = Utils.Helper.GetData(tsql, Utils.Helper.constr);
                                hasrow = ds.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);
                                if (hasrow)
                                {
                                    DataRow row = ds.Tables[0].Rows[0];
                                    string PunchDate = Convert.ToDateTime(row["PunchDate"]).ToString("yyyy-MM-dd HH:mm:ss");
                                    string IOFLG = row["IOFLG"].ToString();
                                    string t1Date = Convert.ToDateTime(row["t1Date"]).ToString("yyyy-MM-dd");
                                    string tYearmt = row["tYearMt"].ToString();
                                    string tYear = row["tYear"].ToString();
                                    string MachineIP = row["MachineIP"].ToString();

                                    #region convert_inpunch
                                    using (SqlConnection cn = new SqlConnection(Utils.Helper.constr))
                                    {
                                        try
                                        {
                                            cn.Open();
                                        }
                                        catch (Exception ex)
                                        {
                                            tMsg = new ServerMsg();
                                            tMsg.MsgTime = DateTime.Now;
                                            tMsg.MsgType = "GatePass Punch Process";
                                            tMsg.Message = id.ToString() + ":" + ex.Message;
                                            Scheduler.Publish(tMsg);
                                            
                                            continue;
                                        }

                                        SqlTransaction tr = cn.BeginTransaction();
                                        try
                                        {
                                            DateTime tDate = Convert.ToDateTime(PunchDate);

                                            sql = "Delete From AttdLunchGate Where EmpUnqID ='" + EmpUnqID + "' and PunchDate between '" + PunchDate + "' And '" + Convert.ToDateTime(row["PunchDate"]).AddMinutes(3).ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                                                " And MachineIP ='" + MachineIP + "' And IOFLG = '" + IOFLG + "' " +
                                                " And tYear ='" + tYear + "' And tYearMt = '" + tYearmt + "' and t1date = '" + t1Date + "'";
                                            SqlCommand cmd = new SqlCommand(sql, cn);
                                            cmd.Transaction = tr;
                                            int t = (int)cmd.ExecuteNonQuery();

                                            if (t > 0)
                                            {
                                                sql = "Insert into AttdLunchIODelete (EmpUnqID,PunchDate,IOFLG,MachineIP,AddDt,AddId,tYear,tYearMt,t1date) Values (" +
                                                " '" + EmpUnqID + "','" + PunchDate + "'," +
                                                " '" + IOFLG + "','" + MachineIP + "',GetDate(),'Conv-Server','" + tYear + "','" + tYearmt + "','" + t1Date + "' )";

                                                cmd = new SqlCommand(sql, cn);
                                                cmd.Transaction = tr;
                                                cmd.ExecuteNonQuery();
                                            }


                                            sql = "Insert into AttdGatePassInOut (EmpUnqID,PunchDate,IOFLG,MachineIP,AddDt,AddId,tYear,tYearMt,t1date) Values (" +
                                            " '" + EmpUnqID + "','" + PunchDate + "'," +
                                            " '" + IOFLG + "','" + MachineIP + "',GetDate(),'TRIPOD-Conv-Server','" + tYear + "','" + tYearmt + "','" + t1Date + "' )";
                                            cmd = new SqlCommand(sql, cn);
                                            cmd.Transaction = tr;
                                            cmd.ExecuteNonQuery();

                                            sql = "Delete From TripodLog Where EmpUnqID ='" + EmpUnqID + "' and PunchDate between '" + PunchDate + "' And '" + Convert.ToDateTime(row["PunchDate"]).AddMinutes(3).ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                                                " And MachineIP ='" + MachineIP + "' And IOFLG = '" + IOFLG + "' and tYear ='" + tYear + "' and tYearMt = '" + tYearmt + "'";

                                            cmd = new SqlCommand(sql, cn);
                                            cmd.Transaction = tr;
                                            cmd.ExecuteNonQuery();

                                            tr.Commit();

                                            inprocess = true;
                                        }
                                        catch (Exception ex)
                                        {
                                            tr.Rollback();
                                            tMsg = new ServerMsg();
                                            tMsg.MsgTime = DateTime.Now;
                                            tMsg.MsgType = "GatePass Punch Process";
                                            tMsg.Message = id.ToString() + ":" + ex.Message; 
                                            Scheduler.Publish(tMsg);
                                            continue;
                                        }
                                    }//using localcn

                                    #endregion

                                }//inpunch found in tripod

                            }//if gatepassinpunch found in ess..
                            else
                            {
                                if (dr["GateINDateTime"] == DBNull.Value && dr["GateOutDateTime"] != DBNull.Value)
                                {
                                    DateTime gout = Convert.ToDateTime(dr["GateOutDateTime"]);
                                    TimeSpan diff = DateTime.Now - gout;
                                    double hours = diff.TotalHours;
                                    if (hours > 24)
                                    {
                                        using (SqlConnection esscn = new SqlConnection(essConStr))
                                        {
                                            try
                                            {
                                                esscn.Open();
                                                using (SqlCommand cmd = new SqlCommand())
                                                {
                                                    cmd.Connection = esscn;
                                                    cmd.CommandText = "Update GatePasses Set GateOutUser = 1 where ID ='" + id + "'";
                                                    cmd.ExecuteNonQuery();
                                                }

                                                
                                            }
                                            catch (Exception ex)
                                            {
                                                tMsg = new ServerMsg();
                                                tMsg.MsgTime = DateTime.Now;
                                                tMsg.MsgType = "GatePass Punch Process";
                                                tMsg.Message = id.ToString() + ":" + ex.Message;
                                                Scheduler.Publish(tMsg);
                                            }

                                            continue;

                                        }//using essconnection
                                    }

                                }
                            }
                            #endregion


                            if (outprocess || inprocess)
                            {
                                //process attendance
                                DateTime tempdt = new DateTime(), sFromDt = new DateTime(), sToDate = new DateTime();
                                tempdt = Convert.ToDateTime(outtime.ToString("yyyy-MM-dd"));
                                sFromDt = tempdt.AddDays(-1);
                                sToDate = tempdt.AddDays(1);

                                if (sFromDt != DateTime.MinValue && sToDate != DateTime.MinValue)
                                {
                                    clsProcess pro = new clsProcess();
                                    int res = 0; string errpro = string.Empty;
                                    pro.LunchInOutProcess(EmpUnqID, sFromDt, sToDate, out res);
                                    pro.Lunch_HalfDayPost_Process(EmpUnqID, tempdt, out res);
                                    pro.AttdProcess(EmpUnqID, sFromDt, sToDate, out res, out errpro);
                                }
                            }

                        }

                        #region update_essgp

                        //mark the process
                        if (outprocess && inprocess)
                        {
                            using (SqlConnection esscn = new SqlConnection(essConStr))
                            {
                                try
                                {
                                    esscn.Open();
                                    using (SqlCommand cmd = new SqlCommand())
                                    {
                                        cmd.Connection = esscn;
                                        cmd.CommandText = "Update GatePasses Set GateOutUser = 1 where ID ='" + id + "'";
                                        cmd.ExecuteNonQuery();
                                    }
                                }
                                catch(Exception ex)
                                {
                                    tMsg = new ServerMsg();
                                    tMsg.MsgTime = DateTime.Now;
                                    tMsg.MsgType = "GatePass Punch Process";
                                    tMsg.Message = id.ToString() + ":" + ex.Message ;
                                    Scheduler.Publish(tMsg);
                                }

                            }//using essconnection
                        }
                        #endregion
                        
                    }//foreachloop

                    tMsg = new ServerMsg();
                    tMsg.MsgTime = DateTime.Now;
                    tMsg.MsgType = "GatePass Punch Process";
                    tMsg.Message = "Finished";
                    Scheduler.Publish(tMsg);

                }

            }

        }

        public class AutoDownLoadTask : IJob
        {
            public void Execute(IJobExecutionContext context)
            {
                if (_ShutDown)
                {
                    return;
                }

                bool hasrow = Globals.G_DsMachine.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);
                if (hasrow)
                {
                    string filenm = "AutoErrLogTask_" + DateTime.Now.ToString("yyyyMMdd_HHmm") + ".txt";
                    string fullpath = Path.Combine(Errfilepath, filenm);

                    _StatusAutoDownload = true;

                    // Define the cancellation token.
                    CancellationTokenSource source = new CancellationTokenSource();
                    CancellationToken token = source.Token;
                    List<Task> tasks = new List<Task>();
                    TaskFactory factory = new TaskFactory(token);
                    foreach (DataRow dr in Globals.G_DsMachine.Tables[0].Rows)
                    {
                        string ip = dr["MachineIP"].ToString();
                        string ioflg = dr["IOFLG"].ToString();

                        tasks.Add(factory.StartNew(() => download(ip, ioflg,token)));
                        Thread.Sleep(100);
                    }

                    try
                    {
                        Task.WaitAll(tasks.ToArray());
                                                
                    }
                    catch (AggregateException ae)
                    {
                       
                        foreach (Exception e in ae.InnerExceptions)
                        {
                            if (e is TaskCanceledException)
                            {
                                using (System.IO.StreamWriter file = new System.IO.StreamWriter(fullpath, true))
                                {
                                    file.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "-AutoDownload-" + ((TaskCanceledException)e).Message );
                                }   
 
                            }else
                            {
                                using (System.IO.StreamWriter file = new System.IO.StreamWriter(fullpath, true))
                                {
                                    file.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "-AutoDownload-" + e.GetType().Name );
                                }
                            }                             
                        }
                    }
                    catch (Exception ex)
                    {
                        using (System.IO.StreamWriter file = new System.IO.StreamWriter(fullpath, true))
                        {
                            file.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "-AutoDownload-" + ex.ToString());
                        }
                    }
                    finally
                    {
                        source.Cancel();
                        source.Dispose();
                    }


                    _StatusAutoDownload = false;
                }
            }

            private static void download(string ip, string ioflg,CancellationToken token)
            {
                try
                {

                    ServerMsg tMsg = new ServerMsg();
                    tMsg.MsgTime = DateTime.Now;
                    tMsg.MsgType = "Auto Download";
                    tMsg.Message = ip;
                    Scheduler.Publish(tMsg);


                    string err = string.Empty;
                    string filenm = "AutoErrLog_" + DateTime.Now.ToString("yyyyMMdd_HHmm") + "_" + ip + ".txt";
                    string fullpath = Path.Combine(Errfilepath, filenm);

                    clsMachine m = new clsMachine(ip, ioflg);
                    m.Connect(out err);
                    if (!string.IsNullOrEmpty(err))
                    {
                        //write primary errors
                        using (System.IO.StreamWriter file = new System.IO.StreamWriter(fullpath, true))
                        {
                            file.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "-AutoDownload-[" + ip + "]-" + err);
                        }

                        tMsg.MsgTime = DateTime.Now;
                        tMsg.MsgType = "Auto Download";
                        tMsg.Message = ip;
                        Scheduler.Publish(tMsg);
                        return;
                    }
                    err = string.Empty;

                    //if (token.IsCancellationRequested)
                    //{
                    //    m.DisConnect(out err);
                    //    //token.ThrowIfCancellationRequested();
                    //    return;
                    //}

                    List<AttdLog> tempattd = new List<AttdLog>();
                    m.GetAttdRec(out tempattd, out err);
                    if (!string.IsNullOrEmpty(err))
                    {
                        //write errlog
                        using (System.IO.StreamWriter file = new System.IO.StreamWriter(fullpath, true))
                        {
                            file.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "-AutoDownload-[" + ip + "]-" + err);
                        }

                        tMsg.MsgTime = DateTime.Now;
                        tMsg.MsgType = "Auto Download";
                        tMsg.Message = ip + "->Error :" + err;
                        Scheduler.Publish(tMsg);
                        if (err.Contains("ErrorCode=-2"))
                            m.Restart(out err);


                        return;
                    }


                    filenm = "AutoDownload_Log_" + DateTime.Now.ToString("yyyyMMdd") + "_" + ip + ".txt";
                    fullpath = Path.Combine(Loginfopath, filenm);
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(fullpath, true))
                    {
                        file.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "-AutoDownload-[" + ip + "]-Completed");
                    }

                    m.DisConnect(out err);
                }
                catch (Exception ex)
                {
                    string filenm = "AutoErrLog_" + DateTime.Now.ToString("yyyyMMdd_HHmm") + "_" + ip + ".txt";
                    string fullpath = Path.Combine(Errfilepath, filenm);
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(fullpath, true))
                    {
                        file.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "-AutoDownload-[" + ip + "]-" + ex.ToString());
                    }
                }

            }

        }

        public class AutoTimeSet : IJob
        {
            public void Execute(IJobExecutionContext context)
            {
                if (_ShutDown)
                {
                    return;
                }

                bool hasrow = Globals.G_DsMachine.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);
                if (hasrow)
                {
                    _StatusAutoTimeSet = true;
                    foreach (DataRow dr in Globals.G_DsMachine.Tables[0].Rows)
                    {
                        if (_ShutDown)
                        {
                            return;
                        }
                        
                        
                        string ip = dr["MachineIP"].ToString();

                        ServerMsg tMsg = new ServerMsg();
                        tMsg.MsgTime = DateTime.Now;
                        tMsg.MsgType = "Auto Time Set";
                        tMsg.Message = ip;
                        Scheduler.Publish(tMsg);
                        
                        string ioflg = dr["IOFLG"].ToString();
                        string err = string.Empty;
                        string filenm = "AutoErrLog_" + DateTime.Now.ToString("yyyyMMdd_HHmm") + ".txt";
                        string fullpath = Path.Combine(Errfilepath, filenm);

                        clsMachine m = new clsMachine(ip, ioflg);
                        m.Connect(out err);
                        if (!string.IsNullOrEmpty(err))
                        {
                            //write errlog

                            using (System.IO.StreamWriter file = new System.IO.StreamWriter(fullpath, true))
                            {
                                file.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "-AutoTimeSet-[" + ip + "]-" + err);
                            }

                            tMsg.MsgTime = DateTime.Now;
                            tMsg.MsgType = "Auto Time Set";
                            tMsg.Message = ip + "->Error :" + err;
                            Scheduler.Publish(tMsg);
                            continue;
                        }

                        err = string.Empty;
                        m.SetTime(out err);
                        if (!string.IsNullOrEmpty(err))
                        {
                            //write errlog

                            using (System.IO.StreamWriter file = new System.IO.StreamWriter(fullpath, true))
                            {
                                file.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "-AutoTimeSet-[" + ip + "]-" + err);
                            }
                            tMsg.MsgTime = DateTime.Now;
                            tMsg.MsgType = "Auto Time Set";
                            tMsg.Message = ip + "->Error :" + err;
                            Scheduler.Publish(tMsg);
                            continue;
                        }

                        m.DisConnect(out err);
                        tMsg.MsgTime = DateTime.Now;
                        tMsg.MsgType = "Auto Time Set";
                        tMsg.Message = ip + "->Completed";
                        Scheduler.Publish(tMsg);

                        filenm = "AutoTimeSet_Log_" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
                        fullpath = Path.Combine(Loginfopath, filenm);
                        using (System.IO.StreamWriter file = new System.IO.StreamWriter(fullpath, true))
                        {
                            file.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "-AutoTimeSet-[" + ip + "]-Completed");
                        }

                    }

                    _StatusAutoTimeSet = false;
                }
            }
        }

        //iStatefuljob will help to preserv jobdatamap after execution
        [PersistJobDataAfterExecution]
        public class AutoProcess : IJob
        {
            public void Execute(IJobExecutionContext context)
            {
                if (_ShutDown)
                {
                    return;
                }
                
                JobKey key = context.JobDetail.Key;
                JobDataMap dataMap = context.JobDetail.JobDataMap;
                string tWrkGrp = dataMap.GetString("WrkGrp");
                
                
                string tsql = "Select EmpUnqID from MastEmp where CompCode = '01' and WrkGrp = '" + tWrkGrp + "' And Active = 1";
                DateTime ToDt = DateTime.Now.Date;
                DateTime FromDt = DateTime.Now.Date.AddDays(-1);
                string cnerr = string.Empty;

                DataSet DsEmp = Utils.Helper.GetData(tsql, Utils.Helper.constr,out cnerr);
                
                if (!string.IsNullOrEmpty(cnerr))
                {
                    _StatusAutoProcess = false;
                    string filenminfo = "AutoProcess_Error_" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
                    string fullpath2 = Path.Combine(Errfilepath, filenminfo);
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(fullpath2, true))
                    {
                        file.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "-AutoProcess-Error-" + tWrkGrp + " : " + cnerr);
                        file.WriteLine("SQL : " + tsql);
                    }
                    return;
                }
                

                bool hasRows = DsEmp.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);
                if (hasRows)
                {
                    

                    string filenminfo = "AutoProcess_Info_" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
                    string fullpath2 = Path.Combine(Loginfopath, filenminfo);
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(fullpath2, true))
                    {
                        file.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "-AutoProcess-Started-" + tWrkGrp);
                    }

                    foreach (DataRow dr in DsEmp.Tables[0].Rows)
                    {
                        if (_ShutDown)
                        {
                            return;
                        }

                        _StatusAutoProcess = true;
                        
                        string tEmpUnqID = dr["EmpUnqID"].ToString();
                        
                        ServerMsg tMsg = new ServerMsg();
                        tMsg.MsgTime = DateTime.Now;
                        tMsg.MsgType = "Auto Process";
                        tMsg.Message = tEmpUnqID;
                        Scheduler.Publish(tMsg);
                        
                        string err = string.Empty;
                        int tres = 0;
                        clsProcess pro = new clsProcess();
                        pro.AttdProcess(tEmpUnqID,FromDt,ToDt,out tres,out err);

                        if (!string.IsNullOrEmpty(err))
                        {
                            string filenm = "AutoProcess_Error_" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
                            string fullpath = Path.Combine(Errfilepath, filenm);
                            using (System.IO.StreamWriter file = new System.IO.StreamWriter(fullpath, true))
                            {
                                file.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "-AutoProcess-[" + tEmpUnqID + "]-" + err);
                            }

                            tMsg.MsgTime = DateTime.Now;
                            tMsg.MsgType = "Auto Process";
                            tMsg.Message = tEmpUnqID + ": Error=>" + err;
                            Scheduler.Publish(tMsg);

                        }
                    }

                    filenminfo = "AutoProcess_Info_" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
                    fullpath2 = Path.Combine(Loginfopath, filenminfo);
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(fullpath2, true))
                    {
                        file.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "-AutoProcess-Completed-" + tWrkGrp);
                    }

                }
                else
                {
                    string filenminfo = "AutoProcess_Error_" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
                    string fullpath2 = Path.Combine(Errfilepath, filenminfo);
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(fullpath2, true))
                    {
                        file.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "-AutoProcess-Error-" + tWrkGrp + " : " + "No Records Found..");
                    }
                    
                }

                _StatusAutoProcess = false;
            }
        }

        //iStatefuljob will help to preserv jobdatamap after execution
        [PersistJobDataAfterExecution]
        public class AutoArrival : IJob
        {
            public void Execute(IJobExecutionContext context)
            {

                JobKey key = context.JobDetail.Key;
                JobDataMap dataMap = context.JobDetail.JobDataMap;               
                
                string FromTime = dataMap.GetString("FromTime");
                string ToTime  = dataMap.GetString("ToTime");

                TimeSpan tFrom, tTo;
                ServerMsg tMsg = new ServerMsg();

                if (!TimeSpan.TryParse(FromTime, out tFrom))
                {
                    tMsg.MsgTime = DateTime.Now;
                    tMsg.MsgType = "Arrival";
                    tMsg.Message = "did not get arrival from time";
                    Scheduler.Publish(tMsg);
                    return;
                }

                if (!TimeSpan.TryParse(ToTime, out tTo))
                {

                    tMsg.MsgTime = DateTime.Now;
                    tMsg.MsgType = "Arrival";
                    tMsg.Message = "did not get arrival To time";
                    Scheduler.Publish(tMsg);
                    
                    return;
                }

                _StatusAutoArrival = true;

                DateTime tFromTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                tFromTime = tFromTime.AddHours(tFrom.Hours).AddMinutes(tFrom.Minutes);
                DateTime tToTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                tToTime = tToTime.AddHours(tTo.Hours).AddMinutes(tTo.Minutes);

                tMsg.MsgTime = DateTime.Now;
                tMsg.MsgType = "Arrival";
                tMsg.Message = "Processing Started : From " + FromTime + "-" + ToTime ;
                Scheduler.Publish(tMsg);
                
                clsProcess pro = new clsProcess();
                int result;
                pro.ArrivalProcess(tFromTime, tToTime, out result);

                if (result == 1)
                {
                    
                    string filenm = "AutoArrival_Info_" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
                    string fullpath = Path.Combine(Loginfopath, filenm);
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(fullpath, true))
                    {
                        file.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "-AutoArrival-Completed" );
                    }

                    tMsg.MsgTime = DateTime.Now;
                    tMsg.MsgType = "Arrival";
                    tMsg.Message = "Processing Complete : From " + FromTime + "-" + ToTime;
                    Scheduler.Publish(tMsg);

                }
                else
                {
                    
                    string filenm = "AutoArrival_Error_" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
                    string fullpath = Path.Combine(Errfilepath, filenm);
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(fullpath, true))
                    {
                        file.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "-AutoArrival");
                    }


                    tMsg.MsgTime = DateTime.Now;
                    tMsg.MsgType = "Arrival";
                    tMsg.Message = "Processing Error : From " + FromTime + "-" + ToTime;
                    Scheduler.Publish(tMsg);
                }

                _StatusAutoArrival = false;

            }
        }

        public class WorkerProcess : IJob
        {
            public void Execute(IJobExecutionContext context)
            {
                if (_ShutDown)
                {
                    return;
                }

                ServerMsg tMsg = new ServerMsg();
                tMsg.MsgTime = DateTime.Now;
                tMsg.MsgType = "Greetings";
                tMsg.Message = "HeartBeat";
                Scheduler.Publish(tMsg);

                if(_StatusWorker == false)
                { 
                    string cnerr = string.Empty;
                    string sql = "Select top 200 w.* from attdworker w where w.doneflg = 0  Order by MsgId desc" ;
                    DataSet DsEmp = Utils.Helper.GetData(sql, Utils.Helper.constr,out cnerr);
                    if (!string.IsNullOrEmpty(cnerr))
                    {
                        _StatusWorker = false;
                        return;
                    }

                    bool hasRows = DsEmp.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);
                    if (hasRows)
                    {

                        

                        foreach (DataRow dr in DsEmp.Tables[0].Rows)
                        {

                            if (_ShutDown)
                            {
                                _StatusWorker = false;
                                return;
                            }


                            _StatusWorker = true;
                            
                            string tEmpUnqID = dr["EmpUnqID"].ToString();
                            DateTime tFromDt = Convert.ToDateTime(dr["FromDt"]);
                            DateTime tToDt = Convert.ToDateTime(dr["ToDt"]);

                            string MsgID = dr["MsgID"].ToString();

                            tMsg = new ServerMsg();
                            tMsg.MsgTime = DateTime.Now;
                            tMsg.MsgType = "Worker Process";
                            tMsg.Message = tEmpUnqID;
                            Scheduler.Publish(tMsg);

                            string err = string.Empty;
                            int tres = 0;
                            clsProcess pro = new clsProcess();

                            string ProType = dr["ProcessType"].ToString();

                            if (ProType == "ATTD")
                                pro.AttdProcess(tEmpUnqID, tFromDt, tToDt, out tres, out err);
                            else if (ProType == "LUNCHINOUT")
                                pro.LunchInOutProcess(tEmpUnqID, tFromDt, tToDt, out tres);
                            else if (ProType == "MESS")
                                pro.LunchProcess(tEmpUnqID, tFromDt, tToDt, out tres);
                            else
                                pro.AttdProcess(tEmpUnqID, tFromDt, tToDt, out tres, out err);
                            
                                if (!string.IsNullOrEmpty(err))
                                {


                                    string filenm = "AutoProcess_Error_" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
                                    string fullpath = Path.Combine(Errfilepath, filenm);
                                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(fullpath, true))
                                    {
                                        file.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "-AutoProcess-[" + tEmpUnqID + "]-" + err);
                                    }

                                    tMsg.MsgTime = DateTime.Now;
                                    tMsg.MsgType = "Auto Process";
                                    tMsg.Message = tEmpUnqID + ": Error=>" + err;
                                    Scheduler.Publish(tMsg);
                                }
                                else
                                {
                                    using (SqlConnection cn = new SqlConnection(Utils.Helper.constr))
                                    {
                                        try
                                        {
                                            cn.Open();
                                            using (SqlCommand cmd = new SqlCommand())
                                            {
                                                cmd.Connection = cn;
                                                string upsql = "Update AttdWorker set doneflg = 1 , pushflg = 1,workerid ='Server' where msgid = '" + MsgID + "'";
                                                cmd.CommandText = upsql;
                                                cmd.ExecuteNonQuery();
                                            }
                                        }
                                        catch
                                        {

                                        }
                                    }
                                }
                        }

                        _StatusWorker = false;

                    }
                    else
                    {
                        //check if any pending machine operation if yes do it....
                        #region newmachinejob
                        DataSet ds = Utils.Helper.GetData("Select top 10 * from MastMachineUserOperation where DoneFlg = 0 and Operation not in ('BLOCK','UNBLOCK') order by MachineIP ", Utils.Helper.constr);
                        hasRows = ds.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);
                        if (hasRows)
                        {
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {

                                if (_ShutDown)
                                {
                                    _StatusWorker = false;
                                    return;
                                }

                                _StatusWorker = true;

                                string ip = dr["MachineIP"].ToString();
                                string err = string.Empty;
                                clsMachine m = new clsMachine(ip, dr["IOFLG"].ToString());
                                m.Connect(out err);
                                if (string.IsNullOrEmpty(err))
                                {

                                    tMsg.MsgTime = DateTime.Now;
                                    tMsg.MsgType = "Machine Operation->";
                                    tMsg.Message = "Performing : " + dr["Operation"].ToString() + " : EmpUnqID=>" + dr["EmpUnqID"].ToString() + "->" + dr["MachineIP"].ToString();
                                    Scheduler.Publish(tMsg);

                                    m.EnableDevice(false);
                                    #region machineoperation
                                    switch (dr["Operation"].ToString())
                                    {
                                        
                                        case "DELETE":
                                            m.DeleteUser(dr["EmpUnqID"].ToString(), out err);
                                            break;
                                        case "REGISTER":
                                            m.Register(dr["EmpUnqID"].ToString(), out err);
                                            break;
                                        case "DOWNLOADTEMP":
                                            m.DownloadTemplate(dr["EmpUnqID"].ToString(), out err);
                                            break;
                                        case "SETTIME":
                                            m.SetTime(out err);
                                            break;
                                        default:
                                            err = "undefined activity";
                                            break;
                                    }
                                    #endregion

                                    #region setsts
                                    using (SqlConnection cn = new SqlConnection(Utils.Helper.constr))
                                    {
                                        try
                                        {
                                            cn.Open();
                                            using (SqlCommand cmd = new SqlCommand())
                                            {
                                                cmd.Connection = cn;
                                                if (string.IsNullOrEmpty(err))
                                                {
                                                    sql = "Update MastMachineUserOperation Set DoneFlg = 1, DoneDt = GetDate(), LastError = 'Completed' , " +
                                                        " UpdDt=GetDate() where ID ='" + dr["ID"].ToString() + "' and MachineIP = '" + dr["MachineIP"].ToString() + "' and Operation = '" + dr["Operation"].ToString() + "' and EmpUnqID ='" +  dr["EmpUnqID"].ToString() + "';";
                                                }
                                                else
                                                {
                                                    sql = "Update MastMachineUserOperation Set UpdDt=GetDate(), LastError = '" + err + "' " +
                                                        " where ID ='" + dr["ID"].ToString() + "' and MachineIP = '" + dr["MachineIP"].ToString() + "' and Operation = '" + dr["Operation"].ToString() + "' and EmpUnqID ='" + dr["EmpUnqID"].ToString() + "';";
                                                }
                                                cmd.CommandText = sql;
                                                cmd.ExecuteNonQuery();
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            tMsg.MsgTime = DateTime.Now;
                                            tMsg.MsgType = "Machine Operation->";
                                            tMsg.Message = "Error : " + dr["Operation"].ToString() + " : EmpUnqID=>" + dr["EmpUnqID"].ToString() + "->" + dr["MachineIP"].ToString() + "->" + ex.ToString();
                                            Scheduler.Publish(tMsg);

                                        }
                                    }//using
                                    #endregion

                                    m.RefreshData();
                                    m.EnableDevice(true);
                                    m.DisConnect(out err);
                                }
                                else
                                {
                                    #region setsts
                                    tMsg.MsgTime = DateTime.Now;
                                    tMsg.MsgType = "Machine Operation->";
                                    tMsg.Message = "Error : " + dr["Operation"].ToString() + " : EmpUnqID=>" + dr["EmpUnqID"].ToString() + "->" + dr["MachineIP"].ToString() + "->" + err.ToString();
                                    Scheduler.Publish(tMsg);

                                    //record errs
                                    using (SqlConnection cn = new SqlConnection(Utils.Helper.constr))
                                    {
                                        try
                                        {
                                            cn.Open();
                                            using (SqlCommand cmd = new SqlCommand())
                                            {
                                                cmd.Connection = cn;
                                                sql = "Update MastMachineUserOperation Set UpdDt=GetDate(), LastError = '" + err + "' " +
                                                    " where ID ='" + dr["ID"].ToString() + "' and MachineIP = '" + dr["MachineIP"].ToString() + "' " +
                                                    " and Operation = '" + dr["Operation"].ToString() + "';";
                                                cmd.CommandText = sql;
                                                cmd.ExecuteNonQuery();
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            tMsg.MsgTime = DateTime.Now;
                                            tMsg.MsgType = "Machine Operation->";
                                            tMsg.Message = "Error : " + dr["Operation"].ToString() + " : EmpUnqID=>" + dr["EmpUnqID"].ToString() + "->" + dr["MachineIP"].ToString() + "->" + ex.ToString();
                                            Scheduler.Publish(tMsg);
                                        }
                                    }//using
                                    #endregion
                                }
                               
                            }//foreach

                            
                        }
                        #endregion
                    }
                }
                else
                {
                    _StatusWorker = false;

                }

                _StatusWorker = false;
            }
        }
        
        public class BlockUnBlockOperation : IJob
        {
            async Task Block(string machineip, string EmpUnqID, int id,string optype)
            {
                
                string err = string.Empty;
                clsMachine m = new clsMachine(machineip, "B");
                await Task.Run(() =>
                {
                    ServerMsg tMsg = new ServerMsg();
                    m.Connect(out err);
                    if (string.IsNullOrEmpty(err))
                    {
                        
                        tMsg.MsgTime = DateTime.Now;
                        tMsg.MsgType = "Machine Operation->";
                        tMsg.Message = "Performing : " + optype + " : EmpUnqID=>" + EmpUnqID + "->" + machineip;
                        Scheduler.Publish(tMsg);
                        if (optype == "BLOCK")
                            m.BlockUser(EmpUnqID, out err);
                        //else if (optype == "UNBLOCK")
                        //    m.UnBlockUser(EmpUnqID, out err);
                        string err2 = string.Empty;
                        m.DisConnect(out err2);

                        using (SqlConnection cn = new SqlConnection(Utils.Helper.constr))
                        {
                            try
                            {
                                cn.Open();
                                using (SqlCommand cmd = new SqlCommand())
                                {
                                    string sql = string.Empty;
                                    cmd.Connection = cn;
                                    if (string.IsNullOrEmpty(err))
                                    {
                                        sql = "Update MastMachineUserOperation Set DoneFlg = 1, DoneDt = GetDate(), LastError = 'Completed' , " +
                                            " UpdDt=GetDate() where ID ='" + id.ToString() + "' and MachineIP = '" + machineip.ToString() + "' and Operation = '" + optype + "' and EmpUnqID ='" + EmpUnqID.ToString() + "';";
                                    }
                                    else
                                    {
                                        sql = "Update MastMachineUserOperation Set UpdDt=GetDate(), LastError = '" + err + "' " +
                                            " where ID ='" + id.ToString() + "' and MachineIP = '" + machineip.ToString() + "' and Operation = '" + optype + "' and EmpUnqID ='" + EmpUnqID.ToString() + "';";
                                    }
                                    cmd.CommandText = sql;
                                    cmd.ExecuteNonQuery();
                                }
                            }
                            catch (Exception ex)
                            {
                                tMsg.MsgTime = DateTime.Now;
                                tMsg.MsgType = "Machine Operation->";
                                tMsg.Message = "Error : " + optype + " : EmpUnqID=>" + EmpUnqID.ToString() + "->" + machineip.ToString() + "->" + ex.Message.ToString();
                                Scheduler.Publish(tMsg);

                            }
                        }
                        
                       
                    }

                    

                });
                
               
                
            }
            
            public void Execute(IJobExecutionContext context)
            {
                if (_ShutDown)
                {
                    return;
                }

                
                    string cnerr = string.Empty;
                    string sql = "Select top 10 * from MastMachineUserOperation where DoneFlg = 0 and Operation in('BLOCK','UNBLOCK') order by MachineIP ";
                    
                    //check if any pending machine operation if yes do it....
                    #region newmachinejob
                    DataSet ds = Utils.Helper.GetData(sql, Utils.Helper.constr);
                    bool hasRows = ds.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);
                    if (hasRows)
                    {
                        int cnt = ds.Tables[0].Rows.Count;
                        List<Task> tasklist = new List<Task>();
                        
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {

                            if (_ShutDown)
                            {
                                _StatusWorker = false;
                                return;
                            }
                            
                            string emp = dr["EmpUnqID"].ToString();
                            string ip = dr["MachineIP"].ToString();
                            string optyp = dr["Operation"].ToString();

                            string err = string.Empty;
                            int id = Convert.ToInt32(dr["ID"]);
                            tasklist.Add(Block(ip, emp, id,optyp));
                             
                        }

                        foreach (Task t in tasklist)
                        {
                            t.Start();
                            Thread.Sleep(10);
                        }
                        
                    }
                    #endregion
                    
            }
        }


        public class AutoDeleteExpireValidityEmp : IJob
        {
            public void Execute(IJobExecutionContext context)
            {
                if (_ShutDown)
                {
                    return;
                }

                if (_StatusAutoArrival == false &&
                   _StatusAutoDownload == false &&
                   _StatusAutoProcess == false &&
                   _StatusAutoTimeSet == false &&
                   _StatusWorker == false)
                {
                    //
                    string sql = "Select EmpUnqID from MastEmp where Active = 1 and ValidityExpired = 0 and ValidTo < GetDate() and WrkGrp <> 'COMP' AND COMPCODE = '01'";
                    string cnerr = string.Empty;

                    DataSet dsEmp = Utils.Helper.GetData(sql, Utils.Helper.constr, out cnerr);
                    if (!string.IsNullOrEmpty(cnerr))
                    {
                        _StatusWorker = false;
                        return;
                    }

                    bool Emphasrows = dsEmp.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);
                    string filenm = "AutoDeleteEmp_" + DateTime.Now.ToString("yyyyMMdd_HHmm") + ".txt";

                    if (Emphasrows)
                    {

                        foreach (DataRow dr in dsEmp.Tables[0].Rows)
                        {
                            if (_ShutDown)
                            {
                                _StatusWorker = false;
                                return;
                            }

                            _StatusWorker = true;
                            string tsql = "select * from ReaderConFig where canteenflg = 0  and [master] = 0 and compcode = '01' and MachineIP in " +
                                "( " +
                                "select Distinct MachineIP from AttdLog where EmpUnqID = '" + dr["EmpUnqID"].ToString() + "' and Punchdate between DATEADD(day,-60,getdate()) and DateAdd(day,1,GETDATE()) " +
                                ")" +
                                " Union Select MachineIP From TripodReaderConfig  ";

                            string err = string.Empty;
                            DataSet empIPds = Utils.Helper.GetData(tsql, Utils.Helper.constr, out err);
                            bool hasrow = empIPds.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);
                            if (hasrow)
                            {
                                string maxid = Utils.Helper.GetDescription("Select isnull(Max(ID),0) + 1 from MastMachineUserOperation", Utils.Helper.constr, out err);
                                if (!string.IsNullOrEmpty(err))
                                {
                                    _StatusWorker = false;
                                    return;
                                }
                                
                                foreach (DataRow tdr in empIPds.Tables[0].Rows)
                                {
                                    ServerMsg tMsg = new ServerMsg();
                                    tMsg.MsgTime = DateTime.Now;
                                    tMsg.MsgType = "Auto Delete Validity Expired Employee";
                                    tMsg.Message = tdr["MachineIP"].ToString();
                                    Scheduler.Publish(tMsg);

                                    using (SqlConnection cn = new SqlConnection(Utils.Helper.constr))
                                    {
                                        try
                                        {
                                            cn.Open();
                                            using (SqlCommand cmd = new SqlCommand())
                                            {
                                                tsql = "Insert into MastMachineUserOperation (ID,EmpUnqID,MachineIP,IOFLG,Operation,ReqDt,ReqBy,Remarks,AddDt) Values " +
                                                    "('" + maxid + "','" + dr["EmpUnqID"].ToString() + "','" + tdr["MachineIP"].ToString() + "','" + tdr["IOFLG"].ToString() + "','DELETE',GetDate(),'SERVER','Validity Expired',GetDate())";

                                                cmd.Connection = cn;
                                                cmd.CommandType = CommandType.Text;
                                                
                                                cmd.CommandText = tsql;
                                                cmd.ExecuteNonQuery();

                                                tsql = "Update MastEmp Set ValidityExpired = 1 Where EmpUnqID = '" + dr["EmpUnqID"].ToString() + "' And CompCode = '01'";
                                                cmd.CommandText = tsql;
                                                cmd.ExecuteNonQuery();

                                                string filenm2 = "AutoDeleteExpEmp_" + DateTime.Now.ToString("yyyyMMdd_HHmm") + ".txt";
                                                string fullpath2 = Path.Combine(Loginfopath, filenm2);
                                                using (System.IO.StreamWriter file = new System.IO.StreamWriter(fullpath2, true))
                                                {
                                                    file.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "-Auto Delete Validity Expired Employee->" + dr["EmpUnqID"].ToString() + "=>[" + tdr["MachineIP"].ToString() + "]-Completed");
                                                }
                                            }

                                        }
                                        catch (Exception ex)
                                        {
                                            string filenm2 = "AutoDeleteExpEmp_" + DateTime.Now.ToString("yyyyMMdd_HHmm") + ".txt";
                                            string fullpath = Path.Combine(Errfilepath, filenm2);
                                            using (System.IO.StreamWriter file = new System.IO.StreamWriter(fullpath, true))
                                            {
                                                file.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "-Auto Delete Validity Expired Employee->" + dr["EmpUnqID"].ToString() + "=>[" + tdr["MachineIP"].ToString() + "]-" + ex.Message.ToString());
                                            }
                                        }
                                    }//using connection

                                    _StatusWorker = true;

                                }//foreach employee ip

                            } //if employees punching ip's found

                        } //foreach employee

                        _StatusWorker = false;

                    }// if employee found

                }//if server free

                _StatusWorker = false;
            }
        }

      class TriggerListenerExample:ITriggerListener
      {
          public void TriggerFired(ITrigger trigger, IJobExecutionContext context)
         {
             Console.WriteLine("The scheduler called {0} for trigger {1}", MethodBase.GetCurrentMethod().Name, trigger.Key);
         }
 
         public bool VetoJobExecution(ITrigger trigger, IJobExecutionContext context)
         {
             Console.WriteLine("The scheduler called {0} for trigger {1}", MethodBase.GetCurrentMethod().Name,trigger.Key);
             return false;
         }
 
         public void TriggerComplete(ITrigger trigger, IJobExecutionContext context, SchedulerInstruction triggerInstructionCode)
         {
             Console.WriteLine("The scheduler called {0} for trigger {1}", MethodBase.GetCurrentMethod().Name, trigger.Key);
         }
 
         public void TriggerMisfired(ITrigger trigger)
         {
             Console.WriteLine("The scheduler called {0} for trigger {1}", MethodBase.GetCurrentMethod().Name, trigger.Key);
         }
 
         public string Name
         {
             get { return "TriggerListenerExample"; }
         }
     }

      public class SchedulerListenerExample : ISchedulerListener
      {

          public void JobAdded(IJobDetail jobDetail)
          {
              Console.WriteLine("The scheduler called {0}", MethodBase.GetCurrentMethod().Name);
          }

          public void JobDeleted(JobKey jobKey)
          {
              Console.WriteLine("The scheduler called {0}", MethodBase.GetCurrentMethod().Name);
          }

          public void JobPaused(JobKey jobKey)
          {
              Console.WriteLine("The scheduler called {0}", MethodBase.GetCurrentMethod().Name);
          }

          public void JobResumed(JobKey jobKey)
          {
              Console.WriteLine("The scheduler called {0}", MethodBase.GetCurrentMethod().Name);
          }

          public void JobScheduled(ITrigger trigger)
          {
              Console.WriteLine("The scheduler called {0}", MethodBase.GetCurrentMethod().Name);
          }

          public void JobUnscheduled(TriggerKey triggerKey)
          {
              Console.WriteLine("The scheduler called {0}", MethodBase.GetCurrentMethod().Name);
          }

          public void JobsPaused(string jobGroup)
          {
              Console.WriteLine("The scheduler called {0}", MethodBase.GetCurrentMethod().Name);
          }

          public void JobsResumed(string jobGroup)
          {
              Console.WriteLine("The scheduler called {0}", MethodBase.GetCurrentMethod().Name);
          }

          public void SchedulerError(string msg, SchedulerException cause)
          {
              Console.WriteLine("The scheduler called {0}", MethodBase.GetCurrentMethod().Name);
          }

          public void SchedulerInStandbyMode()
          {
              Console.WriteLine("The scheduler called {0}", MethodBase.GetCurrentMethod().Name);
          }

          public void SchedulerShutdown()
          {
              Console.WriteLine("The scheduler called {0}", MethodBase.GetCurrentMethod().Name);
          }

          public void SchedulerShuttingdown()
          {
              Console.WriteLine("The scheduler called {0}", MethodBase.GetCurrentMethod().Name);
          }

          public void SchedulerStarted()
          {
              Console.WriteLine("The scheduler called {0}", MethodBase.GetCurrentMethod().Name);
          }

          public void SchedulerStarting()
          {
              Console.WriteLine("The scheduler called {0}", MethodBase.GetCurrentMethod().Name);
          }

          public void SchedulingDataCleared()
          {
              Console.WriteLine("The scheduler called {0}", MethodBase.GetCurrentMethod().Name);
          }

          public void TriggerFinalized(ITrigger trigger)
          {
              Console.WriteLine("The scheduler called {0}", MethodBase.GetCurrentMethod().Name);
          }

          public void TriggerPaused(TriggerKey triggerKey)
          {
              Console.WriteLine("The scheduler called {0}", MethodBase.GetCurrentMethod().Name);
          }

          public void TriggerResumed(TriggerKey triggerKey)
          {
              Console.WriteLine("The scheduler called {0}", MethodBase.GetCurrentMethod().Name);
          }

          public void TriggersPaused(string triggerGroup)
          {
              Console.WriteLine("The scheduler called {0}", MethodBase.GetCurrentMethod().Name);
          }

          public void TriggersResumed(string triggerGroup)
          {
              Console.WriteLine("The scheduler called {0}", MethodBase.GetCurrentMethod().Name);
          }
      }
        
      class DummyJobListener : IJobListener
        {
            
            public readonly Guid Id = Guid.NewGuid();
          
            public void JobToBeExecuted(IJobExecutionContext context)
            {
                JobKey jobKey = context.JobDetail.Key;
            
            }

            public void JobWasExecuted(IJobExecutionContext context, JobExecutionException jobException)
            {
                JobKey jobKey = context.JobDetail.Key;                
  
                    
                string body = "Job ID : " + context.JobDetail.Key.ToString() + "</br>" +
                                "Job Group : " + context.JobDetail.Key.Group.ToString() + "</br>" +
                                "Job Description : " + context.JobDetail.Description + "</br>" +
                                "Job Executed on : " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    
                string subject = "Attendance System : Notification : " + context.JobDetail.Description;

                    
                if (context.JobDetail.Key.ToString().Contains("Job_AutoDownload.Job_AutoDownload"))
                {
                    #region tryattach
                    try
                    {
                        const int chunkSize = 2 * 1024; // 2KB

                        var inputFiles = Directory.GetFiles(Errfilepath)
                            .Where(x => new FileInfo(x).CreationTime.Date == DateTime.Today.Date);

                        string allErrFileName = DateTime.Now.Date.ToString("yyyyMMdd") + "Error_Logs.txt";
                        
                        string fullpath = Path.Combine(Errfilepath, allErrFileName);

                        using (var output = File.Create(fullpath))
                        {
                            foreach (var file in inputFiles)
                            {
                                if (file.Contains(allErrFileName))
                                    continue;
                                using (var input = File.OpenRead(file))
                                {
                                    var buffer = new byte[chunkSize];
                                    int bytesRead;
                                    while ((bytesRead = input.Read(buffer, 0, buffer.Length)) > 0)
                                    {
                                        output.Write(buffer, 0, bytesRead);
                                    }
                                }
                            }
                        }

                        byte[] filecontent = File.ReadAllBytes(fullpath);

                        if (filecontent.Length > 0)
                        {
                            MailAttachment m = new MailAttachment(filecontent, allErrFileName);
                            string err = EmailHelper.Email(Globals.G_JobNotificationEmail, "", "", body, subject, Globals.G_DefaultMailID,
                            Globals.G_DefaultMailID, "", "", m);
                        }
                        else
                        {
                            string err = EmailHelper.Email(Globals.G_JobNotificationEmail, "", "", body, subject, Globals.G_DefaultMailID,
                            Globals.G_DefaultMailID, "", "");
                        }

                    }
                    catch {

                        string err = EmailHelper.Email(Globals.G_JobNotificationEmail, "", "", body, subject, Globals.G_DefaultMailID,
                        Globals.G_DefaultMailID, "", "");
                    }

                    #endregion
                }
                else
                {
                    string err = EmailHelper.Email(Globals.G_JobNotificationEmail, "", "", body, subject, Globals.G_DefaultMailID,
                    Globals.G_DefaultMailID, "", "");
                }
               
            }

            public void JobExecutionVetoed(IJobExecutionContext context)
            {
            }

            public string Name
            {
                get { return "DummyJobListener" + Id; }
            }
        }
        
    }

}
