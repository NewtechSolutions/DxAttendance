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

        private void ctrlEmpValidateEvent_Handler(object sender, EventArgs e)
        {
            if (!ctrlEmp1.cEmp.Active)
            {
                Emp = new clsEmp();
            }
            else
            {
                Emp.CompCode = ctrlEmp1.txtCompCode.Text.Trim();
                Emp.EmpUnqID = ctrlEmp1.txtEmpUnqID.Text.Trim();
                Emp.GetEmpDetails(Emp.CompCode, Emp.EmpUnqID);
                DispalyData(Emp);
                mode = "OLD";
                SetRights();
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

        private string DataValidate()
        {
            string err = string.Empty;

            if (string.IsNullOrEmpty(ctrlEmp1.txtCompCode.Text.Trim().ToString()))
            {
                err = err + "Please Enter CompCode..." + Environment.NewLine;
            }

            if (string.IsNullOrEmpty(ctrlEmp1.cEmp.CompDesc.Trim().ToString()))
            {
                err = err + "Invalid CompCode..." + Environment.NewLine;
            }

            
            if (string.IsNullOrEmpty(ctrlEmp1.txtEmpUnqID.Text.Trim().ToString()))
            {
                err = err + "Please Enter EmpUnqID..." + Environment.NewLine;
            }

            if (!string.IsNullOrEmpty(ctrlEmp1.cEmp.EmpUnqID) && !ctrlEmp1.IsValid )
            {
                err = err + "Invalid/InActive EmpUnqID..." + Environment.NewLine;
            }

            if (Emp.UnitCode == "")
            {
                err = err + "Unit Code is required../Please update from EmpBasicData module..." + Environment.NewLine;
            }

            if (Emp.PayrollFlg || Emp.ContFlg)
            {
                if (string.IsNullOrEmpty(txtEmpCode.Text.Trim()))
                {
                    err = err + "EmpCode is required.." + Environment.NewLine;
                }
            }

            if (Emp.ContFlg)
            {
                if (string.IsNullOrEmpty(txtContCode.Text.Trim()) || string.IsNullOrEmpty(txtContDesc.Text.Trim()))
                {
                    err = err + "Invalid ContCode.." + Environment.NewLine;
                }
            }

            if (chkAutoShift.Checked && txtShiftCode.Text.Trim() == "")
            {
                err = err + "ShiftCode Required...." + Environment.NewLine;
            }

            return err;
        }

        private void ResetCtrl()
        {
            btnAdd.Enabled = false;
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;

            ctrlEmp1.ResetCtrl();

            txtCatCode.Text = string.Empty;
            txtCatDesc.Text = string.Empty;
            txtEmpTypeCode.Text = string.Empty;
            txtEmpTypeDesc.Text = string.Empty;
            txtDeptCode.Text = string.Empty;
            txtDeptDesc.Text = string.Empty;
            txtStatCode.Text = string.Empty;
            txtStatDesc.Text = string.Empty;
            txtDesgCode.Text = string.Empty;
            txtDesgDesc.Text = string.Empty;
            txtGradeCode.Text = string.Empty;
            txtGradeDesc.Text = string.Empty;
            txtContCode.Text = "";
            txtContDesc.Text = "";

            txtEmpCode.Text = "";
            txtOldEmpCode.Text = "";
            txtSAPID.Text = "";
            txtWeekOff.Text = "";

            chkAutoShift.Checked = false;
            chkOTFlg.Checked = false;

            txtLeftDt.EditValue = null;



            oldCode = "";
            mode = "NEW";
        }
        
        private void SetRights()
        {
            btnAdd.Enabled = false;
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;

            if ( ctrlEmp1.txtEmpUnqID.Text.Trim() != "" && mode == "NEW" && GRights.Contains("A") )
            {
                btnAdd.Enabled = false;
                btnUpdate.Enabled = true;
                btnDelete.Enabled = false;
            }
            else if (ctrlEmp1.txtEmpUnqID.Text.Trim() != "" && mode == "OLD")
            {
                btnAdd.Enabled = false;

                if(GRights.Contains("U"))
                    btnUpdate.Enabled = true;
                if (GRights.Contains("D"))
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

            MessageBox.Show("Not implemented....", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);  
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string err = DataValidate();
            if (!string.IsNullOrEmpty(err))
            {
                MessageBox.Show(err, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show("Not Allowed....", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);           

            //using (SqlConnection cn = new SqlConnection(Utils.Helper.constr))
            //{
            //    using (SqlCommand cmd = new SqlCommand())
            //    {
            //        try
            //        {
            //            cn.Open();
            //            cmd.Connection = cn;
            //            string sql = "Update MastCostCode set CostDesc = '{0}',UpdDt = GetDate(),UpdID ='{1}' where CostCode = '{2}'";
            //            sql = string.Format(sql, txtDescription.Text.Trim().ToString(),
            //                Utils.User.GUserID,
            //                txtCostCode.Text.Trim().ToString().ToUpper()
            //                );

            //            cmd.CommandText = sql;
            //            cmd.ExecuteNonQuery();
            //            MessageBox.Show("Record saved...", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //            ResetCtrl();
            //            LoadGrid();
            //            return;

            //        }
            //        catch (Exception ex)
            //        {
            //            MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //        }
            //    }
            //}

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
           
            string err = DataValidate();
            if (!string.IsNullOrEmpty(err))
            {
                MessageBox.Show(err, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrEmpty(err))
            {

                DialogResult qs = MessageBox.Show("Are You Sure to Delete this Record...?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (qs == DialogResult.No)
                {
                    return;
                }

                using (SqlConnection cn = new SqlConnection(Utils.Helper.constr))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        try
                        {
                            cn.Open();
                            string sql = " ";
                                
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = sql;
                            cmd.Connection = cn;
                            cmd.ExecuteNonQuery();
                                                       
                            
                            
                            

                            MessageBox.Show("Record Deleted...", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ResetCtrl();
                           
                            return;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }
            }
            
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

        private void DispalyData(clsEmp temp)
        {
            txtContCode.Text = temp.ContCode;
            txtEmpCode.Text = temp.EmpCode;
            txtSAPID.Text = temp.SAPID;
            txtOldEmpCode.Text = temp.OLDEmpCode;
            txtShiftCode.Text = temp.ShiftCode;
            txtWeekOff.Text = temp.WeekOffDay;
            chkAutoShift.Checked = temp.AutoShift;
            chkOTFlg.Checked = temp.OTFLG;

            txtDeptCode.Text = temp.DeptCode;
            txtDeptDesc.Text = temp.DeptDesc;
            txtStatCode.Text = temp.StatCode;
            txtStatDesc.Text = temp.StatDesc;
            txtEmpTypeCode.Text = temp.EmpTypeCode;
            txtEmpTypeDesc.Text = temp.EmpTypeDesc;
            txtDesgCode.Text = temp.DesgCode;
            txtDesgDesc.Text = temp.DesgDesc;
            txtGradeCode.Text = temp.GradeCode;
            txtGradeDesc.Text = temp.GradeDesc;
            txtCatCode.Text = temp.CatCode;
            txtCatDesc.Text = temp.CatDesc;
            
            if(temp.LeftDt.HasValue){
                txtLeftDt.DateTime = Convert.ToDateTime(temp.LeftDt);
                txtLeftDt.Enabled = false;
            }
            else
            {
                txtLeftDt.EditValue = null;
                txtLeftDt.Enabled = true;
            }

            if (Globals.GetWrkGrpRights(685,"",temp.EmpUnqID))
            {
                txtLeftDt.Enabled = true;
            }
            
            
        }

        private void txtShiftCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (ctrlEmp1.txtCompCode.Text.Trim() == "" )
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
                    txtShiftDesc.Text = obj.ElementAt(1).ToString();

                    
                }
            }
        }

        private void txtShiftCode_Validated(object sender, EventArgs e)
        {
            if (ctrlEmp1.txtCompCode.Text.Trim() == "" || ctrlEmp1.txtCompDesc.Text.Trim() == "" || txtShiftCode.Text.Trim() == "")
            {
                mode = "NEW";
                return;
            }

            DataSet ds = new DataSet();
            string sql = "select * From  MastShift where CompCode ='" + ctrlEmp1.txtCompCode.Text.Trim() + "' and ShiftCode ='" + txtShiftCode.Text.Trim() + "'";

            ds = Utils.Helper.GetData(sql, Utils.Helper.constr);
            bool hasRows = ds.Tables.Cast<DataTable>()
                           .Any(table => table.Rows.Count != 0);

            if (hasRows)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                   
                    txtShiftCode.Text = dr["ShiftCode"].ToString();
                    txtShiftDesc.Text = dr["ShiftDesc"].ToString();
                   
                }
            }
            else
            {
                txtShiftDesc.Text = "";
            }
            
        }

        private void txtDeptCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (ctrlEmp1.txtCompCode.Text.Trim() == "" || ctrlEmp1.txtWrkGrpCode.Text.Trim() == "" || ctrlEmp1.txtUnitCode.Text.Trim() == "")
                return;

            if (e.KeyCode == Keys.F1 || e.KeyCode == Keys.F2)
            {
                List<string> obj = new List<string>();

                Help_F1F2.ClsHelp hlp = new Help_F1F2.ClsHelp();
                string sql = "";

                sql = "Select DeptCode,DeptDesc From MastDept Where CompCode ='" + ctrlEmp1.txtCompCode.Text.Trim() + "' " +
                    " and WrkGrp = '" + ctrlEmp1.txtWrkGrpCode.Text.Trim() + "' and UnitCode ='" + ctrlEmp1.txtUnitCode.Text.Trim() + "'";

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
                || ctrlEmp1.txtUnitCode.Text.Trim() == ""
                || ctrlEmp1.txtDeptCode.Text.Trim() == ""
                )
                return;

            txtDeptCode.Text = txtDeptCode.Text.Trim().ToString().PadLeft(3, '0');

            DataSet ds = new DataSet();
            string sql = "select * From MastDept where CompCode ='" + ctrlEmp1.txtCompCode.Text.Trim() + "' " +
                    " and WrkGrp='" + ctrlEmp1.txtWrkGrpCode.Text.Trim() + "' " +
                    " and UnitCode ='" + ctrlEmp1.txtUnitCode.Text.Trim() + "' " +
                    " and DeptCode ='" + ctrlEmp1.txtDeptCode.Text.Trim() + "' ";

            ds = Utils.Helper.GetData(sql, Utils.Helper.constr);
            bool hasRows = ds.Tables.Cast<DataTable>()
                           .Any(table => table.Rows.Count != 0);

            if (hasRows)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                   
                    txtDeptCode.Text = dr["DeptCode"].ToString();
                    txtDeptDesc.Text = dr["DeptDesc"].ToString();
                   
                }
            }
            else
            {
                txtDeptDesc.Text = "";
            }
        }

        private void txtStatCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (ctrlEmp1.txtCompCode.Text.Trim() == "" || ctrlEmp1.txtWrkGrpCode.Text.Trim() == ""
                 || ctrlEmp1.txtUnitCode.Text.Trim() == ""
                 || ctrlEmp1.txtDeptCode.Text.Trim() == ""
                 )
                return;

            if (e.KeyCode == Keys.F1 || e.KeyCode == Keys.F2)
            {
                List<string> obj = new List<string>();

                Help_F1F2.ClsHelp hlp = new Help_F1F2.ClsHelp();
                string sql = "";

                sql = "Select StatCode,StatDesc From MastStat Where CompCode ='" + ctrlEmp1.txtCompCode.Text.Trim() + "' " +
                   " and WrkGrp = '" + ctrlEmp1.txtWrkGrpCode.Text.Trim() + "' and UnitCode ='" + ctrlEmp1.txtUnitCode.Text.Trim() + "' " +
                   " and DeptCode='" + ctrlEmp1.txtDeptCode.Text.Trim() + "'";

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
                || ctrlEmp1.txtUnitCode.Text.Trim() == ""
                || ctrlEmp1.txtDeptCode.Text.Trim() == ""
                || ctrlEmp1.txtStatCode.Text.Trim() == ""
                )
                return;

            txtStatCode.Text = txtStatCode.Text.Trim().ToString().PadLeft(3, '0');

            DataSet ds = new DataSet();
            string sql = "select * From MastStat where CompCode ='" + ctrlEmp1.txtCompCode.Text.Trim() + "' " +
                    " and WrkGrp='" + ctrlEmp1.txtWrkGrpCode.Text.Trim() + "' " +
                    " and UnitCode ='" + ctrlEmp1.txtUnitCode.Text.Trim() + "' " +
                    " and DeptCode ='" + ctrlEmp1.txtDeptCode.Text.Trim() + "' " +
                    " and StatCode ='" + ctrlEmp1.txtStatCode.Text.Trim() + "'";

            ds = Utils.Helper.GetData(sql, Utils.Helper.constr);
            bool hasRows = ds.Tables.Cast<DataTable>()
                           .Any(table => table.Rows.Count != 0);

            if (hasRows)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
            
                    txtStatCode.Text = dr["StatCode"].ToString();
                    txtStatDesc.Text = dr["StatDesc"].ToString();
                    
                }
            }
            else
            {
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
               
           

           
            DataSet ds = new DataSet();
            string sql = "select * From MastCat where CompCode ='" + ctrlEmp1.txtCompCode.Text.Trim() + "' "
                    + " and WrkGrp='" + ctrlEmp1.txtWrkGrpCode.Text.Trim() + "' "
                    + " and CatCode ='" + txtCatCode.Text.Trim() + "' ";

            ds = Utils.Helper.GetData(sql, Utils.Helper.constr);
            bool hasRows = ds.Tables.Cast<DataTable>()
                           .Any(table => table.Rows.Count != 0);

            if (hasRows)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {

                    txtCatCode.Text = dr["CatCode"].ToString();
                    txtCatDesc.Text = dr["CatDesc"].ToString();
                }
            }
            else
            {
                txtCatDesc.Text = "";
            }

        }

        private void txtEmpTypeCode_KeyDown(object sender, KeyEventArgs e)
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


                sql = "Select EmpTypeCode,EmpTypeDesc From MastEmpType Where CompCode ='" + ctrlEmp1.txtCompCode.Text.Trim() + "' and WrkGrp = '" + ctrlEmp1.txtWrkGrpCode.Text.Trim() + "' ";
                if (e.KeyCode == Keys.F1)
                {

                    obj = (List<string>)hlp.Show(sql, "EmpTypeCode", "EmpTypeCode", typeof(string), Utils.Helper.constr, "System.Data.SqlClient",
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

                    txtEmpTypeCode.Text = obj.ElementAt(0).ToString();
                    txtEmpTypeDesc.Text = obj.ElementAt(1).ToString();
                   

                }
            }
        }

        private void txtEmpTypeCode_Validated(object sender, EventArgs e)
        {
            if (ctrlEmp1.txtCompCode.Text.Trim() == "" || ctrlEmp1.txtWrkGrpCode.Text.Trim() == "" )
                return;

            txtEmpTypeCode.Text = txtEmpTypeCode.Text.Trim().ToString().PadLeft(3, '0');

            DataSet ds = new DataSet();
            string sql = "select * From MastEmpType where CompCode ='" + ctrlEmp1.txtCompCode.Text.Trim() + "' " +
                    " and WrkGrp='" + ctrlEmp1.txtWrkGrpCode.Text.Trim() + "' and EmpTypeCode ='" + txtEmpTypeCode.Text.Trim() + "'";

            ds = Utils.Helper.GetData(sql, Utils.Helper.constr);
            bool hasRows = ds.Tables.Cast<DataTable>()
                           .Any(table => table.Rows.Count != 0);

            if (hasRows)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {                   
                    txtEmpTypeCode.Text = dr["EmpTypeCode"].ToString();
                    txtEmpTypeDesc.Text = dr["EmpTypeDesc"].ToString();                   
                }
            }
            else
            {
                txtEmpTypeDesc.Text = "";
            }
            

        }

        private void txtContCode_KeyDown(object sender, KeyEventArgs e)
        {
           

            if (ctrlEmp1.txtCompCode.Text.Trim() == "" 
                || ctrlEmp1.txtWrkGrpCode.Text.Trim() == ""
                || ctrlEmp1.txtUnitCode.Text.Trim() == ""
                )
                return;


            if (e.KeyCode == Keys.F1 || e.KeyCode == Keys.F2)
            {
                List<string> obj = new List<string>();

                Help_F1F2.ClsHelp hlp = new Help_F1F2.ClsHelp();
                string sql = "";

                sql = "Select ContCode,ContName From MastCont Where CompCode ='" + ctrlEmp1.txtCompCode.Text.Trim() + "' " +
                 " and WrkGrp = '" + ctrlEmp1.txtWrkGrpCode.Text.Trim() + "' and UnitCode ='" + ctrlEmp1.txtUnitCode.Text.Trim() + "'";

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
                || ctrlEmp1.txtUnitCode.Text.Trim() == ""
                )
                return;

            txtContCode.Text = txtContCode.Text.Trim().ToString().PadLeft(3, '0');

            DataSet ds = new DataSet();
            string sql = "select * From MastCont where CompCode ='" + ctrlEmp1.txtCompCode.Text.Trim() + "' " +
                    " and WrkGrp='" + ctrlEmp1.txtWrkGrpCode.Text.Trim() + "' " +
                    " and UnitCode ='" + ctrlEmp1.txtUnitCode.Text.Trim() + "' " +
                    " and ContCode ='" + txtContCode.Text.Trim() + "' ";

            ds = Utils.Helper.GetData(sql, Utils.Helper.constr);
            bool hasRows = ds.Tables.Cast<DataTable>()
                           .Any(table => table.Rows.Count != 0);

            if (hasRows)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    txtContCode.Text = dr["ContCode"].ToString();
                    txtContDesc.Text = dr["ContName"].ToString();
                   
                }

            }
            else
            {
                txtContDesc.Text = "";
            }
            
        }

        private void txtGradeCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (ctrlEmp1.txtCompCode.Text.Trim() == ""
                || ctrlEmp1.txtWrkGrpCode.Text.Trim() == ""
                || ctrlEmp1.txtUnitCode.Text.Trim() == ""
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

            txtGradeCode.Text = txtGradeCode.Text.Trim().ToString().PadLeft(3, '0');

            DataSet ds = new DataSet();
            string sql = "select * From MastGrade where CompCode ='" + ctrlEmp1.txtCompCode.Text.Trim() + "' " +
                    " and WrkGrp='" + ctrlEmp1.txtWrkGrpCode.Text.Trim() + "' and GradeCode ='" + txtGradeCode.Text.Trim() + "'";

            ds = Utils.Helper.GetData(sql, Utils.Helper.constr);
            bool hasRows = ds.Tables.Cast<DataTable>()
                           .Any(table => table.Rows.Count != 0);

            if (hasRows)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    
                    txtGradeCode.Text = dr["GradeCode"].ToString();
                    txtGradeDesc.Text = dr["GradeDesc"].ToString();
                    
                }
            }
            else
            {
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
                || ctrlEmp1.txtDesgCode.Text.Trim() == ""
                )
                return;

            txtDesgCode.Text = txtDesgCode.Text.Trim().ToString().PadLeft(3, '0');

            DataSet ds = new DataSet();
            string sql = "select * From MastDesg where CompCode ='" + ctrlEmp1.txtCompCode.Text.Trim() + "' " +
                    " and WrkGrp='" + ctrlEmp1.txtWrkGrpCode.Text.Trim() + "' and DesgCode ='" + txtDesgCode.Text.Trim() + "'";

            ds = Utils.Helper.GetData(sql, Utils.Helper.constr);
            bool hasRows = ds.Tables.Cast<DataTable>()
                           .Any(table => table.Rows.Count != 0);

            if (hasRows)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    
                    txtDesgCode.Text = dr["DesgCode"].ToString();
                    txtDesgDesc.Text = dr["DesgDesc"].ToString();
                   
                }
            }
            else
            {
                txtDesgDesc.Text = "";
            }

           

        }

    }
}
