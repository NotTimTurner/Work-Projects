using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Update_UOM
{
    class Program
    {
        static void Main(string[] args)
        {
            Utility UT = new Utility();
            int cnt=0;
            DebugLog Log = new DebugLog();
             DatabaseConnection  DbCon = new DatabaseConnection();
            SqlConnection Con = new SqlConnection(DbCon.GetConString());
            SqlDataReader MyReader = null;
            SqlCommand SqlCommand = new SqlCommand("select * from product_master where BASE_UOM_Per_caselevel is null or BASE_UOM_Per_caselevel = 2 ", Con);
            Con.Open();
            MyReader = SqlCommand.ExecuteReader();
            while (MyReader.Read())
            {
                string Product_id = MyReader["Product_id"].ToString();
                string UOM = UT.GetCaseUOM(Product_id);

                UT.UpdateUOM(Product_id, UOM);

                Log.Write(UOM + MyReader["SHORT_DESCRIPTION"].ToString());

            }
            Con.Close();
        }
    }
}
