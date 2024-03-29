﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Data.SqlClient;
using Attendance.Classes;
using System.IO;
using System.Data.OleDb;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;

namespace Attendance.Forms
{
    public partial class frmMastUserManagement : DevExpress.XtraEditors.XtraForm
    {
       
        private List<UserBioInfo> tUserList  = new List<UserBioInfo>();
        private string GRights = "XXXV";
        private DataTable dt = new DataTable();
        private static DataSet srcDs = new DataSet();
        private bool SelAllFlg = false;

        public frmMastUserManagement()
        {
            InitializeComponent();
        }

        private void LoadGrid(int tMachineType)
        {
            DataTable tmpmachine = srcDs.Tables[0].Copy();
            tmpmachine.Rows.Clear();
            DataRow[] dtrs ;

            switch (tMachineType)
            {
                case 1: //attendance
                    dtrs = srcDs.Tables[0].Select("CanteenFlg = 0 And GateInOut = 0 and LunchInOut = 0");
                    
                    break;
                case 2: //canteen
                    dtrs = srcDs.Tables[0].Select("CanteenFlg = 1 And GateInOut = 0 and LunchInOut = 0");
                    break;
                case 3: //lunch inout
                    dtrs = srcDs.Tables[0].Select("CanteenFlg = 0 And GateInOut = 0 and LunchInOut = 1");
                    break;
                case 4: //gate inout
                    dtrs = srcDs.Tables[0].Select("CanteenFlg = 0 And GateInOut = 1 and LunchInOut = 0");
                    break;
                default:
                    dtrs = srcDs.Tables[0].Select("CanteenFlg = 0 And GateInOut = 0 and LunchInOut = 0");
                    break;
            }

            foreach (DataRow dr in dtrs)
            {
                
                tmpmachine.ImportRow(dr);
            }

            
            grpGrid.DataSource = tmpmachine;
            //grpGrid.DataMember = srcDs.Tables[0].TableName;

            gv_avbl.Appearance.HeaderPanel.Font = new System.Drawing.Font(gv_avbl.Appearance.ViewCaption.Font, FontStyle.Bold);
            gv_avbl.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            foreach (GridColumn gc in gv_avbl.Columns)
            {
                if (gc.Caption == "Location" || gc.Caption == "Remarks")
                {
                    gc.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                    gc.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                    gc.Width = 200;
                }
                else
                {
                    gc.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    gc.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                }

               
            }

            gv_avbl.RefreshData();

        }

        private void txtMessGrpCode_Validated(object sender, EventArgs e)
        {
            if (txtMessGrpCode.Text.Trim() == "" )
            {
                txtMessGrpDesc.Text = "";
                return;
            }

            txtMessGrpCode.Text = txtMessGrpCode.Text.Trim().ToString().PadLeft(3, '0');

            DataSet ds = new DataSet();
            string sql = "select * From MastMessGrp where CompCode = '01' and " +
                    " UnitCode ='" + txtUnitCode.Text.Trim() + "' and MessGrpCode= '" + txtMessGrpCode.Text.Trim() + "'";

            ds = Utils.Helper.GetData(sql, Utils.Helper.constr);
            bool hasRows = ds.Tables.Cast<DataTable>()
                           .Any(table => table.Rows.Count != 0);

            if (hasRows)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    
                    txtMessGrpCode.Text = dr["MessGrpCode"].ToString();
                    txtMessGrpDesc.Text = dr["MessGrpDesc"].ToString();
                    

                }
            }
        }

        private void txtUnitCode_Validated(object sender, EventArgs e)
        {
            if (txtUnitCode.Text.Trim() == "" )
            {
                txtUnitDesc.Text = "";
                return;
            }

            txtUnitCode.Text = txtUnitCode.Text.Trim().ToString().PadLeft(3, '0');

            DataSet ds = new DataSet();
            string sql = "select * From MastUnit where CompCode ='01' " +
                    " and  UnitCode ='" + txtUnitCode.Text.Trim() + "'";

            ds = Utils.Helper.GetData(sql, Utils.Helper.constr);
            bool hasRows = ds.Tables.Cast<DataTable>()
                           .Any(table => table.Rows.Count != 0);

            if (hasRows)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                   
                    txtUnitCode.Text = dr["UnitCode"].ToString();
                    txtUnitDesc.Text = dr["UnitName"].ToString();
                   

                }
            }
            else
            {
                txtUnitDesc.Text = "";
            }
        }

        private void txtMessCode_Validated(object sender, EventArgs e)
        {
            if ( txtMessCode.Text.Trim() == "")
            {
                txtMessDesc.Text = "";
                return;
            }

            txtMessCode.Text = txtMessCode.Text.Trim().ToString().PadLeft(3, '0');

            DataSet ds = new DataSet();
            string sql = "select * From MastMess where CompCode ='01' " +
                    " and UnitCode ='" + txtUnitCode.Text.Trim() + "' and MessCode ='" + txtMessCode.Text.Trim() + "'";

            ds = Utils.Helper.GetData(sql, Utils.Helper.constr);
            bool hasRows = ds.Tables.Cast<DataTable>()
                           .Any(table => table.Rows.Count != 0);

            if (hasRows)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    
                    txtMessCode.Text = dr["MessCode"].ToString();
                    txtMessDesc.Text = dr["MessDesc"].ToString();
                    

                }
            }
            else
            {
                txtMessDesc.Text = "";
            }

            
        }

        private void txtWrkGrpCode_Validated(object sender, EventArgs e)
        {

            if (txtWrkGrpCode.Text.Trim() == "")
            {
                txtWrkGrpDesc.Text = "";
                return;
            }
            
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
            else
            {
                txtWrkGrpDesc.Text = "";
            }


        }

        private void txtEmpUnqID_Validated(object sender, EventArgs e)
        {
            if (txtEmpUnqID.Text.Trim() == "")
            {
                txtEmpName.Text = "";
                txtUnitCode.Text = "";
                txtUnitDesc.Text = "";

                txtWrkGrpCode.Text = "";
                txtWrkGrpDesc.Text = "";

                txtMessCode.Text = "";
                txtMessDesc.Text = "";

                txtMessGrpCode.Text = "";
                txtMessGrpDesc.Text = "";
                txtOldRFID.Text = "";

                chkActive.Checked = false;
                chkComp.Checked = false;
                chkCont.Checked = false;
                chkFace.Checked = false;
                chkFinger.Checked = false;
                chkRFID.Checked = false;
                return;
            }

            DataSet ds = new DataSet();
            string sql = "select EmpUnqID,EmpName,WrkGrp,UnitCode,'001' as MessCode,'001' as MessGrpCode,1 as PayrollFlg,0 as ContractFlg,Active,PunchingBlocked From V_EMPMast where EmpUnqID ='" + txtEmpUnqID.Text.Trim() + "' And Active = 1";

            ds = Utils.Helper.GetData(sql, Utils.Helper.constr);
            bool hasRows = ds.Tables.Cast<DataTable>()
                           .Any(table => table.Rows.Count != 0);

            if (hasRows)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    txtWrkGrpCode.Text = dr["WrkGrp"].ToString();
                    txtMessCode.Text = dr["MessCode"].ToString();
                    txtMessGrpCode.Text = dr["MessGrpCode"].ToString();
                    txtEmpName.Text = dr["EmpName"].ToString();
                    txtUnitCode.Text = dr["UnitCode"].ToString();

                    chkActive.Checked = Convert.ToBoolean(dr["Active"]);
                    chkComp.Checked = Convert.ToBoolean(dr["PayrollFlg"]);
                    chkCont.Checked = Convert.ToBoolean(dr["ContractFlg"]);

                    bool isBlocked = Convert.ToBoolean(dr["PunchingBlocked"]);
                    if (isBlocked)
                    {
                        chkActive.Checked = false;
                        MessageBox.Show("This Employee is Blocked...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    
                    txtWrkGrpCode_Validated(sender, e);
                    txtUnitCode_Validated(sender, e);
                   // txtMessCode_Validated(sender, e);
                   // txtMessGrpCode_Validated(sender, e);
                                   

                } //foreach employee
            }
            else
            {
                txtEmpName.Text = "";
                txtWrkGrpCode.Text = "";
                txtWrkGrpDesc.Text = "";
                txtMessCode.Text = "";
                txtMessDesc.Text = "";
                txtMessGrpCode.Text = "";
                txtMessGrpDesc.Text = "";
                txtOldRFID.Text = "";
                chkActive.Checked = false;
                chkComp.Checked = false;
                chkCont.Checked = false;
                chkFace.Checked = false;
                chkFinger.Checked = false;
                chkRFID.Checked = false;
            }

            UserBioInfo x = new UserBioInfo();
            x.GetBioInfoFromDB(txtEmpUnqID.Text.Trim());
            chkRFID.Checked = ((!string.IsNullOrEmpty(x.CardNumber)) ? true : false);
            chkFace.Checked = ((!string.IsNullOrEmpty(x.FaceTemp)) ? true : false);
            chkFinger.Checked = ((!string.IsNullOrEmpty(x.FingerTemp)) ? true : false);
            txtOldRFID.Text = x.CardNumber;
            
        }
                        
        private void btnBulkUpload_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(cmbListMachine2.Text.Trim() ))
            {
                MessageBox.Show("Please Select Machine...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if(gv_Upload.DataRowCount <= 0)
            {
                 MessageBox.Show("Please Enter EmpUnqID...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            

            DataTable dtMaterial = new DataTable();
            DataTable sortedDT = new DataTable();
            try
            {
                foreach (GridColumn column in gv_Upload.VisibleColumns)
                {
                    if (column.FieldName != string.Empty)
                        dtMaterial.Columns.Add(column.FieldName, column.ColumnType);
                }


                for (int i = 0; i < gv_Upload.DataRowCount; i++)
                {
                    DataRow row = dtMaterial.NewRow();

                    foreach (GridColumn column in gv_Upload.VisibleColumns)
                    {
                        row[column.FieldName] = gv_Upload.GetRowCellValue(i, column);
                    }
                    dtMaterial.Rows.Add(row);
                }

                DataView dv = dtMaterial.DefaultView;
                dv.Sort = "EmpUnqID asc";
                sortedDT = dv.ToTable();


                string ip = cmbListMachine2.Text.Trim();
                string ioflg = "B";
                string err;
                clsMachine m = new clsMachine(ip,ioflg);
                m.Connect(out err);

                if(!string.IsNullOrEmpty(err)){
                    MessageBox.Show(err,"Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    return;
                }
                
                //Cursor.Current = Cursors.WaitCursor;
                LockCtrl();
                //if (m.BeginBathUpload())
                //{

                    foreach (DataRow dr in sortedDT.Rows)
                    {
                        string tEmpUnqID = dr["EmpUnqID"].ToString();
                        if (string.IsNullOrEmpty(tEmpUnqID))
                        {
                            dr["Remarks"] = "EmpUnqID Required...";
                            continue;
                        }
                        err = string.Empty;
                        Application.DoEvents();
                        m.Register(tEmpUnqID, out err);

                        if (string.IsNullOrEmpty(err))
                        {
                            using (SqlConnection cn = new SqlConnection(Utils.Helper.constr))
                            {
                                using (SqlCommand cmd = new SqlCommand())
                                {
                                    try
                                    {
                                        cn.Open();
                                        cmd.Connection = cn;

                                        int tmaxid = Convert.ToInt32(Utils.Helper.GetDescription("Select isnull(Max(ID),0) + 1 from MastMachineUserOperation", Utils.Helper.constr));

                                        string sql = "insert into MastMachineUserOperation (ID,EmpUnqID,MachineIP,IOFLG,Operation,ReqDt,ReqBy,DoneFlg,AddDt,LastError) Values ('" + tmaxid + "','" +
                                            tEmpUnqID + "','" + ip + "','" + ioflg + "','BULKREGISTER',GetDate(),'" + Utils.User.GUserID + "',1,GetDate(),'" + err + "')";


                                        cmd.CommandText = sql;
                                        cmd.ExecuteNonQuery();

                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                }//using command
                            }//using connection
                        }
                        
                        dr["Remarks"] = (!string.IsNullOrEmpty(err) ? err : "Registered");

                    }
                //    m.BathUpload();
                //}
                m.RefreshData();
                m.DisConnect(out err);
                //Cursor.Current = Cursors.Default;
                MessageBox.Show("file uploaded Successfully, please check the remarks for indivisual record status...", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            DataSet ds = new DataSet();
            ds.Tables.Add(sortedDT);
            grd_Upload.DataSource = ds;
            grd_Upload.DataMember = ds.Tables[0].TableName;
            grd_Upload.Refresh();

            UnLockCtrl();
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

            if (GRights.Contains("A") || GRights.Contains("U"))
            {
                btnBulkUpload.Enabled = true;
            }
            else
            {
                btnBulkUpload.Enabled = false;
            }

            if(GRights.Contains("D"))
            {
                btnBulkDelete.Enabled = true;

            }else{
                btnBulkDelete.Enabled = false;
            }



            Cursor.Current = Cursors.WaitCursor;
            grd_Upload.DataSource = null;
            string filePath = txtBrowse.Text.ToString();

            string sexcelconnectionstring = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties=\"Excel 12.0 Xml;HDR=YES;IMEX=1;\"";
            //string sexcelconnectionstring = @"provider=microsoft.jet.oledb.4.0;data source=" + filePath + ";extended properties=" + "\"excel 8.0;hdr=yes;IMEX=1;\"";

            OleDbConnection oledbconn = new OleDbConnection(sexcelconnectionstring);
            oledbconn.Open();
            string str = oledbconn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null).Rows[0]["TABLE_NAME"].ToString();
            string sheetname = "[" + str.Replace("'", "") + "]";

            try
            {
                string myexceldataquery = "select EmpUnqID,'' as Remarks from " + sheetname;
                OleDbDataAdapter oledbda = new OleDbDataAdapter(myexceldataquery, oledbconn);
                dt.Clear();
                oledbda.Fill(dt);
                oledbconn.Close();
            }
            catch (Exception ex)
            {
                oledbconn.Close();
                MessageBox.Show("Please Check upload template..", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Cursor.Current = Cursors.Default;
                btnBrowse.Enabled = true;
                btnBulkUpload.Enabled = false;
                oledbconn.Close();
                return;
            }


            DataView dv = dt.DefaultView;
            dv.Sort = "EmpUnqID asc";
            DataTable sortedDT = dv.ToTable();




            grd_Upload.DataSource = sortedDT;

            if (GRights.Contains("A") || GRights.Contains("U"))
            {
                btnBulkUpload.Enabled = true;
            }
            else
            {
                btnBulkUpload.Enabled = false;
            }

            if(GRights.Contains("D"))
            {
                btnBulkDelete.Enabled = true;
            }
            else
            {
                btnBulkDelete.Enabled = false;
            }
            
            gv_Upload.RefreshData();
            btnBrowse.Enabled = true;
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
                            grd_Upload.ExportToXls(exportFilePath);
                            break;
                        case ".xlsx":
                            grd_Upload.ExportToXlsx(exportFilePath);
                            break;
                        case ".rtf":
                            grd_Upload.ExportToRtf(exportFilePath);
                            break;
                        case ".pdf":
                            grd_Upload.ExportToPdf(exportFilePath);
                            break;
                        case ".html":
                            grd_Upload.ExportToHtml(exportFilePath);
                            break;
                        case ".mht":
                            grd_Upload.ExportToMht(exportFilePath);
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

        private void frmMastUserManagement_Load(object sender, EventArgs e)
        {

            cmbListMachine1.Properties.Items.Clear();
            cmbListMachine2.Properties.Items.Clear();
            //cmbListMachine1.Properties.Items.Add("192.168.6.9" + "-" + "Master_Tripod");
            //cmbListMachine1.Properties.Items.Add(Globals.MasterMachineIP + "-" + "Master_Old");

            foreach (KeyValuePair<string,string> t in Globals.MasterIP)
            {
                cmbListMachine1.Properties.Items.Add(t.Key.Trim() + "-" + t.Value.Trim());
            }


            //load all machine ip in combo
            DataSet ds = new DataSet();
            ds = Utils.Helper.GetData("Select MachineIP From ReaderConfig where master = 0 and active = 1 Order By MachineNo", Utils.Helper.constr);
            bool hasRows = ds.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);

            if (hasRows)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    cmbListMachine1.Properties.Items.Add(dr["MachineIP"].ToString());
                    cmbListMachine2.Properties.Items.Add(dr["MachineIP"].ToString());
                }
            }
            cmbListMachine1.SelectedIndex = 0;

            string sql = "Select CONVERT(BIT,0) as SEL,MachineDesc,MachineIP,MachineNo,'' as Remarks,AutoClear,IOFLG,RFID,FACE,FINGER,CanteenFLG,GateInOut,LunchInOut From ReaderConfig Where Active = 1 and Master = 0 Order By MachineDesc,IOFLG";

            srcDs = Utils.Helper.GetData(sql, Utils.Helper.constr);

            GRights = Globals.GetFormRights(this.Name);
            grd_Upload.DataSource = null;
            btnBulkUpload.Enabled = false;
            optMachineType.EditValue = 1;

            if (Utils.User.IsAdmin)
            {
                btnUnlock.Visible = true;
                btnUnlock.Enabled = true;
            }else
            {
                btnUnlock.Visible = false;
                btnUnlock.Enabled = false;
            }
        }

        private void optMachineType_EditValueChanged(object sender, EventArgs e)
        {
            //load machine type to grid...
            LoadGrid(Convert.ToInt32(optMachineType.EditValue));
        }

        private void btnSelAll_Click(object sender, EventArgs e)
        {
            if (gv_avbl.DataRowCount <= 0)
            {
                return;
            }
            Cursor.Current = Cursors.WaitCursor;

            SelAllFlg = (!SelAllFlg);

            for (int i = 0; i <= gv_avbl.DataRowCount - 1; i++)
            {
                if (SelAllFlg == true)
                {
                    gv_avbl.SetRowCellValue(i, "SEL", true);

                }
                else
                {
                    gv_avbl.SetRowCellValue(i, "SEL", false);

                }

            }

            Cursor.Current = Cursors.Default;
        }

        private void btnAddEmp_Click(object sender, EventArgs e)
        {
            

            string tEmpUnqID = txtEmpUnqID.Text.Trim();
            UserBioInfo user = new UserBioInfo();   
           
            user.GetBioInfoFromDB(tEmpUnqID);
            user.MessCode = txtMessCode.Text.Trim();
            user.MessGrpCode = txtMessGrpCode.Text.Trim();
            user.WrkGrp = txtWrkGrpCode.Text.Trim();
            user.UserID = tEmpUnqID;
            
            if (user.UserID == tEmpUnqID)
            {
                tUserList.RemoveAll(tmpuser => tmpuser.UserID == tEmpUnqID);
                tUserList.Add(user);   
                grd_Emp.DataSource = tUserList.Select(myClass => new {  myClass.UserID, myClass.UserName, myClass.err }).ToList();
            }

            txtEmpUnqID.Text = "";
            txtEmpUnqID_Validated(sender, e);
            txtEmpUnqID.Focus();
        }

        private void btnDelEmp_Click(object sender, EventArgs e)
        {
            string tEmpUnqID = "";
            if (gv_Emp.SelectedRowsCount <= 0)
                return;


            try
            {
                foreach (int i in gv_Emp.GetSelectedRows())
                {
                    tEmpUnqID = gv_Emp.GetRowCellValue(i, "UserID").ToString();
                    tUserList.RemoveAll(tmpuser => tmpuser.UserID == tEmpUnqID);
                }
            }catch (Exception ex)
            {
                
            }
            

            grd_Emp.DataSource = tUserList.Select(myClass => new { myClass.UserID, myClass.UserName, myClass.err }).ToList();
        }

        private void btn_MasterDownload_Click(object sender, EventArgs e)
        {
            if (cmbListMachine1.Text.Trim() == string.Empty)
            {
                UnLockCtrl();
                Cursor.Current = Cursors.Default;
                
                return;
            }
            
            lblDownAll.Text = "";

            string err;
            //machine selection
                
            string machineip = string.Empty;
            string[] tmpmachine = cmbListMachine1.Text.Trim().Split('-');
            if (tmpmachine.Length > 0)
                machineip = tmpmachine[0].Trim();
            else
                machineip = cmbListMachine1.Text.Trim();
                
            lblDownAll.Text = "Downloading...";
            lblDownAll.Update();

            if (tUserList.Count > 0)
            {
                LockCtrl();
                Cursor.Current = Cursors.WaitCursor;

                clsMachine m = new clsMachine(machineip, "B");
                  

                //try to connect
                m.Connect(out err);
                if (!string.IsNullOrEmpty(err))
                {
                    lblDownAll.Text = err;
                    return;
                }
                List<UserBioInfo> tempusers = new List<UserBioInfo>();
                m.DownloadTemplate(tUserList, out err, out tempusers);

                m.DisConnect(out err);
                grd_Emp.DataSource = tUserList.Select(myClass => new { myClass.UserID, myClass.UserName, myClass.err }).ToList();
                
                UnLockCtrl();
                Cursor.Current = Cursors.Default;
                MessageBox.Show("Completed", "Info", MessageBoxButtons.OK);

            }
            else
            {
                DialogResult dr = MessageBox.Show("Are you Sure to download All User from Selected Machine ?", "Question", MessageBoxButtons.YesNoCancel);

                if (dr == DialogResult.Cancel || dr == DialogResult.No)
                    return;

                this.Cursor = Cursors.WaitCursor;
                List<UserBioInfo> tusers = new List<UserBioInfo>();
                clsMachine tmach = new clsMachine(machineip.Trim(), "B");
                tmach.Connect(out err);
                if (!string.IsNullOrEmpty(err))
                {
                    lblDownAll.Text = err;
                    return;
                }
                tmach.DownloadALLUsers(true, out err, out tusers);
                tmach.DisConnect(out err);
                lblDownAll.Text = tusers.Count().ToString() + " Downloaded Users";
                lblDownAll.Update();

                this.Cursor = Cursors.Default;
                MessageBox.Show("Download Complete", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
           
            if (tUserList.Count == 0)
            {
                MessageBox.Show("Please Add Employeee first...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            
            ResetRemarks();
            LockCtrl();
            //Cursor.Current = Cursors.WaitCursor;

            for (int i = 0; i < gv_avbl.DataRowCount; i++)
            {

                Application.DoEvents();

                string tsel = gv_avbl.GetRowCellValue(i, "SEL").ToString();
                if (!Convert.ToBoolean(tsel))
                    continue;

                string ip = gv_avbl.GetRowCellValue(i, "MachineIP").ToString();
                string ioflg = gv_avbl.GetRowCellValue(i, "IOFLG").ToString().Trim();
                gv_avbl.SetRowCellValue(i, "Remarks", "Connecting");

                clsMachine m = new clsMachine(ip, ioflg);
                string err = string.Empty;

                //try to connect
                m.Connect(out err);
                gv_avbl.SetRowCellValue(i, "Remarks", err);

                if (!string.IsNullOrEmpty(err))
                {
                    continue;
                }
                
                string allerr = "";
                m.EnableDevice(false);
                foreach (UserBioInfo emp in tUserList)
                {
                    

                    m.Register(emp.UserID, out err);
                    if (!string.IsNullOrEmpty(err))
                    {
                        allerr += emp.UserID + "--" + err + Environment.NewLine;
                    }
                    else
                    {
                       
                        using (SqlConnection cn = new SqlConnection(Utils.Helper.constr))
                        {
                            using (SqlCommand cmd = new SqlCommand())
                            {
                                try
                                {
                                    cn.Open();
                                    cmd.Connection = cn;

                                    int tmaxid = Convert.ToInt32(Utils.Helper.GetDescription("Select isnull(Max(ID),0) + 1 from MastMachineUserOperation", Utils.Helper.constr));
                                    string sql = "insert into MastMachineUserOperation (ID,EmpUnqID,MachineIP,IOFLG,Operation,ReqDt,ReqBy,DoneFlg,AddDt,LastError) Values ('" + tmaxid + "','" +
                                        emp.UserID + "','" + ip + "','" + ioflg + "','REGISTER',GetDate(),'" + Utils.User.GUserID + "',1,GetDate(),'Completed')";


                                    cmd.CommandText = sql;
                                    cmd.ExecuteNonQuery();

                                }
                                catch (Exception ex)
                                {

                                }
                            }//using command
                        }//using connection                   
                    }

                }//foreach...

                if (string.IsNullOrEmpty(allerr))
                {
                    gv_avbl.SetRowCellValue(i, "Remarks", "Registered..");
                }
                else
                {
                    gv_avbl.SetRowCellValue(i, "Remarks", allerr + ",Others are Registered");
                }
                m.RefreshData();
                m.EnableDevice(true);
                m.DisConnect(out err);

            }


            UnLockCtrl();
            //Cursor.Current = Cursors.Default;
        }

        private void ResetRemarks()
        {
            for (int i = 0; i < gv_avbl.DataRowCount; i++)
            {
                gv_avbl.SetRowCellValue(i, "Remarks", "");
            }
        }
        
        private void LockCtrl()
        {
            grpButtons1.Enabled = false;
            grpButtons2.Enabled = false;

            grpButtons3.Enabled = false;
            grpButtons4.Enabled = false;

            grpButtons5.Enabled = false;
            grpButtons6.Enabled = false;

            grpButtons7.Enabled = false;
            grpButtons8.Enabled = false;

            grpButtons9.Enabled = false;
            grpButtons10.Enabled = false;

            //grpButtons11.Enabled = false;
            grpButtons12.Enabled = false;
            grpButtons13.Enabled = false;

           
            grpGrid.Enabled = false;
        }
        
        private void UnLockCtrl()
        {
            grpButtons1.Enabled = true;
            grpButtons2.Enabled = true;
            
            grpButtons3.Enabled = true;
            grpButtons4.Enabled = true;

            grpButtons5.Enabled = true;
            grpButtons6.Enabled = true;

            grpButtons7.Enabled = true;
            grpButtons8.Enabled = true;

            grpButtons9.Enabled = true;
            grpButtons10.Enabled = true;

            grpButtons11.Enabled = true;
            grpButtons12.Enabled = true;

            grpButtons13.Enabled = true;

            grpGrid.Enabled = true;
        }

        private void btnDownTemp_Click(object sender, EventArgs e)
        {
            if (tUserList.Count == 0)
            {
                MessageBox.Show("Please Add Employeee first...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            ResetRemarks();
            LockCtrl();
            //Cursor.Current = Cursors.WaitCursor;

            for (int i = 0; i < gv_avbl.DataRowCount; i++)
            {
                string tsel = gv_avbl.GetRowCellValue(i, "SEL").ToString();
                if (!Convert.ToBoolean(tsel))
                    continue;

                string ip = gv_avbl.GetRowCellValue(i, "MachineIP").ToString();
                string ioflg = gv_avbl.GetRowCellValue(i, "IOFLG").ToString().Trim();
                gv_avbl.SetRowCellValue(i, "Remarks", "Connecting");

                clsMachine m = new clsMachine(ip, ioflg);
                string err = string.Empty;

                //try to connect
                m.Connect(out err);
                gv_avbl.SetRowCellValue(i, "Remarks", err);

                if (!string.IsNullOrEmpty(err))
                {
                    continue;
                }

                //grd_Emp.DataSource = tUserList.Select(myClass => new { myClass.UserID, myClass.UserName, myClass.err }).ToList();

                //user bulk method
                List<UserBioInfo> tempusers = new List<UserBioInfo>();
                m.DownloadTemplate(tUserList, out err, out tempusers);
                
                string allerr = "";
                foreach (UserBioInfo emp in tempusers)
                {
                    allerr +=  (emp.err.Length > 0 ? emp.UserID + emp.err : "");                  
                }

                if (string.IsNullOrEmpty(allerr.Replace(Environment.NewLine,"")))
                {
                    gv_avbl.SetRowCellValue(i, "Remarks", "Template Downloaded..");
                }
                else
                {
                    gv_avbl.SetRowCellValue(i, "Remarks", allerr);
                }

                m.DisConnect(out err);
                grd_Emp.DataSource = tempusers.Select(myClass => new { myClass.UserID, myClass.UserName, myClass.err }).ToList();
                MessageBox.Show("Completed", "Info", MessageBoxButtons.OK);
            }


            UnLockCtrl();
            Cursor.Current = Cursors.Default;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (tUserList.Count == 0)
            {
                MessageBox.Show("Please Add Employeee first...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            ResetRemarks();
            LockCtrl();
            //Cursor.Current = Cursors.WaitCursor;

            for (int i = 0; i < gv_avbl.DataRowCount; i++)
            {
                string tsel = gv_avbl.GetRowCellValue(i, "SEL").ToString();
                if (!Convert.ToBoolean(tsel))
                    continue;

                string ip = gv_avbl.GetRowCellValue(i, "MachineIP").ToString();
                string ioflg = gv_avbl.GetRowCellValue(i, "IOFLG").ToString().Trim();
                gv_avbl.SetRowCellValue(i, "Remarks", "Connecting");

                clsMachine m = new clsMachine(ip, ioflg);
                string err = string.Empty;

                //try to connect
                m.Connect(out err);
                gv_avbl.SetRowCellValue(i, "Remarks", err);

                if (!string.IsNullOrEmpty(err))
                {
                    continue;
                }

                m.EnableDevice(false);
                //user bulk method
                List<UserBioInfo> tempusers = new List<UserBioInfo>();
                m.DeleteUser(tUserList, out err, out tempusers);
                m.RefreshData();
                m.EnableDevice(true);
                //string allerr = "";
                foreach (UserBioInfo emp in tempusers)
                {
                    using (SqlConnection cn = new SqlConnection(Utils.Helper.constr))
                    {
                        using (SqlCommand cmd = new SqlCommand())
                        {
                            try
                            {
                                cn.Open();
                                cmd.Connection = cn;

                                int tmaxid = Convert.ToInt32(Utils.Helper.GetDescription("Select isnull(Max(ID),0) + 1 from MastMachineUserOperation", Utils.Helper.constr));

                                string sql = "insert into MastMachineUserOperation (ID,EmpUnqID,MachineIP,IOFLG,Operation,ReqDt,ReqBy,DoneFlg,AddDt,LastError) Values ('" + tmaxid + "','" +
                                    emp.UserID + "','" + ip + "','" + ioflg + "','DELETE',GetDate(),'" + Utils.User.GUserID + "',1,GetDate(),'Completed')";


                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();

                            }
                            catch (Exception ex)
                            {

                            }
                        }//using command
                    }//using connection
                                   
                }

                //if (string.IsNullOrEmpty(allerr.Replace(Environment.NewLine, "")))
                //{
                    gv_avbl.SetRowCellValue(i, "Remarks", "Deleted..");
                //}
                //else
                //{
                //    gv_avbl.SetRowCellValue(i, "Remarks", allerr);
                //}
                //m.RefreshData();
                m.DisConnect(out err);
                
                Application.DoEvents();

                //grd_Emp.DataSource = tempusers.Select(myClass => new { myClass.UserID, myClass.UserName, myClass.err }).ToList();

            }


            UnLockCtrl();
            //Cursor.Current = Cursors.Default;
            MessageBox.Show("Completed", "Info", MessageBoxButtons.OK);
        }

        private void btnBlock_Click(object sender, EventArgs e)
        {
            if (tUserList.Count == 0)
            {
                MessageBox.Show("Please Add Employeee first...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            ResetRemarks();
            LockCtrl();
            //Cursor.Current = Cursors.WaitCursor;

            for (int i = 0; i < gv_avbl.DataRowCount; i++)
            {
                string tsel = gv_avbl.GetRowCellValue(i, "SEL").ToString();
                if (!Convert.ToBoolean(tsel))
                    continue;

                Application.DoEvents();         

                string ip = gv_avbl.GetRowCellValue(i, "MachineIP").ToString();
                string ioflg = gv_avbl.GetRowCellValue(i, "IOFLG").ToString().Trim();
                gv_avbl.SetRowCellValue(i, "Remarks", "Connecting");

                clsMachine m = new clsMachine(ip, ioflg);
                string err = string.Empty;

                //try to connect
                m.Connect(out err);
                gv_avbl.SetRowCellValue(i, "Remarks", err);

                if (!string.IsNullOrEmpty(err))
                {
                    continue;
                }
                string allerr = "";

                foreach (UserBioInfo emp in tUserList)
                {
                              
                    m.BlockUser(emp.UserID, out err);
                    allerr += (err.Length > 0 ? emp.UserID + err + Environment.NewLine : "");
                }
                
                if (string.IsNullOrEmpty(allerr.Replace(Environment.NewLine, "")))
                {
                    gv_avbl.SetRowCellValue(i, "Remarks", "Blocked..");
                }
                else
                {
                    gv_avbl.SetRowCellValue(i, "Remarks", allerr);
                }
                //m.RefreshData();
                m.DisConnect(out err);
                //grd_Emp.DataSource = tempusers.Select(myClass => new { myClass.UserID, myClass.UserName, myClass.err }).ToList();

            }

            UnLockCtrl();
            MessageBox.Show("Completed", "Info", MessageBoxButtons.OK);
            //Cursor.Current = Cursors.Default;
        }

        private void btnUnBlock_Click(object sender, EventArgs e)
        {
            if (tUserList.Count == 0)
            {
                MessageBox.Show("Please Add Employeee first...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            ResetRemarks();
            LockCtrl();
            //Cursor.Current = Cursors.WaitCursor;

            for (int i = 0; i < gv_avbl.DataRowCount; i++)
            {
                string tsel = gv_avbl.GetRowCellValue(i, "SEL").ToString();
                if (!Convert.ToBoolean(tsel))
                    continue;

                string ip = gv_avbl.GetRowCellValue(i, "MachineIP").ToString();
                string ioflg = gv_avbl.GetRowCellValue(i, "IOFLG").ToString().Trim();
                gv_avbl.SetRowCellValue(i, "Remarks", "Connecting");

                clsMachine m = new clsMachine(ip, ioflg);
                string err = string.Empty;

                //try to connect
                m.Connect(out err);
                gv_avbl.SetRowCellValue(i, "Remarks", err);

                if (!string.IsNullOrEmpty(err))
                {
                    continue;
                }
                string allerr = "";

                foreach (UserBioInfo emp in tUserList)
                {
                    m.UnBlockUser(emp.UserID, out err);
                    allerr += (err.Length > 0 ? emp.UserID + err + Environment.NewLine : "");
                }

                if (string.IsNullOrEmpty(allerr.Replace(Environment.NewLine, "")))
                {
                    gv_avbl.SetRowCellValue(i, "Remarks", "UnBlocked..");
                }
                else
                {
                    gv_avbl.SetRowCellValue(i, "Remarks", allerr);
                }
                //m.RefreshData();
                m.DisConnect(out err);
                //grd_Emp.DataSource = tempusers.Select(myClass => new { myClass.UserID, myClass.UserName, myClass.err }).ToList();

            }

            UnLockCtrl();
            //Cursor.Current = Cursors.Default;
            MessageBox.Show("Completed", "Info", MessageBoxButtons.OK);
        }

        private void btnUnlockMaster_Click(object sender, EventArgs e)
        {
            ResetRemarks();
            
            //Cursor.Current = Cursors.WaitCursor;

            string ip = Globals.MasterMachineIP;
            string ioflg = "B";
            clsMachine m = new clsMachine(ip, ioflg);
            string err = string.Empty;
            //try to connect
            m.Connect(out err);
            if(!string.IsNullOrEmpty(err))
            {
                MessageBox.Show(err, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            m.Unlock(out err);
            if (!string.IsNullOrEmpty(err))
            {
                MessageBox.Show(err, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                MessageBox.Show("Master Machine Unlocked", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            m.DisConnect(out err);
            //Cursor.Current = Cursors.Default;
            MessageBox.Show("Completed", "Info", MessageBoxButtons.OK);
        }

        private void btnUnlock_Click(object sender, EventArgs e)
        {
            ResetRemarks();
            LockCtrl();
            //Cursor.Current = Cursors.WaitCursor;

            for (int i = 0; i < gv_avbl.DataRowCount; i++)
            {
                string tsel = gv_avbl.GetRowCellValue(i, "SEL").ToString();
                if (!Convert.ToBoolean(tsel))
                    continue;

                string ip = gv_avbl.GetRowCellValue(i, "MachineIP").ToString();
                string ioflg = gv_avbl.GetRowCellValue(i, "IOFLG").ToString().Trim();
                gv_avbl.SetRowCellValue(i, "Remarks", "Connecting");

                clsMachine m = new clsMachine(ip, ioflg);
                string err = string.Empty;

                //try to connect
                m.Connect(out err);
                gv_avbl.SetRowCellValue(i, "Remarks", err);

                if (!string.IsNullOrEmpty(err))
                {
                    continue;
                }

                m.Unlock(out err);
                
                if (string.IsNullOrEmpty(err))
                {
                    gv_avbl.SetRowCellValue(i, "Remarks", "UnBlocked..");
                }
                else
                {
                    gv_avbl.SetRowCellValue(i, "Remarks", err);
                }

                m.DisConnect(out err);
               

            }

            UnLockCtrl();
            MessageBox.Show("Completed", "Info", MessageBoxButtons.OK);
            //Cursor.Current = Cursors.Default;
        }

        private void btnUpdateRFID_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(txtEmpUnqID.Text.Trim()))
            {
                MessageBox.Show("Please Enter EmpUnqID", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if(string.IsNullOrEmpty(txtNewRFID.Text.Trim()))
            {
                MessageBox.Show("New RFID Card No is required..", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //added : 2021-03-18
            //check for avoid duplicate card no of existing active employee code
            //as per requirement where rfid card is used
            //not suitable for all version
            //Optional->based on customization
            string err;
            string tsql = "Select top 1 EmpUnqID,EmpName,CardNo from MastEmp Where Active = 1 and CardNo ='" + txtNewRFID.Text.Trim().ToString() + "' And EmpUnqID <> '" + txtEmpUnqID.Text.Trim().ToString() + "'";
            DataSet ds = Utils.Helper.GetData(tsql, Utils.Helper.constr, out err);
            bool hasRows = ds.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);
            if (hasRows)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                string allemp = dr["EmpUnqID"].ToString();
                string allname = dr["EmpName"].ToString();
                MessageBox.Show("New RFID Card No is already alloted to " + allemp + " " + allname, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            if(!Globals.GetWrkGrpRights(655,txtWrkGrpCode.Text.Trim(),txtEmpUnqID.Text.Trim()))
            {
                MessageBox.Show("You are not authorised for this kind of employee..", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (SqlConnection con = new SqlConnection(Utils.Helper.constr))
            {
                try
                {
                    con.Open();
                    string tEmpUnqID = txtEmpUnqID.Text.Trim();
                    string sql = string.Empty;

                    if (string.IsNullOrEmpty(txtOldRFID.Text.Trim()))
                    {
                        MessageBox.Show("Old RFID Detais not Found, System is Adding new RFID Card", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        sql = "Insert Into EmpBioData (EmpUnqID,MachineIP,Type,idx,MachineNo,EmpName,Pass,Privilege,Blocked,RFIDNO,AddDt,AddID)" +
                                 " Values ('" + tEmpUnqID + "' " +
                                 " ,'" + "Master" + "'" +
                                 " ,'RFID',10 " +
                                 " ,'9999'  " +
                                 " ,'' " +
                                 " ,'' " +
                                 " ,'0'" +
                                 " ,'0'" +
                                 " ,'" + txtNewRFID.Text.Trim() + "',GetDate(),'" + Utils.User.GUserID + "')";                        
                    }
                    else
                    {
                        sql = "Update EmpBioData Set RFIDNO = '" + txtNewRFID.Text.Trim() + "' where Type in ('RFID','FACE','FINGER') And " +
                            " EmpUnqID ='" + tEmpUnqID + "' and MachineIP='Master' and MachineNo = '9999'" +
                            "; Update MastEmp Set CardNo ='" + txtNewRFID.Text.Trim().ToString() + "' where EmpUnqID = '" + tEmpUnqID + "'";
                    
                    }
                    
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandText = sql;
                        cmd.CommandType = CommandType.Text;
                        cmd.ExecuteNonQuery();

                        if (txtOldRFID.Text.Trim() != txtNewRFID.Text.Trim())
                        {
                            int tmaxid = Convert.ToInt32(Utils.Helper.GetDescription("Select isnull(Max(ID),0) + 1 from MastMachineUserOperation", Utils.Helper.constr));
                            sql = "insert into MastMachineUserOperation (ID,EmpUnqID,MachineIP,IOFLG,Operation,ReqDt,ReqBy,DoneFlg,AddDt,LastError,Remarks) Values ('" + tmaxid + "','" +
                                tEmpUnqID + "','9999','B','RFID Change',GetDate(),'" + Utils.User.GUserID + "',1,GetDate(),'Completed','Changed from " + txtOldRFID.Text.Trim() + "->" + txtNewRFID.Text.Trim() + "')";


                            cmd.CommandText = sql;
                            cmd.ExecuteNonQuery();
                        }
                       

                    }
                    
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                MessageBox.Show("RFID Card No is changed..", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                con.Close();
            }

        }

        private void btnGetExcelUser_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtIPAdd1.Text.Trim()))
            {
                MessageBox.Show("Please Enter IP Address", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string ip = txtIPAdd1.Text.Trim().ToString();
            string ioflg = "B";
            clsMachine m = new clsMachine(ip, ioflg);
            string err = string.Empty;
            m.Connect(out err);
            if (!string.IsNullOrEmpty(err))
            {
                MessageBox.Show(err, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            LockCtrl();
            
            this.Cursor = Cursors.WaitCursor;

            List<UserBioInfo> tmpuser = new List<UserBioInfo>();
            m.DownloadAllUsers_QuickReport(out err, out tmpuser);

            if (tmpuser.Count > 0)
            {
                using (SqlConnection cn = new SqlConnection(Utils.Helper.constr))
                {
                    try
                    {
                        cn.Open();
                        string reqno;
                        reqno = Utils.Helper.GetDescription("Select isnull(Max(ReqNo),0) + 1 from tmp_machineusers", Utils.Helper.constr);
                        string sql = string.Empty;

                        SqlCommand cmd = new SqlCommand();

                        foreach (UserBioInfo emp in tmpuser)
                        {
                            sql = "Delete From tmp_machineusers where reqno ='" + reqno + "' And MachineIP='" + ip + "' And EmpUnqID ='" + emp.UserID + "'";
                            cmd = new SqlCommand(sql, cn);
                            cmd.ExecuteNonQuery();
                            
                            sql = "Insert Into tmp_machineusers (ReqNo,MachineIP,EmpUnqID,RFID,AMthD,AddDt,AddID )" +
                            " Values ('" + reqno + "','" + ip + "','" + emp.UserID + "','" + emp.CardNumber + "','0',GetDate(),'" + Utils.User.GUserID +  "')";

                            cmd = new SqlCommand(sql, cn);
                            cmd.ExecuteNonQuery();
                        }
                        
                        //update other info in tmp_machineusers
                        sql = "Update a " +
                              " Set a.WrkGrp = b.WrkGrp , a.EmpName = b.EmpName , a.Active = b.Active " +
                              " From tmp_machineusers a , MastEmp b " +
                              " Where a.EmpUnqID = b.EmpUnqID and ReqNo = '" + reqno + "'";

                        cmd = new SqlCommand(sql, cn);
                        cmd.ExecuteNonQuery();

                        lblReqNo.Text = reqno;

                    }
                    catch (Exception ex)
                    {
                        UnLockCtrl();
                        this.Cursor = Cursors.Default;
                        MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);                        
                        return;
                    }
                }
            }

            UnLockCtrl();
            this.Cursor = Cursors.Default;
            MessageBox.Show("Request Generated : " + lblReqNo.Text.Trim(), "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void btnDelLeftEmp_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtIPAdd2.Text.Trim()))
            {
                MessageBox.Show("Please Enter IP Address", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string ip = txtIPAdd2.Text.Trim().ToString();
            string ioflg = "B";
            clsMachine m = new clsMachine(ip, ioflg);
            string err = string.Empty;
            m.Connect(out err);
            if (!string.IsNullOrEmpty(err))
            {
                MessageBox.Show(err, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            LockCtrl();

            //this.Cursor = Cursors.WaitCursor;

            Application.DoEvents();
            m.DeleteLeftEmp_NEW(out err);
            if (!string.IsNullOrEmpty(err))
            {
                MessageBox.Show(err , "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show("Deleted Left Employee" , "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            

            UnLockCtrl();
            //this.Cursor = Cursors.Default;
            

        }

        private void btnCopyUsers_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtIPAddSrc.Text.Trim()))
            {
                MessageBox.Show("Please Enter IP Address", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrEmpty(txtIPAddDest.Text.Trim()))
            {
                MessageBox.Show("Please Enter IP Address", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if(txtIPAddSrc.Text.Trim() == txtIPAddDest.Text.Trim())
            {
                MessageBox.Show("Src And Dest IP is same please enter diff Address", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string ip = txtIPAddSrc.Text.Trim().ToString();
            string ioflg = "B";
            clsMachine m = new clsMachine(ip, ioflg);
            string err = string.Empty;
            m.Connect(out err);
            if (!string.IsNullOrEmpty(err))
            {
                MessageBox.Show(err, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            

            this.Cursor = Cursors.WaitCursor;
            LockCtrl();

            List<UserBioInfo> tmpuser = new List<UserBioInfo>();
            m.DownloadALLUsers(false, out err, out tmpuser);
            if (!string.IsNullOrEmpty(err))
            {
                UnLockCtrl();
                this.Cursor = Cursors.Default;
                MessageBox.Show("Source IP : " + err, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            m.DisConnect(out err);

            err = string.Empty;
            
            if (tmpuser.Count > 0)
            {
                ip = txtIPAddDest.Text.Trim();
                ioflg = "B";
                m = new clsMachine(ip, ioflg);
                m.Connect(out err);
                
                if(!string.IsNullOrEmpty(err)){
                    UnLockCtrl();
                    this.Cursor = Cursors.Default;
                    MessageBox.Show("Dest. IP : " + err, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string allerr = string.Empty;
                m.EnableDevice(false);
                foreach (UserBioInfo emp in tmpuser)
                {
                    if (emp.Previlege == 3)
                        continue;

                    m.Register(emp, out err);
                    
                    if(!string.IsNullOrEmpty(err))
                    {
                        allerr += emp.UserID + ":" + err ;
                    }
                }

                m.EnableDevice(true);
                m.RefreshData();
                m.DisConnect(out err);

                
                if (!string.IsNullOrEmpty(allerr))
                {
                    UnLockCtrl();
                    this.Cursor = Cursors.Default;
                    MessageBox.Show(allerr, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                
                UnLockCtrl();
                this.Cursor = Cursors.Default;
                MessageBox.Show("No Users Found", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            UnLockCtrl();
            this.Cursor = Cursors.Default;
            MessageBox.Show("Copy Users Completed..", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

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

        private void btnBulkDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cmbListMachine2.Text.Trim()))
            {
                MessageBox.Show("Please Select Machine...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (gv_Upload.DataRowCount <= 0)
            {
                MessageBox.Show("Please Enter EmpUnqID...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }



            DataTable dtMaterial = new DataTable();
            DataTable sortedDT = new DataTable();
            try
            {
                foreach (GridColumn column in gv_Upload.VisibleColumns)
                {
                    if (column.FieldName != string.Empty)
                        dtMaterial.Columns.Add(column.FieldName, column.ColumnType);
                }


                for (int i = 0; i < gv_Upload.DataRowCount; i++)
                {
                    DataRow row = dtMaterial.NewRow();

                    foreach (GridColumn column in gv_Upload.VisibleColumns)
                    {
                        row[column.FieldName] = gv_Upload.GetRowCellValue(i, column);
                    }
                    dtMaterial.Rows.Add(row);
                }

                DataView dv = dtMaterial.DefaultView;
                dv.Sort = "EmpUnqID asc";
                sortedDT = dv.ToTable();


                string ip = cmbListMachine2.Text.Trim();
                string ioflg = "B";
                string err;
                clsMachine m = new clsMachine(ip, ioflg);
                m.Connect(out err);

                if (!string.IsNullOrEmpty(err))
                {
                    MessageBox.Show(err, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //Cursor.Current = Cursors.WaitCursor;
                LockCtrl();

                foreach (DataRow dr in sortedDT.Rows)
                {
                    string tEmpUnqID = dr["EmpUnqID"].ToString();
                    if (string.IsNullOrEmpty(tEmpUnqID))
                    {
                        dr["Remarks"] = "EmpUnqID Required...";
                        continue;
                    }
                    err = string.Empty;

                    m.DeleteUser(tEmpUnqID, out err);

                    using (SqlConnection cn = new SqlConnection(Utils.Helper.constr))
                    {
                        using (SqlCommand cmd = new SqlCommand())
                        {
                            try
                            {
                                cn.Open();
                                cmd.Connection = cn;

                                int tmaxid = Convert.ToInt32(Utils.Helper.GetDescription("Select isnull(Max(ID),0) + 1 from MastMachineUserOperation", Utils.Helper.constr));

                                string sql = "insert into MastMachineUserOperation (ID,EmpUnqID,MachineIP,IOFLG,Operation,ReqDt,ReqBy,DoneFlg,AddDt,LastError) Values ('" + tmaxid + "','" +
                                    tEmpUnqID + "','" + ip + "','" + ioflg + "','BULKDELETE',GetDate(),'" + Utils.User.GUserID + "',1,GetDate(),'Completed')";


                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();

                            }
                            catch (Exception ex)
                            {

                            }
                        }//using command
                    }//using connection

                    dr["Remarks"] = (!string.IsNullOrEmpty(err) ? err : "Deleted");
                    Application.DoEvents();
                }

                //m.RefreshData();
                m.DisConnect(out err);
                //Cursor.Current = Cursors.Default;
                MessageBox.Show("file processed Successfully, please check the remarks for indivisual record status...", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            DataSet ds = new DataSet();
            ds.Tables.Add(sortedDT);
            grd_Upload.DataSource = ds;
            grd_Upload.DataMember = ds.Tables[0].TableName;
            grd_Upload.Refresh();

            UnLockCtrl();
            Cursor.Current = Cursors.Default;
        }

        private void btnSetTime_Click(object sender, EventArgs e)
        {
            ResetRemarks();
            LockCtrl();
            //Cursor.Current = Cursors.WaitCursor;

            for (int i = 0; i < gv_avbl.DataRowCount; i++)
            {
                string tsel = gv_avbl.GetRowCellValue(i, "SEL").ToString();
                if (!Convert.ToBoolean(tsel))
                    continue;

                string ip = gv_avbl.GetRowCellValue(i, "MachineIP").ToString();
                string ioflg = gv_avbl.GetRowCellValue(i, "IOFLG").ToString().Trim();
                gv_avbl.SetRowCellValue(i, "Remarks", "Connecting");

                clsMachine m = new clsMachine(ip, ioflg);
                string err = string.Empty;

                //try to connect
                m.Connect(out err);
                gv_avbl.SetRowCellValue(i, "Remarks", err);

                if (!string.IsNullOrEmpty(err))
                {
                    continue;
                }

                m.SetTime(out err);

                if (string.IsNullOrEmpty(err))
                {
                    gv_avbl.SetRowCellValue(i, "Remarks", "Time Set Completed..");
                }
                else
                {
                    gv_avbl.SetRowCellValue(i, "Remarks", err);
                }

                m.DisConnect(out err);
            }

            UnLockCtrl();
            //Cursor.Current = Cursors.Default;
            MessageBox.Show("Completed", "Info", MessageBoxButtons.OK);
        }

        private void optMachineType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txtEmpUnqID_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void frmMastUserManagement_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyData == Keys.Enter))
            {
                SelectNextControl(ActiveControl, true, true, true, true);
            }
        }

        private void grd_Emp_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.V)
            {
                string s = Clipboard.GetText();
                string[] lines = s.Split('\n');

                foreach (string line in lines)
                {
                    string cells = line.Replace("\r", "");
                    cells = cells.Replace("\t", "");

                    if (!string.IsNullOrEmpty(cells.ToString()))
                    {
                        txtEmpUnqID.Text = cells.Trim();
                        txtEmpUnqID_Validated(sender, e);

                        //if (txtEmpUnqID.Text.Trim() == string.Empty || txtEmpName.Text.Trim() == string.Empty)
                        //{

                        //    DialogResult dr = MessageBox.Show("warning! Employee not found,are you sure ?", "Question", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                        //    if (dr == System.Windows.Forms.DialogResult.No)
                        //        continue;
                        //    else if (dr == System.Windows.Forms.DialogResult.Cancel)
                        //        break;

                        //}

                        btnAddEmp_Click(sender, e);
                    }
                }

            }
        }

        private void btnDownloadAccessRecord_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtIPAddSrc2.Text.Trim()))
            {
                MessageBox.Show("Please Enter IP Address", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            string ip = txtIPAddSrc2.Text.Trim().ToString();
            string ioflg = "B";
            clsMachine m = new clsMachine(ip, ioflg);
            string err = string.Empty;
            m.Connect(out err);
            if (!string.IsNullOrEmpty(err))
            {
                MessageBox.Show(err, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            this.Cursor = Cursors.WaitCursor;
            LockCtrl();
            err = string.Empty;
            int reqno = m.GetAccessRecords(out err);

            UnLockCtrl();
            this.Cursor = Cursors.Default;

            if(string.IsNullOrEmpty(err))            
                lblAccessLogReq.Text = "Req.No : " + reqno.ToString();
            else
                MessageBox.Show(err, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);



        }

        private void btnDevInfo_Click(object sender, EventArgs e)
        {
            ResetRemarks();
            LockCtrl();
            //Cursor.Current = Cursors.WaitCursor;

            for (int i = 0; i < gv_avbl.DataRowCount; i++)
            {
                string tsel = gv_avbl.GetRowCellValue(i, "SEL").ToString();
                if (!Convert.ToBoolean(tsel))
                    continue;

                string ip = gv_avbl.GetRowCellValue(i, "MachineIP").ToString();
                string ioflg = gv_avbl.GetRowCellValue(i, "IOFLG").ToString().Trim();
                gv_avbl.SetRowCellValue(i, "Remarks", "Connecting");

                clsMachine m = new clsMachine(ip, ioflg);
                string err = string.Empty;

                //try to connect
                m.Connect(out err);
                gv_avbl.SetRowCellValue(i, "Remarks", err);

                if (!string.IsNullOrEmpty(err))
                {
                    continue;
                }

                err = "";
                if(!m.SaveDeviceData(out err))
                {
                    gv_avbl.SetRowCellValue(i, "Remarks", err);
                    continue;
                }

                if (string.IsNullOrEmpty(err))
                {
                    gv_avbl.SetRowCellValue(i, "Remarks", "Completed..");
                }
                else
                {
                    gv_avbl.SetRowCellValue(i, "Remarks", err);
                }

                m.DisConnect(out err);
            }

            UnLockCtrl();
            //Cursor.Current = Cursors.Default;
            MessageBox.Show("Completed", "Info", MessageBoxButtons.OK);
        }
        
        private void btnDeleteAllUsers_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtIPAdd2.Text.Trim()))
            {
                MessageBox.Show("Please Select Machine...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (Utils.User.GUserID == "anand")
            {
                DialogResult dr = MessageBox.Show("Are You Sure This will delete all users from machine ?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (dr == System.Windows.Forms.DialogResult.Yes)
                {
                    string ip = txtIPAdd2.Text.Trim();
                    string ioflg = "B";
                    string err;
                    clsMachine m = new clsMachine(ip, ioflg);
                    m.Connect(out err);

                    if (!string.IsNullOrEmpty(err))
                    {
                        MessageBox.Show(err, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    Cursor.Current = Cursors.WaitCursor;
                    LockCtrl();
                    m.ClearALLUserData(out err);
                    m.RefreshData();
                    m.DisConnect(out err);
                    UnLockCtrl();
                    Cursor.Current = Cursors.Default;
                }
                
                
            }
            else
            {
                MessageBox.Show("Please this function is disabled for security reason...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }

            
            
            

        }

        private void btnDownloadPhotos_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtIPAdd1.Text.Trim()))
            {
                MessageBox.Show("Please Enter MachineIP...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //MessageBox.Show("Please this function is disabled for security reason...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //return;

            string ip = txtIPAdd1.Text.Trim();
            string ioflg = "B";
            string err;
            clsMachine m = new clsMachine(ip, ioflg);
            m.Connect(out err);

            if (!string.IsNullOrEmpty(err))
            {
                MessageBox.Show(err, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Cursor.Current = Cursors.WaitCursor;
            LockCtrl();
            m.DownloadAllUsers_Photos(out err);
            m.DisConnect(out err);
            UnLockCtrl();
            Cursor.Current = Cursors.Default;
        }

        private void btnSetUserGroup_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cmbListMachine2.Text.Trim()))
            {
                MessageBox.Show("Please Select Machine...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string ip = cmbListMachine2.Text.Trim();
            string ioflg = "B";
            string err;
            clsMachine m = new clsMachine(ip, ioflg);
            m.Connect(out err);

            if (!string.IsNullOrEmpty(err))
            {
                MessageBox.Show(err, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            List<UserBioInfo> tmpuser = new List<UserBioInfo>();
            m.DownloadAllUsers_QuickReport(out err, out tmpuser);

            dt.Clear();
            dt.Columns.Clear();
            dt.Columns.Add("EmpUnqID");
            dt.Columns.Add("Remarks");
            dt.BeginLoadData();
            foreach (UserBioInfo t in tmpuser)
            {
                DataRow dr = dt.NewRow();
                dr["EmpUnqID"] = t.UserID;
                dr["Remarks"] = string.Empty;
                dt.Rows.Add(dr);
            }
            dt.EndLoadData();
            grd_Upload.DataSource = dt;
            gv_Upload.RefreshData();
            LockCtrl();
            this.Cursor = Cursors.WaitCursor;
            foreach (DataRow dr in dt.Rows)
            {
                string err2 = string.Empty;
                bool vret = m.SetUserGroup(Convert.ToInt32(dr["EmpUnqID"]), 1, out err2);
                if (vret)
                    dr["Remarks"] = "Group Set Completed";
                else
                    dr["Remarks"] = err2;


                Application.DoEvents();
            }
            m.RefreshData();
            m.DisConnect(out err);
            this.Cursor = Cursors.Default;
            UnLockCtrl();
        }

        private void btnSetUserFace_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cmbListMachine2.Text.Trim()))
            {
                MessageBox.Show("Please Select Machine...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string ip = cmbListMachine2.Text.Trim();
            string ioflg = "B";
            string err;
            clsMachine m = new clsMachine(ip, ioflg);
            m.Connect(out err);

            if (!string.IsNullOrEmpty(err))
            {
                MessageBox.Show(err, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            List<UserBioInfo> tmpuser = new List<UserBioInfo>();
            m.DownloadAllUsers_QuickReport(out err, out tmpuser);

            dt.Clear();
            dt.Columns.Clear();
            dt.Columns.Add("EmpUnqID");
            dt.Columns.Add("Remarks");
            dt.BeginLoadData();
            foreach (UserBioInfo t in tmpuser)
            {
                DataRow dr = dt.NewRow();
                dr["EmpUnqID"] = t.UserID;
                dr["Remarks"] = string.Empty;
                dt.Rows.Add(dr);
            }
            dt.EndLoadData();
            grd_Upload.DataSource = dt;
            gv_Upload.RefreshData();
            LockCtrl();
            this.Cursor = Cursors.WaitCursor;
            foreach (DataRow dr in dt.Rows)
            {
                string err2 = string.Empty;

                err = string.Empty;
                bool x = m.Register_Face(dr["EmpUnqID"].ToString(),out err);
                
                if (!x)
                {
                     dr["Remarks"] = err;
                }
                else
                {
                    dr["Remarks"] = "Registered";
                }

                Application.DoEvents();
            }
            m.RefreshData();
            m.DisConnect(out err);
            this.Cursor = Cursors.Default;
            UnLockCtrl();
        }

        private void btnSetUserAccGroup_Click(object sender, EventArgs e)
        {
            ResetRemarks();
            LockCtrl();
            Cursor.Current = Cursors.WaitCursor;

            for (int i = 0; i < gv_avbl.DataRowCount; i++)
            {
                string tsel = gv_avbl.GetRowCellValue(i, "SEL").ToString();
                if (!Convert.ToBoolean(tsel))
                    continue;

                string ip = gv_avbl.GetRowCellValue(i, "MachineIP").ToString();
                string ioflg = gv_avbl.GetRowCellValue(i, "IOFLG").ToString().Trim();
                gv_avbl.SetRowCellValue(i, "Remarks", "Connecting");

                clsMachine m = new clsMachine(ip, ioflg);
                string err = string.Empty;

                //try to connect
                m.Connect(out err);
                gv_avbl.SetRowCellValue(i, "Remarks", err);
                if (!string.IsNullOrEmpty(err))
                {
                    gv_avbl.SetRowCellValue(i, "Remarks", "Machine not connected..");
                    return;
                }
                List<UserBioInfo> tUsers = new List<UserBioInfo>();
                m.DownloadAllUsers_QuickReport(out err, out tUsers);
                if (!string.IsNullOrEmpty(err))
                {
                    gv_avbl.SetRowCellValue(i, "Remarks", err);
                    return;
                }

                foreach (UserBioInfo b in tUsers)
                {
                    string err2 = string.Empty;
                    bool vret = m.SetUserGroup(Convert.ToInt32(b.UserID), 1, out err2);
                    
                    Application.DoEvents();
                }
                m.RefreshData();
                m.DisConnect(out err);
                gv_avbl.SetRowCellValue(i, "Remarks","Group Set Complete");
            }//for loop

            UnLockCtrl();
            Cursor.Current = Cursors.Default;
            MessageBox.Show("Completed", "Info", MessageBoxButtons.OK);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            tUserList.Clear();
            grd_Emp.DataSource = tUserList.Select(myClass => new {  myClass.UserID, myClass.UserName, myClass.err }).ToList();
        }

        private void btnDelLeftUser_Click(object sender, EventArgs e)
        {
            ResetRemarks();
            LockCtrl();
            Cursor.Current = Cursors.WaitCursor;

            for (int i = 0; i < gv_avbl.DataRowCount; i++)
            {
                string tsel = gv_avbl.GetRowCellValue(i, "SEL").ToString();
                if (!Convert.ToBoolean(tsel))
                    continue;

                string ip = gv_avbl.GetRowCellValue(i, "MachineIP").ToString();
                string ioflg = gv_avbl.GetRowCellValue(i, "IOFLG").ToString().Trim();
                gv_avbl.SetRowCellValue(i, "Remarks", "Connecting");

                clsMachine m = new clsMachine(ip, ioflg);
                string err = string.Empty;

                //try to connect
                m.Connect(out err);
                gv_avbl.SetRowCellValue(i, "Remarks", err);
                if (!string.IsNullOrEmpty(err))
                {
                    gv_avbl.SetRowCellValue(i, "Remarks", "Machine not connected..");
                    return;
                }
                List<UserBioInfo> tUsers = new List<UserBioInfo>();
                m.DownloadAllUsers_QuickReport(out err, out tUsers);
                if (!string.IsNullOrEmpty(err))
                {
                    gv_avbl.SetRowCellValue(i, "Remarks", err);
                    return;
                }

                int leftcnt = 0;

                
                foreach (UserBioInfo b in tUsers)
                {
                    bool tActive = false;
                    bool tBlocked = false;
                    bool tEmpFound = false;

                    DataSet ds = Utils.Helper.GetData("Select EmpUnqID,Active,PunchingBlocked from MastEmp where Empunqid ='" + b.UserID + "'", Utils.Helper.constr, out err);

                    bool hasRows = ds.Tables.Cast<DataTable>() .Any(table => table.Rows.Count != 0);

                    if (hasRows)
                    {
                        DataRow dr = ds.Tables[0].Rows[0];
                        tActive = Convert.ToBoolean(dr["Active"]);
                        tBlocked = Convert.ToBoolean(dr["PunchingBlocked"]);
                        tEmpFound = true;
                    }
                    else
                    {
                        tActive = false;
                        tBlocked = true;
                        tEmpFound = false;
                    }

                    if (tActive == false || tBlocked == true)
                    {
                        leftcnt = leftcnt + 1;
                        m.DeleteUser(b.UserID, out err);
                    }

                    
                }

                m.RefreshData();
                m.DisConnect(out err);
                gv_avbl.SetRowCellValue(i, "Remarks", "Deleted :" + Convert.ToString(leftcnt) + ", New Count : " + (tUsers.Count - leftcnt).ToString() );

            }//for loop

            UnLockCtrl();
            Cursor.Current = Cursors.Default;
            MessageBox.Show("Completed", "Info", MessageBoxButtons.OK);
        }

        private void btnDownloadSpTemp_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtIPAdd1.Text.Trim()))
            {
                MessageBox.Show("Please Enter IP Address", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string ip = txtIPAdd1.Text.Trim().ToString();
            string ioflg = "B";
            clsMachine m = new clsMachine(ip, ioflg);
            
            string err = string.Empty;
            m.Connect(out err);
            if (!string.IsNullOrEmpty(err))
            {
                MessageBox.Show(err, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            LockCtrl();

            this.Cursor = Cursors.WaitCursor;

            List<UserBioInfo> tmpuser = new List<UserBioInfo>();
            m.DownloadAllUsers_QuickReport(out err, out tmpuser);

            

            if (tmpuser.Count > 0)
            {
                using (SqlConnection cn = new SqlConnection(Utils.Helper.constr))
                {
                    try
                    {
                        cn.Open();
                        
                        string sql = string.Empty;

                        SqlCommand cmd = new SqlCommand();

                        foreach (UserBioInfo emp in tmpuser)
                        {

                            sql = "Delete From tmp_templates where MachineIP='" + ip + "' And EmpUnqID ='" + emp.UserID + "' and tmptype ='RFID' ; " +
                            " Insert Into tmp_templates (MachineIP,EmpUnqID,tmptype,tmpidx,tmplen,template,AddDt,AddID )" +
                            " Values ('" + ip + "','" + emp.UserID + "','RFID',0,0,'" + emp.CardNumber + "',GetDate(),'" + Utils.User.GUserID + "')";

                            cmd = new SqlCommand(sql, cn);
                            cmd.ExecuteNonQuery();
                            int templen = 0; string tempate = string.Empty;
                            bool x = m.Get_UserFaceTempByIndex(emp.UserID, 50, out templen, out tempate);
                            if (x)
                            {
                                sql = "Delete From tmp_templates where MachineIP='" + ip + "' And EmpUnqID ='" + emp.UserID + "' and tmptype ='FACE' and tmpIdx ='50' ; " +
                                     " Insert Into tmp_templates (MachineIP,EmpUnqID,tmptype,tmpidx,tmplen,template,AddDt,AddID )" +
                                     " Values ('" + ip + "','" + emp.UserID + "','FACE','50','" + templen + "','" + tempate + "',GetDate(),'" + Utils.User.GUserID + "')";

                                cmd = new SqlCommand(sql, cn);
                                cmd.ExecuteNonQuery();
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        m.DisConnect(out err);
                        UnLockCtrl();
                        this.Cursor = Cursors.Default;
                        MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }
            m.DisConnect(out err);
            UnLockCtrl();
            this.Cursor = Cursors.Default;
            MessageBox.Show("template downloaded", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }
        
    }

}