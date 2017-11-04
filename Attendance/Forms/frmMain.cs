using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraBars.Helpers;
using DevExpress.Skins;
using DevExpress.LookAndFeel;
using DevExpress.UserSkins;


namespace Attendance
{
    public partial class frmMain : XtraForm
    {
        public static string cnstr = Utils.Helper.constr;
        public frmMain()
        {
            InitializeComponent();
            stsUserID.Text = Utils.User.GUserID;
            stsUserDesc.Text = Utils.User.GUserName;
        }

        private void mnuUserRights_Click(object sender, EventArgs e)
        {
           
            Form t = Application.OpenForms["frmUserRights"];

            if (t == null)
            {
                Attendance.Forms.frmUserRights m = new Attendance.Forms.frmUserRights();
                m.MdiParent = this;
                m.Show();
            }

        }

        private void mnuLogOff_Click(object sender, EventArgs e)
        {
            Utils.User.GUserID = string.Empty;
            Utils.User.GUserName = string.Empty;
            Utils.User.GUserPass = string.Empty;
            Utils.User.IsAdmin = false;


            Program.OpenMDIFormOnClose = false;
            this.Hide();
            Application.Restart();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {

            ToolStripItemCollection tmnu = menuStrip1.Items;
            SetToolStripItems(tmnu);

            mnuAdmin.Enabled = true;
            mnuConfig.Enabled = true;
            mnuMast.Enabled = true;        
            mnuProfile.Enabled = true;
            mnuEmployee.Enabled = true;
            mnuMess.Enabled = true;
            mnuTranS.Enabled = true;
            mnuLeave.Enabled = true;        
            mnuShift.Enabled = true;
            mnuSanction.Enabled = true;
            mnuData.Enabled = true;
            mnuCostCent.Enabled = true;
            mnuChangePass.Enabled = true;
            mnuLogOff.Enabled = true;

            DataSet ds = new DataSet();
            string sql = "select menuname from  MastFrm where formid in (select FormId from userRights where UserId ='" + Utils.User.GUserID + "' and View1=1) order by seqid";
            ds = Utils.Helper.GetData(sql,cnstr);
            
            mnuUser.Enabled = true;
            Boolean hasRows = ds.Tables.Cast<DataTable>()
                           .Any(table => table.Rows.Count != 0);
                
            
            if (hasRows)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string mnu = dr["menuname"].ToString();

                    ToolStripItem[] t = tmnu.Find(mnu, true);

                    foreach (ToolStripItem ti in t)
                    {
                        ti.Enabled = true;
                    }

                }    
            }

            //Set GateInOutIP
            Attendance.Classes.Globals.SetGateInOutIPList();

            //set LunchInOutIP
            Attendance.Classes.Globals.SetLunchInOutIPList();

            //set waterip
            Attendance.Classes.Globals.SetWaterIPList();

            //set ShiftList
            Attendance.Classes.Globals.SetShiftList();


        }
        
        private void SetToolStripItems(ToolStripItemCollection dropDownItems)
        {
            try
            {
                foreach (object obj in dropDownItems)
                //for each object.
                {
                    ToolStripMenuItem subMenu = obj as ToolStripMenuItem;
                    //Try cast to ToolStripMenuItem as it could be toolstrip separator as well.

                    if (subMenu != null)
                    //if we get the desired object type.
                    {
                        if (subMenu.HasDropDownItems) // if subMenu has children
                        {
                            SetToolStripItems(subMenu.DropDownItems); // Call recursive Method.
                        }
                        else // Do the desired operations here.
                        {
                            subMenu.Enabled = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "SetToolStripItems",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void mnuChangePass_Click(object sender, EventArgs e)
        {   
            Form t = Application.OpenForms["frmChangePass"];

            if (t == null)
            {
                Attendance.Forms.frmChangePass m = new Attendance.Forms.frmChangePass();
                m.MdiParent = this;
                m.Show();
            }

        }

        private void mnuDomainConfig_Click(object sender, EventArgs e)
        {
           
            Form t = Application.OpenForms["frmDomainConfig"];

            if (t == null)
            {
                Attendance.Forms.frmDomainConfig m = new Attendance.Forms.frmDomainConfig();
                m.MdiParent = this;
               
                m.Show();
            }

        }

        private void mnuDBConn_Click(object sender, EventArgs e)
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

        private void mnuMastWrkGrp_Click(object sender, EventArgs e)
        {
            
            Form t = Application.OpenForms["frmMastWrkGrp"];

            if (t == null)
            {
                Attendance.Forms.frmMastWrkGrp m = new Attendance.Forms.frmMastWrkGrp();
                m.MdiParent = this;                
                m.Show();
            }

        }

        private void mnuMastUnit_Click(object sender, EventArgs e)
        {
            Form t = Application.OpenForms["frmMastUnit"];

            if (t == null)
            {
                Attendance.Forms.frmMastUnit m = new Attendance.Forms.frmMastUnit();
                m.MdiParent = this;
                m.Show();
            }
        }

        private void mnuMastDept_Click(object sender, EventArgs e)
        {
            Form t = Application.OpenForms["frmMastDept"];

            if (t == null)
            {
                Attendance.Forms.frmMastDept m = new Attendance.Forms.frmMastDept();
                m.MdiParent = this;
                m.Show();
            }
        }

        private void mnuMastCat_Click(object sender, EventArgs e)
        {
            Form t = Application.OpenForms["frmMastCat"];

            if (t == null)
            {
                Attendance.Forms.frmMastCat m = new Attendance.Forms.frmMastCat();
                m.MdiParent = this;
                m.Show();
            }
        }

        private void mnuMastDesg_Click(object sender, EventArgs e)
        {
            Form t = Application.OpenForms["frmMastDesg"];

            if (t == null)
            {
                Attendance.Forms.frmMastDesg m = new Attendance.Forms.frmMastDesg();
                m.MdiParent = this;
                m.Show();
            }
        }

        private void mnuMastGrade_Click(object sender, EventArgs e)
        {
            Form t = Application.OpenForms["frmMastGrade"];

            if (t == null)
            {
                Attendance.Forms.frmMastGrade m = new Attendance.Forms.frmMastGrade();
                m.MdiParent = this;
                m.Show();
            }
        }

        private void mnuMastEmpType_Click(object sender, EventArgs e)
        {
            Form t = Application.OpenForms["frmMastEmpType"];

            if (t == null)
            {
                Attendance.Forms.frmMastEmpType m = new Attendance.Forms.frmMastEmpType();
                m.MdiParent = this;
                m.Show();
            }
        }

        private void mnuMastComp_Click(object sender, EventArgs e)
        {
            Form t = Application.OpenForms["frmMastComp"];

            if (t == null)
            {
                Attendance.Forms.frmMastComp m = new Attendance.Forms.frmMastComp();
                m.MdiParent = this;
                m.Show();
            }
        }

        private void mnuMastCont_Click(object sender, EventArgs e)
        {
            Form t = Application.OpenForms["frmMastCont"];

            if (t == null)
            {
                Attendance.Forms.frmMastCont m = new Attendance.Forms.frmMastCont();
                m.MdiParent = this;
                m.Show();
            }
        }

        private void mnuMessConfig_Click(object sender, EventArgs e)
        {
            Form t = Application.OpenForms["frmMastMess"];

            if (t == null)
            {
                Attendance.Forms.frmMastMess m = new Attendance.Forms.frmMastMess();
                m.MdiParent = this;
                m.Show();
            }
        }

        private void mnuMastFood_Click(object sender, EventArgs e)
        {
            Form t = Application.OpenForms["frmMastMessFood"];

            if (t == null)
            {
                Attendance.Forms.frmMastMessFood m = new Attendance.Forms.frmMastMessFood();
                m.MdiParent = this;
                m.Show();
            }
        }

        private void mnuMastMessGrp_Click(object sender, EventArgs e)
        {
            Form t = Application.OpenForms["frmMastMessGrp"];

            if (t == null)
            {
                Attendance.Forms.frmMastMessGrp m = new Attendance.Forms.frmMastMessGrp();
                m.MdiParent = this;
                m.Show();
            }
        }

        private void mnuMastRate_Click(object sender, EventArgs e)
        {
            Form t = Application.OpenForms["frmMastMessRate"];

            if (t == null)
            {
                Attendance.Forms.frmMastMessRate m = new Attendance.Forms.frmMastMessRate();
                m.MdiParent = this;
                m.Show();
            }
        }

        private void mnuConfig_Click(object sender, EventArgs e)
        {

        }

        private void mnuMastTime_Click(object sender, EventArgs e)
        {
            Form t = Application.OpenForms["frmMastMessTime"];

            if (t == null)
            {
                Attendance.Forms.frmMastMessTime m = new Attendance.Forms.frmMastMessTime();
                m.MdiParent = this;
                m.Show();
            }
        }

        private void MnuReaderConfig_Click(object sender, EventArgs e)
        {
            Form t = Application.OpenForms["frmReaderConfig"];

            if (t == null)
            {
                Attendance.Forms.frmReaderConfig m = new Attendance.Forms.frmReaderConfig();
                m.MdiParent = this;
                m.Show();
            }
        }

        private void mnuReaderMessAsign_Click(object sender, EventArgs e)
        {
            Form t = Application.OpenForms["frmReaderConfigMess"];

            if (t == null)
            {
                Attendance.Forms.frmReaderConfigMess m = new Attendance.Forms.frmReaderConfigMess();
                m.MdiParent = this;
                m.Show();
            }
        }

        private void mnuMastLeave_Click(object sender, EventArgs e)
        {
            Form t = Application.OpenForms["frmMastLeave"];

            if (t == null)
            {
                Attendance.Forms.frmMastLeave m = new Attendance.Forms.frmMastLeave();
                m.MdiParent = this;
                m.Show();
            }
        }

        private void mnuMastShift_Click(object sender, EventArgs e)
        {
            Form t = Application.OpenForms["frmMastShift"];

            if (t == null)
            {
                Attendance.Forms.frmMastShift m = new Attendance.Forms.frmMastShift();
                m.MdiParent = this;
                m.Show();
            }
        }

        private void mnuMastRules_Click(object sender, EventArgs e)
        {
            Form t = Application.OpenForms["frmMastRules"];

            if (t == null)
            {
                Attendance.Forms.frmMastRules m = new Attendance.Forms.frmMastRules();
                m.MdiParent = this;
                m.Show();
            }
        }

        private void mnuMastHoliday_Click(object sender, EventArgs e)
        {
            Form t = Application.OpenForms["frmMastHoliday"];

            if (t == null)
            {
                Attendance.Forms.frmMastHoliday m = new Attendance.Forms.frmMastHoliday();
                m.MdiParent = this;
                m.Show();
            }
        }

        private void mnuCostCodeMast_Click(object sender, EventArgs e)
        {
            Form t = Application.OpenForms["frmMastCostCode"];

            if (t == null)
            {
                Attendance.Forms.frmMastCostCode m = new Attendance.Forms.frmMastCostCode();
                m.MdiParent = this;
                m.Show();
            }
        }

        private void mnuCostCodeManPowerProcess_Click(object sender, EventArgs e)
        {
            Form t = Application.OpenForms["frmMastCostCodeProcess"];

            if (t == null)
            {
                Attendance.Forms.frmMastCostCodeProcess m = new Attendance.Forms.frmMastCostCodeProcess();
                m.MdiParent = this;
                m.Show();
            }
        }

        private void mnuEmpCostCode_Click(object sender, EventArgs e)
        {
            Form t = Application.OpenForms["frmMastCostCodeEmp"];

            if (t == null)
            {
                Attendance.Forms.frmMastCostCodeEmp m = new Attendance.Forms.frmMastCostCodeEmp();
                m.MdiParent = this;
                m.Show();
            }
        }

        private void mnuCostCodeSanManPower_Click(object sender, EventArgs e)
        {
            Form t = Application.OpenForms["frmMastCostCodeSanManPower"];

            if (t == null)
            {
                Attendance.Forms.frmMastCostCodeSanManPower m = new Attendance.Forms.frmMastCostCodeSanManPower();
                m.MdiParent = this;
                m.Show();
            }
        }

        private void mnuEmpCostCodeBulk_Click(object sender, EventArgs e)
        {
            Form t = Application.OpenForms["frmBulkCostCodeUpdate"];

            if (t == null)
            {
                Attendance.Forms.frmBulkCostCodeUpdate m = new Attendance.Forms.frmBulkCostCodeUpdate();
                m.MdiParent = this;
                m.Show();
            }
        }

        private void mnuValidityMass_Click(object sender, EventArgs e)
        {
            Form t = Application.OpenForms["frmValidityExtend"];

            if (t == null)
            {
                Attendance.Forms.frmValidityExtend m = new Attendance.Forms.frmValidityExtend();
                m.MdiParent = this;
                m.Show();
            }
        }

        private void mnuMastStat_Click(object sender, EventArgs e)
        {
            Form t = Application.OpenForms["frmMastStat"];

            if (t == null)
            {
                Attendance.Forms.frmMastStat m = new Attendance.Forms.frmMastStat();
                m.MdiParent = this;
                m.Show();
            }
        }

        private void mnuCreateMuster_Click(object sender, EventArgs e)
        {
            Form t = Application.OpenForms["frmCreateMuster"];

            if (t == null)
            {
                Attendance.Forms.frmCreateMuster m = new Attendance.Forms.frmCreateMuster();
                m.MdiParent = this;
                m.Show();
            }
        }

        private void mnuUserSpRights_Click(object sender, EventArgs e)
        {
            Form t = Application.OpenForms["frmUserSpRights"];
            if (t == null)
            {
                Attendance.Forms.frmUserSpRights m = new Attendance.Forms.frmUserSpRights();
                m.MdiParent = this;
                m.Show();
            }
        }

        private void mnuUserDSRights_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Not Implemented....", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void mnuUserEmpRights_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Not Implemented....", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        

    }
}