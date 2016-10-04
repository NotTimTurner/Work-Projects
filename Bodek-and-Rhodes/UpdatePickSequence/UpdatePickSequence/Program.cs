using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace UpdatePickSequence
{
    class Program
    {//if we need to roll back search by _last_Updated_ from 2004 and 2015 and set picksequence of those to 0
        static void Main(string[] args)
        {
            //Locations[] locationTable = new Locations[100000];
            int count = 0;
           Utility UT = new Utility();
            int cnt=0;
            DebugLog Log = new DebugLog();
             DatabaseConnection  DbCon = new DatabaseConnection();
            SqlConnection Con = new SqlConnection(DbCon.GetConString());
            SqlDataReader MyReader = null;
            SqlCommand SqlCommand = new SqlCommand("select * from Locations where pick_sequence=0", Con);
            Con.Open();
            MyReader = SqlCommand.ExecuteReader();
            while (MyReader.Read())
            {
                string LocationID = MyReader["Location_id"].ToString();
                int PickSqquence = UT.CreatePickSequence(LocationID);
                UT.UpdatePickSequence(PickSqquence,LocationID);
                Log.Write("Location: " + LocationID + " Updated to pick Sequence: "+PickSqquence);

            }
        }
    }
}
