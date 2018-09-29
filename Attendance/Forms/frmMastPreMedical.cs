using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using Attendance.Classes;

namespace Attendance.Forms
{
    public partial class frmMastPreMedical : Form
    {
        public string mode = "NEW";
        public string GRights = "XXXV";
        public string oldCode = "";
        public bool dupadhar = false;
        public string dupadharemp = string.Empty;

        public frmMastPreMedical()
        {
            InitializeComponent();
        }

        private void frmMastPreMedical_Load(object sender, EventArgs e)
        {
            ResetCtrl();
            GRights = Attendance.Classes.Globals.GetFormRights(this.Name);
            SetRights();
            
        }

        private string DataValidate()
        {
            string err = string.Empty;

          

            if (string.IsNullOrEmpty(txtEmpName.Text))
            {
                err = err + "Please Enter EmpName..." + Environment.NewLine;
            }

            if (string.IsNullOrEmpty(txtWrkGrpCode.Text))
            {
                err = err + "Please Enter WrkGrpCode " + Environment.NewLine;
            }

            if (string.IsNullOrEmpty(txtWrkGrpDesc.Text))
            {
                err = err + "Please Enter WrkGrp Description" + Environment.NewLine;
            }
          
            if (string.IsNullOrEmpty(txtAdharNo.Text))
            {
                err = err + "Please Enter Adhar No" + Environment.NewLine;
            }


            if(txtAdharNo.Text.Trim().ToString().Length < 12) {
                err = err + "Please Enter 12 digit Adhar No" + Environment.NewLine;
            }
        

          
            if(txtBirthDT.EditValue == null){
                err = err + "Please Enter BirthDate" + Environment.NewLine;
            }

            
            if (txtBirthDT.DateTime == DateTime.MinValue)
            {
                err = err + "Please Enter Valid Birth Date..." + Environment.NewLine;
                return err;
            }

            if (txtBirthDT.DateTime >= DateTime.Now)
            {
                err = err + "Please Enter Valid Birth Date..." + Environment.NewLine;
                return err;
            }

            int tage = Convert.ToInt32((DateTime.Now - txtBirthDT.DateTime).Days / 365);

            if (tage <= 0)
            {
                err = err + "Invalid Age Range.." + Environment.NewLine;
                return err;
            }


            //if (tage < 18 || tage > 58)
            //{
            //    MessageBox.Show("Invalid Age Range..", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //}

            //check for duplicate adharno..
            DataSet ds = new DataSet();
            string sql = "select EmpName,WrkGrp,ContCode from MastPreJoin where  " +
                " AdharNo = '" + txtAdharNo.Text.Trim() + "' and EmpName+WrkGrp+ContCode not in ('" + txtEmpName.Text.Trim() + txtWrkGrpCode.Text.Trim() + txtContCode.Text.Trim() + "')";

            ds = Utils.Helper.GetData(sql, Utils.Helper.constr);
            bool hasRows = ds.Tables.Cast<DataTable>()
                           .Any(table => table.Rows.Count != 0);

            if (hasRows)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {                      
                    dupadhar = true;
                    dupadharemp =  dr["EmpName"].ToString() + "," + dr["WrkGrp"].ToString() + "," + dr["ContCode"].ToString() ;
                }
            }
            else
            {
                dupadhar = false;
                dupadharemp = string.Empty;
            }

            
            string blacklist = Utils.Helper.GetDescription("Select AdharNo from MastEmpBlackList where BlackList = 1 and AdharNo='" + txtAdharNo.Text.Trim() + "'", Utils.Helper.constr);
            if (!string.IsNullOrEmpty(blacklist))
            {
                err = err + "This Adhar Card is Black Listed..." + Environment.NewLine;
                return err;
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

            if(mode == "NEW" && dupadhar )
            {
                string msg  ="This Adhar No is Already Registered with " + dupadharemp + Environment.NewLine
                    + " Are you sure to Insert this as New Employee ?" ;

                DialogResult qdr = MessageBox.Show(msg, "Question",MessageBoxButtons.YesNo,MessageBoxIcon.Question);

                if (qdr == DialogResult.No)
                {
                    return;
                }

            }

           
            Cursor.Current = Cursors.WaitCursor;
            GrpMain.Enabled = false;

            using (SqlConnection cn = new SqlConnection(Utils.Helper.constr))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        err = string.Empty;

                        int MaxSrNo = Convert.ToInt32(Utils.Helper.GetDescription("Select isnull(Max(SrNo),0) + 1 from MastPreJoin",Utils.Helper.constr,out err));
                        cn.Open();
                        cmd.Connection = cn;
                        string sql = "Insert into MastPreJoin (SrNo,WrkGrp,ContCode,EmpName,AdharNo,BirthDt,Age,MedChkDt,MedStatus,Remarks,AddDt,Addid) " +
                            " Values ('" + MaxSrNo.ToString() + "','" + txtWrkGrpCode.Text.Trim().ToString() + "'," +
                            " '" + txtContCode.Text.Trim().ToString() + "','" + txtEmpName.Text.Trim().ToString() + "'," +
                            " '" + txtAdharNo.Text.Trim().ToString() + "','" + txtBirthDT.DateTime.Date.ToString("yyyy-MM-dd") + "'," +
                            " '" + txtAge.Text.Trim().ToString() + "'," +
                            " " + (txtMedChkDt.DateTime == DateTime.MinValue || txtMedChkDt.EditValue == null ? "null" : "'" + txtBirthDT.DateTime.Date.ToString("yyyy-MM-dd") + "'") + "," +
                            " '" + txtMedRes.Text.Trim().ToString() + "','" + txtRemark.Text.Trim().ToString() + "',GetDate(),'" + Utils.User.GUserID + "')";

                        cmd.CommandText = sql;
                        int receff = (int)cmd.ExecuteNonQuery();
                                                
                        if(receff == 1)
                            MessageBox.Show("Record saved...", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        else
                            MessageBox.Show("Record did not saved..", "Error" ,MessageBoxButtons.OK, MessageBoxIcon.Error);
                        
                        ResetCtrl();

                    }catch(Exception ex){
                        MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            GrpMain.Enabled = true;

            Cursor.Current = Cursors.Default;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string err = DataValidate();
            
            if (!string.IsNullOrEmpty(err))
            {
                MessageBox.Show(err, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (dupadhar)
            {
                string msg = "This Adhar No is Already Registered with " + dupadharemp + Environment.NewLine
                    + " Are you sure to Update this Adhar No ?";

                DialogResult qdr = MessageBox.Show(msg, "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (qdr == DialogResult.No)
                {
                    return;
                }

            }


            GrpMain.Enabled = false;

            Cursor.Current = Cursors.WaitCursor;
            
            using (SqlConnection cn = new SqlConnection(Utils.Helper.constr))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        cn.Open();
                        cmd.Connection = cn;
                        string medchkdt = (txtMedChkDt.DateTime == DateTime.MinValue
                                            || txtMedChkDt.EditValue == null
                                            ? "null" : "'" + txtMedChkDt.DateTime.Date.ToString("yyyy-MM-dd") + "'");

                        string sql = "Update MastPreJoin set ContCode ='" + txtContCode.Text.Trim().ToString() + "',EmpName ='" + txtEmpName.Text.Trim().ToString() + "'," +
                            " BirthDt ='" + txtBirthDT.DateTime.ToString("yyyy-MM-dd") + "', Age='" + txtAge.Text.Trim().ToString() + "'," +
                            " AdharNo ='" + txtAdharNo.Text.Trim().ToString() + "',MedChkDt = " + medchkdt + ", " + 
                            " MedStatus ='" + txtMedRes.Text.Trim().ToString() + "', Remarks ='" + txtRemark.Text.Trim().ToString() + "'," +
                            " UpdDt = GetDate(),UpdID ='" + Utils.User.GUserID + "'" + 
                            "  Where SrNo = '" + txtSrNo.Text.Trim().ToString() + "'";

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
            GrpMain.Enabled = true;

            Cursor.Current = Cursors.Default;
        }

        private void ResetCtrl()
        {
            btnAdd.Enabled = false;
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;
            dupadhar = false;
            dupadharemp = string.Empty;

            txtSrNo.Text = "";
            txtEmpName.Text = "";
            txtAdharNo.Text = "";

            txtWrkGrpCode.Text = "";
            txtWrkGrpDesc.Text = "";
            txtContCode.Text = "";

            txtBirthDT.EditValue = null;
            txtAge.Text = "";
            txtMedChkDt.EditValue = null;
            txtRemark.Text = "";
            txtMedRes.Text = "";

            GrpMain.Enabled = true;
            grid.DataSource = null;
            
            oldCode = "";
            mode = "NEW";
        }

        private void SetRights()
        {
            if ( txtSrNo.Text.Trim() != "" && mode == "NEW" && GRights.Contains("A") )
            {
                btnAdd.Enabled = true;
                btnUpdate.Enabled = false;
                btnDelete.Enabled = false;
            }
            else if (txtSrNo.Text.Trim() != "" && mode == "OLD")
            {
                btnAdd.Enabled = false;

                if(GRights.Contains("U"))
                    btnUpdate.Enabled = true;
                if (GRights.Contains("D"))
                    btnDelete.Enabled = true;
            }
            else if (string.IsNullOrEmpty(txtSrNo.Text.Trim().ToString()) && mode == "NEW" && GRights.Contains("A"))
            {
                btnAdd.Enabled = true;
                btnUpdate.Enabled = false;
                btnDelete.Enabled = false;
            }

            if (GRights.Contains("XXXV"))
            {
                btnAdd.Enabled = false;
                btnUpdate.Enabled = false;
                btnDelete.Enabled = false;
            }
        }

        private void txtWrkGrpCode_KeyDown(object sender, KeyEventArgs e)
        {
          
            if (e.KeyCode == Keys.F1 || e.KeyCode == Keys.F2)
            {
                List<string> obj = new List<string>();

                Help_F1F2.ClsHelp hlp = new Help_F1F2.ClsHelp();
                string sql = "";

                sql = "Select WrkGrp,WrkGrpDesc From MastWorkGrp Where CompCode = '01'";
                if (e.KeyCode == Keys.F1)
                {
                    obj = (List<string>)hlp.Show(sql, "WrkGrp", "WrkGrp", typeof(string), Utils.Helper.constr, "System.Data.SqlClient",
                   100, 300, 400, 600, 100, 100);
                }
                
                if (obj.Count == 0)
                {                   
                    return;
                }
                else if (obj.ElementAt(0).ToString() == "0")
                {
                    return;
                }
                else if (obj.ElementAt(0).ToString() == "")
                {
                    return;
                }
                else
                {
                    txtWrkGrpCode.Text = obj.ElementAt(0).ToString();
                    txtWrkGrpDesc.Text = obj.ElementAt(1).ToString();
                    
                   
                }
            }
        }

        private void txtWrkGrpCode_Validated(object sender, EventArgs e)
        {
            

            DataSet ds = new DataSet();
            string sql = "select * From MastWorkGrp where CompCode ='01' and WrkGrp='" + txtWrkGrpCode.Text.Trim() + "'";
            
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
            
        }

       
        private void btnDelete_Click(object sender, EventArgs e)
        {
            string err = DataValidate();
            if (!string.IsNullOrEmpty(err))
            {
                MessageBox.Show(err, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            GrpMain.Enabled = false;
            Cursor.Current = Cursors.WaitCursor;
            using (SqlConnection cn = new SqlConnection(Utils.Helper.constr))
            {
                try
                {
                    cn.Open();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(err, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                
                
                try
                {
                   
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = cn;
                
                        cmd.CommandText = "Delete from MastPreJoin where SrNo = '" + txtSrNo.Text.Trim() + "'";
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Record Deleted Sucessfully...", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(err + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }

            }


            GrpMain.Enabled = true;
            Cursor.Current = Cursors.Default;
            ResetCtrl();
            SetRights();
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

        private void txtSrNo_KeyDown(object sender, KeyEventArgs e)
        {
            
            if (e.KeyCode == Keys.F1 || e.KeyCode == Keys.F2)
            {
                List<string> obj = new List<string>();

                Help_F1F2.ClsHelp hlp = new Help_F1F2.ClsHelp();
                string sql = "";


                sql = "Select SrNo,EmpName,WrkGrp,ContCode From MastPreJoin Where 1=1 ";
                if (e.KeyCode == Keys.F1)
                {
                    obj = (List<string>)hlp.Show(sql, "SrNo", "SrNo", typeof(int), Utils.Helper.constr, "System.Data.SqlClient",
                    100, 300, 400, 600, 100, 100);
                }
                else
                {
                    obj = (List<string>)hlp.Show(sql, "EmpName", "EmpName", typeof(string), Utils.Helper.constr, "System.Data.SqlClient",
                    100, 300, 400, 600, 100, 100);
                }

                if (obj.Count == 0)
                {
                    return;
                }
                else if (obj.ElementAt(0).ToString() == "0")
                {
                    return;
                }
                else if (obj.ElementAt(0).ToString() == "")
                {
                    return;
                }
                else
                {
                    txtSrNo.Text = obj.ElementAt(0).ToString();
                    txtSrNo_Validated(sender, e);
                }
            }
        }

        private void txtSrNo_Validated(object sender, EventArgs e)
        {

            string err = string.Empty;
            if (string.IsNullOrEmpty(txtSrNo.Text.Trim()) )
            {
                mode = "NEW";
                oldCode = "";
               
                int MaxSrNo = Convert.ToInt32(Utils.Helper.GetDescription("Select isnull(Max(SrNo),0) + 1 from MastPreJoin", Utils.Helper.constr, out err));
                txtSrNo.Text = MaxSrNo.ToString();
                return;
            }

            string sql = "Select Count(*) from MastPreJoin Where SrNo ='" + txtSrNo.Text.Trim().ToString() + "'";
            int cnt = Convert.ToInt32(Utils.Helper.GetDescription(sql, Utils.Helper.constr));


            if (cnt == 1)
            {
                mode = "OLD";                
                DisplayData();
            }
            else
            {
                int MaxSrNo = Convert.ToInt32(Utils.Helper.GetDescription("Select isnull(Max(SrNo),0) + 1 from MastPreJoin", Utils.Helper.constr, out err));
                txtSrNo.Text = MaxSrNo.ToString();
                mode = "NEW";
            }

            SetRights();
        }

        private void DisplayData()
        {
            string sql = "Select * from MastPreJoin Where SrNo ='" + txtSrNo.Text.Trim().ToString() + "'";

            DataSet ds = Utils.Helper.GetData(sql, Utils.Helper.constr);
             bool hasrow = ds.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);
             if (hasrow)
             {
                 DataRow dr = ds.Tables[0].Rows[0];

                 txtEmpName.Text = dr["EmpName"].ToString();
                 txtAdharNo.Text = dr["AdharNo"].ToString();

                 txtWrkGrpCode.Text = dr["WrkGrp"].ToString();
                 txtContCode.Text = dr["ContCode"].ToString();

                 txtBirthDT.EditValue = dr["BirthDt"];
                 txtAge.Text = dr["Age"].ToString();
                 
                 txtMedChkDt.EditValue = dr["MedChkDt"];
                 txtMedRes.Text = dr["MedStatus"].ToString();
                 txtRemark.Text = dr["Remarks"].ToString();
                 
                 mode = "OLD";
                 object sender = new object(); EventArgs evt = new EventArgs();
                 txtAdharNo_Validated(sender, evt);
                 txtWrkGrpCode_Validated(sender, evt);
             }
             else
             {
                 mode = "NEW";
             }

             SetRights();
            
        }

        private void txtContCode_KeyDown(object sender, KeyEventArgs e)
        {
            
            if (e.KeyCode == Keys.F1 || e.KeyCode == Keys.F2)
            {
                List<string> obj = new List<string>();

                Help_F1F2.ClsHelp hlp = new Help_F1F2.ClsHelp();
                string sql = "";


                sql = "Select distinct ContCode,ContName From MastCont Where CompCode ='01' ";
                if (e.KeyCode == Keys.F1)
                {
                    obj = (List<string>)hlp.Show(sql, "ContCode", "ContCode", typeof(string), Utils.Helper.constr, "System.Data.SqlClient",
                   100, 300, 400, 600, 100, 100);
                }

                if (obj.Count == 0)
                {

                    return;
                }
                else if (obj.ElementAt(0).ToString() == "0")
                {
                    return;
                }
                else if (obj.ElementAt(0).ToString() == "")
                {
                    return;
                }
                else
                {
                    txtContCode.Text = obj.ElementAt(0).ToString();
                }
            }
        }

        private void txtContCode_Validated(object sender, EventArgs e)
        {
            
            
            DataSet ds = new DataSet();
            string sql = "select top 1 * From MastCont where ContCode ='" + txtContCode.Text.Trim() + "' ";
                    

            ds = Utils.Helper.GetData(sql, Utils.Helper.constr);
            bool hasRows = ds.Tables.Cast<DataTable>()
                           .Any(table => table.Rows.Count != 0);

            if (hasRows)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    txtContCode.Text = dr["ContCode"].ToString();

                }
            }
        }

        private void frmMastPreMedical_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyData == Keys.Enter))
            {
                SelectNextControl(ActiveControl, true, true, true, true);
            }
        }

        private void txtBirthDT_Validated(object sender, EventArgs e)
        {
            if (txtBirthDT.DateTime == DateTime.MinValue || txtBirthDT.EditValue == null)
            {
                txtAge.Text = "0";

            }
            else
            {
                int tage = Convert.ToInt32((DateTime.Now - txtBirthDT.DateTime).Days/365);
                txtAge.Text = tage.ToString();

                if (tage < 18 || tage > 58)
                {
                    MessageBox.Show("Invalid Age Range..", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void txtAdharNo_Validated(object sender, EventArgs e)
        {
            LoadGrid();
        }

        private void LoadGrid()
        {
            string sql = "Select SrNo,EmpName,WrkGrp,ContCode,AdharNo,BirthDt,Age,MedChkDt,MedStatus,AddDt,AddId,UpdDt,UpdID From MastPreJoin Where AdharNo = '" + txtAdharNo.Text.Trim().ToString() + "'";
            DataSet ds = Utils.Helper.GetData(sql, Utils.Helper.constr);

            ds = Utils.Helper.GetData(sql, Utils.Helper.constr);

            Boolean hasRows = ds.Tables.Cast<DataTable>()
                           .Any(table => table.Rows.Count != 0);

            if (hasRows)
            {
                grid.DataSource = ds;
                grid.DataMember = ds.Tables[0].TableName;
            }
            else
            {
                grid.DataSource = null;
            }

        }

    }
}
