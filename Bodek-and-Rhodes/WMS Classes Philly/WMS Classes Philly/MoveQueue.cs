using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace WMS_Classes_Philly
{
    class MoveQueue
    {
        public int RID { get; set; }
        public int InvRID { get; set; }
        public string MQType { get; set; }
        public int Priority { get; set; }
        public DateTime CreateTime { get; set; }
        public string Status { get; set; }
        public string LabelLevel { get; set; }
        public string ToZone { get; set; }
        public string ToLocation { get; set; }
        public string FromZone { get; set; }
        public string FromLocation { get; set; }
        public float? OrigionalQuantityRequired { get; set; }
        public float? QuantityRequired { get; set; }
        public float? QuantityInTransit { get; set; }
        public string UserIdInTransit { get; set; }
        public string VehicleInTranset { get; set; }
        public string shipmentID { get; set; }
        public string OrderNumber { get; set; }
        public float? OrderLine { get; set; }
        public DateTime? LastUpdated { get; set; }
        public string LastUser { get; set; }
        public string LastModule { get; set; }


        public MoveQueue(int InRID)
        {
            
                   
                
            
        }



        public void GetNumberOfItemsInPhilly()
        {
            long items = 0;
            long Products = 0;
            String strConnString = "Data Source=192.168.1.2;Initial Catalog=WMS;User ID=bodek;";
            //multiple connections because each reader needs it's own connection
            SqlConnection conn = new SqlConnection(strConnString);
            SqlDataReader rdr = null;
            string sql = "select Quantity from Inventory where Quantity <> 0 and Quantity is not null";

            SqlCommand cmd = new SqlCommand(sql, conn);
            conn.Open();
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                string temp = rdr["Quantity"].ToString();
                //Console.WriteLine(temp);
                items += Convert.ToInt64(temp);
            }
            conn.Close();
            Console.WriteLine("Number of pieces in the Philly Warehouse: " + items);

            string sql2 = "select distinct product_id from Inventory where Quantity <> 0 and Quantity is not null";
            SqlCommand cmd2 = new SqlCommand(sql2, conn);
            conn.Open();
            rdr = cmd2.ExecuteReader();

            while (rdr.Read())
            {

                Products++;
            }
            Console.WriteLine("Number of Different Products in the Philly Warehouse: " + Products);
            conn.Close();
            Console.ReadLine();


        }
    }
}
