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
    public partial class frmTranLeaveEncash : Form
    {
        public string mode = "NEW";
        public string GRights = "XXXV";
        public string oldCode = "";
        public static clsEmp Emp = new clsEmp();

        DataSet LeaveTypeDS = new DataSet();
        DataTable LeaveTypeTB = new DataTable();
        public frmTranLeaveEncash()
        {
            InitializeComponent();
            this.ctrlEmp1.EmpUnqIDValidated += new EventHandler(this.ctrlEmpValidateEvent_Handler);
            //this.ctrlEmp1.CompCodeValidated += new EventHandler(this.ctrlCompValidateEvent_Handler);
        }

        private void ctrlEmpValidateEvent_Handler(object sender, EventArgs e)
        {
            Emp = new clsEmp();
            Emp.EmpUnqID = ctrlEmp1.txtEmpUnqID.Text.Trim();
            Emp.GetEmpDetails(Emp.EmpUnqID);

            txtLeaveType.Properties.Items.Clear();
            grid.DataSource = null;

            if (!ctrlEmp1.cEmp.Active)
            {
                grid.DataSource = null;
            }
            else
            {
                //load Leave Types
                if (ctrlEmp1.txtWrkGrpCode.Text.Trim() != "")
                {

                    DataRow[] tdr = LeaveTypeTB.Select("WrkGrp='" + ctrlEmp1.cEmp.WrkGrp + "'");
                    foreach (DataRow dr in tdr)
                    {
                        txtLeaveType.Properties.Items.Add(dr["LeaveTyp"].ToString());
                    }

                }
            }

        }

      

        private void frmMastEmpLeaveEncash_Load(object sender, EventArgs e)
        {
            ResetCtrl();
            GRights = Attendance.Classes.Globals.GetFormRights(this.Name);
            SetRights();
            LeaveTypeDS = Utils.Helper.GetData("Select * from MastLeave Where Compcode='" + ctrlEmp1.txtCompCode.Text.Trim().ToString() + "'  and KeepBal = 1 and AllowEncash=1 ", Utils.Helper.constr);
            //select wrkgrp from LeaveTyps
            bool hasRows = LeaveTypeDS.Tables.Cast<DataTable>()
                           .Any(table => table.Rows.Count != 0);

            if (hasRows)
            {
                LeaveTypeTB = LeaveTypeDS.Tables[0];
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


            if (string.IsNullOrEmpty(txtYear.Text))
            {
                err = err + "Please Enter Year..." + Environment.NewLine;
            }

            if (string.IsNullOrEmpty(txtLeaveType.Text))
            {
                err = err + "Please Enter LeaveType Description..." + Environment.NewLine;
            }

            if(txtEncashDt.EditValue == null)
            {
                err += "Encashment Date is required..." + Environment.NewLine;
                return err;
            }

            if(txtEncashDt.DateTime.Year != Convert.ToInt32(txtYear.Text.ToString().Trim()))
            {
                err += "invalid Encashment Date..." + Environment.NewLine;
                return err;
            }
            if(mode == "NEW")
            {
                if (txtEncDay.Value > txtAvlBal.Value)
                {
                    err += "Leave Balance not available..." + Environment.NewLine;
                    return err;
                }
            }
            
            if(mode == "OLD")
            {

            }

            return err;
        }

        private void CancelReset()
        {
            ctrlEmp1.ResetCtrl();
            txtYear.Text = "";
            txtLeaveType.Text = "";
            txtLeaveType.Text = "";
            txtEncashDt.EditValue = null;
            txtAvlBal.Value = 0;
            txtEncDay.Value = 0;
            grid.DataSource = null;
            oldCode = "";
            mode = "NEW";
        }


        private void ResetCtrl()
        {

            //

            //txtYear.Text = "";

            txtYear.Enabled = true;
            //txtLeaveType.Properties.Items.Clear();
            txtLeaveType.Text = "";
            txtEncashDt.EditValue = null;
            txtAvlBal.Value = 0;
            txtEncDay.Value = 0;

            //grid.DataSource = null;

            oldCode = "";
            mode = "NEW";
        }
        
        private void SetRights()
        {
            btnAdd.Enabled = false;
            //btnUpdate.Enabled = false;
            btnDelete.Enabled = false;
            
            if (ctrlEmp1.txtEmpUnqID.Text.Trim() != "" )
            {
                //btnAdd.Enabled = false;
                if(mode == "NEW" && GRights.Contains("A"))
                {
                    btnAdd.Enabled = true;
                    
                }
                
                if(mode == "OLD" && GRights.Contains("U"))
                    btnDelete.Enabled = true;
                if (mode == "OLD" && GRights.Contains("D"))
                    btnDelete.Enabled = true;
            }
            
            if (GRights.Contains("XXXV"))
            {
                btnAdd.Enabled = false;
                //btnUpdate.Enabled = false;
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
                string tEmpUnqID = ctrlEmp1.txtEmpUnqID.Text.Trim().ToString();
                string tYear = txtYear.Text.ToString();
                string tDate = txtEncashDt.DateTime.Date.ToString("yyyy-MM-dd");
                string tLeaveTyp = txtLeaveType.Text.Trim();
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        cn.Open();
                        cmd.Connection = cn;

                        string sql = "insert into MastEmpLeaveEncash (tYear,EmpUnqID,LeaveTyp,EncashDt,EncashNo,AddDt,AddID ) values (" +
                            " '" + tYear + "','" + tEmpUnqID + "','" + tLeaveTyp + "','" + tDate + "','" + txtEncDay.Text.ToString() +  "'," + 
                            " GetDate(),'" + Utils.User.GUserID + "')";
                        

                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = sql;
                        cmd.ExecuteNonQuery();

                        SetEncashDayLeaveBal(tEmpUnqID, Convert.ToInt32(tYear), tLeaveTyp);
                        LoadGrid();
                        MessageBox.Show("Record Saved...", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        //private void btnUpdate_Click(object sender, EventArgs e)
        //{
        //    string err = DataValidate();
            
        //    if (!string.IsNullOrEmpty(err))
        //    {
        //        MessageBox.Show(err, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        return;
        //    }

        //    GrpMain.Enabled = false;

        //    Cursor.Current = Cursors.WaitCursor;

        //    using (SqlConnection cn = new SqlConnection(Utils.Helper.constr))
        //    {
        //        string tEmpUnqID = ctrlEmp1.txtEmpUnqID.Text.Trim().ToString();
        //        string tYear = txtYear.Text.ToString();
        //        string tDate = txtEncashDt.DateTime.Date.ToString("yyyy-MM-dd");
        //        string tLeaveTyp = txtLeaveType.Text.ToString().Trim();
        //        using (SqlCommand cmd = new SqlCommand())
        //        {
        //            try
        //            {
        //                cn.Open();
        //                cmd.Connection = cn;

        //                string sql = "Update MastEmpLeaveEncash Set EncashNo ='" + txtEncDay.Text.Trim().ToString() + "' " +                           
        //                   " UpdDt=GetDate(),UpdID='" + Utils.User.GUserID + "'" +
        //                   " Where  tYear ='" + tYear + "' EmpUnqID ='" + tEmpUnqID + "' and LeaveTyp = '" + tLeaveTyp + "' and EncashDt ='" + tDate + "' and";

        //                cmd.CommandType = CommandType.Text;
        //                cmd.CommandText = sql;
        //                cmd.ExecuteNonQuery();

        //                SetEncashDayLeaveBal(tEmpUnqID, Convert.ToInt32(tYear), tLeaveTyp);
        //                LoadGrid();
        //                MessageBox.Show("Record Updated...", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //                ResetCtrl();

        //            }
        //            catch (Exception ex)
        //            {
        //                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            }
        //        }
        //    }
        //    GrpMain.Enabled = true;

        //    Cursor.Current = Cursors.Default;

        //}

        private void SetEncashDayLeaveBal(string tEmpUnqID,int tYear, string LeaveType)
        {
            using(SqlConnection cn = new SqlConnection(Utils.Helper.constr))
            {
                try
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = cn;

                        int Encash = 0;
                        string sql = "select isnull(sum(EncashNo), 0) from MastEmpLeaveEncash where EmpUnqID = '" + tEmpUnqID + "' " +
                            " and LeaveTyp = '" + LeaveType + "' and tYear ='" + tYear.ToString() + "'";
                        string err;
                        string strEncash = Utils.Helper.GetDescription(sql, Utils.Helper.constr, out err);
                        if (int.TryParse(strEncash, out Encash))
                        {
                            sql = " Update LeaveBal Set ENC = '" + Encash.ToString() + "' Where EmpUnqID = '" + tEmpUnqID + "' " +
                            " and LeaveTyp = '" + LeaveType + "' and tYear ='" + tYear.ToString() + "'";

                            cmd.CommandText = sql;
                            cmd.ExecuteNonQuery();
                        }
                        
                    }

                }catch(Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
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
                string tEmpUnqID = ctrlEmp1.txtEmpUnqID.Text.Trim().ToString();
                string tYear = txtYear.Text.ToString();
                string tDate = txtEncashDt.DateTime.Date.ToString("yyyy-MM-dd");
                string tLeaveTyp = txtLeaveType.Text.ToString().Trim();

                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        cn.Open();
                        cmd.Connection = cn;

                        string sql = "Delete from MastEmpLeaveEncash Where tYear='" + tYear + "' and EmpUnqID ='" + tEmpUnqID + "'" +
                            " and LeaveTyp='" + tLeaveTyp + "' And EncashDt='" + tDate + "'";

                        cmd.CommandText = sql;
                        cmd.ExecuteNonQuery();

                        SetEncashDayLeaveBal(tEmpUnqID, Convert.ToInt32(tYear), tLeaveTyp);
                        LoadGrid();
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
            CancelReset();
            GRights = Attendance.Classes.Globals.GetFormRights(this.Name);
            
            SetRights();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
      
        private void frmTranLeaveEncash_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyData == Keys.Enter))
            {
                SelectNextControl(ActiveControl, true, true, true, true);
            }
        }

        private void txtLeaveType_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtAvlBal.Value = 0;

            if (string.IsNullOrEmpty(txtYear.Text.Trim()) 
                || string.IsNullOrEmpty(txtLeaveType.Text.ToString().Trim())
                || string.IsNullOrEmpty(ctrlEmp1.txtEmpUnqID.Text.Trim().ToString())
                )
                return;

            string tEmpUnqID = ctrlEmp1.txtEmpUnqID.Text.Trim().ToString();
            string tYear = txtYear.Text.ToString();
            string tDate = txtEncashDt.DateTime.Date.ToString("yyyy-MM-dd");
            string tLeaveTyp = txtLeaveType.Text.ToString().Trim();

            string sql = "SELECT isnull(Opn,0) - (isnull(AVL,0) + isnull(Enc,0) + isnull(ADV,0) )  as BAL FROM LeaveBal "
                    + " where EmpUnqID = '" + tEmpUnqID + "' "
                    + " And tYear ='" + tYear + "' "
                    + " And LeaveTyp ='" + tLeaveTyp+ "'";

            string tcnt = Utils.Helper.GetDescription(sql, Utils.Helper.constr);

            int tavl = 0;
            if (int.TryParse(tcnt, out tavl))
            {
                txtAvlBal.Value = tavl; 
            }
        }

        private void txtYear_Validated(object sender, EventArgs e)
        {
            if (txtYear.Text.Trim() == "")
            {
                grid.DataSource = null;
                return;
            }

            LoadGrid();
            
        }

        private void LoadGrid()
        {

            if (string.IsNullOrEmpty(ctrlEmp1.txtWrkGrpCode.Text.Trim().ToString())
                || string.IsNullOrEmpty(ctrlEmp1.txtCompCode.Text.Trim().ToString())
                || string.IsNullOrEmpty(ctrlEmp1.txtEmpUnqID.Text.Trim().ToString())
                || string.IsNullOrEmpty(txtYear.Text.Trim())

                )
            {
                return;
            }



            DataSet ds = new DataSet();
            string sql =
            " Select tYear, LeaveTyp,Opn,Avl,(OPN-(AVL+ADV+ENC)) as Bal ,Adv,ENC from leaveBal " +
             " where EmpUnqID='" + ctrlEmp1.txtEmpUnqID.Text.Trim().ToString() + "'" +
             " and tYear ='" + txtYear.Text + "' and LeaveTyp in (Select LeaveTyp from MastLeave Where WrkGrp ='" +
             "" + ctrlEmp1.txtWrkGrpCode.Text.Trim().ToString() + "' and AllowEncash=1)";

            ds = Utils.Helper.GetData(sql, Utils.Helper.constr);

            Boolean hasRows = ds.Tables.Cast<DataTable>()
                           .Any(table => table.Rows.Count != 0);

            if (hasRows)
            {
                grid.DataSource = ds;
                grid.DataMember = ds.Tables[0].TableName;
            }
            else
            {
                grid.DataSource = null;
            }
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            GridView view = (GridView)sender;
            Point pt = view.GridControl.PointToClient(Control.MousePosition);
            DoRowDoubleClick(view, pt);
        }

        private void DoRowDoubleClick(GridView view, Point pt)
        {
            GridHitInfo info = view.CalcHitInfo(pt);
            if (info.InRow || info.InRowCell)
            {
                txtYear.Text = gridView1.GetRowCellValue(info.RowHandle, "tYear").ToString();
                txtLeaveType.Text = gridView1.GetRowCellValue(info.RowHandle, "LeaveTyp").ToString();
                txtAvlBal.Text = gridView1.GetRowCellValue(info.RowHandle, "Bal").ToString();
                
                
            }


        }

        private void txtEncashDt_Validated(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ctrlEmp1.txtWrkGrpCode.Text.Trim().ToString())
               || string.IsNullOrEmpty(ctrlEmp1.txtCompCode.Text.Trim().ToString())
               || string.IsNullOrEmpty(ctrlEmp1.txtEmpUnqID.Text.Trim().ToString())
               || string.IsNullOrEmpty(txtYear.Text.Trim())
               || txtEncashDt.EditValue == null
               || string.IsNullOrEmpty(txtLeaveType.Text.Trim().ToString())
               )
            {
                return;
            }
            string tEmpUnqID = ctrlEmp1.txtEmpUnqID.Text.Trim().ToString();
            string tLeaveTyp = txtLeaveType.Text.Trim().ToString();

            DataSet ds = new DataSet();
            string sql = "Select tYear,EmpUnqID,LeaveTyp,EncashDt,EncashNo From MastEmpLeaveEncash " +
            " Where tYear ='" + txtYear.Text.Trim() + "' and EmpUnqID='" + tEmpUnqID + "' " +
            " and LeaveTyp ='" + tLeaveTyp + "' and EncashDt ='" + txtEncashDt.DateTime.Date.ToString("yyyy-MM-dd") + "'";


            ds = Utils.Helper.GetData(sql, Utils.Helper.constr);
            bool hasRows = ds.Tables.Cast<DataTable>()
                           .Any(table => table.Rows.Count != 0);

            if (hasRows)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {

                    txtYear.Text = dr["tYear"].ToString();
                    txtLeaveType.Text = dr["LeaveTyp"].ToString();
                    txtEncashDt.EditValue = Convert.ToDateTime(dr["EncashDt"]).ToString("yyyy-MM-dd");
                    txtEncDay.Text = dr["EncashNo"].ToString();
                    mode = "OLD";
                    oldCode = dr["EncashDt"].ToString();

                }
            }
            else
            {
                mode = "NEW";
                oldCode = "";
            }


            SetRights();
        }

        private void txtEncashDt_KeyDown(object sender, KeyEventArgs e)
        {
            if (string.IsNullOrEmpty(ctrlEmp1.txtWrkGrpCode.Text.Trim().ToString())
              || string.IsNullOrEmpty(ctrlEmp1.txtCompCode.Text.Trim().ToString())
              || string.IsNullOrEmpty(ctrlEmp1.txtEmpUnqID.Text.Trim().ToString())
              || string.IsNullOrEmpty(txtYear.Text.Trim())
            
              )
            {
                return;
            }


            if (e.KeyCode == Keys.F1 )
            {
                string tEmpUnqID = ctrlEmp1.txtEmpUnqID.Text.Trim().ToString();
                List<string> obj = new List<string>();

                Help_F1F2.ClsHelp hlp = new Help_F1F2.ClsHelp();
                string sql = "";


                sql = "Select tYear,EmpUnqID,LeaveTyp,EncashDt,EncashNo From MastEmpLeaveEncash Where tYear ='" + txtYear.Text.Trim() + "' and EmpUnqID='" + tEmpUnqID + "'";
                if (e.KeyCode == Keys.F1)
                {

                    obj = (List<string>)hlp.Show(sql, "LeaveTyp", "LeaveTyp", typeof(string), Utils.Helper.constr, "System.Data.SqlClient",
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
                    txtYear.Text = obj.ElementAt(0).ToString();
                    txtLeaveType.Text = obj.ElementAt(2).ToString();
                    txtEncashDt.EditValue = obj.ElementAt(3).ToString();
                    txtEncDay.Text = obj.ElementAt(4).ToString();

                }
            }
           
        }
    }
}
