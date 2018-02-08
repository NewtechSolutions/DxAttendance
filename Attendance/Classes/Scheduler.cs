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
                        .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(tTime.Hours, tTime.Minutes))
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

                foreach (string wrk in tWrkGrp)
                {

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
                        .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(tTime.Hours, tTime.Minutes))
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
                .WithCronSchedule("0 0/2 * * * ?")
                .Build();

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
                    .WithSchedule(CronScheduleBuilder.AtHourAndMinuteOnGivenDaysOfWeek(Globals.G_AutoDelEmpTime.Hours, Globals.G_AutoDelEmpTime.Minutes, onSunday))
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
                        .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(tTime.Hours, tTime.Minutes))                                          
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
                            continue;
                        }

                        m.DisConnect(out err);

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
                    string sql = "Select top 200 w.* from attdworker w where w.doneflg = 0 Order by MsgId desc" ;
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
                                using(SqlConnection cn = new SqlConnection(Utils.Helper.constr))
                                {
                                    try{
                                        cn.Open();
                                        using(SqlCommand cmd = new SqlCommand())
                                        {
                                            cmd.Connection = cn;
                                            string upsql = "Update AttdWorker set doneflg = 1 , pushflg = 1,workerid ='Server' where msgid = '" + MsgID +"'";
                                            cmd.CommandText = upsql;
                                            cmd.ExecuteNonQuery();
                                        }
                                    }catch{

                                    }
                                }
                            }
                        }

                        _StatusWorker = false;

                    }
                }
                else
                {
                    _StatusWorker = false;

                }

                
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
                    string sql = "Select EmpUnqID from MastEmp where Active = 1 and ValidTo < GetDate() and WrkGrp <> 'COMP' AND COMPCODE = '01'";
                    string cnerr = string.Empty;
                    
                    DataSet ds = Utils.Helper.GetData(sql, Utils.Helper.constr,out cnerr);
                    if (!string.IsNullOrEmpty(cnerr))
                    {
                        _StatusWorker = false;
                        return;
                    }

                    bool hasrows = ds.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);
                    string filenm = "AutoDeleteEmp_" + DateTime.Now.ToString("yyyyMMdd_HHmm") + ".txt";
                    
                    if (hasrows)
                    {

                        #region create_expired_emplist
                        //create list of users
                        List<UserBioInfo> tUserList = new List<UserBioInfo>();
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            UserBioInfo tuser = new UserBioInfo();
                            tuser.UserID = dr["EmpUnqID"].ToString();
                            tUserList.Add(tuser);
                        }
                        #endregion
                        
                        bool hasrow = Globals.G_DsMachine.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);
                        if (hasrow)
                        {
                            if (_ShutDown)
                            {
                                _StatusWorker = false;
                                return;
                            }

                            _StatusWorker = true;

                            //loop all machine
                            foreach (DataRow dr in Globals.G_DsMachine.Tables[0].Rows)
                            {
                                
                                if (_ShutDown)
                                {
                                    _StatusWorker = false;
                                    return;
                                }

                                _StatusWorker = true;

                                string Errfullpath = Path.Combine(Errfilepath, "");
                                string ip = dr["MachineIP"].ToString();

                                try
                                {
                                    ServerMsg tMsg = new ServerMsg();
                                    tMsg.MsgTime = DateTime.Now;
                                    tMsg.MsgType = "Auto Delete Validity Expired Employee";
                                    tMsg.Message = ip;
                                    Scheduler.Publish(tMsg);
                                    string ioflg = dr["IOFLG"].ToString();
                                    string err = string.Empty;
                                    #region ConnectMachine
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
                                    #endregion
                                    
                                    err = string.Empty;
                                    List<UserBioInfo> temp = new List<UserBioInfo>();
                                    m.DeleteUser(tUserList,out err,out temp);
                                    string filenm2 = "AutoDeleteExpEmp_" + DateTime.Now.ToString("yyyyMMdd_HHmm") + ".txt";
                                    string fullpath2 = Path.Combine(Loginfopath, filenm2);
                                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(fullpath2, true))
                                    {
                                        file.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "-Auto Delete Validity Expired Employee-[" + ip + "]-Completed");
                                    }
                                    m.RefreshData();
                                    m.DisConnect(out err);
                                    
                                }
                                catch (Exception ex)
                                {
                                    string filenm2 = "AutoDeleteExpEmp_" + DateTime.Now.ToString("yyyyMMdd_HHmm") + ".txt";
                                    string fullpath = Path.Combine(Errfilepath, filenm2);
                                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(fullpath, true))
                                    {
                                        file.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "-Auto Delete Validity Expired Employee-[" + ip + "]-" + ex.ToString());
                                    }
                                }
                            }//foreach loop

                            _StatusWorker = false;
                            
                        }
                        else
                        {
                            _StatusWorker = false;
                            return;
                        }

                    }
                    else
                    {
                        _StatusWorker = false;
                        return;
                    }
                    
                    _StatusWorker = false;
                    
                }
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
                        MailAttachment m = new MailAttachment(filecontent, allErrFileName);
                        string err = EmailHelper.Email(Globals.G_JobNotificationEmail, "", "", body, subject, Globals.G_DefaultMailID,
                        Globals.G_DefaultMailID, "", "",m);
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
