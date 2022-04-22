using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace AttdLog_TxtToDB
{
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
                " '" + this.AddDt.ToString("yyyy-MM-dd HH:mm:ss") + "','" + this.AddID + "');";

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
                " '" + this.AddDt.ToString("yyyy-MM-dd HH:mm:ss") + "','" + this.AddID + "');";

            return dbstr;
        }



        
    }


}
