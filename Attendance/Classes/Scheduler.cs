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
                scheduler.ListenerManager.AddJobListener(new DummyJobListener(), GroupMatcher<JobKey>.AnyGroup());
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
           scheduler = StdSchedulerFactory.GetDefaultScheduler();
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
                        .WithIdentity(jobid, "AutoDownload")
                        .Build();

                    // Trigger the job to run now, and then repeat every 10 seconds
                    ITrigger trigger = TriggerBuilder.Create()
                        .WithIdentity(triggerid, "AutoDownload")
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
                        .WithIdentity(jobid, "AutoTimeSet")
                        .Build();

                    // Trigger the job to run now, and then repeat every 10 seconds
                    ITrigger trigger = TriggerBuilder.Create()
                        .WithIdentity(triggerid, "AutoTimeSet")                        
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
                string triggerid = "T_AutoProcess_" + wrk.Replace("'", "");
                
                // define the job and tie it to our HelloJob class
                IJobDetail job = JobBuilder.Create<AutoTimeSet>()
                    .WithIdentity(jobid, "AutoProcess")
                    .Build();

                

                // Trigger the job to run now, and then repeat every 10 seconds
                ITrigger trigger = TriggerBuilder.Create()
                    .WithIdentity(triggerid, "AutoProcess")
                    .StartNow()
                    .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(tTime.Hours, tTime.Minutes  ))
                    .UsingJobData("WrkGrp", wrk.Replace("'", ""))
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

        public void RegSchedule_WorkerProcess()
        {
            string jobid = "Job_WorkerProcess";
            string triggerid = "Trigger_WorkerProcess" 
                ;
            // define the job and tie it to our HelloJob class
            IJobDetail job = JobBuilder.Create<WorkerProcess>()
                .WithIdentity(jobid, "WorkerProcess")
                .Build();

            // Trigger the job to run every 3 minute
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity(triggerid, "WorkerProcess")
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
                     
                    
                    string jobid = "Job_Arrival_" + tTime.Hours.ToString() + tTime.Minutes.ToString();
                    string triggerid = "Trigger_Arrival_" + tTime.Hours.ToString() + tTime.Minutes.ToString();
                    // define the job and tie it to our HelloJob class
                    IJobDetail job = JobBuilder.Create<AutoArrival>()
                        .WithIdentity(jobid, "Arrival")
                        .Build();

                    // Trigger the job to run now, and then repeat every 10 seconds
                    ITrigger trigger = TriggerBuilder.Create()
                        .WithIdentity(triggerid, "Arrival")
                        .StartNow()
                        .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(tTime.Hours, tTime.Minutes))
                        .UsingJobData("FromTime",FromTime.ToString())
                        .UsingJobData("ToTime", ToTime.ToString())                        
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
                                file.WriteLine(DateTime.Now.ToString("yyyyy-MM-dd HH:mm:ss") + "-AutoDownload-[" + ip + "]-Completed");
                            }

                            m.DisConnect(out err);
                        }
                        catch (Exception ex)
                        {
                            string filenm = "AutoErrLog_" + DateTime.Now.ToString("yyyyMMdd_HHmm") + ".txt";
                            string fullpath = Path.Combine(Errfilepath, filenm);
                            using (System.IO.StreamWriter file = new System.IO.StreamWriter(fullpath, true))
                            {
                                file.WriteLine(DateTime.Now.ToString("yyyyy-MM-dd HH:mm:ss") + "-AutoDownload-[" + ip + "]-" + ex.ToString());
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
                _StatusAutoProcess = true;
                JobKey key = context.JobDetail.Key;

                JobDataMap dataMap = context.JobDetail.JobDataMap;

                string tWrkGrp = dataMap.GetString("WrkGrp");
                
                
                string tsql = "Select EmpUnqID from MastEmp where CompCode = '01' and WrkGrp = '" + tWrkGrp + "' And Active = 1";
                DateTime ToDt = Globals.GetSystemDateTime();
                
                if(ToDt == DateTime.MinValue){
                    return;
                }
                DateTime FromDt = ToDt.AddDays(-1);

                DataSet DsEmp = Utils.Helper.GetData(tsql, Utils.Helper.constr);
                bool hasRows = DsEmp.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);
                if (hasRows)
                {
                    clsProcess pro = new clsProcess();
                    
                    foreach (DataRow dr in DsEmp.Tables[0].Rows)
                    {
                        if (_ShutDown)
                        {
                            return;
                        }
                        
                        
                        string tEmpUnqID = dr["EmpUnqID"].ToString();
                        
                        ServerMsg tMsg = new ServerMsg();
                        tMsg.MsgTime = DateTime.Now;
                        tMsg.MsgType = "Auto Process";
                        tMsg.Message = tEmpUnqID;
                        Scheduler.Publish(tMsg);
                        
                        string err = string.Empty;
                        int tres = 0;
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

                    string filenminfo = "AutoProcess_Info_" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
                    string fullpath2 = Path.Combine(Loginfopath, filenminfo);
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(fullpath2, true))
                    {
                        file.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "-AutoProcess-Completed-" + tWrkGrp);
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

                
                if (_StatusAutoArrival == false && 
                    _StatusAutoDownload == false && 
                    _StatusAutoProcess == false && 
                    _StatusAutoTimeSet == false && 
                    _StatusWorker == false)
                {

                    

                    
                    string sql = "Select top 100 w.* from attdworker w where w.doneflg = 0 Order by MsgId desc" ;
                    DataSet DsEmp = Utils.Helper.GetData(sql, Utils.Helper.constr);

                    bool hasRows = DsEmp.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);
                    if (hasRows)
                    {

                       
                        
                        
                        clsProcess pro = new clsProcess();

                        foreach (DataRow dr in DsEmp.Tables[0].Rows)
                        {

                            if (_ShutDown)
                            {
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
