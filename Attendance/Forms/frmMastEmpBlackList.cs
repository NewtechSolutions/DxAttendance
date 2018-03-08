using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Attendance.Forms
{
    public partial class frmMastEmpBlackList : Form
    {
        public string mode = "NEW";
        public string GRights = "XXXV";
        public string oldCode = "";

        public frmMastEmpBlackList()
        {
            InitializeComponent();
        }

        private void frmMastEmpBlackList_Load(object sender, EventArgs e)
        {

            ResetCtrl();
            GRights = Attendance.Classes.Globals.GetFormRights(this.Name);
            SetRights();
            
        }

        

        private void SetRights()
        {
            if (txtAdharNo.Text.Trim() != "" && mode == "NEW" && GRights.Contains("A"))
            {
                btnAdd.Enabled = true;
                btnUpdate.Enabled = false;
                btnDelete.Enabled = false;
            }
            else if (txtAdharNo.Text.Trim() != "" && mode == "OLD")
            {
                btnAdd.Enabled = false;
                if (GRights.Contains("U"))
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

       
        private void ResetCtrl()
        {
            btnAdd.Enabled = false;
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;


            object s = new object();
            EventArgs e = new EventArgs();
            txtAdharNo.Text = "";
            txtAddDt.EditValue = null;
            txtAddID.Text = "";
            txtUpdDt.EditValue = null;
            txtUpdID.Text = "";
            oldCode = "";
            mode = "NEW";
        }

        private string DataValidate()
        {
            string err = string.Empty;

            if (string.IsNullOrEmpty(txtAdharNo.Text))
            {
                err = err + "Please Enter AdharNo" + Environment.NewLine;
            }

            return err;
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
                        string sql = "Insert into MastEmpBlackList (AdharNo,BlackList,AddDt,AddID) Values ('{0}','{1}',GetDate(),'{2}')";
                        sql = string.Format(sql, txtAdharNo.Text.Trim(),1,Utils.User.GUserID);

                        cmd.CommandText = sql;
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Record saved...", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ResetCtrl();

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Not Implemented...", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        private void btnUpdate_Click(object sender, EventArgs e)
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
                        string sql = "Update MastEmpBlackList Set BlackList = '{1}',UpdDt = GetDate(),UpdID = '{2}' Where AdharNo ='{3}'";
                        sql = string.Format(sql, (chkActive.Checked ? 1 : 0), Utils.User.GUserID,txtAdharNo.Text);

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

        private void txtAdharNo_Validated(object sender, EventArgs e)
        {
            if (txtAdharNo.Text.Trim() == "")
            {
                return;
            }

            DataSet ds = new DataSet();
            string sql = "select * from MastEmpBlackList where AdharNo ='" + txtAdharNo.Text.Trim() + "'";

            ds = Utils.Helper.GetData(sql, Utils.Helper.constr);
            bool hasRows = ds.Tables.Cast<DataTable>()
                           .Any(table => table.Rows.Count != 0);

            if (hasRows)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    txtAdharNo.Text = dr["AdharNo"].ToString();
                    txtAddID.Text = dr["AddID"].ToString();
                    txtAddDt.DateTime = Convert.ToDateTime(dr["AddDt"]);
                    if (dr["UpdDt"] != DBNull.Value)
                    {
                        txtUpdDt.DateTime = Convert.ToDateTime(dr["UpdDt"]);
                        txtUpdID.Text = dr["UpdID"].ToString();
                    }
                    chkActive.Checked = Convert.ToBoolean(dr["BlackList"]);
                    oldCode = dr["AdharNo"].ToString();
                    mode = "OLD";
                }
            }
            else
            {
                mode = "NEW";
                oldCode = "";

                txtAddDt.EditValue = null;
                txtAddID.Text = "";
                txtUpdDt.EditValue = null;
                txtUpdID.Text = "";
                
            }

            SetRights();
        }

        
    }
}
