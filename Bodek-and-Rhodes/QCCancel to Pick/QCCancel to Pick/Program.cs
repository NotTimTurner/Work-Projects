using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace QCCancel_to_Pick
{
    class Program
    {
        static void Main(string[] args)
        { Utility UT = new Utility();
            Validation Valid=new Validation();
            int cnt = 0;
            DebugLog Log = new DebugLog();
            DatabaseConnection DbCon = new DatabaseConnection();
            SqlConnection Con = new SqlConnection(DbCon.GetConString());
            SqlDataReader MyReader = null;
            SqlCommand SqlCommand = new SqlCommand("select * from inventory where location_id='QC Cancel'", Con);
            Con.Open();
            MyReader = SqlCommand.ExecuteReader();
            while (MyReader.Read())
            {
                string RID=MyReader["_RID_"].ToString();
                string Product_ID=MyReader["Product_id"].ToString();
                string PiecePick = UT.GetPiecePickLocationForProductID(Product_ID);
                if(Valid.IsThisaPiecePickLocation(PiecePick)==true)
                {
                    UT.DeleteMoveQueueByInv_RID(RID,"50","putaway");
                    UT.UpdateInventoryLocationID(RID, PiecePick);

                    Console.WriteLine(RID);
                    cnt++;
                }

               

            }
            Con.Close();
            
        }
    }
}
