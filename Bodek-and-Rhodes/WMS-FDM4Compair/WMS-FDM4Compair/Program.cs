using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Data.OleDb;
using System.Text;
using Excel = Microsoft.Office.Interop.Excel;



namespace WMS_FDM4Compair
{
    class Program
    {
        static void Main(string[] args)
        {
            DataTable table = new DataTable();

            string directory = @"\\192.168.1.192\d$\Tim's Projects\WMS-FDM4Compair\Test";

            ReadExcel(directory,table);

            CompaireonHand(table);




          //  PrintDataTable(table);
           // Console.ReadLine();
        }

        private static void CompaireonHand(DataTable table)
        {
            DebugLog log = new DebugLog();
            Utility UT = new Utility();
            int count = 0;
            for (int i = 1; i < table.Rows.Count; i++)
            {
                string ProductID = table.Rows[i][0].ToString();
                //string ShortDesc= table.Rows[i][1].ToString();
                double OnHand = Convert.ToDouble(table.Rows[i][1].ToString());

                double WMSQuantity = UT.GetFullWMSQuantity(ProductID);

               // ExportToExcel(ProductID, ShortDesc, OnHand, WMSQuantity,count);
                count++;
                log.Write("ProductID: "+ProductID+" FDM4 Quantity: " + OnHand + "     WMS Quantity: " + WMSQuantity + "     Difference:" + (OnHand - WMSQuantity));
                log.newLine();
            }


        }

        private static void ExportToExcel(string ProductID, string ShortDesc, double OnHand, double WMSQuantity, int count)
        {
        


                string ExcelFilePath = @"\\192.168.1.192\d$\Tim's Projects\WMS-FDM4Compair\ExcelTest";
                
                // load excel, and create a new workbook
                Excel.Application excelApp = new Excel.Application();
                excelApp.Workbooks.Add();

                // single worksheet
                Excel._Worksheet workSheet = excelApp.ActiveSheet;

                // column headings
                if (count == 0)
                {
                    workSheet.Cells[1, 1] = "Product ID";
                    workSheet.Cells[1,2] = "Short Desc";
                    workSheet.Cells[1, 3] = "FDM4 quantity";
                    workSheet.Cells[1, 4] = "WMS Quantity";
                    workSheet.Cells[1, 5] = "Difference";
                }
                count++;
                

               //rows
                workSheet.Cells[count, 1] = ProductID;
                workSheet.Cells[count, 2] = ShortDesc;
                workSheet.Cells[count, 3] = OnHand;
                workSheet.Cells[count, 4] = WMSQuantity;
                workSheet.Cells[count, 5] = OnHand - WMSQuantity;


                // check fielpath
                if (ExcelFilePath != null && ExcelFilePath != "")
                {
                    try
                    {
                        workSheet.SaveAs(ExcelFilePath);
                        excelApp.Quit();
                      //  MessageBox.Show("Excel file saved!");
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("ExportToExcel: Excel file could not be saved! Check filepath.\n"
                            + ex.Message);
                    }
                }
                else    // no filepath is given
                {
                    excelApp.Visible = true;
                }
            }

            
        

        private static void PrintDataTable(DataTable table)
        {
            foreach (DataRow DR in table.Rows)
            {
                foreach (var item in DR.ItemArray)
                {
                    Console.Write(item);
                }
            }
        }



        public static void ReadExcel(string filePath, DataTable table)
        {
           // DataTable table = new DataTable();
            table.Columns.Add("SKU", typeof(string));
            //table.Columns.Add("Short Description", typeof(string));
            table.Columns.Add("On Hand", typeof(double));
            //... add as many columns as your excel file has!!

            string connString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0 Xml;HDR=YES;IMEX=1;TypeGuessRows=0;ImportMixedTypes=Text\"", filePath);
            using (OleDbConnection dbConn = new OleDbConnection(connString))
            {
                using (OleDbCommand cmd = new OleDbCommand(@"SELECT * FROM [Sheet1$]", dbConn))
                {
                    dbConn.Open();
                    using (OleDbDataReader reader = cmd.ExecuteReader())
                    {
                        DataRow row;
                        while (reader.Read())
                        {
                            row = table.NewRow();
                            row["SKU"] = (string)reader[0];
                           // row["Short Description"] =(string)reader[4].ToString();
                            row["On Hand"] = (double)reader[1];
                            table.Rows.Add(row);
                        }
                    }
                }
            }
        }
    }
}
