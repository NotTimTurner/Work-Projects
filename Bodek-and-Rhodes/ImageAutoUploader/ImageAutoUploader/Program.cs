using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using System.Data.SqlClient;

/// <summary>
/// Author: Tim Turner
/// Creation Data: 3/15/2015
/// Purpose: To grab images from a file( the images are provided by the web team) 
///          and insert them into the Warehousestrg Database so that the POS signiture pads
///          can just call the database instead of trying to update from a file
/// imporvments: instead of Deleting all the items in the table every day and readding them
///              have it check and see if the files are different from the ones in the database        
/// </summary>




namespace ImageAutoUploader
{
    class Program
    {
        static void Main(string[] args)

        { 
            //deletes the old images from the database
            string strQuery1 = "truncate table Advertisement_Images";
            SqlCommand cmd1 = new SqlCommand(strQuery1);
          

            String strConnString = "Data Source=192.168.1.192;Initial Catalog=WMS;User ID=bodek;";
            SqlConnection con1 = new SqlConnection(strConnString);
            cmd1.CommandType = CommandType.Text;
            cmd1.Connection = con1;
           
                try
                {
                    con1.Open();
                    cmd1.ExecuteNonQuery();
                   
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.ReadLine();
                    
                }
                finally
                {
                    con1.Close();
                    con1.Dispose();
                }
               
            


            //gets the files and stores them in filepaths
            String[] filePaths = Directory.GetFiles(@"\\br-pa-fs4\WMSImages", "*.bmp");


            //for each file it will get the picture and put it in the database
            for(int i=0;i<filePaths.Length;i++)
            {
                //gets the file path
            string filename = Path.GetFileName(filePaths[i]);
            string ext = Path.GetExtension(filename);
            string contenttype = String.Empty;
            DateTime date = new DateTime();
            date = DateTime.Now;

            //calls getphoto PathTooLongException grab the image 
            byte[] image = GetPhoto(filePaths[i]);
            //inserting the picture into the database
            string strQuery2 = "insert into Advertisement_Images(Name, ContentType, Data, Date)" +
                   " values (@Name, @ContentType, @Data, @Date)";
                SqlCommand cmd2 = new SqlCommand(strQuery2);
                cmd2.Parameters.Add("@Name", SqlDbType.VarChar).Value = filename;
                cmd2.Parameters.Add("@ContentType", SqlDbType.VarChar).Value
                  = contenttype;
                cmd2.Parameters.Add("@Data", SqlDbType.Binary).Value = image;
                cmd2.Parameters.Add("@Date", SqlDbType.DateTime).Value = date;


               
                SqlConnection con2 = new SqlConnection(strConnString);
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = con2;
                try
                {
                    con2.Open();
                    cmd2.ExecuteNonQuery();
                   
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    
                }
                finally
                {
                    con2.Close();
                    con2.Dispose();
                }
               
            }
         
           

          




        
        }


       //takes the file path and converts the picture to type byte
        public static byte[] GetPhoto(string filePath)
        {
            FileStream stream = new FileStream(
                filePath, FileMode.Open, FileAccess.Read);
            BinaryReader reader = new BinaryReader(stream);

            byte[] photo = reader.ReadBytes((int)stream.Length);

            reader.Close();
            stream.Close();

            return photo;
        }
        
    }
}
