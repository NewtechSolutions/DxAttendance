﻿using System;
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
        private SqlDataAdapter daAttdData;
        private SqlCommandBuilder AttdCmdBuilder;

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
                    if (!Emp.GetEmpDetails( Emp.EmpUnqID))
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
                    if (!Emp.GetEmpDetails(Emp.EmpUnqID))
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

        public void GateInOutProcess(string tEmpUnqID, DateTime tFromDt, out int result)
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
                    if (!Emp.GetEmpDetails(Emp.EmpUnqID))
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
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "GateInOutProcess";

                        SqlParameter spout = new SqlParameter();
                        spout.Direction = ParameterDirection.Output;
                        spout.DbType = DbType.Int32;
                        spout.ParameterName = "@result";
                        int tout = 0;
                        spout.Value = tout;

                        tFromDt = tFromDt.AddHours(0).AddMinutes(1);
                        cmd.Parameters.AddWithValue("@pEmpUnqID", Emp.EmpUnqID);
                        cmd.Parameters.AddWithValue("@pFromDt", tFromDt);
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

        public void EmpCostCodeRpt_Process(string tEmpUnqID, DateTime tFromDt, out int result, out string err)
        {
            result = 0;
            err = string.Empty;
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

            #endregion chk_primary

            //call main store proce.
            using (SqlConnection cn = new SqlConnection(Utils.Helper.constr))
            {
                try
                {

                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = cn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "sp_rpt_CostCodeDailyEmpManPower";

                        cmd.Parameters.AddWithValue("@pDate", tFromDt);
                        cmd.Parameters.AddWithValue("@pEmpUnqId", tEmpUnqID);
                        cmd.Parameters.Add("@result", SqlDbType.Int, 4);
                        cmd.Parameters["@result"].Direction = ParameterDirection.Output;
                        cmd.CommandTimeout = 0;
                        cmd.ExecuteNonQuery();

                        //get the output
                        result = Convert.ToInt32(cmd.Parameters["@result"].Value.ToString());

                        err = string.Empty;

                    }

                }
                catch (Exception ex)
                {
                    err = ex.Message.ToString();
                    result = 0;
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
            string newwooterr = string.Empty;
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
                    
                    Emp.EmpUnqID = tEmpUnqID;

                    //check employee status
                    if (!Emp.GetEmpDetails( Emp.EmpUnqID))
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
                            bool ChkSanInOut = false;

                            //Open EmpAttdRecord
                            sql = "Select tYear,tDate,CompCode,WrkGrp,EmpUnqID,ScheDuleShift,ConsShift,ConsIN,ConsOut,ConsWrkHrs,ConsOverTime," +
                                "Status,HalfDay,LeaveTyp,LeaveHalf,ActualStatus,Earlycome,EarlyGoing,GracePeriod," +
                                "INPunch1,OutPunch1,WrkHrs1,INPunch2,OutPunch2,WrkHrs2," +
                                " TotalWorkhrs,TotalINPunchCount," +
                                "TotalOutPunchCount,LateCome,Rules,CalcOverTime,HalfDRule,ActualShift " +
                                " from AttdData where tYear ='" + drDate["CalYear"].ToString() +
                                "' and EmpUnqID = '" + Emp.EmpUnqID + "' and tDate ='" + Convert.ToDateTime(drDate["Date"]).ToString("yyyy-MM-dd") + "'";
                               

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
                                        ChkSanInOut = true;
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
                                        ChkSanInOut = true;
                                    }
                                    else
                                    {
                                        drAttd["ConsOut"] = DBNull.Value;
                                    }

                                    daAttdData.Update(dsAttdData, "AttdData");

                                    #endregion SanctionOutTime

                                    if (!ChkSanInOut)
                                    {

                                        #region Check_For_Continueous_Duty
                                        bool chk_continue_duty = false;

                                        if (drAttd["InPunch1"] != DBNull.Value && drAttd["OutPunch1"] != DBNull.Value && drAttd["InPunch2"] != DBNull.Value && drAttd["OutPunch2"] != DBNull.Value)
                                        {
                                            if (Convert.ToDateTime(drAttd["InPunch1"]).ToString("yyyy-MM-dd")
                                                == Convert.ToDateTime(drAttd["InPunch2"]).ToString("yyyy-MM-dd"))
                                            {
                                                TimeSpan outdiff1 = Convert.ToDateTime(drAttd["InPunch2"]) - Convert.ToDateTime(drAttd["OutPunch1"]);
                                                if (outdiff1.TotalMinutes <= 60 && outdiff1.TotalMinutes >= 0)
                                                {
                                                    TimeSpan outdiff2 = Convert.ToDateTime(drAttd["OutPunch1"]) - Convert.ToDateTime(drAttd["InPunch1"]);
                                                    if (outdiff2.TotalHours >= 6)
                                                    {
                                                        drAttd["ConsIn"] = drAttd["InPunch1"];
                                                        drAttd["ConsOut"] = drAttd["OutPunch2"];
                                                        daAttdData.Update(dsAttdData, "AttdData");
                                                        chk_continue_duty = true;
                                                    }
                                                }
                                            }
                                        }
                                        #endregion Check_For_Continueous_Duty

                                        #region Check_For_Maximum_Hours
                                        if (!chk_continue_duty)
                                        {
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
                                        }
                                        
                                        #endregion Check_For_Maximum_Hours
                                    }

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
                                        
                                        if(drAttd["LeaveTyp"].ToString() == "WO" || drAttd["LeaveTyp"].ToString() == "PH" )
                                        {
                                            drAttd["LeaveHalf"] = 0;
                                        }

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

                                        CalcShiftOT(daAttdData, dsAttdData, drAttdCopy, Emp, dr["ConsShift"].ToString(),out shifterr);
                                    }
                                    else
                                    {
                                        DataRow drAttdCopy = drAttd;
                                        CalcShiftOT(daAttdData, dsAttdData, drAttdCopy, Emp, drAttd["ScheduleShift"].ToString(), out shifterr);
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
                                            case "PH":
                                                drAttd["LateCome"] = "";
                                                drAttd["EarlyGoing"] = "";
                                                drAttd["EarlyCome"] = "";
                                                break;
                                            default:
                                                if (drAttd["LeaveTyp"].ToString() != "")
                                                {
                                                    if (drAttd["ConsIn"] is DateTime && Convert.ToBoolean(drAttd["LeaveHalf"]))
                                                    {
                                                        drAttd["HalfDay"] = 1;
                                                    }
                                                    else if (drAttd["ConsIn"] is DateTime && !Convert.ToBoolean(drAttd["LeaveHalf"]))
                                                    {
                                                        drAttd["HalfDay"] = 0;
                                                    }
                                                }else
                                                {
                                                    if (Emp.WrkGrp != "COMP" && drAttd["Status"].ToString() == "P")
                                                    {
                                                        if (tconswrkhrs < 6 )
                                                        {
                                                            drAttd["Status"] = "A";
                                                            drAttd["halfday"] = 0;
                                                            drAttd["LateCome"] = "";
                                                            drAttd["EarlyGoing"] = "";
                                                            drAttd["EarlyCome"] = "";
                                                        }
                                                    }
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
                                            case "PH":
                                                drAttd["LateCome"] = "";
                                                drAttd["EarlyGoing"] = "";
                                                drAttd["EarlyCome"] = "";
                                                break;
                                            default:
                                                if (drAttd["LeaveTyp"].ToString() != "")
                                                {
                                                    if (drAttd["ConsIn"] is DateTime && Convert.ToBoolean(drAttd["LeaveHalf"]))
                                                    {
                                                        drAttd["Status"] = "A";
                                                        drAttd["HalfDay"] = 0;
                                                    }
                                                    else if (drAttd["ConsIn"] is DateTime && !Convert.ToBoolean(drAttd["LeaveHalf"]))
                                                    {
                                                        drAttd["HalfDay"] = 0;
                                                    }
                                                }
                                                else
                                                {
                                                    
                                                        drAttd["Status"] = "A";
                                                        drAttd["HalfDay"] = 0;
                                                        drAttd["LateCome"] = "";
                                                        drAttd["EarlyGoing"] = "";
                                                        drAttd["EarlyCome"] = "";
                                                        
                                                    
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

                                        //added on dt 2020-09-30
                                        if (Convert.ToBoolean(drAttd["LeaveHalf"]) 
                                            &&  tconswrkhrs <= 0 
                                            && drAttd["ConsIn"] == DBNull.Value
                                            && drAttd["ConsOut"] == DBNull.Value                                            
                                            )
                                        {
                                            drAttd["HalfDay"] = 1;
                                        }

                                        if (Convert.ToBoolean(drAttd["LeaveHalf"])
                                            && tconswrkhrs >= 0 && tconswrkhrs <= 4                                             
                                            )
                                        {
                                            drAttd["Status"] = "P";
                                            drAttd["HalfDay"] = 1;
                                        }

                                        if (Convert.ToBoolean(drAttd["LeaveHalf"])
                                            && tconswrkhrs > 4                                            
                                            )
                                        {
                                            drAttd["Status"] = "P";
                                            drAttd["HalfDay"] = 0;
                                        }


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

                                    //bugfix : 07/04/2018 : Reset Sanction ot if status is absent and makesure to round it
                                    if (drAttd["Status"].ToString() == "A")
                                    {
                                        drAttd["ConsOverTime"] = 0;
                                        drAttd["GracePeriod"] = "";
                                    }
                                    //else
                                    //{
                                    //   //bugfix : 25-05-2018 : consider sanctioned overtime on od
                                    //    if (drAttd["LeaveTyp"].ToString() != "OD")
                                    //    {
                                    //        //bugfix : 02/05/2018 : Sanction Ot on WO also considered if employee did not come.
                                    //        if (!string.IsNullOrEmpty(drAttd["ConsIn"].ToString()) && !string.IsNullOrEmpty(drAttd["ConsOut"].ToString()))
                                    //        {
                                    //            double Overtime = 0;
                                    //            Overtime = Convert.ToDouble(drAttd["ConsOverTime"]);

                                    //            if (Overtime >= 1)
                                    //            {
                                    //                drAttd["ConsOverTime"] = Math.Truncate(Overtime);
                                    //            }
                                    //        }
                                    //        else
                                    //        {
                                    //            drAttd["ConsOverTime"] = 0;
                                    //        }
                                    //    }
                                    //    //02/05/2018

                                    //}

                                    ////for trying to to single process 
                                    //if (Emp.WrkGrp == "CONT")
                                    //{
                                    //    if(drAttd["ConsIn"] != DBNull.Value && drAttd["ConsOut"] != DBNull.Value 
                                    //        && Convert.ToDouble(drAttd["ConsWrkHrs"]) > 6 
                                    //        && Convert.ToBoolean(drAttd["HalfDay"]) == false
                                    //        && drAttd["LeaveTyp"].ToString() == "AB"
                                    //        )
                                    //    {
                                    //        drAttd["LeaveTyp"] = "";
                                    //        drAttd["Status"] = "P";
                                    //    }
                                    //}
                                    daAttdData.Update(dsAttdData, "AttdData");

                                    #endregion Final_Status_Marking

                                    /* blocked

                                    #region TripodDataProcess
                                        int tRet = 0;
                                        TripodDataProcess(tEmpUnqID, Convert.ToDateTime(drDate["Date"]), Convert.ToDateTime(drDate["Date"]), out tRet);
                                        
                                    #endregion

                                    //new requirement mailed 05/05/2020 
                                    //shiftwise/costcenter wise manpower with based on stdshift hours
                                    //required two new additional fields in attd data table
                                    #region StdShift_wise_OverTime
                                        Calc_StdHrs(daAttdData, dsAttdData, drAttd);  
                                    #endregion



                                    #region GateInOutProcess_CONT
                                    if (Emp.WrkGrp == "CONT")
                                    {  
                                        //NEW PROCESS DI SEGMENT - SCHEDULED EMP
                                        //PROVIDE WO -> 8 HRS OT - IF NOT ACTUALLY COMES
                                        //->A;WO;A ->RULE SHOULD BE APPLIED
                                        //->Ref : Mail - Dated : 24/7/21 - K.K. Giri

                                        ContScheduledManWO_OTCalc(tEmpUnqID, drAttd, out newwooterr);
                                        
                                        int res = 0;
                                        GateInOutProcess(Emp.EmpUnqID, Convert.ToDateTime(drDate["Date"]), out res);

                                    }
                                    #endregion GateInOutProcess_CONT

                                    ***////blocked/


                                }// AttdDataLoop

                            }//AttdData DataSet


                        }//Date loop
                    }//Date DataSet

                    result = 1;

                    if (!string.IsNullOrEmpty(shifterr))
                    {
                        err = err + shifterr;
                    }

                    if (!string.IsNullOrEmpty(newwooterr))
                    {
                        err = err + newwooterr;
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


        public void ContScheduledManWO_OTCalc(string tEmpUnqID, DataRow drAttd,out string err)
        {
             err = string.Empty;
             string err4;
             DateTime curDt = Convert.ToDateTime(drAttd["tDate"]);
             SqlConnection cn = new SqlConnection(Utils.Helper.constr);
             try
             {
                 cn.Open();
             }
             catch (Exception ex)
             {
                 err = ex.ToString();
                 return;
             }

            if (curDt >= Convert.ToDateTime("2021-07-26"))
            {
                DateTime nxtday = curDt.Date.AddDays(1);
                DateTime preday = curDt.Date.AddDays(-1);

                DataSet dsNxt = Utils.Helper.GetData("Select * from AttdData Where Empunqid ='" + tEmpUnqID + "' and tDate = '" + nxtday.Date.ToString("yyyy-MM-dd") + "'", Utils.Helper.constr, out err4);
                if (!string.IsNullOrEmpty(err4))
                {
                    err += err4;
                    return;
                }
                
                DataSet dsPrv = Utils.Helper.GetData("Select * from AttdData Where Empunqid ='" + tEmpUnqID + "' and tDate = '" + preday.Date.ToString("yyyy-MM-dd") + "'", Utils.Helper.constr, out err4);
                if (!string.IsNullOrEmpty(err4))
                {
                    err += err4;
                    return;
                }
                
                bool prvhasrow = dsPrv.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);
                if (prvhasrow)
                {
                    DataRow drprv = dsPrv.Tables[0].Rows[0];
                    DataRow drnxt = dsNxt.Tables[0].Rows[0];

                    string prvschshift = drprv["ScheduleShift"].ToString();

                    string prvstatus = drprv["Status"].ToString();
                    string curSchShift = drAttd["ScheduleShift"].ToString();
                    string nxtSchShift = drnxt["ScheduleShift"].ToString();
                    string curStatus = drAttd["Status"].ToString();
                    string nxtStatus = drnxt["Status"].ToString();

                    if (!string.IsNullOrEmpty(prvschshift) && !string.IsNullOrEmpty(curSchShift) && !string.IsNullOrEmpty(nxtSchShift))
                    {
                        if ((curSchShift == "WO" && prvstatus == "P" && nxtStatus == "P") || 
                            (curSchShift == "WO" && prvstatus == "A" && nxtStatus == "P") ||
                            (curSchShift == "WO" && prvstatus == "P" && nxtStatus == "A")
                            )
                        {
                            //check sanction ot on wo days
                            string tsql = "Select top 1 * from MastLeaveSchedule where Empunqid ='" + tEmpUnqID + "' and tDate ='" + curDt.Date.ToString("yyyy-MM-dd") + "' and ConsOverTime is not null and SchLeave is null order by sanid desc";
                            DataSet dsSanWo = Utils.Helper.GetData(tsql, Utils.Helper.constr, out err4);
                            if (!string.IsNullOrEmpty(err4))
                            {
                                err += err4;
                                return;
                            }
                            bool sanhasrow = dsSanWo.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);
                            if (!sanhasrow)
                            {
                                //insert sanction overtime to 8 hrs
                                //and set consovertime 8 hrs

                                using (SqlCommand cmd1 = new SqlCommand())
                                {
                                    try
                                    {
                                        
                                        cmd1.Connection = cn;
                                        string sql1 = "Insert Into MastLeaveSchedule (tDate,EmpUnqID,WrkGrp,ConsOverTime,Remarks,AddId,AddDt) Values (" +
                                        " '" + curDt.Date.ToString("yyyy-MM-dd") + "','" + tEmpUnqID + "','" + drAttd["WrkGrp"].ToString() + "',8,'8hr day off','Auto',GetDate())";
                                        cmd1.CommandText = sql1;
                                        cmd1.CommandTimeout = 0;
                                        cmd1.ExecuteNonQuery();

                                        sql1 = "Update AttdData Set ConsOverTime = 8 where EmpUnqid = '" + tEmpUnqID + "' and tDate ='" + curDt.Date.ToString("yyyy-MM-dd") + "' ";
                                        cmd1.CommandText = sql1;
                                        cmd1.CommandTimeout = 0;
                                        cmd1.ExecuteNonQuery();


                                    }
                                    catch (Exception ex)
                                    {
                                        err += ex.ToString();
                                        return;
                                    }
                                }

                            }
                            else
                            {
                                //update sanction overtime to 8 hrs
                                DataRow drsan = dsSanWo.Tables[0].Rows[0];
                                using (SqlCommand cmd1 = new SqlCommand())
                                {
                                    try
                                    {
                                        cmd1.Connection = cn;
                                        //string sql1 = "update MastLeaveSchedule set ConsOverTime = 8 where SanId = '" + drsan["SanID"].ToString() + "'";
                                        //cmd1.CommandText = sql1;
                                        //cmd1.CommandTimeout = 0;
                                        //cmd1.ExecuteNonQuery();
                                        string sql1 = string.Empty;
                                        sql1 = "Update AttdData Set ConsOverTime = '" + drsan["ConsOverTime"].ToString() + "' where EmpUnqid = '" + tEmpUnqID + "' and tDate ='" + curDt.Date.ToString("yyyy-MM-dd") + "' ";
                                        cmd1.CommandText = sql1;
                                        cmd1.CommandTimeout = 0;
                                        cmd1.ExecuteNonQuery();

                                    }
                                    catch (Exception ex)
                                    {
                                        err += ex.ToString();
                                        return;
                                    }
                                }

                            }

                        }
                        
                        else if (curSchShift == "WO" && prvstatus == "A" && nxtStatus == "A")
                        {
                            //delete if exist
                            string tsql = "Select top 1 * from MastLeaveSchedule where Empunqid ='" + tEmpUnqID + "' and tDate ='" + curDt.Date.ToString("yyyy-MM-dd") + "' and isnull(ConsOverTime,0) > 0 and SchLeave is null and AddId ='auto'";
                            DataSet dsSanWo = Utils.Helper.GetData(tsql, Utils.Helper.constr, out err4);

                            if (!string.IsNullOrEmpty(err4))
                            {
                                err += err4;
                                return;
                            }

                            bool sanhasrow = dsSanWo.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);
                            if (sanhasrow)
                            {
                                //update sanction overtime to 8 hrs
                                DataRow drsan = dsSanWo.Tables[0].Rows[0];
                                using (SqlCommand cmd1 = new SqlCommand())
                                {
                                    try
                                    {
                                        cmd1.Connection = cn;
                                        string sql1 = "delete from MastLeaveSchedule where SanId = '" + drsan["SanID"].ToString() + "'";
                                        cmd1.CommandText = sql1;
                                        cmd1.CommandTimeout = 0;
                                        cmd1.ExecuteNonQuery();

                                        sql1 = "Update AttdData Set ConsOverTime = 0 , Status = 'A',LeaveTyp = '' where EmpUnqid = '" + tEmpUnqID + "' tDate ='" + curDt.Date.ToString("yyyy-MM-dd") + "' ";
                                        cmd1.CommandText = sql1;
                                        cmd1.CommandTimeout = 0;
                                        cmd1.ExecuteNonQuery();


                                    }
                                    catch (Exception ex)
                                    {
                                        err += ex.ToString();
                                        return;
                                    }
                                }
                            }
                            else
                            {

                                //mark status A, ot 0
                                using (SqlCommand cmd1 = new SqlCommand())
                                {
                                    try
                                    {
                                        cmd1.Connection = cn;
                                        string sql1 = "Update AttdData Set ConsOverTime = 0, Status = 'A', LeaveTyp = '' where EmpUnqid = '" + tEmpUnqID + "' and tDate ='" + curDt.Date.ToString("yyyy-MM-dd") + "' ";
                                        cmd1.CommandText = sql1;
                                        cmd1.CommandTimeout = 0;
                                        cmd1.ExecuteNonQuery();


                                    }
                                    catch (Exception ex)
                                    {
                                        err += ex.ToString();
                                        return;
                                    }
                                }

                            }
                        }
                    }//end of shceduled contract employee
                }

            }//main add cont 8hrs function
        }


        public void TripodDataProcess(string tEmpUnqID, DateTime tFromDt, DateTime tToDate, out int result)
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
                    
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = cn;
                     
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "sp_Tripod_Data_Process";

                        //SqlParameter spout = new SqlParameter();
                        //spout.Direction = ParameterDirection.Output;
                        //spout.DbType = DbType.Int32;
                        //spout.ParameterName = "@result";
                        //int tout = 0;
                        //spout.Value = tout;

                        //tFromDt = tFromDt.AddHours(-2).AddMinutes(1);
                        //tToDate = tFromDt.AddHours(18).AddMinutes(59).AddSeconds(59);

                        cmd.Parameters.AddWithValue("@pEmpUnqID", tEmpUnqID);
                        cmd.Parameters.AddWithValue("@pDate", tFromDt);
                        //cmd.Parameters.AddWithValue("@pToDt", tToDate);
                        //cmd.Parameters.Add(spout);
                        cmd.CommandTimeout = 0;
                        cmd.ExecuteNonQuery();
                        result = 1;
                        //get the output
                        //result = (int)cmd.Parameters["@result"].Value;



                    }

                }
                catch (Exception ex)
                {
                    
                }

            }//using connection
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
                    //clsEmp Emp = new clsEmp();
                    //Emp.CompCode = "01";
                    //Emp.EmpUnqID = tEmpUnqID;

                    //check employee status
                    //if (!Emp.GetEmpDetails(Emp.CompCode, Emp.EmpUnqID))
                    //{
                    //    return;

                    //}
                    //else
                    //{
                    //    //if not active 
                    //    if (!Emp.Active)
                    //        return;
                    //}

                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = cn;
                        //cmd.CommandType = CommandType.Text;
                        //string sql = "Insert into ProcessLog (AddDt,AddId,ProcessType,FromDt,ToDt,EmpUnqID ) Values (" +
                        //    " GetDate(),'" + Utils.User.GUserID + "','MessProcess','" + tFromDt.ToString("yyyy-MM-dd") + "'," +
                        //    " '" + tToDate.ToString("yyyy-MM-dd") + "','" + tEmpUnqID + "')";
                        //cmd.CommandText = sql;
                        //cmd.ExecuteNonQuery();

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

                        cmd.Parameters.AddWithValue("@pEmpUnqID", tEmpUnqID);
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

        public void ArrivalProcess(DateTime tFromDt, DateTime tToDate, out int result)
        {
            result = 0;

            
            if (tToDate < tFromDt)
            {
                return;
            }

            //call main store proce.
            using (SqlConnection cn = new SqlConnection(Utils.Helper.constr))
            {
                try
                {
                    
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = cn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "Attd_Process_Arrival";

                        cmd.Parameters.AddWithValue("@pFromDt", tFromDt);
                        cmd.Parameters.AddWithValue("@pToDt", tToDate);
                        cmd.Parameters.Add("@result",SqlDbType.Int,4);
                        cmd.Parameters["@result"].Direction = ParameterDirection.Output;                        
                        cmd.CommandTimeout = 0;
                        cmd.ExecuteNonQuery();

                        //get the output
                        result = Convert.ToInt32(cmd.Parameters["@result"].Value.ToString());



                    }

                }
                catch (Exception ex)
                {
                    result = 0;
                }

            }//using connection
        }

        public void CalcShiftOT(SqlDataAdapter daAttdData, DataSet dsAttdData, DataRow drAttd, clsEmp Emp, string tSchShift, out string err)
        {
            err = string.Empty;
            try
            {
                Globals.GetDateWiseGlobalVars(Convert.ToDateTime(drAttd["tDate"]));

                // this Function will set EarlyCome,Latecome,EarlyGoing,HalfDay,Shift,OverTime
                #region Setting_Vars

                string tShift = string.Empty;
                string sEmpCode = Emp.EmpUnqID;
                string eWrkGrp = string.Empty;
                int eGradeCode = 0;
                DateTime tDate = Convert.ToDateTime(drAttd["tDate"]);
                DateTime tInTime;
                DateTime? tOutTime = new DateTime?();
                //used for LateCount Exception
                Boolean tLateExp = false;
                //check if Emp has LateCome exception
                int tcnt = Convert.ToInt32(Utils.Helper.GetDescription("Select Count(*) From MastException Where EmpUnqID ='" + drAttd["EmpUnqID"].ToString() + "' and ExecLateCome = 1", Utils.Helper.constr));
                if (tcnt > 0)
                {
                    tLateExp = true;
                }

                Emp = new clsEmp();
                Emp.GetEmpDetails(sEmpCode,tDate);

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
                if (tShift == "WO" || tShift == "PH")
                {
                    tShift = "";
                }


                if (eWrkGrp == "COMP")
                {
                    int t = 0;

                    if (int.TryParse(Emp.GradeCode, out t))
                    {
                        eGradeCode = t;
                    }
                    else
                    {
                        eGradeCode = 999999;
                    }

                }
                else
                {
                    eGradeCode = 0;
                }


                //if (eWrkGrp == "COMP")
                //{
                //    eGradeCode = Convert.ToInt32(Emp.GradeCode);
                //}
                //else
                //{
                //    eGradeCode = 0;
                //}

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
                double secgone = 0, hurgone = 0, mingone = 0, restsecgone = 0 ;
                string smingone = "";

                //for grace period
                double secgrace = 0, hurgrace = 0, mingrace = 0;
                string smingrace = "";

                #endregion Setting_Vars

                drAttd["ActualShift"] = Globals.GetActualShift(Convert.ToDateTime(drAttd["ConsIn"]));

                if (drAttd["ConsIN"] is DateTime && tShift == "")
                {
                    #region AutoSiftCalc
                    // Create DataView
                    DataView dvShift = new DataView(Globals.dtShift);
                    dvShift.Sort = "ShiftSeq ASC";

                    foreach (DataRowView drShift in dvShift)
                    {

                        #region Set_shiftvars
                        ShiftHrs = Convert.ToDouble(drShift["ShiftHrs"].ToString());
                        ShiftBreak = Convert.ToDouble(drShift["BreakHrs"].ToString());

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
                            
                            if (tInTime > ShiftStart)
                            {
                            
                                #region grace
                                ////grace period
                                if (tDiff.TotalSeconds > 0)
                                {
                                    secgrace += tDiff.TotalSeconds;
                                    hurgrace = Math.Truncate(secgrace / 3600);
                                    mingrace = Math.Truncate((secgrace - (hurgrace * 3600)) / 60);
                                    smingrace = string.Format("{0:00}:{1:00}", hurgrace, mingrace);                                   
                                }
                                if (secgrace > Globals.G_GracePeriodSec)
                                {
                                    drAttd["GracePeriod"] = smingrace;
                                    if (drAttd["LeaveTyp"].ToString() != "")
                                    {
                                        drAttd["GracePeriod"] = "";
                                    }
                                }//new development for dynamic halfday
                                else
                                {
                                    drAttd["GracePeriod"] = "";
                                }

                                if (eWrkGrp == "COMP" && eGradeCode <= Globals.G_GlobalGradeExclude)
                                {
                                    drAttd["GracePeriod"] = "";
                                }

                                #endregion grace

                                if (tDiff.TotalSeconds >= Globals.G_LateComeSec)
                                {

                                    seclate = tDiff.TotalSeconds;
                                    if (seclate > 1 && seclate <= 59)
                                        seclate = 60;

                                    hurlate = Math.Truncate(seclate / 3600);
                                    minlate = Math.Truncate((seclate - (hurlate * 3600)) / 60);
                                    sminlate = string.Format("{0:00}:{1:00}", hurlate, minlate);
                                    
                                        drAttd["LateCome"] = sminlate;
                                    
                                    //new development for dynamic halfday
                                    if (Globals.G_HFFLG_LateCome)
                                    {
                                        //'added on 14-02-2014 if late more than 30 min mark halfday exclude agm and above on comp emp.
                                        if (eWrkGrp == "COMP" && eGradeCode <= Globals.G_GlobalGradeExclude)
                                        {
                                            drAttd["Halfday"] = 0;
                                            drAttd["Latecome"] = "";
                                            drAttd["EarlyCome"] = "";
                                            drAttd["GracePeriod"] = "";

                                        }
                                        else if (seclate > Globals.G_HFSEC_LateCome && eWrkGrp == "COMP" && eGradeCode > Globals.G_GlobalGradeExclude && drAttd["LeaveTyp"].ToString() != "")
                                        {
                                            drAttd["Halfday"] = 0;
                                            drAttd["Latecome"] = "";
                                            drAttd["EarlyCome"] = "";
                                            drAttd["GracePeriod"] = "";

                                        }
                                        else if ((seclate > Globals.G_HFSEC_LateCome && seclate < 5400 && eWrkGrp == "COMP" && eGradeCode > Globals.G_GlobalGradeExclude && drAttd["LeaveTyp"].ToString() == ""))
                                        {
                                            drAttd["Halfday"] = 1;
                                            drAttd["Latecome"] = "";
                                            drAttd["GracePeriod"] = "";

                                        }
                                        else if (seclate > 5400 && eWrkGrp == "COMP" && eGradeCode > Globals.G_GlobalGradeExclude && drAttd["LeaveTyp"].ToString() == "")
                                        {
                                            drAttd["Halfday"] = 0;
                                            drAttd["Status"] = "A";
                                            drAttd["Latecome"] = "";
                                            drAttd["GracePeriod"] = "";
                                        }
                                        else if (seclate > 5400 && eWrkGrp == "COMP")
                                        {
                                            drAttd["Halfday"] = 0;
                                            drAttd["Status"] = "A";
                                            drAttd["Latecome"] = "";
                                            drAttd["GracePeriod"] = "";
                                        }


                                    }//new development for dynamic halfday

                                    daAttdData.Update(dsAttdData, "AttdData");
                                }
                            }
                            
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
                                            ShiftHrs = Convert.ToDouble(tdr["Shifthrs"]);
                                            ShiftEnd = ShiftStart.AddHours(ShiftHrs);
                                            ShiftBreak = Convert.ToDouble(tdr["BreakHrs"]);
                                        }
                                        daAttdData.Update(dsAttdData, "AttdData");
                                    }

                                }
                                #endregion Fix_For_GI_SMG

                                #region Set_EarlyGoing

                                TimeSpan ts = (Convert.ToDateTime(drAttd["ConsOut"]) - ShiftEnd );

                                #region grace

                                ///Removed Grace Period From Out Time as per Mail of Vallabh Bhai
                                ///Date : 04/03/2019

                                //if (Convert.ToDateTime(drAttd["ConsOut"]) < ShiftEnd)
                                //{
                                //    if (ts.TotalSeconds < 0)
                                //    {
                                //        secgrace += Math.Abs(ts.TotalSeconds);
                                //        hurgrace = Math.Truncate(secgrace / 3600);
                                //        mingrace = Math.Truncate((secgrace - (hurgrace * 3600)) / 60);
                                //        smingrace = string.Format("{0:00}:{1:00}", hurgrace, mingrace);

                                //    }
                                //    if (secgrace > Globals.G_GracePeriodSec)
                                //    {
                                //        drAttd["GracePeriod"] = smingrace;
                                //        if (drAttd["LeaveTyp"].ToString() != "")
                                //        {
                                //            drAttd["GracePeriod"] = "";
                                //        }

                                //    }//new development for dynamic halfday
                                //    else
                                //    {
                                //        drAttd["GracePeriod"] = "";
                                //    }

                                //}

                                //removed : 04/03/2019

                                if (eWrkGrp == "COMP" && eGradeCode <= Globals.G_GlobalGradeExclude)
                                {
                                    drAttd["GracePeriod"] = "";
                                }

                                #endregion
                                
                                if (ts.TotalSeconds < (-1 * Globals.G_EarlyGoingSec) && ts.TotalSeconds < 0)
                                {
                                    TimeSpan t1 =  ShiftEnd - Convert.ToDateTime(drAttd["ConsOut"]) ;

                                    secgone = t1.TotalSeconds;
                                    
                                    //if (secgone > 1 && secgone <= 59)
                                    //    secgone = 60;

                                    hurgone = Math.Truncate(secgone / 3600);
                                    mingone = Math.Truncate((secgone - (hurgone * 3600)) / 60);
                                    //smingone = string.Format("{0:00}:{1:00}", hurgone, mingone);
                                    restsecgone = (secgone - ((hurgone * 3600) + (mingone * 60)));
                                    smingone = string.Format("{0:00}:{1:00}:{2:00}", hurgone, mingone, restsecgone);

                                    drAttd["EarlyGoing"] = smingone;


                                    //
                                    if (Globals.G_HFFLG_EarlyGoing)
                                    {
                                        if (eWrkGrp == "COMP" && eGradeCode <= Globals.G_GlobalGradeExclude)
                                        {
                                            drAttd["Halfday"] = 0;
                                            drAttd["EarlyGoing"] = "";
                                        }
                                        else if (secgone > Globals.G_HFSEC_EarlyGoing && eWrkGrp == "COMP" && eGradeCode > Globals.G_GlobalGradeExclude && drAttd["LeaveTyp"].ToString() != "")
                                        {
                                            drAttd["Halfday"] = 0;
                                            drAttd["EarlyGoing"] = "";
                                            drAttd["GracePeriod"] = "";
                                        }
                                        else if (secgone > Globals.G_HFSEC_EarlyGoing && secgone <= 7200 && eWrkGrp == "COMP" && eGradeCode > Globals.G_GlobalGradeExclude && drAttd["LeaveTyp"].ToString() == "" && drAttd["Status"].ToString() == "P")
                                        {
                                            drAttd["Halfday"] = 1;
                                            drAttd["GracePeriod"] = "";
                                            drAttd["EarlyGoing"] = "";
                                        }
                                        else if (secgone > Globals.G_HFSEC_EarlyGoing && secgone <= 7200 && eWrkGrp == "COMP" && eGradeCode > Globals.G_GlobalGradeExclude && drAttd["LeaveTyp"].ToString() == "" && drAttd["Status"].ToString() == "A")
                                        {
                                            drAttd["Halfday"] = 0;
                                            drAttd["EarlyGoing"] = "";
                                            drAttd["GracePeriod"] = "";
                                        }
                                        else if (secgone > Globals.G_HFSEC_EarlyGoing && secgone <= 7200 && eWrkGrp == "COMP")
                                        {
                                            drAttd["Halfday"] = 0;
                                        }
                                        else if (secgone > Globals.G_HFSEC_EarlyGoing && secgone >= 7200  && eWrkGrp == "COMP")
                                        {
                                            drAttd["Halfday"] = 1;
                                            drAttd["GracePeriod"] = "";
                                            drAttd["EarlyGoing"] = "";

                                        }
                                        
                                        
                                    }
                                }
                                #endregion Set_EarlyGoing


                            }

                            #region Set_EarlyCome

                            TimeSpan t2 = (ShiftStart - tInTime);
                            secearly = t2.TotalSeconds;

                            if (secearly > Globals.G_EarlyComeSec)
                            {
                                hurearly = Math.Truncate(secearly / 3600);
                                minearly = Math.Truncate((secearly - (hurearly * 3600)) / 60);
                                sminearly = string.Format("{0:00}:{1:00}", hurearly, minearly); 

                                drAttd["EarlyCome"] = sminearly;

                                if (eWrkGrp == "COMP" && eGradeCode <= Globals.G_GlobalGradeExclude)
                                {
                                    drAttd["Halfday"] = 0;
                                    drAttd["Latecome"] = "";
                                    drAttd["EarlyCome"] = "";
                                }
                                else if (secearly > 1800 && eWrkGrp == "COMP" && eGradeCode > Globals.G_GlobalGradeExclude && drAttd["LeaveTyp"].ToString() != "")
                                {
                                    drAttd["Halfday"] = 0;
                                    drAttd["Latecome"] = "";
                                    drAttd["EarlyCome"] = "";
                                }
                                else if (secearly > 5400 && eWrkGrp == "COMP" && eGradeCode > Globals.G_GlobalGradeExclude && drAttd["LeaveTyp"].ToString() == "")
                                {
                                    drAttd["Halfday"] = 0;
                                    drAttd["Status"] = "A";
                                    drAttd["EarlyCome"] = "";
                                }
                                else if (secearly > 5400 && eWrkGrp == "COMP")
                                {
                                    drAttd["Halfday"] = 0;
                                    drAttd["Status"] = "A";
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
                        minearly = 0;
                        hurearly = 0;
                        sminearly = "";

                        secgrace = 0;
                        mingrace = 0;
                        hurgrace = 0;
                        smingrace = "";

                        secgone = 0;
                        hurgone = 0;
                        mingone = 0;
                        restsecgone = 0;
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
                                CalcShiftOT(daAttdData, dsAttdData, drAttd, Emp, "", out selferr);
                            }

                        }

                        return;
                    }

                    #region Set_shiftvars
                    foreach (DataRow drShift in drShiftC)
                    {
                        ShiftHrs = Convert.ToDouble(drShift["ShiftHrs"].ToString());
                        ShiftBreak = Convert.ToDouble(drShift["BreakHrs"].ToString());

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
                    if (tInTime >= ShiftInFrom)
                    {

                        #region Set_LateComming
                        //'Calc LateCome,EarlyGone
                        if (tInTime > ShiftStart)
                        {
                            TimeSpan tDiff = (tInTime - ShiftStart);
                            #region grace
                            ////grace period
                            if (tDiff.TotalSeconds > 0)
                            {
                                secgrace += tDiff.TotalSeconds;
                                hurgrace = Math.Truncate(secgrace / 3600);
                                mingrace = Math.Truncate((secgrace - (hurgrace * 3600)) / 60);
                                smingrace = string.Format("{0:00}:{1:00}", hurgrace, mingrace);                                
                            }

                            if (secgrace > Globals.G_GracePeriodSec)
                            {
                                drAttd["GracePeriod"] = smingrace;

                                if (drAttd["LeaveTyp"].ToString() != "")
                                {
                                    drAttd["GracePeriod"] = "";
                                }
                            }
                            else
                            {
                                drAttd["GracePeriod"] = "";

                            }//new development for dynamic halfday

                            if (eWrkGrp == "COMP" && eGradeCode <= Globals.G_GlobalGradeExclude)
                            {
                                drAttd["GracePeriod"] = "";
                            }

                            #endregion grace

                            if (tDiff.TotalSeconds >= Globals.G_LateComeSec)
                            {

                                seclate = tDiff.TotalSeconds;
                                if (seclate > 1 && seclate <= 59)
                                    seclate = 60;

                                hurlate = Math.Truncate(seclate / 3600);
                                minlate = Math.Truncate((seclate - (hurlate * 3600)) / 60);
                                sminlate = string.Format("{0:00}:{1:00}", hurlate, minlate);
                                drAttd["LateCome"] = sminlate;

                                //new development for dynamic halfday
                                if (Globals.G_HFFLG_LateCome)
                                {
                                    //'added on 14-02-2014 if late more than 30 min mark halfday exclude agm and above on comp emp.
                                    if (eWrkGrp == "COMP" && eGradeCode <= Globals.G_GlobalGradeExclude)
                                    {
                                        drAttd["Halfday"] = 0;
                                        drAttd["Latecome"] = "";
                                        drAttd["EarlyCome"] = "";
                                        drAttd["GracePeriod"] = "";
                                    }
                                    else if (seclate > Globals.G_HFSEC_LateCome && eWrkGrp == "COMP" && eGradeCode > Globals.G_GlobalGradeExclude && drAttd["LeaveTyp"].ToString() != "")
                                    {
                                        drAttd["Halfday"] = 0;
                                        drAttd["Latecome"] = "";
                                        drAttd["EarlyCome"] = "";
                                        drAttd["GracePeriod"] = "";
                                    }
                                    else if ((seclate > Globals.G_HFSEC_LateCome && seclate < 5400 && eWrkGrp == "COMP" && eGradeCode > Globals.G_GlobalGradeExclude && drAttd["LeaveTyp"].ToString() == ""))
                                    {
                                        drAttd["Halfday"] = 1;
                                        drAttd["GracePeriod"] = "";
                                        drAttd["LateCome"] = "";
                                    }
                                    else if (seclate > 5400 && eWrkGrp == "COMP" && eGradeCode > Globals.G_GlobalGradeExclude && drAttd["LeaveTyp"].ToString() == "")
                                    {
                                        drAttd["Halfday"] = 0;
                                        drAttd["Status"] = "A";
                                        drAttd["Latecome"] = "";
                                        drAttd["GracePeriod"] = "";
                                    }
                                    else if (seclate > 5400 && eWrkGrp == "COMP")
                                    {
                                        drAttd["Halfday"] = 0;
                                        drAttd["Status"] = "A";
                                        drAttd["Latecome"] = "";
                                        drAttd["GracePeriod"] = "";
                                    }


                                }//new development for dynamic halfday

                                daAttdData.Update(dsAttdData, "AttdData");
                            }
                        }

                        #endregion Set_LateComming

                        #region Set_EarlyCome

                        if (tInTime < ShiftStart)
                        {
                            TimeSpan tDiff = (ShiftStart - tInTime);

                            if (tDiff.TotalSeconds <= (Globals.G_EarlyComeSec))
                            {
                                TimeSpan t4 = (ShiftStart - tInTime);

                                secearly = t4.TotalSeconds;
                                hurearly = Math.Truncate(secearly / 3600);
                                minearly = Math.Truncate((secearly - (hurearly * 3600)) / 60);
                                sminearly = string.Format("{0:00}:{1:00}", hurearly, minearly);

                                drAttd["EarlyCome"] = sminearly;

                                if (eWrkGrp == "COMP" && eGradeCode <= Globals.G_GlobalGradeExclude)
                                {
                                    drAttd["Halfday"] = 0;
                                    drAttd["Latecome"] = "";
                                    drAttd["EarlyCome"] = "";
                                    drAttd["GracePeriod"] = "";
                                }
                                else if (secearly > 1800 && eWrkGrp == "COMP" && eGradeCode > Globals.G_GlobalGradeExclude && drAttd["LeaveTyp"].ToString() != "")
                                {
                                    drAttd["Halfday"] = 0;
                                    drAttd["Latecome"] = "";
                                    drAttd["EarlyCome"] = "";
                                    drAttd["GracePeriod"] = "";
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

                            if (ShiftEnd > Convert.ToDateTime(drAttd["ConsOut"]))
                            {
                                TimeSpan t5 = (Convert.ToDateTime(drAttd["ConsOut"]) - ShiftEnd);

                                #region grace

                                ///Removed Grace Period From Out Time as per Mail of Vallabh Bhai
                                ///Date : 04/03/2019

                                //grace period
                                //if (Math.Abs(t5.TotalSeconds) > 0)
                                //{
                                //    secgrace += Math.Abs(t5.TotalSeconds);
                                //    hurgrace = Math.Truncate(secgrace / 3600);
                                //    mingrace = Math.Truncate((secgrace - (hurgrace * 3600)) / 60);
                                //    smingrace = string.Format("{0:00}:{1:00}", hurgrace, mingrace);
                                   
                                //}
                                //if (secgrace > Globals.G_GracePeriodSec)
                                //{
                                //    drAttd["GracePeriod"] = smingrace;

                                //    if (drAttd["LeaveTyp"].ToString() != "")
                                //    {
                                //        drAttd["GracePeriod"] = "";
                                //    }
                                //}
                                //else
                                //{
                                //    drAttd["GracePeriod"] = "";

                                //}//new development for dynamic halfday

                                //removed : 04/03/2019

                                if (eWrkGrp == "COMP" && eGradeCode <= Globals.G_GlobalGradeExclude)
                                {
                                    drAttd["GracePeriod"] = "";
                                }

                                #endregion

                                if (t5.TotalSeconds < (-1 * Globals.G_EarlyGoingSec) && t5.TotalSeconds < 0)
                                {
                                    secgone = (ShiftEnd - Convert.ToDateTime(drAttd["ConsOut"])).TotalSeconds;
                                    //if (secgone > 1 && secgone <= 59)
                                    //    secgone = 60;

                                    hurgone = Math.Truncate(secgone / 3600);
                                    mingone = Math.Truncate((secgone - (hurgone * 3600)) / 60);
                                    //smingone = string.Format("{0:00}:{1:00}", hurgone, mingone);
                                    restsecgone = (secgone - ((hurgone * 3600) + (mingone * 60)));
                                    smingone = string.Format("{0:00}:{1:00}:{2:00}", hurgone, mingone, restsecgone);
                                    
                                    drAttd["EarlyGoing"] = smingone;

                                    //new development for dynamic halfday
                                    if (Globals.G_HFFLG_EarlyGoing)
                                    {
                                        if (eWrkGrp == "COMP" && eGradeCode <= Globals.G_GlobalGradeExclude)
                                        {
                                            drAttd["Halfday"] = 0;
                                            drAttd["EarlyGoing"] = "";
                                            drAttd["GracePeriod"] = "";
                                        }
                                        else if (secgone > Globals.G_HFSEC_EarlyGoing && eWrkGrp == "COMP" && eGradeCode > Globals.G_GlobalGradeExclude && drAttd["LeaveTyp"].ToString() != "")
                                        {
                                            drAttd["Halfday"] = 0;
                                            drAttd["EarlyGoing"] = "";
                                            drAttd["GracePeriod"] = "";
                                        }
                                        else if (secgone > Globals.G_HFSEC_EarlyGoing && eWrkGrp == "COMP" && eGradeCode > Globals.G_GlobalGradeExclude && drAttd["LeaveTyp"].ToString() == "" && drAttd["Status"].ToString() == "P")
                                        {
                                            drAttd["Halfday"] = 1;
                                            drAttd["EarlyGoing"] = "";
                                            drAttd["GracePeriod"] = "";
                                        }
                                        else if (secgone > Globals.G_HFSEC_EarlyGoing && eWrkGrp == "COMP" && eGradeCode > Globals.G_GlobalGradeExclude && drAttd["LeaveTyp"].ToString() == "" && drAttd["Status"].ToString() == "A")
                                        {
                                            drAttd["Halfday"] = 0;
                                            drAttd["EarlyGoing"] = "";
                                            drAttd["GracePeriod"] = "";
                                        }
                                        else if (secgone > Globals.G_HFSEC_EarlyGoing && eWrkGrp == "COMP" )
                                        {
                                            drAttd["Halfday"] = 1;
                                            drAttd["EarlyGoing"] = "";
                                            drAttd["GracePeriod"] = "";
                                        }

                                    }//new development for dynamic halfday


                                }
                            }


                            daAttdData.Update(dsAttdData, "AttdData");
                        }

                        #endregion Set_EarlyGoing


                    }
                    else //if emp comes before schedule shift set early times
                    {

                        #region Set_EarlyCome1
                       
                        secearly = (tInTime - ShiftStart).TotalSeconds;
                        hurearly = Math.Truncate(secearly / 3600);
                        minearly = Math.Truncate((secearly - (hurearly * 3600)) / 60);
                        sminearly = string.Format("{0:00}:{1:00}", Math.Abs(hurearly), Math.Abs(minearly));
                        drAttd["EarlyCome"] = sminearly;

                        if (eWrkGrp == "COMP" && eGradeCode <= Globals.G_GlobalGradeExclude)
                        {
                            drAttd["Halfday"] = 0;
                            drAttd["Latecome"] = "";
                            drAttd["EarlyCome"] = "";
                        }
                        else if (secearly > (-1 * Globals.G_EarlyComeSec) && eWrkGrp == "COMP" && eGradeCode > Globals.G_GlobalGradeExclude && drAttd["LeaveTyp"].ToString() != "")
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
                       

                        #endregion Set_EarlyCome1

                        #region Set_EarlyGoing1

                        if (drAttd["ConsOut"] != DBNull.Value)
                        {
                            if (Convert.ToDateTime(drAttd["ConsOut"]) < ShiftEnd)
                            {
                                TimeSpan t5 = (ShiftEnd - Convert.ToDateTime(drAttd["ConsOut"]));
                                

                                #region grace

                                ///Removed Grace Period From Out Time as per Mail of Vallabh Bhai
                                ///Date : 04/03/2019

                                //if (t5.TotalSeconds > 0)
                                //{
                                //    secgrace += t5.TotalSeconds;
                                //    hurgrace = Math.Truncate(secgrace / 3600);
                                //    mingrace = Math.Truncate((secgrace - (hurgrace * 3600)) / 60);
                                //    smingrace = string.Format("{0:00}:{1:00}", hurgrace, mingrace);                                    
                                //}
                                ////grace period
                                //if (secgrace > Globals.G_GracePeriodSec)
                                //{
                                //    drAttd["GracePeriod"] = smingrace;
                                //    if (drAttd["LeaveTyp"].ToString() != "")
                                //    {
                                //        drAttd["GracePeriod"] = "";
                                //    }
                                //}
                                //else
                                //{
                                //    drAttd["GracePeriod"] = "";

                                //}//new development for dynamic halfday

                                ///removed - 04/03/2019

                                if (eWrkGrp == "COMP" && eGradeCode <= Globals.G_GlobalGradeExclude)
                                {
                                    drAttd["GracePeriod"] = "";
                                }

                                #endregion
                                
                                if (t5.TotalSeconds > (Globals.G_EarlyGoingSec) )
                                {
                                    secgone = (ShiftEnd - Convert.ToDateTime(drAttd["ConsOut"])).TotalSeconds;
                                    
                                    //if (secgone > 1 && secgone <= 59)
                                    //    secgone = 60;


                                    hurgone = Math.Truncate(secgone / 3600);
                                    mingone = Math.Truncate((secgone - (hurgone * 3600)) / 60);
                                    restsecgone = (secgone - ((hurgone * 3600) + (mingone * 60)));
                                    smingone = string.Format("{0:00}:{1:00}:{2:00}", hurgone, mingone,restsecgone);
                                    drAttd["EarlyGoing"] = smingone;

                                    if (Globals.G_HFFLG_EarlyGoing)
                                    {
                                        if (eWrkGrp == "COMP" && eGradeCode <= Globals.G_GlobalGradeExclude)
                                        {
                                            drAttd["Halfday"] = 0;
                                            drAttd["EarlyGoing"] = "";
                                        }
                                        else if (secgone > Globals.G_HFSEC_EarlyGoing && eWrkGrp == "COMP" && eGradeCode > Globals.G_GlobalGradeExclude && drAttd["LeaveTyp"].ToString() != "")
                                        {
                                            drAttd["Halfday"] = 0;
                                            drAttd["EarlyGoing"] = "";
                                        }
                                        else if (secgone > Globals.G_HFSEC_EarlyGoing && secgone <= 7200 && eWrkGrp == "COMP" && eGradeCode > Globals.G_GlobalGradeExclude && drAttd["LeaveTyp"].ToString() == "" && drAttd["Status"].ToString() == "P")
                                        {
                                            drAttd["Halfday"] = 1;
                                            drAttd["EarlyGoing"] = "";

                                        }
                                        else if (secgone > Globals.G_HFSEC_EarlyGoing && secgone <= 7200 && eWrkGrp == "COMP" && eGradeCode > Globals.G_GlobalGradeExclude && drAttd["LeaveTyp"].ToString() == "" && drAttd["Status"].ToString() == "A")
                                        {
                                            drAttd["Halfday"] = 0;
                                            drAttd["EarlyGoing"] = "";
                                        }
                                        else if (secgone > Globals.G_HFSEC_EarlyGoing && secgone <= 7200 && eWrkGrp == "COMP")
                                        {
                                            drAttd["Halfday"] = 1;
                                            drAttd["EarlyGoing"] = "";
                                           
                                        }
                                        else if (secgone > Globals.G_HFSEC_EarlyGoing && secgone >= 7200 && eWrkGrp == "COMP")
                                        {
                                            drAttd["Halfday"] = 1;
                                            drAttd["EarlyGoing"] = "";

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
                            daAttdData.Update(dsAttdData, "AttdData");
                        }

                        #endregion Set_EarlyGoing1

                        daAttdData.Update(dsAttdData, "AttdData");
                    }

                    #endregion Check_intime_if_follow_in_shiftintime



                    #endregion Setting_Late_Early

                    //'check if both punch found
                    //'then first check intime and outtime convers schedule shift
                    if (drAttd["ConsIn"] != DBNull.Value && drAttd["ConsOut"] != DBNull.Value)
                    {
                        if (Convert.ToDateTime(drAttd["ConsIn"]) < ShiftStart && Convert.ToDateTime(drAttd["ConsOut"]) > ShiftEnd)
                        {
                            drAttd["Halfday"] = 0;
                            drAttd["Latecome"] = "";
                            drAttd["EarlyCome"] = "";
                            drAttd["EarlyGoing"] = "";
                            drAttd["Status"] = "P";
                            drAttd["GracePeriod"] = "";
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

                    secgrace = 0;
                    hurgrace = 0;
                    mingrace = 0;
                    smingrace = "";
                    restsecgone = 0;
                    #endregion Reset_ShiftVars


                    #endregion SchShiftCalc
                }

                if (Convert.ToDecimal(drAttd["ConsWrkHrs"]) > 4 && Convert.ToDecimal(drAttd["ConsWrkHrs"]) <= 6 && eGradeCode <= Globals.G_GlobalGradeExclude)
                {
                    drAttd["Halfday"] = 1;
                    daAttdData.Update(dsAttdData, "AttdData");
                }


                #region OTCalc
            OTCalc:

                //used for LateComeException added on 14/06/2019
                if (Convert.ToDecimal(drAttd["ConsWrkHrs"]) >= 6
                    && tLateExp == true
                    && drAttd["LateCome"].ToString() != ""
                    && seclate > Globals.G_HFSEC_LateCome
                    && drAttd["GracePeriod"].ToString() != ""
                    && seclate < 5400
                    )
                {
                    drAttd["GracePeriod"] = "";
                    drAttd["LateCome"] = "";
                    drAttd["Halfday"] = 0;
                    daAttdData.Update(dsAttdData, "AttdData");
                }



                if (!tOTFLG)
                {
                    drAttd["ConsOverTime"] = 0;
                    drAttd["CalcOverTime"] = 0;
                    daAttdData.Update(dsAttdData, "AttdData");

                    return;
                }


                //'added on 14-02-2014
                if (Convert.ToBoolean(drAttd["Halfday"]))
                {
                    drAttd["ConsOverTime"] = 0;
                    drAttd["CalcOvertime"] = 0;
                    return;
                }

                if (drAttd["Status"].ToString() == "A")
                {
                    drAttd["ConsOverTime"] = 0;
                    drAttd["CalcOvertime"] = 0;
                    return;
                }



                OverTime = 0;
                if (tOTFLG && (Convert.ToDouble(drAttd["ConsWrkHrs"])) > ShiftHrs && drAttd["LeaveTyp"].ToString() == "")
                {

                    DateTime tOut = Convert.ToDateTime(drAttd["ConsOut"]);
                    //DateTime tIn = Convert.ToDateTime(drAttd["ConsInTime"]);
                    int tHour = (tOut - ShiftEnd).Hours;

                    int ot = 0;

                    if (tHour >= 2)
                    {
                        ot = (tHour - Convert.ToInt32(ShiftBreak));

                        if (drAttd["Latecome"].ToString() != "")
                        {
                            int thr = Convert.ToInt32(drAttd["Latecome"].ToString().Substring(0, 2));
                            int tmn = Convert.ToInt32(drAttd["Latecome"].ToString().Substring(3, 2));

                            ot = ot - thr;
                            if (tmn >= 1)
                                ot = ot - 1;
                            
                        }

                        if (ot >= 2)
                        {
                            drAttd["ConsOverTime"] = ot;
                            drAttd["CalcOvertime"] = ot;
                        }
                        else
                        {
                            drAttd["ConsOverTime"] = 0;
                            drAttd["CalcOvertime"] = 0;
                        }
                        
                    }
                    else
                    {
                        drAttd["ConsOverTime"] = 0;
                        drAttd["CalcOvertime"] = 0;
                    }

                    //removed Rounding calculate completed hours only
                    //based on Mr. Vallabh's Mail 04/03/2019
                    
                    //TimeSpan t3 = (Convert.ToDateTime(drAttd["ConsOut"]) - ShiftEnd);
                    //OverTime = t3.TotalSeconds;
                    //double othrs = 0, otmin = 0;
                    //double ot = 0;

                    //othrs = Math.Truncate(OverTime / 3600);
                    //otmin = Math.Truncate((OverTime - (othrs * 3600)) / 60);
                    //ot = othrs;

                    //if (otmin >= 21 && otmin <= 50)
                    //{
                    //    ot = othrs + 0.5;
                    //}
                    //else if (otmin > 50 && otmin <= 59)
                    //{
                    //    ot = othrs + 1;
                    //}

                    //if (ot >= 1)
                    //{
                    //    ot = ot - ShiftBreak;
                    //}

                    //if (ot >= 1.5)
                    //{
                    //    if (drAttd["Latecome"].ToString() != "")
                    //    {
                    //        int thr = Convert.ToInt32(drAttd["Latecome"].ToString().Substring(0, 2));
                    //        int tmn = Convert.ToInt32(drAttd["Latecome"].ToString().Substring(3, 2));

                    //        ot = ot - thr;
                    //        if (tmn >= 15 && tmn <= 40)
                    //            ot = ot - 0.5;
                    //        else if (tmn >= 41 && tmn <= 59)
                    //            ot = ot - 1;

                    //    }

                    //    drAttd["ConsOverTime"] = ot;
                    //    drAttd["CalcOvertime"] = ot;
                    //}
                    //else
                    //{
                    //    drAttd["ConsOverTime"] = 0;
                    //    drAttd["CalcOvertime"] = 0;
                    //}
                }
                else if (tOTFLG && (drAttd["LeaveTyp"].ToString() == "WO" || drAttd["LeaveTyp"].ToString() == "PH" ))
                {
                    OverTime = Math.Truncate(Convert.ToDouble(drAttd["ConsWrkHrs"]) - BreakHours);
                    
                    if (drAttd["LeaveTyp"].ToString() == "WO" || drAttd["LeaveTyp"].ToString() == "PH")
                    {
                        if (OverTime >= 2)
                        {
                            drAttd["ConsOverTime"] = OverTime;
                            drAttd["CalcOvertime"] = OverTime;
                        }
                        else
                        {
                            drAttd["ConsOverTime"] = 0;
                            drAttd["CalcOvertime"] = 0;

                        }
                    }
                }                
                else
                {
                    drAttd["ConsOverTime"] = 0;
                    drAttd["CalcOvertime"] = 0;

                }

                //NEW DEVELOPMENT, AS PER MAIL DT 09/08/21, VALLABH - HR, AUTO CALCULATION OF OVERTIME 
                //SHOULD NOT BE POSTED
                //
                //DateTime stopotdt = new DateTime(2021, 09, 07);
                //if (Emp.WrkGrp == "COMP" && tDate >= stopotdt)
                //{
                //    drAttd["ConsOverTime"] = 0;
                //}


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

        public void WoChange(string tEmpUnqID, DateTime tFromDt, DateTime tToDate, string WoDay, out string err)
        {
            err = string.Empty;
            string proerr;
            bool isScheduled = false;

            #region Primary_Chk
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

            if (string.IsNullOrEmpty(WoDay))
            {
                proerr = "Invalid WO Days...";
                err = proerr;
                return;
            }

            string WOD = "SUN,MON,TUE,WED,THU,FRI,SAT";
            if(!WOD.Contains(WoDay))
            {
                 proerr = "Invalid WO Days...";
                err = proerr;
                return;
            }

            ////make sure to to check months between dates
            //if(tFromDt.ToString("yyyyMM") != tToDate.ToString("yyyyMM")){
            //    proerr = "Cross Month Changes are not allowed";
            //    err = proerr;
            //    return;
            //}


            ////block previous month changes
            //int mth = Convert.ToInt32(tFromDt.ToString("yyyyMM"));
            //int curmth = Convert.ToInt32(Utils.Helper.GetDescription("SELECT LEFT(CONVERT(varchar, GetDate(),112),6)", Utils.Helper.constr));
            //if (mth < curmth)
            //{
            //    err += "Previous Month Changes are not allowed";
            //    return;
            //}

            ////prevent nearest wo from previous wo 
            //string sql = "Select tDate From MastLeaveSchedule Where EmpUnqId = '" + tEmpUnqID + "' " +
            //    " And tDate < '" + tFromDt.ToString("yyyy-MM-dd") + "' and SchLeave = 'WO' Order By SanID Desc ";

            //string pdate = Utils.Helper.GetDescription(sql,Utils.Helper.constr);
            //if (!string.IsNullOrEmpty(pdate))
            //{
            //    DateTime prvWo = Convert.ToDateTime(pdate);
            //    DateTime nxtWo = new DateTime();
            //    for (DateTime date = tFromDt; date.Date <= tToDate.Date; date = date.AddDays(1))
            //    {
            //        if(date.ToString("dddd").ToUpper().Substring(0,3) == WoDay)
            //        {
            //            nxtWo = date;
            //            break;
            //        }
            //    }

            //    if ((nxtWo - prvWo).Days + 1 <= 5)
            //    {
            //        err +=  string.Format("Next WO on {0} is too near from previous WO : {1}",nxtWo.ToString("dd/MM/yyyy"),prvWo.ToString("dd/MM/yyyy"));
            //        return;
            //    }

            //}


            #region Chk_ValidEmp
                clsEmp Emp = new clsEmp();
                Emp.EmpUnqID = tEmpUnqID;
                Emp.CompCode = "01";
                if(!Emp.GetEmpDetails(Emp.EmpUnqID))
                {
                    err = "Employee does not exist..";
                    return;
                }
                if (!Emp.Active)
                {
                    err = "Invalid/InActive Employee..";
                    return;
                }

            #endregion

            #endregion
                        
            #region Chk_IfLeavePosted

              string  sql = "Select count(*) from LeaveEntry Where " +
               " compcode = '" + Emp.CompCode + "'" +
               " And tYear ='" + tFromDt.Year + "'" +
               " And EmpUnqID='" + Emp.EmpUnqID + "'" +
               " And (     FromDt between '" + tFromDt.ToString("yyyy-MM-dd") + "' And '" + tToDate.ToString("yyyy-MM-dd") + "' " +
               "  OR       ToDt Between '" + tFromDt.ToString("yyyy-MM-dd") + "'   And '" + tToDate.ToString("yyyy-MM-dd") + "' " +
               "  OR '" + tFromDt.ToString("yyyy-MM-dd") + "' Between FromDt And ToDt " +
               "  OR '" + tToDate.ToString("yyyy-MM-dd") + "' Between FromDt And ToDt " +
               "     ) ";

            int chkleave = Convert.ToInt32(Utils.Helper.GetDescription(sql, Utils.Helper.constr));
            if(chkleave > 0)
            {
                err += "Error : There are some leave posted...";
                return;
            }
            #endregion
            
            #region Get_IfScheduled

            sql = "Select Count(*) from MastShiftSchedule where " +
              " Yearmt='" + tFromDt.ToString("yyyyMM") + "'" +
              " And EmpUnqID ='" + Emp.EmpUnqID + "'";
        
                int schcnt = Convert.ToInt32(Utils.Helper.GetDescription(sql,Utils.Helper.constr));

                if(schcnt <= 0) 
                    isScheduled = false;
                else
                    isScheduled = true;
            #endregion

            /*
            '1) Delete From MastLeaveSchedule where sEmpcode and SchLeave = 'WO' and tDate Falls between sFromDt,sTodt
            '2) update AttdData ScheduleShift = '' where sEmpCode and ScheduleShift = 'WO' and tDate Falls between sFromDt,sTodt
            '3) insert MastLeaveSchedule where WeekDay = WoDay addid 'ShiftSch'
            '4) update AttdData ScheduleShift = 'WO' where sEmpCode  and tDate Falls between sFromDt,sTodt and WeekDay = sWoDay
            '5) Correct MastShiftSchedule.....if Scheduled
            */
            
            using(SqlConnection cn  = new SqlConnection(Utils.Helper.constr))
            {
                try{
                    cn.Open();
                }catch(Exception ex){
                    err= ex.ToString();
                    return;
                }

                SqlTransaction tr = cn.BeginTransaction();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.Transaction = tr;

                #region common
                sql = " Delete From MastLeaveSchedule Where EmpUnqID ='" + Emp.EmpUnqID + "' And SchLeave ='WO' and tDate between '" + tFromDt.ToString("yyyy-MM-dd") + "' and '" + tToDate.ToString("yyyy-MM-dd") + "'";
                try
                {
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();
                }catch(Exception ex){
                    err= ex.ToString();
                    return;
                }

                sql = " Update AttdData Set ScheduleShift = '' Where EmpUnqID ='" + Emp.EmpUnqID + "' And ScheduleShift ='WO' and tDate between '" + tFromDt.ToString("yyyy-MM-dd") + "' and '" + tToDate.ToString("yyyy-MM-dd") + "'";
                try
                {
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();
                }catch(Exception ex){
                    err= ex.ToString();
                    return;
                }

                sql = " Insert into MastLeaveSchedule (EmpUnqId,WrkGrp,tDate,SchLeave,AddId,AddDt) " +
               " Select '" + Emp.EmpUnqID + "','" + Emp.WrkGrp + "', [Date] ,'WO','ShiftSch',GetDate() From " +
               " F_TABLE_DATE('"  + tFromDt.ToString("yyyy-MM-dd") + "','"  + tToDate.ToString("yyyy-MM-dd") + "') Where WeekDayName ='" + WoDay + "'";
                try
                {
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();
                }catch(Exception ex){
                    err= ex.ToString();
                    return;
                }

                sql = " Update a " +
               "   Set a.ScheduleShift = b.SchLeave " +
               " From AttdData a,MastLeaveSchedule b " +
               " Where a.EmpUnqId = b.EmpUnqID and a.WrkGrp = b.WrkGrp And a.TDate = b.Tdate " +
               " And a.EmpUnqID ='" + Emp.EmpUnqID + "' and b.SchLeave ='WO' " +
               " And a.tDate between '" + tFromDt.ToString("yyyy-MM-dd") + "' And '" + tToDate.ToString("yyyy-MM-dd") + "'" ;

                try
                {
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();
                }catch(Exception ex){
                    err= ex.ToString();
                    return;
                }

                #endregion
                
                #region ifScheduled
                if(isScheduled)
                {

                    sql = "Select * from MastShiftSchedule Where EmpUnqId ='" + Emp.EmpUnqID +  "' and yearmt='" + tFromDt.ToString("yyyyMM") + "'";
                    DataSet ds = Utils.Helper.GetData(sql,Utils.Helper.constr);
                    bool hasRows = ds.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);
                    if (hasRows)
                    {
                        DataRow dr = ds.Tables[0].Rows[0];
                        
                        string fldnm = string.Empty;
                        string pfldnm = string.Empty;
                        string nfldnm = string.Empty;

                        #region Reset_OLDWO
                        
                        //-replace previous shiftcode if WO Allready there
                        for(DateTime date = tFromDt; date.Date <= tToDate.Date; date = date.AddDays(1))
                        {
                            fldnm = "D" + date.ToString("dd");
                            pfldnm = "D" + date.AddDays(-1).ToString("dd");
                            nfldnm = "D" + date.AddDays(1).ToString("dd");

                            if(dr[fldnm].ToString() == "WO")
                            {
                                //check if first Day WO
                                if(date.Day == 1) 
                                {
                                    sql = "Update MastShiftSchedule Set " + fldnm + " = " + nfldnm + " Where EmpUnqId ='" + Emp.EmpUnqID + "' and yearmt = '" + tFromDt.ToString("yyyyMM") + "'";
                                
                                }else{

                                    sql = "Update MastShiftSchedule " +
                                         " Set " + fldnm + " = " + pfldnm + " Where " +
                                         " EmpUnqId ='" + Emp.EmpUnqID + "' and yearmt='" + tFromDt.ToString("yyyyMM") + "'";
                                }
                                    
                                try
                                {
                                    cmd.CommandText = sql;
                                    cmd.ExecuteNonQuery();
                                }catch(Exception ex){
                                    err= ex.ToString();
                                    return;
                                }  
                  
                            }//if old val is wo

                        }//loop for eachday

                        #endregion

                        #region Set_NEWWO
                        //-set WO on woday MastShiftSchedule
                        sql = "Select [Date],[CalendarYearMonth],[WeekdayName] from F_TABLE_DATE('" + tFromDt.ToString("yyyy-MM-dd") +  "','" + tToDate.ToString("yyyy-MM-dd") +  "') where CalendarYearMonth = '" + tFromDt.ToString("yyyyMM") + "' and [WeekdayName] = '" + WoDay + "'";
                        ds = Utils.Helper.GetData(sql,Utils.Helper.constr);
                        hasRows = ds.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);
                        if (hasRows)
                        {
                            foreach(DataRow tdr in ds.Tables[0].Rows)
                            {
                                fldnm = "D" + Convert.ToDateTime(tdr["Date"]).ToString("dd");

                                if(tdr["WeekDayName"].ToString().ToUpper() == WoDay.ToUpper())
                                {
                                    sql = "Update MastShiftSchedule " +
                                    " Set " + fldnm + " = 'WO' Where " +
                                    " EmpUnqId ='" + Emp.EmpUnqID + "' and yearmt='" + tFromDt.ToString("yyyyMM") + "'";

                                    try
                                    {
                                        cmd.CommandText = sql;
                                        cmd.ExecuteNonQuery();
                                    }catch(Exception ex){
                                        err= ex.ToString();
                                        return;
                                    }  
                                    
                                }
                            }
                        }
                       

                        #endregion
                        
                    }
                }//end isscheduled
                #endregion

                try
                {
                    tr.Commit();
                }catch(Exception ex){
                    err = ex.ToString();
                    return;
                }

                
            }//end using cn

            //process data
            int res = 0;
            string outerr = string.Empty;
            this.AttdProcess(Emp.EmpUnqID, tFromDt, tToDate, out res, out outerr);

            //process lunchinout
           // this.LunchInOutProcess(Emp.EmpUnqID, tFromDt, tToDate, out res);

        }

        public void ShiftChange(string tEmpUnqID, DateTime tFromDt, DateTime tToDate,string ShiftCode, out string err)
        {

            err = string.Empty;
            string proerr;

            #region Primary_Chk
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

            if (string.IsNullOrEmpty(ShiftCode))
            {
                proerr = "Invalid ShiftCode...";
                err = proerr;
                return;
            }

            if (!Globals.G_ShiftList.Contains(ShiftCode))
            {
                proerr = "Invalid ShiftCode...";
                err = proerr;
                return;
            }

            ////make sure to to check months between dates
            //if (tFromDt.ToString("yyyyMM") != tToDate.ToString("yyyyMM"))
            //{
            //    proerr = "Cross Month Changes are not allowed";
            //    err = proerr;
            //    return;
            //}


            ////block previous month changes
            //int mth = Convert.ToInt32(tFromDt.ToString("yyyyMM"));
            //int curmth = Convert.ToInt32(Utils.Helper.GetDescription("SELECT LEFT(CONVERT(varchar, GetDate(),112),6)", Utils.Helper.constr));
            //if (mth < curmth)
            //{
            //    err += "Previous Month Changes are not allowed";
            //    return;
            //}

            clsEmp Emp = new clsEmp();
            Emp.EmpUnqID = tEmpUnqID;
            Emp.CompCode = "01";
            if (!Emp.GetEmpDetails(Emp.EmpUnqID))
            {
                err = "Employee does not exist..";
                return;
            }
            if (!Emp.Active)
            {
                err = "Invalid/InActive Employee..";
                return;
            }


            #endregion

            string sql = string.Empty;

            using (SqlConnection cn = new SqlConnection(Utils.Helper.constr))
            {

                try
                {
                    cn.Open();
                }
                catch (Exception ex)
                {
                    err = ex.ToString();
                    return;
                }

                SqlTransaction tr = cn.BeginTransaction();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.Transaction = tr;
                                
                for (DateTime date = tFromDt; date.Date <= tToDate.Date; date = date.AddDays(1))
                {
                    sql = "Insert into MastLeaveSchedule (EmpUnqId,WrkGrp,AddDt,AddId,tDate,ConsShift) " +
                    " Values ('" + Emp.EmpUnqID + "','" + Emp.WrkGrp + "',GetDate(),'" + Utils.User.GUserID + "','" + date.ToString("yyyy-MM-dd") + "','" + ShiftCode + "')";

                    try
                    {
                        cmd.CommandText = sql;
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        err = ex.ToString();
                        return;
                    }  

                }//for each dateloop

                try
                {
                    tr.Commit();
                }
                catch (Exception ex)
                {
                    err = ex.ToString();
                    return;
                }
                
            }//end of using connection


            //process data
            int res = 0;
            string outerr = string.Empty;
            this.AttdProcess(Emp.EmpUnqID, tFromDt, tToDate, out res, out outerr);

            //process lunchinout
            this.LunchInOutProcess(Emp.EmpUnqID, tFromDt, tToDate, out res);


        }


        public void HalfDay_Rules_Process(string tWrkGrp ,string tEmpUnqID, DateTime tFromDt, DateTime tToDate, out string result)
        {
            result = string.Empty;

            if (string.IsNullOrEmpty(tWrkGrp))
            {
                result = "WrkGrp is required...";
                return;
            }

            if (tToDate < tFromDt)
            {
                result = "Invalid date range ...";
                return;
            }

            if (tFromDt == DateTime.MinValue)
            {
                result = "Invalid From Date...";
                return;
            }

            if (tToDate == DateTime.MinValue)
            {
                result = "Invalid To Date...";
                return;
            }


            //call main store proce.
            using (SqlConnection cn = new SqlConnection(Utils.Helper.constr))
            {
                try
                {
            
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = cn;
                        cmd.CommandType = CommandType.Text;

                        string sql = string.Empty;

                        if (string.IsNullOrEmpty(tEmpUnqID))
                        {
                            sql = "Insert into ProcessLog (AddDt,AddId,ProcessType,FromDt,ToDt,WrkGrp ) Values (" +
                            " GetDate(),'" + Utils.User.GUserID + "','HalfDayRule','" + tFromDt.ToString("yyyy-MM-dd") + "'," +
                            " '" + tToDate.ToString("yyyy-MM-dd") + "','" + tWrkGrp + "')";
                        }
                        else
                        {
                            sql = "Insert into ProcessLog (AddDt,AddId,ProcessType,FromDt,ToDt,EmpUnqID ) Values (" +
                           " GetDate(),'" + Utils.User.GUserID + "','HalfDayRule','" + tFromDt.ToString("yyyy-MM-dd") + "'," +
                           " '" + tToDate.ToString("yyyy-MM-dd") + "','" + tEmpUnqID + "')";
                        }
                        
                        cmd.CommandText = sql;
                        cmd.ExecuteNonQuery();

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "Attd_HalfDay_LateEarly";

                        SqlParameter spout = new SqlParameter();
                        spout.Direction = ParameterDirection.Output;
                        spout.DbType = DbType.StringFixedLength;
                        spout.Size = 400;
                        spout.ParameterName = "@status";
                        string tout = "";
                        spout.Value = tout;

                        cmd.Parameters.AddWithValue("@pWrkGrp", tWrkGrp);
                        cmd.Parameters.AddWithValue("@pEmpUnqID", tEmpUnqID);
                        cmd.Parameters.AddWithValue("@pFromDt", tFromDt);
                        cmd.Parameters.AddWithValue("@pToDt", tToDate);
                        cmd.Parameters.Add(spout);
                        cmd.CommandTimeout = 0;
                        cmd.ExecuteNonQuery();

                        //get the output
                        result = (string)cmd.Parameters["@status"].Value;



                    }

                }
                catch (Exception ex)
                {
                    result = "Error : " + ex.ToString();
                }

            }//using connection
        
        }


        public void Calc_StdHrs(SqlDataAdapter daAttdData, DataSet dsAttdData, DataRow drAttd)
        {
            double stdhrs = 0;
            string err2 = string.Empty;
            string stdhrs2 = Utils.Helper.GetDescription("Select StdShiftHrs from MastShift where ShiftCode ='" + drAttd["ConsShift"].ToString() + "'", Utils.Helper.constr, out err2);

            double.TryParse(stdhrs2, out stdhrs);

            drAttd["StdShftHrs"] = stdhrs;
            drAttd["StdHrsOT"] = 0;
            drAttd["StdWrkHrs"] = 0;
            drAttd["StdWrkShift"] = drAttd["ConsShift"]; 
            daAttdData.Update(dsAttdData, "AttdData");

            if (Convert.ToDouble(drAttd["ConsWrkHrs"]) > 0 && drAttd["Status"].ToString() == "P" && Convert.ToDouble(drAttd["StdShftHrs"]) > 0)
            {

                drAttd["StdWrkHrs"] = Convert.ToDouble(drAttd["ConsWrkHrs"]) - stdhrs;
                double thour = 0;

                if (drAttd["ConsIn"] != DBNull.Value && drAttd["ConsOut"] != DBNull.Value && !string.IsNullOrEmpty(drAttd["ConsShift"].ToString()))
                {
                     
                    
                    try
                    {
                        string shiftstarttime = Utils.Helper.GetDescription("Select [ShiftStart] from MastShift where ShiftCode ='" + drAttd["ConsShift"].ToString() + "'", Utils.Helper.constr, out err2);
                        DateTime tInTime = Convert.ToDateTime(drAttd["ConsIn"]);
                        DateTime tShiftStart = Convert.ToDateTime(drAttd["tDate"]);
                        DateTime tmpStart = Convert.ToDateTime(shiftstarttime);

                        tShiftStart = new DateTime(tShiftStart.Year, tShiftStart.Month, tShiftStart.Day, tmpStart.Hour, tmpStart.Minute, tmpStart.Second);
                        
                        //tShiftStart.AddHours(tmpStart.Hour);
                        //tShiftStart.AddMinutes(tmpStart.Minute);
                       
                        int tminute = 0;
                        
                        
                        if (tInTime < tShiftStart)
                        {
                            tInTime = tShiftStart;
                        }
                        

                        TimeSpan ts = (Convert.ToDateTime(drAttd["ConsOut"]) - tInTime);
                       
                        thour = ts.Hours;
                        tminute = ts.Minutes;

                        if (tminute > 30 && tminute <= 50)
                        {
                            thour += 0.5;
                        }
                        else if (tminute > 51)
                        {
                            thour += 1;
                        }

                        drAttd["StdWrkHrs"] = thour;

                    }
                    catch (Exception ex)
                    {

                    }

                    daAttdData.Update(dsAttdData, "AttdData");

                }


                if (Convert.ToDouble(drAttd["ConsWrkHrs"]) > 0 && Convert.ToDouble(drAttd["ConsOverTime"]) > 0)
                {
                    if ((drAttd["LeaveTyp"].ToString() == "WO" || drAttd["LeaveTyp"].ToString() == "PH"))
                    {
                        if (drAttd["WrkGrp"].ToString() == "CONT" && thour > 0 && thour > stdhrs )
                        {
                            drAttd["StdWrkShift"] = drAttd["ConsShift"]; 
                            drAttd["StdHrsOT"] = thour - stdhrs;
                        }
                        else if (drAttd["WrkGrp"].ToString() == "COMP")
                        {
                            drAttd["StdWrkShift"] = drAttd["LeaveTyp"].ToString();
                            drAttd["StdHrsOT"] = Convert.ToDouble(drAttd["ConsOverTime"]);
                        }                        
                        
                    }
                    else if (Convert.ToDouble(drAttd["StdWrkHrs"]) > stdhrs)
                    {
                        drAttd["StdHrsOT"] = Convert.ToDouble(drAttd["StdWrkHrs"]) - stdhrs;
                    }
                    else if (Convert.ToDouble(drAttd["StdWrkHrs"]) <= stdhrs)
                    {
                        drAttd["StdHrsOT"] = 0;
                    }
                }
                else if (Convert.ToDouble(drAttd["ConsWrkHrs"]) > 0 && Convert.ToDouble(drAttd["ConsOverTime"]) <= 0)
                {
                    if (drAttd["LeaveTyp"].ToString() == "WO" || drAttd["LeaveTyp"].ToString() == "PH")
                    { 
                        
                        drAttd["StdWrkShift"] = drAttd["LeaveTyp"].ToString();

                    }
                    
                }
                
            }
            else if (Convert.ToDouble(drAttd["ConsWrkHrs"]) <= 0 && Convert.ToDouble(drAttd["ConsOverTime"]) <= 0)
            {
                if (drAttd["LeaveTyp"].ToString() == "WO" || drAttd["LeaveTyp"].ToString() == "PH")
                {
                    drAttd["StdWrkShift"] = drAttd["LeaveTyp"].ToString();
                }
            }
            daAttdData.Update(dsAttdData, "AttdData");
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
