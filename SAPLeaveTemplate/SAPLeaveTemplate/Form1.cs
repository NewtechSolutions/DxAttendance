using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Attendance.Classes;
using Utils;
using System.Data.SqlClient;
using SAPLeaveTemplate.RS2005;
using SAPLeaveTemplate.RE2005;
//using Microsoft.ReportingServices.Interfaces;
using ParameterValue = SAPLeaveTemplate.RE2005.ParameterValue;
using Warning = SAPLeaveTemplate.RE2005.Warning;
using System.IO;
using System.Net;
using DevExpress.XtraEditors.Controls;
using System.Runtime.InteropServices;
using Spire.Xls;
using Spire.License;

namespace SAPLeaveTemplate
{
    public partial class Form1 : DevExpress.XtraEditors.XtraForm
    {
        private Utils.DbCon dbcon ;
        private string reportpath;
        private string reportpass;
        private string fileprefix;
        private DataSet DS;
        private DataSet LastUploadDS;

        private string selLeave;

        public Form1()
        {
            InitializeComponent();
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string err = DataValidate();
            if (!string.IsNullOrEmpty(err))
            {
                MessageBox.Show(err, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


           

            try
            {
                this.Cursor = Cursors.WaitCursor;
                lockcontrols();

                NetworkCredential clientCredentials = new NetworkCredential(Globals.G_NetworkUser, Globals.G_NetworkPass, Globals.G_NetworkDomain);
                ReportingService2010 rs = new ReportingService2010();
                rs.Credentials = clientCredentials;
                rs.Url = Globals.G_ReportServiceURL;

                ReportExecutionService rsExec = new ReportExecutionService();
                rsExec.Credentials = clientCredentials;
                rsExec.Url = Globals.G_ReportSerExeUrl;
                string historyID = null;
               
                string deviceInfo = null;
                string extension;
                string encoding;
                string mimeType;
                Warning[] warnings = null;
                string[] streamIDs = null;
                string format = "EXCEL";
                Byte[] results;
                
                
                rsExec.LoadReport(reportpath, historyID);
                ParameterValue[] executionParams = new ParameterValue[6];
                executionParams[0] = new ParameterValue();
                executionParams[0].Name = "pFromDt";
                executionParams[0].Value = txtFromDt.Value.ToString("yyyy-MM-dd"); 

                executionParams[1] = new ParameterValue();
                executionParams[1].Name = "pToDate";
                executionParams[1].Value = txtToDate.Value.ToString("yyyy-MM-dd");

                executionParams[2] = new ParameterValue();
                executionParams[2].Name = "pExculdePrvPeriod";
                executionParams[2].Value = chkExclude.Checked.ToString();

                executionParams[3] = new ParameterValue();
                executionParams[3].Name = "pLeaveType";
                executionParams[3].Value = selLeave.Trim();

                executionParams[4] = new ParameterValue();
                executionParams[4].Name = "pCurYearMt";
                executionParams[4].Value = txtCurYearMt.DateTime.Date.ToString("yyyyMM").ToString();


                rsExec.SetExecutionParameters(executionParams, "en-us");
                results = rsExec.Render(format, deviceInfo, out extension, out mimeType, out encoding, out warnings, out streamIDs);

                

                string tmpfile = Path.Combine(Path.GetTempPath(), "Foo.xls");

                if (System.IO.File.Exists(tmpfile))
                    System.IO.File.Delete(tmpfile);

                System.IO.File.WriteAllBytes(tmpfile, results);
                results = null;
                
                Spire.Xls.Workbook wrkbook = new Workbook();
                wrkbook.LoadFromFile(tmpfile);
                Spire.Xls.Worksheet wrksheet = wrkbook.Worksheets[0];
                wrksheet.DeleteRow(1,3);

                if(chkPassOpt.Checked)
                    wrkbook.Protect(reportpass);
               
                
                wrkbook.SaveToFile(GetFileName(), ExcelVersion.Version97to2003);
	           
                if (System.IO.File.Exists(tmpfile))
                    System.IO.File.Delete(tmpfile);
                
                this.Cursor = Cursors.Default;

                MessageBox.Show("File Generated @ " + GetFileName(), "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                if (chkFinalUpload.Checked)
                {
                    savelastupload();
                    
                }

                unlockcontrols();
            }
            catch (Exception ex)
            {
                /***
                excelworkBook = null;
                excel = null;
                excelsheet = null;
                tmprange = null;
                ***/
                unlockcontrols();
                this.Cursor = Cursors.Default;
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
        }


        private void savelastupload()
        {
            using (SqlConnection cn = new SqlConnection(Utils.Helper.constr))
            {
                try
                {
                    cn.Open();
                    int curmth = Convert.ToInt32(txtCurYearMt.DateTime.Date.ToString("yyyyMM"));
                    string sql = "Select * from Mast_SAPTemplate_Tran Where YearMt = '" + curmth.ToString() + "'";

                    DataSet ds = Utils.Helper.GetData(sql, Utils.Helper.constr);
                    bool hasrows = ds.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);
                    if (!hasrows)
                    {
                        sql = "Insert into Mast_SAPTemplate_Tran (YearMt,FinalUpload,UploadDt,FromDt,ToDt) Values " +
                            " ('" + txtCurYearMt.DateTime.Date.ToString("yyyyMM") + "'," +
                            " 1,GetDate(),'" + txtFromDt.Value.Date.ToString("yyyy-MM-dd") + "'," +
                            "'" + txtToDate.Value.Date.ToString("yyyy-MM-dd") + "')";
                    }
                    else
                    {
                        sql = "Update Mast_SAPTemplate_Tran set UploadDt = GetDate(), ToDt ='" + txtToDate.Value.Date.ToString("yyyy-MM-dd") + "' Where YearMt = '" + txtCurYearMt.DateTime.Date.ToString("yyyyMM") + "'";
                    }

                    using (SqlCommand cmd = new SqlCommand(sql, cn))
                    {
                        cmd.ExecuteNonQuery();
                    }


                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error while save last upload " + Environment.NewLine + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private string GetFileName()
        {
            string dskpath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            
            string filepostfix = "_TOOL_" + txtFromDt.Value.Date.ToString("yyyyMMdd") + "_TO_" + txtToDate.Value.Date.ToString("yyyyMMdd") + ".xls";
            string filename = fileprefix + filepostfix;
            string filefullpath = Path.Combine(dskpath, filename);
            return filefullpath;
        }
        
        private void lockcontrols()
        {
            txtFromDt.Enabled = false;
            txtToDate.Enabled = false;
            cmbTemplate.Enabled = false;
            chkPassOpt.Enabled = false;
            chkFinalUpload.Enabled = false;
            
            chkedLeave.Enabled = false;
            button1.Enabled = false;
        }

        private void unlockcontrols()
        {
            txtFromDt.Enabled = true;
            txtToDate.Enabled = true;

            chkPassOpt.Enabled = true;
            chkFinalUpload.Enabled = true;

            chkedLeave.Enabled = true;
            
            cmbTemplate.Enabled = true;
            button1.Enabled = true;
        }

        private string DataValidate()
        {
            string err = string.Empty;

            
            if (string.IsNullOrEmpty(dbcon.DataSource))
                err += "Database Connection not initilized.." + Environment.NewLine;

            Utils.Helper.constr = dbcon.ToString();


            if (txtCurYearMt.EditValue == null)
            {
                err += "Please Select Year Month" + Environment.NewLine;
            }

            
            if(this.txtFromDt.Value.Date > this.txtToDate.Value.Date)
                err += "Invalid Date Range.." + Environment.NewLine;

            if (string.IsNullOrEmpty(cmbTemplate.Text.Trim().ToString()))
            {
                err += "Please Select Template.." + Environment.NewLine;
                reportpath = string.Empty;
            }

           
            

            if (!string.IsNullOrEmpty(err))
                return err;


            

            
            selLeave = string.Empty;
            //List<object> sel = chkedLeave.Properties.Items

            
            foreach (CheckedListBoxItem item in chkedLeave.Properties.GetItems()) 
            {
                if (item.CheckState == System.Windows.Forms.CheckState.Checked)
                {
                    if (string.IsNullOrEmpty(selLeave))
                        selLeave += item.Value.ToString().Trim();
                    else
                        selLeave += ";" + item.Value.ToString().Trim();
                }
            }

            if (string.IsNullOrEmpty(selLeave))
            {
                err += "Please Select Leave Types.." + Environment.NewLine;
                reportpath = string.Empty;
            }

            if (string.IsNullOrEmpty(Utils.Helper.constr))
                err += "Connection string error.." + Environment.NewLine;

            if (!string.IsNullOrEmpty(err))
                return err;

            
            string t1 = cmbTemplate.Text.Trim().ToString().ToUpper();
            fileprefix = t1;

            string sql = "select Config_VAL from MAST_OtherConfig where config_key ='" + t1 + "'";
            reportpath = Utils.Helper.GetDescription(sql, Utils.Helper.constr);

            sql = "select Config_VAL from MAST_OtherConfig where config_key ='SAP_Template_Pass'";
            reportpass = Utils.Helper.GetDescription(sql, Utils.Helper.constr);
            
            if(string.IsNullOrEmpty(reportpath))
                err += "Report path Error.." + Environment.NewLine;            

            if(string.IsNullOrEmpty(Globals.G_ReportServiceURL))
                err += "Report Server Configuration Error.." + Environment.NewLine;

            if (string.IsNullOrEmpty(Globals.G_ReportSerExeUrl))
                err += "Report Execution Configuration Error.." + Environment.NewLine;

            if (string.IsNullOrEmpty(reportpass))
                err += "Password Configuration Error.." + Environment.NewLine;
            
            return err;
        }

        private void getLastUpload()
        {
            int curmth = Convert.ToInt32(txtCurYearMt.DateTime.Date.ToString("yyyyMM")); 

            string sql = "Select * from Mast_SAPTemplate_Tran Where YearMt = '" + curmth.ToString() + "'";
            LastUploadDS = Utils.Helper.GetData(sql,Utils.Helper.constr);
            bool hasrows = LastUploadDS.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);
            if (hasrows)
            {
                txtCurUploadDt.Text = Convert.ToDateTime(LastUploadDS.Tables[0].Rows[0]["UploadDt"]).ToString("yyyy-MM-dd HH:mm:ss");
                
                txtFromDt.Value = Convert.ToDateTime(LastUploadDS.Tables[0].Rows[0]["FromDt"]);
                
                txtToDate.Value = Convert.ToDateTime(LastUploadDS.Tables[0].Rows[0]["ToDt"]);
            }

            string tempmth = (curmth - 1).ToString();
            if (txtCurYearMt.DateTime.Month == 1)
            {
                tempmth = (txtCurYearMt.DateTime.Year - 1).ToString() + "12";
            }

            //previous month 
            sql = "Select * from Mast_SAPTemplate_Tran Where YearMt = '" + tempmth + "'";
            LastUploadDS = Utils.Helper.GetData(sql, Utils.Helper.constr);
            hasrows = LastUploadDS.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);
            if (hasrows)
            {
                txtLastUploadDt.Text = Convert.ToDateTime(LastUploadDS.Tables[0].Rows[0]["UploadDt"]).ToString("yyyy-MM-dd HH:mm:ss");
                txtLastYearMt.Text = LastUploadDS.Tables[0].Rows[0]["YearMt"].ToString();
                txtFromDt.Value = Convert.ToDateTime(LastUploadDS.Tables[0].Rows[0]["ToDt"]).AddDays(1);
            }
            else
            {
                txtLastUploadDt.Text = "";
                txtLastYearMt.Text = "";
            }
            
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dbcon = Utils.Helper.ReadConDb("DBCON");
            Utils.Helper.constr = dbcon.ToString();
            Globals.GetGlobalVars();
            getLastUpload();
            chkFinalUpload.Checked = false;
            chkPassOpt.Checked = false;
            txtLastUploadDt.Text = "";
            txtFromDt.Enabled = false;
        }

        private void cmbTemplate_SelectionChangeCommitted(object sender, EventArgs e)
        {
            string seltemplate = cmbTemplate.SelectedItem.ToString();
            string sql = "Select ATTDLeaveType, (SAPLeaveType + ' - ' + SAPLeaveDesc ) as SAPLeaveType from Mast_SAPLeaveCode";
            
            //SAP_ShiftSchedule_UPLOAD
            //SAP_EL_UPLOAD
            //SAP_FULL_UPLOAD
            //SAP_OD_UPLOAD
            //SAP_HALF_ABLWPAL_UPLOAD
            //SAP_HALF_CLSL_UPLOAD

            switch (seltemplate.ToUpper().Trim())
            {
                
                case "SAP_SHIFTSCHEDULE_UPLOAD":
                    sql = " select 'WrkSchedule' as ATTDLeaveType,'Shift Schedule' as SAPLeaveType ";
                    break;
                case "SAP_EL_UPLOAD":
                    sql += " Where SAPLeaveType = 'EL'";
                    break;
                case "SAP_FULL_UPLOAD":
                    sql += " Where SAPLeaveType != 'EL' ";
                    break;

                case "SAP_ARREAR_FULL_UPLOAD":
                    sql += " Where 1 = 1 ";
                    break;

                case "SAP_ARREAR_HALF_UPLOAD":
                    sql += " Where 1 = 1 ";
                    break;

                case "SAP_OD_UPLOAD":
                    sql = " select 'OD' as ATTDLeaveType,'OD - OUTDOOR DUTY' as SAPLeaveType ";
                    break;
                case "SAP_HALF_ABLWPAL_UPLOAD":
                    sql += " Where SAPLeaveType in ('AB','LWOP','AL') ";
                    break;
                case "SAP_HALF_CLSL_UPLOAD":
                    sql += " Where SAPLeaveType in ( 'CL','SL')";
                    break;
                default:
                    sql += " Where 1 = 1 ";
                    break;
            }

            //chkedLeave.Properties.Items.Clear();
            DS = Utils.Helper.GetData(sql, Utils.Helper.constr);
            chkedLeave.Properties.DataSource = DS.Tables[0];
            
            chkedLeave.Properties.DisplayMember = "SAPLeaveType";
            chkedLeave.Properties.ValueMember = "ATTDLeaveType";
            chkedLeave.Refresh();
        }

        private void txtCurYearMt_Validated(object sender, EventArgs e)
        {
            getLastUpload();
        }

        


    }
}
