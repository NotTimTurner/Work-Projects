using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace DeleteInventoryAndMoveQueue
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
            SqlCommand SqlCommand = new SqlCommand("select I._RID_ as IRID, MQ._RID_ as MQRID from inventory I, move_queue MQ where product_id='07675353'and location_id='RECVSTAGE' and MQ.Inv_RID=I._RID_ ", Con);
            Con.Open();
            MyReader = SqlCommand.ExecuteReader();
            while (MyReader.Read())
            {
                string invRID=MyReader["IRID"].ToString();
                string MQRID = MyReader["MQRID"].ToString();

                UT.DeleteMoveQueueByRID(MQRID);
                UT.DeleteInventoryByRID(invRID);

                Log.Write(invRID + "Deleted");

            }
            Con.Close();
        }
    }
}
