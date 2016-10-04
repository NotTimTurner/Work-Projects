using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Data.Entity.Validation;


using Office = Microsoft.Office.Core;
using System.Diagnostics;

namespace WarehouseTransfer
{
    public class DebugLog
    {


        public DebugLog()
        {

        }


        public void Write(string Message)
        {
            
            string directory = @"\\192.168.1.192\d$\Tim's Projects\Warehouse Transfer\Log";
            string filename = string.Format("{0:yyyy-MM-dd}__{1}", DateTime.Now, "TransferLog.txt");
            string path = Path.Combine(directory, filename);

            if (!File.Exists(path))
            {
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine("file created at" + DateTime.Now, true);
                    Console.WriteLine("new file created at: " + path);


                }
            }
            using (StreamWriter sw2 = File.AppendText(path))
            {
                sw2.WriteLine(Message, true);
                Console.WriteLine(Message);

            }
        }

        public void newLine()
        {
            string directory = @"\\192.168.1.192\d$\Tim's Projects\Warehouse Transfer\Log";
            string filename = string.Format("{0:yyyy-MM-dd}__{1}", DateTime.Now, "TransferLog.txt");
            string path = Path.Combine(directory, filename);

            if (!File.Exists(path))
            {
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine("file created at" + DateTime.Now, true);
                    Console.WriteLine("new file created at: " + path);


                }
            }
            using (StreamWriter sw2 = File.AppendText(path))
            {
                sw2.WriteLine("\n", true);

            }

        }

        public void SendErrorEmail(string Message)
        {
            //doesnt work right now. will figure out later- Tim 
        }

        internal void Write(int Message)
        {
            string directory = @"\\192.168.1.192\d$\Tim's Projects\Warehouse Transfer\Log";
            string filename = string.Format("{0:yyyy-MM-dd}__{1}", DateTime.Now, "TransferLog.txt");
            string path = Path.Combine(directory, filename);

            if (!File.Exists(path))
            {
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine("file created at" + DateTime.Now, true);
                    Console.WriteLine("new file created at: " + path);


                }
            }
            using (StreamWriter sw2 = File.AppendText(path))
            {
                sw2.WriteLine(Message.ToString(), true);

            }
        }

        internal void LogError(string ShipmentID, string OrderNumber, float? OrderLine, string ProductID, string StrText)
        {
            try
            {
                var WMS = new WMSEntities();
                Write(StrText);
                int RID = GetNextErrorRID();
                string SText = "Shipment: " + ShipmentID + " Order: " + OrderNumber + " Line: " + OrderLine + " Product_ID: " + ProductID;
                Write(SText);
                var error = new ERRORS_RUNTIME()
                {
                    C_RID_ = RID,
                    ERRORTIME = DateTime.Now,
                    USERID = "WHT",
                    MODULE = "Warehouse_Trans",
                    FUNCTION = "Log_error",
                    ERRORTYPE = null,
                    ERRORNUMBER = 100,
                    ERRORMESSAGE = StrText,
                    ERRORDATA = SText,
                    C_LAST_UPDATED_ = DateTime.Now,
                    C_LAST_USER_ = "WHT",
                    C_LAST_MODULE_ = "Warehouse_transfer"
                };
                WMS.ERRORS_RUNTIME.Add(error);
                WMS.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    Console.ReadLine();
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                        Console.ReadLine();
                    }
                }
                throw;
            }
        }

        private int GetNextErrorRID()
        {
            int RID = 0;

            using (WMSEntities wms = new WMSEntities())
            {
                IQueryable<SYSTEMNEXTRID> Item = from NewRid in wms.SYSTEMNEXTRIDs where NewRid.GENERICNAME == "Errors_Runtime" select NewRid;
                foreach (SYSTEMNEXTRID New in Item)
                {
                    New.NEXTRID++;
                    RID = New.NEXTRID;
                    wms.SaveChanges();
                }
            }

            return RID;
        }

        internal void Write(string Message, string Message2)
        {
            string directory = @"\\192.168.1.192\d$\Tim's Projects\Warehouse Transfer\Log";
            string filename = string.Format("{0:yyyy-MM-dd}__{1}", DateTime.Now, "TransferLog.txt");
            string path = Path.Combine(directory, filename);

            if (!File.Exists(path))
            {
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine("file created at" + DateTime.Now, true);
                    Console.WriteLine("new file created at: " + path);


                }
            }
            using (StreamWriter sw2 = File.AppendText(path))
            {
                sw2.WriteLine(Message + " " + Message2, true);
                Console.WriteLine(Message + " " + Message2);
            }
        }
    }
}
