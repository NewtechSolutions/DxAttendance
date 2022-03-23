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
using zkemkeeper;

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
            RegSchedule_WDMSPunchTransferProcess();
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


                #region HardCodeProcess

                //daily 09:50AM Company Employee HardCode

                string jobid2 = "Job_AutoProcess_Comp";
                string triggerid2 = "Trigger_AutoProcess_Comp";

                
                // define the job and tie it to our HelloJob class
                IJobDetail job2 = JobBuilder.Create<AutoProcess>()
                    .WithDescription("Auto Process Attendance Data")
                    .WithIdentity(jobid2, "Job_AutoProcess")
                    .UsingJobData("WrkGrp", "COMP")
                    .Build();

                // Trigger the job to run now, and then repeat every 10 seconds
                ITrigger trigger2 = TriggerBuilder.Create()
                    .WithIdentity(triggerid2, "TRG_AutoProcess")
                    .StartNow()
                    .WithSchedule(
                        CronScheduleBuilder.DailyAtHourAndMinute(9, 50)
                        .WithMisfireHandlingInstructionFireAndProceed()
                        )
                    .Build();

                // Tell quartz to schedule the job using our trigger
                scheduler.ScheduleJob(job2, trigger2);
                

                ServerMsg tMsg2 = new ServerMsg();
                tMsg2.MsgType = "Job Building";
                tMsg2.MsgTime = DateTime.Now;
                tMsg2.Message = string.Format("Building Job Job ID : {0} And Trigger ID : {1}", jobid2, triggerid2);
                Publish(tMsg2);

                //daily 09:50AM Company Employee HardCode
                #endregion
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
                .WithSchedule(CronScheduleBuilder.CronSchedule("0 0/15 * * * ?").WithMisfireHandlingInstructionFireAndProceed())
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
       
        public void RegSchedule_WDMSPunchTransferProcess()
        {
            string jobid = "WDMSPunchTransferProcess";
            string triggerid = "Trigger_WDMSPunchTransfer";

            // define the job and tie it to our HelloJob class
            IJobDetail job = JobBuilder.Create<WDMSPunchTransfer>()
                    .WithDescription("Transfer of WDMS/iClock Punch")
                .WithIdentity(jobid, "WDMSPunchTransfer")
                .Build();

            // Trigger the job to run every 5 minute
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity(triggerid, "TRG_WDMSPunchTransfer")
                .StartNow()
                .WithSchedule(SimpleScheduleBuilder.RepeatSecondlyForever(120).RepeatForever().WithMisfireHandlingInstructionFireNow())
                .Build();

            // Tell quartz to schedule the job using our trigger
            scheduler.ScheduleJob(job, trigger);

            ServerMsg tMsg = new ServerMsg();
            tMsg.MsgType = "Job Building";
            tMsg.MsgTime = DateTime.Now;
            tMsg.Message = string.Format("Building Job Job ID : {0} And Trigger ID : {1}", jobid, triggerid);
            Scheduler.Publish(tMsg);
        }
        
        public class WDMSPunchTransfer : IJob
        {
            public void Execute(IJobExecutionContext context)
            {
                ServerMsg tMsg = new ServerMsg();
                tMsg.MsgTime = DateTime.Now;
                tMsg.MsgType = "WDMS->WDMSPunchTransfer";
                tMsg.Message = "WDMS Punch Transfer starting";
                Scheduler.Publish(tMsg);

                string cnerr = string.Empty;

                string wdms_cnstr = string.Empty, wdms_SrcPunchTable = string.Empty, wdms_PunchQuery = string.Empty;
                string wdms_SyncUpdFieldName = string.Empty, wdms_AttdDestTableName = string.Empty;

                wdms_cnstr = Utils.Helper.GetDescription("Select Config_Val from Mast_OtherConfig where Config_Key ='iClockConnStr'", Utils.Helper.constr, out cnerr);
                wdms_SrcPunchTable = Utils.Helper.GetDescription("Select Config_Val from Mast_OtherConfig where Config_Key ='iClockSrcPunchTable'", Utils.Helper.constr, out cnerr);
                wdms_PunchQuery = Utils.Helper.GetDescription("Select Config_Val from Mast_OtherConfig where Config_Key ='iClockPunchQuery'", Utils.Helper.constr, out cnerr);
                wdms_SyncUpdFieldName = Utils.Helper.GetDescription("Select Config_Val from Mast_OtherConfig where Config_Key ='iClockSyncUpdFieldName'", Utils.Helper.constr, out cnerr);
                wdms_AttdDestTableName = Utils.Helper.GetDescription("Select Config_Val from Mast_OtherConfig where Config_Key ='iClockAttdDestTableName'", Utils.Helper.constr, out cnerr);



                if (string.IsNullOrEmpty(wdms_cnstr.Trim()))
                {
                    tMsg = new ServerMsg();
                    tMsg.MsgTime = DateTime.Now;
                    tMsg.MsgType = "WDMS->WDMSPunchTransfer";
                    tMsg.Message = "Please Configure : iClockConnStr";
                    Scheduler.Publish(tMsg);
                    return;
                }

                if (string.IsNullOrEmpty(wdms_SrcPunchTable.Trim()))
                {
                    tMsg = new ServerMsg();
                    tMsg.MsgTime = DateTime.Now;
                    tMsg.MsgType = "WDMS->WDMSPunchTransfer";
                    tMsg.Message = "Please Configure : iClockSrcPunchTable";
                    Scheduler.Publish(tMsg);
                    return;
                }

                if (string.IsNullOrEmpty(wdms_PunchQuery.Trim()))
                {
                    tMsg = new ServerMsg();
                    tMsg.MsgTime = DateTime.Now;
                    tMsg.MsgType = "WDMS->WDMSPunchTransfer";
                    tMsg.Message = "Please Configure : iClockPunchQuery";
                    Scheduler.Publish(tMsg);
                    return;
                }

                if (string.IsNullOrEmpty(wdms_SyncUpdFieldName.Trim()))
                {
                    tMsg = new ServerMsg();
                    tMsg.MsgTime = DateTime.Now;
                    tMsg.MsgType = "WDMS->WDMSPunchTransfer";
                    tMsg.Message = "Please Configure : iClockSyncUpdFieldName";
                    Scheduler.Publish(tMsg);
                    return;
                }

                if (string.IsNullOrEmpty(wdms_AttdDestTableName.Trim()))
                {
                    tMsg = new ServerMsg();
                    tMsg.MsgTime = DateTime.Now;
                    tMsg.MsgType = "WDMS->WDMSPunchTransfer";
                    tMsg.Message = "Please Configure : iClockAttdDestTableName";
                    Scheduler.Publish(tMsg);
                    return;
                }

                string err = string.Empty;
                DataSet dsMachine = Utils.Helper.GetData(wdms_PunchQuery, wdms_cnstr, out err);
                bool hasRows = dsMachine.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);

                if (hasRows)
                {
                    tMsg = new ServerMsg();
                    tMsg.MsgTime = DateTime.Now;
                    tMsg.MsgType = "WDMS->WDMSPunchTransfer";
                    tMsg.Message = dsMachine.Tables[0].Rows.Count.ToString() + " Records Found";
                    Scheduler.Publish(tMsg);
                    
                    #region Destination

                    foreach (DataRow dr in dsMachine.Tables[0].Rows)
                    {
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
                                tMsg.MsgType = "WDMS->WDMSPunchTransfer";
                                tMsg.Message = ex.Message;
                                Scheduler.Publish(tMsg);
                                return;
                            }

                            string sql = "Insert into [" + wdms_AttdDestTableName + "] (Punchdate,EmpUnqid,IOFLG,MachineIP,LunchFlg,tYear,tYearMt,t1date,AddDt,AddID,verifymode) Values (" +
                                "'" + Convert.ToDateTime(dr["PunchDate"]).ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                "'" + dr["EmpUnqID"].ToString().Trim() + "'," +
                                "'" + dr["IOFLG"].ToString().Trim() + "'," +
                                "'" + dr["MachineIP"].ToString().Trim() + "'," +
                                "'" + dr["LunchFlg"].ToString().Trim() + "'," +
                                "'" + dr["tYear"].ToString().Trim() + "'," +
                                "'" + dr["tYearMT"].ToString().Trim() + "'," +
                                "'" + Convert.ToDateTime(dr["t1date"]).ToString("yyyy-MM-dd") + "'," +
                                " GetDate(),'" + dr["AddID"].ToString().Trim()  + "','" + dr["verifymode"].ToString() + "')";

                            //tMsg = new ServerMsg();
                            //tMsg.MsgTime = DateTime.Now;
                            //tMsg.MsgType = "WDMS->WDMSPunchTransfer";
                            //tMsg.Message = sql;
                            //Scheduler.Publish(tMsg);

                            using (SqlCommand cmd = new SqlCommand())
                            {
                                try
                                {
                                    cmd.Connection = cn;
                                    cmd.CommandText = sql;
                                    cmd.ExecuteNonQuery();

                                    dr[wdms_SyncUpdFieldName] = "1";

                                    //schedule a process attd/lunch
                                    if (dr["IOFLG"].ToString().Trim() == "B")
                                    {
                                        sql = "Insert into AttdWorker (EmpUnqID,FromDt,ToDt,Workerid,DoneFlg,PushFlg,AddDt,AddId,ProcessType) values" +
                                        " ('" + dr["EmpUnqID"].ToString().Trim() + "','" + Convert.ToDateTime(dr["t1date"]).AddDays(-1).ToString("yyyy-MM-dd") + "' " +
                                        " ,'" + Convert.ToDateTime(dr["t1date"]).ToString("yyyy-MM-dd") + "','SERVER',0,0,GetDate(),'SERVER','MESS')";
                                    }
                                    else
                                    {
                                        sql = "Insert into AttdWorker (EmpUnqID,FromDt,ToDt,Workerid,DoneFlg,PushFlg,AddDt,AddId,ProcessType) values" +
                                        " ('" + dr["EmpUnqID"].ToString().Trim() + "','" + Convert.ToDateTime(dr["t1date"]).AddDays(-1).ToString("yyyy-MM-dd") + "' " +
                                        " ,'" + Convert.ToDateTime(dr["t1date"]).ToString("yyyy-MM-dd") + "','SERVER',0,0,GetDate(),'SERVER','ATTD')";
                                    }

                                    cmd.CommandText = sql;
                                    cmd.ExecuteNonQuery();

                                    tMsg = new ServerMsg();
                                    tMsg.MsgTime = DateTime.Now;
                                    tMsg.MsgType = "WDMS->WDMSPunchTransfer";
                                    tMsg.Message = "importing wdms punch->ID :" + dr["id"].ToString() + "--" + dr["EmpUnqID"].ToString();
                                    Scheduler.Publish(tMsg);
                                }
                                catch (Exception ex)
                                {
                                    dr[wdms_SyncUpdFieldName] = "0";
                                    

                                    if (ex.Message.Contains("Cannot insert duplicate key in object 'dbo.ATTDLOG'"))
                                    {
                                        dr[wdms_SyncUpdFieldName] = "1";
                                    }
                                    else
                                    {
                                        tMsg = new ServerMsg();
                                        tMsg.MsgTime = DateTime.Now;
                                        tMsg.MsgType = "WDMS->WDMSPunchTransfer";
                                        tMsg.Message = "importing wdms punch-> Error " + dr["id"].ToString() + "--" + dr["EmpUnqID"].ToString() + "--" + ex.Message.ToString();
                                        Scheduler.Publish(tMsg);
                                    }
                                }

                            }//using command

                            dsMachine.Tables[0].AcceptChanges();

                        } //using connection


                    }//for each loop
                    #endregion

                    #region srcupdate

                    foreach (DataRow dr in dsMachine.Tables[0].Rows)
                    {
                        using (SqlConnection cn = new SqlConnection(wdms_cnstr))
                        {
                            try
                            {
                                cn.Open();
                            }
                            catch (Exception ex)
                            {
                                tMsg = new ServerMsg();
                                tMsg.MsgTime = DateTime.Now;
                                tMsg.MsgType = "WDMS->WDMSPunchTransfer->Source Update Err";
                                tMsg.Message = ex.Message;
                                Scheduler.Publish(tMsg);
                                return;
                            }

                            string sql = string.Empty;
                            if (dr[wdms_SyncUpdFieldName].ToString() == "1")
                            {
                                sql = "Update [" + wdms_SrcPunchTable + "] " +
                                " SET " + wdms_SyncUpdFieldName + " = '1'  Where id ='" + dr["id"].ToString() + "'";
                            }
                            else
                            {
                                sql = "Update [" + wdms_SrcPunchTable + "] " +
                                " SET " + wdms_SyncUpdFieldName + " = 0  Where id ='" + dr["id"].ToString() + "'";
                            }



                            using (SqlCommand cmd = new SqlCommand(sql, cn))
                            {
                                try
                                {
                                    cmd.ExecuteNonQuery();


                                    //tMsg = new ServerMsg();
                                    //tMsg.MsgTime = DateTime.Now;
                                    //tMsg.MsgType = "WDMS->WDMSPunchTransfer->Source Update";
                                    //tMsg.Message = wdms_SyncUpdFieldName + "->wdms table-> id :" + dr["id"].ToString();
                                    //Scheduler.Publish(tMsg);
                                }
                                catch (Exception ex)
                                {
                                    dr[wdms_SyncUpdFieldName] = 0;
                                    tMsg = new ServerMsg();
                                    tMsg.MsgTime = DateTime.Now;
                                    tMsg.MsgType = "WDMS->WDMSPunchTransfer->Source Update Err";
                                    tMsg.Message = wdms_SyncUpdFieldName + "->wdms table-> id :" + dr["id"].ToString() + " Error " + ex.Message.ToString();
                                    Scheduler.Publish(tMsg);
                                }

                            }//using command



                        } //using connection


                    }//for each loop


                    #endregion

                    tMsg = new ServerMsg();
                    tMsg.MsgTime = DateTime.Now;
                    tMsg.MsgType = "WDMS->WDMSPunchTransfer";
                    tMsg.Message = "WDMS Transfer Complete";
                    Scheduler.Publish(tMsg);

                }//if has rows
                else
                {
                    tMsg = new ServerMsg();
                    tMsg.MsgTime = DateTime.Now;
                    tMsg.MsgType = "WDMS->WDMSPunchTransfer";
                    tMsg.Message = "WDMS No Records Founds";
                    Scheduler.Publish(tMsg);
                }

            }// execute

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
                                        file.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "-AutoDelete-[" + ip + "]-(" + dr["MachineDesc"].ToString() + ")-" + err);
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
                                        file.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "-Auto Delete Left Employee-[" + ip + "]-(" + dr["MachineDesc"].ToString() + ")-" + err);
                                    }

                                }

                                string fullpath2 = Path.Combine(Loginfopath, filenm);
                                using (System.IO.StreamWriter file = new System.IO.StreamWriter(fullpath2, true))
                                {
                                    file.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "-Auto Delete Left Employee-[" + ip + "]-(" + dr["MachineDesc"].ToString() + ")-Completed");
                                }
                                m.RefreshData();
                                m.DisConnect(out err);
                            }
                            catch (Exception ex)
                            {
                                string fullpath = Path.Combine(Errfilepath, filenm);
                                using (System.IO.StreamWriter file = new System.IO.StreamWriter(fullpath, true))
                                {
                                    file.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "-Auto Delete Left Employee-[" + ip + "]-(" + dr["MachineDesc"].ToString() + ")-" + ex.ToString());
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
                                    file.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "-AutoDownload-[" + ip + "]-(" + dr["MachineDesc"].ToString() + ")-" + err);
                                }

                                tMsg.MsgTime = DateTime.Now;
                                tMsg.MsgType = "Auto Download";
                                tMsg.Message = ip;
                                Scheduler.Publish(tMsg);
                                continue;
                            }
                            err = string.Empty;
                            m.SetDuplicatePunchDuration(3);
                            List<AttdLog> tempattd = new List<AttdLog>();
                            m.GetAttdRec(out tempattd, out err);
                            if (!string.IsNullOrEmpty(err))
                            {
                                //write errlog
                                using (System.IO.StreamWriter file = new System.IO.StreamWriter(fullpath, true))
                                {
                                    file.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "-AutoDownload-[" + ip + "]-(" + dr["MachineDesc"].ToString() + ")-" + err);
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
                                file.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "-AutoDownload-[" + ip + "]-(" + dr["MachineDesc"].ToString() + ")-Completed");
                            }

                            m.DisConnect(out err);
                        }
                        catch (Exception ex)
                        {
                            string filenm = "AutoErrLog_" + DateTime.Now.ToString("yyyyMMdd_HHmm") + ".txt";
                            string fullpath = Path.Combine(Errfilepath, filenm);
                            using (System.IO.StreamWriter file = new System.IO.StreamWriter(fullpath, true))
                            {
                                file.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "-AutoDownload-[" + ip + "]-(" + dr["MachineDesc"].ToString() + ")-" + ex.ToString());
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

                string sql = "Select Top 20 * " +
                    " From GatePasses " +
                    " Where " +
                    " AttdFlag = 'O'  " +
                    " Order By ID  " ;

                DataSet dsGPass = Utils.Helper.GetData(sql,essConStr,out err);
                if (!string.IsNullOrEmpty(err))
                {
                    tMsg = new ServerMsg();
                    tMsg.MsgTime = DateTime.Now;
                    tMsg.MsgType = "GatePass Punch Process Error";
                    tMsg.Message = err;
                    Scheduler.Publish(tMsg);
                    return;
                }

                bool hasrow = dsGPass.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);
                
                
                if (hasrow)
                {
                   

                    foreach (DataRow dr in dsGPass.Tables[0].Rows)
                    {
                        if (_ShutDown)
                        {
                            return;
                        }                       

                        
                        string GatePassID = dr["ID"].ToString();                        
                        string EmpUnqID = dr["EmpUnqID"].ToString();
                        string GatePassMode = dr["Mode"].ToString();
                        DateTime GPOutTime = Convert.ToDateTime(dr["GateOutDateTime"]);

                        clsEmp Empdt = new clsEmp();
                        if(Empdt.GetEmpDetails("01",EmpUnqID))
                        {
                            if (Empdt.UnitCode == "003")
                            {
                                bool sts = UpdateESSRemarks(Convert.ToInt32(GatePassID), "PRG Employee Skiped");
                                sts = UpdateESSClose(Convert.ToInt32(GatePassID));
                                continue;
                            }
                        }


                        tMsg = new ServerMsg();
                        tMsg.MsgTime = DateTime.Now;
                        tMsg.MsgType = "GatePass Punch Process";
                        tMsg.Message = GatePassID + ":" + EmpUnqID + ":Started";
                        Scheduler.Publish(tMsg);

                        
                        if (dr["AttdFlag"].ToString() == "O" &&
                            dr["AttdGPFlag"] == DBNull.Value  && 
                            (DateTime.Now - Convert.ToDateTime(dr["GateOutDateTime"])).TotalHours <= 48 )
                        {
                            
                            int range = 10;
                            while (range <= 50)
                            {
                                if (FindOutPunch(GPOutTime, EmpUnqID, GatePassMode, Convert.ToInt32(GatePassID), range))
                                {
                                    break;
                                }
                                else
                                {
                                    range += 10;
                                }
                            }
                                                      
                        }
                        else if (dr["AttdFlag"].ToString() == "O" &&
                            dr["AttdGPFlag"].ToString() == "O" &&
                            dr["GatePassStatus"].ToString() == "I" &&
                            (DateTime.Now - Convert.ToDateTime(dr["GateINDateTime"])).TotalHours <= 48 )
                        {

                            DateTime GPInTime = Convert.ToDateTime(dr["GateInDateTime"]);                           
                            int range = 10;
                            while (range <= 50)
                            {
                                if (FindInPunch(GPInTime, EmpUnqID, GatePassMode, Convert.ToInt32(GatePassID), range))
                                {
                                    break;
                                }
                                else
                                {
                                    range += 10;
                                }
                            }
                        }
                        else if (dr["AttdFlag"].ToString() == "O" &&
                                dr["AttdGPFlag"] == DBNull.Value &&
                                (DateTime.Now - Convert.ToDateTime(dr["GateOutDateTime"])).TotalHours > 48)
                        {
                            int range = 10;
                            while (range <= 50)
                            {
                                if (FindOutPunch(GPOutTime, EmpUnqID, GatePassMode, Convert.ToInt32(GatePassID), range))
                                {
                                    break;
                                }
                                else
                                {
                                    range += 10;
                                }
                            }
                            if (range >= 50)
                            {
                                UpdateESSRemarks(Convert.ToInt32(GatePassID), "Out Punch Not Found");
                                UpdateESSForceOut(Convert.ToInt32(GatePassID));
                            }
                            
                        }
                        else if (dr["AttdFlag"].ToString() == "O" &&
                               dr["AttdGPFlag"].ToString() == "O" &&
                               dr["GatePassStatus"].ToString() == "I" &&
                               (DateTime.Now - Convert.ToDateTime(dr["GateINDateTime"])).TotalHours > 48)
                        {
                            DateTime GPInTime = Convert.ToDateTime(dr["GateInDateTime"]);
                            int range = 10;
                            while (range <= 50)
                            {
                                if (FindInPunch(GPInTime, EmpUnqID, GatePassMode, Convert.ToInt32(GatePassID), range))
                                {
                                    
                                    break;
                                }
                                else
                                {
                                    range += 10;
                                }
                            }

                            if (range >= 50)
                            {
                                UpdateESSRemarks(Convert.ToInt32(GatePassID), "InPunch Not Found");
                                UpdateESSClose(Convert.ToInt32(GatePassID));
                            }
                            
                        }
                        else if (dr["AttdFlag"].ToString() == "O" &&
                               dr["AttdGPFlag"].ToString() == "O" &&
                               dr["GatePassStatus"].ToString() == "O" &&
                               (DateTime.Now - Convert.ToDateTime(dr["GateOutDateTime"])).TotalHours > 48)
                        {

                            GPOutTime = Convert.ToDateTime(dr["GateOutDateTime"]);
                            
                            if (dr["AttdGPOutTime"] == DBNull.Value)
                            {                             
                                int range = 10;
                                while (range <= 50)
                                {
                                    if (FindOutPunch(GPOutTime, EmpUnqID, GatePassMode, Convert.ToInt32(GatePassID), range))
                                    {

                                        break;
                                    }
                                    else
                                    {
                                        range += 10;
                                    }
                                }
                                if (range >= 50)
                                {
                                    UpdateESSRemarks(Convert.ToInt32(GatePassID), "Out Punch Not Found");
                                    
                                }
                            }                            
                            
                            UpdateESSClose(Convert.ToInt32(GatePassID));
                        }
                        else if (dr["AttdFlag"].ToString() == "O" &&
                               dr["AttdGPFlag"] == DBNull.Value &&
                               dr["GatePassStatus"].ToString() == "O" &&
                               (DateTime.Now - Convert.ToDateTime(dr["GateOutDateTime"])).TotalHours > 48)
                        {
                            UpdateESSRemarks(Convert.ToInt32(GatePassID), "Out Punch Not Found");
                            UpdateESSClose(Convert.ToInt32(GatePassID));
                        }
                        else if (dr["AttdFlag"].ToString() == "O" &&
                               dr["AttdGPFlag"] == DBNull.Value &&
                               dr["GatePassStatus"].ToString() == "I" &&
                               (DateTime.Now - Convert.ToDateTime(dr["GateOutDateTime"])).TotalHours > 48)
                        {
                            UpdateESSRemarks(Convert.ToInt32(GatePassID), "In Punch Not Found");
                            UpdateESSClose(Convert.ToInt32(GatePassID));
                        }

                    }//foreachloop

                    tMsg = new ServerMsg();
                    tMsg.MsgTime = DateTime.Now;
                    tMsg.MsgType = "GatePass Punch Process";
                    tMsg.Message = "Finished";
                    Scheduler.Publish(tMsg);

                }

            }

            public bool FindOutPunch(DateTime GPOutTime, string EmpUnqID, string GPMode, int GatePassID, int varianceMinute)
            {
                DateTime outtimerange_from = GPOutTime.AddMinutes(varianceMinute * -1);
                DateTime outtimerange_to = GPOutTime.AddMinutes(varianceMinute);

                DateTime outtime = GPOutTime;
                ServerMsg tMsg = new ServerMsg();

                tMsg.MsgTime = DateTime.Now;
                tMsg.MsgType = "Searching Out Punch Between";
                tMsg.Message = outtimerange_from.ToString("yyyy-MM-dd HH:mm:ss.fff") + " And " + outtimerange_to.ToString("yyyy-MM-dd HH:mm:ss.fff");
                Scheduler.Publish(tMsg);

                string sql = string.Empty;

                string tsql = "Select top 1 * From TripodLog where EmpUnqID ='" + EmpUnqID + "' " +
                            " And PunchDate BETWEEN '" + outtimerange_from.ToString("yyyy-MM-dd HH:mm:ss") + "' And '" + outtimerange_to.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                            " And IOFLG = 'O' and tYear = '" + outtime.ToString("yyyy") + "' " +
                            " And tYearMt = '" + outtime.ToString("yyyyMM") + "' Order by PunchDate Desc";

                DataSet ds = Utils.Helper.GetData(tsql, Utils.Helper.constr);
                bool hasrow = ds.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);
                if (hasrow)
                {
                    

                    DataRow row = ds.Tables[0].Rows[0];
                    string PunchDate = Convert.ToDateTime(row["PunchDate"]).ToString("yyyy-MM-dd HH:mm:ss.fff");

                    tMsg = new ServerMsg();
                    tMsg.MsgTime = DateTime.Now;
                    tMsg.MsgType = "Out Punch Found->";
                    tMsg.Message = PunchDate;
                    Scheduler.Publish(tMsg);

                    string IOFLG = row["IOFLG"].ToString();
                    string t1Date = Convert.ToDateTime(row["t1Date"]).ToString("yyyy-MM-dd");
                    string tYearmt = row["tYearMt"].ToString();
                    string tYear = row["tYear"].ToString();
                    string MachineIP = row["MachineIP"].ToString();
                    string tAddDt = Convert.ToDateTime(row["AddDt"]).ToString("yyyy-MM-dd HH:mm:ss");

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
                            tMsg.Message = GatePassID + ":" + EmpUnqID + ":" + "Out Punch Process Error->" + ex.Message;
                            Scheduler.Publish(tMsg);
                            return false;
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


                            sql = "Delete From AttdGatePassInOut Where " +
                                " EmpUnqID ='" + EmpUnqID + "' and " +
                                " PunchDate ='" + PunchDate + "' and " +
                                " IOFLG = '" + IOFLG + "' and " +
                                " MachineIP ='" + MachineIP + "' ";

                            cmd = new SqlCommand(sql, cn);
                            cmd.Transaction = tr;
                            cmd.ExecuteNonQuery();

                            sql = "Insert into AttdGatePassInOut (EmpUnqID,PunchDate,IOFLG,MachineIP,AddDt,AddId,tYear,tYearMt,t1date,LunchFlg) Values (" +
                            " '" + EmpUnqID + "','" + PunchDate + "'," +
                            " '" + IOFLG + "','" + MachineIP + "','" + tAddDt + "','TRIPOD-Conv-Server','" + tYear + "','" + tYearmt + "','" + t1Date + "',0 )";
                            cmd = new SqlCommand(sql, cn);
                            cmd.Transaction = tr;
                            cmd.ExecuteNonQuery();


                            if (GPMode != "D")
                            {
                                sql = "Delete From TripodLog Where EmpUnqID ='" + EmpUnqID + "' and PunchDate between '" + PunchDate + "' And '" + Convert.ToDateTime(row["PunchDate"]).AddMinutes(3).ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                                    " And MachineIP ='" + MachineIP + "' And IOFLG = '" + IOFLG + "' and tYear ='" + tYear + "' and tYearMt = '" + tYearmt + "'";

                                cmd = new SqlCommand(sql, cn);
                                cmd.Transaction = tr;
                                cmd.ExecuteNonQuery();
                            }                           

                            bool outsts = UpdateESSOutTime(Convert.ToInt32(GatePassID), tDate);
                            
                            if (GPMode == "D")
                            {

                                sql = "Delete from AttdGateInOut where PunchDate = '" + PunchDate + "' And EmpUnqID = '" + EmpUnqID + "' and IOFLG = 'O' ";
                                cmd = new SqlCommand(sql, cn);
                                cmd.Transaction = tr;
                                cmd.ExecuteNonQuery();

                                sql = "Insert into AttdGateInOut (EmpUnqID,PunchDate,IOFLG,MachineIP,AddDt,AddId,tYear,tYearMt,t1date,LunchFlg) Values (" +
                                " '" + EmpUnqID + "','" + PunchDate + "'," +
                                " '" + IOFLG + "','" + MachineIP + "','" + tAddDt + "','SERVER','" + tYear + "','" + tYearmt + "','" + t1Date + "',0 )";

                                cmd = new SqlCommand(sql, cn);
                                cmd.Transaction = tr;
                                cmd.ExecuteNonQuery();

                            }

                            tr.Commit();
                            

                            //process attendance
                            DateTime tempdt = new DateTime(), sFromDt = new DateTime(), sToDate = new DateTime();
                            tempdt = Convert.ToDateTime(tDate.ToString("yyyy-MM-dd"));
                            sFromDt = tempdt;
                            sToDate = tempdt.AddDays(1);

                            if (sFromDt != DateTime.MinValue && sToDate != DateTime.MinValue)
                            {
                                clsProcess pro = new clsProcess();
                                int res = 0; string errpro = string.Empty;
                                pro.LunchInOutProcess(EmpUnqID, sFromDt, sToDate, out res);
                                pro.Lunch_HalfDayPost_Process(EmpUnqID, tempdt, out res);
                                pro.AttdProcess(EmpUnqID, sFromDt, sToDate, out res, out errpro);
                            }
                            if (GPMode == "D")
                            {
                                UpdateESSClose(Convert.ToInt32(GatePassID));
                            }
                            return true;
                        }
                        catch (Exception ex)
                        {
                            tr.Rollback();

                            tMsg = new ServerMsg();
                            tMsg.MsgTime = DateTime.Now;
                            tMsg.MsgType = "GatePass Punch Process";
                            tMsg.Message = GatePassID + ":" + EmpUnqID + ":" + "Out Punch Process Error->" + ex.Message;
                            Scheduler.Publish(tMsg);
                            return false;
                        }
                    }//using localcn 

                    #endregion
                }
                else
                {
                    return false;
                }                
            }

            public bool FindInPunch(DateTime GPInTime, string EmpUnqID, string GPMode, int GatePassID, int varianceMinute)
            {
                DateTime timerange_from = GPInTime.AddMinutes(varianceMinute * -1);
                DateTime timerange_to = GPInTime.AddMinutes(varianceMinute);

                DateTime intime = GPInTime;
                ServerMsg tMsg = new ServerMsg();
                tMsg.MsgTime = DateTime.Now;
                tMsg.MsgType = "Searching In Punch Between ";
                tMsg.Message = timerange_from.ToString("yyyy-MM-dd HH:mm:ss.fff") + " And " + timerange_to.ToString("yyyy-MM-dd HH:mm:ss.fff");
                Scheduler.Publish(tMsg);

                string sql = string.Empty;

                string tsql = "Select top 1 * From TripodLog where EmpUnqID ='" + EmpUnqID + "' " +
                            " And PunchDate BETWEEN '" + timerange_from.ToString("yyyy-MM-dd HH:mm:ss") + "' And '" + timerange_to.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                            " And IOFLG = 'I' and tYear = '" + intime.ToString("yyyy") + "' " +
                            " And tYearMt = '" + intime.ToString("yyyyMM") + "' Order by PunchDate Desc";

                DataSet ds = Utils.Helper.GetData(tsql, Utils.Helper.constr);
                bool hasrow = ds.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);
                if (hasrow)
                {
                    DataRow row = ds.Tables[0].Rows[0];
                    string PunchDate = Convert.ToDateTime(row["PunchDate"]).ToString("yyyy-MM-dd HH:mm:ss.fff");

                    tMsg = new ServerMsg();
                    tMsg.MsgTime = DateTime.Now;
                    tMsg.MsgType = "In Punch Found -> ";
                    tMsg.Message = PunchDate;
                    Scheduler.Publish(tMsg);
                   

                    string IOFLG = row["IOFLG"].ToString();
                    string t1Date = Convert.ToDateTime(row["t1Date"]).ToString("yyyy-MM-dd");
                    string tYearmt = row["tYearMt"].ToString();
                    string tYear = row["tYear"].ToString();
                    string MachineIP = row["MachineIP"].ToString();
                    string tAddDt = Convert.ToDateTime(row["AddDt"]).ToString("yyyy-MM-dd HH:mm:ss");

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
                            tMsg.Message = GatePassID + ":" + EmpUnqID + ":" + "In Punch Process Error->" + ex.Message;
                            Scheduler.Publish(tMsg);
                            return false;
                        }

                        SqlTransaction tr = cn.BeginTransaction();
                        try
                        {
                            DateTime tDate = Convert.ToDateTime(PunchDate);

                            sql = "Delete From AttdLunchGate Where EmpUnqID ='" + EmpUnqID + "' and PunchDate between '" + PunchDate + "' And '" + Convert.ToDateTime(row["PunchDate"]).AddMinutes(3).ToString("yyyy-MM-dd HH:mm:ss.fff") + "' " +
                                " And MachineIP ='" + MachineIP + "' And IOFLG = '" + IOFLG + "' " +
                                " And tYear ='" + tYear + "' And tYearMt = '" + tYearmt + "' and t1date = '" + t1Date + "'";
                            SqlCommand cmd = new SqlCommand(sql, cn);
                            cmd.Transaction = tr;
                            int t = (int)cmd.ExecuteNonQuery();


                            sql = "Delete From AttdGatePassInOut Where " +
                                " EmpUnqID ='" + EmpUnqID + "' and " +
                                " PunchDate ='" + PunchDate + "' and " +
                                " IOFLG = '" + IOFLG + "' and " +
                                " MachineIP ='" + MachineIP + "' ";

                            cmd = new SqlCommand(sql, cn);
                            cmd.Transaction = tr;
                            cmd.ExecuteNonQuery();

                            sql = "Insert into AttdGatePassInOut (EmpUnqID,PunchDate,IOFLG,MachineIP,AddDt,AddId,tYear,tYearMt,t1date,LunchFlg) Values (" +
                            " '" + EmpUnqID + "','" + PunchDate + "'," +
                            " '" + IOFLG + "','" + MachineIP + "','" + tAddDt + "','TRIPOD-Conv-Server','" + tYear + "','" + tYearmt + "','" + t1Date + "',0 )";
                            cmd = new SqlCommand(sql, cn);
                            cmd.Transaction = tr;
                            cmd.ExecuteNonQuery();


                            if (GPMode != "D")
                            {
                                sql = "Delete From TripodLog Where EmpUnqID ='" + EmpUnqID + "' and PunchDate between '" + PunchDate + "' And '" + Convert.ToDateTime(row["PunchDate"]).AddMinutes(3).ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                                    " And MachineIP ='" + MachineIP + "' And IOFLG = '" + IOFLG + "' and tYear ='" + tYear + "' and tYearMt = '" + tYearmt + "'";

                                cmd = new SqlCommand(sql, cn);
                                cmd.Transaction = tr;
                                cmd.ExecuteNonQuery();
                            }

                            tr.Commit();

                            bool outsts = UpdateESSInTime(Convert.ToInt32(GatePassID), tDate);
                            outsts = UpdateESSClose(Convert.ToInt32(GatePassID));

                            //process attendance
                            DateTime tempdt = new DateTime(), sFromDt = new DateTime(), sToDate = new DateTime();
                            tempdt = Convert.ToDateTime(tDate.ToString("yyyy-MM-dd"));
                            sFromDt = tempdt;
                            sToDate = tempdt.AddDays(1);

                            if (sFromDt != DateTime.MinValue && sToDate != DateTime.MinValue)
                            {
                                clsProcess pro = new clsProcess();
                                int res = 0; string errpro = string.Empty;
                                pro.LunchInOutProcess(EmpUnqID, sFromDt, sToDate, out res);
                                pro.Lunch_HalfDayPost_Process(EmpUnqID, tempdt, out res);
                                pro.AttdProcess(EmpUnqID, sFromDt, sToDate, out res, out errpro);
                            }
                            return true;
                        }
                        catch (Exception ex)
                        {
                            tr.Rollback();

                            tMsg = new ServerMsg();
                            tMsg.MsgTime = DateTime.Now;
                            tMsg.MsgType = "GatePass Punch Process";
                            tMsg.Message = GatePassID + ":" + EmpUnqID + ":" + "in Punch Process Error->" + ex.Message;
                            Scheduler.Publish(tMsg);
                            return false;
                        }
                    }//using localcn 

                    #endregion
                }
                else
                {
                    return false;
                }
            }


            public bool UpdateESSOutTime(int id, DateTime outpunch)
            {
                string essConStr = "Server=172.16.12.14;Database=ESS;User Id=sa;Password=testomonials@123;";
                
                using (SqlConnection esscn = new SqlConnection(essConStr))
                {
                    try
                    {
                        esscn.Open();
                        using (SqlCommand cmd = new SqlCommand())
                        {
                            cmd.Connection = esscn;
                            cmd.CommandText = "Update GatePasses Set AttdUpdate = GetDate(), AttdFlag = 'O', AttdGPFlag = 'O' , AttdGPOutTime = '" + outpunch.ToString("yyyy-MM-dd HH:mm:ss") + "' where ID ='" + id.ToString() + "'";
                            cmd.ExecuteNonQuery();
                            return true;                            
                        }
                    }
                    catch (Exception ex)
                    {
                        ServerMsg tMsg = new ServerMsg();
                        tMsg.MsgTime = DateTime.Now;
                        tMsg.MsgType = "GatePass Punch Process Error ";
                        tMsg.Message = id + ":" + "Out Punch ESS Update Error->" + ex.Message;
                        Scheduler.Publish(tMsg);
                        return false;
                    }

                }//using essconnection

            }

            public bool UpdateESSInTime(int id, DateTime inpunch)
            {
                string essConStr = "Server=172.16.12.14;Database=ESS;User Id=sa;Password=testomonials@123;";

                using (SqlConnection esscn = new SqlConnection(essConStr))
                {
                    try
                    {
                        esscn.Open();
                        using (SqlCommand cmd = new SqlCommand())
                        {
                            cmd.Connection = esscn;
                            cmd.CommandText = "Update GatePasses Set AttdUpdate = GetDate(),AttdFlag = 'I', AttdGPFlag = 'I', AttdGPInTime = '" + inpunch.ToString("yyyy-MM-dd HH:mm:ss") + "' where ID ='" + id.ToString() + "'";
                            cmd.ExecuteNonQuery();

                            return true;

                        }
                    }
                    catch (Exception ex)
                    {
                        ServerMsg tMsg = new ServerMsg();
                        tMsg.MsgTime = DateTime.Now;
                        tMsg.MsgType = "GatePass Punch Process Error ";
                        tMsg.Message = id + ":" + "In Punch ESS Update Error->" + ex.Message;
                        Scheduler.Publish(tMsg);
                        return false;
                    }

                }//using essconnection

            }

            public bool UpdateESSRemarks(int id, string remarks)
            {
                string essConStr = "Server=172.16.12.14;Database=ESS;User Id=sa;Password=testomonials@123;";

                using (SqlConnection esscn = new SqlConnection(essConStr))
                {
                    try
                    {
                        esscn.Open();
                        using (SqlCommand cmd = new SqlCommand())
                        {
                            cmd.Connection = esscn;
                            cmd.CommandText = "Update GatePasses Set AttdUpdate = GetDate(), GPRemarks = '" + remarks.ToString() + "' where ID ='" + id.ToString() + "'";
                            cmd.ExecuteNonQuery();

                            return true;

                        }
                    }
                    catch (Exception ex)
                    {
                        ServerMsg tMsg = new ServerMsg();
                        tMsg.MsgTime = DateTime.Now;
                        tMsg.MsgType = "GatePass Punch Process Error ";
                        tMsg.Message = id + ":" + "Remarks ESS Update Error->" + ex.Message;
                        Scheduler.Publish(tMsg);
                        return false;
                    }

                }//using essconnection

            }

            public bool UpdateESSClose(int id)
            {
                string essConStr = "Server=172.16.12.14;Database=ESS;User Id=sa;Password=testomonials@123;";
                ServerMsg tMsg = new ServerMsg();
                using (SqlConnection esscn = new SqlConnection(essConStr))
                {
                    try
                    {
                        esscn.Open();
                        using (SqlCommand cmd = new SqlCommand())
                        {
                            cmd.Connection = esscn;
                            cmd.CommandText = "Update GatePasses Set AttdUpdate = GetDate(), AttdFlag ='C', AttdGPFlag = 'C' where ID ='" + id.ToString() + "'";
                            cmd.ExecuteNonQuery();
                            tMsg.MsgTime = DateTime.Now;
                            tMsg.MsgType = "Closing GatePass :" + id.ToString();
                            
                            Scheduler.Publish(tMsg);   
                            return true;

                        }
                    }
                    catch (Exception ex)
                    {
                        
                        tMsg.MsgTime = DateTime.Now;
                        tMsg.MsgType = "GatePass Punch Process Error ";
                        tMsg.Message = id + ":" + "Closing ESS Update Error->" + ex.Message;
                        Scheduler.Publish(tMsg);   
                        return false;
                    }

                }//using essconnection
            }

            public bool UpdateESSForceOut(int id)
            {
                string essConStr = "Server=172.16.12.14;Database=ESS;User Id=sa;Password=testomonials@123;";

                using (SqlConnection esscn = new SqlConnection(essConStr))
                {
                    try
                    {
                        esscn.Open();
                        using (SqlCommand cmd = new SqlCommand())
                        {
                            cmd.Connection = esscn;
                            cmd.CommandText = "Update GatePasses Set AttdUpdate = GetDate(), AttdFlag ='O', GPRemarks = GPRemarks + ' Tripod Out Punch Not Found' ,AttdGPFlag = 'O' where ID ='" + id.ToString() + "'";
                            cmd.ExecuteNonQuery();
                            return true;

                        }
                    }
                    catch (Exception ex)
                    {
                        ServerMsg tMsg = new ServerMsg();
                        tMsg.MsgTime = DateTime.Now;
                        tMsg.MsgType = "GatePass Punch Process Error ";
                        tMsg.Message = id + ":" + "Closing ESS Update Error->" + ex.Message;
                        Scheduler.Publish(tMsg);
                        return false;
                    }

                }//using essconnection
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
                                file.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "-AutoTimeSet-[" + ip + "]-(" + dr["MachineDesc"].ToString() + ")-" + err);
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
                                file.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "-AutoTimeSet-[" + ip + "]-(" + dr["MachineDesc"].ToString() + ")-" + err);
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
                            file.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "-AutoTimeSet-[" + ip + "]-(" + dr["MachineDesc"].ToString() + ")-Completed");
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
            public void UpdateProcessStatus(int jobID)
            {
                using (SqlConnection cn = new SqlConnection(Utils.Helper.constr))
                {
                    try
                    {
                        cn.Open();
                        using (SqlCommand cmd = new SqlCommand())
                        {
                            cmd.Connection = cn;
                            string upsql = "Update AttdWorker set doneflg = 1 , pushflg = 1,workerid ='Server' where msgid = '" + jobID.ToString() + "'";
                            cmd.CommandText = upsql;
                            cmd.ExecuteNonQuery();
                        }
                    }
                    catch
                    {

                    }

                }
            }


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
                            {
                                pro.AttdProcess(tEmpUnqID, tFromDt, tToDt, out tres, out err);
                                UpdateProcessStatus(Convert.ToInt32(MsgID));
                            }
                            else if (ProType == "LUNCHINOUT")
                            {
                                pro.LunchInOutProcess(tEmpUnqID, tFromDt, tToDt, out tres);
                                UpdateProcessStatus(Convert.ToInt32(MsgID));
                            }
                            else if (ProType == "GATEINOUT")
                            {
                                pro.GateInOutProcess(tEmpUnqID, tFromDt, out tres);
                                pro.AttdProcess(tEmpUnqID, tFromDt, tToDt, out tres, out err);
                                UpdateProcessStatus(Convert.ToInt32(MsgID));
                            }   
                            else if (ProType == "MESS")
                            {
                                pro.LunchProcess(tEmpUnqID, tFromDt, tToDt, out tres);
                                UpdateProcessStatus(Convert.ToInt32(MsgID));
                            }
                            else if (ProType == "EMPCOSTCODERPT")
                            {
                                pro.EmpCostCodeRpt_Process(tEmpUnqID, tFromDt, out tres, out err);
                                UpdateProcessStatus(Convert.ToInt32(MsgID));
                            }
                            else if (ProType == "TRIPODDATA")
                            {
                                pro.TripodDataProcess(tEmpUnqID, tFromDt, tToDt, out tres);
                                UpdateProcessStatus(Convert.ToInt32(MsgID));
                            }
                            else
                                pro.AttdProcess(tEmpUnqID, tFromDt, tToDt, out tres, out err);
                                UpdateProcessStatus(Convert.ToInt32(MsgID));
                                if (!string.IsNullOrEmpty(err))
                                {


                                    string filenm = "AutoProcess_Error_" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
                                    string fullpath = Path.Combine(Errfilepath, filenm);
                                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(fullpath, true))
                                    {
                                        file.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "-AutoProcess-[" + tEmpUnqID + "]-" + err);
                                    }

                                    tMsg.MsgTime = DateTime.Now;
                                    tMsg.MsgType = "Auto Process->" + ProType;
                                    tMsg.Message = tEmpUnqID + ": Error=>" + err;
                                    Scheduler.Publish(tMsg);
                                }
                                
                        }

                        _StatusWorker = false;

                    }
                    else
                    {
                        //check if any pending machine operation if yes do it....
                        #region newmachinejob
                        sql = "Select top 100 * from MastMachineUserOperation where DoneFlg = 0 and Operation not in ('BLOCK','UNBLOCK') " +
                            " And MachineIP not in (Select MachineIP From TRIPODReaderConFig) order by MachineIP ";
                        DataSet ds = Utils.Helper.GetData(sql, Utils.Helper.constr);
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

                                //check once again if validity extended or not if yes then skip
                                if (dr["Remarks"].ToString().Contains("Validity Expired") && dr["Operation"].ToString() == "DELETE")
                                {
                                    sql = "Select EmpUnqID,Active,WrkGrp,ValidFrom, ValidTo from MastEmp where EmpUnqID = '" + dr["EmpUnqID"].ToString() + "'";
                                    DataSet tds = Utils.Helper.GetData(sql, Utils.Helper.constr);
                                    hasRows = tds.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);
                                    if (hasRows)
                                    {
                                        DataRow tdr = tds.Tables[0].Rows[0];
                                        if(Convert.ToDateTime(tdr["ValidTo"]) > Convert.ToDateTime(dr["ValidTo"]))
                                        {
                                            //delete from MastMachineUserOperation where EmpUnqID
                                            using (SqlConnection cn = new SqlConnection(Utils.Helper.constr))
                                            {
                                                try
                                                {
                                                    cn.Open();
                                                    using (SqlCommand cmd = new SqlCommand())
                                                    {
                                                        cmd.Connection = cn;
                                                        sql = "Delete From MastMachineUserOperation Where DoneFlg = 0 and EmpUnqID = '" + dr["EmpUnqID"].ToString() + "'";
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

                                                continue;
                                            }//using
                                        }
                                        else
                                        {
                                            
                                            DateTime CurDate = DateTime.Now;
                                            DateTime MastValidDt = Convert.ToDateTime(tdr["ValidTo"]);
                                            bool tActive = Convert.ToBoolean(tdr["Active"]);

                                            
                                            //Auto deActivate Employee
                                            if ((CurDate - MastValidDt).Days > 30 && tActive)
                                            {
                                                sql = "Update MastEmp Set Active = 0, LeftDate = '" + MastValidDt.ToString("yyyy-MM-dd") + "' " +
                                                    " UpdDt = GetDate(), UpdID ='SERVER' " +
                                                    " Where EmpUnqID ='" + tdr["EmpUnqID"].ToString() + "';";


                                                using (SqlConnection cn = new SqlConnection(Utils.Helper.constr))
                                                {
                                                    try
                                                    {
                                                        cn.Open();
                                                        using (SqlCommand cmd = new SqlCommand())
                                                        {
                                                            cmd.Connection = cn;
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

                                            }
                                        }
                                    }
                                    
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
                                                    sql = "Update MastMachineUserOperation Set DoneFlg = 1, DoneDt = GetDate(), LastError = 'Completed' , UpdID = 'ATTDServer', " +
                                                        " UpdDt=GetDate() where ID ='" + dr["ID"].ToString() + "' and MachineIP = '" + dr["MachineIP"].ToString() + "' and Operation = '" + dr["Operation"].ToString() + "' and EmpUnqID ='" +  dr["EmpUnqID"].ToString() + "';";
                                                }
                                                else
                                                {
                                                    if (err.Contains("Block"))
                                                    {
                                                        sql = "Update MastMachineUserOperation Set UpdDt=GetDate(), LastError = '" + err + "', DoneFlg = 1, DoneDt = GetDate(), UpdID = 'ATTDServer' " +
                                                       " where ID ='" + dr["ID"].ToString() + "' and MachineIP = '" + dr["MachineIP"].ToString() + "' and Operation = '" + dr["Operation"].ToString() + "' and EmpUnqID ='" + dr["EmpUnqID"].ToString() + "';";
                                                    }
                                                    else
                                                    {
                                                        sql = "Update MastMachineUserOperation Set UpdDt=GetDate(), LastError = '" + err + "',UpdID = 'ATTDServer' " +
                                                        " where ID ='" + dr["ID"].ToString() + "' and MachineIP = '" + dr["MachineIP"].ToString() + "' and Operation = '" + dr["Operation"].ToString() + "' and EmpUnqID ='" + dr["EmpUnqID"].ToString() + "';";
                                                    }
                                                    
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
            public void Execute(IJobExecutionContext context)
            {
                                               
                string cnerr = string.Empty;
                string sql = "Select distinct MachineIP from MastMachineUserOperation where DoneFlg = 0 and Operation = 'BLOCK'";
                DataSet dsMachine = Utils.Helper.GetData(sql, Utils.Helper.constr);
                bool hasRows = dsMachine.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);

                if (hasRows)
                {
                    bool connected = false;

                    foreach (DataRow MainRow in dsMachine.Tables[0].Rows)
                    {
                        connected = false;
                        string machineip = MainRow["MachineIP"].ToString();
                        zkemkeeper.CZKEM tmpMachine = new zkemkeeper.CZKEM();    
                        //check if any pending machine operation if yes do it....
                        #region newmachinejob
                        sql = "Select  * from MastMachineUserOperation where DoneFlg = 0 and Operation ='BLOCK' And MachineIP ='" + machineip + "'";
                        DataSet ds = Utils.Helper.GetData(sql, Utils.Helper.constr);
                        hasRows = ds.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);
                        if (hasRows)
                        {
                            connected = false;
                            connected = tmpMachine.Connect_Net(machineip, 4370);                            
                            if (connected)
                            {
                                bool Deleted = false;
                                bool isTft = tmpMachine.IsTFTMachine(1);
                                
                                foreach (DataRow dr in ds.Tables[0].Rows)
                                {
                                    string emp = dr["EmpUnqID"].ToString();
                                    int id = Convert.ToInt32(dr["ID"]);

                                    //check if empblock status
                                    // if current status is unblocked skip those records.
                                    string err2 = string.Empty;
                                    string empblockstat = Utils.Helper.GetDescription("Select PunchingBlocked from MastEmp Where EmpUnqID='" + emp + "'",Utils.Helper.constr,out err2);

                                    if (!Convert.ToBoolean(empblockstat))
                                    {
                                        using (SqlConnection cn = new SqlConnection(Utils.Helper.constr))
                                        {
                                            try
                                            {
                                                cn.Open();
                                                using (SqlCommand cmd = new SqlCommand())
                                                {
                                                    sql = "Update MastMachineUserOperation Set DoneFlg = 1, DoneDt = GetDate(), LastError = 'already Unblocked before actual block from machine..' , " +
                                                             " UpdDt=GetDate() where ID ='" + id.ToString() + "' and MachineIP = '" + machineip.ToString() + "' and Operation = 'BLOCK' and EmpUnqID ='" + emp.ToString() + "';";

                                                    cmd.Connection = cn;
                                                    cmd.CommandText = sql;
                                                    cmd.ExecuteNonQuery();
                                                }
                                            }
                                            catch (Exception ex)
                                            {

                                            }
                                        }
                                        continue;
                                    }

                                    ServerMsg tMsg = new ServerMsg();
                                    tMsg.MsgTime = DateTime.Now;
                                    tMsg.MsgType = "Machine Operation->";
                                    tMsg.Message = "Performing : " + "BLOCK" + " : EmpUnqID=>" + emp.ToString() + "->" + machineip;
                                    Scheduler.Publish(tMsg);

                                    string err = string.Empty;

                                    if (!isTft)
                                    {
                                        Deleted = tmpMachine.DeleteEnrollData(1, Convert.ToInt32(emp), 1, 0);                                        
                                        Deleted = true;
                                    }
                                    else
                                    {                                        
                                        Deleted = tmpMachine.SSR_DeleteEnrollDataExt(1, emp, 12);
                                        tmpMachine.DelUserFace(1, emp, 50);                                        
                                        
                                    }

                                    if (Deleted)
                                    {
                                        using (SqlConnection cn = new SqlConnection(Utils.Helper.constr))
                                        {
                                            try
                                            {
                                                cn.Open();
                                                using (SqlCommand cmd = new SqlCommand())
                                                {
                                                    
                                                    sql = "Update MastMachineUserOperation Set DoneFlg = 1, DoneDt = GetDate(), LastError = 'Completed' , " +
                                                            " UpdDt=GetDate() where ID ='" + id.ToString() + "' and MachineIP = '" + machineip.ToString() + "' and Operation = 'BLOCK' and EmpUnqID ='" + emp.ToString() + "';";
                                                   
                                                    cmd.Connection = cn;
                                                    cmd.CommandText = sql;
                                                    cmd.ExecuteNonQuery();
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                tMsg.MsgTime = DateTime.Now;
                                                tMsg.MsgType = "Machine Operation->";
                                                tMsg.Message = "Error : " + "BLOCK" + " : EmpUnqID=>" + emp.ToString() + "->" + dr["MachineIP"].ToString() + "->" + ex.Message.ToString();
                                                Scheduler.Publish(tMsg);

                                            }

                                        }//using
                                    }
                                    
                                }//foreach
                                tmpMachine.RefreshData(1);
                                tmpMachine.Disconnect();
                            }//if connected

                        }//for each machine's
                        
                        #endregion

                        tmpMachine = null;
                        
                    }

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
                
                //
                string sql = "Select EmpUnqID,ValidTo from MastEmp where Active = 1 and ValidityExpired = 0 and DATEDIFF(day,ValidTo,GetDate()) > 1 and WrkGrp <> 'COMP' AND COMPCODE = '01'";
                    string cnerr = string.Empty;

                    DataSet dsEmp = Utils.Helper.GetData(sql, Utils.Helper.constr, out cnerr);
                    if (!string.IsNullOrEmpty(cnerr))
                    {

                        string filenm2 = "AutoDeleteExpEmp_" + DateTime.Now.ToString("yyyyMMdd_HHmm") + ".txt";
                        string fullpath = Path.Combine(Errfilepath, filenm2);
                        using (System.IO.StreamWriter file = new System.IO.StreamWriter(fullpath, true))
                        {
                            file.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "-Auto Delete Validity Expired Employee->" + cnerr);
                        }


                        return;
                    }

                    bool Emphasrows = dsEmp.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);
                    string filenm = "AutoDeleteExpEmp_" + DateTime.Now.ToString("yyyyMMdd_HHmm") + ".txt";
                    
                    if (Emphasrows)
                    {

                        foreach (DataRow dr in dsEmp.Tables[0].Rows)
                        {
                            if (_ShutDown)
                            {
                                return;
                            }


                            string tsql = "select MachineIP,IOFLG from ReaderConFig where canteenflg = 0  and [master] = 0 and compcode = '01' and MachineIP in " +
                                "( " +
                                "select Distinct MachineIP from AttdLog where EmpUnqID = '" + dr["EmpUnqID"].ToString() + "' and Punchdate between DATEADD(day,-60,getdate()) and DateAdd(day,1,GETDATE()) " +
                                ")" +
                                " Union Select MachineIP,IOFLG From TripodReaderConfig  ";

                            string err = string.Empty;
                            DataSet empIPds = Utils.Helper.GetData(tsql, Utils.Helper.constr, out err);

                            
                            if (!string.IsNullOrEmpty(err))
                            {
                                string filenm2 = "AutoDeleteExpEmp_" + DateTime.Now.ToString("yyyyMMdd_HHmm") + ".txt";
                                string fullpath = Path.Combine(Errfilepath, filenm2);
                                using (System.IO.StreamWriter file = new System.IO.StreamWriter(fullpath, true))
                                {
                                    file.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "-Auto Delete Validity Expired Employee->" + err);
                                }
                                return;
                            }


                            bool hasrow = empIPds.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);
                            if (hasrow)
                            {
                                err = string.Empty;
                                string maxid = Utils.Helper.GetDescription("Select isnull(Max(ID),0) + 1 from MastMachineUserOperation", Utils.Helper.constr, out err);
                                if (!string.IsNullOrEmpty(err))
                                {
                                    string filenm2 = "AutoDeleteExpEmp_" + DateTime.Now.ToString("yyyyMMdd_HHmm") + ".txt";
                                    string fullpath = Path.Combine(Errfilepath, filenm2);
                                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(fullpath, true))
                                    {
                                        file.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "-Auto Delete Validity Expired Employee->" + err);
                                    }
                                    return;
                                }
                                
                                foreach (DataRow tdr in empIPds.Tables[0].Rows)
                                {
                                    ServerMsg tMsg = new ServerMsg();
                                    tMsg.MsgTime = DateTime.Now;
                                    tMsg.MsgType = "Auto Delete Validity Expired Employee";
                                    tMsg.Message = tdr["MachineIP"].ToString() + "->" + dr["EmpUnqID"].ToString();
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

                                                tsql = "Update  a set a.ValidTo = b.ValidTo " +
                                                    "  from MastMachineUserOperation a ,MastEmp b  " +
                                                    " where a.EmpUnqID = b.EmpUnqID and Remarks  = 'Validity Expired' and Doneflg = 0 " +
                                                    " And a.EmpUnqID='" + dr["EmpUnqID"].ToString() + "' And MachineIP ='" + tdr["MachineIP"].ToString() + "'";
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

                                   

                                }//foreach employee ip

                            } //if employees punching ip's found

                        } //foreach employee

                        

                    }// if employee found

                

                
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
