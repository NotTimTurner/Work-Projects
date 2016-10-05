using System;
using System.IO;
using System.Data;
using System.Data.SqlClient;


/// <summary>
/// Project: Advertisment Updater
/// Date Created: March 2015
/// purpose:    Uses the signotec API to update the Images on the signature pad from the warehousestrg Database.
///             added to the task scheduler on each machine to run at 5 AM while no one is IN
/// </summary>

namespace AdvertismentUpdater
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //signature pad API
                signotec.STPadLibNet.STPadLib spad = new signotec.STPadLibNet.STPadLib();

                // System.Diagnostics.Process reset = System.Diagnostics.Process.Start(@"C:\Program Files (x86)\signotec\signoPAD-API\Applications\signoReset\signoReset.exe");
                // System.Threading.Thread.Sleep(60000);
                //reset.Kill();

                spad.DeviceOpen(0);
                //Connect to warehouseSTRG and run ReadALLImage( basically just a select * from AdvertismentImages)
                String strConnString = "Data Source=192.168.1.192;Initial Catalog=WMS;User ID=bodek;";
                SqlConnection con = new SqlConnection(strConnString);
                SqlCommand cmd = new SqlCommand("ReadAllImage", con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                if (con.State == ConnectionState.Closed)
                    con.Open();

                adp.Fill(dt);
                //Draws all the images onto the signature pad
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        SqlCommand command = new SqlCommand("ReadImage", con);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@imgId", i);
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable dat = new DataTable();
                        MemoryStream ms = new MemoryStream((byte[])dt.Rows[i]["Data"]);

                        System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(System.Drawing.Image.FromStream(ms), new System.Drawing.Size(640, 480));
                        int result = (int)spad.DisplaySetTarget(signotec.STPadLibNet.DisplayTarget.NewStandardStore);
                        /*Console.WriteLine("" + result);
                        if (result == 1)
                            Console.WriteLine("Error");
                         * */


                        spad.DisplaySetImage(0, 0, bitmap);
                    }
                }
                //signature pad display variables and the slidshow speed
                spad.DisplayConfigSlideShow("4;5;6;7;8;9;10;11;12;13", 3000);

                spad.DeviceClose(0);



            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
                Console.Read();
            }




            /* legacy code origionally this program grabed from a file not the database
            string[] pics = System.IO.Directory.GetFiles(@"C:\Users\tturner\Desktop", "*.bmp");
            for (int j = 0; j < pics.Length; j++)
            {
                if (j < 10)
                {
                    System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(System.Drawing.Image.FromFile(pics[j]), new System.Drawing.Size(640, 480));
                    int result = (int)spad.DisplaySetTarget(signotec.STPadLibNet.DisplayTarget.NewStandardStore);
                    /*Console.WriteLine("" + result);
                    if (result == 1)
                        Console.WriteLine("Error");
                    */
            /*
                    spad.DisplaySetImage(0, 0, bitmap);
                }
            }
            
            spad.DisplayConfigSlideShow("1;2;3;4;5;6;7;8;9;10;11;12;13", 3000);

            spad.DeviceClose(0);
             */


        }
    }
}