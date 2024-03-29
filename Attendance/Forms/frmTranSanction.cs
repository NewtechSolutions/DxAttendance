﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using Attendance.Classes;
using DevExpress.XtraGrid.Columns;
using System.Globalization;

namespace Attendance.Forms
{
    public partial class frmTranSanction : Form
    {
        private string GRights = "XXXV";
        private static int MeFormID ;//'get formid
        private static string MeRights = "XXXV" ;//' Allow Sanction otherthan limit
        private static string MeDelPunch = "XXXV" ; //' Allow Delete Punch
        private static string LunchRights = "XXXV" ; //'used for lunch punch prapanch
        private static string GateRights = "XXXV" ; //'used for Gate In/Out Punch prapance
        private static string LunchInOutConvertRights = "XXXV";
        private static string GatePassRights = "XXXV";
        private static string SheduleDelRights = "XXXV";

        private int rSanDayLimit = 0;


        private clsEmp Emp = new clsEmp();

        public frmTranSanction()
        {
            InitializeComponent();
            this.ctrlEmp1.EmpUnqIDValidated += new EventHandler(this.ctrlEmpValidateEvent_Handler);
            //this.ctrlEmp1.CompCodeValidated += new EventHandler(this.ctrlCompValidateEvent_Handler);
            txtSanDt.DateTime = DateTime.Now;
            txtSanDtLunch.DateTime = DateTime.Now;
            txtSanDtGate.DateTime = DateTime.Now;
        }

        private void ctrlEmpValidateEvent_Handler(object sender, EventArgs e)
        {
            //if (!ctrlEmp1.cEmp.Active)
            //{
            //    //grid.DataSource = null;
            //    Emp = new clsEmp();
            //    ResetCtrl();
            //}
            //else
            //{
            //    Emp = ctrlEmp1.cEmp;
            //    Emp.CompCode = Emp.CompCode;
            //    Emp.EmpUnqID = Emp.EmpUnqID;
            //    Emp.GetEmpDetails(Emp.CompCode, Emp.EmpUnqID);

            //    ////'added on 27/06/2016 using new security module
            //    //if (!Globals.GetWrkGrpRights(MeFormID, Emp.WrkGrp, Emp.EmpUnqID))
            //    //{
            //    //    //Emp = new clsEmp();
            //    //    MessageBox.Show("You are not Authorised to sanction,Please Contact System Administrator", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    //    //return;
            //    //}
                
            //    LoadGrid();
            //    SetRights();
            //    txtSanDt.Focus();
            //} 

            if(string.IsNullOrEmpty(ctrlEmp1.cEmp.EmpUnqID))
            {
                Emp = new clsEmp();
                ResetCtrl();
            }
            else
            {
                Emp = ctrlEmp1.cEmp;
            }

            
            Emp.CompCode = Emp.CompCode;
            Emp.EmpUnqID = Emp.EmpUnqID;
            Emp.GetEmpDetails( Emp.EmpUnqID);
            LoadGrid();
            SetRights();
            txtSanDt.Focus();

        }

        

        private void frmSanction_Load(object sender, EventArgs e)
        {
            ResetCtrl();

            GRights = Attendance.Classes.Globals.GetFormRights(this.Name);
            MeDelPunch = Attendance.Classes.Globals.GetFormRights("Tran Punch Delete");           
            //LunchRights = Attendance.Classes.Globals.GetFormRights("Tran Lunch Punch");
            //GateRights = Attendance.Classes.Globals.GetFormRights("Tran Gate Punch");
            //GatePassRights = Attendance.Classes.Globals.GetFormRights("Tran GatePass Punch");
            //LunchInOutConvertRights = Attendance.Classes.Globals.GetFormRights("Tran Convert Lunch InOut Punch To GatePass InOut");
            //SheduleDelRights = Attendance.Classes.Globals.GetFormRights("Tran DeleteShiftSchedule");

            MeFormID = Convert.ToInt32("0" + Utils.Helper.GetDescription("Select FormID from MastFrm Where FormName = 'frmTranSanction'", Utils.Helper.constr));

            #region loadlocations
            ////Lunch InOut load locations
            //string sql = "Select Distinct Location From ReaderConfig where LunchInOut = 1 Union Select Distinct Location From TripodReaderConfig Order By Location";
            //DataSet ds = Utils.Helper.GetData(sql,Utils.Helper.constr);
            //bool hasRows = ds.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);
            
            //if(hasRows)
            //{
            //    txtLocLunch.Properties.Items.Clear();

            //    foreach(DataRow dr in ds.Tables[0].Rows){
            //        txtLocLunch.Properties.Items.Add(dr["Location"].ToString());

            //    }

            //}

            //sql = "Select Distinct Location From ReaderConfig where GateInOut = 1 Union Select Distinct Location From TripodReaderConfig Order By Location";
            //ds =  ds = Utils.Helper.GetData(sql,Utils.Helper.constr);
            //hasRows = ds.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);
            
            //if(hasRows)
            //{
            //    txtLocGate.Properties.Items.Clear();
            //    txtLocGatePass.Properties.Items.Clear();
            //    foreach(DataRow dr in ds.Tables[0].Rows){
            //        txtLocGate.Properties.Items.Add(dr["Location"].ToString());
            //        txtLocGatePass.Properties.Items.Add(dr["Location"].ToString());
            //    }
            //}


            #endregion

            //set sanction limit in days based on other config.
            string cntdays = Utils.Helper.GetDescription("Select SanDayLimit From MastBCFlg where ValidFrom<=getDate() Order by ValidFrom Desc", Utils.Helper.constr);
            if(!string.IsNullOrEmpty(cntdays))
            {
                
                int.TryParse(cntdays, out rSanDayLimit);
                
            }
        }

        private string DataValidate_San()
        {
            string err = string.Empty;

            if (string.IsNullOrEmpty(ctrlEmp1.txtCompCode.Text.Trim().ToString()))
            {
                err = err + "Please Enter CompCode..." + Environment.NewLine;
            }
            
            if (string.IsNullOrEmpty(ctrlEmp1.txtEmpUnqID.Text.Trim().ToString()))
            {
                err = err + "Please Enter EmpUnqID..." + Environment.NewLine;
            }
            else
            {
                if (ctrlEmp1.txtEmpUnqID.Text.Trim().ToString() == Utils.User.GUserID)
                {
                    err = err + "Self Sanction is not allowed..." + Environment.NewLine;
                    return err;
                }
            }

            if (!string.IsNullOrEmpty(ctrlEmp1.cEmp.EmpUnqID) && !ctrlEmp1.IsValid )
            {
                err = err + "Invalid/InActive EmpUnqID..." + Environment.NewLine;
            }


            if (string.IsNullOrEmpty(ctrlEmp1.cEmp.CompDesc.Trim().ToString()))
            {
                err = err + "Invalid CompCode..." + Environment.NewLine;
            }

            if(txtSanDt.EditValue == null)
            {
                err += "Sanction Date Required..." + Environment.NewLine;
            }

            if(txtInOut.Text == "IN" && 
                (txtInTime.Text == "00:00" || txtInTime.EditValue == null || txtInTime.Text == "" )
                )
            {
                err += "In Time Required.." + Environment.NewLine;
            }
            
            if(txtInOut.Text == "OUT" && 
                (txtOutTime.Text == "00:00" || txtOutTime.EditValue == null || txtOutTime.Text == "" )
                )
            {
                err += "Out Time Required.." + Environment.NewLine;
            }
    
            if(txtInOut.Text == "BOTH" && 
                (txtInTime.Text == "00:00" || 
                txtOutTime.Text == "00:00" ||
                txtInTime.EditValue == null ||
                txtOutTime.EditValue == null ||
                txtInTime.Text == "" ||
                txtOutTime.Text == ""
                ))
            {
                err += "In/Out Time Required.." + Environment.NewLine;
            }
            
            if(chkTPA.Checked)
            {
                if(txtOT.Value == 0)
                {
                    DialogResult dr = MessageBox.Show("Are you sure to give Zero TPA ?","Question",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
                    if(dr == DialogResult.No){
                        err+= "TPA Hours Required..." + Environment.NewLine;
                    }
                }
            }
    
            if(chkShift.Checked && txtShiftCode.Text == "")
            {
                err += "ShiftCode Required..." + Environment.NewLine;
            }
        
            if(txtInOut.Text != "")
            {
                switch (txtInOut.Text.Trim().ToUpper())
                {
                    case "IN":
                        txtOutTime.EditValue = null;
                        break;
                    case "OUT" :
                        txtInTime.EditValue = null;
                        break;
                    case "BOTH":
                        break;
                    default :
                        txtInTime.EditValue = null;
                        txtOutTime.EditValue = null;
                        break;
                }
  
            }
    
            if(txtInOut.Text.Trim() == "" && chkShift.Checked == false && chkTPA.Checked == false && txtSanDt.EditValue == null)
            {
                err += "Please enter required data.." + Environment.NewLine;
            }else if (txtInOut.Text.Trim() == "" && chkShift.Checked == false && chkTPA.Checked == false)
            {
                err += "Please enter required data.." + Environment.NewLine;
            }
    
            if(string.IsNullOrEmpty(txtRemarks.Text.Trim().ToString()))
            {
                err += "Please enter Remark/Reason.." + Environment.NewLine;
            }

            //added dt 22/04/22 - as per audit point
            if(txtSanDt.DateTime.Date < Convert.ToDateTime(Emp.JoinDt).Date)
            {
                err += "Sanction Date could not be less than joining date" + Environment.NewLine;
            }

            return err;
        }

        #region NotRequired
        /*
        private string DataValidate_Lunch()
        {
            string err = string.Empty;

            if (string.IsNullOrEmpty(ctrlEmp1.txtCompCode.Text.Trim().ToString()))
            {
                err = err + "Please Enter CompCode..." + Environment.NewLine;
            }

            if (string.IsNullOrEmpty(ctrlEmp1.txtEmpUnqID.Text.Trim().ToString()))
            {
                err = err + "Please Enter EmpUnqID..." + Environment.NewLine;

            }
            else
            {
                if (ctrlEmp1.txtEmpUnqID.Text.Trim().ToString() == Utils.User.GUserID)
                {
                    err = err + "Self Sanction is not allowed..." + Environment.NewLine;
                    return err;
                }
            }
            

            if (!string.IsNullOrEmpty(ctrlEmp1.cEmp.EmpUnqID) && !ctrlEmp1.IsValid)
            {
                err = err + "Invalid/InActive EmpUnqID..." + Environment.NewLine;
            }
            


            if (string.IsNullOrEmpty(ctrlEmp1.cEmp.CompDesc.Trim().ToString()))
            {
                err = err + "Invalid CompCode..." + Environment.NewLine;
            }

            if (txtSanDtLunch.EditValue == null)
            {
                err += "Sanction Date Required..." + Environment.NewLine;
            }

            if (txtInOutLunch.Text.Trim() == "")
            {
                err += "Please Select In/Out.." + Environment.NewLine;
            }
            
            if(txtTimeLunch.Text == "00:00" || txtTimeLunch.EditValue == null || txtTimeLunch.Text == "")
            {
                err += "Time Required.." + Environment.NewLine;
            }

            if (txtLocLunch.Text.Trim() == "")
            {
                err += "Please Select Location.." + Environment.NewLine;
            }
            

            


            return err;
        }

        private string DataValidate_Gate()
        {
            string err = string.Empty;

            if (string.IsNullOrEmpty(ctrlEmp1.txtCompCode.Text.Trim().ToString()))
            {
                err = err + "Please Enter CompCode..." + Environment.NewLine;
            }

            if (string.IsNullOrEmpty(ctrlEmp1.txtEmpUnqID.Text.Trim().ToString()))
            {
                err = err + "Please Enter EmpUnqID..." + Environment.NewLine;
            }
            else
            {
                if (ctrlEmp1.txtEmpUnqID.Text.Trim().ToString() == Utils.User.GUserID)
                {
                    err = err + "Self Sanction is not allowed..." + Environment.NewLine;
                    return err;
                }
            }

            if (!string.IsNullOrEmpty(ctrlEmp1.cEmp.EmpUnqID) && !ctrlEmp1.IsValid)
            {
                err = err + "Invalid/InActive EmpUnqID..." + Environment.NewLine;
            }


            if (string.IsNullOrEmpty(ctrlEmp1.cEmp.CompDesc.Trim().ToString()))
            {
                err = err + "Invalid CompCode..." + Environment.NewLine;
            }

            if (txtSanDtGate.EditValue == null)
            {
                err += "Sanction Date Required..." + Environment.NewLine;
            }

            if (txtInOutGate.Text.Trim() == "")
            {
                err += "Please Select In/Out.." + Environment.NewLine;
            }

            if (txtTimeGate.Text == "00:00" || txtTimeGate.EditValue == null || txtTimeGate.Text == "")
            {
                err += "Time Required.." + Environment.NewLine;
            }

            if (txtLocGate.Text.Trim() == "")
            {
                err += "Please Select Location.." + Environment.NewLine;
            }





            return err;
        }

        private string DataValidate_GatePass()
        {
            string err = string.Empty;

            if (string.IsNullOrEmpty(ctrlEmp1.txtCompCode.Text.Trim().ToString()))
            {
                err = err + "Please Enter CompCode..." + Environment.NewLine;
            }

            if (string.IsNullOrEmpty(ctrlEmp1.txtEmpUnqID.Text.Trim().ToString()))
            {
                err = err + "Please Enter EmpUnqID..." + Environment.NewLine;
            }
            else
            {
                if (ctrlEmp1.txtEmpUnqID.Text.Trim().ToString() == Utils.User.GUserID)
                {
                    err = err + "Self Sanction is not allowed..." + Environment.NewLine;
                    return err;
                }
            }

            if (!string.IsNullOrEmpty(ctrlEmp1.cEmp.EmpUnqID) && !ctrlEmp1.IsValid)
            {
                err = err + "Invalid/InActive EmpUnqID..." + Environment.NewLine;
            }


            if (string.IsNullOrEmpty(ctrlEmp1.cEmp.CompDesc.Trim().ToString()))
            {
                err = err + "Invalid CompCode..." + Environment.NewLine;
            }

            if (txtSanDtGatePass.EditValue == null)
            {
                err += "Sanction Date Required..." + Environment.NewLine;
            }

            if (txtInOutGatePass.Text.Trim() == "")
            {
                err += "Please Select In/Out.." + Environment.NewLine;
            }

            if (txtTimeGatePass.Text == "00:00" || txtTimeGatePass.EditValue == null || txtTimeGatePass.Text == "")
            {
                err += "Time Required.." + Environment.NewLine;
            }

            if (txtLocGatePass.Text.Trim() == "")
            {
                err += "Please Select Location.." + Environment.NewLine;
            }





            return err;
        }
        */
        #endregion
        private void ResetCtrl()
        {
            ctrlEmp1.ResetCtrl();
            
            btn_San_Add.Enabled = false;
            btn_San_Del.Enabled = false;

            btnPunch_IO_Del.Enabled = false;

            btnGate_IO_San.Enabled = false;
            btnGate_IO_Del.Enabled = false;

            btnLunch_IO_Del.Enabled = false;
            btnLunch_IO_San.Enabled = false;

            btnLunch_Ignore.Enabled = false;

            btnGatePass_IO_Del.Enabled = false;
            btnGatePass_IO_San.Enabled = false;
            btnConvertToGatePass.Enabled = false;
            btnDeleteShiftSch.Enabled = false;
            //attd
            txtInOut.Text = "";
            
            txtInTime.Text = "";
            txtOutTime.Text = "";

            chkTPA.Checked = false;
            txtOT.Visible = false;
            txtOT.Text = "";
            txtOT.EditValue = null;

            chkShift.Checked = false;
            txtShiftCode.Visible = false;
            txtShiftCode.Text = "";
            txtRemarks.Text = "";
            
            
            //gate inout         
            txtTimeGate.Text = "";
            txtInOutGate.Text = "";
            txtLocGate.Text = "";

            //gatepass inout         
            txtTimeGatePass.Text = "";
            txtInOutGatePass.Text = "";
            txtLocGatePass.Text = "";



            //lunch inout
            txtTimeLunch.Text = "";
            txtInOutLunch.Text = "";
            txtLocLunch.Text = "";

            txtSanDt.DateTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-") + "01");
            txtSanDtGate.DateTime = txtSanDt.DateTime;
            txtSanDtLunch.DateTime = txtSanDt.DateTime;
            txtSanDtGatePass.DateTime = txtSanDt.DateTime;

            txtInTime.Time = txtSanDt.DateTime;
            txtOutTime.Time = txtSanDt.DateTime;
            txtTimeGate.Time = txtSanDt.DateTime;
            txtTimeLunch.Time = txtSanDt.DateTime;
            txtTimeGatePass.Time = txtSanDt.DateTime;


            object sender = new object();
            EventArgs e = new EventArgs();

            txtInOut_SelectedIndexChanged(sender, e);
            txtInOutGate_SelectedIndexChanged(sender, e);
            txtInOutGatePass_SelectedIndexChanged(sender, e);
            txtInOutLunch_SelectedIndexChanged(sender, e);

        }
        
        private void SetRights()
        {
            btn_San_Del.Enabled = false;
            btn_San_Add.Enabled = false;
            
            btnPunch_IO_Del.Enabled = false;

            btnGate_IO_Del.Enabled = false;
            btnGate_IO_San.Enabled = false;

            btnLunch_IO_Del.Enabled = false;
            btnLunch_IO_San.Enabled = false;

            btnLunch_Ignore.Enabled = false;

            btnGatePass_IO_Del.Enabled = false;
            btnGatePass_IO_San.Enabled = false;
            btnConvertToGatePass.Enabled = false;
            btnDeleteShiftSch.Enabled = false;


            #region Sanction_Add_Delete_300
            //Sanction
            //if (Globals.GetWrkGrpRights(300, Emp.WrkGrp, Emp.EmpUnqID))
            //{
                if (Emp.EmpUnqID != "" && (GRights.Contains("A") || GRights.Contains("U")))
                {
                    btn_San_Add.Enabled = true;
                }

                if (Emp.EmpUnqID != "" && GRights.Contains("D"))
                {
                    btn_San_Del.Enabled = true;
                }

                if (Emp.EmpUnqID != "" && GRights.Contains("XXXV"))
                {
                    btn_San_Add.Enabled = false;
                    btn_San_Del.Enabled = false;
                }
            //}
            #endregion

            #region MachinePunchAttd_460
            //InOut Machine Punch Delete
            //Tran Punch Delete
            if (MeDelPunch.Contains("D"))
            {
                
                //if (Globals.GetWrkGrpRights(460, Emp.WrkGrp, Emp.EmpUnqID))
                //{
                    btnPunch_IO_Del.Enabled = true;
                //}
            }
            #endregion

            #region Lunch_InOut_Punch_Ignore_470
            //"Tran Lunch Punch
            if (LunchRights.Contains("A"))
            {
                //lunch-halfday ignore/unpost
                //if (Globals.GetWrkGrpRights(470, Emp.WrkGrp, Emp.EmpUnqID))
                //{
                    btnLunch_IO_San.Enabled = true;   
                //}

            }

            if(LunchRights.Contains("D")){
                //if(Globals.GetWrkGrpRights(470,Emp.WrkGrp,Emp.EmpUnqID))
                //{
                    btnLunch_IO_Del.Enabled = true;
                    btnLunch_Ignore.Enabled = true;
                //}
            }

            #endregion

            #region Gate_InOut_Punch_471
            if(GateRights.Contains("A"))
            {
                //Gate In Out Punch
                //if (Globals.GetWrkGrpRights(471, Emp.WrkGrp, Emp.EmpUnqID))
                //{
                    btnGate_IO_San.Enabled = true;
                //}               
            }

            if(GateRights.Contains("D"))
            {
                //Gate In Out Punch
                //if (Globals.GetWrkGrpRights(471, Emp.WrkGrp, Emp.EmpUnqID))
                //{
                    btnGate_IO_Del.Enabled = true;
                //}               
            }   


            #endregion

            #region GatePass_InOut_Punch_1000

            if (GatePassRights.Contains("A"))
            {
                //GatePass In Out Punch
                //if (Globals.GetWrkGrpRights(1000, Emp.WrkGrp, Emp.EmpUnqID))
                //{
                    btnGatePass_IO_San.Enabled = true;
                //}
            }

            if (GatePassRights.Contains("D"))
            {
                //Gate In Out Punch
                //if (Globals.GetWrkGrpRights(1000, Emp.WrkGrp, Emp.EmpUnqID))
                //{
                    btnGatePass_IO_Del.Enabled = true;
                //}
            }   


            #endregion

            #region ConvertLunchInOut_To_GatePass_InOut_Punch_1005

            if (LunchInOutConvertRights.Contains("A"))
            {
                //GatePass In Out Punch
                //if (Globals.GetWrkGrpRights(1005, Emp.WrkGrp, Emp.EmpUnqID))
                //{
                    btnConvertToGatePass.Enabled = true;
                //}
            }


            #endregion


            #region DeleteShiftSchDel
            if (SheduleDelRights.Contains("D"))
            {
                //shift schedule delete rights employee -workgrp

                //if (Globals.GetWrkGrpRights(1090, Emp.WrkGrp, Emp.EmpUnqID))
                //{
                //    //btnDeleteShiftSch.Enabled = true;
                    
                //    if(txtSanDt.EditValue != null)
                //    {
                //        //check if schedule exist or not
                //        int yearmt = int.Parse(txtSanDt.DateTime.Date.ToString("yyyyMM"));
                //        string sql = "Select Count(*) from mastShiftSchedule where Empunqid = '" + Emp.EmpUnqID + "' And YearMt = '" + yearmt.ToString() + "'";
                //        string err = string.Empty;
                //        string strcnt = Utils.Helper.GetDescription(sql, Utils.Helper.constr, out err);
                //        if (string.IsNullOrEmpty(err))
                //        {
                //            int cnt = 0;
                //            int.TryParse(strcnt, out cnt);
                //            if (cnt >= 1)
                //            {
                //                if(Emp.WrkGrp.Trim().ToUpper() == "CONT")
                //                    btnDeleteShiftSch.Enabled = true;
                //            }
                //        }

                //    }


                //}
            }

            #endregion

        }

        private void LoadGrid()
        {

            #region Chk_Primary

            if (txtSanDt.EditValue == null)
            {
                ResetGrid();
                return;
            }
            if (txtSanDt.DateTime == DateTime.MinValue)
            {
                ResetGrid();
                return;
            }

            if (Emp.EmpUnqID == string.Empty)
            {
                ResetGrid();
                return;
            }

            if (Emp.Active == false)
            {
                ResetGrid();
                return;
            }

            #endregion

            string SqlAttd = string.Empty;
            string SqlPunch = string.Empty;
            string SqlSanc = string.Empty;
            string SqlGatePunch = string.Empty;
            string SqlGatePass = string.Empty;
            string SqlLunch = string.Empty;
            string sqlLunchDt = string.Empty;
            
            string sqlTripod = string.Empty;

            string FromDt = string.Empty;
            string ToDt = string.Empty;

            FromDt = txtSanDt.DateTime.ToString("yyyy-MM-dd");



            SqlAttd = "Select Top 40 " +
                    " tDate, upper(left(datename(dw, tdate),3)) as Day, ScheduleShift as SchShift,ConsShift, ConsIn, ConsOut, Status,ConsOverTime as TPAHrs, ConsWrkHrs as WrkHrs,  HalfDay,LeaveTyp,LeaveHalf,LateCome,EarlyGoing,EarlyCome " +
                    " ,GracePeriod From AttdData " +
                    " Where EmpUnqId ='" + Emp.EmpUnqID + "' And tDate >= '" + FromDt + "' AND CompCode = '01' And WrkGrp = '" + Emp.WrkGrp + "' Order By tDate" ;
    
            SqlSanc = "Select  " +
                      " SanID,tDate,ConsInTime,ConsOutTime,ConsOverTime,ConsShift,SchLeave,AddID,AddDT,Remarks " +
                      " From MastLeaveSchedule " +
                      " Where EmpUnqId ='" + Emp.EmpUnqID + "' And tDate between (select CalendarStartOfMonthDate from dbo.F_TABLE_DATE('" + FromDt + "','" + FromDt + "')) And (select CalendarEndOfMonthDate from dbo.F_TABLE_DATE('" + FromDt + "','" + FromDt + "'))  " +
                      " And (ConsInTime is not null OR ConsOutTime is not null OR ConsOverTime is not null Or ConsShift is not null) Order By SanID Desc ";

            SqlPunch = "Select Top 100 " +
                      " PunchDate,IOFLG,MachineIP,AddDt,AddID " +
                      " From AttdLog " +
                      " Where EmpUnqId ='" + Emp.EmpUnqID + "' And PunchDate >= '" + FromDt + "' and LunchFlg = 0 and IOFLG in ('I','O') Order by PunchDate";


            //'Punch Details
            DataSet ds = Utils.Helper.GetData(SqlAttd, Utils.Helper.constr);
            bool hasRows = ds.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);
            if (hasRows) { grd_Attd.DataSource = ds.Tables[0]; } else { grd_Attd.DataSource = null; }


            ds = Utils.Helper.GetData(SqlPunch, Utils.Helper.constr);
            hasRows = ds.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);
            if (hasRows) { grd_InOut.DataSource = ds.Tables[0]; } else { grd_InOut.DataSource = null; }

            ds = Utils.Helper.GetData(SqlSanc, Utils.Helper.constr);
            hasRows = ds.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);
            if (hasRows) { grd_Sanction.DataSource = ds.Tables[0]; } else { grd_Sanction.DataSource = null; }

            #region notrequired
            /*
            sqlTripod = "Select Top 200 " +
                     " PunchDate,IOFLG,MachineIP,AddDt,AddID " +
                     " From TripodLog " +
                     " Where EmpUnqId ='" + Emp.EmpUnqID + "' And PunchDate >= '" + FromDt + "' and IOFLG in ('I','O') Order by PunchDate";


            SqlGatePunch = "Select Top 100 " +
                      " PunchDate,IOFLG,MachineIP,AddDt,AddID " +
                      " From AttdGateInOut " +
                      " Where EmpUnqId ='" + Emp.EmpUnqID + "' And PunchDate >= '" + FromDt + "' and LunchFlg = 0 and IOFLG in ('I','O') Order by PunchDate";



            SqlLunch = "Select Top 100 " +
                      " PunchDate,IOFLG,MachineIP,AddDt,AddID " +
                      " From AttdLunchGate " +
                      " Where EmpUnqId ='" + Emp.EmpUnqID + "' And PunchDate >= '" + FromDt + "' and IOFLG in ('I','O') " +
                      " Union " +
                      " Select Top 100 " +
                      " PunchDate,IOFLG,MachineIP,AddDt,AddID " +
                      " From AttdLog " +
                      " Where EmpUnqId ='" + Emp.EmpUnqID + "' And PunchDate >= '" + FromDt + "' and IOFLG = 'B' Order by PunchDate ";
              

            sqlLunchDt = "Select Top 40 tDate,[Shift] as Shft," +
                      " LunchLocation ,LunchOuttime as LunchOut, LunchTime as Lunch, LunchInTime as LunchIn, LunchRemarks," +
                      " DinnerLocation,DinnerOuttime as DinnerOut, DinnerTime as Dinner, DinnerInTime as DinnerIn, DinnerRemarks," +
                      " LeaveStatus,LeaveHalf,Ignore,Posted,ignoreBy " +
                      " From AttdLunchHistory " +
                      " Where EmpUnqId ='" + Emp.EmpUnqID + "' And tDate >= '" + FromDt + "' ";


            SqlGatePass = "Select Top 100 " +
                      " PunchDate,IOFLG,MachineIP,AddDt,AddID " +
                      " From AttdGatePassInOut " +
                      " Where EmpUnqId ='" + Emp.EmpUnqID + "' And PunchDate >= '" + FromDt + "' and LunchFlg = 0 and IOFLG in ('I','O') Order by PunchDate";

           

            

            
            ds = Utils.Helper.GetData(SqlGatePunch, Utils.Helper.constr);
            hasRows = ds.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);
            if (hasRows) { grd_InOutGate.DataSource = ds.Tables[0]; } else { grd_InOutGate.DataSource = null; }

            ds = Utils.Helper.GetData(SqlLunch, Utils.Helper.constr);
            hasRows = ds.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);
            if (hasRows) { grd_InOutLunch.DataSource = ds.Tables[0]; } else { grd_InOutLunch.DataSource = null; }
            



            ds = Utils.Helper.GetData(sqlLunchDt, Utils.Helper.constr);
            hasRows = ds.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);
            if (hasRows) { grd_LunchDt.DataSource = ds.Tables[0]; } else { grd_LunchDt.DataSource = null; }

            //grd_InOutGatePass
            ds = Utils.Helper.GetData(SqlGatePass, Utils.Helper.constr);
            hasRows = ds.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);
            if (hasRows) { grd_InOutGatePass.DataSource = ds.Tables[0]; } else { grd_InOutGatePass.DataSource = null; }

            ds = Utils.Helper.GetData(sqlTripod, Utils.Helper.constr);
            hasRows = ds.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);
            if (hasRows) { grd_Tripod.DataSource = ds.Tables[0]; } else { grd_Tripod.DataSource = null; }
            */
            #endregion


            GridFormat();

        }

        private void ResetGrid()
        {
            grd_Attd.DataSource = null;
            grd_Sanction.DataSource = null;

            grd_InOut.DataSource = null;
            
            //grd_InOutGate.DataSource = null;

            //grd_InOutLunch.DataSource = null;
            //grd_LunchDt.DataSource = null;

            //grd_InOutGatePass.DataSource = null;
            //grd_Tripod.DataSource = null;
        }

        private void GridFormat()
        {
            gv_Attd.Appearance.HeaderPanel.Font = new System.Drawing.Font(gv_Attd.Appearance.ViewCaption.Font, FontStyle.Bold);
            gv_Attd.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            //gv_Attd.Appearance.HeaderPanel.Options.UseFont = true;
            try
            {
                gv_Attd.Columns["ConsIn"].Width = 100;
                gv_Attd.Columns["ConsOut"].Width = 100;

            }
            catch
            {

            }

            gv_Sanction.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            gv_Sanction.Appearance.HeaderPanel.Font = new System.Drawing.Font(gv_Attd.Appearance.ViewCaption.Font, FontStyle.Bold);

            gv_InOut.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            gv_InOut.Appearance.HeaderPanel.Font = new System.Drawing.Font(gv_Attd.Appearance.ViewCaption.Font, FontStyle.Bold);

            //gv_InOutGate.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            //gv_InOutGate.Appearance.HeaderPanel.Font = new System.Drawing.Font(gv_Attd.Appearance.ViewCaption.Font, FontStyle.Bold);

            //gv_InOutLunch.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            //gv_InOutLunch.Appearance.HeaderPanel.Font = new System.Drawing.Font(gv_Attd.Appearance.ViewCaption.Font, FontStyle.Bold);

            

            //gv_LunchDt.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            //gv_LunchDt.Appearance.HeaderPanel.Font = new System.Drawing.Font(gv_Attd.Appearance.ViewCaption.Font, FontStyle.Bold);

            //gv_InOutGatePass.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            //gv_InOutGatePass.Appearance.HeaderPanel.Font = new System.Drawing.Font(gv_Attd.Appearance.ViewCaption.Font, FontStyle.Bold);

            //gv_Tripod.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            //gv_Tripod.Appearance.HeaderPanel.Font = new System.Drawing.Font(gv_Tripod.Appearance.ViewCaption.Font, FontStyle.Bold);

            GridColumn colDate = new GridColumn();
            
            if (grd_Attd.DataSource != null)
            {
                colDate = gv_Attd.Columns["tDate"];
                colDate.DisplayFormat.Format = new CultureInfo("en");
                colDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
                colDate.DisplayFormat.FormatString = "dd/MM/yy";
            
                colDate = gv_Attd.Columns["ConsIn"];
                colDate.DisplayFormat.Format = new CultureInfo("en");
                colDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
                colDate.DisplayFormat.FormatString = "dd/MM/yy HH:mm:ss";
                        
                colDate = gv_Attd.Columns["ConsOut"];
                colDate.DisplayFormat.Format = new CultureInfo("en");
                colDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
                colDate.DisplayFormat.FormatString = "dd/MM/yy HH:mm:ss";

                foreach (GridColumn gc in gv_Attd.Columns)
                {
                    gc.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    gc.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                }
            }

            if (grd_Sanction.DataSource != null)
            {
                //sanction
                colDate = gv_Sanction.Columns["ConsInTime"];
                colDate.DisplayFormat.Format = new CultureInfo("en");
                colDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
                colDate.DisplayFormat.FormatString = "dd/MM/yy HH:mm:ss";

                colDate = gv_Sanction.Columns["tDate"];
                colDate.DisplayFormat.Format = new CultureInfo("en");
                colDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
                colDate.DisplayFormat.FormatString = "dd/MM/yy";

                colDate = gv_Sanction.Columns["ConsOutTime"];
                colDate.DisplayFormat.Format = new CultureInfo("en");
                colDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
                colDate.DisplayFormat.FormatString = "dd/MM/yy HH:mm:ss";

                foreach (GridColumn gc in gv_Sanction.Columns)
                {
                    gc.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    gc.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                }
            }


            if (grd_InOut.DataSource != null)
            {
                //inout punch
                colDate = gv_InOut.Columns["PunchDate"];
                colDate.DisplayFormat.Format = new CultureInfo("en");
                colDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
                colDate.DisplayFormat.FormatString = "dd/MM/yyyy HH:mm:ss";

                colDate = gv_InOut.Columns["AddDt"];
                colDate.DisplayFormat.Format = new CultureInfo("en");
                colDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
                colDate.DisplayFormat.FormatString = "dd/MM/yy HH:mm:ss";


                foreach (GridColumn gc in gv_InOut.Columns)
                {
                    gc.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    gc.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                }

            }

            #region notrequired
            /****

            if (grd_InOutLunch.DataSource != null)
            {
                //lunch inout
                colDate = gv_InOutLunch.Columns["PunchDate"];
                colDate.DisplayFormat.Format = new CultureInfo("en");
                colDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
                colDate.DisplayFormat.FormatString = "dd/MM/yyyy HH:mm:ss";

                colDate = gv_InOutLunch.Columns["AddDt"];
                colDate.DisplayFormat.Format = new CultureInfo("en");
                colDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
                colDate.DisplayFormat.FormatString = "dd/MM/yyyy HH:mm:ss";

                foreach (GridColumn gc in gv_InOutLunch.Columns)
                {
                    gc.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    gc.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                }
            }


            if (grd_InOutGate.DataSource != null)
            {
                //lunch inout
                colDate = gv_InOutGate.Columns["PunchDate"];
                colDate.DisplayFormat.Format = new CultureInfo("en");
                colDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
                colDate.DisplayFormat.FormatString = "dd/MM/yyyy HH:mm:ss";

                colDate = gv_InOutGate.Columns["AddDt"];
                colDate.DisplayFormat.Format = new CultureInfo("en");
                colDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
                colDate.DisplayFormat.FormatString = "dd/MM/yyyy HH:mm:ss";

                foreach (GridColumn gc in gv_InOutGate.Columns)
                {
                    gc.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    gc.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                }

            }

            if (grd_InOutGatePass.DataSource != null)
            {
                //lunch inout
                colDate = gv_InOutGatePass.Columns["PunchDate"];
                colDate.DisplayFormat.Format = new CultureInfo("en");
                colDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
                colDate.DisplayFormat.FormatString = "dd/MM/yyyy HH:mm:ss";

                colDate = gv_InOutGatePass.Columns["AddDt"];
                colDate.DisplayFormat.Format = new CultureInfo("en");
                colDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
                colDate.DisplayFormat.FormatString = "dd/MM/yyyy HH:mm:ss";

                foreach (GridColumn gc in gv_InOutGatePass.Columns)
                {
                    gc.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    gc.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                }

            }


            if (grd_Tripod.DataSource != null)
            {
                //lunch inout
                colDate = gv_Tripod.Columns["PunchDate"];
                colDate.DisplayFormat.Format = new CultureInfo("en");
                colDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
                colDate.DisplayFormat.FormatString = "dd/MM/yyyy HH:mm:ss";

                colDate = gv_Tripod.Columns["AddDt"];
                colDate.DisplayFormat.Format = new CultureInfo("en");
                colDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
                colDate.DisplayFormat.FormatString = "dd/MM/yyyy HH:mm:ss";

                foreach (GridColumn gc in gv_Tripod.Columns)
                {
                    gc.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    gc.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                }

            }

            if (grd_LunchDt.DataSource != null)
            {
                //lunch Details
                colDate = gv_LunchDt.Columns["tDate"];
                colDate.DisplayFormat.Format = new CultureInfo("en");
                colDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
                colDate.DisplayFormat.FormatString = "dd/MM/yy";

                colDate = gv_LunchDt.Columns["LunchOut"];
                colDate.DisplayFormat.Format = new CultureInfo("en");
                colDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
                colDate.DisplayFormat.FormatString = "HH:mm";

                colDate = gv_LunchDt.Columns["Lunch"];
                colDate.DisplayFormat.Format = new CultureInfo("en");
                colDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
                colDate.DisplayFormat.FormatString = "HH:mm";

                colDate = gv_LunchDt.Columns["LunchIn"];
                colDate.DisplayFormat.Format = new CultureInfo("en");
                colDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
                colDate.DisplayFormat.FormatString = "HH:mm";

                colDate = gv_LunchDt.Columns["DinnerOut"];
                colDate.DisplayFormat.Format = new CultureInfo("en");
                colDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
                colDate.DisplayFormat.FormatString = "HH:mm";

                colDate = gv_LunchDt.Columns["Dinner"];
                colDate.DisplayFormat.Format = new CultureInfo("en");
                colDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
                colDate.DisplayFormat.FormatString = "HH:mm";

                colDate = gv_LunchDt.Columns["DinnerIn"];
                colDate.DisplayFormat.Format = new CultureInfo("en");
                colDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
                colDate.DisplayFormat.FormatString = "HH:mm";


                foreach (GridColumn gc in gv_LunchDt.Columns)
                {
                    gc.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    gc.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                }
            }

            */
            #endregion
        }

        private void txtSanDt_EditValueChanged(object sender, EventArgs e)
        {
            
        }

        private void txtInOut_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtInTime.EditValue = null;
            txtOutTime.EditValue = null;
            
            if (Emp.EmpUnqID != "")
            {
                if (txtInOut.Text == "IN")
                {
                    lblInTime.Visible = true;
                    txtInTime.Visible = true;
                    lblOutTime.Visible = false;
                    txtOutTime.Visible = false;
                }

                if (txtInOut.Text == "OUT")
                {
                    lblInTime.Visible = false;
                    txtInTime.Visible = false;
                
                    lblOutTime.Visible = true;
                    txtOutTime.Visible = true;

                }
                
                if (txtInOut.Text == "BOTH")
                {
                    lblInTime.Visible = true;
                    txtInTime.Visible = true;
                    lblOutTime.Visible = true;
                    txtOutTime.Visible = true;
                }
                
                if(txtInOut.Text == "")
                {
                    txtInTime.Visible = false;
                    txtOutTime.Visible = false;
                 
                    
                    lblInTime.Visible = false;
                    lblOutTime.Visible = false;
                }
                    
                
            }
            else
            {

                txtInTime.Visible = false;
                txtOutTime.Visible = false;
                txtShiftCode.Visible = false;
                lblInTime.Visible = false;
                lblOutTime.Visible = false;

            }
            
        }

        private void txtInOutLunch_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtTimeLunch.EditValue = null;
            
            if (Emp.EmpUnqID != "")
            {
                if (txtInOutLunch.Text != "")
                {
                    txtTimeLunch.Visible = true;
                }
                else
                {
                    txtTimeLunch.Visible = false;
                }

            }
            else
            {
                txtTimeLunch.Visible = false;
            }
        }

        private void txtInOutGate_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtTimeGate.EditValue = null;

            if (Emp.EmpUnqID != "")
            {
                if (txtInOutGate.Text != "")
                {
                    txtTimeGate.Visible = true;
                }
                else
                {
                    txtTimeGate.Visible = false;
                }

            }
            else
            {
                txtTimeGate.Visible = false;
            }
        }

        private void txtInOutGatePass_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtTimeGatePass.EditValue = null;

            if (Emp.EmpUnqID != "")
            {
                if (txtInOutGatePass.Text != "")
                {
                    txtTimeGatePass.Visible = true;
                }
                else
                {
                    txtTimeGatePass.Visible = false;
                }

            }
            else
            {
                txtTimeGatePass.Visible = false;
            }
        }


        private void chkShift_CheckedChanged(object sender, EventArgs e)
        {
            if (Emp.EmpUnqID != "")
            {
                if (chkShift.Checked)
                {
                    txtShiftCode.Visible = true;                   
                }
                else
                {
                    txtShiftCode.Visible = false;                   
                }
            }
            else
            {
                txtShiftCode.Visible = false;               
            }

            txtShiftCode.Text = "";
        }

        private void txtShiftCode_KeyDown(object sender, KeyEventArgs e)
        {

            if (Emp.EmpUnqID == "")
            {
                txtShiftCode.Text = "";
                return;
            }

            if (ctrlEmp1.txtCompCode.Text.Trim() == "")
                return;

            if (e.KeyCode == Keys.F1 || e.KeyCode == Keys.F2)
            {
                List<string> obj = new List<string>();

                Help_F1F2.ClsHelp hlp = new Help_F1F2.ClsHelp();
                string sql = "";


                sql = "Select ShiftCode,ShiftDesc from MastShift Where CompCode ='" + ctrlEmp1.txtCompCode.Text.Trim() + "' ";
                if (e.KeyCode == Keys.F1)
                {

                    obj = (List<string>)hlp.Show(sql, "ShiftCode", "ShiftCode", typeof(string), Utils.Helper.constr, "System.Data.SqlClient",
                   100, 300, 400, 600, 100, 100);
                }

                if (obj.Count == 0)
                {

                    return;
                }
                else if (obj.ElementAt(0).ToString() == "0")
                {
                    return;
                }
                else if (obj.ElementAt(0).ToString() == "")
                {
                    return;
                }
                else
                {

                    txtShiftCode.Text = obj.ElementAt(0).ToString();
                   
                }
            }
        }

        private void btnSanAttd_Click(object sender, EventArgs e)
        {
            string err = DataValidate_San();

            if (!string.IsNullOrEmpty(err))
            {
                MessageBox.Show(err, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            DateTime curDate ,reqDate ;
            curDate = Globals.GetSystemDateTime();
            reqDate = txtSanDt.DateTime;

            //'Added on 21/11/2014 as required do not let enter intime outtime prerier and "past entry must be within 2days"
                
            if(Utils.User.IsAdmin == false)
            {
                if (txtInTime.Time.Hour > 0|| txtOutTime.Time.Hour > 0)
                {
                    TimeSpan t = reqDate - curDate;

                    if (t.Days > rSanDayLimit)
                    {
                        MessageBox.Show("System Does not allow to sanction unless fall within : " +
                            rSanDayLimit.ToString() + " days...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        return;
                    }

                }   
            }

            if (txtInTime.Time.Hour > 0 || txtOutTime.Time.Hour > 0 || txtOT.Value > 0)
            {
                
                if (reqDate > curDate.Date)
                {
                    string tmpsql = "Select Count(*) from MastSanctionException where '" + reqDate.ToString("yyyy-MM-dd") + "' between FromDt and Todate";
                    string err2 = string.Empty;
                    string tid = Utils.Helper.GetDescription(tmpsql, Utils.Helper.constr, out err2);
                    if (string.IsNullOrEmpty(err2))
                    {

                        if (Convert.ToInt32(tid) <= 0)
                        {
                            MessageBox.Show("Future date sanction (In Time/Out Time/TPA Hours) denied", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;

                        }
                    }
                    
                    
                    
                    //MessageBox.Show("Future date sanction (In Time/Out Time/TPA Hours) denied", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //return;                    
                }

            }   

            string  sql ;
            string  sqlintime = string.Empty ,sqlouttime = string.Empty, sqlShift = string.Empty, sqlOt = string.Empty, sqlremarks = string.Empty;
   
            sql = "Insert Into MastLeaveSchedule " +
                " (tDate,EmpUnqID,WrkGrp,ConsInTime,ConsOutTime,ConsOverTime,ConsShift,AddId,AddDt,Remarks) Values (" +
                " '" + reqDate.ToString("yyyy-MM-dd") + "','" + Emp.EmpUnqID + "','" + Emp.WrkGrp + "'" ;
    
            if (txtInTime.EditValue == null || txtInTime.Text == "00:00") 
            {
                sqlintime = " null ";
            }else{
                sqlintime = "'" + txtSanDt.DateTime.ToString("yyyy-MM-dd") + " " + txtInTime.Time.ToString("HH:mm") + "'";
            }

            if (txtOutTime.EditValue == null || txtOutTime.Text == "00:00") 
            {
                sqlouttime = " null ";

            }else
            {
                if(
                    (txtOutTime.Text != "00:00" || txtOutTime.EditValue != null)   &&  
                    (txtInTime.Text != "00:00" || txtInTime.EditValue != null)
                    )
                {
                    if(txtOutTime.Time < txtInTime.Time) 
                    {
                        DateTime tDate = txtSanDt.DateTime.AddDays(1);
                        sqlouttime = "'" + tDate.ToString("yyyy-MM-dd") + " " + txtOutTime.Time.ToString("HH:mm") + "'";
                    }else if(txtOutTime.Time > txtInTime.Time)
                    {
                        sqlouttime = "'" + txtSanDt.DateTime.ToString("yyyy-MM-dd") + " " + txtOutTime.Time.ToString("HH:mm") + "'";
                    }
                }else
                {
                    sqlouttime = "'" + txtSanDt.DateTime.ToString("yyyy-MM-dd")+ " " + txtOutTime.Time.ToString("HH:mm") + "'";
                }           
            }

            if(txtRemarks.Text.Trim() != ""){
                sqlremarks = "'" + txtRemarks.Text.Trim() + "'";
            }else{
                sqlremarks = " null ";
            }
    
            if(chkShift.Checked){
                sqlShift = "'" + txtShiftCode.Text.Trim() + "'";
            }else{
                sqlShift = " null ";
            }
    
            if(chkTPA.Checked){
                sqlOt = "'" + txtOT.Value.ToString() + "'";
            }else{
                sqlOt = " null ";
            }

            sql = sql + "," + sqlintime + "," + sqlouttime + "," + sqlOt + "," + sqlShift + ",'" + Utils.User.GUserID + "',GetDate() , " + sqlremarks + " )";

            using (SqlConnection cn = new SqlConnection(Utils.Helper.constr))
            {
                try
                {
                    cn.Open();
                    SqlCommand cmd = new SqlCommand(sql, cn);
                    cmd.ExecuteNonQuery();

                    

                    DateTime sFromDt, sToDate;
                    sFromDt = txtSanDt.DateTime.AddDays(-1);
                    sToDate = txtSanDt.DateTime.AddDays(1);

                    clsProcess pro = new clsProcess();
                    int res = 0;string errpro = string.Empty;
                    pro.AttdProcess(Emp.EmpUnqID, sFromDt, sToDate, out res, out errpro);

                    if (!string.IsNullOrEmpty(errpro))
                    {
                        MessageBox.Show("Data Process Error : " + errpro, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    
                    txtInOut.Text = "";
                    txtInTime.EditValue = null;
                    txtOT.Value = 0;
                    txtShiftCode.Text = "";
                    txtOutTime.EditValue = null;
                    chkTPA.Checked = false;
                    chkShift.Checked = false;
                    //txtRemarks.Text = "";
                    LoadGrid();

                    MessageBox.Show("Sanctioned...", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ctrlEmp1.txtEmpUnqID.Focus();
                    return;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    
                }
            }

        }

        private void chkTPA_CheckedChanged(object sender, EventArgs e)
        {
            if (Emp.EmpUnqID != "")
            {
                if (chkTPA.Checked)
                {
                    txtOT.Visible = true;
                }
                else
                {
                    txtOT.Visible = false;
                   
                }
            }
            else
            {
                txtOT.Visible = false;
               
            }

            txtOT.EditValue = 0;
        }


        private void btnDel_IOPunch_Click(object sender, EventArgs e)
        {
            if (gv_InOut.SelectedRowsCount == 0)
            {
                MessageBox.Show("Please Select a Row First..", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            DialogResult dr = MessageBox.Show("Are you sure to Delete selected row ?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.No)
            {
                return;
            }

            foreach (int i in gv_InOut.GetSelectedRows())
            {
                string cellpunch = gv_InOut.GetRowCellValue(i, "PunchDate").ToString().Trim();
                string cellio = gv_InOut.GetRowCellValue(i, "IOFLG").ToString().Trim();
                string cellip = gv_InOut.GetRowCellValue(i, "MachineIP").ToString().Trim();

                using (SqlConnection cn = new SqlConnection(Utils.Helper.constr))
                {
                    try
                    {                        
                        cn.Open();
                    }
                    catch (Exception ex)
                    {                        
                        MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    SqlTransaction tr = cn.BeginTransaction();
                    try
                    {
                        DateTime tDate = Convert.ToDateTime(cellpunch);

                        string sql = "Insert into AttdLOG_Delete (EmpUnqID,PunchDate,IOFLG,MachineIP,AddDt,AddId,tYear,tYearMt) Values (" +
                        " '" + Emp.EmpUnqID + "','" + tDate.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                        " '" + cellio + "','" + cellip + "',GetDate(),'" + Utils.User.GUserID + "','" + tDate.Year + "','" + tDate.ToString("yyyyMM") + "' )";
                        SqlCommand cmd = new SqlCommand(sql, cn);
                        cmd.Transaction = tr;
                        cmd.ExecuteNonQuery();

                        sql = "Delete From AttdLog Where EmpUnqID ='" + Emp.EmpUnqID + "' and PunchDate ='" + tDate.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                            " And MachineIP ='" + cellip + "' And IOFLG = '" + cellio + "' and tYear ='" + tDate.Year.ToString() + "'";

                        cmd = new SqlCommand(sql, cn);
                        cmd.Transaction = tr;
                        cmd.ExecuteNonQuery();
                        tr.Commit();

                        DateTime tempdt = new DateTime(), sFromDt = new DateTime(), sToDate = new DateTime();
                        if (cellpunch != "")
                        {
                            tempdt = Convert.ToDateTime(cellpunch);
                            sFromDt = tempdt.AddDays(-1);
                            sToDate = tempdt.AddDays(1);
                        }                       

                        if (sFromDt != DateTime.MinValue && sToDate != DateTime.MinValue)
                        {
                            clsProcess pro = new clsProcess();
                            int res = 0; string errpro = string.Empty;
                            pro.AttdProcess(Emp.EmpUnqID, sFromDt, sToDate, out res, out errpro);

                            if (!string.IsNullOrEmpty(errpro))
                            {
                                MessageBox.Show("Data Process Error : " + errpro, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }


                        LoadGrid();


                    }
                    catch (Exception ex)
                    {
                        tr.Rollback();
                        MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }
        }

        private void btn_San_Del_Click(object sender, EventArgs e)
        {
            if (gv_Sanction.SelectedRowsCount == 0)
            {
                MessageBox.Show("Please Select a Row First..", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            
            DialogResult dr = MessageBox.Show("Are you sure to Delete selected row ?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.No)
            {
                return;
            }

            foreach(int i in gv_Sanction.GetSelectedRows())
            {
                string celltext = gv_Sanction.GetRowCellValue(i, "SanID").ToString();
                if (celltext != "")
                {
                    long sanid = 0;
                    long.TryParse(celltext,out sanid);
                    if (sanid > 0)
                    {
                        using (SqlConnection cn = new SqlConnection(Utils.Helper.constr))
                        {
                            try
                            {
                                cn.Open();
                                

                                string celldate = gv_Sanction.GetRowCellValue(i, "tDate").ToString();
                                DateTime tempdt = new DateTime(), sFromDt = new DateTime(), sToDate = new DateTime();

                                if (celldate != "")
                                {
                                    tempdt = Convert.ToDateTime(celldate);
                                    sFromDt = tempdt.AddDays(-1);
                                    sToDate = tempdt.AddDays(1);

                                }

                                string sql = "Insert into MastLeaveScheduleDelHistory ( " +
                                    "[SanID],[EmpUnqID],[tDate],[WrkGrp],[ConsInTime],[ConsOutTime],[ConsOverTime]," +
                                    "[ConsShift],[SchShift],[SchLeave],[SchLeaveAdv],[SchLeaveHalf],[SchInstedOf]," +
                                    "[tyear],[tyearmt],[AddDt],[AddID],[UpdDt],[UpdID],[DelFlg],[Remarks])  " +
                                    "Select " +
                                    "[SanID],[EmpUnqID],[tDate],[WrkGrp],[ConsInTime],[ConsOutTime],[ConsOverTime]," +
                                    "[ConsShift],[SchShift],[SchLeave],[SchLeaveAdv],[SchLeaveHalf],[SchInstedOf]," +
                                    "[tyear],[tyearmt],[AddDt],[AddID],GetDate(),'" + Utils.User.GUserID + "',[DelFlg], 'Changed By " + Utils.User.GUserID + "'" +
                                    " from MastLeaveSchedule where SanID ='" + sanid.ToString() + "' ";

                                SqlCommand cmd = new SqlCommand(sql, cn);
                                cmd.ExecuteNonQuery();

                                sql = "Delete From MastLeaveSchedule where SanID = '" + sanid.ToString() + "' and SchLeave is null and SchShift is null ";
                                cmd = new SqlCommand(sql, cn);
                                cmd.ExecuteNonQuery();

                                sql = "Update MastLeaveSchedule set ConsInTime = null , ConsOutTime = null , ConsShift = null, ConsOverTime = null where SanID = '" + sanid.ToString() + "' ";
                                cmd = new SqlCommand(sql, cn);
                                cmd.ExecuteNonQuery();


                                if (sFromDt != DateTime.MinValue && sToDate != DateTime.MinValue)
                                {
                                    clsProcess pro = new clsProcess();
                                    int res = 0; string errpro = string.Empty;
                                    pro.AttdProcess(Emp.EmpUnqID, sFromDt, sToDate, out res, out errpro);

                                    if (!string.IsNullOrEmpty(errpro))
                                    {
                                        MessageBox.Show("Data Process Error : " + errpro, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                }
                                LoadGrid();
                                

                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }


                        }//using connection
                    }//if sanid > 0
                }//if celltext sanid
            }//foreach selected rows
            
        }

        private void btnLunch_IO_Del_Click(object sender, EventArgs e)
        {

            if (gv_InOutLunch.SelectedRowsCount == 0)
            {
                MessageBox.Show("Please Select a Row First..", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            DialogResult dr = MessageBox.Show("Are you sure to Delete selected row ?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.No)
            {
                return;
            }


            foreach (int i in gv_InOutLunch.GetSelectedRows())
            {
                string cellpunch = gv_InOutLunch.GetRowCellValue(i, "PunchDate").ToString().Trim();
                string cellio = gv_InOutLunch.GetRowCellValue(i, "IOFLG").ToString().Trim();
                string cellip = gv_InOutLunch.GetRowCellValue(i, "MachineIP").ToString().Trim();

                using (SqlConnection cn = new SqlConnection(Utils.Helper.constr))
                {
                    try
                    {
                        cn.Open();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    SqlTransaction tr = cn.BeginTransaction();
                    try
                    {
                        DateTime tDate = Convert.ToDateTime(cellpunch);

                        string sql = "Insert into AttdLunchIODelete (EmpUnqID,PunchDate,IOFLG,MachineIP,AddDt,AddId,tYear,tYearMt) Values (" +
                        " '" + Emp.EmpUnqID + "','" + tDate.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                        " '" + cellio + "','" + cellip + "',GetDate(),'" + Utils.User.GUserID + "','" + tDate.Year + "','" + tDate.ToString("yyyyMM") + "' )";
                        SqlCommand cmd = new SqlCommand(sql, cn);
                        cmd.Transaction = tr;
                        cmd.ExecuteNonQuery();

                        sql = "Delete From AttdLunchGate Where EmpUnqID ='" + Emp.EmpUnqID + "' and PunchDate ='" + tDate.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                            " And MachineIP ='" + cellip + "' And IOFLG = '" + cellio + "' and tYear ='" + tDate.Year.ToString() + "'";

                        cmd = new SqlCommand(sql, cn);
                        cmd.Transaction = tr;
                        cmd.ExecuteNonQuery();
                        tr.Commit();

                       
                        clsProcess pro = new clsProcess();
                        int res = 0;

                        //process lunch_inout
                        pro.LunchInOutProcess(Emp.EmpUnqID, tDate, tDate, out res);


                        //process lunch_halfday_post...
                        res = 0;
                        pro.Lunch_HalfDayPost_Process(Emp.EmpUnqID, tDate, out res);

                        txtSanDtLunch.EditValue = null;
                        txtInOutLunch.Text = "";
                        txtTimeLunch.Text = "";
                        txtLocLunch.Text = "";
                        LoadGrid();

                        MessageBox.Show("Deleted...", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;


                    }
                    catch (Exception ex)
                    {
                        tr.Rollback();
                        MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }

        }

        private void btnGate_IO_Del_Click(object sender, EventArgs e)
        {
            if (gv_InOutGate.SelectedRowsCount == 0)
            {
                MessageBox.Show("Please Select a Row First..", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            DialogResult dr = MessageBox.Show("Are you sure to Delete selected row ?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.No)
            {
                return;
            }


            foreach (int i in gv_InOutGate.GetSelectedRows())
            {
                string cellpunch = gv_InOutGate.GetRowCellValue(i, "PunchDate").ToString().Trim();
                string cellio = gv_InOutGate.GetRowCellValue(i, "IOFLG").ToString().Trim();
                string cellip = gv_InOutGate.GetRowCellValue(i, "MachineIP").ToString().Trim();
                string celluser = gv_InOutGate.GetRowCellValue(i, "AddID").ToString().Trim();
                
                using (SqlConnection cn = new SqlConnection(Utils.Helper.constr))
                {
                    try
                    {
                        cn.Open();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    SqlTransaction tr = cn.BeginTransaction();
                    try
                    {
                        DateTime tDate = Convert.ToDateTime(cellpunch);

                        string sql = "Insert into AttdGateInOut_Delete (EmpUnqID,PunchDate,IOFLG,MachineIP,AddDt,AddId,tYear,tYearMt) Values (" +
                        " '" + Emp.EmpUnqID + "','" + tDate.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                        " '" + cellio + "','" + cellip + "',GetDate(),'" + Utils.User.GUserID + "','" + tDate.Year + "','" + tDate.ToString("yyyyMM") + "' )";
                        SqlCommand cmd = new SqlCommand(sql, cn);
                        cmd.Transaction = tr;
                        cmd.ExecuteNonQuery();

                        
                        sql = "Delete From AttdGateInOut Where EmpUnqID ='" + Emp.EmpUnqID + "' and PunchDate ='" + tDate.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                            " And MachineIP ='" + cellip + "' And IOFLG = '" + cellio + "' and tYear ='" + tDate.Year.ToString() + "'";

                        cmd = new SqlCommand(sql, cn);
                        cmd.Transaction = tr;
                        cmd.ExecuteNonQuery();


                        tr.Commit();

                        //process attendance
                        DateTime tempdt = new DateTime(), sFromDt = new DateTime(), sToDate = new DateTime();
                        tempdt = Convert.ToDateTime(tDate.ToString("yyyy-MM-dd"));
                        sFromDt = tempdt.AddDays(-1);
                        sToDate = tempdt.AddDays(1);

                        if (sFromDt != DateTime.MinValue && sToDate != DateTime.MinValue)
                        {
                            clsProcess pro = new clsProcess();
                            int res = 0; string errpro = string.Empty;
                            pro.AttdProcess(Emp.EmpUnqID, sFromDt, sToDate, out res, out errpro);

                            if (!string.IsNullOrEmpty(errpro))
                            {
                                MessageBox.Show("Data Process Error : " + errpro, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        
                        txtSanDtGate.EditValue = null;
                        txtInOutGate.Text = "";
                        txtTimeGate.Text = "";
                        txtLocGate.Text = "";
                        LoadGrid();

                        MessageBox.Show("Deleted...", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;


                    }
                    catch (Exception ex)
                    {
                        tr.Rollback();
                        MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }

        }

        private void btnLunch_Ignore_Click(object sender, EventArgs e)
        {
            if (gv_LunchDt.SelectedRowsCount == 0)
            {
                MessageBox.Show("Please Select a Row First..", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult dr = MessageBox.Show("Are you sure to ignore halfday selected row ?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.No)
            {
                return;
            }
            
            foreach (int i in gv_LunchDt.GetSelectedRows())
            {
                string cellpunch = gv_LunchDt.GetRowCellValue(i, "tDate").ToString().Trim();
                DateTime tDate = Convert.ToDateTime(cellpunch);

                using (SqlConnection cn = new SqlConnection(Utils.Helper.constr))
                {
                    try
                    {
                    
                        cn.Open();
                        string sql = "Update AttdLunchHistory Set ignore = 1 , ignoreby = '" + Utils.User.GUserID + "',ignoredt = getdate() " +
                                " Where tDate = '" + tDate.ToString("yyyy-MM-dd") + "' and EmpUnqID = '" + Emp.EmpUnqID + "' and Posted = 0 ";

                        using (SqlCommand cmd = new SqlCommand(sql, cn))
                        {
                            cmd.ExecuteNonQuery();
                            LoadGrid();
                        }

                        MessageBox.Show("Record ignored..", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return; 

                    }catch(Exception ex)
                    {
                        MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;   
                    }
                }
            }
        }

        private void txtSanDtLunch_EditValueChanged(object sender, EventArgs e)
        {
            txtTimeLunch.Time = txtSanDtLunch.DateTime;
        }

        private void txtSanDtGate_EditValueChanged(object sender, EventArgs e)
        {
            txtTimeGate.Time = txtSanDtGate.DateTime;
        }

        private void txtSanDtGatePass_EditValueChanged(object sender, EventArgs e)
        {
            txtTimeGatePass.Time = txtSanDtGatePass.DateTime;
        }

        //private void btnLunch_IO_San_Click(object sender, EventArgs e)
        //{
        //    string err = DataValidate_Lunch();

        //    if (!string.IsNullOrEmpty(err))
        //    {
        //        MessageBox.Show(err, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        return;
        //    }


        //    DateTime curDate, reqDate;
        //    curDate = Globals.GetSystemDateTime();
        //    reqDate = txtSanDtLunch.DateTime;

        //    //'Added on 21/11/2014 as required do not let enter intime outtime prerier and "past entry must be within 2days"
        //    //if (Utils.User.IsAdmin == false)
        //    //{
        //    //    TimeSpan t = reqDate - curDate;

        //    //    if (t.Days > rSanDayLimit)
        //    //    {
        //    //        MessageBox.Show("System Does not allow to post InTime/outTime unless fall within" +
        //    //            rSanDayLimit.ToString() + " days...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

        //    //        return;
        //    //    }                
        //    //}

        //    string tMachineIP = string.Empty;
        //    string sql = string.Empty;
        //    string ioflg = txtInOutLunch.Text.ToString().Substring(0,1);
        //    DateTime tSanDt = Convert.ToDateTime(txtSanDtLunch.DateTime.ToString("yyyy-MM-dd") + " " + txtTimeLunch.Time.ToString("HH:mm"));

        //    sql = "Select MachineIP From Readerconfig Where LunchInOut = 1 and IOFLG = '" + ioflg  +"' and Location ='" + txtLocLunch.Text.Trim()  + "' " +
        //        " Union Select MachineIP From TripodReaderconfig Where IOFLG = '" + ioflg  +"' and Location ='" + txtLocLunch.Text.Trim()  + "'" ;

        //    tMachineIP = Utils.Helper.GetDescription(sql,Utils.Helper.constr);

        //    if(tMachineIP == "")
        //    {
        //        MessageBox.Show("Invalid Location...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        return;
        //    }

        //    using (SqlConnection cn = new SqlConnection(Utils.Helper.constr))
        //    {
        //        try
        //        {
        //            cn.Open();
        //            sql = "Insert into AttdLunchGate (PunchDate,EmpUnqID,IOFLG,MachineIP,LunchFlg,tYear,tYearMt,t1Date,AddDt,AddId) " +
        //                " Values ('" + tSanDt.ToString("yyyy-MM-dd HH:mm:ss") + "'" +
        //                " ,'" + Emp.EmpUnqID + "','" + ioflg + "','" + tMachineIP + "','0' " +
        //                " ,'" + tSanDt.ToString("yyyy") + "','" + tSanDt.ToString("yyyyMM") + "'" +
        //                " ,'" + tSanDt.ToString("yyyy-MM-dd") + "',GetDate(),'" + Utils.User.GUserID + "-San')";

        //            SqlCommand cmd = new SqlCommand(sql, cn);
        //            cmd.ExecuteNonQuery();

        //            clsProcess pro = new clsProcess();

        //            //process lunch_inout
        //            int res;
        //            pro.LunchInOutProcess(Emp.EmpUnqID, tSanDt, tSanDt, out res);

        //            //process lunch_halfday_post...
        //            res = 0;
        //            pro.Lunch_HalfDayPost_Process(Emp.EmpUnqID, tSanDt, out res);

        //            //txtSanDtLunch.EditValue = null;
        //            //txtInOutLunch.Text =  "";
        //            //txtTimeLunch.Text = "";
        //            //txtLocLunch.Text = "";
        //            LoadGrid();

        //            MessageBox.Show("Sanctioned...", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //            ctrlEmp1.txtEmpUnqID.Focus();
        //            return;
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            return;
        //        }


        //    }


        //}

        //private void btnGate_IO_San_Click(object sender, EventArgs e)
        //{
        //    string err = DataValidate_Gate();

        //    if (!string.IsNullOrEmpty(err))
        //    {
        //        MessageBox.Show(err, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        return;
        //    }


        //    DateTime curDate, reqDate;
        //    curDate = Globals.GetSystemDateTime();
        //    reqDate = txtSanDtGate.DateTime;

        //    //'Added on 21/11/2014 as required do not let enter intime outtime prerier and "past entry must be within 2days"
        //    //if (Utils.User.IsAdmin == false)
        //    //{
        //    //    TimeSpan t = reqDate - curDate;

        //    //    if (t.Days > rSanDayLimit)
        //    //    {
        //    //        MessageBox.Show("System Does not allow to post InTime/outTime unless fall within" +
        //    //            rSanDayLimit.ToString() + " days...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

        //    //        return;
        //    //    }                
        //    //}

        //    string tMachineIP = string.Empty;
        //    string sql = string.Empty;
        //    string ioflg = txtInOutGate.Text.ToString().Substring(0, 1);
        //    DateTime tSanDt = Convert.ToDateTime(txtSanDtGate.DateTime.ToString("yyyy-MM-dd") + " " + txtTimeGate.Time.ToString("HH:mm"));

        //    sql = "Select MachineIP From Readerconfig Where GateInOut = 1 and IOFLG = '" + ioflg + "' and Location ='" + txtLocGate.Text.Trim() + "'" +
        //        " Union Select MachineIP From TripodReaderconfig Where IOFLG = '" + ioflg + "' and Location ='" + txtLocGate.Text.Trim() + "'";

        //    tMachineIP = Utils.Helper.GetDescription(sql, Utils.Helper.constr);

        //    if (tMachineIP == "")
        //    {
        //        MessageBox.Show("Invalid Location...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        return;
        //    }

        //    using (SqlConnection cn = new SqlConnection(Utils.Helper.constr))
        //    {
        //        try
        //        {
        //            cn.Open();
        //            sql = "Insert into AttdGateInOut (PunchDate,EmpUnqID,IOFLG,MachineIP,LunchFlg,tYear,tYearMt,t1Date,AddDt,AddId) " +
        //                " Values ('" + tSanDt.ToString("yyyy-MM-dd HH:mm:ss") + "'" +
        //                " ,'" + Emp.EmpUnqID + "','" + ioflg + "','" + tMachineIP + "','0' " +
        //                " ,'" + tSanDt.ToString("yyyy") + "','" + tSanDt.ToString("yyyyMM") + "'" +
        //                " ,'" + tSanDt.ToString("yyyy-MM-dd") + "',GetDate(),'" + Utils.User.GUserID + "-San')";

        //            SqlCommand cmd = new SqlCommand(sql, cn);
        //            cmd.ExecuteNonQuery();

        //            //process Attendance...
        //            DateTime tempdt = new DateTime(), sFromDt = new DateTime(), sToDate = new DateTime();
        //                tempdt = Convert.ToDateTime(tSanDt.ToString("yyyy-MM-dd"));
        //                sFromDt = tempdt.AddDays(-1);
        //                sToDate = tempdt.AddDays(1);
                    
        //            if (sFromDt != DateTime.MinValue && sToDate != DateTime.MinValue)
        //            {
        //                clsProcess pro = new clsProcess();
        //                int res = 0; string errpro = string.Empty;
        //                pro.AttdProcess(Emp.EmpUnqID, sFromDt, sToDate, out res, out errpro);
        //                pro.AttdProcess(Emp.EmpUnqID, sFromDt, sToDate, out res, out errpro);
                        
        //                if (!string.IsNullOrEmpty(errpro))
        //                {
        //                    MessageBox.Show("Data Process Error : " + errpro, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //                }
        //            }

        //            //txtSanDtGate.EditValue = null;
        //            //txtInOutGate.Text = "";
        //            //txtTimeGate.Text = "";
        //            //txtLocGate.Text = "";
        //            LoadGrid();

        //            MessageBox.Show("Sanctioned...", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //            ctrlEmp1.txtEmpUnqID.Focus();
        //            return;
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            return;
        //        }


        //    }
        //}

        private void frmSanction_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyData == Keys.Enter))
            {
                SelectNextControl(ActiveControl, true, true, true, true);
            }
        }

        private void txtSanDt_Validated(object sender, EventArgs e)
        {
            txtInTime.Time = txtSanDt.DateTime;
            txtOutTime.Time = txtSanDt.DateTime;

            if (txtSanDt.OldEditValue != txtSanDt.EditValue)
            {
                LoadGrid();
            }
        }

        private void btnConvertToGatePass_Click(object sender, EventArgs e)
        {
            if (gv_InOutLunch.SelectedRowsCount == 0)
            {
                MessageBox.Show("Please Select a Row First..", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult dr = MessageBox.Show("Are you sure to Convert To GatePass Punch selected row ?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.No)
            {
                return;
            }


            foreach (int i in gv_InOutLunch.GetSelectedRows())
            {
                string cellpunch = gv_InOutLunch.GetRowCellValue(i, "PunchDate").ToString().Trim();
                string cellio = gv_InOutLunch.GetRowCellValue(i, "IOFLG").ToString().Trim();
                string cellip = gv_InOutLunch.GetRowCellValue(i, "MachineIP").ToString().Trim();
                string celladddt = gv_InOutLunch.GetRowCellValue(i, "AddDt").ToString().Trim();
                using (SqlConnection cn = new SqlConnection(Utils.Helper.constr))
                {
                    try
                    {
                        cn.Open();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    SqlTransaction tr = cn.BeginTransaction();
                    try
                    {
                        //DateTime tDate = Convert.ToDateTime(cellpunch);
                        DateTime tDate = DateTime.ParseExact(cellpunch, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                        DateTime tAddDt = DateTime.ParseExact(celladddt, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                        
                        string sql = "Insert into AttdLunchIODelete (EmpUnqID,PunchDate,IOFLG,MachineIP,AddDt,AddId,tYear,tYearMt) Values (" +
                        " '" + Emp.EmpUnqID + "','" + tDate.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                        " '" + cellio + "','" + cellip + "','" + tAddDt.ToString("yyyy-MM-dd HH:mm:ss")  + "','" + Utils.User.GUserID + "','" + tDate.Year + "','" + tDate.ToString("yyyyMM") + "' )";
                        SqlCommand cmd = new SqlCommand(sql, cn);
                        cmd.Transaction = tr;
                        cmd.ExecuteNonQuery();

                        sql = "Delete From AttdLunchGate Where EmpUnqID ='" + Emp.EmpUnqID + "' and PunchDate ='" + tDate.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                            " And MachineIP ='" + cellip + "' And IOFLG = '" + cellio + "' and tYear ='" + tDate.Year.ToString() + "'";

                        cmd = new SqlCommand(sql, cn);
                        cmd.Transaction = tr;
                        cmd.ExecuteNonQuery();
                       

                        sql = "Insert into AttdGatePassInOut (EmpUnqID,PunchDate,IOFLG,MachineIP,AddDt,AddId,tYear,tYearMt,t1date) Values (" +
                        " '" + Emp.EmpUnqID + "','" + tDate.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                        " '" + cellio + "','" + cellip + "','" + tAddDt.ToString("yyyy-MM-dd HH:mm:ss") + "','" + "TRIPOD-Conv-" + Utils.User.GUserID + "','" + tDate.Year + "','" + tDate.ToString("yyyyMM") + "','" + tDate.ToString("yyyy-MM-dd") + "' )";
                        cmd = new SqlCommand(sql, cn);
                        cmd.Transaction = tr;
                        cmd.ExecuteNonQuery();

                        sql = "Delete From TripodLog Where EmpUnqID ='" + Emp.EmpUnqID + "' and PunchDate ='" + tDate.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                            " And MachineIP ='" + cellip + "' And IOFLG = '" + cellio + "' and tYear ='" + tDate.Year.ToString() + "'";

                        cmd = new SqlCommand(sql, cn);
                        cmd.Transaction = tr;
                        cmd.ExecuteNonQuery();
                        
                        tr.Commit();

                        //process attendance
                        DateTime tempdt = new DateTime(), sFromDt = new DateTime(), sToDate = new DateTime();
                        tempdt = Convert.ToDateTime(tDate.ToString("yyyy-MM-dd"));
                        sFromDt = tempdt.AddDays(-1);
                        sToDate = tempdt.AddDays(1);

                        if (sFromDt != DateTime.MinValue && sToDate != DateTime.MinValue)
                        {
                            clsProcess pro = new clsProcess();
                            int res = 0; string errpro = string.Empty;
                            pro.LunchInOutProcess(Emp.EmpUnqID, sFromDt, sToDate, out res);
                            pro.Lunch_HalfDayPost_Process(Emp.EmpUnqID, tempdt, out res);
                            pro.AttdProcess(Emp.EmpUnqID, sFromDt, sToDate, out res, out errpro);
                            
                            if (!string.IsNullOrEmpty(errpro))
                            {
                                MessageBox.Show("Data Process Error : " + errpro, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }

                        //txtSanDtGatePass.EditValue = null;
                        //txtInOutGatePass.Text = "";
                        //txtTimeGatePass.Text = "";
                        //txtLocGatePass.Text = "";

                        LoadGrid();

                        MessageBox.Show("Converted...", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;


                    }
                    catch (Exception ex)
                    {
                        tr.Rollback();
                        MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }
        }

        //private void btnGatePass_IO_Del_Click(object sender, EventArgs e)
        //{
        //    if (gv_InOutGatePass.SelectedRowsCount == 0)
        //    {
        //        MessageBox.Show("Please Select a Row First..", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        return;
        //    }

        //    DialogResult dr = MessageBox.Show("Are you sure to Delete selected row ?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        //    if (dr == DialogResult.No)
        //    {
        //        return;
        //    }


        //    foreach (int i in gv_InOutGatePass.GetSelectedRows())
        //    {
        //        string cellpunch = gv_InOutGatePass.GetRowCellValue(i, "PunchDate").ToString().Trim();
        //        string cellio = gv_InOutGatePass.GetRowCellValue(i, "IOFLG").ToString().Trim();
        //        string cellip = gv_InOutGatePass.GetRowCellValue(i, "MachineIP").ToString().Trim();
        //        string celluser = gv_InOutGatePass.GetRowCellValue(i, "AddID").ToString().Trim();

        //        using (SqlConnection cn = new SqlConnection(Utils.Helper.constr))
        //        {
        //            try
        //            {
        //                cn.Open();
        //            }
        //            catch (Exception ex)
        //            {
        //                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //                return;
        //            }

        //            SqlTransaction tr = cn.BeginTransaction();
        //            try
        //            {
        //                DateTime tDate = Convert.ToDateTime(cellpunch);

        //                string sql = "Insert into AttdGatePassInOut_Delete (EmpUnqID,PunchDate,IOFLG,MachineIP,AddDt,AddId,tYear,tYearMt) Values (" +
        //                " '" + Emp.EmpUnqID + "','" + tDate.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
        //                " '" + cellio + "','" + cellip + "',GetDate(),'" + Utils.User.GUserID + "','" + tDate.Year + "','" + tDate.ToString("yyyyMM") + "' )";
        //                SqlCommand cmd = new SqlCommand(sql, cn);
        //                cmd.Transaction = tr;
        //                cmd.ExecuteNonQuery();

        //                sql = "Delete From AttdGatePassInOut Where EmpUnqID ='" + Emp.EmpUnqID + "' and PunchDate ='" + tDate.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
        //                    " And MachineIP ='" + cellip + "' And IOFLG = '" + cellio + "' and tYear ='" + tDate.Year.ToString() + "'";

        //                cmd = new SqlCommand(sql, cn);
        //                cmd.Transaction = tr;
        //                cmd.ExecuteNonQuery();
                       


        //                if (celluser.Contains("TRIPOD-Conv"))
        //                {
        //                    string sql1 = "Insert into TripodLog (EmpUnqID,PunchDate,IOFLG,MachineIP,AddDt,AddId,tYear,t1date,tYearMt,LunchFlg,DutyFlg,GatePassFlg,Processed) Values (" +
        //                    " '" + Emp.EmpUnqID + "','" + tDate.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
        //                    " '" + cellio + "','" + cellip + "',GetDate(),'SERVER','" + tDate.Year + "','" + tDate.ToString("yyyy-MM-dd") + "','" + tDate.ToString("yyyyMM") + "',0,0,0,0 )";
                            
        //                    cmd = new SqlCommand(sql1, cn);
        //                    cmd.Transaction = tr;
        //                    cmd.ExecuteNonQuery();
        //                }

        //                tr.Commit();

        //                //process attendance
        //                //DateTime tempdt = new DateTime(), sFromDt = new DateTime(), sToDate = new DateTime();
        //                //tempdt = Convert.ToDateTime(tDate.ToString("yyyy-MM-dd"));
        //                //sFromDt = tempdt.AddDays(-1);
        //                //sToDate = tempdt.AddDays(1);

        //                //if (sFromDt != DateTime.MinValue && sToDate != DateTime.MinValue)
        //                //{
        //                //    clsProcess pro = new clsProcess();
        //                //    int res = 0; string errpro = string.Empty;
        //                //    pro.AttdProcess(Emp.EmpUnqID, sFromDt, sToDate, out res, out errpro);

        //                //    if (!string.IsNullOrEmpty(errpro))
        //                //    {
        //                //        MessageBox.Show("Data Process Error : " + errpro, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //                //    }
        //                //}

        //                txtSanDtGatePass.EditValue = null;
        //                txtInOutGatePass.Text = "";
        //                txtTimeGatePass.Text = "";
        //                txtLocGatePass.Text = "";
        //                LoadGrid();

        //                MessageBox.Show("Deleted...", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //                return;


        //            }
        //            catch (Exception ex)
        //            {
        //                tr.Rollback();
        //                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //                return;
        //            }
        //        }
        //    }
        //}

        //private void btnGatePass_IO_San_Click(object sender, EventArgs e)
        //{
        //    string err = DataValidate_GatePass();

        //    if (!string.IsNullOrEmpty(err))
        //    {
        //        MessageBox.Show(err, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        return;
        //    }


        //    DateTime curDate, reqDate;
        //    curDate = Globals.GetSystemDateTime();
        //    reqDate = txtSanDtGate.DateTime;

        //    string tMachineIP = string.Empty;
        //    string sql = string.Empty;
        //    string ioflg = txtInOutGatePass.Text.ToString().Substring(0, 1);
        //    DateTime tSanDt = Convert.ToDateTime(txtSanDtGatePass.DateTime.ToString("yyyy-MM-dd") + " " + txtTimeGatePass.Time.ToString("HH:mm"));

        //    sql = "Select MachineIP From Readerconfig Where GateInOut = 1 and IOFLG = '" + ioflg + "' and Location ='" + txtLocGate.Text.Trim() + "'" +
        //        " Union Select MachineIP From TripodReaderconfig Where IOFLG = '" + ioflg + "' and Location ='" + txtLocGate.Text.Trim() + "'";

        //    tMachineIP = Utils.Helper.GetDescription(sql, Utils.Helper.constr);

        //    if (tMachineIP == "")
        //    {
        //        MessageBox.Show("Invalid Location...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        return;
        //    }

        //    using (SqlConnection cn = new SqlConnection(Utils.Helper.constr))
        //    {
        //        try
        //        {
        //            cn.Open();
        //            sql = "Insert into AttdGatePassInOut (PunchDate,EmpUnqID,IOFLG,MachineIP,LunchFlg,tYear,tYearMt,t1Date,AddDt,AddId) " +
        //                " Values ('" + tSanDt.ToString("yyyy-MM-dd HH:mm:ss") + "'" +
        //                " ,'" + Emp.EmpUnqID + "','" + ioflg + "','" + tMachineIP + "','0' " +
        //                " ,'" + tSanDt.ToString("yyyy") + "','" + tSanDt.ToString("yyyyMM") + "'" +
        //                " ,'" + tSanDt.ToString("yyyy-MM-dd") + "',GetDate(),'" + Utils.User.GUserID + "-San')";

        //            SqlCommand cmd = new SqlCommand(sql, cn);
        //            cmd.ExecuteNonQuery();

        //            ////process Attendance...
        //            //DateTime tempdt = new DateTime(), sFromDt = new DateTime(), sToDate = new DateTime();
        //            //tempdt = Convert.ToDateTime(tSanDt.ToString("yyyy-MM-dd"));
        //            //sFromDt = tempdt.AddDays(-1);
        //            //sToDate = tempdt.AddDays(1);

        //            //if (sFromDt != DateTime.MinValue && sToDate != DateTime.MinValue)
        //            //{
        //            //    clsProcess pro = new clsProcess();
        //            //    int res = 0; string errpro = string.Empty;
        //            //    pro.AttdProcess(Emp.EmpUnqID, sFromDt, sToDate, out res, out errpro);
        //            //    pro.AttdProcess(Emp.EmpUnqID, sFromDt, sToDate, out res, out errpro);

        //            //    if (!string.IsNullOrEmpty(errpro))
        //            //    {
        //            //        MessageBox.Show("Data Process Error : " + errpro, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            //    }
        //            //}

        //            txtSanDtGatePass.EditValue = null;
        //            txtInOutGatePass.Text = "";
        //            txtTimeGatePass.Text = "";
        //            txtLocGatePass.Text = "";
        //            LoadGrid();

        //            MessageBox.Show("Sanctioned...", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //            ctrlEmp1.txtEmpUnqID.Focus();
        //            return;
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            return;
        //        }


        //    }
        //}

        private void btnDeleteShiftSch_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Are you sure to delete Shift Schedule for the month of " + txtSanDt.DateTime.Date.ToString("yyyyMM") + " ?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.No)
            {
                return;
            }
            

            //call the strore procedure Delete_ShiftSchedule
            using (SqlConnection cn = new SqlConnection(Utils.Helper.constr))
            {
                try
                {
                    cn.Open();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Could not connect to server/database", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                
                SqlCommand cmd = new SqlCommand();
                
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Delete_ShiftSchedule";

                SqlParameter spout = new SqlParameter();
                spout.Direction = ParameterDirection.Output;
                spout.DbType = DbType.Int32;
                spout.ParameterName = "@result";
                int tout = 0;
                spout.Value = tout;
                cmd.Parameters.AddWithValue("@pEmpUnqID", Emp.EmpUnqID);
                cmd.Parameters.AddWithValue("@pYearMt", Convert.ToInt32(txtSanDt.DateTime.Date.ToString("yyyyMM")) );
                cmd.Parameters.Add(spout);
                cmd.CommandTimeout = 0;
                cmd.ExecuteNonQuery();
                //get the output
                int result = (int)cmd.Parameters["@result"].Value;

                if (result == 0)
                {
                    MessageBox.Show("System does not allow to delete shift schedule,please contact administrator..", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("Shift Schedule Deleted..", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }

            }

        }

        

        

    }
}
