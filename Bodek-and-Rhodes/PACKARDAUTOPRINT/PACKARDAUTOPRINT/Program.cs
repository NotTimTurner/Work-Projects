using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Printing;
using System.Runtime.InteropServices;
using System.Data;
using System.Data.SqlClient;

namespace PACKARDAUTOPRINT
{

    class Program
    {
        static void Main(string[] args)
        {
            DebugLog log = new DebugLog();
            
            String strConnString = "Data Source=192.168.5.1;Initial Catalog=WMS;User ID=bodek;";
            //multiple connections because each reader needs it's own connection
            SqlConnection conn = new SqlConnection(strConnString);
            SqlConnection conn2 = new SqlConnection(strConnString);
            SqlConnection conn3 = new SqlConnection(strConnString);
            SqlConnection conn4 = new SqlConnection(strConnString);


            SqlDataReader rdr = null;

            log.Write("Test");

            try
            {
                //gets a new QC Alert
                string sql = "select distinct top 1 * from outbound_shipments where customer_id='101210' and trailer_number is null and (Status = 'P'or status ='C' or Status ='I' or Status ='Z') ";
                // Open the connection
                conn.Open();
                conn2.Open();
                conn3.Open();
               
                SqlCommand cmd = new SqlCommand(sql, conn);

                rdr = cmd.ExecuteReader();
                string Pickticket = "";
                string msg = "";
                string header_msg = "";
                string RID = "";
                string QCType = "";



                while (rdr.Read())
                {
                    Utility UT = new Utility();
                    string shipmentID=rdr["SHIPMENT_ID"].ToString();
                    string printer = rdr["Trailer_seal_1"].ToString();
                    string copies = rdr["Trailer_seal_2"].ToString();

                   //  RID = rdr["_RID_"].ToString();
                    int count = UT.GetNumberOfCartons(shipmentID);
                    //string sql2 = "select shipment_id from outbound_shipment_deatils where shipment_id='" + rdr["SHIPMENT_ID"].ToString() + "'";
                    
                    //conn4.Open();
                    //SqlCommand cmd2 = new SqlCommand(sql2, conn4);
                    //SqlDataReader rdr2 = cmd.ExecuteReader();
                    //while (rdr2.Read())
                    //{
                    //    count++;
                    //}
                    //rdr2.Close();
                   

                    SqlDataReader Reader = null;
                    SqlDataReader SDReader = null;
                    string Auditsql;
                    string ShortDescsql;
                    string ShortDesc = "";

                    Pen blackPen = new Pen(Color.Black, 3);

                    // Create location and size of rectangle.
                    int x = 0;
                    int y = 0;
                    int width = 200;
                    int height = 200;

                    // Draw rectangle to screen.
                   System.Drawing.Printing.PrintPageEventArgs e;

                   msg = "\n";
                    msg += "ORDER #: " + shipmentID;
                    msg += "\nPO #: " + UT.GetPurchaseOrder(shipmentID)+"\n";
                    msg += "# of Cartons: ___/"+count+"\n";
                    msg += "asn #: ____________\n";
                    if (count < 160)
                    {
                        msg += "partial cases: ☑\n";
                        msg += "full cases:    ☐\n ";
                    }
                    else
                    {
                        msg += "partial cases: ☐\n ";
                        msg += "full cases:    ☑\n ";
                    }
                    int max = 0;
                    if (copies == "" || copies == null)

                        max = 15;
                    else
                        max = Convert.ToInt16(copies);
                    
                    

                   
                    int cnt=0;
                    PrintDocument p = new PrintDocument();
                    PrintController Controler = new StandardPrintController();
                    //designates a printer
                    if (printer == "" || printer == null)
                        p.PrinterSettings.PrinterName = "PACKSLIP8";
                        
                    else
                        p.PrinterSettings.PrinterName = printer;
                    p.DefaultPageSettings.Landscape = true;
                    //will hide the printing dialoge box
                    p.PrintController = Controler;
                    p.PrintPage += delegate(object sender1, PrintPageEventArgs e1)
                    {
                        e1.Graphics.DrawString(msg, new Font("Times New Roman", 55), new SolidBrush(Color.Black), new RectangleF(0, 0, p.DefaultPageSettings.PrintableArea.Width, p.DefaultPageSettings.PrintableArea.Height));
                        //e1.Graphics.DrawString(msg1, new Font("Times New Roman", 50), new SolidBrush(Color.Black), new RectangleF(0, 0, p.DefaultPageSettings.PrintableArea.Width, p.DefaultPageSettings.PrintableArea.Height));
                        
                    };
                    try
                    {
                        while (cnt < max)
                        {
                            cnt++;
                            //if(QCType!="PLS")
                            p.Print();
                        }
                    }

                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                        


                    }
                    
                    updateAuditInventory(shipmentID);
                    log.Write(" SHipment: " + shipmentID + " Printed");
                }
               
                rdr.Close();
                // Console.ReadLine();


            }
            catch (Exception e)
            {
             
                Console.WriteLine(e.Message);
                //  Console.ReadLine();
            }
            finally
            {

                if (conn != null)
                    conn.Close();

                if (conn2 != null)
                    conn2.Close();

                if (conn3 != null)
                    conn3.Close();
                if (conn4 != null)
                    conn3.Close();

            }
        }

       

        private static string updateAuditInventory(string Shipment_id)
        {
            
            string sql = "";
            string strConnString = "Data Source=192.168.5.1;Initial Catalog=WMS;User ID=bodek;";
            SqlConnection conn = new SqlConnection(strConnString);


            if (Shipment_id.Length == 0)
            {
                return "0";
            }

            conn.Open();


            sql = "update outbound_shipments WITH (ROWLOCK) set trailer_number='27' ";
            sql += ", _last_module_ = 'PacardPrint', _last_updated_ = getdate(), _last_user_ = 'TTURNER' ";
            sql += " where shipment_id='"+Shipment_id+"'";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.ExecuteNonQuery();
            
            Console.WriteLine(Shipment_id);
            // Console.ReadLine();



            return "0";
        }
    }
}
