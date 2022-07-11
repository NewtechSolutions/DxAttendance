using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Attendance.Forms
{
    public partial class frmMastEmpPerInfo : Form
    {
        public string mode = "NEW";
        public string GRights = "XXXV";
        public string oldCode = "";

        public frmMastEmpPerInfo()
        {
            InitializeComponent();
            this.ctrlEmp1.EmpUnqIDValidated += new EventHandler(this.ctrlEmpValidateEvent_Handler);
            //this.ctrlEmp1.CompCodeValidated += new EventHandler(this.ctrlCompValidateEvent_Handler);
        }

        private void ctrlEmpValidateEvent_Handler(object sender, EventArgs e)
        {
            if (!ctrlEmp1.cEmp.Active)
            {
               
                mode = "New";
                oldCode = "";
                ResetCtrl();
            }
            else
            {
                mode = "OLD";
                oldCode = ctrlEmp1.cEmp.EmpUnqID;
                DisplayData(ctrlEmp1.txtCompCode.Text.Trim() ,oldCode);
                //LoadGrid();
                SetRights();
            }
        }

        

        private void frmMastEmpPerInfo_Load(object sender, EventArgs e)
        {
            ResetCtrl();
            GRights = Attendance.Classes.Globals.GetFormRights(this.Name);
            SetRights();

        }

        private string DataValidate()
        {
            string err = string.Empty;

            if (string.IsNullOrEmpty(ctrlEmp1.txtCompCode.Text.Trim().ToString()))
            {
                err = err + "Please Enter CompCode..." + Environment.NewLine;
            }

            if (string.IsNullOrEmpty(ctrlEmp1.txtEmpUnqID.Text.Trim().ToString()))
            {
                err = err + "Please Enter EmpUnqID..." + Environment.NewLine;
            }

            if (!string.IsNullOrEmpty(ctrlEmp1.cEmp.EmpUnqID) && !ctrlEmp1.IsValid)
            {
                err = err + "Invalid/InActive EmpUnqID..." + Environment.NewLine;
            }


            if (string.IsNullOrEmpty(ctrlEmp1.cEmp.CompDesc.Trim().ToString()))
            {
                err = err + "Invalid CompCode..." + Environment.NewLine;
            }

           

            return err;
        }

        private void ResetCtrl()
        {
            btnAdd.Enabled = false;
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;

            ctrlEmp1.ResetCtrl();


            txtAdd1.Text = "";
            txtAdd2.Text = "";
            txtAdd3.Text = "";
            txtAdd4.Text = "";
            txtCity.Text = "";
            txtDistrict.Text = "";
            txtState.Text = "";
            txtPinCode.Text = "";
            txtPhone.Text = "";
          

            txtpAdd1.Text = "";
            txtpAdd2.Text = "";
            txtpAdd3.Text = "";
            txtpAdd4.Text = "";
            txtpCity.Text = "";
            txtpDistrict.Text = "";
            txtpState.Text = "";
            txtpPinCode.Text = "";
            txtpPhone.Text = "";

            txtBankAc.Text = "";
            txtBankBranch.Text = "";
            txtBankIFSC.Text = "";
            txtBankName.Text = "";

            txtAdharNo.Text = "";
            txtDRVNo.Text = "";
            txtPassportNo.Text = "";
            txtElectionNo.Text = "";
            txtPANNo.Text = "";
            txtPFNo.Text = "";
            txtUANNo.Text = "";



            oldCode = "";
            mode = "NEW";
        }

        private void SetRights()
        {
            btnAdd.Enabled = false;
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;
            
            if (ctrlEmp1.txtEmpUnqID.Text.Trim() != "" && mode == "NEW" && GRights.Contains("A"))
            {
                btnAdd.Enabled = true;
                btnUpdate.Enabled = false;
                btnDelete.Enabled = false;             
            }
            
            if (ctrlEmp1.txtEmpUnqID.Text.Trim() != "" && mode == "OLD" && (GRights.Contains("U") || GRights.Contains("D")))
            {
                btnAdd.Enabled = false;

                if (GRights.Contains("U"))
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

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string err = DataValidate();
            if (!string.IsNullOrEmpty(err))
            {
                MessageBox.Show(err, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //identity
            string tEmpUnqID = ctrlEmp1.txtEmpUnqID.Text.Trim();

            string sql = "Select count(*) from MastEmpIdentity where EmpUnqID = '" + tEmpUnqID + "'";
            string strcnt = Utils.Helper.GetDescription(sql, Utils.Helper.constr);
            if (Convert.ToInt32(strcnt) > 0)
            {
                btnUpdate_Click(sender, e);
                return;
            }
            else
            {
                sql = "Insert into MastEmpIdentity ( EmpUnqid," +
                    " [Passport],[AdharCard],[ElectionCard],[PanCard],[DrivingLicense],[PFAccountNo],[UANNO],[BankName],[BankAccountNo] " +
                    ",[BankBranchCode],[BankIFSCCode],[AddDt],[AddID] ) Values " +
                    " ('" + tEmpUnqID + "','" + txtPassportNo.Text.Trim().ToString() + "'," +
                    "'" + txtAdharNo.Text.Trim().ToString() + "'," +
                    "'" + txtElectionNo.Text.Trim().ToString() + "'," +
                    "'" + txtPANNo.Text.Trim().ToString() + "'," +
                    "'" + txtDRVNo.Text.Trim().ToString() + "'," +
                    "'" + txtPFNo.Text.Trim().ToString() + "'," +
                    "'" + txtUANNo.Text.Trim().ToString() + "'," +
                    "'" + txtBankName.Text.Trim().ToString() + "'," +
                    "'" + txtBankAc.Text.Trim().ToString() + "'," +
                    "'" + txtBankBranch.Text.Trim().ToString() + "'," +
                    "'" + txtBankIFSC.Text.Trim().ToString() + "',GetDate(),'" + Utils.User.GUserID + "')";

                using (SqlConnection cn = new SqlConnection(Utils.Helper.constr))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        try
                        {
                            cn.Open();
                            cmd.Connection = cn;
                            cmd.CommandText = sql;
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
            }


            //present address
            sql = "Select count(*) from MastEmpAddress where EmpUnqID = '" + tEmpUnqID + "' and AddressType='PRE'";
            strcnt = Utils.Helper.GetDescription(sql, Utils.Helper.constr);
            if (Convert.ToInt32(strcnt) > 0)
            {
                btnUpdate_Click(sender, e);
                return;
            }
            else
            {
                sql = "Insert into MastEmpAddress ( EmpUnqid,AddressType" +
                    " Add1,Add2,Add3,Add4,City,District,StateName,Pincode,Phone,AddDt,AddId) Values " +
                    " ('" + tEmpUnqID + "','PRE','" + txtAdd1.Text.Trim().ToString() + "'," +
                    "'" + txtAdd2.Text.Trim().ToString() + "'," +
                    "'" + txtAdd3.Text.Trim().ToString() + "'," +
                    "'" + txtAdd4.Text.Trim().ToString() + "'," +
                    "'" + txtCity.Text.Trim().ToString() + "'," +
                    "'" + txtDistrict.Text.Trim().ToString() + "'," +
                    "'" + txtState.Text.Trim().ToString() + "'," +
                    "'" + txtPinCode.Text.Trim().ToString() + "'," +
                    "'" + txtPhone.Text.Trim().ToString() + "',GetDate(),'" + Utils.User.GUserID + "')";

                using (SqlConnection cn = new SqlConnection(Utils.Helper.constr))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        try
                        {
                            cn.Open();
                            cmd.Connection = cn;
                            cmd.CommandText = sql;
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
            }

            //permenant address
            sql = "Select count(*) from MastEmpAddress where EmpUnqID = '" + tEmpUnqID + "' and AddressType='PER'";
            strcnt = Utils.Helper.GetDescription(sql, Utils.Helper.constr);
            if (Convert.ToInt32(strcnt) > 0)
            {
                btnUpdate_Click(sender, e);
            }
            else
            {
                sql = "Insert into MastEmpAddress ( EmpUnqid,AddressType," +
                    " Add1,Add2,Add3,Add4,City,District,StateName,Pincode,Phone,AddDt,AddId) Values " +
                    " ('" + tEmpUnqID + "','PER','" + txtAdd1.Text.Trim().ToString() + "'," +
                    "'" + txtAdd2.Text.Trim().ToString() + "'," +
                    "'" + txtAdd3.Text.Trim().ToString() + "'," +
                    "'" + txtAdd4.Text.Trim().ToString() + "'," +
                    "'" + txtCity.Text.Trim().ToString() + "'," +
                    "'" + txtDistrict.Text.Trim().ToString() + "'," +
                    "'" + txtState.Text.Trim().ToString() + "'," +
                    "'" + txtPinCode.Text.Trim().ToString() + "'," +
                    "'" + txtPhone.Text.Trim().ToString() + "',GetDate(),'" + Utils.User.GUserID + "')";

                using (SqlConnection cn = new SqlConnection(Utils.Helper.constr))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        try
                        {
                            cn.Open();
                            cmd.Connection = cn;
                            cmd.CommandText = sql;
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }

            }

            //ResetCtrl();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string err = DataValidate();
            if (!string.IsNullOrEmpty(err))
            {
                MessageBox.Show(err, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }



            using (SqlConnection cn = new SqlConnection(Utils.Helper.constr))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        cn.Open();
                        cmd.Connection = cn;
                        string sql = string.Empty;


                        sql = "Update MastEmpAddress set Add1='" + txtAdd1.Text.Trim().ToString() + "'," +
                               " Add2='" + txtAdd2.Text.Trim().ToString() + "'," +
                               " Add3='" + txtAdd3.Text.Trim().ToString() + "'," +
                               " Add4='" + txtAdd4.Text.Trim().ToString() + "'," +
                               " City='" + txtCity.Text.Trim().ToString() + "'," +
                               " District='" + txtDistrict.Text.Trim().ToString() + "'," +
                               " StateName='" + txtState.Text.Trim().ToString() + "'," +
                               " PinCode='" + txtPinCode.Text.Trim().ToString() + "'," +
                               " Phone='" + txtPhone.Text.Trim().ToString() + "'," +
                               " UpdDt=GetDate() , UpdID='" + Utils.User.GUserID + "' " +
                               " Where EmpUnqID='" + ctrlEmp1.txtEmpUnqID.Text.Trim() + "' and AddressType='PRE'";

                        cmd.CommandText = sql;
                        cmd.ExecuteNonQuery();

                        sql = "Update MastEmpAddress set Add1='" + txtpAdd1.Text.Trim().ToString() + "'," +
                               " Add2='" + txtpAdd2.Text.Trim().ToString() + "'," +
                               " Add3='" + txtpAdd3.Text.Trim().ToString() + "'," +
                               " Add4='" + txtpAdd4.Text.Trim().ToString() + "'," +
                               " City='" + txtpCity.Text.Trim().ToString() + "'," +
                               " District='" + txtpDistrict.Text.Trim().ToString() + "'," +
                               " StateName='" + txtpState.Text.Trim().ToString() + "'," +
                               " PinCode='" + txtpPinCode.Text.Trim().ToString() + "'," +
                               " Phone='" + txtpPhone.Text.Trim().ToString() + "'," +
                               " UpdDt=GetDate() , UpdID='" + Utils.User.GUserID + "' " +
                               " Where EmpUnqID='" + ctrlEmp1.txtEmpUnqID.Text.Trim() + "' and AddressType='PER'";

                        cmd.CommandText = sql;
                        cmd.ExecuteNonQuery();

                        sql = "Update MastEmpIdentity set " +
                            " Passport ='" + txtPassportNo.Text.Trim().ToString() + "'," +
                            " AdharCard='" + txtAdharNo.Text.Trim().ToString() + "'," +
                            " ElectionCard ='" + txtElectionNo.Text.Trim().ToString() + "'," +
                            " PanCard='" + txtPANNo.Text.Trim().ToString() + "'," +
                            " DrivingLicense='" + txtDRVNo.Text.Trim().ToString() + "'," +
                            " PFAccountNo ='" + txtPFNo.Text.Trim().ToString() + "'," +
                            " UANNO='" + txtUANNo.Text.Trim().ToString() + "'," +
                            " BankName='" + txtBankName.Text.Trim().ToString() + "'," +
                            " BankAccountNo='" + txtBankAc.Text.Trim().ToString() + "'," +
                            " BankBranchCode='" + txtBankBranch.Text.Trim().ToString() + "'," +
                            " BankIFSCCode='" + txtBankIFSC.Text.Trim().ToString() + "' " +
                            " UpdDt=GetDate(), UpdID='" + Utils.User.GUserID + "'" +
                            " Where EmpUnqID='" + ctrlEmp1.txtEmpUnqID.Text.Trim() + "'";

                        cmd.CommandText = sql;
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Record saved...", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ResetCtrl();
                        return;

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
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

            if (string.IsNullOrEmpty(err))
            {

                DialogResult qs = MessageBox.Show("Are You Sure to Delete this Record...?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (qs == DialogResult.No)
                {
                    return;
                }



                using (SqlConnection cn = new SqlConnection(Utils.Helper.constr))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        try
                        {
                            cn.Open();
                            cmd.Connection = cn;

                            string sql = "Update MastEmpAddress set Add1=''," +
                               " Add2='', Add3=''," +
                               " Add4='', City=''," +
                               " District='',StateName=''," +
                               " PinCode='',Phone=''," +
                               " UpdDt=GetDate() , UpdID='" + Utils.User.GUserID + "' " +
                               " Where EmpUnqID='" + ctrlEmp1.txtEmpUnqID.Text.Trim() + "' and AddressType='PRE'";

                            cmd.CommandText = sql;
                            cmd.ExecuteNonQuery();

                            sql = "Update MastEmpAddress set Add1=''," +
                                " Add2='', Add3=''," +
                                " Add4='', City=''," +
                                " District='',StateName=''," +
                                " PinCode='',Phone=''," +
                                " UpdDt=GetDate() , UpdID='" + Utils.User.GUserID + "' " +
                                " Where EmpUnqID='" + ctrlEmp1.txtEmpUnqID.Text.Trim() + "' and AddressType='PER'";

                            cmd.CommandText = sql;
                            cmd.ExecuteNonQuery();

                            sql = "Update MastEmpIdentity set " +
                                " Passport ='', AdharCard='',ElectionCard =''," +
                                " PanCard='', DrivingLicense='', PFAccountNo =''," +
                                " UANNO='', BankName='', BankAccountNo=''," +
                                " BankBranchCode='', BankIFSCCode='' " +
                                " UpdDt=GetDate(), UpdID='" + Utils.User.GUserID + "'" +
                                " Where EmpUnqID='" + ctrlEmp1.txtEmpUnqID.Text.Trim() + "'";
                            cmd.CommandText = sql;
                            cmd.ExecuteNonQuery();

                            MessageBox.Show("Record Deleted...", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ResetCtrl();
                           
                            return;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }
            }

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

      
        private void DisplayData(string tCompCode,string tEmpUnqID)
        {
            string sql = "Select * from MastEmpAddress Where EmpUnqID = '" + tEmpUnqID + "'";
            
            DataSet ds = Utils.Helper.GetData(sql, Utils.Helper.constr);
            Boolean hasRows = ds.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);

            if (hasRows)
            {
                foreach(DataRow dr in ds.Tables[0].Rows)
                {
                    if(dr["AddressType"].ToString() == "PRE")
                    {
                        txtAdd1.Text = dr["Add1"].ToString();
                        txtAdd2.Text = dr["Add2"].ToString();
                        txtAdd3.Text = dr["Add3"].ToString();
                        txtAdd4.Text = dr["Add4"].ToString();
                        txtCity.Text = dr["City"].ToString();
                        txtDistrict.Text = dr["District"].ToString();
                        txtState.Text = dr["StateName"].ToString();
                        txtPinCode.Text = dr["PinCode"].ToString();
                        txtPhone.Text = dr["Phone"].ToString();
                    }
                    else
                    {
                        txtpAdd1.Text = dr["Add1"].ToString();
                        txtpAdd2.Text = dr["Add2"].ToString();
                        txtpAdd3.Text = dr["Add3"].ToString();
                        txtpAdd4.Text = dr["Add4"].ToString();
                        txtpCity.Text = dr["City"].ToString();
                        txtpDistrict.Text = dr["District"].ToString();
                        txtpState.Text = dr["StateName"].ToString();
                        txtpPinCode.Text = dr["PinCode"].ToString();
                        txtpPhone.Text = dr["Phone"].ToString();
                    }
                }


                //IDENTITY1
                ds = Utils.Helper.GetData("Select * from MastEmpIdentity where EmpUnqId = '" + tEmpUnqID + "'", Utils.Helper.constr);
                hasRows = ds.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);

                if (hasRows)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    txtPassportNo.Text = dr["Passport"].ToString();
                    txtAdharNo.Text =dr["AdharCard"].ToString();
                    txtElectionNo.Text =  dr["ElectionCard"].ToString();
                    txtPANNo.Text = dr["PanCard"].ToString();
                    txtDRVNo.Text = dr["DrivingLicense"].ToString();
                    txtPFNo.Text= dr["PFAccountNo"].ToString();
                    txtUANNo.Text= dr["UANNO"].ToString();
                    txtBankName.Text= dr["BankName"].ToString();
                    txtBankAc.Text= dr["BankAccountNo"].ToString();
                    txtBankBranch.Text= dr["BankBranchCode"].ToString();
                    txtBankIFSC.Text= dr["BankIFSCCode"].ToString();
                }


                mode = "OLD";
                oldCode = tEmpUnqID;


            }
            else
            {
                mode = "NEW";
                oldCode = "";
            }
                       
        }

       
        private void frmMastEmpPerInfo_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyData == Keys.Enter))
            {
                SelectNextControl(ActiveControl, true, true, true, true);
            }
        }

      


    }
}
