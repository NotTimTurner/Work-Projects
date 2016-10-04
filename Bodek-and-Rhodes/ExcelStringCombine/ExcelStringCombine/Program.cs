using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Data.OleDb;
using System.Text;
using Excel = Microsoft.Office.Interop.Excel;

namespace ExcelStringCombine
{
    class Program
    {
        static void Main(string[] args)
        {

            DataTable table = new DataTable();

            string directory = @"\\192.168.1.192\d$\Tim's Projects\WMS-FDM4Compair\Test2";
            string testfile = @"\\192.168.1.192\d$\Tim's Projects\WMS-FDM4Compair\Test3";

            ReadExcel(directory, table);
            writetoExcel(table,testfile);
        }

        private static void writetoExcel(DataTable table, string PathFileName)
        {
            string ExcelFilePath = @"\\192.168.1.192\d$\Tim's Projects\WMS-FDM4Compair\ExcelTest";

            // load excel, and create a new workbook
            Excel.Application excelApp = new Excel.Application();
            excelApp.Workbooks.Add();

            // single worksheet
            Excel._Worksheet workSheet = excelApp.ActiveSheet;
                workSheet.Name = "TestSheet";
                workSheet.Cells[1, "A"].Value2 = "Product_id";
                workSheet.Cells[1, "B"].Value2 = "Description";
               

                /* Insert Rows */
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    workSheet.Cells[i + 2, "A"].Value2 = table.Rows[i][0]; // EmployeeNumber
                    workSheet.Cells[i + 2, "B"].Value2 += table.Rows[i][1]; // Time From
                   
                }

                workSheet.SaveAs(ExcelFilePath);
                excelApp.Quit();
           
        }

        public static void ReadExcel(string filePath, DataTable table)
        {
            // DataTable table = new DataTable();
            table.Columns.Add("ProdID", typeof(string));
            //table.Columns.Add("Short Description", typeof(string));
            table.Columns.Add("combined string", typeof(string));
            // table.Columns.Add("lab Description", typeof(double));
            //... add as many columns as your excel file has!!

            string connString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0 Xml;HDR=YES;IMEX=1;TypeGuessRows=0;ImportMixedTypes=Text\"", filePath);
            using (OleDbConnection dbConn = new OleDbConnection(connString))
            {
                using (OleDbCommand cmd = new OleDbCommand(@"SELECT * FROM [ckgtin$]", dbConn))
                {
                    dbConn.Open();
                    using (OleDbDataReader reader = cmd.ExecuteReader())
                    {
                        DataRow row;
                        while (reader.Read())
                        {
                            if(reader[2]!=System.DBNull.Value &&reader[8]!=null&&reader[9]!=null)
                            {
                            row = table.NewRow();

                            row["ProdID"] = (string)reader[2];
                            // row["Short Description"] =(string)reader[4].ToString();
                            row["combined string"] = (string)reader[8].ToString() +" "+ (string)reader[9].ToString();
                            //  row["lab Description"] = (double)reader[8];
                            table.Rows.Add(row);
                            }
                        }
                    }
                }
            }
        }
           
    }
}
