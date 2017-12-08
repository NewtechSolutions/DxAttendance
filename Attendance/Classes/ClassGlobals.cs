using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace Attendance.Classes
{
    
    
    class Globals
    {
        
        public static List<string> GateInOutIP = new List<string>();
        public static string G_GateInOutIP;

        public static List<string> LunchInOutIP = new List<string>();
        public static string G_LunchInOutIP;

        public static string G_AutoProcessWrkGrp;
        public static TimeSpan G_AutoProcessTime;
        public static string G_ReportServiceURL;
        public static string G_ReportSerExeUrl;
        public static string G_DefaultMailID;
        public static string G_SmtpHostIP;
        public static string G_ServerWorkerIP;

        public static string G_NetworkDomain;
        public static string G_NetworkUser;
        public static string G_NetworkPass;
        public static List<string> G_SchAutoTimeSet;

        public static bool GetGlobalVars()
        {
            bool tset = false;
            
            DataSet ds = new DataSet();
            string sql = "Select top 1 * From MastNetwork ";
            ds = Utils.Helper.GetData(sql, Utils.Helper.constr);
            bool hasRows = ds.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);

            if (hasRows)
            {
                tset = true;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if(dr["AutoProcessWrkGrp"].ToString() != string.Empty)
                    {
                        G_AutoProcessWrkGrp = dr["AutoProcessWrkGrp"].ToString().Replace(",", "','");
                        G_AutoProcessWrkGrp = "'" + G_AutoProcessWrkGrp + "'";
                    }
                    TimeSpan t = new TimeSpan();
                    if (dr["AutoProcessTime"] != DBNull.Value)
                    {
                            TimeSpan.TryParse(dr["AutoProcessTime"].ToString(),out t);
                            G_AutoProcessTime = t;
                    }
                    else
                    {
                        G_AutoProcessTime = t;
                    }

                    //G_AutoProcessTime = (dr["AutoProccessTime"] != null) ? Convert.ToDateTime(dr["AutoProccessTime"]) : t;
                    G_ReportServiceURL = dr["ReportServiceURL"].ToString();
                    G_ReportSerExeUrl = dr["ReportSerExeURL"].ToString();
                    G_DefaultMailID = dr["DefaultMailID"].ToString();
                    G_SmtpHostIP = dr["SmtpHostIP"].ToString();
                    G_ServerWorkerIP = dr["ServerWorkerIP"].ToString();
                    
                    G_NetworkDomain = dr["NetworkDomain"].ToString();
                    G_NetworkUser = dr["NetworkUser"].ToString();
                    G_NetworkPass = dr["NetworkPass"].ToString();
                    
                    Utils.DomainUserConfig.DomainName = dr["NetworkDomain"].ToString();
                    Utils.DomainUserConfig.DomainUser = dr["NetworkUser"].ToString();
                    Utils.DomainUserConfig.DomainPassword = dr["NetworkPass"].ToString();

                }
                
            }
            else
            {
                tset = false ;
            }


            sql = "Select * From AutoTimeSet";
            ds = Utils.Helper.GetData(sql, Utils.Helper.constr);
            hasRows = ds.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);
            if (hasRows)
            {
                G_SchAutoTimeSet = new List<string>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    G_SchAutoTimeSet.Add(dr["SchTime"].ToString());

                }
            }


            return tset;
        }


        public static List<string> WaterIP = new List<string>();
        public static string G_WaterIP;

        public static List<ShiftData> ShiftList = new List<ShiftData>();
        public static string G_ShiftList;

        public static DataTable dtShift;

        public static void SetGateInOutIPList()
        {
            //Setting GateInOut IP
            DataSet ds = new DataSet();
            string sql = "Select MachineIP From ReaderConfig where GateInOut = 1 Order By MachineIP";
            ds = Utils.Helper.GetData(sql, Utils.Helper.constr);
            bool hasRows = ds.Tables.Cast<DataTable>()
                           .Any(table => table.Rows.Count != 0);


            G_GateInOutIP = string.Empty;

            if (hasRows)
            {
                GateInOutIP.Clear();

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    GateInOutIP.Add(dr["MachineIP"].ToString());
                    G_GateInOutIP = G_GateInOutIP + dr["MachineIP"].ToString() + ",";
                }

            }
        }

        public static void SetLunchInOutIPList()
        {
            //Setting LunchInOut IP
            DataSet ds = new DataSet();
            string sql = "Select MachineIP From ReaderConfig where LunchInOut = 1 Order By MachineIP";
            ds = Utils.Helper.GetData(sql, Utils.Helper.constr);
            bool hasRows = ds.Tables.Cast<DataTable>()
                           .Any(table => table.Rows.Count != 0);

            G_LunchInOutIP = string.Empty;
            if (hasRows)
            {
                LunchInOutIP.Clear();

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    LunchInOutIP.Add(dr["MachineIP"].ToString());
                    G_LunchInOutIP = G_LunchInOutIP + dr["MachineIP"].ToString() + ",";
                }

            }
        }

        public static void SetWaterIPList()
        {
            //Setting WaterIP
            DataSet ds = new DataSet();
            string sql = "Select MachineIP From ReaderConfig where MachineDesc like '%Water%' Order By MachineIP";
            ds = Utils.Helper.GetData(sql, Utils.Helper.constr);
            bool hasRows = ds.Tables.Cast<DataTable>()
                           .Any(table => table.Rows.Count != 0);

            G_WaterIP = string.Empty;

            if (hasRows)
            {
                WaterIP.Clear();

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    WaterIP.Add(dr["MachineIP"].ToString());
                    G_WaterIP = G_WaterIP  + dr["MachineIP"].ToString() + ",";
                }

            }
        }

        public static void SetShiftList()
        {
            //Setting WaterIP
            DataSet ds = new DataSet();
            string sql = "Select * From MastShift where CompCode = '01' Order By ShiftSeq";
            ds = Utils.Helper.GetData(sql, Utils.Helper.constr);
            bool hasRows = ds.Tables.Cast<DataTable>()
                           .Any(table => table.Rows.Count != 0);

            G_ShiftList = string.Empty;
            if (hasRows)
            {
                ShiftList.Clear();
                dtShift = ds.Tables[0].Copy();

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    ShiftData t = new ShiftData();
                    t.ShiftCode = dr["ShiftCode"].ToString();
                    t.ShiftDesc = dr["ShiftDesc"].ToString();

                    t.ShiftStart = (TimeSpan)dr["ShiftStart"];
                    t.ShiftEnd = (TimeSpan)dr["ShiftEnd"];                 
                    t.ShiftInFrom = (TimeSpan)dr["ShiftInFrom"];                   
                    t.ShiftInTo = (TimeSpan)dr["ShiftInTo"];                   
                    t.ShiftOutFrom = (TimeSpan)dr["ShiftOutFrom"];                   
                    t.ShiftOutTo = (TimeSpan)dr["ShiftOutTo"];

                    t.BreakHrs = Convert.ToInt32(dr["BreakHrs"]);
                    t.ShiftHrs = Convert.ToInt32(dr["ShiftHrs"]);

                    ShiftList.Add(t);
                    G_ShiftList = G_ShiftList + dr["ShiftCode"].ToString() + ",";
                }

            }
        }

        public static DateTime GetSystemDateTime()
        {
            DateTime dt = new DateTime();

            DataSet ds = new DataSet();
            string sql = "Select GetDate() as CurrentDate ";
            ds = Utils.Helper.GetData(sql, Utils.Helper.constr);
            bool hasRows = ds.Tables.Cast<DataTable>()
                           .Any(table => table.Rows.Count != 0);          

            if (hasRows)
            {
               

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    dt = (DateTime)dr["CurrentDate"];
                }

            }
            return dt;
        }

        public static string GetFormRights(string FormName1)
        {
            string frmrights = "XXXV";
            int FormID = 0;

             //Setting WaterIP
            DataSet ds = new DataSet();
            string sql = "select FormId from MastFrm where FormName ='" + FormName1.Trim() + "'";
            
            ds = Utils.Helper.GetData(sql, Utils.Helper.constr);
            bool hasRows = ds.Tables.Cast<DataTable>()
                           .Any(table => table.Rows.Count != 0);

            if (hasRows)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    FormID = Convert.ToInt32(dr["FormID"]);
                }
            }

            sql = "Select top 1 * From UserRights where FormId = '" + FormID.ToString() + "' and Userid = '" + Utils.User.GUserID + "'";
            ds = new DataSet();
            ds = Utils.Helper.GetData(sql, Utils.Helper.constr);
            hasRows = ds.Tables.Cast<DataTable>()
                           .Any(table => table.Rows.Count != 0);

            if (hasRows)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    frmrights = ((Convert.ToBoolean(dr["Add1"])) ? "A" : "X")
                        + ((Convert.ToBoolean(dr["UpDate1"])) ? "U" : "X")
                        + ((Convert.ToBoolean(dr["Delete1"])) ? "D" : "X")
                        + ((Convert.ToBoolean(dr["View1"])) ? "V" : "X");

                }
            }

            return frmrights;
        }

        public static bool GetWrkGrpRights(int Formid, string WrkGrp, string EmpUnqID)
        {
            bool returnval = false;

            DataSet ds = new DataSet();

            if ( EmpUnqID != "")
            {
                WrkGrp = Utils.Helper.GetDescription("Select WrkGrp From MastEmp Where EmpUnqID ='" + EmpUnqID + "'", Utils.Helper.constr);
            }
            
            if (WrkGrp == "" && EmpUnqID == "")
            {
                return false;
            }
            

            string wkgsql = "Select * from UserSpRight where UserID = '" + Utils.User.GUserID + "' and FormID = '" + Formid.ToString() + "' and WrkGrp = '" + WrkGrp + "' and Active = 1";


            ds = Utils.Helper.GetData(wkgsql, Utils.Helper.constr);
            bool hasRows = ds.Tables.Cast<DataTable>()
                           .Any(table => table.Rows.Count != 0);

            if (hasRows)
            {
                returnval = true;
            }
            else
            {
                returnval = false;
            }

            return returnval;
        }


        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Defining type of data column gives proper data table 
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

    }

    class ShiftData
    {
        public string ShiftCode;
        public int ShiftSeq;
        public string ShiftDesc;
        public TimeSpan ShiftStart;
        public TimeSpan ShiftEnd;
        public TimeSpan ShiftInFrom;
        public TimeSpan ShiftInTo;
        
        public TimeSpan ShiftOutFrom;
        public TimeSpan ShiftOutTo;
        public bool NightFLG;
        public int BreakHrs;
        public int ShiftHrs;

    }

    class AttdLog
    {
        public string EmpUnqID;
        public DateTime PunchDate;
        public string IOFLG;
        public string MachineIP;
        public bool LunchFlg;
        public int tYear;
        public int tYearMt;
        public DateTime t1Date;
        public DateTime AddDt;
        public string AddID;
        public string TableName;
        public string Error;

        public override string ToString()
        {
            return IOFLG.ToString() + ";" + PunchDate.ToString("yyyy-MM-dd HH:mm:ss") + ";" + EmpUnqID.ToString() + ";" + MachineIP + ";" + ((LunchFlg) ? "1" : "0");
        }

        public string GetDBWriteString()
        {
            string dbstr = string.Empty;
            if (this.TableName == string.Empty)
                this.TableName = "AttdLog";

            dbstr = "Insert into " + this.TableName.Trim() + " (PunchDate,EmpUnqID,IOFLG,MachineIP,LunchFlg,tYear,tYearMt,t1Date,AddDt,AddID) Values (" +
                " '" + this.PunchDate.ToString("yyyy-MM-dd HH:mm:ss") + "','" + this.EmpUnqID + "','" + this.IOFLG + "','" + this.MachineIP + "'," +
                " '" + ((this.LunchFlg) ? "1" : "0") + "','" + this.tYear.ToString() + "'," +
                " '" + this.tYearMt.ToString() + "','" + this.t1Date.ToString("yyyy-MM-dd") + "'," +
                " '" + this.AddDt.ToString("yyyy-MM-dd HH:mm:ss") + "','" + Utils.User.GUserID + "');";

            return dbstr;
        }

        public string GetDBWriteErrString()
        {
            string dbstr = string.Empty;
           
            //if duplicate punch found place in errAttdLog
            dbstr = "Insert into errATTDLOG (PunchDate,EmpUnqID,IOFLG,MachineIP,LunchFlg,tYear,tYearMt,t1Date,AddDt,AddID) Values (" +
                " '" + this.PunchDate.ToString("yyyy-MM-dd HH:mm:ss") + "','" + this.EmpUnqID + "','" + this.IOFLG + "','" + this.MachineIP + "'," +
                " '" + ((this.LunchFlg) ? "1" : "0") + "','" + this.tYear.ToString() + "'," +
                " '" + this.tYearMt.ToString() + "','" + this.t1Date.ToString("yyyy-MM-dd") + "'," +
                " '" + this.AddDt.ToString("yyyy-MM-dd HH:mm:ss") + "','" + Utils.User.GUserID + "');";

            return dbstr;
        }
    }

}
