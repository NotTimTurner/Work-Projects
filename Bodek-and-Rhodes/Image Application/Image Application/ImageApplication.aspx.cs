using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using System.Data.SqlClient;



namespace Image_Application
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        //connects to the database, then tries to execute a query
        private Boolean InsertUpdateData(SqlCommand cmd)
        {
            String strConnString = "Data Source=192.168.1.192;Initial Catalog=WMS;User ID=bodek;";
            SqlConnection con = new SqlConnection(strConnString);
            cmd.CommandType = CommandType.Text;
            cmd.Connection = con;
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
                return false;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }



       

        protected void UploadButton_Click(object sender, EventArgs e)
        {


            // Read the file and convert it to Byte Array
            string filePath = FileUpload1.PostedFile.FileName;
            string filename = Path.GetFileName(filePath);
            string ext = Path.GetExtension(filename);
            string contenttype = String.Empty;

            //Set the contenttype based on File Extension
            switch (ext)
            {
             
                case ".bmp":
                    contenttype = "image/bmp";
                    break;
            }
            if (contenttype != String.Empty)
            {

                Stream fs = FileUpload1.PostedFile.InputStream;
                BinaryReader br = new BinaryReader(fs);
                Byte[] bytes = br.ReadBytes((Int32)fs.Length);
                DateTime date= new DateTime();
                date= DateTime.Now;
                

                //insert the file into database
                string strQuery = "insert into Advertisement_Images(Name, ContentType, Data, Date)" +
                   " values (@Name, @ContentType, @Data, @Date)";
                SqlCommand cmd = new SqlCommand(strQuery);
                cmd.Parameters.Add("@Name", SqlDbType.VarChar).Value = filename;
                cmd.Parameters.Add("@ContentType", SqlDbType.VarChar).Value
                  = contenttype;
                cmd.Parameters.Add("@Data", SqlDbType.Binary).Value = bytes;
                cmd.Parameters.Add("@Date", SqlDbType.DateTime).Value = date;
                InsertUpdateData(cmd);
                lblMessage.ForeColor = System.Drawing.Color.Green;
                lblMessage.Text = "File Uploaded Successfully";
            }
            else
            {
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = "File format not recognised." +
                  " Upload a bmp file";
            }

        }

        

    }   
    
 }
