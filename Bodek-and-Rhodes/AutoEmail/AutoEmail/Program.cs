using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using System.Data.SqlClient;

//to do: change it so it grabs the shipment Id from database
namespace AutoEmail
{
    class Program
    {
        static void Main(string[] args)
        {
            var fromAddress = new MailAddress("WMS@BodekandRhodes.com", "From Name");
            var toAddress = new MailAddress("JSuttmoeller@BodekandRhodes.com", "To Name");
            const string fromPassword = "wms4alerts";
            string subject="";
            string body = CreateBody(ref subject);

            if (body != "")
                
            {
                SmtpClient smtp = new SmtpClient
                {
                    Host = "10.0.1.158",
                    EnableSsl = false,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                };

                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body
                   
                })
                  

                {
                    //message.To.Add("RMartone@alphabroder.com");
                  //  message.To.Add("CWeathers@alphabroder.com");
                    message.To.Add("RJW@BodekandRhodes.com");
                   // message.To.Add("tturner@BodekandRhodes.com");
                   
                    smtp.Send(message);
                    Console.WriteLine("Email Sent");

                }
            }

        }
    

        private static string CreateBody(ref string Subject)
        {
            string shipment_id =GetShipmentID();
            if (shipment_id != "0")
            {
                string body = "";
                // string RID = "7524640";
                CartonManafest CM = new CartonManafest(shipment_id);

                //for (int i = 0; i < CM.CartonLines; i++)
                //{
                //    body += CM.ShipmentID + CM.CML[i].OrderNumber+"\n";
                //}

                body = "Order Date: ";
                if (CM.PurchaseOrderDate == null)
                    body += "Unspecified";
                else
                    body += (CM.PurchaseOrderDate.Month + "/" + CM.PurchaseOrderDate.Day + "/" + CM.PurchaseOrderDate.Year.ToString()).PadRight(25);


                body += ("CustomerPO: " + CM.PurchaseOrder).PadRight(25);
                body += "Post Date: " + CM.Schedulded_ship_date.Month + "/" + CM.Schedulded_ship_date.Day + "/" + CM.Schedulded_ship_date.Year + "\n\n";

                body += ("Ship To: " + CM.Name).PadRight(52) + "\n";

                body += ("                 " + CM.Address1 + " " + CM.Address2).PadRight(60) + "\n";

                body += "                  " + CM.City + ", " + CM.State + " " + CM.ZIP.PadRight(60) + "\n\n\n";

                body += "Bill To: " + CM.CustomerID + " - " + CM.CustName + "\n";
                body += "              " + CM.CustAdd1 + " " + CM.CustAdd2 + "\n";
                body += "          " + CM.CustCity + ", " + CM.CustState + " " + CM.CustZip + "\n\n\n";

                body += "TERMS: " + CM.TermsDesc + "   Warehouse Order: " + CM.ShipmentID + "    Order Type:" + CM.Status + "    Freight: " + CM.ShipDesc + "\n";
                body += " Carton  Product                  Pieces  \n";
                body += "------------------------------------------\n";
                for (int i = 0; i < CM.CartonLines; i++)
                {
                    body += CM.CML[i].OrderNumber.PadRight(7);
                    body += CM.CML[i].ShortDescription.PadRight(25);
                    body += CM.CML[i].OrderedUnits + "\n";
                }

                Subject = "Order: " + CM.ShipmentID + "   PO: " + CM.PurchaseOrder;
                Utility UT = new Utility();
                UT.updateFlag(shipment_id);
                return body;
            }
            else
            {
                
                return "";
            }
        }

       

        private static string GetShipmentID()
        {
             string InvRid = "0";
        DatabaseConnection DbCon = new DatabaseConnection();
        SqlConnection Con = new SqlConnection(DbCon.GetConString());
        SqlDataReader MyReader = null;
        SqlCommand SqlCommand = new SqlCommand("Select top 1 shipment_id from outbound_shipments where status='Z' and order_number='0001' and customer_id='101210' and Trailer_seal_3 is null", Con);
        Con.Open();
        MyReader = SqlCommand.ExecuteReader();
        while (MyReader.Read())
        {
            InvRid = MyReader["shipment_id"].ToString();
        }
        Con.Close();
        return InvRid;
    }
        }

          
            


        
    
}
