using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace Attendance.Classes
{
    
    
    class Globals
    {
        
        public static List<string> GateInOutIP = new List<string>();
        public static string G_GateInOutIP;

        public static List<string> LunchInOutIP = new List<string>();
        public static string G_LunchInOutIP;

        public static List<string> WaterIP = new List<string>();
        public static string G_WaterIP;

        public static List<ShiftData> ShiftList = new List<ShiftData>();
        public static string G_ShiftList;


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

}
