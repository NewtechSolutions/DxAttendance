using Attendance.Classes;
using ConnectUNCWithCredentials;
using DevExpress.XtraBars;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Attendance.Forms
{
    public partial class frmMain : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void mnuMastUnit_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form t = Application.OpenForms["frmMastUnit"];

            if (t == null)
            {
                Attendance.Forms.frmMastUnit m = new Attendance.Forms.frmMastUnit();
                m.MdiParent = this;
                m.Show();
            }
        }

        private void mnuMastComp_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form t = Application.OpenForms["frmMastComp"];

            if (t == null)
            {
                Attendance.Forms.frmMastComp m = new Attendance.Forms.frmMastComp();
                m.MdiParent = this;
                m.Show();
            }
        }

        private void mnuMastDept_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form t = Application.OpenForms["frmMastDept"];

            if (t == null)
            {
                Attendance.Forms.frmMastDept m = new Attendance.Forms.frmMastDept();
                m.MdiParent = this;
                m.Show();
            }
        }

        private void mnuMastStat_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form t = Application.OpenForms["frmMastStat"];

            if (t == null)
            {
                Attendance.Forms.frmMastStat m = new Attendance.Forms.frmMastStat();
                m.MdiParent = this;
                m.Show();
            }
        }

        private void mnuMastWrkGrp_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form t = Application.OpenForms["frmMastWrkGrp"];

            if (t == null)
            {
                Attendance.Forms.frmMastWrkGrp m = new Attendance.Forms.frmMastWrkGrp();
                m.MdiParent = this;
                m.Show();
            }
        }

        private void mnuMastCat_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form t = Application.OpenForms["frmMastCat"];

            if (t == null)
            {
                Attendance.Forms.frmMastCat m = new Attendance.Forms.frmMastCat();
                m.MdiParent = this;
                m.Show();
            }
        }

        private void mnuMastGrade_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form t = Application.OpenForms["frmMastGrade"];

            if (t == null)
            {
                Attendance.Forms.frmMastGrade m = new Attendance.Forms.frmMastGrade();
                m.MdiParent = this;
                m.Show();
            }
        }

        private void mnuMastDesg_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form t = Application.OpenForms["frmMastDesg"];

            if (t == null)
            {
                Attendance.Forms.frmMastDesg m = new Attendance.Forms.frmMastDesg();
                m.MdiParent = this;
                m.Show();
            }
        }

        private void mnuMastCont_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form t = Application.OpenForms["frmMastCont"];

            if (t == null)
            {
                Attendance.Forms.frmMastCont m = new Attendance.Forms.frmMastCont();
                m.MdiParent = this;
                m.Show();
            }
        }

        private void mnuMastCostCode_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form t = Application.OpenForms["frmMastCostCode"];

            if (t == null)
            {
                Attendance.Forms.frmMastCostCode m = new Attendance.Forms.frmMastCostCode();
                m.MdiParent = this;
                m.Show();
            }
        }

        private void barButtonExit_ItemClick(object sender, ItemClickEventArgs e)
        {
            Utils.User.GUserID = string.Empty;
            Utils.User.GUserName = string.Empty;
            Utils.User.GUserPass = string.Empty;
            Utils.User.IsAdmin = false;

            Program.OpenMDIFormOnClose = false;
            this.Hide();
            Application.Restart();
        }

        private void mnuDBConn_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form t = Application.OpenForms["FrmConnection"];

            if (t == null)
            {
                FrmConnection m = new FrmConnection();
                m.MdiParent = this;
                m.typeofcon = "DBCON";
                m.Show();
            }
        }

        private void mnuDomainConfig_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form t = Application.OpenForms["frmDomainConfig"];

            if (t == null)
            {
                Attendance.Forms.frmDomainConfig m = new Attendance.Forms.frmDomainConfig();
                m.MdiParent = this;

                m.Show();
            }
        }

        private void mnuConfig_KeyVal_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form t = Application.OpenForms["frmMastConfigKeys"];
            if (t == null)
            {
                Attendance.Forms.frmMastConfigKeys m = new Attendance.Forms.frmMastConfigKeys();
                m.MdiParent = this;
                m.Show();
            }
        }

        private void mnuMastShift_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form t = Application.OpenForms["frmMastShift"];

            if (t == null)
            {
                Attendance.Forms.frmMastShift m = new Attendance.Forms.frmMastShift();
                m.MdiParent = this;
                m.Show();
            }
        }

        private void mnuMastLeave_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form t = Application.OpenForms["frmMastLeave"];

            if (t == null)
            {
                Attendance.Forms.frmMastLeave m = new Attendance.Forms.frmMastLeave();
                m.MdiParent = this;
                m.Show();
            }
        }

        private void mnuUserRights_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form t = Application.OpenForms["frmUserRights"];

            if (t == null)
            {
                Attendance.Forms.frmUserRights m = new Attendance.Forms.frmUserRights();
                m.MdiParent = this;
                m.Show();
            }
        }

        private void mnuOtherConfig_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form t = Application.OpenForms["frmMastOtherConfig"];

            if (t == null)
            {
                Attendance.Forms.frmMastOtherConfig m = new Attendance.Forms.frmMastOtherConfig();
                m.MdiParent = this;
                m.Show();
            }
        }

        private void mnuEmpMast_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form t = Application.OpenForms["frmMastEmpBasicData"];

            if (t == null)
            {
                Attendance.Forms.frmMastEmpBasicData m = new Attendance.Forms.frmMastEmpBasicData();
                m.MdiParent = this;
                m.Show();
            }
        }

        private void mnuCreateMuster_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form t = Application.OpenForms["frmMastEmpCreateMuster"];

            if (t == null)
            {
                Attendance.Forms.frmMastEmpCreateMuster m = new Attendance.Forms.frmMastEmpCreateMuster();
                m.MdiParent = this;
                m.Show();
            }
        }

        private void mnuImportEmp_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form t = Application.OpenForms["frmMastEmpBulkEmployeeImport"];

            if (t == null)
            {
                Attendance.Forms.frmMastEmpBulkEmployeeImport m = new Attendance.Forms.frmMastEmpBulkEmployeeImport();
                m.MdiParent = this;
                m.Show();
            }
        }

        private void mnuEmpBulkChange_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form t = Application.OpenForms["frmMastEmpBulkChange"];

            if (t == null)
            {
                Attendance.Forms.frmMastEmpBulkChange m = new Attendance.Forms.frmMastEmpBulkChange();
                m.MdiParent = this;
                m.Show();
            }
        }

        private void mnuMastJob_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form t = Application.OpenForms["frmMastEmpJobProfile"];

            if (t == null)
            {
                Attendance.Forms.frmMastEmpJobProfile m = new Attendance.Forms.frmMastEmpJobProfile();
                m.MdiParent = this;
                m.Show();
            }
        }

        private void mnuMastEmpPer_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form t = Application.OpenForms["frmMastEmpPerInfo"];

            if (t == null)
            {
                Attendance.Forms.frmMastEmpPerInfo m = new Attendance.Forms.frmMastEmpPerInfo();
                m.MdiParent = this;
                m.Show();
            }
        }

        private void mnuBulkEmpAddress_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form t = Application.OpenForms["frmMastEmpBulkAddress"];

            if (t == null)
            {
                Attendance.Forms.frmMastEmpBulkAddress m = new Attendance.Forms.frmMastEmpBulkAddress();
                m.MdiParent = this;
                m.Show();
            }
        }

        private void mnuBulkEmpIdentity_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form t = Application.OpenForms["frmMastEmpBulkIdentity"];

            if (t == null)
            {
                Attendance.Forms.frmMastEmpBulkIdentity m = new Attendance.Forms.frmMastEmpBulkIdentity();
                m.MdiParent = this;
                m.Show();
            }
        }

        private void mnuMastHoliday_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form t = Application.OpenForms["frmMastHoliday"];

            if (t == null)
            {
                Attendance.Forms.frmMastHoliday m = new Attendance.Forms.frmMastHoliday();
                m.MdiParent = this;
                m.Show();
            }
        }

        private void mnuMastHolidayOpt_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form t = Application.OpenForms["frmMastHolidayOpt"];

            if (t == null)
            {
                Attendance.Forms.frmMastHolidayOpt m = new Attendance.Forms.frmMastHolidayOpt();
                m.MdiParent = this;
                m.Show();
            }
        }

        private void mnuLeaveBalEntry_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form t = Application.OpenForms["frmMastEmpLeaveBalEntry"];

            if (t == null)
            {
                Attendance.Forms.frmMastEmpLeaveBalEntry m = new Attendance.Forms.frmMastEmpLeaveBalEntry();
                m.MdiParent = this;
                m.Show();
            }
        }

        private void mnuLeaveBalUpload_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form t = Application.OpenForms["frmMastEmpBulkLeaveBal"];

            if (t == null)
            {
                Attendance.Forms.frmMastEmpBulkLeaveBal m = new Attendance.Forms.frmMastEmpBulkLeaveBal();
                m.MdiParent = this;
                m.Show();
            }
        }

        private void mnuTranLeaveEntry_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form t = Application.OpenForms["frmTranLeaveEntry"];

            if (t == null)
            {
                Attendance.Forms.frmTranLeaveEntry m = new Attendance.Forms.frmTranLeaveEntry();
                m.MdiParent = this;
                m.Show();
            }
        }

        private void mnuMastLeaveRules_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form t = Application.OpenForms["frmMastLeaveRules"];

            if (t == null)
            {
                Attendance.Forms.frmMastLeaveRules m = new Attendance.Forms.frmMastLeaveRules();
                m.MdiParent = this;
                m.Show();
            }
        }

        private void mnuTranLeaveEncash_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form t = Application.OpenForms["frmTranLeaveEncash"];

            if (t == null)
            {
                Attendance.Forms.frmTranLeaveEncash m = new Attendance.Forms.frmTranLeaveEncash();
                m.MdiParent = this;
                m.Show();
            }
        }

        private void mnuDataProcess_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form t = Application.OpenForms["frmTranDataProcess"];

            if (t == null)
            {
                Attendance.Forms.frmTranDataProcess m = new Attendance.Forms.frmTranDataProcess();
                m.MdiParent = this;
                m.ProcessName = "ATTD";
                m.Show();
            }
        }

        private void mnuManualSan_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form t = Application.OpenForms["frmTranSanction"];

            if (t == null)
            {
                Attendance.Forms.frmTranSanction m = new Attendance.Forms.frmTranSanction();
                m.MdiParent = this;
               
                m.Show();
            }
        }

        public static Boolean IsNetworkPath(String path)
        {

            try
            {
                Uri uri = new Uri(path);
                if (uri.IsUnc)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return true;
            }



        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            //Set GateInOutIP
            Globals.SetGateInOutIPList();

            //set LunchInOutIP
            Globals.SetLunchInOutIPList();

            //set waterip
            //Globals.SetWaterIPList();

            //set ShiftList
            Globals.SetShiftList();

            Globals.SetMasterIPList();
            //set global vars
            Globals.GetGlobalVars();

            //get localmodification date
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string localfile = Uri.UnescapeDataString(uri.Path);

            //disable all menu items
            foreach (BarItem item in ribbon.Items)
            {
                if (!(item is BarButtonItem))
                    continue;

                item.Enabled = false;
            }



            barButtonExit.Enabled = true;

            //here we can start Quartz if host is server
            if (Utils.User.GUserID == "SERVER")
            {
                Globals.G_myscheduler = new Scheduler();

                Form t = Application.OpenForms["frmServerStatus"];

                if (t == null)
                {
                    Attendance.Forms.frmServerStatus m = new Attendance.Forms.frmServerStatus();
                    m.MdiParent = this;
                    m.WindowState = FormWindowState.Maximized;
                    m.Show();
                }

                Thread.Sleep(TimeSpan.FromSeconds(5));

                Globals.G_myscheduler.Start();
                //create triggers
                Globals.G_myscheduler.RegSchedule_AutoTimeSet();
                Globals.G_myscheduler.RegSchedule_WorkerProcess();
                Globals.G_myscheduler.RegSchedule_AutoArrival();
                Globals.G_myscheduler.RegSchedule_AutoProcess();
                Globals.G_myscheduler.RegSchedule_DownloadPunch();
                //Globals.G_myscheduler.RegSchedule_BlockUnBlockProcess();
                //Globals.G_myscheduler.RegSchedule_GatePassPunchProcess();
                //Globals.G_myscheduler.RegSchedule_WDMSPunchTransferProcess();
            }
            else
            {
                //enable only user's have rights != "XXXV"

                DataSet ds = Utils.Helper.GetData("Select * from V_UserRights where UserID='" + Utils.User.GUserID + "'", Utils.Helper.constr);

                bool hasRows = ds.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);

                if (hasRows)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        bool uadd = Convert.ToBoolean(dr["Add1"]);
                        bool uupd = Convert.ToBoolean(dr["Update1"]);
                        if(uadd && uupd)
                        {
                            foreach (BarItem item in ribbon.Items)
                            {
                                if (!(item is BarButtonItem))
                                    continue;
                                if(item.Name == dr["MenuName"].ToString())
                                    item.Enabled = true;
                            }
                        }
                        
                    }
                }
                        /**** temp close
                         *  //check for update version.
                        DateTime servermodified = new DateTime();
                        DateTime localmodified = new DateTime();

                        if (!string.IsNullOrEmpty(Globals.G_UpdateChkPath))
                        {
                            this.Cursor = Cursors.WaitCursor;
                            Application.DoEvents();

                            using (UNCAccessWithCredentials unc = new UNCAccessWithCredentials())
                            {
                                if (unc.NetUseWithCredentials(Globals.G_UpdateChkPath,
                                                              Globals.G_NetworkUser,
                                                              Globals.G_NetworkDomain,
                                                              Globals.G_NetworkPass))
                                {
                                    string fullpath = Path.Combine(Globals.G_UpdateChkPath, "Attendance.exe");
                                    if (File.Exists(fullpath))
                                    {
                                        servermodified = File.GetLastWriteTime(fullpath);
                                    }
                                }
                            }

                            localmodified = File.GetLastWriteTime(localfile);
                            if (servermodified > localmodified)
                            {
                                MessageBox.Show("New Upgrade is available, please update", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                this.Close();
                            }

                            if (servermodified != DateTime.MinValue && localmodified != DateTime.MinValue)
                            {
                                if (localmodified < servermodified)
                                {
                                    MessageBox.Show("New Upgrade is available, please update", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    this.Close();
                                }
                            }


                            this.Cursor = Cursors.Default;
                        }
                        */
                    }

        }

        private void mnuBulkSan_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form t = Application.OpenForms["frmTranBulkSanction"];

            if (t == null)
            {
                Attendance.Forms.frmTranBulkSanction m = new Attendance.Forms.frmTranBulkSanction();
                m.MdiParent = this;

                m.Show();
            }
        }

        private void mnuReaderConfig_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form t = Application.OpenForms["frmReaderConfig"];

            if (t == null)
            {
                Attendance.Forms.frmReaderConfig m = new Attendance.Forms.frmReaderConfig();
                m.MdiParent = this;

                m.Show();
            }
        }

        private void mnuBulkLeavePost_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form t = Application.OpenForms["frmTranBulkLeavePost"];

            if (t == null)
            {
                Attendance.Forms.frmTranBulkLeavePost m = new Attendance.Forms.frmTranBulkLeavePost();
                m.MdiParent = this;

                m.Show();
            }
        }

        private void mnuDataDownload_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form t = Application.OpenForms["frmDataDownload"];

            if (t == null)
            {
                Attendance.Forms.frmDataDownload m = new Attendance.Forms.frmDataDownload();
                m.MdiParent = this;

                m.Show();
            }
        }

        private void mnuMachineManagement_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form t = Application.OpenForms["frmMastUserManagement"];

            if (t == null)
            {
                Attendance.Forms.frmMastUserManagement m = new Attendance.Forms.frmMastUserManagement();
                m.MdiParent = this;

                m.Show();
            }
        }

        private void mnuWeekoffSan_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form t = Application.OpenForms["frmBulkWOChange"];

            if (t == null)
            {
                Attendance.Forms.frmBulkWOChange m = new Attendance.Forms.frmBulkWOChange();
                m.MdiParent = this;

                m.Show();
            }
        }

        private void mnuShiftSchUpload_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form t = Application.OpenForms["frmTranShiftScheduleUpload"];

            if (t == null)
            {
                Attendance.Forms.frmTranShiftScheduleUpload m = new Attendance.Forms.frmTranShiftScheduleUpload();
                m.MdiParent = this;
                m.Show();
            }
        }
    }
}