using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace RemoveAllPriority30s
{
    class Program
    {
        static void Main(string[] args)
        {   Utility UT = new Utility();
            int cnt = 0;
            DebugLog Log = new DebugLog();
            DatabaseConnection DbCon = new DatabaseConnection();
            SqlConnection Con = new SqlConnection(DbCon.GetConString());
            SqlDataReader MyReader = null;
            SqlCommand SqlCommand = new SqlCommand("select * from move_queue where priority='30'", Con);
            Con.Open();
            MyReader = SqlCommand.ExecuteReader();
            while (MyReader.Read())
            {
                string RID=MyReader["Inv_RID"].ToString();
                UT.RemoveHold(RID);
                UT.DeleteMoveQueue(RID);
                Log.Write(RID + " RESET");

            }
            Con.Close();
        }
    }
}
