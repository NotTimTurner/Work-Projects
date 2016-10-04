using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace UpdateVLEVEL
{
    class Program
    {
        static void Main(string[] args)
        {
            Utility UT = new Utility();
            int cnt = 0;
            DebugLog Log = new DebugLog();
            DatabaseConnection DbCon = new DatabaseConnection();
            SqlConnection Con = new SqlConnection(DbCon.GetConString());
            SqlDataReader MyReader = null;
            SqlCommand SqlCommand = new SqlCommand("select * from locations where vlevel='' or vlevel='0' or vlevel='7' and EIS_LOCATION!=''", Con);
            Con.Open();
            MyReader = SqlCommand.ExecuteReader();
            while (MyReader.Read())
            {
               // string Product_id = MyReader["Product_id"].ToString();
              //  string UOM = UT.GetCaseUOM(Product_id);

               // UT.UpdateUOM(Product_id, UOM);
                string VLEVEL = UT.GetVlevel(MyReader["Location_id"].ToString());
                UT.updateVLEVEL(VLEVEL, MyReader["Location_id"].ToString());
                cnt++;
                Console.WriteLine(VLEVEL);
               // Log.Write(UOM + MyReader["SHORT_DESCRIPTION"].ToString());

            }
           // Console.ReadLine();
            Con.Close();
        }
    }
}
