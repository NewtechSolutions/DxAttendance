using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attendance.Classes;
using System.Data;
using System.Data.SqlClient;

namespace Attendance
{

    public class clsProcess
    {

        private DataSet dsAttdData;
        public SqlDataAdapter daAttdData;
        public SqlCommandBuilder AttdCmdBuilder;

        public clsProcess()
        {
            dsAttdData = new DataSet();
        }

        public void Lunch_HalfDayPost_Process(string tEmpUnqID, DateTime tDate,  out int result)
        {
            result = 0;

            if (string.IsNullOrEmpty(tEmpUnqID))
            {
                return;
            }

            
            //call main store proce.
            using (SqlConnection cn = new SqlConnection(Utils.Helper.constr))
            {
                try
                {
                    clsEmp Emp = new clsEmp();
                    Emp.CompCode = "01";
                    Emp.EmpUnqID = tEmpUnqID;

                    //check employee status
                    if (!Emp.GetEmpDetails(Emp.CompCode, Emp.EmpUnqID))
                    {
                        return;
                    }
                    else
                    {
                        //if not active 
                        if (!Emp.Active)
                            return;
                    }

                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = cn;
                        cmd.CommandType = CommandType.Text;
                        string sql = "Insert into ProcessLog (AddDt,AddId,ProcessType,FromDt,ToDt,EmpUnqID ) Values (" +
                            " GetDate(),'" + Utils.User.GUserID + "','LunchHalfDayPost','" + tDate.ToString("yyyy-MM-dd") + "'," +
                            " '" + tDate.ToString("yyyy-MM-dd") + "','" + tEmpUnqID + "')";
                        cmd.CommandText = sql;
                        cmd.ExecuteNonQuery();

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "Attd_Lunch_HalfDayPost";

                        SqlParameter spout = new SqlParameter();
                        spout.Direction = ParameterDirection.Output;
                        spout.DbType = DbType.Int32;
                        spout.ParameterName = "@presult";
                        int tout = 0;
                        spout.Value = tout;
                                                
                        cmd.Parameters.AddWithValue("@pEmpUnqID", Emp.EmpUnqID);
                        cmd.Parameters.AddWithValue("@pFromDt", tDate);
                        cmd.Parameters.Add(spout);
                        cmd.CommandTimeout = 0;
                        cmd.ExecuteNonQuery();

                        //get the output
                        result = (int)cmd.Parameters["@presult"].Value;

                    }

                }
                catch (Exception ex)
                {

                }

            }//using connection
        }
        
        public void LunchInOutProcess(string tEmpUnqID, DateTime tFromDt, DateTime tToDate, out int result)
        {
            result = 0;

            if (string.IsNullOrEmpty(tEmpUnqID))
            {
                return;
            }

            if (tToDate < tFromDt)
            {
                return;
            }

            //call main store proce.
            using (SqlConnection cn = new SqlConnection(Utils.Helper.constr))
            {
                try
                {
                    clsEmp Emp = new clsEmp();
                    Emp.CompCode = "01";
                    Emp.EmpUnqID = tEmpUnqID;

                    //check employee status
                    if (!Emp.GetEmpDetails(Emp.CompCode, Emp.EmpUnqID))
                    {
                        return;

                    }
                    else
                    {
                        //if not active 
                        if (!Emp.Active)
                            return;
                    }

                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = cn;
                        cmd.CommandType = CommandType.Text;
                        string sql = "Insert into ProcessLog (AddDt,AddId,ProcessType,FromDt,ToDt,EmpUnqID ) Values (" +
                            " GetDate(),'" + Utils.User.GUserID + "','LunchInOutProcess','" + tFromDt.ToString("yyyy-MM-dd") + "'," +
                            " '" + tToDate.ToString("yyyy-MM-dd") + "','" + tEmpUnqID + "')";
                        cmd.CommandText = sql;
                        cmd.ExecuteNonQuery();

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "Lunch_InOut_Process";

                        SqlParameter spout = new SqlParameter();
                        spout.Direction = ParameterDirection.Output;
                        spout.DbType = DbType.Int32;
                        spout.ParameterName = "@result";
                        int tout = 0;
                        spout.Value = tout;

                        tFromDt = tFromDt.AddHours(0).AddMinutes(1);
                        tToDate = tToDate.AddHours(23).AddMinutes(59);

                        cmd.Parameters.AddWithValue("@pWrkGrp", "");
                        cmd.Parameters.AddWithValue("@pEmpUnqID", Emp.EmpUnqID);
                        cmd.Parameters.AddWithValue("@pFromDt", tFromDt);
                        cmd.Parameters.AddWithValue("@pToDt", tToDate);
                        cmd.Parameters.Add(spout);
                        cmd.CommandTimeout = 0;
                        cmd.ExecuteNonQuery();

                        //get the output
                        result = (int)cmd.Parameters["@result"].Value;



                    }

                }
                catch (Exception ex)
                {

                }

            }//using connection
        }

        public void AttdProcess(string tEmpUnqID, DateTime tFromDt, DateTime tToDate, out int result,out string err)
        {
            result = 0;
            err = string.Empty;
            //trace setshift err
            string shifterr = string.Empty;
            string proerr = string.Empty;

            #region chk_primary
            if (string.IsNullOrEmpty(tEmpUnqID))
            {

                proerr = "EmpUnqID required...";
                err = proerr;                
                return;
            }

            if (tFromDt == DateTime.MinValue)
            {
                proerr = "Invalid From Date...";
                err = proerr;  
                return;
            }

            if (tToDate == DateTime.MinValue)
            {
                proerr = "Invalid To Date...";
                err = proerr; 
                return;
            }

            if (tToDate < tFromDt)
            {
                proerr = "Invalid Date Range...";
                err = proerr; 
                return;
            }

            #endregion chk_primary

           

           

            //call main store proce.
            using (SqlConnection cn = new SqlConnection(Utils.Helper.constr))
            {
                try
                {
                    clsEmp Emp = new clsEmp();
                    Emp.CompCode = "01";
                    Emp.EmpUnqID = tEmpUnqID;

                    //check employee status
                    if (!Emp.GetEmpDetails(Emp.CompCode, Emp.EmpUnqID))
                    {
                        proerr = "Employee Not Found...";
                        err = proerr;
                        return;

                    }
                    else
                    {
                        //if not active 
                        if (!Emp.Active)
                        {
                            proerr = "Employee is Not Active...";
                            err = proerr;
                            return;
                        }
                            
                    }

                    cn.Open();

                    #region Call_SP_Attd_Process

                    string sql = "Insert into ProcessLog (AddDt,AddId,ProcessType,FromDt,ToDt,WrkGrp,EmpUnqID) Values " +
                        "( GetDate(),'" + Utils.User.GUserID + "','AttdProcessSelf','" + tFromDt.ToString("yyyy-MM-dd") + "'," +
                        " '" + tToDate.ToString("yyyy-MM-dd") + "','" + Emp.WrkGrp + "','" + Emp.EmpUnqID + "')";
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();

                    cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "Attd_Process";

                    SqlParameter spout = new SqlParameter();
                    spout.Direction = ParameterDirection.Output;
                    spout.DbType = DbType.Int32;
                    spout.ParameterName = "@result";
                    int tout = 0;
                    spout.Value = tout;
                    cmd.Parameters.AddWithValue("@pWrkGrp", "");
                    cmd.Parameters.AddWithValue("@pEmpUnqID", Emp.EmpUnqID);
                    cmd.Parameters.AddWithValue("@pFromDt", tFromDt);
                    cmd.Parameters.AddWithValue("@pToDt", tToDate);
                    cmd.Parameters.Add(spout);
                    cmd.CommandTimeout = 0;
                    cmd.ExecuteNonQuery();
                    //get the output
                    result = (int)cmd.Parameters["@result"].Value;

                    if (result == 0)
                    {
                        proerr = "Primary Attd_Process Error in Store Procedure...";
                        err = proerr;
                        return;
                    }

                    #endregion Call_SP_Attd_Process

                    #region AppProcess

                    string nxtdt = "";


                    sql = "Select [Date],NextDayDate,CalendarYear as CalYear from f_table_date('" + tFromDt.ToString("yyyy-MM-dd") + "','" + tToDate.ToString("yyyy-MM-dd") + "') Order by Date";
                    DataSet dsDate = Utils.Helper.GetData(sql, Utils.Helper.constr);

                    bool hasRows = dsDate.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);

                    if (hasRows)
                    {
                        foreach (DataRow drDate in dsDate.Tables[0].Rows)
                        {
                            //Open EmpAttdRecord
                            sql = "Select tYear,tDate,CompCode,WrkGrp,EmpUnqID,ScheDuleShift,ConsShift,ConsIN,ConsOut,ConsWrkHrs,ConsOverTime," +
                                "Status,HalfDay,LeaveTyp,LeaveHalf,ActualStatus,Earlycome,EarlyGoing," +
                                "INPunch1,OutPunch1,WrkHrs1,INPunch2,OutPunch2,WrkHrs2,INPunch3,OutPunch3," +
                                "WrkHrs3,INPunch4,OutPunch4,WrkHrs4,TotalWorkhrs,TotalINPunchCount," +
                                "TotalOutPunchCount,LateCome,Rules,CalcOverTime,HalfDRule,partdate,CostCode " +
                                " from AttdData where CompCode = '01' and tYear ='" + drDate["CalYear"].ToString() +
                                "' and EmpUnqID = '" + Emp.EmpUnqID + "' and tDate ='" + Convert.ToDateTime(drDate["Date"]).ToString("yyyy-MM-dd") + "'" +
                                " And WrkGrp = '" + Emp.WrkGrp + "'  ";

                            //create data adapter
                            dsAttdData = new DataSet();
                            daAttdData = new SqlDataAdapter(new SqlCommand(sql, cn));
                            AttdCmdBuilder = new SqlCommandBuilder(daAttdData);

                            daAttdData.Fill(dsAttdData, "AttdData");

                            hasRows = dsAttdData.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);
                            if (hasRows)
                            {

                                foreach (DataRow drAttd in dsAttdData.Tables[0].Rows)
                                {


                                    #region SanctionINTime
                                    //Check for Sanction In Time
                                    string sDate, sInFrom, sInTo, sEmpCode;
                                    sInFrom = Convert.ToDateTime(drDate["Date"]).ToString("yyyy-MM-dd") + " 00:01";
                                    sInTo = Convert.ToDateTime(drDate["Date"]).ToString("yyyy-MM-dd") + " 23:59";
                                    sEmpCode = Emp.EmpUnqID;
                                    sDate = Convert.ToDateTime(drDate["Date"]).ToString("yyyy-MM-dd");

                                    string insql = "Select Convert(varchar(19),punchdate,121) from F_Get_Punches('" + sEmpCode + "','" + sInFrom + "','" + sInTo + "')" +
                                          " where tDate = '" + sDate + "' and IOFLG = 'I'  And SanFlg = 1 Order By SanID Desc ";

                                    string insantime = Utils.Helper.GetDescription(insql, Utils.Helper.constr);
                                    if (!string.IsNullOrEmpty(insantime))
                                    {
                                        drAttd["ConsIN"] = Convert.ToDateTime(insantime);
                                    }
                                    else
                                    {
                                        drAttd["ConsIN"] = DBNull.Value;
                                    }

                                    daAttdData.Update(dsAttdData, "AttdData");
                                    #endregion SanctionINTime


                                    #region SanctionOutTime

                                    //Get Previous Day Out Ref

                                    DateTime preday = Convert.ToDateTime(drAttd["tDate"]).AddDays(-1);
                                    sql = "Select Convert(varchar(19),ConsOut,121) from AttdData Where EmpUnqID = '" + sEmpCode + "' " +
                                        " and CompCode ='" + Emp.CompCode + "' And WrkGrp ='" + Emp.WrkGrp + "' and tYear ='" + drDate["CalYear"].ToString() + "' " +
                                        " and tdate ='" + preday.ToString("yyyy-MM-dd") + "' and ConsIn is not Null";

                                    string OutRef = Utils.Helper.GetDescription(sql, Utils.Helper.constr);
                                    sql = "Select Convert(varchar(19),ConsIn,121) from AttdData Where EmpUnqID = '" + sEmpCode + "' " +
                                          " and CompCode ='" + Emp.CompCode + "' And WrkGrp ='" + Emp.WrkGrp + "' and tYear ='" + drDate["CalYear"].ToString() + "' " +
                                        " and tdate ='" + preday.ToString("yyyy-MM-dd") + "'";
                                    string prein = Utils.Helper.GetDescription(sql, Utils.Helper.constr);


                                    string outsql = string.Empty;

                                    if (drAttd["ConsIN"].ToString() != "")
                                    {
                                        nxtdt = Convert.ToDateTime(drAttd["ConsIN"]).AddHours(25).ToString("yyyy-MM-dd HH:mm:ss");
                                        if (OutRef != "")
                                        {

                                            outsql = "Select Convert(varchar(19),punchdate,121) from F_Get_Punches('" + sEmpCode + "','" + Convert.ToDateTime(drAttd["ConsIn"]).ToString("yyyy-MM-dd HH:mm:ss") + "','" + nxtdt + "')" +
                                               " where  PunchDate > '" + OutRef + "'" +
                                               " and IOFLG = 'O' And SanFlg = 1 And SanDt ='" + Convert.ToDateTime(drDate["Date"]).ToString("yyyy-MM-dd HH:mm:ss") + "'" +
                                               " Order By SanID  Desc ";
                                        }
                                        else
                                        {
                                            outsql = "Select Convert(varchar(19),punchdate,121) from F_Get_Punches('" + sEmpCode + "','" + Convert.ToDateTime(drAttd["ConsIn"]).ToString("yyyy-MM-dd HH:mm:ss") + "','" + nxtdt + "')" +
                                               " where  " +
                                               " IOFLG = 'O' And SanFlg = 1 And SanDt ='" + Convert.ToDateTime(drDate["Date"]).ToString("yyyy-MM-dd HH:mm:ss") + "'" +
                                               " Order By SanID   ";
                                        }
                                    }
                                    else //if ConsIn is null
                                    {
                                        nxtdt = Convert.ToDateTime(drDate["Date"]).AddDays(1).AddHours(10).AddMinutes(20).ToString("yyyy-MM-dd HH:mm:ss");

                                        if (drAttd["InPunch1"] == DBNull.Value && OutRef == "")
                                        {
                                            outsql = "Select Convert(varchar(19),punchdate,121) from F_Get_Punches('" + sEmpCode + "','" + Convert.ToDateTime(drDate["Date"]).ToString("yyyy-MM-dd") + "','" + nxtdt + "') " +
                                              " where IOFLG = 'O' And SanFlg = 1 Order By SanID  Desc ";
                                        }
                                        else if (drAttd["InPunch1"] == DBNull.Value && OutRef != "")
                                        {
                                            outsql = "Select Convert(varchar(19),punchdate,121) from F_Get_Punches('" + sEmpCode + "','" + Convert.ToDateTime(drDate["Date"]).ToString("yyyy-MM-dd") + "','" + nxtdt + "')" +
                                                " where IOFLG = 'O' And SanFlg = 1 And PunchDate > '" + OutRef + "' and PunchDate < '" + nxtdt + "'  Order By SanID  Desc ";
                                        }
                                        else if (drAttd["InPunch1"] != DBNull.Value && OutRef == "")
                                        {
                                            outsql = "Select Convert(varchar(19),punchdate,121) from F_Get_Punches('" + sEmpCode + "','" + Convert.ToDateTime(drDate["Date"]).ToString("yyyy-MM-dd") + "','" + nxtdt + "')" +
                                               " where IOFLG = 'O' And SanFlg = 1 and PunchDate > '" + Convert.ToDateTime(drAttd["InPunch1"]).ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                                               " And PunchDate < '" + nxtdt + "' Order By SanID  Desc ";
                                        }
                                        else if (drAttd["InPunch1"] != DBNull.Value && OutRef != "")
                                        {
                                            outsql = "Select Convert(varchar(19),punchdate,121) from F_Get_Punches('" + sEmpCode + "','" + Convert.ToDateTime(drDate["Date"]).ToString("yyyy-MM-dd") + "','" + nxtdt + "')" +
                                               " where IOFLG = 'O' And SanFlg = 1 and PunchDate > '" + Convert.ToDateTime(drAttd["InPunch1"]).ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                                               " And PunchDate > '" + OutRef + "' and PunchDate < '" + nxtdt + "' Order By SanID  Desc ";

                                        }
                                    }


                                    string outsantime = Utils.Helper.GetDescription(outsql, Utils.Helper.constr);

                                    if (!string.IsNullOrEmpty(outsantime))
                                    {
                                        drAttd["ConsOut"] = Convert.ToDateTime(outsantime);
                                    }
                                    else
                                    {
                                        drAttd["ConsOut"] = DBNull.Value;
                                    }

                                    daAttdData.Update(dsAttdData, "AttdData");

                                    #endregion SanctionOutTime

                                    #region Check_For_Continueous_Duty

                                    if (drAttd["InPunch1"] != DBNull.Value && drAttd["OutPunch1"] != DBNull.Value && drAttd["InPunch2"] != DBNull.Value && drAttd["OutPunch2"] != DBNull.Value)
                                    {
                                        if (Convert.ToDateTime(drAttd["InPunch1"]).ToString("yyyy-MM-dd")
                                            == Convert.ToDateTime(drAttd["InPunch2"]).ToString("yyyy-MM-dd"))
                                        {
                                            TimeSpan outdiff1 = Convert.ToDateTime(drAttd["OutPunch1"]) - Convert.ToDateTime(drAttd["InPunch2"]);
                                            if (outdiff1.TotalMinutes <= 60 && outdiff1.TotalMinutes >= 0)
                                            {
                                                TimeSpan outdiff2 = Convert.ToDateTime(drAttd["InPunch1"]) - Convert.ToDateTime(drAttd["OutPunch1"]);
                                                if (outdiff2.TotalHours >= 6)
                                                {
                                                    drAttd["ConsIn"] = drAttd["InPunch1"];
                                                    drAttd["ConsOut"] = drAttd["OutPunch2"];
                                                    daAttdData.Update(dsAttdData, "AttdData");
                                                }
                                            }
                                        }
                                    }
                                    #endregion Check_For_Continueous_Duty

                                    #region Check_For_Maximum_Hours

                                    if (drAttd["InPunch1"] != DBNull.Value && drAttd["OutPunch1"] != DBNull.Value && drAttd["InPunch2"] != DBNull.Value && drAttd["OutPunch2"] != DBNull.Value)
                                    {
                                        if (Convert.ToDateTime(drAttd["OutPunch2"]) > Convert.ToDateTime(drAttd["OutPunch1"]))
                                        {
                                            if (Convert.ToDateTime(drAttd["InPunch1"]).ToString("yyyy-MM-dd")
                                            == Convert.ToDateTime(drAttd["InPunch2"]).ToString("yyyy-MM-dd"))
                                            {
                                                if (Convert.ToDouble(drAttd["WrkHrs1"]) > Convert.ToDouble(drAttd["WrkHrs2"]))
                                                {
                                                    drAttd["ConsIn"] = drAttd["InPunch1"];
                                                    drAttd["ConsOut"] = drAttd["OutPunch1"];
                                                    drAttd["ConsWrkHrs"] = drAttd["WrkHrs1"];
                                                    daAttdData.Update(dsAttdData, "AttdData");
                                                }
                                                else
                                                {
                                                    drAttd["ConsIn"] = drAttd["InPunch2"];
                                                    drAttd["ConsOut"] = drAttd["OutPunch2"];
                                                    drAttd["ConsWrkHrs"] = drAttd["WrkHrs2"];
                                                    daAttdData.Update(dsAttdData, "AttdData");
                                                }
                                            }

                                        }
                                    }
                                    #endregion Check_For_Maximum_Hours

                                    #region Double_Check_ConsIn
                                    if (drAttd["ConsIN"] == DBNull.Value)
                                    {
                                        if (drAttd["InPunch1"] != DBNull.Value && OutRef != "")
                                        {
                                            if (Convert.ToDateTime(drAttd["InPunch1"]) > Convert.ToDateTime(prein))
                                            {
                                                drAttd["ConsIn"] = drAttd["InPunch1"];
                                            }
                                        }
                                        else
                                        {
                                            drAttd["ConsIn"] = drAttd["InPunch1"];
                                        }

                                        daAttdData.Update(dsAttdData, "AttdData");
                                    }
                                    #endregion Double_Check_ConsIn

                                    #region Double_Check_ConsOut

                                    if (drAttd["ConsOut"] == DBNull.Value)
                                    {
                                        if (drAttd["OutPunch1"] != DBNull.Value && OutRef != "" && drAttd["ConsIn"] == DBNull.Value)
                                        {
                                            if (Convert.ToDateTime(drAttd["OutPunch1"]) > Convert.ToDateTime(OutRef))
                                            {
                                                drAttd["ConsOut"] = drAttd["OutPunch1"];
                                                daAttdData.Update(dsAttdData, "AttdData");
                                            }
                                        }
                                        else if (drAttd["OutPunch1"] != DBNull.Value && OutRef != "" && drAttd["ConsIn"] != DBNull.Value)
                                        {
                                            if (Convert.ToDateTime(drAttd["OutPunch1"]) > Convert.ToDateTime(drAttd["ConsIn"]) &&
                                                Convert.ToDateTime(drAttd["OutPunch1"]) != Convert.ToDateTime(OutRef)
                                                )
                                            {
                                                drAttd["ConsOut"] = drAttd["OutPunch1"];
                                                daAttdData.Update(dsAttdData, "AttdData");
                                            }
                                            else //'Get OutPunch from f_Get_Punches
                                            {
                                                outsql = "Select Convert(varchar(19),punchdate,121) from F_Get_Punches('" + sEmpCode + "','" + Convert.ToDateTime(drDate["Date"]).ToString("yyyy-MM-dd") + "','" + nxtdt + "')" +
                                                   " where IOFLG = 'O' " +
                                                   " And PunchDate > '" + OutRef + "' and PunchDate < '" + nxtdt + "' Order By SanID  Desc ";

                                                string nout = Utils.Helper.GetDescription(outsql, Utils.Helper.constr);
                                                if (nout != "")
                                                {
                                                    drAttd["ConsOut"] = Convert.ToDateTime(nout);
                                                    drAttd["OutPunch1"] = drAttd["ConsOut"];

                                                    string wrksql = "Select round( convert(float,DATEDIFF(MINUTE,'" + Convert.ToDateTime(drAttd["InPunch1"]).ToString("yyyy-MM-dd hh:mm:ss") + "','"
                                                        + Convert.ToDateTime(drAttd["OutPunch1"]).ToString("yyyy-MM-dd HH:mm:ss") + "'))/60 * 2,0)/2";

                                                    drAttd["WrkHrs1"] = Utils.Helper.GetDescription(wrksql, Utils.Helper.constr);
                                                    daAttdData.Update(dsAttdData, "AttdData");
                                                }
                                            }
                                        }
                                        else
                                        {
                                            drAttd["ConsOut"] = drAttd["OutPunch1"];
                                            daAttdData.Update(dsAttdData, "AttdData");
                                        }
                                    }
                                    #endregion Double_Check_ConsOut

                                    #region Set_Primary_AttdStatus
                                    if (drAttd["ConsIN"] != DBNull.Value && drAttd["ConsOut"] != DBNull.Value)
                                    {
                                        //TimeSpan t = Convert.ToDateTime(drAttd["ConsOut"]) - Convert.ToDateTime(drAttd["ConsIN"]);
                                        string wrksql = "Select round( convert(float,DATEDIFF(MINUTE,'" + Convert.ToDateTime(drAttd["ConsIN"]).ToString("yyyy-MM-dd HH:mm:ss") + "','" + Convert.ToDateTime(drAttd["ConsOut"]).ToString("yyyy-MM-dd HH:mm:ss") + "'))/60 * 2,0)/2";

                                        drAttd["ConsWrkHrs"] = Utils.Helper.GetDescription(wrksql, Utils.Helper.constr);
                                        drAttd["Status"] = "P";

                                    }
                                    else
                                    {
                                        drAttd["Status"] = "A";
                                    }

                                    daAttdData.Update(dsAttdData, "AttdData");
                                    #endregion Set_Primary_AttdStatus

                                    #region PostLeave

                                    string sansql = "Select top 1 sanid,SchLeave,SchLeaveHalf From MastLeaveSchedule " +
                                       " where EmpUnqID = '" + sEmpCode + "' " +
                                       " And tDate ='" + Convert.ToDateTime(drDate["Date"]).ToString("yyyy-MM-dd") + "' " +
                                       " And SchLeave is not null Order By SanID Desc";
                                    DataSet ds = Utils.Helper.GetData(sansql, Utils.Helper.constr);
                                    hasRows = ds.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);
                                    if (hasRows)
                                    {
                                        DataRow dr = ds.Tables[0].Rows[0];
                                        drAttd["LeaveTyp"] = dr["schLeave"];
                                        drAttd["LeaveHalf"] = dr["SchLeaveHalf"];

                                        daAttdData.Update(dsAttdData, "AttdData");
                                    }

                                    #endregion PostLeave

                                    #region Check_Sanction_Shift_OtherInfo

                                    sansql = "Select top 1 ConsShift From MastLeaveSchedule " +
                                       " where EmpUnqID = '" + sEmpCode + "' " +
                                       " And tDate ='" + Convert.ToDateTime(drDate["Date"]).ToString("yyyy-MM-dd") + "' " +
                                       " And ConsShift is not null Order By SanID Desc";

                                    ds = Utils.Helper.GetData(sansql, Utils.Helper.constr);
                                    hasRows = ds.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);
                                    if (hasRows)
                                    {
                                        DataRow dr = ds.Tables[0].Rows[0];
                                        DataRow drAttdCopy = drAttd;

                                        SetShift(daAttdData, dsAttdData, drAttdCopy, Emp, dr["ConsShift"].ToString(),out shifterr);
                                    }
                                    else
                                    {
                                        DataRow drAttdCopy = drAttd;
                                        SetShift(daAttdData, dsAttdData, drAttdCopy, Emp, drAttd["ScheduleShift"].ToString(), out shifterr);
                                    }



                                    #endregion Check_Sanction_Shift_OtherInfo

                                    #region Post_Sanction_OT
                                    if (Emp.OTFLG)
                                    {
                                        sansql = "Select top 1 ConsOverTime From MastLeaveSchedule " +
                                           " where EmpUnqID = '" + sEmpCode + "' " +
                                           " And tDate ='" + Convert.ToDateTime(drDate["Date"]).ToString("yyyy-MM-dd") + "' " +
                                           " And ConsOverTime is not null Order By SanID Desc";

                                        ds = Utils.Helper.GetData(sansql, Utils.Helper.constr);
                                        hasRows = ds.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);
                                        if (hasRows)
                                        {
                                            DataRow dr = ds.Tables[0].Rows[0];
                                            drAttd["ConsOverTime"] = dr["ConsOverTime"];
                                            daAttdData.Update(dsAttdData, "AttdData");

                                        }
                                    }
                                    #endregion Post_Sanction_OT

                                    #region Post_HalfDay
                                    if (drAttd["LeaveTyp"] != DBNull.Value && !string.IsNullOrEmpty(drAttd["LeaveTyp"].ToString()))
                                    {
                                        drAttd["LateCome"] = "";
                                        drAttd["EarlyGoing"] = "";
                                        drAttd["EarlyCome"] = "";
                                        daAttdData.Update(dsAttdData, "AttdData");

                                    }

                                    double tconswrkhrs = 0;
                                    double.TryParse(drAttd["ConsWrkHrs"].ToString(), out tconswrkhrs);
                                    if (tconswrkhrs >= 4 && tconswrkhrs < 6 && drAttd["Status"].ToString() == "P")
                                    {
                                        switch (drAttd["LeaveTyp"].ToString())
                                        {
                                            case "WO":
                                                drAttd["LateCome"] = "";
                                                drAttd["EarlyGoing"] = "";
                                                drAttd["EarlyCome"] = "";
                                                break;
                                            case "HL":
                                                drAttd["LateCome"] = "";
                                                drAttd["EarlyGoing"] = "";
                                                drAttd["EarlyCome"] = "";
                                                break;
                                            default:
                                                if (drAttd["ConsIn"] is DateTime)
                                                {
                                                    drAttd["HalfDay"] = 1;
                                                }
                                                break;
                                        }
                                        daAttdData.Update(dsAttdData, "AttdData");
                                    }
                                    else if (tconswrkhrs >= 0 && tconswrkhrs < 4 && drAttd["Status"].ToString() == "P")
                                    {
                                        switch (drAttd["LeaveTyp"].ToString())
                                        {
                                            case "WO":
                                                drAttd["LateCome"] = "";
                                                drAttd["EarlyGoing"] = "";
                                                drAttd["EarlyCome"] = "";
                                                break;
                                            case "HL":
                                                drAttd["LateCome"] = "";
                                                drAttd["EarlyGoing"] = "";
                                                drAttd["EarlyCome"] = "";
                                                break;
                                            default:
                                                if (drAttd["ConsIn"] is DateTime)
                                                {
                                                    drAttd["Status"] = "A";
                                                    drAttd["HalfDay"] = 0;
                                                }
                                                break;
                                        }
                                        daAttdData.Update(dsAttdData, "AttdData");
                                    }

                                    #endregion Post_HalfDay

                                    #region Final_Status_Marking
                                    if (drAttd["LeaveTyp"].ToString() != "")
                                    {
                                        drAttd["Status"] = "P";

                                        if (drAttd["LeaveTyp"].ToString() == "AB")
                                        {
                                            drAttd["Status"] = "A";
                                            drAttd["ConsOverTime"] = 0;
                                        }
                                        if (drAttd["LeaveTyp"].ToString() == "SP")
                                        {
                                            drAttd["Status"] = "A";
                                            drAttd["ConsOverTime"] = 0;
                                        }

                                        daAttdData.Update(dsAttdData, "AttdData");

                                    }

                                    if (drAttd["ConsIn"] == DBNull.Value)
                                    {
                                        drAttd["ActualStatus"] = "A";
                                    }
                                    else
                                    {
                                        drAttd["ActualStatus"] = "P";
                                    }

                                    daAttdData.Update(dsAttdData, "AttdData");

                                    #endregion Final_Status_Marking

                                    #region GateInOutProcess_CONT
                                    if (Emp.WrkGrp == "CONT")
                                    {
                                        cmd = new SqlCommand();
                                        cmd.Connection = cn;
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.CommandText = "GateINOutProcess";
                                        cmd.Parameters.AddWithValue("@pEmpUnqID", Emp.EmpUnqID);
                                        cmd.Parameters.AddWithValue("@pDate", Convert.ToDateTime(drDate["Date"]));
                                        cmd.CommandTimeout = 0;
                                        cmd.ExecuteNonQuery();
                                    }
                                    #endregion GateInOutProcess_CONT

                                }// AttdDataLoop

                            }//AttdData DataSet


                        }//Date loop
                    }//Date DataSet

                    result = 1;

                    if (!string.IsNullOrEmpty(shifterr))
                    {
                        err = err + shifterr;
                    }
                    
                    if(!string.IsNullOrEmpty(proerr))
                    {
                        err = err + proerr;
                    }

                    return;

                    #endregion AppProcess

                    

                }
                catch (Exception ex)
                {
                    result = 0;
                    err = "Error in Main Function : " + ex.ToString();
                    return;
                }
            }
        }

        public void LunchProcess(string tEmpUnqID, DateTime tFromDt, DateTime tToDate, out int result)
        {
            result = 0;

            if (string.IsNullOrEmpty(tEmpUnqID))
            {
                return;
            }

            if (tToDate < tFromDt)
            {
                return;
            }

            //call main store proce.
            using (SqlConnection cn = new SqlConnection(Utils.Helper.constr))
            {
                try
                {
                    clsEmp Emp = new clsEmp();
                    Emp.CompCode = "01";
                    Emp.EmpUnqID = tEmpUnqID;

                    //check employee status
                    if (!Emp.GetEmpDetails(Emp.CompCode, Emp.EmpUnqID))
                    {
                        return;

                    }
                    else
                    {
                        //if not active 
                        if (!Emp.Active)
                            return;
                    }

                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = cn;
                        cmd.CommandType = CommandType.Text;
                        string sql = "Insert into ProcessLog (AddDt,AddId,ProcessType,FromDt,ToDt,EmpUnqID ) Values (" +
                            " GetDate(),'" + Utils.User.GUserID + "','MessProcess','" + tFromDt.ToString("yyyy-MM-dd") + "'," +
                            " '" + tToDate.ToString("yyyy-MM-dd") + "','" + tEmpUnqID + "')";
                        cmd.CommandText = sql;
                        cmd.ExecuteNonQuery();

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "Mess_Process";

                        SqlParameter spout = new SqlParameter();
                        spout.Direction = ParameterDirection.Output;
                        spout.DbType = DbType.Int32;
                        spout.ParameterName = "@result";
                        int tout = 0;
                        spout.Value = tout;

                        tFromDt = tFromDt.AddHours(0).AddMinutes(1);
                        tToDate = tToDate.AddHours(23).AddMinutes(59);

                        cmd.Parameters.AddWithValue("@pEmpUnqID", Emp.EmpUnqID);
                        cmd.Parameters.AddWithValue("@pFromDt", tFromDt);
                        cmd.Parameters.AddWithValue("@pToDt", tToDate);
                        cmd.Parameters.Add(spout);
                        cmd.CommandTimeout = 0;
                        cmd.ExecuteNonQuery();

                        //get the output
                        result = (int)cmd.Parameters["@result"].Value;



                    }

                }
                catch (Exception ex)
                {

                }

            }//using connection
        }

        public void SetShift(SqlDataAdapter daAttdData, DataSet dsAttdData, DataRow drAttd, clsEmp Emp, string tSchShift, out string err)
        {
            err = string.Empty;
            try
            {


                // this Function will set EarlyCome,Latecome,EarlyGoing,HalfDay,Shift,OverTime
                #region Setting_Vars

                string tShift = string.Empty;
                string sEmpCode = Emp.EmpUnqID;
                string eWrkGrp = string.Empty;
                int eGradeCode = 0;
                DateTime tDate = Convert.ToDateTime(drAttd["tDate"]);
                DateTime tInTime;
                DateTime? tOutTime = new DateTime?();

                Boolean tOTFLG = false;
                tOTFLG = Emp.OTFLG;
                tShift = tSchShift;
                eWrkGrp = Emp.WrkGrp;

                if (drAttd["ConsIN"] == DBNull.Value)
                {

                    drAttd["ConsOverTime"] = 0;
                    drAttd["CalcOvertime"] = 0;

                    daAttdData.Update(dsAttdData, "AttdData");
                    return;
                }

                tInTime = Convert.ToDateTime(drAttd["ConsIN"]);

                if (drAttd["ConsOut"] != DBNull.Value)
                {
                    tOutTime = (DateTime?)drAttd["ConsOut"];
                }

                //calc AutoShift on WO,HL
                if (tShift == "WO" || tShift == "HL")
                {
                    tShift = "";
                }

                if (eWrkGrp == "COMP")
                {
                    eGradeCode = Convert.ToInt32(Emp.GradeCode);
                }
                else
                {
                    eGradeCode = 0;
                }

                DateTime ShiftStart = new DateTime(), ShiftEnd = new DateTime(), ShiftInFrom = new DateTime(), ShiftInTo = new DateTime(), ShiftOutFrom = new DateTime(), ShiftOutTo = new DateTime();
                double ShiftHrs = 0, ShiftBreak = 0;
                double OverTime = 0, BreakHours = 0;

                //'for early come
                double secearly = 0, hurearly = 0, minearly = 0;
                string sminearly = "";


                //'for late come
                double seclate = 0, hurlate = 0, minlate = 0;
                string sminlate = "";

                //'for early going
                double secgone = 0, hurgone = 0, mingone = 0;
                string smingone = "";

                #endregion Setting_Vars

                if (drAttd["ConsIN"] is DateTime && tShift == "")
                {
                    #region AutoSiftCalc
                    // Create DataView
                    DataView dvShift = new DataView(Globals.dtShift);
                    dvShift.Sort = "ShiftSeq ASC";

                    foreach (DataRowView drShift in dvShift)
                    {

                        #region Set_shiftvars
                        ShiftHrs = Convert.ToInt32(drShift["ShiftHrs"].ToString());
                        ShiftBreak = Convert.ToInt32(drShift["BreakHrs"].ToString());

                        ShiftStart = tDate.AddHours(Convert.ToDateTime(drShift["ShiftStart"].ToString()).Hour)
                            .AddMinutes(Convert.ToDateTime(drShift["ShiftStart"].ToString()).Minute);

                        ShiftEnd = Convert.ToDateTime(ShiftStart.AddHours(ShiftHrs));

                        ShiftInFrom = Convert.ToDateTime(tDate
                            .AddHours(Convert.ToDateTime(drShift["ShiftInFrom"].ToString()).Hour)
                            .AddMinutes(Convert.ToDateTime(drShift["ShiftInFrom"].ToString()).Minute)
                            .AddSeconds(Convert.ToDateTime(drShift["ShiftInFrom"].ToString()).Second));

                        ShiftInTo = Convert.ToDateTime(tDate
                            .AddHours(Convert.ToDateTime(drShift["ShiftInTo"].ToString()).Hour)
                            .AddMinutes(Convert.ToDateTime(drShift["ShiftInTo"].ToString()).Minute)
                            .AddSeconds(Convert.ToDateTime(drShift["ShiftInTo"].ToString()).Second));

                        if (!Convert.ToBoolean(drShift["NightFlg"]))
                        {
                            //if not night shift
                            ShiftOutFrom = Convert.ToDateTime(tDate
                                .AddHours(Convert.ToDateTime(drShift["ShiftOutFrom"].ToString()).Hour)
                                .AddMinutes(Convert.ToDateTime(drShift["ShiftOutFrom"].ToString()).Minute)
                                .AddSeconds(Convert.ToDateTime(drShift["ShiftOutFrom"].ToString()).Second));

                            ShiftOutTo = Convert.ToDateTime(tDate
                                .AddHours(Convert.ToDateTime(drShift["ShiftOutTo"].ToString()).Hour)
                                .AddMinutes(Convert.ToDateTime(drShift["ShiftOutTo"].ToString()).Minute)
                                .AddSeconds(Convert.ToDateTime(drShift["ShiftOutTo"].ToString()).Second));
                        }
                        else
                        {
                            //if shift is night shift
                            ShiftOutFrom = Convert.ToDateTime(tDate
                                .AddDays(1).AddHours(Convert.ToDateTime(drShift["ShiftOutTo"].ToString()).Hour)
                                .AddMinutes(Convert.ToDateTime(drShift["ShiftOutTo"].ToString()).Minute)
                                .AddSeconds(Convert.ToDateTime(drShift["ShiftOutTo"].ToString()).Second));

                            ShiftOutTo = Convert.ToDateTime(tDate.AddDays(1)
                                .AddHours(Convert.ToDateTime(drShift["ShiftOutTo"].ToString()).Hour)
                                .AddMinutes(Convert.ToDateTime(drShift["ShiftOutTo"].ToString()).Minute)
                                .AddSeconds(Convert.ToDateTime(drShift["ShiftOutTo"].ToString()).Second));
                        }

                        #endregion Set_shiftvars

                        #region Setting_Late_Early

                        #region Check_intime_if_follow_in_shiftintime
                        if (tInTime >= ShiftInFrom && tInTime <= ShiftInTo)
                        {
                            drAttd["ConsShift"] = drShift["ShiftCode"].ToString();

                            #region Set_LateComming
                            //'Calc LateCome,EarlyGone
                            TimeSpan tDiff = (tInTime - ShiftStart);

                            if (tDiff.TotalSeconds > 660)
                            {

                                seclate = tDiff.TotalSeconds;
                                hurlate = Math.Truncate(seclate / 3600);
                                minlate = Math.Truncate((seclate - (hurlate * 3600)) / 60);
                                sminlate = string.Format("{0:00}:{1:00}", hurlate,minlate);
                                drAttd["LateCome"] = sminlate;

                                //'added on 14-02-2014 if late more than 30 min mark halfday exclude agm and above on comp emp.
                                if (eWrkGrp == "COMP" && eGradeCode <= 15)
                                {
                                    drAttd["Halfday"] = 0;
                                    drAttd["Latecome"] = "";
                                    drAttd["EarlyCome"] = "";
                                }
                                else if (seclate > 1800 && eWrkGrp == "COMP" && eGradeCode > 15 && drAttd["LeaveTyp"].ToString() != "")
                                {
                                    drAttd["Halfday"] = 0;
                                    drAttd["Latecome"] = "";
                                    drAttd["EarlyCome"] = "";
                                }
                                else if (seclate > 5400 && eWrkGrp == "COMP" && eGradeCode > 15 && drAttd["LeaveTyp"].ToString() == "")
                                {
                                    drAttd["Halfday"] = 0;
                                    drAttd["Status"] = "A";
                                    drAttd["Latecome"] = "";
                                }
                                else if (seclate > 5400 && eWrkGrp == "COMP")
                                {
                                    drAttd["Halfday"] = 0;
                                    drAttd["Status"] = "A";
                                    drAttd["Latecome"] = "";
                                }
                                //
                                daAttdData.Update(dsAttdData, "AttdData");
                            }//

                            #endregion Set_LateComming


                            if (drAttd["ConsOut"] is DateTime)
                            {

                                if (Convert.ToDateTime(drAttd["ConsOut"]) >= ShiftOutFrom && Convert.ToDateTime(drAttd["ConsOut"]) <= ShiftOutTo)
                                {
                                    drAttd["ConsShift"] = drShift["ShiftCode"].ToString();
                                    daAttdData.Update(dsAttdData, "AttdData");
                                }

                                #region Fix_For_GI_SMG
                                if (drAttd["ConsShift"].ToString() == "DD")
                                {
                                    if (Convert.ToDateTime(drAttd["ConsOut"]) >= ShiftOutFrom && Convert.ToDateTime(drAttd["ConsOut"]) <= ShiftOutTo)
                                    {
                                        drAttd["ConsShift"] = "DD";
                                        daAttdData.Update(dsAttdData, "AttdData");
                                    }
                                    else
                                    {
                                        drAttd["ConsShift"] = "GI";

                                        DataRow[] drs = Globals.dtShift.Select("ShiftCode = 'GI'");
                                        foreach (DataRow tdr in drs)
                                        {
                                            ShiftHrs = Convert.ToInt32(tdr["Shifthrs"]);
                                            ShiftEnd = ShiftStart.AddHours(ShiftHrs);
                                            ShiftBreak = Convert.ToInt32(tdr["BreakHrs"]);
                                        }
                                        daAttdData.Update(dsAttdData, "AttdData");
                                    }

                                }
                                #endregion Fix_For_GI_SMG

                                #region Set_EarlyGoing

                                TimeSpan ts = (Convert.ToDateTime(drAttd["ConsOut"]) - ShiftEnd );
                                if (ts.TotalSeconds < -660 && ts.TotalSeconds < 0)
                                {
                                    TimeSpan t1 =  ShiftEnd - Convert.ToDateTime(drAttd["ConsOut"]) ;

                                    secgone = t1.TotalSeconds;
                                    hurgone = Math.Truncate(secgone / 3600);
                                    mingone = Math.Truncate((secgone - (hurgone * 3600)) / 60);
                                    smingone = string.Format("{0:00}:{1:00}", hurgone, mingone); 

                                    drAttd["EarlyGoing"] = smingone;
                                    if (eWrkGrp == "COMP" && eGradeCode <= 15)
                                    {
                                        drAttd["Halfday"] = 0;
                                        drAttd["EarlyGoing"] = "";
                                    }
                                    else if (secgone > 3600 && eWrkGrp == "COMP" && eGradeCode > 15 && drAttd["LeaveTyp"].ToString() != "")
                                    {
                                        drAttd["Halfday"] = 0;
                                        drAttd["EarlyGoing"] = "";
                                    }
                                    else if (secgone > 3600 && secgone <= 7200 && eWrkGrp == "COMP" && eGradeCode > 15 && drAttd["LeaveTyp"].ToString() == "" && drAttd["Status"].ToString() == "P")
                                    {
                                        drAttd["Halfday"] = 1;
                                    }
                                    else if (secgone > 3600 && secgone <= 7200 && eWrkGrp == "COMP" && eGradeCode > 15 && drAttd["LeaveTyp"].ToString() == "" && drAttd["Status"].ToString() == "A")
                                    {
                                        drAttd["Halfday"] = 0;
                                        drAttd["EarlyGoing"] = "";
                                    }
                                    else if (secgone > 3600 && secgone <= 7200 && eWrkGrp == "COMP")
                                    {
                                        drAttd["Halfday"] = 0;
                                    }


                                }
                                #endregion Set_EarlyGoing


                            }

                            #region Set_EarlyCome

                            TimeSpan t2 = (ShiftStart - tInTime);
                            secearly = t2.TotalSeconds;

                            if (secearly > 660)
                            {
                                hurearly = Math.Truncate(secearly / 3600);
                                minearly = Math.Truncate((secearly - (hurearly * 3600)) / 60);
                                sminearly = string.Format("{0:00}:{1:00}", hurearly, minearly); 

                                drAttd["EarlyCome"] = sminearly;

                                if (eWrkGrp == "COMP" && eGradeCode <= 15)
                                {
                                    drAttd["Halfday"] = 0;
                                    drAttd["Latecome"] = "";
                                    drAttd["EarlyCome"] = "";
                                }
                                else if (secearly > 1800 && eWrkGrp == "COMP" && eGradeCode > 15 && drAttd["LeaveTyp"].ToString() != "")
                                {
                                    drAttd["Halfday"] = 0;
                                    drAttd["Latecome"] = "";
                                    drAttd["EarlyCome"] = "";
                                }
                                else if (secearly > 5400 && eWrkGrp == "COMP" && eGradeCode > 15 && drAttd["LeaveTyp"].ToString() == "")
                                {
                                    drAttd["Halfday"] = 0;
                                    drAttd["Latecome"] = "A";
                                    drAttd["EarlyCome"] = "";
                                }
                                else if (secearly > 5400 && eWrkGrp == "COMP")
                                {
                                    drAttd["Halfday"] = 0;
                                    drAttd["Latecome"] = "A";
                                    drAttd["EarlyCome"] = "";
                                }
                                daAttdData.Update(dsAttdData, "AttdData");

                                BreakHours = Convert.ToDouble(drShift["BreakHrs"]);
                            }


                            #endregion Set_EarlyCome

                            goto OTCalc;

                        }

                        #endregion Check_intime_if_follow_in_shiftintime

                        #endregion Setting_Late_Early

                        #region Reset_ShiftVars
                        seclate = 0;
                        hurlate = 0;
                        minlate = 0;
                        sminlate = "";

                        secearly = 0;
                        hurearly = 0;
                        sminearly = "";

                        secgone = 0;
                        hurgone = 0;
                        mingone = 0;
                        smingone = "";
                        #endregion Reset_ShiftVars

                    }//foreach loop for shift
                    #endregion AutoSiftCalc
                }
                else
                {
                    #region SchShiftCalc

                    DataRow[] drShiftC = Globals.dtShift.Select("ShiftCode = '" + tShift + "'");

                    // 'Resolve Type MisMatch Error : Added for if user wrongly entered sanction shift - using Bulk Sanction
                    if (drShiftC.GetLength(0) == 0)
                    {
                        if (ShiftInFrom == DateTime.MinValue)
                        {
                            //auto shift calc
                            if (tInTime != DateTime.MinValue && tOutTime.HasValue)
                            {
                                string selferr = string.Empty;
                                SetShift(daAttdData, dsAttdData, drAttd, Emp, "", out selferr);
                            }

                        }

                        return;
                    }

                    #region Set_shiftvars
                    foreach (DataRow drShift in drShiftC)
                    {
                        ShiftHrs = Convert.ToInt32(drShift["ShiftHrs"].ToString());
                        ShiftBreak = Convert.ToInt32(drShift["BreakHrs"].ToString());

                        ShiftStart = tDate.AddHours(Convert.ToDateTime(drShift["ShiftStart"].ToString()).Hour)
                            .AddMinutes(Convert.ToDateTime(drShift["ShiftStart"].ToString()).Minute);

                        ShiftEnd = Convert.ToDateTime(ShiftStart.AddHours(ShiftHrs));

                        ShiftInFrom = tDate.AddHours(Convert.ToDateTime(drShift["ShiftInFrom"].ToString()).Hour)
                            .AddMinutes(Convert.ToDateTime(drShift["ShiftInFrom"].ToString()).Minute)
                            .AddSeconds(Convert.ToDateTime(drShift["ShiftInFrom"].ToString()).Second);

                        ShiftInTo = tDate.AddHours(Convert.ToDateTime(drShift["ShiftInTo"].ToString()).Hour)
                            .AddMinutes(Convert.ToDateTime(drShift["ShiftInTo"].ToString()).Minute)
                            .AddSeconds(Convert.ToDateTime(drShift["ShiftInTo"].ToString()).Second);

                        if (!Convert.ToBoolean(drShift["NightFlg"]))
                        {
                            //if not night shift
                            ShiftOutFrom = tDate.AddHours(Convert.ToDateTime(drShift["ShiftOutFrom"].ToString()).Hour)
                                .AddMinutes(Convert.ToDateTime(drShift["ShiftOutFrom"].ToString()).Minute)
                                .AddSeconds(Convert.ToDateTime(drShift["ShiftOutFrom"].ToString()).Second);

                            ShiftOutTo = tDate.AddHours(Convert.ToDateTime(drShift["ShiftOutTo"].ToString()).Hour)
                                .AddMinutes(Convert.ToDateTime(drShift["ShiftOutTo"].ToString()).Minute)
                                .AddSeconds(Convert.ToDateTime(drShift["ShiftOutTo"].ToString()).Second);
                        }
                        else
                        {
                            //if night shift
                            ShiftOutFrom = tDate.AddDays(1).AddHours(Convert.ToDateTime(drShift["ShiftOutFrom"].ToString()).Hour)
                                .AddMinutes(Convert.ToDateTime(drShift["ShiftOutFrom"].ToString()).Minute)
                                .AddSeconds(Convert.ToDateTime(drShift["ShiftOutFrom"].ToString()).Second);

                            ShiftOutTo = tDate.AddDays(1).AddHours(Convert.ToDateTime(drShift["ShiftOutTo"].ToString()).Hour)
                                .AddMinutes(Convert.ToDateTime(drShift["ShiftOutTo"].ToString()).Minute)
                                .AddSeconds(Convert.ToDateTime(drShift["ShiftOutTo"].ToString()).Second);
                        }
                    }

                    drAttd["ConsShift"] = tShift;

                    #endregion Set_shiftvars

                    #region Setting_Late_Early

                    #region Check_intime_if_follow_in_shiftintime
                    if (tInTime >= ShiftInFrom && tInTime <= ShiftInTo)
                    {

                        #region Set_LateComming
                        //'Calc LateCome,EarlyGone
                        if (tInTime > ShiftStart)
                        {
                            TimeSpan tDiff = (tInTime - ShiftStart);
                            if (tDiff.TotalSeconds > 660)
                            {

                                seclate = tDiff.TotalSeconds;
                                hurlate = Math.Truncate(seclate / 3600);
                                minlate = Math.Truncate((seclate - (hurlate * 3600)) / 60);
                                sminlate = string.Format("{0:00}:{1:00}", hurlate, minlate); 
                                drAttd["LateCome"] = sminlate;

                                //'added on 14-02-2014 if late more than 30 min mark halfday exclude agm and above on comp emp.
                                if (eWrkGrp == "COMP" && eGradeCode <= 15)
                                {
                                    drAttd["Halfday"] = 0;
                                    drAttd["Latecome"] = "";
                                    drAttd["EarlyCome"] = "";
                                }
                                else if (seclate > 1800 && eWrkGrp == "COMP" && eGradeCode > 15 && drAttd["LeaveTyp"].ToString() != "")
                                {
                                    drAttd["Halfday"] = 0;
                                    drAttd["Latecome"] = "";
                                    drAttd["EarlyCome"] = "";
                                }
                                else if (seclate > 1800 && seclate < 5400 && eWrkGrp == "COMP" && eGradeCode > 15 && drAttd["LeaveTyp"].ToString() == "")
                                    drAttd["Halfday"] = 1;

                                else if (seclate > 5400 && eWrkGrp == "COMP" && eGradeCode > 15 && drAttd["LeaveTyp"].ToString() == "")
                                {
                                    drAttd["Halfday"] = 0;
                                    drAttd["Status"] = "A";
                                    drAttd["Latecome"] = "";
                                }
                                else if (seclate > 5400 && eWrkGrp == "COMP")
                                {
                                    drAttd["Halfday"] = 0;
                                    drAttd["Status"] = "A";
                                    drAttd["Latecome"] = "";
                                }
                                
                                daAttdData.Update(dsAttdData, "AttdData");
                            }
                        }
                        
                        #endregion Set_LateComming

                        #region Set_EarlyCome

                        if (tInTime < ShiftStart)
                        {
                            TimeSpan tDiff = (tInTime - ShiftStart);

                            if (tDiff.TotalSeconds <= -660)
                            {
                                TimeSpan t4 = (ShiftStart - tInTime);

                                secearly = t4.TotalSeconds;

                                hurearly = Math.Truncate(secearly / 3600);
                                minearly = Math.Truncate((secearly - (hurearly * 3600)) / 60);

                                sminearly = string.Format("{0:00}:{1:00}", hurearly, minearly);

                                drAttd["EarlyCome"] = sminearly;

                                if (eWrkGrp == "COMP" && eGradeCode <= 15)
                                {
                                    drAttd["Halfday"] = 0;
                                    drAttd["Latecome"] = "";
                                    drAttd["EarlyCome"] = "";
                                }
                                else if (secearly > 1800 && eWrkGrp == "COMP" && eGradeCode > 15 && drAttd["LeaveTyp"].ToString() != "")
                                {
                                    drAttd["Halfday"] = 0;
                                    drAttd["Latecome"] = "";
                                    drAttd["EarlyCome"] = "";

                                }
                                else if (tInTime <= ShiftStart)
                                {
                                    if (drAttd["ConsOut"] != DBNull.Value)
                                    {
                                        if (Convert.ToDateTime(drAttd["ConsOut"]) >= ShiftEnd)
                                        {
                                            drAttd["Halfday"] = 0;
                                            drAttd["Status"] = "P";
                                            drAttd["EarlyCome"] = sminearly;

                                        }
                                    }
                                }
                                daAttdData.Update(dsAttdData, "AttdData");
                            }
                        }

                        
                        #endregion Set_EarlyCome

                        #region Set_EarlyGoing
                        if (drAttd["ConsOut"] != DBNull.Value)
                        {

                            if(ShiftEnd > Convert.ToDateTime(drAttd["ConsOut"]))
                            {
                                TimeSpan t5 = (ShiftEnd - Convert.ToDateTime(drAttd["ConsOut"]));
                                if (t5.TotalSeconds < -660 && t5.TotalSeconds < 0)
                                {
                                    secgone = (Convert.ToDateTime(drAttd["ConsOut"]) - ShiftEnd).TotalSeconds;
                                    hurgone = Math.Truncate(secgone / 3600);
                                    mingone = Math.Truncate((secgone - (hurgone * 3600)) / 60);
                                    smingone = string.Format("{0:00}:{1:00}", hurgone, mingone);
                                    drAttd["EarlyGoing"] = smingone;

                                    if (eWrkGrp == "COMP" && eGradeCode <= 15)
                                    {
                                        drAttd["Halfday"] = 0;
                                        drAttd["EarlyGoing"] = "";

                                    }
                                    else if (secgone > 3600 && eWrkGrp == "COMP" && eGradeCode > 15 && drAttd["LeaveTyp"].ToString() != "")
                                    {
                                        drAttd["Halfday"] = 0;
                                        drAttd["EarlyGoing"] = "";
                                    }
                                    else if (secgone > 3600 && eWrkGrp == "COMP" && eGradeCode > 15 && drAttd["LeaveTyp"].ToString() == "" && drAttd["Status"].ToString() == "P")
                                    {
                                        drAttd["Halfday"] = 1;
                                    }
                                    else if (secgone > 3600 && eWrkGrp == "COMP" && eGradeCode > 15 && drAttd["LeaveTyp"].ToString() == "" && drAttd["Status"].ToString() == "A")
                                    {
                                        drAttd["Halfday"] = 0;
                                        drAttd["EarlyGoing"] = "";
                                    }
                                    else if (secgone > 3600 && eWrkGrp == "COMP")
                                    {
                                        drAttd["Halfday"] = 1;
                                    }

                                }
                            }
                            

                            daAttdData.Update(dsAttdData, "AttdData");
                        }

                        #endregion Set_EarlyGoing

                    }
                    else //if emp comes before schedule shift set early times
                    {

                        #region Set_EarlyCome1
                        if (tInTime < ShiftStart)
                        {
                            secearly = (tInTime - ShiftStart).TotalSeconds;
                            hurearly = Math.Truncate(secearly / 3600);
                            minearly = Math.Truncate((secearly - (hurearly * 3600)) / 60);
                            sminearly = string.Format("{0:00}:{1:00}", hurearly, minearly);
                            drAttd["EarlyCome"] = sminearly;

                            if (eWrkGrp == "COMP" && eGradeCode <= 15)
                            {
                                drAttd["Halfday"] = 0;
                                drAttd["Latecome"] = "";
                                drAttd["EarlyCome"] = "";
                            }
                            else if (secearly > 1800 && eWrkGrp == "COMP" && eGradeCode > 15 && drAttd["LeaveTyp"].ToString() != "")
                            {
                                drAttd["Halfday"] = 0;
                                drAttd["Latecome"] = "";
                                drAttd["EarlyCome"] = "";
                            }
                            else if (tInTime <= ShiftStart)
                            {
                                if (drAttd["ConsOut"] != DBNull.Value)
                                {
                                    if (Convert.ToDateTime(drAttd["ConsOut"]) >= ShiftEnd)
                                    {
                                        drAttd["Halfday"] = 0;
                                        drAttd["Status"] = "P";
                                        drAttd["EarlyCome"] = sminearly;

                                    }

                                }
                            }
                        }
                        #endregion Set_EarlyCome1

                        #region Set_EarlyGoing1

                        if (drAttd["ConsOut"] != DBNull.Value)
                        {
                            if (ShiftEnd > Convert.ToDateTime(drAttd["ConsOut"]))
                            {
                                TimeSpan t5 = (ShiftEnd - Convert.ToDateTime(drAttd["ConsOut"]));
                                if (t5.TotalSeconds < -660 && t5.TotalSeconds < 0)
                                {
                                    secgone = (Convert.ToDateTime(drAttd["ConsOut"]) - ShiftEnd).TotalSeconds;
                                    hurgone = Math.Truncate(secgone / 3600);
                                    mingone = Math.Truncate((secgone - (hurgone * 3600)) / 60);
                                    smingone = string.Format("{0:00}:{1:00}", hurgone, mingone);
                                    drAttd["EarlyGoing"] = smingone;

                                    if (eWrkGrp == "COMP" && eGradeCode <= 15)
                                    {
                                        drAttd["Halfday"] = 0;
                                        drAttd["EarlyGoing"] = "";
                                    }
                                    else if (secgone > 3600 && eWrkGrp == "COMP" && eGradeCode > 15 && drAttd["LeaveTyp"].ToString() != "")
                                    {
                                        drAttd["Halfday"] = 0;
                                        drAttd["EarlyGoing"] = "";
                                    }
                                    else if (secgone > 3600 && secgone <= 7200 && eWrkGrp == "COMP" && eGradeCode > 15 && drAttd["LeaveTyp"].ToString() == "" && drAttd["Status"].ToString() == "P")
                                    {
                                        drAttd["Halfday"] = 1;
                                    }
                                    else if (secgone > 3600 && secgone <= 7200 && eWrkGrp == "COMP" && eGradeCode > 15 && drAttd["LeaveTyp"].ToString() == "" && drAttd["Status"].ToString() == "A")
                                    {
                                        drAttd["Halfday"] = 0;
                                        drAttd["EarlyGoing"] = "";
                                    }
                                    else if (secgone > 3600 && secgone <= 7200 && eWrkGrp == "COMP")
                                    {
                                        drAttd["Halfday"] = 1;
                                    }
                                    //'check if early come BUT GOES BETWEEN 1HOUR OF SHIFTEND- ALLOW 20/07/2015.
                                    else if (Convert.ToDateTime(drAttd["ConsIn"]) < ShiftStart && secgone <= 3600 && eWrkGrp == "COMP")
                                    {

                                    }
                                    else if (Convert.ToDateTime(drAttd["ConsOut"]) < ShiftOutFrom && Convert.ToDouble(drAttd["ConsWrkHrs"]) >= 6)
                                    {
                                        drAttd["Halfday"] = 0;
                                        drAttd["EarlyGoing"] = "";
                                        drAttd["Status"] = "A";
                                    }
                                }
                            }
                        }

                        #endregion Set_EarlyGoing1

                        daAttdData.Update(dsAttdData, "AttdData");
                    }

                    #endregion Check_intime_if_follow_in_shiftintime

                    #endregion Setting_Late_Early

                    //'check if both punch found
                    //'then first check intime and outtime convers schedule shift
                    if (drAttd["ConsIn"] != DBNull.Value && drAttd["ConsIn"] != DBNull.Value)
                    {
                        if (Convert.ToDateTime(drAttd["ConsIn"]) < ShiftStart && Convert.ToDateTime(drAttd["ConsOut"]) > ShiftEnd)
                        {
                            drAttd["Halfday"] = 0;
                            drAttd["Latecome"] = "";
                            drAttd["EarlyCome"] = "";
                            drAttd["EarlyGoing"] = "";
                            drAttd["Status"] = "P";
                            daAttdData.Update(dsAttdData, "AttdData");
                        }
                    }

                    #region Reset_ShiftVars
                    seclate = 0;
                    hurlate = 0;
                    minlate = 0;
                    sminlate = "";

                    secearly = 0;
                    hurearly = 0;
                    sminearly = "";

                    secgone = 0;
                    hurgone = 0;
                    mingone = 0;
                    smingone = "";
                    #endregion Reset_ShiftVars


                    #endregion SchShiftCalc
                }


                #region OTCalc
            OTCalc:

                if (!tOTFLG)
                {
                    drAttd["ConsOverTime"] = 0;
                    drAttd["CalcOverTime"] = 0;
                    daAttdData.Update(dsAttdData, "AttdData");

                    return;
                }


                OverTime = 0;
                if (tOTFLG && (Convert.ToDouble(drAttd["ConsWrkHrs"])) > ShiftHrs && drAttd["LeaveTyp"].ToString() == "")
                {
                    TimeSpan t3 = (Convert.ToDateTime(drAttd["ConsOut"]) - ShiftEnd);
                    OverTime = t3.TotalSeconds;
                    double othrs = 0, otmin = 0;
                    double ot = 0;

                    othrs = Math.Truncate(OverTime / 3600);
                    otmin = Math.Truncate((OverTime - (othrs * 3600)) / 60);
                    //otmin = ((int)OverTime - (othrs * 60));
                    ot = othrs;

                    if (otmin >= 21 && otmin <= 50)
                    {
                        ot = othrs + 0.5;
                    }
                    else if (otmin > 50 && otmin <= 59)
                    {
                        ot = othrs + 1;
                    }

                    if (ot >= 1)
                    {
                        ot = ot - ShiftBreak;
                    }

                    if (ot >= 1.5)
                    {
                        if (drAttd["Latecome"].ToString() != "")
                        {
                            int thr = Convert.ToInt32(drAttd["Latecome"].ToString().Substring(0, 2));
                            int tmn = Convert.ToInt32(drAttd["Latecome"].ToString().Substring(4, 2));

                            ot = ot - thr;
                            if (tmn >= 15 && tmn <= 40)
                                ot = ot - 0.5;
                            else if (tmn >= 41 && tmn <= 59)
                                ot = ot - 1;

                        }

                        drAttd["ConsOverTime"] = ot;
                        drAttd["CalcOvertime"] = ot;
                    }
                    else
                    {
                        drAttd["ConsOverTime"] = 0;
                        drAttd["CalcOvertime"] = 0;
                    }
                }
                else if (tOTFLG && drAttd["LeaveTyp"].ToString() == "WO")
                {
                    OverTime = (Convert.ToDouble(drAttd["ConsWrkHrs"]) - BreakHours);
                    if (OverTime >= 1.5)
                    {
                        drAttd["ConsOverTime"] = Convert.ToInt32(OverTime);
                        drAttd["CalcOvertime"] = Convert.ToInt32(OverTime);
                    }
                }
                else if (tOTFLG && drAttd["LeaveTyp"].ToString() == "HL")
                {
                    OverTime = (Convert.ToDouble(drAttd["ConsWrkHrs"]) - BreakHours);
                    if (OverTime >= 1.5)
                    {
                        drAttd["ConsOverTime"] = Convert.ToInt32(OverTime);
                        drAttd["CalcOvertime"] = Convert.ToInt32(OverTime);

                    }
                }
                else
                {
                    drAttd["ConsOverTime"] = 0;
                    drAttd["CalcOvertime"] = 0;

                }

                //'added on 14-02-2014
                if (Convert.ToBoolean(drAttd["Halfday"]))
                {
                    drAttd["ConsOverTime"] = 0;
                    drAttd["CalcOvertime"] = 0;
                }

                if (drAttd["Status"].ToString() == "A")
                {
                    drAttd["ConsOverTime"] = 0;
                    drAttd["CalcOvertime"] = 0;
                }
                daAttdData.Update(dsAttdData, "AttdData");

                #endregion OTCalc

                err = string.Empty;
                return;

            } //end SetShift
            catch (Exception ex)
            {
                err = "Error in SetShift-> :" + ex.ToString();
                return;
            }

        }

    }

    public static class TimeSpanExtensions
    {
        public static TimeSpan RoundToNearestMinutes(this TimeSpan input, int minutes)
        {
            var totalMinutes = (int)(input + new TimeSpan(0, minutes / 2, 0)).TotalMinutes;

            return new TimeSpan(0, totalMinutes - totalMinutes % minutes, 0);
        }
    }
}
