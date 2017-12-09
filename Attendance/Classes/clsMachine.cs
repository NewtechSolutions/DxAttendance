using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zkemkeeper;
using Attendance.Classes;
using System.IO;
using System.Data.SqlClient;
using System.Data;

namespace Attendance.Classes
{
    class clsMachine
    {
        private string _ip,_machinedesc , _tableName,_location;
        private bool _connected;
      
        private int _port;
        private int _machineno,_LastErrCode,_AttdLogCount;
        private zkemkeeper.CZKEM CZKEM1;
        private string _ioflg;

        private bool _messflg,_autoclear,_lunchinout,_gateinout;


        private bool GetMachineInfoFromDb()
        {
            bool ret = false;

            DataSet ds = Utils.Helper.GetData("Select * from ReaderConfig Where MachineIP ='" + _ip + "'", Utils.Helper.constr);
            bool hasrows = ds.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);
            if (hasrows)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    _machinedesc = dr["MachineDesc"].ToString();
                    _machineno = Convert.ToInt32(dr["MachineNo"]);
                    _ioflg = dr["IOFLG"].ToString().Substring(0, 1);
                    _autoclear = Convert.ToBoolean(dr["AutoClear"]);
                    _messflg = Convert.ToBoolean(dr["CanteenFlg"]);
                    _lunchinout = Convert.ToBoolean(dr["LunchInOut"]);
                    _gateinout = Convert.ToBoolean(dr["GateInOut"]);
                    _location = dr["Location"].ToString();

                    if (_lunchinout)
                        _tableName = "AttdLunchGate";
                    else if (_messflg)
                        _tableName = "AttdLog";
                    else if (_gateinout)
                        _tableName = "AttdGateInOut";
                    else if (_machinedesc.Contains("water"))
                        _tableName = "AttdWater";
                    else
                        _tableName = "AttdLog";

                    ret = true;
                }
            }
            else
            {
                ret = false;
                _machinedesc = "Not Found...";
                _machineno = 1;
                
                _messflg = false;
                _autoclear = false;
                _lunchinout = false;
                _gateinout = false;
                _location = "Not Found...";
                
                _tableName = "AttdLog";
            }

            return ret;
        } 

        public clsMachine(string IPAddress,string ioflg)
        {
            _ip = IPAddress;
            _ioflg = ioflg;

            _connected = false;
            _port = 4370;
            _LastErrCode = 0;
            
            CZKEM1 = new zkemkeeper.CZKEM();
        }

        public int GetLastErr { get { return _LastErrCode; } }
        
        public void Connect(out string err)
        {
            err = string.Empty;
            _LastErrCode = 0;

            if(string.IsNullOrEmpty(_ip))
            {
                err = "IP Address is required..";
                return;
            }

            if (_ioflg == string.Empty)
            {
                err = "I/O Flg need to set before connect..";
                return;
            }

           

            if (!"I|O|B".Contains(_ioflg))
            {
                err = "Invalid I/O Flg required(I,O,B)";
                return;
            }
            
            this.GetMachineInfoFromDb();

            _connected = CZKEM1.Connect_Net(_ip, _port);
        }

        public void DisConnect (out string err)
        {
            err = string.Empty;
            

            if(string.IsNullOrEmpty(_ip))
            {
                err = "IP Address is required..";
                return;
            }
            try
            {
                CZKEM1.Disconnect();
                _connected = false;
                return;
            }catch(Exception ex ){

                CZKEM1.GetLastError(ref _LastErrCode);
                err = "Operation failed,ErrorCode=" + _LastErrCode.ToString() + Environment.NewLine + ex.ToString();
                _connected = true;
            }
            
        }

        public void GetAttdCnt(out int count, out string err)
        {
            count = 0;
            err = string.Empty;

            if(!_connected)
            {
                err = "Machine not connected..";
                return;
            }

            CZKEM1.EnableDevice(_machineno, false);//disable the device
            if (CZKEM1.GetDeviceStatus(_machineno, 6, ref _AttdLogCount)) //Here we use the function "GetDeviceStatus" to get the record's count.The parameter "Status" is 6.
            {
                count = _AttdLogCount;
            }
            else
            {
               CZKEM1.GetLastError(ref _LastErrCode);
                err = "Operation failed,ErrorCode=" + _LastErrCode.ToString();
            }
            CZKEM1.EnableDevice(_machineno, true);//enable the device
        }

        public void GetAttdRec(out List<AttdLog> AttdLogRec,out string err)
        {
            AttdLogRec = new List<AttdLog>();


            err = string.Empty;
            if (!_connected)
            {
                err = "Machine not connected..";
                return;
            }


            int idwYear = 0;
            int idwMonth = 0;
            int idwDay = 0;
            int idwHour = 0;
            int idwMinute = 0;
            int idwSecond = 0;
            int idwWorkcode = 0;
            string sdwEnrollNumber = "";
            int odwEnrollNumber = 0; //for old machine
            int idwVerifyMode = 0;
            int idwInOutMode = 0;
            
            int idwReserved = 0;
            bool vRet = false;
            bool m_tft = false;

            //count records
            int cnt = 0; string outerr = string.Empty;
            this.GetAttdCnt(out cnt, out outerr);

            if (cnt == 0)
            {
                err = outerr + err;
                return;
            }
            
            m_tft = CZKEM1.IsTFTMachine(_machineno);
            
            //'Prepare File Name for writing log data
            CZKEM1.GetDeviceTime(_machineno, ref idwYear, ref idwMonth, ref idwDay, ref idwHour, ref idwMinute, ref idwSecond);
            string filepath = Utils.Helper.GetLogFilePath();
            string filenm = "AttdLog_" + idwDay.ToString() + "_" + idwMonth.ToString() + "_" + idwYear.ToString() + "_" + idwHour.ToString() + "_" + idwMinute.ToString() + ".txt";
            string fullpath = Path.Combine(filepath, filenm);

            

            CZKEM1.EnableDevice(_machineno, false);//disable the device
            if (CZKEM1.ReadGeneralLogData(_machineno))//read all the attendance records to the memory
            {
                if (m_tft)
                {
                    while (CZKEM1.SSR_GetGeneralLogData(_machineno, out sdwEnrollNumber, out idwVerifyMode,
                            out idwInOutMode, out idwYear, out idwMonth, out idwDay, out idwHour, out idwMinute, out idwSecond, ref idwWorkcode))//get records from the memory
                    {

                        AttdLog t = new AttdLog();
                        DateTime logdt = new DateTime(idwYear,idwMonth,idwDay,idwHour,idwMinute,idwSecond);
                        

                        t.EmpUnqID = sdwEnrollNumber.ToString();
                        t.PunchDate = logdt;
                        t.MachineIP = _ip;
                        t.IOFLG = _ioflg;
                        t.LunchFlg = _messflg;
                        t.t1Date = new DateTime(idwYear,idwMonth,idwDay);
                        t.tYear = t.t1Date.Year;
                        t.tYearMt = Convert.ToInt32(t.t1Date.Year.ToString() + t.t1Date.Month.ToString("00"));
                        t.AddID = Utils.User.GUserID;
                        t.AddDt = DateTime.Now;
                        t.TableName = _tableName;
                        AttdLogRec.Add(t);
                        
                    }
                }
                else
                {
                    while (CZKEM1.GetGeneralExtLogData(_machineno,ref odwEnrollNumber,ref idwVerifyMode,
                            ref idwInOutMode,ref idwYear,ref idwMonth,ref idwDay,ref idwHour,ref idwMinute,ref idwSecond,ref idwWorkcode,ref idwReserved ))//get records from the memory
                    {
                        AttdLog t = new AttdLog();
                        DateTime logdt = new DateTime(idwYear, idwMonth, idwDay, idwHour, idwMinute, idwSecond);


                        t.EmpUnqID = sdwEnrollNumber.ToString();
                        t.PunchDate = logdt;
                        t.MachineIP = _ip;
                        t.IOFLG = _ioflg;
                        t.LunchFlg = _messflg;
                        t.t1Date = new DateTime(idwYear, idwMonth, idwDay);
                        t.tYear = t.t1Date.Year;
                        t.tYearMt = Convert.ToInt32(t.t1Date.Year.ToString() + t.t1Date.Month.ToString("00"));
                        t.AddID = Utils.User.GUserID;
                        t.AddDt = DateTime.Now;
                        t.TableName = _tableName;
                        AttdLogRec.Add(t);                        
                    }
                }
                
                
            }
            else
            {
                
                CZKEM1.GetLastError(ref _LastErrCode);

                if (_LastErrCode != 0)
                {
                    err =  "Reading data from terminal failed,ErrorCode: " +_LastErrCode.ToString();
                }
                else
                {
                    err = "No Records Found...";
                }
                
            }

            CZKEM1.EnableDevice(_machineno, true);//enable the device

            //write text file and also store in db
            foreach (AttdLog t in AttdLogRec)
            {
                string dberr = StoreToDb(t);
                if (!string.IsNullOrEmpty(dberr))
                {
                    t.Error = dberr;

                    err += t.EmpUnqID + " : " + dberr + Environment.NewLine;
                }

                using (System.IO.StreamWriter file = new System.IO.StreamWriter(fullpath, true))
                {
                    file.WriteLine(t.ToString());
                }
            }    


        }

        public string StoreToDb(AttdLog t)
        {
            string err = string.Empty;

            using (SqlConnection cn = new SqlConnection(Utils.Helper.constr))
            {
                try
                {
                    cn.Open();
                    string sql = t.GetDBWriteString();

                    using (SqlCommand cmd = new SqlCommand(sql, cn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    err = ex.ToString();

                    try
                    {
                        string sql = t.GetDBWriteErrString();

                        using (SqlCommand cmd = new SqlCommand(sql, cn))
                        {
                            cmd.ExecuteNonQuery();
                            err = "Duplicate Data Found..";
                        }

                    }catch(Exception ex1)
                    {
                        err = ex1.ToString();
                    }
                    
                }
            }

            return err;
        }

        public void ClearAttdLog(out string err)
        {
            err = string.Empty;
            if (!_connected)
            {
                err = "Machine not connected..";
                return;
            }

            CZKEM1.EnableDevice(_machineno, false);//disable the device
            
            //before clearing machine make sure to  download the data
            


            if (CZKEM1.ClearGLog(_machineno))
            {
                CZKEM1.RefreshData(_machineno);//the data in the device should be refreshed
            }
            else
            {
                CZKEM1.GetLastError(ref _LastErrCode);
                err = "Operation failed,ErrorCode=" + _LastErrCode.ToString();
            }

            CZKEM1.EnableDevice(_machineno, true);//enable the device
        }

        public void SetTime(out string err)
        {
            this.CZKEM1.EnableDevice(_machineno,false);

            err = (this.CZKEM1.SetDeviceTime(_machineno) ? "" : "Unable to Set Time...");

            this.CZKEM1.EnableDevice(_machineno, true);
        }
    }
}
