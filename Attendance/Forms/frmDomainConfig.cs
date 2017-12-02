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
    public partial class frmDomainConfig : Form
    {
        public string GRights = "XXXV";
        
        public frmDomainConfig()
        {
            InitializeComponent();
        }



        private void frmDomainConfig_Load(object sender, EventArgs e)
        {
            string sql = "Select top 1 * from MastNetwork Where 1 = 1" ;
                DataSet ds = Utils.Helper.GetData(sql, Utils.Helper.constr);

                bool hasrows = ds.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);

                if (hasrows)
                {
                    
                    txtDomainName.EditValue = ds.Tables[0].Rows[0]["NetWorkDomain"].ToString();
                    txtUserID.EditValue = ds.Tables[0].Rows[0]["NetWorkUser"].ToString();
                    txtPassword.EditValue = ds.Tables[0].Rows[0]["NetWorkPass"].ToString();
                }
                else
                {
                   
                    txtDomainName.EditValue = string.Empty;
                    txtUserID.EditValue = string.Empty;
                    txtPassword.EditValue = string.Empty;
                }

                GRights = Attendance.Classes.Globals.GetFormRights(this.Name);
                SetRights();
            
        }

        private void SetRights()
        {
            if (GRights.Contains("A") || GRights.Contains("U"))
            {
                btnSubmit.Enabled = true;
            }
            

            if (GRights.Contains("XXXV"))
            {
                btnSubmit.Enabled = false;            
            }
        }


        private void btnSubmit_Click(object sender, EventArgs e)
        {
            
            using (SqlConnection cn = new SqlConnection(Utils.Helper.constr))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        cn.Open();
                        cmd.Connection = cn;
                        cmd.CommandText = "Delete From MastNetwork Where 1=1";
                        cmd.ExecuteNonQuery();
                        
                        cmd.CommandText = "Insert into MastNetwork (NetworkDomain,NetWorkUser,NetWorkPass,AddDt,AddId) values (" +
                            " '" + txtDomainName.Text.Trim() + "','" + txtUserID.Text.Trim() + "','" + txtPassword.Text.Trim().ToString() + "',GetDate()," +
                            " '" + Utils.User.GUserID + "')";

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Network configuration saved...", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        

                    }catch(Exception ex){
                        MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

        }
    }
}
