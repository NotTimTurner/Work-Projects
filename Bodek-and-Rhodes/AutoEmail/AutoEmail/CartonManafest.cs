using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace AutoEmail
{
    class CartonManafest
    {
        public string ShipmentID { get; set; }
        public string OrderNumber { get; set; }
        public string Status { get; set; }
        public string CustomerID { get; set; }
        public string Name { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZIP { get; set; }
        public string PurchaseOrder { get; set; }
        public DateTime PurchaseOrderDate { get; set; }
        public DateTime Schedulded_ship_date  { get; set; }
        public string Notes { get; set; }
        public string Terms { get; set; }
        public string Shipmode { get; set; }
        public int CartonLines = 0;
        public string CustName { get; set; }
        public string CustAdd1 { get; set; }
        public string CustAdd2 { get; set; }
        public string CustCity { get; set; }
        public string CustState { get; set; }
        public string CustZip { get; set; }
        public string TermsDesc { get; set; }
        public string ShipDesc { get; set; }
        

        public CartonManafestLine[] CML = new CartonManafestLine[9999];

        public CartonManafest(string InShipment_ID)
        {
            string sql = @"Select OBS.Shipment_ID ShipID, OBS.Order_Number OrdNum, 
     OBS.Status Status, OBS.Customer_ID CustID, 
    OBS.Name STName, OBS.Address_1 STAddr1, OBS.Address_2 STAddr2, 
    OBS.City STCity, OBS.State STState, OBS.Zip STZip, 
    OBS.Purchase_Order PO, OBS.Purchase_Order_Date ODate, OBS.Scheduled_Ship_Date SDate, 
    OBS.Notes Note, OBS.Payment_Terms Terms, OBS.Ship_Mode SMode, 
    OBD.Ordered_Units Units, PM.Short_Description Prod 
    From Outbound_Shipments OBS (nolock), Outbound_Shipment_Details OBD (nolock), Product_Master PM (nolock) 
    Where (OBS.Shipment_ID = OBD.Shipment_ID) And
    (OBS.Order_Number = OBD.Order_Number) And 
    (OBD.Product_ID = PM.Product_ID) And 
    (OBS.Shipment_ID = '" + InShipment_ID + "') And (CharIndex('EAG', QC_CODES) = 0) Order By OBS.Shipment_ID, OBS.Order_Number, PM.Product_ID ";

            DatabaseConnection  DbCon = new DatabaseConnection();
            SqlConnection Con = new SqlConnection(DbCon.GetConString());
            SqlDataReader MyReader = null;
            SqlCommand SqlCommand = new SqlCommand(sql, Con);
            Con.Open();
            MyReader = SqlCommand.ExecuteReader(CommandBehavior.SingleRow);
            while (MyReader.Read())
            {

                ShipmentID = MyReader["ShipID"].ToString();
                Status = MyReader["Status"].ToString();
                CustomerID = MyReader["CustID"].ToString();
                Name = MyReader["STName"].ToString();  
                Address1 = MyReader["STAddr1"].ToString();
                Address2 = MyReader["STAddr2"].ToString();
                City = MyReader["STCity"].ToString();
                State = MyReader["STState"].ToString();
                ZIP = MyReader["STZip"].ToString();
                PurchaseOrder = MyReader["PO"].ToString();
                PurchaseOrderDate =Convert.ToDateTime(MyReader["ODate"].ToString());
                Schedulded_ship_date =Convert.ToDateTime( MyReader["SDate"].ToString());
                Notes = MyReader["Note"].ToString();
                Terms = MyReader["TERMS"].ToString();
                Shipmode=MyReader["SMode"].ToString();
                GetCustomerInfo(CustomerID);
                GetTermsDescription(Terms);
                GetShipModeDescription(Shipmode);
               

            }
            MyReader.Close();
           
           
           
            SqlDataReader MyReader2 = SqlCommand.ExecuteReader();
            while (MyReader2.Read())
            {
                CML[CartonLines] = new CartonManafestLine();
                CML[CartonLines].OrderNumber = MyReader2["OrdNum"].ToString();
                CML[CartonLines].OrderedUnits = MyReader2["Units"].ToString();
                CML[CartonLines].ShortDescription = MyReader2["Prod"].ToString();
                CartonLines++;
            }


        }

        private void GetShipModeDescription(string Shipmode)
        {
            string sql = "Select Description From LookUp_Values (nolock) Where (Table_Name = 'OUTBOUND_SHIPMENTS') And (FIELD_NAME = 'Ship_Mode') And VALUE='" + Shipmode + "'";
            DatabaseConnection DbCon = new DatabaseConnection();
            SqlConnection Con = new SqlConnection(DbCon.GetConString());
            SqlDataReader MyReader = null;
            SqlCommand SqlCommand = new SqlCommand(sql, Con);
            Con.Open();
            MyReader = SqlCommand.ExecuteReader(CommandBehavior.SingleRow);
            while (MyReader.Read())
            {
                ShipDesc = MyReader["Description"].ToString();
            }
            if (ShipDesc == null||ShipDesc=="")
            {
                ShipDesc = "UNDEFINED";
            }
        }

        private void GetTermsDescription(string Terms)
        {
             string sql = "Select Description From LookUp_Values (nolock) Where (Table_Name = 'OUTBOUND_SHIPMENTS') And (FIELD_NAME = 'PAYMENT_TERMS') And VALUE='"+Terms+"'";
            DatabaseConnection  DbCon = new DatabaseConnection();
            SqlConnection Con = new SqlConnection(DbCon.GetConString());
            SqlDataReader MyReader = null;
            SqlCommand SqlCommand = new SqlCommand(sql, Con);
            Con.Open();
            MyReader = SqlCommand.ExecuteReader(CommandBehavior.SingleRow);
            while (MyReader.Read())
            {
                TermsDesc = MyReader["Description"].ToString();
            }
            if (TermsDesc == null||ShipDesc=="")
            {
                TermsDesc = "UNDEFINED";
            }
        }

        private void GetCustomerInfo(string CustomerID)
        {
            string sql = "Select Name, Address_1, Address_2, City, State, Zip From Customers (nolock) Where Customer_ID = '" + CustomerID + "'";
            DatabaseConnection  DbCon = new DatabaseConnection();
            SqlConnection Con = new SqlConnection(DbCon.GetConString());
            SqlDataReader MyReader = null;
            SqlCommand SqlCommand = new SqlCommand(sql, Con);
            Con.Open();
            MyReader = SqlCommand.ExecuteReader(CommandBehavior.SingleRow);
            while (MyReader.Read())
            {
                CustName = MyReader["Name"].ToString();
                CustAdd1 = MyReader["Address_1"].ToString();
                CustAdd2 = MyReader["Address_2"].ToString();
                CustCity = MyReader["City"].ToString();
                CustState = MyReader["State"].ToString();
                CustZip = MyReader["Zip"].ToString();
            }
            Con.Close();
        }
    }
}
