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
    public partial class frmMastEmpBasicData : Form
    {
        public string mode = "NEW";
        public string GRights = "XXXV";
        public string oldCode = "";
        public bool dupadhar = false;
        public string dupadharemp = string.Empty;
       
        public frmMastEmpBasicData()
        {
            InitializeComponent();
        }

        private void frmMastWrkGrp_Load(object sender, EventArgs e)
        {
            ResetCtrl();
            GRights = Attendance.Classes.Globals.GetFormRights(this.Name);
            SetRights();
            
        }

       
        private string DataValidate()
        {
            string err = string.Empty;

            if (string.IsNullOrEmpty(txtCompCode.Text))
            {
                err = err + "Please Enter CompCode " + Environment.NewLine;
            }

            if (string.IsNullOrEmpty(txtCompName.Text))
            {
                err = err + "Please Enter CompName..." + Environment.NewLine;
            }


            if (string.IsNullOrEmpty(txtEmpUnqID.Text))
            {
                err = err + "Please Enter EmpUnqID " + Environment.NewLine;
            }

            if (string.IsNullOrEmpty(txtEmpName.Text))
            {
                err = err + "Please Enter EmpName..." + Environment.NewLine;
            }

            


            if (string.IsNullOrEmpty(txtFatherName.Text))
            {
                err = err + "Please Enter FatherName..." + Environment.NewLine;
            }
            
            if (string.IsNullOrEmpty(txtWrkGrpCode.Text))
            {
                err = err + "Please Enter WrkGrpCode " + Environment.NewLine;
            }

            if (string.IsNullOrEmpty(txtWrkGrpDesc.Text))
            {
                err = err + "Please Enter WrkGrp Description" + Environment.NewLine;
            }
           

            if(txtJoinDt.EditValue == null){
                err = err + "Please Enter JoinDate" + Environment.NewLine;
            }

            if(txtBirthDT.EditValue == null){
                err = err + "Please Enter BirthDate" + Environment.NewLine;
            }

            
            

            if (txtJoinDt.DateTime == DateTime.MinValue)
            {
                err = err + "Please Enter Valid Join Date..." + Environment.NewLine;
                return err;
            }

            if (txtBirthDT.DateTime == DateTime.MinValue)
            {
                err = err + "Please Enter Valid Birth Date..." + Environment.NewLine;
                return err;
            }

            if (txtJoinDt.DateTime < txtBirthDT.DateTime)
            {
                err = err + "Birth Date must be less than JoinDate Date..." + Environment.NewLine;
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

            

            Cursor.Current = Cursors.WaitCursor;
            GrpMain.Enabled = false;

            using (SqlConnection cn = new SqlConnection(Utils.Helper.constr))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cn.Open();
                    SqlTransaction tr = cn.BeginTransaction();

                    try
                    {                       
                        cmd.Connection = cn;
                        cmd.Transaction = tr;

                        string sql = "Insert into MastEmp (CompCode,WrkGrp,EmpUnqID,EmpName,FatherName," +
                            " BirthDt,JoinDt,Gender,Active,AddDt,AddID,ContactNo,EmailID,CardNo) Values (" +
                            "'{0}','{1}','{2}','{3}','{4}' ," +
                            " '{5}','{6}','{7}','1',GetDate(),'{8}','{9}','{10}','{11}')";
 
                        sql = string.Format(sql, txtCompCode.Text.Trim().ToString(), txtWrkGrpCode.Text.Trim().ToString(),txtEmpUnqID.Text.Trim().ToString(),txtEmpName.Text.Trim().ToString(),txtFatherName.Text.Trim(),                           
                            txtBirthDT.DateTime.ToString("yyyy-MM-dd"),txtJoinDt.DateTime.ToString("yyyy-MM-dd"), txtGender.EditValue.ToString(), 
                            Utils.User.GUserID, txtContactNo.Text.Trim().ToString(), txtEmail.Text.Trim().ToString(),txtCardNo.Text.Trim().ToString()
                            );

                        cmd.CommandText = sql;
                        cmd.ExecuteNonQuery();



                        //create jobprofile default from joindt
                        sql = "Insert into MastEmpJobProfile ( EmpUnqID,ValidFrom,UnitCode,DeptCode,StatCode,DesgCode,GradeCode,CatCode,WeekOff,ContCode,OTFLG,CostCode, AddDt,AddID ) Values " +
                            "('" + txtEmpUnqID.Text.Trim().ToString() + "','" + txtJoinDt.DateTime.ToString("yyyy-MM-dd") + "', '','','','','','','SUN','','0','',getdate(),'" + Utils.User.GUserID + "')";
                        cmd.CommandText = sql;
                        cmd.ExecuteNonQuery();


                        sql = "Insert into MastEmpIdentity ( EmpUnqID, AddDt,AddID ) Values " +
                            "('" + txtEmpUnqID.Text.Trim().ToString() + "',getdate(),'" + Utils.User.GUserID + "')";
                        cmd.CommandText = sql;
                        cmd.ExecuteNonQuery();

                        sql = "Insert into MastEmpAddress ( EmpUnqID,AddressType, AddDt,AddID ) Values " +
                            " select '" + txtEmpUnqID.Text.Trim().ToString() + "','PER',getdate(),'" + Utils.User.GUserID + "' union " +
                            " select '" + txtEmpUnqID.Text.Trim().ToString() + "','PRE',getdate(),'" + Utils.User.GUserID + "' ";
                        cmd.CommandText = sql;
                        cmd.ExecuteNonQuery();



                        tr.Commit();
                        MessageBox.Show("Record saved...", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        
                        ResetCtrl();

                    }catch(Exception ex){
                        tr.Rollback();
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

                        string sql = "insert into MastEmpHistory  (Remarks,ActionDT,CompCode,EmpUnqID,WrkGrp,EmpName,FatherName,Gender,BirthDt,ContactNo," +
                           " EmailID,Cardno,PunchingBlocked,LeftDT,JoinDT,Active,AddDt,AddID,UpdDt,UpdID,DelFlg) " +
                           " select 'Before Update Master Data, Action By " + Utils.User.GUserID + "', GetDate()," +
                           " CompCode, EmpUnqID, WrkGrp, EmpName, FatherName, Gender, BirthDt, ContactNo," +                           
                           " EmailID,Cardno,PunchingBlocked,LeftDT,JoinDT,Active,AddDt,AddID,UpdDt,UpdID,DelFlg " +
                           " from MastEmp where CompCode = '" + txtCompCode.Text.Trim() + "' " +
                           " and EmpUnqID ='" + txtEmpUnqID.Text.Trim() + "'";
                        
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = sql;
                        cmd.ExecuteNonQuery();



                        
                        sql = "Update MastEmp set WrkGrp ='{0}',EmpName='{1}',FatherName = '{2}'," +
                            " BirthDt ='{3}',JoinDt ='{4}',Gender='{5}',UpdDt=GetDate(),UpdID ='{6}'," +
                            " ContactNo='{7}',EmailID='{8}',CardNo='{9}',LeftDt={10},Active='{11}' " +
                            "  Where " +
                            " CompCode ='{11}' and EmpUnqID = '{12}'";


                        sql = string.Format(sql,  txtWrkGrpCode.Text.Trim().ToString(), txtEmpName.Text.Trim().ToString(), txtFatherName.Text.Trim(),
                          
                            txtBirthDT.DateTime.ToString("yyyy-MM-dd"), txtJoinDt.DateTime.ToString("yyyy-MM-dd"),txtGender.EditValue.ToString(),
                            txtContactNo.Text.Trim().ToString(), txtEmail.Text.Trim().ToString(),
                            txtCardNo.Text.Trim().ToString(), Utils.User.GUserID,
                            (txtLeftDt.EditValue != null ? "'" + txtLeftDt.DateTime.Date.ToString("yyyy-MM-dd") + "'" : "null"),
                            (txtLeftDt.EditValue != null ? "0" : "1"),
                            //where
                            txtCompCode.Text.Trim(),
                            txtEmpUnqID.Text.Trim()
                            
                            );

                        cmd.CommandText = sql;
                        cmd.ExecuteNonQuery();


                        //if (WrkGrpChange )
                        //{
                        //    MessageBox.Show("Employee Job Profile is Discarded.." + Environment.NewLine +
                        //            "Please Fill the Employee Job Profile Again.."                                
                        //        , "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        //}

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
                     
            //txtCompCode.Text = "01";
            //txtCompName.Text = "";
            txtWrkGrpCode.Text = "";
            txtWrkGrpDesc.Text = "";
            txtEmpUnqID.Text = "";
            txtEmpName.Text = "";
            txtFatherName.Text = "";
            txtContactNo.Text = "";
            txtEmail.Text = "";
            txtCardNo.Text = "";

            txtBirthDT.EditValue = null;
            txtJoinDt.EditValue = null;
           
            txtGender.EditValue = "M";
            chkActive.Checked = false;
            GrpMain.Enabled = true;

            oldCode = "";
            mode = "NEW";
        }

        private void SetRights()
        {
            if ( txtEmpUnqID.Text.Trim() != "" && mode == "NEW" && GRights.Contains("A") )
            {
                btnAdd.Enabled = true;
                btnUpdate.Enabled = false;
                btnDelete.Enabled = false;
            }
            else if (txtEmpUnqID.Text.Trim() != "" && mode == "OLD")
            {
                btnAdd.Enabled = false;

                if(GRights.Contains("U"))
                    btnUpdate.Enabled = true;
                if (GRights.Contains("D"))
                    btnDelete.Enabled = true;
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
            if (txtCompCode.Text.Trim() == "")
                return;
            
            if (e.KeyCode == Keys.F1 || e.KeyCode == Keys.F2)
            {
                List<string> obj = new List<string>();

                Help_F1F2.ClsHelp hlp = new Help_F1F2.ClsHelp();
                string sql = "";


                sql = "Select WrkGrp,WrkGrpDesc From MastWorkGrp Where CompCode ='" + txtCompCode.Text.Trim() + "'";
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
                    
                    mode = "OLD";
                }
            }
        }

        private void txtWrkGrpCode_Validated(object sender, EventArgs e)
        {
            if (txtCompCode.Text.Trim() == "" || txtCompName.Text.Trim() == "")
            {
                
                return;
            }

            DataSet ds = new DataSet();
            string sql = "select * From MastWorkGrp where CompCode ='" + txtCompCode.Text.Trim() + "' and WrkGrp='" + txtWrkGrpCode.Text.Trim() + "'";
            
            ds = Utils.Helper.GetData(sql, Utils.Helper.constr);
            bool hasRows = ds.Tables.Cast<DataTable>()
                           .Any(table => table.Rows.Count != 0);

            if (hasRows)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    txtCompCode.Text = dr["CompCode"].ToString();
                    txtWrkGrpCode.Text = dr["WrkGrp"].ToString();
                    txtWrkGrpDesc.Text = dr["WrkGrpDesc"].ToString();
                    
                    txtCompCode_Validated(sender,e);
                    
                }
            }
            
        }

        private void txtCompCode_Validated(object sender, EventArgs e)
        {
            if (txtCompCode.Text.Trim() == "")
            {   
                return;
            }

            DataSet ds = new DataSet();
            string sql = "select * from MastComp where CompCode ='" + txtCompCode.Text.Trim() + "'";

            ds = Utils.Helper.GetData(sql, Utils.Helper.constr);
            bool hasRows = ds.Tables.Cast<DataTable>()
                           .Any(table => table.Rows.Count != 0);

            if (hasRows)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    txtCompCode.Text = dr["CompCode"].ToString();
                    txtCompName.Text = dr["CompName"].ToString();        

                }
            }
            else
            {
                txtCompName.Text = "";
            }
            
        }

        private void txtCompCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1 )
            {
                List<string> obj = new List<string>();

                Help_F1F2.ClsHelp hlp = new Help_F1F2.ClsHelp();
                string sql = "";


                sql = "Select CompCode,CompName From MastComp Where 1 = 1";
                if (e.KeyCode == Keys.F1)
                {

                    obj = (List<string>)hlp.Show(sql, "CompCode", "CompCode", typeof(string), Utils.Helper.constr, "System.Data.SqlClient",
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

                    txtCompCode.Text = obj.ElementAt(0).ToString();
                    txtCompName.Text = obj.ElementAt(1).ToString();

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
                
                
                SqlTransaction tr = cn.BeginTransaction("DeleteEmp");
                
                
                try
                {
                   
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = cn;
                        cmd.Transaction = tr;

                        string sql = "insert into MastEmpHistory  (Remarks,ActionDT,CompCode,EmpUnqID,WrkGrp,EmpName,FatherName,Gender,BirthDt,ContactNo," +
                           " EmailID,Cardno,PunchingBlocked,LeftDT,JoinDT,Active,AddDt,AddID,UpdDt,UpdID,DelFlg) " +
                           " select 'Before Delete Master Data, Action By " + Utils.User.GUserID + "', GetDate()," +
                           " CompCode, EmpUnqID, WrkGrp, EmpName, FatherName, Gender, BirthDt, ContactNo," +
                           " EmailID,Cardno,PunchingBlocked,LeftDT,JoinDT,Active,AddDt,AddID,UpdDt,UpdID,DelFlg " +
                           " from MastEmp where CompCode = '" + txtCompCode.Text.Trim() + "' " +
                           " and EmpUnqID ='" + txtEmpUnqID.Text.Trim() + "'";

                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = sql;
                        cmd.ExecuteNonQuery();

                        //cmd.CommandText = "Delete from AttdData where EmpUnqID = '" + txtEmpUnqID.Text.Trim() + "'";
                        //cmd.ExecuteNonQuery();

                        //cmd.CommandText = "Delete from MastEmpBio where EmpUnqID = '" + txtEmpUnqID.Text.Trim() + "'";
                        //cmd.ExecuteNonQuery();

                        //cmd.CommandText = "Delete from LeaveEntry where EmpUnqID = '" + txtEmpUnqID.Text.Trim() + "'";
                        //cmd.ExecuteNonQuery();

                        //cmd.CommandText = "Delete from MastEmpFamily  where EmpUnqID = '" + txtEmpUnqID.Text.Trim() + "'";
                        //cmd.ExecuteNonQuery();

                        //cmd.CommandText = "Delete from MastEmpExp  where EmpUnqID = '" + txtEmpUnqID.Text.Trim() + "'";
                        //cmd.ExecuteNonQuery();

                        //cmd.CommandText = "Delete from MastEmpEDU  where EmpUnqID = '" + txtEmpUnqID.Text.Trim() + "'";
                        //cmd.ExecuteNonQuery();

                        //cmd.CommandText = "Delete from MastEmpPPE  where EmpUnqID = '" + txtEmpUnqID.Text.Trim() + "'";
                        //cmd.ExecuteNonQuery();

                        //cmd.CommandText = "Delete from MastLeaveSchedule  where EmpUnqID = '" + txtEmpUnqID.Text.Trim() + "'";
                        //cmd.ExecuteNonQuery();


                        //cmd.CommandText = "Delete from MastShiftSchedule  where EmpUnqID = '" + txtEmpUnqID.Text.Trim() + "'";
                        //cmd.ExecuteNonQuery();

                        //cmd.CommandText = "Delete from ATTDLOG  where EmpUnqID = '" + txtEmpUnqID.Text.Trim() + "'";
                        //cmd.ExecuteNonQuery();

                        //cmd.CommandText = "Delete from LeaveBal  where EmpUnqID = '" + txtEmpUnqID.Text.Trim() + "'";
                        //cmd.ExecuteNonQuery();

                        cmd.CommandText = "Delete from MastEmp  where EmpUnqID = '" + txtEmpUnqID.Text.Trim() + "'";
                        cmd.ExecuteNonQuery();

                        tr.Commit();

                        MessageBox.Show("Record Deleted Sucessfull...", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    tr.Rollback();

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

        private void txtEmpUnqID_KeyDown(object sender, KeyEventArgs e)
        {
            if (txtCompCode.Text.Trim() == "")
                return;

            if (e.KeyCode == Keys.F1 || e.KeyCode == Keys.F2)
            {
                List<string> obj = new List<string>();

                Help_F1F2.ClsHelp hlp = new Help_F1F2.ClsHelp();
                string sql = "";


                sql = "Select EmpUnqID,EmpName,WrkGrp,CompCode From MastEmp Where CompCode ='" + txtCompCode.Text.Trim() + "' ";
                if (e.KeyCode == Keys.F1)
                {
                    obj = (List<string>)hlp.Show(sql, "EmpUnqID", "EmpUnqID", typeof(string), Utils.Helper.constr, "System.Data.SqlClient",
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

                    txtCompCode.Text = obj.ElementAt(3).ToString();
                    txtEmpUnqID.Text = obj.ElementAt(0).ToString();
                    txtEmpUnqID_Validated(sender, e);
                }
            }
        }

        private void txtEmpUnqID_Validated(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtEmpUnqID.Text.Trim()) || string.IsNullOrEmpty(txtCompCode.Text.Trim()))
            {
                mode = "NEW";
                oldCode = "";
                return;
            }

            clsEmp t = new clsEmp();
            
            t.EmpUnqID = txtEmpUnqID.Text.Trim();
            bool isold = t.GetEmpDetails( t.EmpUnqID);

            if (isold)
            {
                
                DisplayData(t);
            }
            else
            {
                mode = "NEW";
                oldCode = "";
            }
            SetRights();
        }

        private void DisplayData(clsEmp cEmp)
        {

            txtEmpName.Text = cEmp.EmpName;
            txtFatherName.Text = cEmp.FatherName;
            txtGender.EditValue = cEmp.Gender;
            chkActive.Checked = cEmp.Active;
           
            txtWrkGrpCode.Text = cEmp.WrkGrp;
            txtWrkGrpDesc.Text = cEmp.WrkGrpDesc;

            txtContactNo.Text = cEmp.ContactNo;
            txtEmail.Text = cEmp.Email;
            txtCardNo.Text = cEmp.CardNo;

            txtLeftDt.EditValue = cEmp.LeftDt;
            txtJoinDt.EditValue = cEmp.JoinDt;
            txtBirthDT.EditValue = cEmp.BirthDt;
            
            mode = "OLD";
            oldCode = cEmp.EmpUnqID;
        }



        private void frmMastEmpBasicData_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyData == Keys.Enter))
            {
                SelectNextControl(ActiveControl, true, true, true, true);
            }
        }

        

    }
}
