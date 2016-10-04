using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using System.Runtime.InteropServices;

using System.Diagnostics;


    public class DebugLog
    {


        public DebugLog()
        {

        }


        public void Write(string Message)
        {

            string directory = @"\\192.168.1.192\d$\Tim's Projects\Update_Location_Group\Log";
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
            string directory = @"\\192.168.1.192\d$\Tim's Projects\Update_Location_Group\Log";
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
            string directory = @"\\192.168.1.192\d$\Tim's Projects\Update_Location_Group\Log";
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
    }

      

