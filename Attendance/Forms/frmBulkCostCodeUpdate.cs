using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.IO;
using System.Data.OleDb;
using System.Data.SqlClient;
using DevExpress.XtraGrid.Columns;
using Attendance.Classes;

namespace Attendance.Forms
{
    public partial class frmBulkCostCodeUpdate : DevExpress.XtraEditors.XtraForm
    {
        public const int ModuleID = 110;

        DataTable dt = new DataTable();

        public frmBulkCostCodeUpdate()
        {
            InitializeComponent();
            lockcontrol();

        }

        private void lockcontrol()
        {
            txtBrowse.Enabled = false;
            btnImport.Enabled = false;
            grd_view.DataSource = null;
        }

        private void unlockcontrol()
        {
            txtBrowse.Enabled = true;
            btnImport.Enabled = true;
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

        private string validateStrcture(DataTable dt)
        {
            string err = string.Empty;

            if (dt.Columns[0].ColumnName.ToUpper() != "EMPUNQID") 
            {
                err += "Invalid Column Heading of (1)..." + Environment.NewLine;
            }
            if (dt.Columns[0].ColumnName.ToUpper() != "COSTCODE")
            {
                err += "Invalid Column Heading of (2)..." + Environment.NewLine;
            }

            if (dt.Columns[0].ColumnName.ToUpper() != "VALIDFROM")
            {
                err += "Invalid Column Heading of (3)..." + Environment.NewLine;
            }

            if(!string.IsNullOrEmpty(err))
                err += "Kindly check the upload template.." + Environment.NewLine;

            return err;
        }


        private string DataValidate(string tEmpUnqID,string tCostCode,DateTime tValidFrom)
        {
            string err = string.Empty;
            clsEmp t = new clsEmp();
            t.CompCode = "01";
            t.EmpUnqID = tEmpUnqID;
            if (!t.GetEmpDetails(t.CompCode,t.EmpUnqID))
            {
                err = err + "Invalid/InActive EmpUnqID..." + Environment.NewLine;
            }

            t.CostCode = tCostCode;
            t.GetCostDesc(tCostCode);
            if (string.IsNullOrEmpty(t.CostDesc))
            {
                err = err + "Invalid CostCode..." + Environment.NewLine;
            }

            

            if (tValidFrom == DateTime.MinValue)
            {
                err = err + "Please Enter Valid From Date..." + Environment.NewLine;
                return err;
            }


            string sql = "Select max(ValidFrom) From MastCostCodeEmp where EmpUnqId = '" + t.EmpUnqID + "' " +
                " and ValidFrom > '" + tValidFrom.ToString("yyyy-MM-dd") + "'";

            string tMaxDt = Utils.Helper.GetDescription(sql, Utils.Helper.constr);

            if (!string.IsNullOrEmpty(tMaxDt))
            {
                err = err + "System Does not Allow to insert/update/delete between...." + Environment.NewLine;
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
                        string tCostCode = dr["CostCode"].ToString();
                        DateTime tdt = Convert.ToDateTime(dr["ValidFrom"]);
                        string err = DataValidate(tEmpUnqID, tCostCode, tdt);

                        if (!string.IsNullOrEmpty(err))
                        {
                            dr["Remarks"] = err;
                            continue; 
                        }


                        using (SqlCommand cmd = new SqlCommand())
                        {


                            try
                            {
                                
                                cmd.Connection = con;
                                string sql = "Insert into MastCostCodeEmp (EmpUnqID,CostCode,ValidFrom,AddDt,AddID) Values ('{0}','{1}',GetDate(),'{2}')";
                                sql = string.Format(sql, tEmpUnqID, tCostCode.ToUpper(),
                                    tdt.ToString("yyyy-MM-dd"),
                                    Utils.User.GUserID);

                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                dr["remarks"] = "Record saved...";

                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                continue;
                            }
                            
                            
                        }
                    }

                    con.Close();
                }

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
            btnImport.Enabled = true;

            Cursor.Current = Cursors.WaitCursor;
            grd_view.DataSource = null;
            string filePath = txtBrowse.Text.ToString();

            string sexcelconnectionstring = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties=\"Excel 12.0 Xml;HDR=YES;IMEX=1;\"";
            //string sexcelconnectionstring = @"provider=microsoft.jet.oledb.4.0;data source=" + filePath + ";extended properties=" + "\"excel 8.0;hdr=yes;IMEX=1;\"";

            OleDbConnection oledbconn = new OleDbConnection(sexcelconnectionstring);
            oledbconn.Open();
            string str = oledbconn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null).Rows[0]["TABLE_NAME"].ToString();
            string sheetname = "[" + str.Replace("'", "") + "]";

            try
            {
                string myexceldataquery = "select EmpUnqID,CostCode,ValidFrom,'' as Remarks from " + sheetname;
                OleDbDataAdapter oledbda = new OleDbDataAdapter(myexceldataquery, oledbconn);
                dt.Clear();
                oledbda.Fill(dt);
            }
            catch (Exception ex)
            {
                oledbconn.Close();
                MessageBox.Show("Please Check upload template..", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Cursor.Current = Cursors.Default;
                btnImport.Enabled = false;
                return;
            }
            

            DataView dv = dt.DefaultView;
            dv.Sort = "EmpUnqID asc";
            DataTable sortedDT = dv.ToTable();




            grd_view.DataSource = sortedDT;

            btnImport.Enabled = true;

            Cursor.Current = Cursors.Default;
        }
    }
}