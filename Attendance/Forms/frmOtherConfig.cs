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
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;

namespace Attendance.Forms
{
    public partial class frmOtherConfig : Form
    {
        public string GRights = "XXXV";
        private string GNetWorkDomain = string.Empty;
        private string GNetWorkUser = string.Empty;
        private DataSet dsAutoTime = new DataSet();
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

            if (txtLateComeSec.Value == 0)
            {
                MessageBox.Show("Please Specify Late Come Seconds...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (txtEarlyComeSec.Value == 0)
            {
                MessageBox.Show("Please Specify Early Come Seconds...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (txtEarlyGoingSec.Value == 0)
            {
                MessageBox.Show("Please Specify Early Going Seconds...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (txtGracePeriodSec.Value == 0)
            {
                MessageBox.Show("Please Specify Grace Period Seconds...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                            sql = "Update MastBCFlg Set SanDayLimit = '" + txtSanDayLimit.Value.ToString() + "'," +
                                " LateComeSec = '" + txtLateComeSec.Value.ToString() + "'," +
                                " EarlyComeSec ='" + txtEarlyComeSec.Value.ToString() + "'," +
                                " EarlyGoingSec ='" + txtEarlyGoingSec.Value.ToString() + "'," +
                                " GracePeriodSec ='" + txtGracePeriodSec.Value.ToString() + "'," +
                                " GraceHalfDayFlg ='" + ((chkGraceHalfDayFlg.Checked) ? "1" : "0") + "'," +
                                " LateHalfDayFlg = '" + ((chkLateHalfDayFlg.Checked) ? "1" : "0") + "'," +
                                " EarlyGoingHalfDayFlg ='" + ((chkEarlyGoingHalfDayFlg.Checked) ? "1" : "0") + "'," +
                                " EarlyGoingHalfDaySec ='" + txtEarlyGoingHalfDaySec.Value.ToString() + "'," +
                                " LateHalfDaySec ='" + txtLateHalfDaySec.Value.ToString() + "'";

                        }else{

                            sql = "Insert into MastBCFlg (SanDayLimit,LateComeSec,EarlyComeSec,EarlyGoingSec,GracePeriodSec" +
                            "  GraceHalfDayFlg, LateHalfDayFlg,EarlyGoingHalfDayFlg,EarlyGoingHalfDaySec,LateHalfDaySec ) values (" +
                                "'" + txtSanDayLimit.Value.ToString() + "'," +
                                "'" + txtLateComeSec.Value.ToString() + "'," +
                                "'" + txtEarlyComeSec.Value.ToString() + "'," +
                                "'" + txtEarlyGoingSec.Value.ToString() + "'," +
                                "'" + txtGracePeriodSec.Value.ToString() + "'," +
                                "'" + ((chkGraceHalfDayFlg.Checked) ? "1" : "0") + "'," +
                                "'" + ((chkLateHalfDayFlg.Checked) ? "1" : "0") + "'," +
                                "'" + ((chkEarlyGoingHalfDayFlg.Checked) ? "1" : "0") + "'," +
                                "'" + txtEarlyGoingHalfDaySec.Value.ToString() + "'," +
                                "'" + txtLateHalfDaySec.Value.ToString() + "')";
                        }
                        cmd.Connection = cn;
                        cmd.CommandText = sql;
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Record Updated...,Please Restart Application/Server to Reflact Changes.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            
            DisplayData();
            LoadGrid();
            SetRights();
        }

        private void LoadGrid()
        {
            dsAutoTime = Utils.Helper.GetData("select * from AutoTimeSet", Utils.Helper.constr);
            bool hasRows = dsAutoTime.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);

            if (hasRows)
            {
                grd_avbl.DataSource = dsAutoTime;
                grd_avbl.DataMember = dsAutoTime.Tables[0].TableName;
                grd_avbl.Refresh();
            }
            else
            {
                grd_avbl.DataSource = null;
                grd_avbl.Refresh();
            }
        }

        private void DoRowDoubleClick(GridView view, Point pt)
        {
            GridHitInfo info = view.CalcHitInfo(pt);
            if (info.InRow || info.InRowCell)
            {
                txtTime.EditValue = gv_avbl.GetRowCellValue(info.RowHandle, "SchTime").ToString();                

            }


        }

        private void DisplayData()
        {
            DataSet ds = Utils.Helper.GetData("select top 1 * from MastBCFlg", Utils.Helper.constr);
            bool hasRows = ds.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);

            if (hasRows)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    txtSanDayLimit.Value = Convert.ToInt32(dr["SanDayLimit"].ToString());
                    txtLateComeSec.Value = Convert.ToInt32(dr["LateComeSec"].ToString());
                    txtEarlyComeSec.Value = Convert.ToInt32(dr["EarlyComeSec"].ToString());
                    txtEarlyGoingSec.Value = Convert.ToInt32(dr["EarlyGoingSec"].ToString());
                    txtGracePeriodSec.Value = Convert.ToInt32(dr["GracePeriodSec"].ToString());

                    chkGraceHalfDayFlg.Checked = Convert.ToBoolean(dr["GraceHalfDayFlg"]);
                    chkLateHalfDayFlg.Checked = Convert.ToBoolean(dr["LateHalfDayFlg"]);
                    chkEarlyGoingHalfDayFlg.Checked = Convert.ToBoolean(dr["EarlyGoingHalfDayFlg"]);
                    txtEarlyGoingHalfDaySec.Value = Convert.ToInt32(dr["EarlyGoingHalfDaySec"].ToString());
                    txtLateHalfDaySec.Value = Convert.ToInt32(dr["LateHalfDaySec"].ToString());

                }
            }
            else
            {
                txtSanDayLimit.Value = 0;
                txtLateComeSec.Value = 0;
                txtEarlyComeSec.Value = 0;
                txtEarlyGoingSec.Value = 0;
                txtGracePeriodSec.Value = 0;
                
                txtLateHalfDaySec.Value = 0;
                txtEarlyGoingHalfDaySec.Value = 0;

                chkEarlyGoingHalfDayFlg.Checked = false;
                chkGraceHalfDayFlg.Checked = false;
                chkLateHalfDayFlg.Checked = false;
            }

            ds = Utils.Helper.GetData("select top 1 * from MastNetWork", Utils.Helper.constr);
            hasRows = ds.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);

            if (hasRows)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    txtEmailID.Text = dr["DefaultMailID"].ToString();
                    txtSMTPIP.Text = dr["SmtpHostIP"].ToString();
                    txtReportServiceURL.Text = dr["ReportServiceURL"].ToString();
                    txtReportSerExeURL.Text = dr["ReportSerExeURL"].ToString();
                    txtServerWorkerIP.Text = dr["ServerWorkerIP"].ToString();

                    GNetWorkDomain = dr["NetWorkDomain"].ToString();
                    GNetWorkUser = dr["NetWorkUser"].ToString();
                    txtAutoProcessWrkGrp.Text = dr["AutoProcessWrkGrp"].ToString();

                    if (dr["AutoProcessTime"] != DBNull.Value)
                    {
                        TimeSpan t = new TimeSpan();
                        TimeSpan.TryParse(dr["AutoProcessTime"].ToString(), out t);
                        txtAutoProccessTime.EditValue = t;
                    }

                }
            }
        }

        private void SetRights()
        {
            if (GRights.Contains("XXXV"))
            {
                btnUpdateSan.Enabled = false;
                btnUpdateNetwork.Enabled = false;
            }
            else if (GRights.Contains("AU"))
            {
                btnUpdateSan.Enabled = true;
                btnUpdateNetwork.Enabled = true;

            }
            else
            {
                btnUpdateSan.Enabled = false;
                btnUpdateNetwork.Enabled = false;
            }
        }

        private void btnUpdateNetwork_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtEmailID.Text.Trim()))
            {
                MessageBox.Show("Please Specify Default Email ID...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrEmpty(txtSMTPIP.Text.Trim()))
            {
                MessageBox.Show("Please Specify SMTP Host IP...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrEmpty(txtReportServiceURL.Text.Trim()))
            {
                string msg = "Please Specify Report Server Url..." + Environment.NewLine +
                    " Ex. (http://172.16.12.47/reportserver/reportservice2010.asmx) ";
                
                MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);                
                return;
            }

            if (string.IsNullOrEmpty(txtReportSerExeURL.Text.Trim()))
            {
                string msg = "Please Specify Report Server Url..." + Environment.NewLine +
                    " Ex. (http://172.16.12.47/reportserver/reportexecution2005.asmx) ";
                
                MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrEmpty(txtServerWorkerIP.Text.Trim()))
            {
                string msg = "Please Specify IP Address of hosting scheduler server host...";
                MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            if (string.IsNullOrEmpty(txtAutoProcessWrkGrp.Text.Trim()))
            {
                string msg = "Please Specify WrkGrps for Auto Process...";
                MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (txtAutoProccessTime.Time == DateTime.MinValue || txtAutoProccessTime.Time == null)
            {
                string msg = "Please Specify Time of Start Auto Process...";
                MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            using (SqlConnection cn = new SqlConnection(Utils.Helper.constr))
            {
                try
                {
                    cn.Open();

                    //int cnt = Convert.ToInt32(Utils.Helper.GetDescription("Select Count(*) from MastNetwork", Utils.Helper.constr));

                    using (SqlCommand cmd = new SqlCommand())
                    {

                       
                        string sql = "";
                        
                            sql = "Update MastNetwork Set DefaultMailID = '" + txtEmailID.Text.ToString() + "'," +
                            " SmtpHostIP ='" + txtSMTPIP.Text.ToString() + "', " +
                            " ReportServiceURL='" + txtReportServiceURL.Text.ToString() + "', " +
                            " ReportSerExeURL='" + txtReportSerExeURL.Text.ToString() + "', " +
                            " ServerWorkerIP='" + txtServerWorkerIP.Text.Trim().ToString() + "', " +
                            " UpdDt = GetDate() , UpdID ='" + Utils.User.GUserID + "', " +
                            " AutoProcessWrkGrp ='" + txtAutoProcessWrkGrp.Text.Trim().ToString() + "'," +
                            " AutoProcessTime=" + ((txtAutoProccessTime.Time.TimeOfDay.Hours == 0) ? " NULL " : "'" + txtAutoProccessTime.Time.ToString("HH:mm") + "'") +
                            " where NetWorkDomain ='" + GNetWorkDomain + "' And NetworkUser ='"  + GNetWorkUser +  "'";
                        
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

        private void btnTimeAdd_Click(object sender, EventArgs e)
        {
            if (txtTime.Time == DateTime.MinValue || txtAutoProccessTime.Time == null)
            {
                string msg = "Please Specify Time...";
                MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (SqlConnection cn = new SqlConnection(Utils.Helper.constr))
            {
                try
                {
                    cn.Open();

                    using (SqlCommand cmd = new SqlCommand())
                    {

                        string sql = "";
                        sql = "Insert into AutoTimeSet (SchTime,AddDt,AddID) Values ('" + txtTime.Time.ToString("HH:mm") + "',GetDate(),'" + Utils.User.GUserID + "');";
                        
                        cmd.Connection = cn;
                        cmd.CommandText = sql;
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Record Updated...", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadGrid();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnTimeDel_Click(object sender, EventArgs e)
        {
            if (txtTime.Time == DateTime.MinValue || txtAutoProccessTime.Time == null)
            {
                string msg = "Please Specify Time...";
                MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (SqlConnection cn = new SqlConnection(Utils.Helper.constr))
            {
                try
                {
                    cn.Open();

                    using (SqlCommand cmd = new SqlCommand())
                    {

                        string sql = "";
                        sql = "Delete  From AutoTimeSet Where SchTime = '" + txtTime.Time.ToString("HH:mm") + "'";

                        cmd.Connection = cn;
                        cmd.CommandText = sql;
                        int t = (int)cmd.ExecuteNonQuery();

                        if (t > 0)
                        {
                            MessageBox.Show("Record Updated...", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadGrid();
                        }
                        
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void gv_avbl_DoubleClick(object sender, EventArgs e)
        {
            GridView view = (GridView)sender;
            Point pt = view.GridControl.PointToClient(Control.MousePosition);
            DoRowDoubleClick(view, pt);
        }

        

    }
}
