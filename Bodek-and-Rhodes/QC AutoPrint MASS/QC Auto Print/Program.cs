using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Printing;
using System.Runtime.InteropServices;
using System.Data;
using System.Data.SqlClient;
//add printers to database table with 
//add time on the ticket

namespace QC_Auto_Print
{
    class Program
    {

        //used to hide the consol window
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;




        static void Main(string[] args)
        {


            var handle = GetConsoleWindow();
            //hides cosole window
            // ShowWindow(handle, SW_HIDE);
            String strConnString = "Data Source=192.168.5.1;Initial Catalog=WMS;User ID=bodek;";
            //multiple connections because each reader needs it's own connection
            SqlConnection conn = new SqlConnection(strConnString);
            SqlConnection conn2 = new SqlConnection(strConnString);
            SqlConnection conn3 = new SqlConnection(strConnString);

            SqlDataReader rdr = null;
            

            try
            {
                //gets a new QC Alert
                string sql = "select top 1 AI._rid_, AI.inv_trace, AI.inv_trace_old, AI.user_id, AI.owner_id, AI.product_id, AI.location_id, AI.Transaction_time, OS.Carrier_id " +
                   " from audit_inventory AI, Outbound_shipments os " +
                   " where trantype = 'QC' and subtype = 'AUDIT' and AI.inv_trace=OS.Shipment_id " +
                   " and location_id is not null and exporttime_sys3 is null ";
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
                                 SqlDataReader Reader = null;
                                 SqlDataReader SDReader = null;
                                 string Auditsql;
                                 string ShortDescsql;
                                 string ShortDesc="";
                 

            

                                if(Pickticket!=(rdr["inv_trace"].ToString() + rdr["inv_trace_old"].ToString()))
                                {
                                    header_msg="\n **************************\n";
                                    header_msg += " *** QC Pick Required ***\n";
                                    header_msg += " Origional Pick Ticket:  " + rdr["inv_trace"]+ rdr["inv_trace_old"] ;
                                    header_msg += "\n CarrierID:  " + rdr["carrier_id"];
                                    header_msg += "\n QC User ID:  " + rdr["user_id"];
                                    header_msg += "\n Picker ID: " + rdr["owner_id"]+'\n';
                                }

                                Pickticket = (rdr["inv_trace"].ToString() + rdr["inv_trace_old"].ToString());
                
                                Auditsql = "select product_id, ISNULL(defect_id,'UNKNOWN') as QC_CODE,";
              
                                Auditsql += " ISNULL(defect_id_old,'0') as QUANTITY ";
                                Auditsql += " from audit_inventory_detail WITH (NOLOCK)";
                                Auditsql += " Where audit_inventory_rid = " + rdr["_rid_"];


               
                                SqlCommand Command = new SqlCommand(Auditsql, conn2);
                                Reader = Command.ExecuteReader();
                         
                while (Reader.Read())
                {

                    ShortDescsql = "select short_description from product_master WITH (NOLOCK)";
                    ShortDescsql += " where product_id = '" + rdr["product_id"] + "'";
                    SqlCommand SDCommand = new SqlCommand(ShortDescsql, conn3);
                    SDReader = SDCommand.ExecuteReader();
                    while (SDReader.Read())
                    {
                        ShortDesc = SDReader["short_description"].ToString();

                    }
                    SDReader.Close();
                    if (header_msg.Length > 0)
                    {
                        msg += header_msg;
                        header_msg = "";
                    }

                    msg += "\n * * * * * * * * * * * * \n";
                    msg += "\n Error Time :    " + rdr["transaction_time"];
                    msg += "\n Product ID:     " + rdr["product_id"];
                    msg += "\n Description:  " + ShortDesc;
                    msg += "\n Location ID:  " + rdr["location_id"];
                    msg += "\n QC Code:  " + Reader["QC_CODE"];
                    msg += "\n Quantity:  " + Reader["Quantity"] + '\n';

                    QCType = Reader["QC_CODE"].ToString();
                }

                if (RID.Length == 0)
                    RID = rdr["_rid_"].ToString();
                else
                    RID += ',' + rdr["_rid_"].ToString();


                
              Console.WriteLine(msg);
              Reader.Close();

            PrintDocument p = new PrintDocument();
            PrintController Controler = new StandardPrintController();
            //designates a printer
            p.PrinterSettings.PrinterName = "Norton QC";
            //will hide the printing dialoge box
            p.PrintController = Controler;
            p.PrintPage += delegate(object sender1, PrintPageEventArgs e1)
            {
                e1.Graphics.DrawString(msg, new Font("Times New Roman", 25), new SolidBrush(Color.Black), new RectangleF(0, 0, p.DefaultPageSettings.PrintableArea.Width, p.DefaultPageSettings.PrintableArea.Height));
            };
            try
            {
               //if(QCType!="PLS")
                p.Print();
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
               // Console.ReadLine();


            }
            
 
                        }
                         updateAuditInventory(RID);
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

                     }
                  }

                    private static int updateAuditInventory(string RID)
                    {
                        string sql="";
                        string strConnString = "Data Source=192.168.5.1;Initial Catalog=WMS;User ID=bodek;";
                        SqlConnection conn = new SqlConnection(strConnString);

            
                        if(RID.Length==0)
                        {
                            return 0;
                        }

                        conn.Open();


                        sql = "update audit_inventory WITH (ROWLOCK) set exporttime_sys3 = getdate() ";
                        sql += ", _last_module_ = 'QCALERTER', _last_updated_ = getdate(), _last_user_ = 'SLS' ";
                         sql += " where _rid_ in (" + RID + ")";
                         SqlCommand cmd = new SqlCommand(sql, conn);
                         cmd.ExecuteNonQuery();
                        Console.WriteLine(RID);
                       // Console.ReadLine();

        
                      
             return 0;
        }

    }
}


        
    

 




