using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SapManPowerFTPClient
{
    
    //costcode wise details
    class Output
    {
        public string WorkCenter { get; set; }
        public string CostCenter { get; set; }
        public decimal OutPut { get; set; }
        public string CostGroup { get; set; }
        public DateTime RecDate { get; set; }

        public bool update(string cnstr, out string err)
        {
            bool result = false;
            err = string.Empty;

            int reccnt = 0;
            string rec = Utility.GetDescription("select count(*) from MastCostCodeDailyoutput where CostCode = '" + this.CostCenter.Trim() + "' and tDate ='" + this.RecDate.Date.ToString("yyyy-MM-dd") + "'", cnstr, out err);

            if (!string.IsNullOrEmpty(err))
            {
                result = false;
                return result;
            }

            int.TryParse(rec,out reccnt);
            string sql = string.Empty;
            if (reccnt == 0)
            {
                sql = "insert into [MastCostCodeDailyoutput] (tdate,costcode,output1,adddt) values (" +
                    " '" + this.RecDate.Date.ToString("yyyy-MM-dd") + "'," +
                    " '" + this.CostCenter.Trim() + "'," +
                    " '" + this.OutPut.ToString() + "',GetDate() )";

            }
            else
            {
                sql = "update [MastCostCodeDailyoutput] set output1 =  '" + this.OutPut.ToString() + "' " +
                    " where tDate ='" + this.RecDate.Date.ToString("yyyy-MM-dd") + "' " +
                    " And CostCode='" + this.CostCenter.Trim() + "'";
            }
            
            SqlConnection conn = new SqlConnection(cnstr);
            SqlCommand command = new SqlCommand(sql, conn) { CommandType = CommandType.Text };
            
            try
            {
                conn.Open();
                command.CommandTimeout = 1500;
                int result1 = command.ExecuteNonQuery();

                if (result1 != 0)
                {
                    result = true;
                    err = this.CostCenter.Trim() + "--" + "Updated";
                }
                else
                {
                    err = "no record found to update...";
                    result = false;
                }

                conn.Close();
            }
            catch (SqlException ex) { err = ex.Message.ToString(); result = false; }
            catch (Exception ex) { err = ex.Message.ToString(); result = false; }
            finally
            {
                conn.Close();
            }

            return result;
        }
    }
}
