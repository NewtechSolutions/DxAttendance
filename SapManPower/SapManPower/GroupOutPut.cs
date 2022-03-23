using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SapManPowerFTPClient
{
    class GroupOutPut
    {
        public DateTime RecDate {get;set;}
        public string CostGroup { get; set; }
        public decimal output { get; set; }
        public int RecCount { get; set; }

        public bool update(string cnstr,out string err)
        {
            bool result = false;
            err = string.Empty;
            string sql = "update [MastCostGroupManPowerRpt] set output1 ='" + this.output.ToString() + "' where tDate ='" + this.RecDate.Date.ToString("yyyy-MM-dd") + "' and CostGroup='" + this.CostGroup + "'";

            SqlConnection conn = new SqlConnection(cnstr);
            SqlCommand command = new SqlCommand(sql, conn) { CommandType = CommandType.Text };


            try
            {
                conn.Open();
                command.CommandTimeout = 1500;
                int result1 = command.ExecuteNonQuery();

                if (result1 != 0){
                    result = true;
                    err = "Updated";
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
