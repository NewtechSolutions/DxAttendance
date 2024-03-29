﻿using Attendance.Classes;
using DevExpress.XtraGrid.Columns;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;

namespace Attendance.Forms
{
    public partial class frmMastEmpBulkIdentity : DevExpress.XtraEditors.XtraForm
    {
        public string GRights = "XXXV";
       


        DataTable dt = new DataTable();

        public frmMastEmpBulkIdentity()
        {
            InitializeComponent();
            
        }

        
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog openKeywordsFileDialog = new OpenFileDialog();
            openKeywordsFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            openKeywordsFileDialog.Multiselect = false;
            openKeywordsFileDialog.ValidateNames = true;
            //openKeywordsFileDialog.CheckFileExists = true;
            openKeywordsFileDialog.DereferenceLinks = false;        //Will return .lnk in shortcuts.
            openKeywordsFileDialog.Filter = "Files|*.xls;*.xlsx;*.xlsb";
            openKeywordsFileDialog.FileOk += new System.ComponentModel.CancelEventHandler(OpenKeywordsFileDialog_FileOk);
            var dialogResult = openKeywordsFileDialog.ShowDialog();

            if (dialogResult == System.Windows.Forms.DialogResult.OK)
            {
                //first check if already exits if found return..
                string filenm = openKeywordsFileDialog.FileName.ToString();
                if (string.IsNullOrEmpty(filenm))
                    return;
                try
                {
                    txtBrowse.Text = openKeywordsFileDialog.FileName;
                }
                catch (Exception ex)
                {
                    txtBrowse.Text = "";
                }
            }
            else
            {
                txtBrowse.Text = "";
            }
        }

        void OpenKeywordsFileDialog_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            OpenFileDialog fileDialog = sender as OpenFileDialog;
            string selectedFile = fileDialog.FileName;
            if (string.IsNullOrEmpty(selectedFile) || selectedFile.Contains(".lnk"))
            {
                MessageBox.Show("Please select a valid File");
                e.Cancel = true;
            }
            return;
        }

        private string DataValidate(DataRow tdr)
        {
            string err = string.Empty;
            clsEmp t = new clsEmp();
            
            t.EmpUnqID = tdr["EmpUnqID"].ToString();
            if (!t.GetEmpDetails(t.EmpUnqID))
            {
                err = err + "Invalid/InActive EmpUnqID..." + Environment.NewLine;
                return err;
            }
            
            return err;
        }


        private void btnImport_Click(object sender, EventArgs e)
        {

            Cursor.Current = Cursors.WaitCursor;

            DataTable dtMaterial = new DataTable();
            DataTable sortedDT = new DataTable();
            try
            {

                foreach (GridColumn column in grd_view1.VisibleColumns)
                {
                    if (column.FieldName != string.Empty)
                        dtMaterial.Columns.Add(column.FieldName, column.ColumnType);
                }


                for (int i = 0; i < grd_view1.DataRowCount; i++)
                {
                    DataRow row = dtMaterial.NewRow();

                    foreach (GridColumn column in grd_view1.VisibleColumns)
                    {
                        row[column.FieldName] = grd_view1.GetRowCellValue(i, column);
                    }
                    dtMaterial.Rows.Add(row);
                }

                DataView dv = dtMaterial.DefaultView;
                dv.Sort = "EmpUnqID asc";
                sortedDT = dv.ToTable();

                using (SqlConnection con = new SqlConnection(Utils.Helper.constr))
                {
                    
                    con.Open();
                    foreach (DataRow dr in sortedDT.Rows)
                    {
                        string tEmpUnqID = dr["EmpUnqID"].ToString();
                        
                        string err = DataValidate(dr);

                        if (!string.IsNullOrEmpty(err))
                        {
                            dr["Remarks"] = err;
                            continue;
                        }

                        
                        bool tfound = false;
                        string tsql = "Select count(*) from MastEmpIdentity where EmpUnqID ='" + tEmpUnqID + "' ";
                        DataSet rds = Utils.Helper.GetData(tsql, Utils.Helper.constr, out string terr);
                        if (!string.IsNullOrEmpty(terr))
                        {
                            tfound = false;
                            dr["Remarks"] = terr;
                            return;
                           
                        }
                        else
                        {
                            tfound = true;
                        }
                       
                        string tPassportNo = dr["PassportNo"].ToString().Trim();
                        string tAdharNo = dr["AdharNo"].ToString().Trim();
                        string tDrvLicNo = dr["DrvLicNo"].ToString().Trim();                       
                        string tElectionNo = dr["ElectionNo"].ToString().Trim();
                        string tPANNo = dr["PANNo"].ToString().Trim();
                        string tPFACNo = dr["PFACNo"].ToString().Trim();
                        string tUANNo = dr["UANNo"].ToString().Trim();
                        string tBankName = dr["BankName"].ToString().Trim();
                        string tBankAcNo = dr["BankAcNo"].ToString().Trim();
                        string tBankBranch = dr["BankBranch"].ToString().Trim();
                        string tBankIFSC = dr["BankIFSC"].ToString().Trim();

                        #region Final_Update

                        using (SqlCommand cmd = new SqlCommand())
                        {
                            try
                            {

                                cmd.Connection = con;
                                cmd.CommandType = CommandType.Text;
                                string sql = String.Empty;

                                if (tfound)
                                {
                                   sql= "Update MastEmpIdentity " +
                                        " Set Passport='" + tPassportNo + "', AdharCard='" + tAdharNo + "'," +
                                        " ElectionCard='" + tElectionNo + "', PanCard='" + tPANNo + "'," +
                                        " DrivingLicense='" + tDrvLicNo + "', PFAccountNo='" + tPFACNo + "'," +
                                        " UANNO='" + tUANNo + "'," +
                                        " BankName = '" + tBankName + "', BankAccountNo = '" + tBankAcNo + "'," +
                                        " BankBranchCode='" + tBankBranch + "', BankIFSCCode='" + tBankIFSC + "'," +
                                        " UpdDt =GetDate() , UpdID='" + Utils.User.GUserID + "'" + 
                                        "  Where EmpUnqID ='" + tEmpUnqID + "' ";
                                }
                                else
                                {
                                    sql = "insert into MastEmpIdentity (EmpUnqID,Passport,AdharCard,ElectionCard,PanCard," +
                                        " DrivingLicense,PFAccountNo,UANNO,BankName,BankAccountNo,BankBranchCode,BankIFSCCode," +
                                        " AddDt,AddID) Values (" +
                                        "'" + tEmpUnqID + "','" + tPassportNo + "','" + tAdharNo + "','" + tElectionNo + "','" + tPANNo + "','" + tDrvLicNo + "'," +
                                        "'" + tPFACNo + "','" + tUANNo + "','" + tBankName + "','" + tBankAcNo + "','" + tBankBranch + "','" + tBankIFSC + "',GetDate(),'" + Utils.User.GUserID + "')";
                                }

                                cmd.CommandText = sql;
                                cmd.CommandTimeout = 0;
                                cmd.ExecuteNonQuery();

                                dr["remarks"] = "Updated..";

                            }
                            catch (Exception ex)
                            {
                                dr["remarks"] = dr["remarks"].ToString() + ex.ToString();
                                continue;
                            }

                        }//using sqlcommand
                        #endregion
                    }//using foreach

                    con.Close();
                }//using connection

                Cursor.Current = Cursors.Default;
                MessageBox.Show("file uploaded Successfully, please check the remarks for indivisual record status...", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            DataSet ds = new DataSet();
            ds.Tables.Add(sortedDT);
            grd_view.DataSource = ds;
            grd_view.DataMember = ds.Tables[0].TableName;
            grd_view.Refresh();

            Cursor.Current = Cursors.Default;
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtBrowse.Text.Trim().ToString()))
            {
                MessageBox.Show("Please Select Excel File First...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            btnBrowse.Enabled = false;

            if (GRights.Contains("A") || GRights.Contains("U") || GRights.Contains("D"))
            {
                btnImport.Enabled = true;
            }
            else
            {
                btnImport.Enabled = false;
            }

            

            Cursor.Current = Cursors.WaitCursor;
            grd_view.DataSource = null;
            string filePath = txtBrowse.Text.ToString();

            string sexcelconnectionstring = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties=\"Excel 12.0 Xml;HDR=YES;IMEX=1;\"";
            //string sexcelconnectionstring = @"provider=microsoft.jet.oledb.4.0;data source=" + filePath + ";extended properties=" + "\"excel 8.0;hdr=yes;IMEX=1;\"";

            OleDbConnection oledbconn = new OleDbConnection(sexcelconnectionstring);
            List<SheetName> sheets = ExcelHelper.GetSheetNames(oledbconn);
            string sheetname = "[" + sheets[0].sheetName.Replace("'", "") + "]";

            try
            {
                oledbconn.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            


            try
            {
                string myexceldataquery = "select EmpUnqID,PassportNo,AdharNo,DrvLicNo,ElectionNo,PANNo,PFACNo,UANNo,BankName,BankAcNo,BankBranch,BankIFSC, " +
                    " '' as Remarks from " + sheetname;

                OleDbDataAdapter oledbda = new OleDbDataAdapter(myexceldataquery, oledbconn);
                dt.Clear();
                oledbda.Fill(dt);
                
                dt.AcceptChanges();
                foreach (DataRow row in dt.Rows)
                {
                    if (string.IsNullOrEmpty(row["EmpUnqID"].ToString().Trim()))
                        row.Delete();
                }
                dt.AcceptChanges();

                oledbconn.Close();
            }
            catch (Exception ex)
            {
                oledbconn.Close();
                MessageBox.Show("Please Check upload template.." + Environment.NewLine + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Cursor.Current = Cursors.Default;
                btnImport.Enabled = false;
                oledbconn.Close();
                return;
            }
            

            DataView dv = dt.DefaultView;
            dv.Sort = "EmpUnqID asc";
            DataTable sortedDT = dv.ToTable();




            grd_view.DataSource = sortedDT;

            if (GRights.Contains("A") || GRights.Contains("U") || GRights.Contains("D"))
            {
                btnImport.Enabled = true;
            }
            else
            {
                btnImport.Enabled = false;
            }

            Cursor.Current = Cursors.Default;
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "Excel (2003)(.xls)|*.xls|Excel (2010) (.xlsx)|*.xlsx |RichText File (.rtf)|*.rtf |Pdf File (.pdf)|*.pdf |Html File (.html)|*.html";
                if (saveDialog.ShowDialog() != DialogResult.Cancel)
                {
                    string exportFilePath = saveDialog.FileName;
                    string fileExtenstion = new FileInfo(exportFilePath).Extension;

                    switch (fileExtenstion)
                    {
                        case ".xls":
                            grd_view.ExportToXls(exportFilePath);
                            break;
                        case ".xlsx":
                            grd_view.ExportToXlsx(exportFilePath);
                            break;
                        case ".rtf":
                            grd_view.ExportToRtf(exportFilePath);
                            break;
                        case ".pdf":
                            grd_view.ExportToPdf(exportFilePath);
                            break;
                        case ".html":
                            grd_view.ExportToHtml(exportFilePath);
                            break;
                        case ".mht":
                            grd_view.ExportToMht(exportFilePath);
                            break;
                        default:
                            break;
                    }

                    if (File.Exists(exportFilePath))
                    {
                        try
                        {
                            //Try to open the file and let windows decide how to open it.
                            System.Diagnostics.Process.Start(exportFilePath);
                        }
                        catch
                        {
                            String msg = "The file could not be opened." + Environment.NewLine + Environment.NewLine + "Path: " + exportFilePath;
                            MessageBox.Show(msg, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        String msg = "The file could not be saved." + Environment.NewLine + Environment.NewLine + "Path: " + exportFilePath;
                        MessageBox.Show(msg, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void frmMastEmpBulkIdentity_Load(object sender, EventArgs e)
        {
            GRights = Attendance.Classes.Globals.GetFormRights(this.Name);
                
            grd_view.DataSource = null;
            btnImport.Enabled = false;
        }
    }
}