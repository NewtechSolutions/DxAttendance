using System;
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

namespace Attendance.Forms
{
    public partial class frmMastEmpJobProfile : Form
    {
        public string mode = "NEW";
        public string GRights = "XXXV";
        public string oldCode = "";
        public static clsEmp Emp = new clsEmp();


        public frmMastEmpJobProfile()
        {
            InitializeComponent();
            this.ctrlEmp1.EmpUnqIDValidated += new EventHandler(this.ctrlEmpValidateEvent_Handler);
            //this.ctrlEmp1.CompCodeValidated += new EventHandler(this.ctrlCompValidateEvent_Handler);
        }

        private void loadGrid(string tEmpUnqID)
        {
            DataSet ds = Utils.Helper.GetData("Select * from MastEmpJobProfile Where EmpUnqID='"+tEmpUnqID + "'", Utils.Helper.constr);
            grid.DataSource = ds;
            grid.DataMember = ds.Tables[0].TableName;
        }

        private void ctrlEmpValidateEvent_Handler(object sender, EventArgs e)
        {
            Emp = new clsEmp();
            Emp.EmpUnqID = ctrlEmp1.txtEmpUnqID.Text.Trim();
            if (Emp.GetEmpDetails(Emp.EmpUnqID))
            {
                txtValidFrom.EditValue = Emp.ProfileValidFrom;
                txtValidFrom_Validated(sender, e);
                loadGrid(Emp.EmpUnqID);
            }
            else
            {
                MessageBox.Show("Invalid Employee...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ResetCtrl();
            }
            
            
        }

        //private void ctrlCompValidateEvent_Handler(object sender, EventArgs e)
        //{
        //    if (string.IsNullOrEmpty(ctrlEmp1.txtCompCode.Text.Trim()))
        //        return;
            

        //}

        private void frmMastEmpJobProfile_Load(object sender, EventArgs e)
        {
            ResetCtrl();
            GRights = Attendance.Classes.Globals.GetFormRights(this.Name);
            SetRights();
                       
        }

        private void txtCostCode_Validated(object sender, EventArgs e)
        {
            if (txtCostCode.Text.Trim() == "")
            {
                return;
            }

            DataSet ds = new DataSet();
            string sql = "select * from MastCostCode where CostCode ='" + txtCostCode.Text.Trim() + "'";

            ds = Utils.Helper.GetData(sql, Utils.Helper.constr);
            bool hasRows = ds.Tables.Cast<DataTable>()
                           .Any(table => table.Rows.Count != 0);

            if (hasRows)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    txtCostCode.Text = dr["CostCode"].ToString();
                    txtCostDesc.Text = dr["CostDesc"].ToString();

                }
            }
            else
            {
                txtCostCode.Text = "";
                txtCostDesc.Text = "";
            }

        }

        private void txtCostCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1 || e.KeyCode == Keys.F2)
            {
                List<string> obj = new List<string>();

                Help_F1F2.ClsHelp hlp = new Help_F1F2.ClsHelp();
                string sql = "";


                sql = "Select CostCode,CostDesc From MastCostCode Where Active = 1";
                if (e.KeyCode == Keys.F1)
                {

                    obj = (List<string>)hlp.Show(sql, "CostCode", "CostCode", typeof(string), Utils.Helper.constr, "System.Data.SqlClient",
                   100, 300, 400, 600, 100, 100);
                }
                else
                {
                    obj = (List<string>)hlp.Show(sql, "CostDesc", "CostDesc", typeof(string), Utils.Helper.constr, "System.Data.SqlClient",
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

                    txtCostCode.Text = obj.ElementAt(0).ToString();
                    txtCostDesc.Text = obj.ElementAt(1).ToString();

                }
            }
        }
        
        private string DataValidate()
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

           
            if (string.IsNullOrEmpty(txtUnitDesc.Text.Trim().ToString()))
            {
                err = err + "Unit Code is required...." + Environment.NewLine;
            }
            

            if (string.IsNullOrEmpty(txtDeptDesc.Text.Trim().ToString()))
            {
                err = err + "Dept Code is required...." + Environment.NewLine;
            }

            if (string.IsNullOrEmpty(txtStatDesc.Text.Trim().ToString()))
            {
                err = err + "Station Code is required...." + Environment.NewLine;
            }

            if (string.IsNullOrEmpty(txtCatDesc.Text.Trim().ToString()))
            {
                err = err + "Catagory Code is required...." + Environment.NewLine;
            }

            if (string.IsNullOrEmpty(txtGradeDesc.Text.Trim().ToString()))
            {
                err = err + "Grade Code is required...." + Environment.NewLine;
            }

            if (string.IsNullOrEmpty(txtDesgDesc.Text.Trim().ToString()))
            {
                err = err + "Designation Code is required...." + Environment.NewLine;
            }

            if (string.IsNullOrEmpty(txtWeekOff.Text.Trim().ToString()))
            {
                err = err + "Week Off is required...." + Environment.NewLine;
            }

            if (string.IsNullOrEmpty(txtCostDesc.Text.Trim()))
            {
                err = err + "Please Enter CostCode..." + Environment.NewLine;
                return err;
            }

           

            string tsql = "Select Active from MastCostCode where CostCode = '" + txtCostCode.Text.Trim().ToString() + "'";
            string ifcostact = Utils.Helper.GetDescription(tsql, Utils.Helper.constr, out err);
            if (string.IsNullOrEmpty(ifcostact))
            {
                ifcostact = "false";
            }
            if (!Convert.ToBoolean(ifcostact))
            {
                err = err + "CostCode not active..." + Environment.NewLine;
                return err;
            }

            


            return err;
        }

        private void ResetCtrl()
        {
            btnAdd.Enabled = false;
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;

            txtValidFrom.EditValue = null;

           
            txtUnitCode.Text = string.Empty;
            txtUnitDesc.Text = String.Empty;

            txtDeptCode.Text = string.Empty;
            txtDeptDesc.Text = string.Empty;
            txtStatCode.Text = string.Empty;
            txtStatDesc.Text = string.Empty;
            txtCatCode.Text = string.Empty;
            txtCatDesc.Text = string.Empty;

            txtDesgCode.Text = string.Empty;
            txtDesgDesc.Text = string.Empty;
            txtGradeCode.Text = string.Empty;
            txtGradeDesc.Text = string.Empty;
            txtContCode.Text = "";
            txtContDesc.Text = "";
            chkOTFlg.Checked = false;
            txtCostCode.Text = "";
            txtCostDesc.Text = "";

            oldCode = "";
            mode = "NEW";
            //ctrlEmp1.txtEmpUnqID.Focus();

            grid.DataSource = null;
        }
        
        private void SetRights()
        {
            btnAdd.Enabled = false;
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;
            
            if (ctrlEmp1.txtEmpUnqID.Text.Trim() != "" && txtValidFrom.EditValue != null)
            {
                //btnAdd.Enabled = false;
                if(mode == "NEW" && GRights.Contains("A"))
                {
                    btnAdd.Enabled = true;
                    
                }
                
                if(mode == "OLD" && GRights.Contains("U"))
                    btnUpdate.Enabled = true;
                if (mode == "OLD" && GRights.Contains("D"))
                    btnDelete.Enabled = true;
            }
            
            if (GRights.Contains("XXXV"))
            {
                btnAdd.Enabled = false;
                btnUpdate.Enabled = false;
                btnDelete.Enabled = false;
            }


        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string err = DataValidate();
            if (!string.IsNullOrEmpty(err))
            {
                MessageBox.Show(err, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (SqlConnection cn = new SqlConnection(Utils.Helper.constr))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        cn.Open();
                        cmd.Connection = cn;

                        string sql = "insert into MastEmpJobProfile " +
                            " (EmpUnqID,ValidFrom,UnitCode,DeptCode,StatCode,CatCode,DesgCode,GradeCode,WeekOff,OTFLG,CostCode,ContCode,AddDt,AddID ) Values ( "
                            + " '" + ctrlEmp1.txtEmpUnqID.Text.Trim() + "','" + txtValidFrom.DateTime.Date.ToString("yyyy-MM-dd") + "',"
                            + " '" + txtUnitCode.Text.Trim().ToString() + "','" + txtDeptCode.Text.Trim().ToString() + "',"
                            + " '" + txtStatCode.Text.Trim().ToString() + "','" + txtCatCode.Text.Trim().ToString() + "',"
                            + " '" + txtDesgCode.Text.Trim().ToString() + "','" + txtGradeCode.Text.Trim().ToString() + "',"
                            + " '" + txtWeekOff.Text.Trim().ToUpper() + "','" + (chkOTFlg.Checked ? 1 : 0).ToString() + "',"
                            + " '" + txtCostCode.Text.Trim().ToString() + "','" + txtContCode.Text.Trim().ToString() + "',"
                            + " GetDate(),'" + Utils.User.GUserID + "')";
                        

                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = sql;
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Record Saved...", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        //ResetCtrl();

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            GrpMain.Enabled = true;

            Cursor.Current = Cursors.Default;


        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string err = DataValidate();
            
            if (!string.IsNullOrEmpty(err))
            {
                MessageBox.Show(err, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            GrpMain.Enabled = false;

            Cursor.Current = Cursors.WaitCursor;

            using (SqlConnection cn = new SqlConnection(Utils.Helper.constr))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        cn.Open();
                        cmd.Connection = cn;

                        string sql = "Update MastEmpJobProfile Set " +
                           " UnitCode ='" + txtUnitCode.Text.Trim().ToString() + "'," +
                           " DeptCode='" + txtDeptCode.Text.Trim().ToString() + "'," +
                           " StatCode='" + txtStatCode.Text.Trim().ToString() + "'," +
                           " CatCode='" + txtCatCode.Text.Trim().ToString() + "'," +
                           " DesgCode='" + txtDesgCode.Text.Trim().ToString() + "'," +
                           " GradeCode='" + txtGradeCode.Text.Trim().ToString() + "'," +
                           " ContCode='" + txtContCode.Text.Trim().ToString() + "'," +
                           " CostCode='" + txtCostCode.Text.Trim().ToString() + "'," +
                           " WeekOff='" + txtWeekOff.Text.Trim().ToString() + "'," +
                           " OTFLG='" + (chkOTFlg.Checked?1:0).ToString() + "'," +
                           " UpdDt=GetDate(),UpdID='" + Utils.User.GUserID + "'" +
                           " Where EmpUnqID ='" + ctrlEmp1.txtEmpUnqID.Text.Trim() + "' and ValidFrom ='" + txtValidFrom.DateTime.Date.ToString("yyyy-MM-dd") + "'";

                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = sql;
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Record Updated...", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        //ResetCtrl();

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            GrpMain.Enabled = true;

            Cursor.Current = Cursors.Default;

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
           
            string err = DataValidate();

            if (!string.IsNullOrEmpty(err))
            {
                MessageBox.Show(err, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            GrpMain.Enabled = false;

            Cursor.Current = Cursors.WaitCursor;

            using (SqlConnection cn = new SqlConnection(Utils.Helper.constr))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        cn.Open();
                        cmd.Connection = cn;

                        string sql = "Update MastEmpJobProfile set OTFLG=0,Weekoff='SUN', " +
                            "  CatCode = '' , DeptCode = '' , StatCode = '' , DesgCode = '' , " +
                            " GradeCode = '' , ContCode = '' , CostCode = '' , UnitCode = '', " +
                            " UpdDt=GetDate(),UpdID ='" + Utils.User.GUserID + "' Where " +
                            " EmpUnqID = '" + ctrlEmp1.txtEmpUnqID.Text.Trim().ToString() + "' " +
                            " and ValidFrom ='" + txtValidFrom.DateTime.Date.ToString("yyyy-MM-dd") + "'";

                        cmd.CommandText = sql;
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Record Updated...", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ResetCtrl();

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            GrpMain.Enabled = true;

            Cursor.Current = Cursors.Default;
            
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ResetCtrl();
            GRights = Attendance.Classes.Globals.GetFormRights(this.Name);
            SetRights();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void DisplayData(clsEmp temp)
        {
            txtValidFrom.EditValue = temp.ProfileValidFrom;
           
            
            txtUnitCode.Text = temp.UnitCode;
            txtUnitDesc.Text = temp.UnitDesc;
            txtDeptCode.Text = temp.DeptCode;
            txtDeptDesc.Text = temp.DeptDesc;
            txtStatCode.Text = temp.StatCode;
            txtStatDesc.Text = temp.StatDesc;
           
            txtDesgCode.Text = temp.DesgCode;
            txtDesgDesc.Text = temp.DesgDesc;
            txtGradeCode.Text = temp.GradeCode;
            txtGradeDesc.Text = temp.GradeDesc;
            txtCatCode.Text = temp.CatCode;
            txtCatDesc.Text = temp.CatDesc;

            txtCostCode.Text = temp.CostCode;
            txtCostDesc.Text = temp.CostDesc;

            txtContCode.Text = temp.ContCode;
            txtContDesc.Text = temp.ContDesc;
            txtWeekOff.Text = temp.WeekOffDay;
            chkOTFlg.Checked = temp.OTFLG;

           

        }

        //private void txtShiftCode_KeyDown(object sender, KeyEventArgs e)
        //{
        //    if (ctrlEmp1.txtCompCode.Text.Trim() == "" )
        //        return;

        //    if (e.KeyCode == Keys.F1 || e.KeyCode == Keys.F2)
        //    {
        //        List<string> obj = new List<string>();

        //        Help_F1F2.ClsHelp hlp = new Help_F1F2.ClsHelp();
        //        string sql = "";


        //        sql = "Select ShiftCode,ShiftDesc from MastShift Where CompCode ='" + ctrlEmp1.txtCompCode.Text.Trim() + "' ";
        //        if (e.KeyCode == Keys.F1)
        //        {

        //            obj = (List<string>)hlp.Show(sql, "ShiftCode", "ShiftCode", typeof(string), Utils.Helper.constr, "System.Data.SqlClient",
        //           100, 300, 400, 600, 100, 100);
        //        }
        //        else
        //        {
        //            obj = (List<string>)hlp.Show(sql, "ShiftDesc", "ShiftDesc", typeof(string), Utils.Helper.constr, "System.Data.SqlClient",
        //           100, 300, 400, 600, 100, 100);
        //        }

        //        if (obj.Count == 0)
        //        {

        //            return;
        //        }
        //        else if (obj.ElementAt(0).ToString() == "0")
        //        {
        //            return;
        //        }
        //        else if (obj.ElementAt(0).ToString() == "")
        //        {
        //            return;
        //        }
        //        else
        //        {

        //            txtShiftCode.Text = obj.ElementAt(0).ToString();
        //            txtShiftDesc.Text = obj.ElementAt(1).ToString();

                    
        //        }
        //    }
        //}

        //private void txtShiftCode_Validated(object sender, EventArgs e)
        //{
        //    if (ctrlEmp1.txtCompCode.Text.Trim() == "" || ctrlEmp1.txtCompDesc.Text.Trim() == "" )
        //    {
               
        //        return;
        //    }

        //    DataSet ds = new DataSet();
        //    string sql = "select * From  MastShift where CompCode ='" + ctrlEmp1.txtCompCode.Text.Trim() + "' and ShiftCode ='" + txtShiftCode.Text.Trim() + "'";

        //    ds = Utils.Helper.GetData(sql, Utils.Helper.constr);
        //    bool hasRows = ds.Tables.Cast<DataTable>()
        //                   .Any(table => table.Rows.Count != 0);

        //    if (hasRows)
        //    {
        //        foreach (DataRow dr in ds.Tables[0].Rows)
        //        {
                   
        //            txtShiftCode.Text = dr["ShiftCode"].ToString();
        //            txtShiftDesc.Text = dr["ShiftDesc"].ToString();
                   
        //        }
        //    }
        //    else
        //    {
        //        txtShiftDesc.Text = "";
        //    }
            
        //}

        private void txtDeptCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (ctrlEmp1.txtCompCode.Text.Trim() == "" || ctrlEmp1.txtWrkGrpCode.Text.Trim() == "" || txtUnitCode.Text.Trim() == "")
                return;

            if (e.KeyCode == Keys.F1 || e.KeyCode == Keys.F2)
            {
                List<string> obj = new List<string>();

                Help_F1F2.ClsHelp hlp = new Help_F1F2.ClsHelp();
                string sql = "";

                sql = "Select DeptCode,DeptDesc From MastDept Where CompCode ='" + ctrlEmp1.txtCompCode.Text.Trim() + "' " +
                    " and WrkGrp = '" + ctrlEmp1.txtWrkGrpCode.Text.Trim() + "' and UnitCode ='" + txtUnitCode.Text.Trim() + "'";

                if (e.KeyCode == Keys.F1)
                {
                    
                    obj = (List<string>)hlp.Show(sql, "DeptCode", "DeptCode", typeof(string), Utils.Helper.constr, "System.Data.SqlClient",
                   100, 300, 400, 600, 100, 100);
                }
                else
                {
                   
                    obj = (List<string>)hlp.Show(sql, "DeptDesc", "DeptDesc", typeof(string), Utils.Helper.constr, "System.Data.SqlClient",
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

                    txtDeptCode.Text = obj.ElementAt(0).ToString();
                    txtDeptDesc.Text = obj.ElementAt(1).ToString();

                }
            }
        }

        private void txtDeptCode_Validated(object sender, EventArgs e)
        {
            if (ctrlEmp1.txtCompCode.Text.Trim() == "" || ctrlEmp1.txtWrkGrpCode.Text.Trim() == "" 
                || txtUnitCode.Text.Trim() == ""
                || txtDeptCode.Text.Trim() == ""
                )
                return;

            

            string tCompCode = ctrlEmp1.txtCompCode.Text.Trim();
            string tWrkGrpCode = ctrlEmp1.txtWrkGrpCode.Text.Trim();
            string tUnitCode = txtUnitCode.Text.Trim();
           // string tDeptCode = txtDeptCode.Text.Trim();
            string tCode = txtDeptCode.Text.Trim();
            string desc;
            Utils.MastCodeValidate.GetDeptDesc(tCompCode, tWrkGrpCode, tUnitCode,  tCode, out desc);
            txtDeptDesc.Text = desc;

            if (string.IsNullOrEmpty(desc))
            {
                txtDeptCode.Text = "";
                txtDeptDesc.Text = "";
            }


        }

        private void txtStatCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (ctrlEmp1.txtCompCode.Text.Trim() == "" || ctrlEmp1.txtWrkGrpCode.Text.Trim() == ""
                 || txtUnitCode.Text.Trim() == ""
                 || txtDeptCode.Text.Trim() == ""
                 )
                return;

            if (e.KeyCode == Keys.F1 || e.KeyCode == Keys.F2)
            {
                List<string> obj = new List<string>();

                Help_F1F2.ClsHelp hlp = new Help_F1F2.ClsHelp();
                string sql = "";

                sql = "Select StatCode,StatDesc From MastStat Where CompCode ='" + ctrlEmp1.txtCompCode.Text.Trim() + "' " +
                   " and WrkGrp = '" + ctrlEmp1.txtWrkGrpCode.Text.Trim() + "' and UnitCode ='" + txtUnitCode.Text.Trim() + "' " +
                   " and DeptCode='" + txtDeptCode.Text.Trim() + "'";

                if (e.KeyCode == Keys.F1)
                {

                    obj = (List<string>)hlp.Show(sql, "StatCode", "StatCode", typeof(string), Utils.Helper.constr, "System.Data.SqlClient",
                   100, 300, 400, 600, 100, 100);
                }
                else
                {
                    obj = (List<string>)hlp.Show(sql, "StatDesc", "StatDesc", typeof(string), Utils.Helper.constr, "System.Data.SqlClient",
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
                    txtStatCode.Text = obj.ElementAt(0).ToString();
                    txtStatDesc.Text = obj.ElementAt(1).ToString();
                }
            }
        }

        private void txtStatCode_Validated(object sender, EventArgs e)
        {
            if (ctrlEmp1.txtCompCode.Text.Trim() == "" || ctrlEmp1.txtWrkGrpCode.Text.Trim() == ""
               
                || txtUnitCode.Text.Trim() == ""
                || txtDeptCode.Text.Trim() == ""
                || txtStatCode.Text.Trim() == ""
                )
                return;

            string tCompCode = ctrlEmp1.txtCompCode.Text.Trim();
            string tWrkGrpCode = ctrlEmp1.txtWrkGrpCode.Text.Trim();
            string tUnitCode = txtUnitCode.Text.Trim();
            string tDeptCode = txtDeptCode.Text.Trim();
            string tCode = txtStatCode.Text.Trim();
            string desc;
            Utils.MastCodeValidate.GetStatDesc(tCompCode, tWrkGrpCode,tUnitCode ,tDeptCode, tCode, out desc);
            txtStatDesc.Text = desc;

            if (string.IsNullOrEmpty(desc))
            {
                txtStatCode.Text = "";
                txtStatDesc.Text = "";
            }



        }

        private void txtCatCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (ctrlEmp1.txtCompCode.Text.Trim() == "" 
                || ctrlEmp1.txtWrkGrpCode.Text.Trim() == ""
              
                )
                return;

            if (e.KeyCode == Keys.F1 || e.KeyCode == Keys.F2)
            {
                List<string> obj = new List<string>();

                Help_F1F2.ClsHelp hlp = new Help_F1F2.ClsHelp();
                string sql = "";


                sql = "Select CatCode,CatDesc From MastCat Where CompCode ='" + ctrlEmp1.txtCompCode.Text.Trim() + "' and WrkGrp = '" + ctrlEmp1.txtWrkGrpCode.Text.Trim() + "' ";
                if (e.KeyCode == Keys.F1)
                {

                    obj = (List<string>)hlp.Show(sql, "CatCode", "CatCode", typeof(string), Utils.Helper.constr, "System.Data.SqlClient",
                   100, 300, 400, 600, 100, 100);
                }
                else
                {
                    obj = (List<string>)hlp.Show(sql, "CatDesc", "CatDesc", typeof(string), Utils.Helper.constr, "System.Data.SqlClient",
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

                    txtCatCode.Text = obj.ElementAt(0).ToString();
                    txtCatDesc.Text = obj.ElementAt(1).ToString();


                }
            }
        }

        private void txtCatCode_Validated(object sender, EventArgs e)
        {
             if (ctrlEmp1.txtCompCode.Text.Trim() == "" 
                || ctrlEmp1.txtWrkGrpCode.Text.Trim() == ""
                || txtCatCode.Text.Trim() == ""
                )
                return;

            string tCompCode = ctrlEmp1.txtCompCode.Text.Trim();
            string tWrkGrpCode = ctrlEmp1.txtWrkGrpCode.Text.Trim();
            //string tUnitCode = txtUnitCode.Text.Trim();
            string tCode = txtCatCode.Text.Trim();
            string desc;
            Utils.MastCodeValidate.GetCatDesc(tCompCode, tWrkGrpCode,  tCode, out desc);
            txtCatDesc.Text = desc;

            if (string.IsNullOrEmpty(desc))
            {
                txtCatCode.Text = "";
                txtCatDesc.Text = "";
            }

        }

        //private void txtEmpTypeCode_KeyDown(object sender, KeyEventArgs e)
        //{
        //    if (ctrlEmp1.txtCompCode.Text.Trim() == ""
        //       || ctrlEmp1.txtWrkGrpCode.Text.Trim() == ""

        //       )
        //        return;

        //    if (e.KeyCode == Keys.F1 || e.KeyCode == Keys.F2)
        //    {
        //        List<string> obj = new List<string>();

        //        Help_F1F2.ClsHelp hlp = new Help_F1F2.ClsHelp();
        //        string sql = "";


        //        sql = "Select EmpTypeCode,EmpTypeDesc From MastEmpType Where CompCode ='" + ctrlEmp1.txtCompCode.Text.Trim() + "' and WrkGrp = '" + ctrlEmp1.txtWrkGrpCode.Text.Trim() + "' ";
        //        if (e.KeyCode == Keys.F1)
        //        {

        //            obj = (List<string>)hlp.Show(sql, "EmpTypeCode", "EmpTypeCode", typeof(string), Utils.Helper.constr, "System.Data.SqlClient",
        //           100, 300, 400, 600, 100, 100);
        //        }
        //        else
        //        {
        //            obj = (List<string>)hlp.Show(sql, "EmpTypeDesc", "EmpTypeDesc", typeof(string), Utils.Helper.constr, "System.Data.SqlClient",
        //          100, 300, 400, 600, 100, 100);
        //        }

        //        if (obj.Count == 0)
        //        {

        //            return;
        //        }
        //        else if (obj.ElementAt(0).ToString() == "0")
        //        {
        //            return;
        //        }
        //        else if (obj.ElementAt(0).ToString() == "")
        //        {
        //            return;
        //        }
        //        else
        //        {

        //            txtEmpTypeCode.Text = obj.ElementAt(0).ToString();
        //            txtEmpTypeDesc.Text = obj.ElementAt(1).ToString();


        //        }
        //    }
        //}

        //private void txtEmpTypeCode_Validated(object sender, EventArgs e)
        //{
        //    if (ctrlEmp1.txtCompCode.Text.Trim() == "" || ctrlEmp1.txtWrkGrpCode.Text.Trim() == "" )
        //        return;



        //    DataSet ds = new DataSet();
        //    string sql = "select * From MastEmpType where CompCode ='" + ctrlEmp1.txtCompCode.Text.Trim() + "' " +
        //            " and WrkGrp='" + ctrlEmp1.txtWrkGrpCode.Text.Trim() + "' and EmpTypeCode ='" + txtEmpTypeCode.Text.Trim() + "'";

        //    ds = Utils.Helper.GetData(sql, Utils.Helper.constr);
        //    bool hasRows = ds.Tables.Cast<DataTable>()
        //                   .Any(table => table.Rows.Count != 0);

        //    if (hasRows)
        //    {
        //        foreach (DataRow dr in ds.Tables[0].Rows)
        //        {                   
        //            txtEmpTypeCode.Text = dr["EmpTypeCode"].ToString();
        //            txtEmpTypeDesc.Text = dr["EmpTypeDesc"].ToString();                   
        //        }
        //    }
        //    else
        //    {
        //        txtEmpTypeCode.Text = "";
        //        txtEmpTypeDesc.Text = "";
        //    }


        //}

        private void txtContCode_KeyDown(object sender, KeyEventArgs e)
        {
           

            if (ctrlEmp1.txtCompCode.Text.Trim() == "" 
                || ctrlEmp1.txtWrkGrpCode.Text.Trim() == ""
                || txtUnitCode.Text.Trim() == ""
                )
                return;


            if (e.KeyCode == Keys.F1 || e.KeyCode == Keys.F2)
            {
                List<string> obj = new List<string>();

                Help_F1F2.ClsHelp hlp = new Help_F1F2.ClsHelp();
                string sql = "";

                sql = "Select ContCode,ContName From MastCont Where CompCode ='" + ctrlEmp1.txtCompCode.Text.Trim() + "' " +
                 " and WrkGrp = '" + ctrlEmp1.txtWrkGrpCode.Text.Trim() + "' and UnitCode ='" + txtUnitCode.Text.Trim() + "'";

                if (e.KeyCode == Keys.F1)
                {
                  
                    obj = (List<string>)hlp.Show(sql, "ContCode", "ContCode", typeof(string), Utils.Helper.constr, "System.Data.SqlClient",
                   100, 300, 400, 600, 100, 100);
                }
                else
                {
              
                    obj = (List<string>)hlp.Show(sql, "ContName", "ContName", typeof(string), Utils.Helper.constr, "System.Data.SqlClient",
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

                    txtContCode.Text = obj.ElementAt(0).ToString();
                    txtContDesc.Text = obj.ElementAt(1).ToString();

                }
            }
        }

        private void txtContCode_Validated(object sender, EventArgs e)
        {
            if (ctrlEmp1.txtCompCode.Text.Trim() == ""
                || ctrlEmp1.txtWrkGrpCode.Text.Trim() == ""
                || txtContCode.Text.Trim() == ""
                || txtUnitCode.Text.Trim() == ""
                )
                return;

            string tCompCode = ctrlEmp1.txtCompCode.Text.Trim();
            string tWrkGrpCode = ctrlEmp1.txtWrkGrpCode.Text.Trim();
            string tUnitCode = txtUnitCode.Text.Trim();
            string tCode = txtContCode.Text.Trim();
            string desc;
            Utils.MastCodeValidate.GetContDesc(tCompCode, tWrkGrpCode,tUnitCode ,tCode, out desc);
            txtContDesc.Text = desc;

            if (string.IsNullOrEmpty(desc))
            {
                txtContCode.Text = "";
                txtContDesc.Text = "";
            }

        }

        private void txtGradeCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (ctrlEmp1.txtCompCode.Text.Trim() == ""
                || ctrlEmp1.txtWrkGrpCode.Text.Trim() == ""
                || txtUnitCode.Text.Trim() == ""
                )
                return;

            if (e.KeyCode == Keys.F1 || e.KeyCode == Keys.F2)
            {
                List<string> obj = new List<string>();

                Help_F1F2.ClsHelp hlp = new Help_F1F2.ClsHelp();
                string sql = "";


                sql = "Select GradeCode,GradeDesc From MastGrade Where CompCode ='" + ctrlEmp1.txtCompCode.Text.Trim() + "' and WrkGrp = '" + ctrlEmp1.txtWrkGrpCode.Text.Trim() + "' ";
                if (e.KeyCode == Keys.F1)
                {

                    obj = (List<string>)hlp.Show(sql, "GradeCode", "GradeCode", typeof(string), Utils.Helper.constr, "System.Data.SqlClient",
                   100, 300, 400, 600, 100, 100);
                }
                else
                {
                    obj = (List<string>)hlp.Show(sql, "GradeDesc", "GradeDesc", typeof(string), Utils.Helper.constr, "System.Data.SqlClient",
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

                    txtGradeCode.Text = obj.ElementAt(0).ToString();
                    txtGradeDesc.Text = obj.ElementAt(1).ToString();
                    
                }
            }
        }

        private void txtGradeCode_Validated(object sender, EventArgs e)
        {
            if (ctrlEmp1.txtCompCode.Text.Trim() == ""
                || ctrlEmp1.txtWrkGrpCode.Text.Trim() == ""
                || txtGradeCode.Text.Trim() == ""
                )
                return;

            string tCompCode = ctrlEmp1.txtCompCode.Text.Trim();
            string tWrkGrpCode = ctrlEmp1.txtWrkGrpCode.Text.Trim();
            string tCode = txtGradeCode.Text.Trim();
            string desc;
            Utils.MastCodeValidate.GetGradeDesc(tCompCode, tWrkGrpCode, tCode, out desc);
            txtGradeDesc.Text = desc;

            if (string.IsNullOrEmpty(desc))
            {
                txtGradeCode.Text = "";
                txtGradeDesc.Text = "";
            }
        }

        private void txtDesgCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (ctrlEmp1.txtCompCode.Text.Trim() == ""
                || ctrlEmp1.txtWrkGrpCode.Text.Trim() == ""
               
                )
                return;

            if (e.KeyCode == Keys.F1 || e.KeyCode == Keys.F2)
            {
                List<string> obj = new List<string>();

                Help_F1F2.ClsHelp hlp = new Help_F1F2.ClsHelp();
                string sql = "";


                sql = "Select DesgCode,DesgDesc From MastDesg Where CompCode ='" + ctrlEmp1.txtCompCode.Text.Trim() + "' and WrkGrp = '" + ctrlEmp1.txtWrkGrpCode.Text.Trim() + "' ";
                if (e.KeyCode == Keys.F1)
                {

                    obj = (List<string>)hlp.Show(sql, "DesgCode", "DesgCode", typeof(string), Utils.Helper.constr, "System.Data.SqlClient",
                   100, 300, 400, 600, 100, 100);
                }
                else
                {
                    obj = (List<string>)hlp.Show(sql, "DesgDesc", "DesgDesc", typeof(string), Utils.Helper.constr, "System.Data.SqlClient",
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

                    txtDesgCode.Text = obj.ElementAt(0).ToString();
                    txtDesgDesc.Text = obj.ElementAt(1).ToString();
                   

                }
            }
        }

        private void txtDesgCode_Validated(object sender, EventArgs e)
        {
            if (ctrlEmp1.txtCompCode.Text.Trim() == ""
                || ctrlEmp1.txtWrkGrpCode.Text.Trim() == ""
                || txtDesgCode.Text.Trim() ==  ""
                )
                return;

            string tCompCode = ctrlEmp1.txtCompCode.Text.Trim();
            string tWrkGrpCode = ctrlEmp1.txtWrkGrpCode.Text.Trim();
            string tCode = txtDesgCode.Text.Trim();
            string desc;
            Utils.MastCodeValidate.GetDesgDesc(tCompCode, tWrkGrpCode, tCode, out desc);
            txtDesgDesc.Text = desc;

            if (string.IsNullOrEmpty(desc))
            {
                txtDesgCode.Text = "";
                txtDesgDesc.Text = "";
            }

        }

      
        private void frmMastEmpJobProfile_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyData == Keys.Enter))
            {
                SelectNextControl(ActiveControl, true, true, true, true);
            }
        }

        private void txtValidFrom_KeyDown(object sender, KeyEventArgs e)
        {
            if (ctrlEmp1.txtCompCode.Text.Trim() == "" || ctrlEmp1.txtEmpUnqID.Text.Trim() == ""
                
                )
                return;

            if (e.KeyCode == Keys.F1 )
            {
                List<string> obj = new List<string>();

                Help_F1F2.ClsHelp hlp = new Help_F1F2.ClsHelp();
                string sql = "";

                sql = "Select ValidFrom,EmpUnqID,UnitCode,DeptCode,StatCode From MastEmpJobProfile Where EmpUnqID ='" + ctrlEmp1.txtEmpUnqID.Text.Trim() + "' ";

                obj = (List<string>)hlp.Show(sql, "ValidFrom", "ValidFrom", typeof(string), Utils.Helper.constr, "System.Data.SqlClient",
                100, 300, 400, 600, 100, 100);
               

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
                    txtValidFrom.EditValue = obj.ElementAt(0).ToString();
                                       
                }
            }
        }

        private void txtValidFrom_Validated(object sender, EventArgs e)
        {
            if (ctrlEmp1.txtCompCode.Text.Trim() == "" 
                || ctrlEmp1.txtEmpUnqID.Text.Trim() == ""
                || txtValidFrom.EditValue == null
                )              
            {
                mode = "NEW";
                return;
            }
            string sql = "Select * from MastEmpJobProfile Where EmpUnqID='" + ctrlEmp1.txtEmpUnqID.Text.Trim() + "' and " +
                " ValidFrom ='" + txtValidFrom.DateTime.Date.ToString("yyyy-MM-dd") + "'";
            string err = string.Empty;
            DataSet ds = Utils.Helper.GetData(sql, Utils.Helper.constr, out err);
            bool hasRows = ds.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);
            mode = "NEW";
            if (hasRows)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    txtUnitCode.Text = dr["UnitCode"].ToString();
                    txtUnitCode_Validated(sender, e);

                    txtDeptCode.Text = dr["DeptCode"].ToString();
                    txtDeptCode_Validated(sender, e);
                    txtStatCode.Text = dr["StatCode"].ToString();
                    txtStatCode_Validated(sender, e);
                    txtCatCode.Text = dr["CatCode"].ToString();
                    txtCatCode_Validated(sender, e);
                    txtDesgCode.Text = dr["DesgCode"].ToString();
                    txtDesgCode_Validated(sender, e);
                    txtGradeCode.Text = dr["GradeCode"].ToString();
                    txtGradeCode_Validated(sender, e);

                    txtContCode.Text = dr["ContCode"].ToString();
                    txtContCode_Validated(sender, e);
                    txtCostCode.Text = dr["CostCode"].ToString();
                    txtCostCode_Validated(sender, e);

                    txtWeekOff.Text = dr["WeekOff"].ToString();
                    chkOTFlg.Checked = Convert.ToBoolean(dr["OTFLG"]);

                    mode = "OLD";
                }
            }

            SetRights();
            
        }

        private void txtUnitCode_Validated(object sender, EventArgs e)
        {
            if (ctrlEmp1.txtCompCode.Text.Trim() == "" || 
                ctrlEmp1.txtWrkGrpCode.Text.Trim() == "" || 
                txtUnitCode.Text.Trim() == ""
                )
                return;
            string tCompCode = ctrlEmp1.txtCompCode.Text.Trim();
            string tWrkGrpCode = ctrlEmp1.txtWrkGrpCode.Text.Trim();
            string tUnitCode = txtUnitCode.Text.Trim();
            string desc;
            Utils.MastCodeValidate.GetUnitDesc(tCompCode, tWrkGrpCode, tUnitCode, out desc);
            txtUnitDesc.Text = desc;

            if (string.IsNullOrEmpty(desc))
            {
                txtUnitCode.Text = "";
                txtUnitDesc.Text = "";
            }
        }

        private void txtUnitCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (ctrlEmp1.txtCompCode.Text.Trim() == "" || ctrlEmp1.txtWrkGrpCode.Text.Trim() == "")
                return;

            if (e.KeyCode == Keys.F1 || e.KeyCode == Keys.F2)
            {
                List<string> obj = new List<string>();

                Help_F1F2.ClsHelp hlp = new Help_F1F2.ClsHelp();
                string sql = "";

                sql = "Select UnitCode,UnitName From MastUnit Where CompCode ='" + ctrlEmp1.txtCompCode.Text.Trim() + "' " +
                   " and WrkGrp = '" + ctrlEmp1.txtWrkGrpCode.Text.Trim() + "'  ";
                  

                if (e.KeyCode == Keys.F1)
                {

                    obj = (List<string>)hlp.Show(sql, "UnitCode", "UnitCode", typeof(string), Utils.Helper.constr, "System.Data.SqlClient",
                   100, 300, 400, 600, 100, 100);
                }
                else
                {
                    obj = (List<string>)hlp.Show(sql, "UnitName", "UnitName", typeof(string), Utils.Helper.constr, "System.Data.SqlClient",
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
                    txtUnitCode.Text = obj.ElementAt(0).ToString();
                    txtUnitDesc.Text = obj.ElementAt(1).ToString();
                }
            }
        }
    }
}
