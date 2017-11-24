using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using Attendance.Classes;

namespace Attendance.Forms
{
    public partial class frmOtherConfig : Form
    {
        public string GRights = "XXXV";
        
        public frmOtherConfig()
        {
            InitializeComponent();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (txtSanDayLimit.Value == 0)
            {
                MessageBox.Show("Please Specify No. of Days...", "Error", MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }
            using (SqlConnection cn = new SqlConnection(Utils.Helper.constr))
            {
                try
                {
                    cn.Open();
                    int cnt  = Convert.ToInt32(Utils.Helper.GetDescription("Select Count(*) from MastBCFlg",Utils.Helper.constr));


                    using (SqlCommand cmd = new SqlCommand())
                    {
                        
                        string sql = "";
                        if(cnt > 0)
                        {
                            sql = "Update MastBCFlg Set SanDayLimit = '" + txtSanDayLimit.Value.ToString() + "'";
                        }else{

                            sql = "Insert into MastBCFlg (SanDayLimit) values ('" + txtSanDayLimit.Value.ToString() + "')";
                        }
                        cmd.Connection = cn;
                        cmd.CommandText = sql;
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Record Updated...", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }

        private void frmOtherConfig_Load(object sender, EventArgs e)
        {
            GRights = Attendance.Classes.Globals.GetFormRights(this.Name);

            //txtSanDayLimit.Value = Convert.ToInt32(Utils.Helper.GetDescription("Select SanDayLimit From MastBCFlg", Utils.Helper.constr));
            string cntdays = Utils.Helper.GetDescription("Select SanDayLimit From MastBCFlg", Utils.Helper.constr);
            if(!string.IsNullOrEmpty(cntdays))
            {
                int tdays = 0;
                int.TryParse(cntdays, out tdays);
                txtSanDayLimit.Value = tdays;
            }
            if (GRights.Contains("XXXV"))
            {
                btnUpdate.Enabled = false;
               
            }
            else if (GRights.Contains("AU"))
            {
                btnUpdate.Enabled = true;
               
            }
            else
            {
                btnUpdate.Enabled = false;
            }


        }
    }
}
