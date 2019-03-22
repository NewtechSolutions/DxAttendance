using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using Attendance.Classes;

namespace Attendance.Forms
{
    public partial class frmMastManPowerGroupDt : Form
    {
        public string mode = "NEW";
        public string GRights = "XXXV";
        public string oldCode = "";
       
        public frmMastManPowerGroupDt()
        {
            InitializeComponent();
        }

        private void frmMastManPowerGroupDt_Load(object sender, EventArgs e)
        {
            ResetCtrl();
            GRights = Attendance.Classes.Globals.GetFormRights(this.Name);
            SetRights();
            
        }

        private string DataValidate()
        {
            string err = string.Empty;

            

            if (string.IsNullOrEmpty(txtWrkGrpCode.Text))
            {
                err = err + "Please Enter WrkGrpCode..." + Environment.NewLine;
            }

            if (string.IsNullOrEmpty(txtWrkGrpDesc.Text))
            {
                err = err + "Please Enter WrkGrp Description..." + Environment.NewLine;
            }

            if (string.IsNullOrEmpty(txtUnitCode.Text))
            {
                err = err + "Please Enter Unit Code..." + Environment.NewLine;
            }

            if (string.IsNullOrEmpty(txtUnitDesc.Text))
            {
                err = err + "Please Enter Unit Description..." + Environment.NewLine;
            }


            if (string.IsNullOrEmpty(txtGroupCode.Text))
            {
                err = err + "Please Enter Group Code..." + Environment.NewLine;
            }

            if (string.IsNullOrEmpty(txtGroupDesc.Text))
            {
                err = err + "Please Enter Group Desc..." + Environment.NewLine;
            }
            
            if (string.IsNullOrEmpty(txtDeptCode.Text))
            {
                err = err + "Please Enter Department Code..." + Environment.NewLine;
            }

            if (string.IsNullOrEmpty(txtDeptDesc.Text))
            {
                err = err + "Please Enter Department Desc..." + Environment.NewLine;
            }

            if (string.IsNullOrEmpty(txtStatCode.Text))
            {
                err = err + "Please Enter Station Code..." + Environment.NewLine;
            }

            if (string.IsNullOrEmpty(txtStatDesc.Text))
            {
                err = err + "Please Enter Station Desc..." + Environment.NewLine;
            }

            if (string.IsNullOrEmpty(txtSanManPower.Text))
            {
                err = err + "Please Enter San. Manpower..." + Environment.NewLine;
            }

            if (string.IsNullOrEmpty(txtExtraManPower.Text))
            {
                err = err + "Please Enter Extra Manpower..." + Environment.NewLine;
            }



            return err;
        }
        
        private void ResetCtrl()
        {
            btnAdd.Enabled = false;
            btnDelete.Enabled = false;

            
            txtWrkGrpCode.Text = "";
            txtWrkGrpDesc.Text = "";
            txtUnitCode.Text = "";
            txtUnitDesc.Text = "";
            txtDeptCode.Text = "";
            txtDeptDesc.Text = "";
            txtStatCode.Text = "";
            txtStatDesc.Text = "";
            txtExtraManPower.Text = "";
            txtSanManPower.Text = "";
            txtRemarks.Text = "";
            txtGroupCode.Text = "";
            txtGroupDesc.Text = "";

            oldCode = "";
            mode = "NEW";
            

            txtWrkGrpCode.Enabled = true;
            txtGroupCode.Enabled = true;
            txtUnitCode.Enabled = true;
            txtDeptCode.Enabled = true;
            txtStatCode.Enabled = true;

            

        }

        private void SetRights()
        {
            btnAdd.Enabled = false;
            btnDelete.Enabled = false;
            btnUpdate.Enabled = false;

            if ( mode == "NEW" && GRights.Contains("A") )
            {
                btnAdd.Enabled = true;
               
                btnDelete.Enabled = false;
            }
            else if ( mode == "OLD" )
            {
                btnAdd.Enabled = false;
                btnUpdate.Enabled = false;
              

                if (GRights.Contains("U"))
                {
                    btnUpdate.Enabled = true;
                    
                }

                if (GRights.Contains("D"))
                {
                    btnDelete.Enabled = true;
                   
                }
                    
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

            if (string.IsNullOrEmpty(err))
            {
                using (SqlConnection cn = new SqlConnection(Utils.Helper.constr))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        try
                        {
                            int SanMan = 0;
                            int ExtraMan = 0;

                            int.TryParse(txtSanManPower.Text.Trim(), out SanMan);
                            int.TryParse(txtExtraManPower.Text.Trim(), out ExtraMan);

                            cn.Open();
                            cmd.Connection = cn;
                            string sql = "Insert into PBO_GroupDT (GroupCode,WrkGrp,UnitCode,DeptCode,StatCode," +
                                " SanManPower,ExtraManPower,Remarks,AddDt,AddId ) Values (" +
                                "'{0}','{1}','{2}','{3}','{4}'," +
                                " '{5}','{6}','{7}',GetDate(),'{8}')";

                            sql = string.Format(sql,
                                
                                txtGroupCode.Text.Trim(),
                                txtWrkGrpCode.Text.Trim(),
                                txtUnitCode.Text.Trim(),
                                txtDeptCode.Text.Trim(),
                                txtStatCode.Text.Trim(),
                                SanMan.ToString(),
                                ExtraMan.ToString(),
                                txtRemarks.Text.ToString(),
                                Utils.User.GUserID
                                );

                            cmd.CommandText = sql;

                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Record Inserted...", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ResetCtrl();

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
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

            if (string.IsNullOrEmpty(err))
            {
               
                DialogResult qs = MessageBox.Show("Are You Sure to Delete this Record...?","Question",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
                if(qs == DialogResult.No){
                    return;
                }

                using (SqlConnection cn = new SqlConnection(Utils.Helper.constr))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        try
                        {
                            int SanMan = 0;
                            int ExtraMan = 0;

                            int.TryParse(txtSanManPower.Text.Trim(), out SanMan);
                            int.TryParse(txtExtraManPower.Text.Trim(), out ExtraMan);

                            cn.Open();
                            cmd.Connection = cn;
                            string sql = "Delete From PBO_GroupDT  where " +
                                " GroupCode ='{0}' and WrkGrp = '{1}' and UnitCode = '{2}' " +
                                " and DeptCode = '{3}' and StatCode = '{4}' ";

                            sql = string.Format(sql,
                               
                                txtGroupCode.Text.Trim(),
                                txtWrkGrpCode.Text.Trim(),
                                txtUnitCode.Text.Trim(),
                                txtDeptCode.Text.Trim(),
                                txtStatCode.Text.Trim()
                                );

                            cmd.CommandText = sql;

                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Record Deleted...", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ResetCtrl();

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                
            }

           // MessageBox.Show("Not Implemented...", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        

        private void txtWrkGrpCode_KeyDown(object sender, KeyEventArgs e)
        {
            

            if (e.KeyCode == Keys.F1 || e.KeyCode == Keys.F2)
            {
                List<string> obj = new List<string>();

                Help_F1F2.ClsHelp hlp = new Help_F1F2.ClsHelp();
                string sql = "";


                sql = "Select WrkGrp,WrkGrpDesc From MastWorkGrp Where 1 = 1 " ;
                if (e.KeyCode == Keys.F1)
                {

                    obj = (List<string>)hlp.Show(sql, "WrkGrp", "WrkGrp", typeof(string), Utils.Helper.constr, "System.Data.SqlClient",
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

                    txtWrkGrpCode.Text = obj.ElementAt(0).ToString();
                    txtWrkGrpDesc.Text = obj.ElementAt(1).ToString();


                }
            }
        }

        

        private void txtUnitCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (txtWrkGrpCode.Text.Trim() == "" || txtWrkGrpDesc.Text.Trim() == "")
                return;

            if (e.KeyCode == Keys.F1 || e.KeyCode == Keys.F2)
            {
                List<string> obj = new List<string>();

                Help_F1F2.ClsHelp hlp = new Help_F1F2.ClsHelp();
                string sql = "";


                sql = "Select UnitCode,UnitName From MastUnit Where CompCode ='01' and WrkGrp = '" + txtWrkGrpCode.Text.Trim() + "' ";
                if (e.KeyCode == Keys.F1)
                {

                    obj = (List<string>)hlp.Show(sql, "UnitCode", "UnitCode", typeof(string), Utils.Helper.constr, "System.Data.SqlClient",
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

        private void txtUnitCode_Validated(object sender, EventArgs e)
        {
            if ( txtWrkGrpCode.Text.Trim() == "" || txtWrkGrpDesc.Text.Trim() == "")
            {

                return;
            }

            txtUnitCode.Text = txtUnitCode.Text.Trim().ToString().PadLeft(3, '0');

            DataSet ds = new DataSet();
            string sql = "select * From MastUnit where CompCode ='01' " +
                    " and WrkGrp='" + txtWrkGrpCode.Text.Trim() + "' and UnitCode ='" + txtUnitCode.Text.Trim() + "'";

            ds = Utils.Helper.GetData(sql, Utils.Helper.constr);
            bool hasRows = ds.Tables.Cast<DataTable>()
                           .Any(table => table.Rows.Count != 0);

            if (hasRows)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    
                    txtWrkGrpCode.Text = dr["WrkGrp"].ToString();
                    txtUnitCode.Text = dr["UnitCode"].ToString();
                    txtUnitDesc.Text = dr["UnitName"].ToString();
                    
                    txtWrkGrpCode_Validated(sender, e);

                }
            }
        }

        private void txtDeptCode_KeyDown(object sender, KeyEventArgs e)
        {
            if ( txtWrkGrpCode.Text.Trim() == "" 
                || txtWrkGrpDesc.Text.Trim() == ""
                || txtUnitCode.Text.Trim() == "")
                return;

            if (e.KeyCode == Keys.F1 || e.KeyCode == Keys.F2)
            {
                List<string> obj = new List<string>();

                Help_F1F2.ClsHelp hlp = new Help_F1F2.ClsHelp();
                string sql = "";

                sql = "Select DeptCode,DeptDesc From MastDept Where CompCode ='01' " +
                    " and WrkGrp = '" + txtWrkGrpCode.Text.Trim() + "' and UnitCode ='" + txtUnitCode.Text.Trim() + "'";

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
            if ( txtWrkGrpCode.Text.Trim() == ""
                || txtWrkGrpDesc.Text.Trim() == ""
                || txtUnitCode.Text.Trim() == "")
            {

                return;
            }

            txtDeptCode.Text = txtDeptCode.Text.Trim().ToString().PadLeft(3, '0');

            DataSet ds = new DataSet();
            string sql = "select * From MastDept where CompCode ='01' " +
                    " and WrkGrp='" + txtWrkGrpCode.Text.Trim() + "' " +
                    " and UnitCode ='" + txtUnitCode.Text.Trim() + "' " +
                    " and DeptCode ='" + txtDeptCode.Text.Trim() + "' ";

            ds = Utils.Helper.GetData(sql, Utils.Helper.constr);
            bool hasRows = ds.Tables.Cast<DataTable>()
                           .Any(table => table.Rows.Count != 0);

            if (hasRows)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    
                    txtWrkGrpCode.Text = dr["WrkGrp"].ToString();
                    txtUnitCode.Text = dr["UnitCode"].ToString();
                    txtDeptCode.Text = dr["DeptCode"].ToString();
                    txtDeptDesc.Text = dr["DeptDesc"].ToString();
                   
                    txtWrkGrpCode_Validated(sender, e);
                    txtUnitCode_Validated(sender, e);
                }
            }

        }

        private void txtGroupCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (txtWrkGrpCode.Text.Trim() == "" || txtWrkGrpDesc.Text.Trim() == ""
               )
                return;

            if (e.KeyCode == Keys.F1 || e.KeyCode == Keys.F2)
            {
                List<string> obj = new List<string>();

                Help_F1F2.ClsHelp hlp = new Help_F1F2.ClsHelp();
                string sql = "";

                sql = "Select GroupCode,GroupDesc From PBO_GroupMaster Where  " +
                    " WrkGrp = '" + txtWrkGrpCode.Text.Trim() + "' ";

                if (e.KeyCode == Keys.F1)
                {

                    obj = (List<string>)hlp.Show(sql, "GroupCode", "GroupCode", typeof(string), Utils.Helper.constr, "System.Data.SqlClient",
                   100, 300, 400, 600, 100, 100);
                }
                else
                {

                    obj = (List<string>)hlp.Show(sql, "GroupDesc", "GroupDesc", typeof(string), Utils.Helper.constr, "System.Data.SqlClient",
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

                    txtGroupCode.Text = obj.ElementAt(0).ToString();
                    txtGroupDesc.Text = obj.ElementAt(1).ToString();

                }
            }
        }

        private void txtGroupCode_Validated(object sender, EventArgs e)
        {
            if (txtWrkGrpCode.Text.Trim() == ""
                || txtWrkGrpDesc.Text.Trim() == ""
                || txtGroupCode.Text.Trim() == "")
            {

                return;
            }

            txtGroupCode.Text = txtGroupCode.Text.Trim().ToString().PadLeft(3, '0');

            DataSet ds = new DataSet();
            string sql = "select * From PBO_GroupMaster where  " +
                    " WrkGrp='" + txtWrkGrpCode.Text.Trim() + "' " +
                    " and GroupCode ='" + txtGroupCode.Text.Trim() + "' ";

            ds = Utils.Helper.GetData(sql, Utils.Helper.constr);
            bool hasRows = ds.Tables.Cast<DataTable>()
                           .Any(table => table.Rows.Count != 0);

            if (hasRows)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {

                    txtWrkGrpCode.Text = dr["WrkGrp"].ToString();
                    
                    txtGroupCode.Text = dr["GroupCode"].ToString();
                    txtGroupDesc.Text = dr["GroupDesc"].ToString();

                    txtWrkGrpCode_Validated(sender, e);
                    
                }
            }

        }

        private void txtStatCode_KeyDown(object sender, KeyEventArgs e)
        {
            if ( txtWrkGrpCode.Text.Trim() == "" || txtGroupCode.Text.Trim() == ""
                || txtUnitCode.Text.Trim() == "" || txtDeptCode.Text.Trim() == "")
            {
                mode = "NEW";
                SetRights();
                return;
            }
                

            if (e.KeyCode == Keys.F1 || e.KeyCode == Keys.F2)
            {
                List<string> obj = new List<string>();

                Help_F1F2.ClsHelp hlp = new Help_F1F2.ClsHelp();
                string sql = "";

                sql = "Select StatCode,StatDesc From MastStat Where CompCode ='01' " +
                   " and WrkGrp = '" + txtWrkGrpCode.Text.Trim() + "' and UnitCode ='" + txtUnitCode.Text.Trim() + "' " +
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
            if ( txtWrkGrpCode.Text.Trim() == "" || txtWrkGrpDesc.Text.Trim() == ""
                || txtUnitCode.Text.Trim() == "" || txtUnitDesc.Text.Trim() == ""
                || txtDeptCode.Text.Trim() == "" || txtDeptDesc.Text.Trim() == ""
                || txtStatCode.Text.Trim() == "" || txtStatDesc.Text.Trim() == "" 
                || txtGroupCode.Text.Trim() == ""
                )
            {
                mode = "NEW";
                SetRights();
                return;
            }

            txtStatCode.Text = txtStatCode.Text.Trim().ToString().PadLeft(3, '0');

            DataSet ds = new DataSet();
            string sql = "select * From PBO_GroupDT where  " +
                    " WrkGrp='" + txtWrkGrpCode.Text.Trim() + "' " +
                    " and GroupCode = '" + txtGroupCode.Text.Trim() + "' " +
                    " and UnitCode ='" + txtUnitCode.Text.Trim() + "' " +
                    " and DeptCode ='" + txtDeptCode.Text.Trim() + "' " +
                    " and StatCode ='" + txtStatCode.Text.Trim() + "' ";

            ds = Utils.Helper.GetData(sql, Utils.Helper.constr);
            bool hasRows = ds.Tables.Cast<DataTable>()
                           .Any(table => table.Rows.Count != 0);

            if (hasRows)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    
                    txtWrkGrpCode.Text = dr["WrkGrp"].ToString();
                    txtUnitCode.Text = dr["UnitCode"].ToString();
                    txtDeptCode.Text = dr["DeptCode"].ToString();
                    txtStatCode.Text = dr["StatCode"].ToString();
                    txtGroupCode.Text = dr["GroupCode"].ToString();
                    txtSanManPower.Text = dr["SanManPower"].ToString();
                    txtExtraManPower.Text = dr["ExtraManPower"].ToString();
                    txtRemarks.Text = dr["Remarks"].ToString();
                    //txtWrkGrpCode_Validated(sender, e);
                    //txtGroupCode_Validated(sender, e);
                    //txtUnitCode_Validated(sender, e);
                    //txtDeptCode_Validated(sender, e);
                    //txtStatCode_Validated(sender, e);

                    mode = "OLD";
                    SetRights();                   

                }
            }
            else
            {
                ds = new DataSet();
                sql = "select * From MastStat where CompCode = '01'   " +
                        " and WrkGrp='" + txtWrkGrpCode.Text.Trim() + "' " +
                        " and UnitCode ='" + txtUnitCode.Text.Trim() + "' " +
                        " and DeptCode ='" + txtDeptCode.Text.Trim() + "' " +
                        " and StatCode ='" + txtStatCode.Text.Trim() + "' ";

                ds = Utils.Helper.GetData(sql, Utils.Helper.constr);
                hasRows = ds.Tables.Cast<DataTable>()
                               .Any(table => table.Rows.Count != 0);
                if (hasRows)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        txtStatCode.Text = dr["StatCode"].ToString();
                        txtStatDesc.Text = dr["StatDesc"].ToString();
                        mode = "NEW";
                        SetRights();
                    }
                }
                else
                {
                    txtStatCode.Text = "";
                    txtStatDesc.Text = "";
                    mode = "NEW";
                    SetRights();
                }
            }
            

            
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (mode != "OLD")
            {
                return;
            }
            
            string err = DataValidate();

            //string tID = Utils.Helper.GetDescription("Select isnull(Max(SrNo),0) + 1 from AutomailSubScription where 1 = 1", Utils.Helper.constr);


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
                        int SanMan = 0;
                        int ExtraMan = 0;

                        int.TryParse(txtSanManPower.Text.Trim(),out SanMan);
                        int.TryParse(txtExtraManPower.Text.Trim(),out ExtraMan);

                        cn.Open();
                        cmd.Connection = cn;
                        string sql = "Update PBO_GroupDT Set SanManPower ='{0}' , ExtraManPower ='{1}' " +
                            ",Remarks = '{2}' , UpdDt = GetDate(), UpdID = '{3}'  where " +
                            " GroupCode ='{4}' and WrkGrp = '{5}' and UnitCode = '{6}' " + 
                            " and DeptCode = '{7}' and StatCode = '{8}' ";
                        
                        sql = string.Format(sql, 
                            SanMan.ToString(),
                            ExtraMan.ToString(),
                            txtRemarks.Text.Trim(),
                            Utils.User.GUserID,
                            txtGroupCode.Text.Trim(),
                            txtWrkGrpCode.Text.Trim(),
                            txtUnitCode.Text.Trim(),
                            txtDeptCode.Text.Trim(),
                            txtStatCode.Text.Trim()
                            );
                        
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
        }

        private void frmMastManPowerGroupDt_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyData == Keys.Enter))
            {
                SelectNextControl(ActiveControl, true, true, true, true);
            }
        }

        private void txtSanManPower_Validated(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtSanManPower.Text.Trim()))
                txtSanManPower.Text = "0";

        }

        private void txtExtraManPower_Validated(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtExtraManPower.Text.Trim()))
                txtExtraManPower.Text = "0";
        }

        private void txtWrkGrpCode_Validated(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
            string sql = "select * From MastWorkGrp where CompCode ='01'  and WrkGrp='" + txtWrkGrpCode.Text.Trim() + "'";

            ds = Utils.Helper.GetData(sql, Utils.Helper.constr);
            bool hasRows = ds.Tables.Cast<DataTable>()
                           .Any(table => table.Rows.Count != 0);

            if (hasRows)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {

                    txtWrkGrpCode.Text = dr["WrkGrp"].ToString();
                    txtWrkGrpDesc.Text = dr["WrkGrpDesc"].ToString();

                }
            }
        }
    }
}
