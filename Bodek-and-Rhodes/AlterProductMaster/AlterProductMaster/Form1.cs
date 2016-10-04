using System;
using System.Windows.Forms;
using System.IO;
using System.Data;
using System.Data.OleDb;
using System.Data;
using System.Data.SqlClient;

namespace AlterProductMaster
{
    public partial class Form1 : Form
    {
        DataTable dtExcel = new DataTable();
        public Form1()
        {
            InitializeComponent();
            button3.Visible = false;

           // label1.Text = "Choose a file";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string filePath = string.Empty;
            string fileExt = string.Empty;
            OpenFileDialog file = new OpenFileDialog(); //open dialog to choose file  
            if (file.ShowDialog() == System.Windows.Forms.DialogResult.OK) //if there is a file choosen by the user  
            {
                filePath = file.FileName; //get the path of the file  
                fileExt = Path.GetExtension(filePath); //get the file extension  
                if (fileExt.CompareTo(".xls") == 0 || fileExt.CompareTo(".xlsx") == 0)
                {
                    try
                    {
                       
                        dtExcel = ReadExcel(filePath, fileExt); //read excel file  
                        if (dtExcel.Rows.Count <= 200)
                        {
                            dataGridView1.Visible = true;
                            dataGridView1.DataSource = dtExcel;
                        }
                        else
                        {
                            MessageBox.Show("File too big to preview, but it still imported");
                        }
                        button3.Visible = true;
                      //  label1.Text = "Click Import to start importing the file to the database";
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString());
                    }
                }
                else
                {
                    MessageBox.Show("Please choose .xls or .xlsx file only.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error); //custom messageBox to show error  
                }
            }  
        }




        public DataTable ReadExcel(string fileName, string fileExt)
        {
            string conn = string.Empty;
            DataTable dtexcel = new DataTable();
            if (fileExt.CompareTo(".xls") == 0)
                conn = @"provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fileName + ";Extended Properties='Excel 8.0;HRD=Yes;IMEX=1';"; //for below excel 2007  
            else
                conn = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileName + ";Extended Properties='Excel 12.0;HDR=NO';"; //for above excel 2007  
            using (OleDbConnection con = new OleDbConnection(conn))
            {
                try
                {
                    OleDbDataAdapter oleAdpt = new OleDbDataAdapter("select * from [ckgtin$]", con); //here we read data from sheet1  
                    oleAdpt.Fill(dtexcel); //fill excel data into dataTable  
                }
                catch { }
            }
            return dtexcel;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int rows = dtExcel.Rows.Count;
           int colums= dtExcel.Columns.Count;
            Utility UT = new Utility();
            Validation valid=new Validation();

         //  UpdateMissingQTIN();

           MessageBox.Show("All files imported");

           for (int i = 0; i < rows; i++)
           {
               string Product_id = null;
               string GTIN = null;
               string shortDescription = null;
               string Caselevel = null;
               string CubePerUom = null;
               string EIS_Piece_weight = null;
               string Vendor_id = null;
               string EIS_Mill = null;
               string Base_uom = null;
               string description = null;

               Product_id = dtExcel.Rows[i][2].ToString();
              // shortDescription = dtExcel.Rows[i][1].ToString();
              // Caselevel = dtExcel.Rows[i][2].ToString();
              // CubePerUom = dtExcel.Rows[i][3].ToString();
              // EIS_Piece_weight = dtExcel.Rows[i][4].ToString();
             //  Vendor_id = dtExcel.Rows[i][5].ToString();
             //  EIS_Mill = dtExcel.Rows[i][6].ToString();
             //  Base_uom = dtExcel.Rows[i][7].ToString();
               GTIN = dtExcel.Rows[i][3].ToString();
               if (valid.CheckIfGTINExistsInXref(GTIN) == false)
               {
                  // Product_Master PM = new Product_Master(Product_id, shortDescription, Caselevel, CubePerUom, EIS_Piece_weight, Vendor_id, EIS_Mill, Base_uom, description);
                 //  PM.UpdateDatabase();
                   DatabaseConnection DbCon = new DatabaseConnection();
                   DataSet ds = new DataSet();
                   string conString = DbCon.GetConString();
                   DbCon.connection_String = conString;
                   DbCon.Sql = "insert into GTIN_XREF values('" + GTIN + "','" + Product_id + "','tturner',getdate(),'UpdateFromExcel')";
                    ds = DbCon.GetConnection;
               }
               // label1.Text = "Updating Product: " + Product_id;


           }


               
        }



        private void UpdateMissingQTIN()
        {
            
            DebugLog log = new DebugLog();
           DatabaseConnection DbCon = new DatabaseConnection();
            SqlConnection Con = new SqlConnection(DbCon.GetConString());
            SqlDataReader MyReader = null;
            SqlCommand SqlCommand = new SqlCommand("select product_master.Product_id  from product_master where  not exists(select product_id from GTIN_XREF where product_master.product_id=GTIN_XREF.product_id) order by product_id", Con);
            Con.Open();
            MyReader = SqlCommand.ExecuteReader();
            while (MyReader.Read())
            {
                string Product_id = "";
                string InGTIN = "";
                Product_id = MyReader["Product_id"].ToString();
                 int rows = dtExcel.Rows.Count;

                 for (int i = 0; i < rows; i++)
                 {
                     if( dtExcel.Rows[i][3].ToString()==Product_id)
                     {
                         Validation valid = new Validation();
                          InGTIN=dtExcel.Rows[i][4].ToString();
                        if( valid.checkIfProdectExistsInProductMaster(Product_id)==true)
                        {
                            Utility UT = new Utility();
                            UT.CreateNewGTINReference(Product_id, InGTIN);
                        }

                        
                     }
                 }

              //  GTIN=dtExcel.Select()


            }
            Con.Close();
        } 


     
    }
}
