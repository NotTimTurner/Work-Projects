using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Update_Location_Group
{
    class Program
    {
        static void Main(string[] args)
        {
            Validation Valid = new Validation();
            Utility UT = new Utility();
            DebugLog log = new DebugLog();
            int ChangeCount = 0;
            int RightCount = 0;
            int TotalCount = 0;
          
             DatabaseConnection  DbCon = new DatabaseConnection();
            SqlConnection Con = new SqlConnection(DbCon.GetConString());
            SqlDataReader MyReader = null;
            SqlCommand SqlCommand = new SqlCommand("select Location_id, Location_group from locations (nolock) where Location_Group <> 'BSCS' and location_id <> 'OLD' and location_id <> 'TEST' AND location_id <> 'DISC' AND location_id <> 'UC' and Len(Picking_Zone) > 0", Con);
            Con.Open();
            MyReader = SqlCommand.ExecuteReader();
            while (MyReader.Read())
            {

                string Location_id = MyReader["Location_ID"].ToString();
                string Location_Group = MyReader["Location_Group"].ToString();

                string Correct_Location_Group = UT.CreateLocationGroup(Location_id);

                if(Location_Group!=Correct_Location_Group&&Correct_Location_Group!="")
                {
                    UT.UpdateLocationGroup(Location_id, Correct_Location_Group);
                    ChangeCount++;
                }
                else
                {
                    RightCount++;
                }

                TotalCount++;

                log.Write("LocationID: " + Location_id + " Location Group: " + Location_Group + " New Location Group: " + Correct_Location_Group);

            }

            log.Write("Total Correct Locations: " + RightCount);
            log.Write("Total Locations changed: " + ChangeCount);
            log.Write("Total Locations Grabbed: " + TotalCount);


        }
    }
}
