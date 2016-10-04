using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Configuration;
using System.Data.Sql;
using System.Data.SqlClient;
/// <summary>
/// Summary description for Validation
/// </summary>
public class Validation
{
    DatabaseConnection DbCon = new DatabaseConnection();
    string conString;
    DataSet ds;
    DataRow dRow;

    int MaxRows;
    int inc = 0;

    public Validation()
    {
        //
        // TODO: Add constructor logic here
        //
    }



    public bool IsValidLocation(string LocationID)
    {
        try
        {
            DbCon = new DatabaseConnection();
            conString = DbCon.GetPhillyConString();
            DbCon.connection_String = conString;
            DbCon.Sql = "select Location_id from Locations where location_id='" + LocationID + "'";
            ds = DbCon.GetConnection;
            MaxRows = ds.Tables[0].Rows.Count;

            if (MaxRows == 1)
                return true;
            else
                return false;

        }
        catch
        {
            return false;

        }

    }

    public string GetZone(string LocationID)
    {

        char zone = LocationID[0];
        zone = Char.ToUpper(zone);
        return (zone.ToString());
    }

    public bool IsValidOrder(string Source)
    {
        try
        {
            if (Source == "")
                return false;

            string shipmentID = GetShipmentID(Source);
            string OrderNumber = GetOrderNumber(Source);
            DbCon = new DatabaseConnection();
            conString = DbCon.GetPhillyConString();
            DbCon.connection_String = conString;
            DbCon.Sql = "select _rid_ from Move_queue where shipment_id='" + shipmentID + "' and Order_Number='" + OrderNumber + "'";
            ds = DbCon.GetConnection;
            MaxRows = ds.Tables[0].Rows.Count;

            if (MaxRows != 0)
                return true;
            else
                return false;
        }
        catch
        {

            return false;
        }

    }

    public string GetOrderNumber(string Source)
    {
        return (Source.Substring(10, 4));

    }

    public string GetShipmentID(string Source)
    {

        return (Source.Substring(0, 10));
    }

    public bool IsThereAPriority1ReplenComingHere(string PiecePickLoation)
    {
        DbCon = new DatabaseConnection();
        conString = DbCon.GetPhillyConString();
        DbCon.connection_String = conString;
        DbCon.Sql = "select To_Location from Move_queue where To_Location='" + PiecePickLoation + "' and Priority='1'";
        ds = DbCon.GetConnection;
        MaxRows = ds.Tables[0].Rows.Count;

        if (MaxRows > 0)
        {
            return true;
        }
        return false;

    }

    public bool IsThereAPriority30ReplenComingHere(string PiecePickLocation)
    {
        DbCon = new DatabaseConnection();
        conString = DbCon.GetPhillyConString();
        DbCon.connection_String = conString;
        DbCon.Sql = "select To_Location from Move_queue where To_Location='" + PiecePickLocation + "' and Priority='30'";
        ds = DbCon.GetConnection;
        MaxRows = ds.Tables[0].Rows.Count;

        if (MaxRows > 0)
        {
            return true;
        }
        return false;
    }

    public bool ShipmentStillHasMoveQs(string BoxID)
    {
        if (BoxID == "")
        {
            return false;
        }

        string shipmentID = GetShipmentID(BoxID);
        string OrderNumber = GetOrderNumber(BoxID);
        DbCon = new DatabaseConnection();
        conString = DbCon.GetPhillyConString();
        DbCon.connection_String = conString;
        DbCon.Sql = "select _rid_ from Move_Queue where shipment_id='" + shipmentID + "'and Order_number='" + OrderNumber + "'";
        ds = DbCon.GetConnection;
        MaxRows = ds.Tables[0].Rows.Count;

        if (MaxRows > 0)
        {
            return true;
        }

        return false;

    }

    public bool ExistsInMoveQueue(string orderLine, string BoxID)
    {
        try
        {

            if (BoxID == "")
                return false;

            string ShipmentID = GetShipmentID(BoxID);
            string OrderNumber = GetOrderNumber(BoxID);
            DbCon = new DatabaseConnection();
            conString = DbCon.GetPhillyConString();
            DbCon.connection_String = conString;
            DbCon.Sql = "select _RID_ from Move_queue where shipment_id='" + ShipmentID + "' and order_number='" + OrderNumber + "'and order_Line='" + orderLine + "'";
            ds = DbCon.GetConnection;
            MaxRows = ds.Tables[0].Rows.Count;

            if (MaxRows >= 1)
                return true;
            else
                return false;

        }
        catch
        {
            return false;

        }

    }

    public bool ProductIsInTHisOrder(string ShipmentID, string OrderNumber,string productID)
    {
         if (ShipmentID== ""||OrderNumber=="")
                return false;

            
            DbCon = new DatabaseConnection();
            conString = DbCon.GetPhillyConString();
            DbCon.connection_String = conString;
            DbCon.Sql="SELECT MOVE_QUEUE._RID_, INVENTORY.PRODUCT_ID FROM INVENTORY INNER JOIN MOVE_QUEUE ON INVENTORY._RID_ = MOVE_QUEUE.INV_RID WHERE MOVE_QUEUE.shipment_id='" + ShipmentID + "' and MOVE_QUEUE.order_number='" + OrderNumber + "'and Inventory.Product_Id='" + productID + "' and Move_Queue.type='PICK'"; 
            ds = DbCon.GetConnection;
            MaxRows = ds.Tables[0].Rows.Count;

            if (MaxRows >= 1)
                return true;
            else
                return false;

        
    }

    public bool IsLocationInOrder(string locationID, string ShipmentID, string OrderNumber)
    {
        if (ShipmentID == "" || OrderNumber == "")
            return false;


        DbCon = new DatabaseConnection();
        conString = DbCon.GetPhillyConString();
        DbCon.connection_String = conString;
        DbCon.Sql = "SELECT _RID_ from move_queue WHERE shipment_id='" + ShipmentID + "' and order_number='" + OrderNumber + "' and Move_Queue.type='PICK' and From_Location='"+locationID+"'";
        ds = DbCon.GetConnection;
        MaxRows = ds.Tables[0].Rows.Count;

        if (MaxRows >= 1)
            return true;
        else
            return false;
    }

    public bool LocationInZone(string locationID, string CurrentZOne)
    {
        if (locationID == "" || CurrentZOne == "")
            return false;

        string InZone = GetZone(locationID);
        
       
        
        if (InZone == CurrentZOne)
        {
            return true;
        }
        return false;
    }

    public bool DescriptionInThisOrder(string ShipmentID, string OrderNumber, string ProductID, string LocationID)
    {
        if (ShipmentID == "" || OrderNumber == "")
            return false;


        DbCon = new DatabaseConnection();
        conString = DbCon.GetPhillyConString();
        DbCon.connection_String = conString;
        DbCon.Sql = "SELECT move_queue._rid_ from Move_queue inner join Product_master on Move_queue.From_Location=Product_master.Piece_Pick_Location where move_queue.shipment_id='" + ShipmentID + "' and move_queue.order_Number='" + OrderNumber + "' and product_master.Short_Description='" + ProductID + "'";
        ds = DbCon.GetConnection;
        MaxRows = ds.Tables[0].Rows.Count;
        if (MaxRows >= 1)
            return true;
        else
            return false;
    }

  

    public bool ProductInEnteredLocation(string ShipmentID, string OrderNumber, string LocationID, string ProductID)
    {
        if (ShipmentID == "" || OrderNumber == "")
            return false;


        DbCon = new DatabaseConnection();
        conString = DbCon.GetPhillyConString();
        DbCon.connection_String = conString;
        DbCon.Sql = "SELECT Product_id from Product_Master where Product_ID='" + ProductID + "' and Piece_Pick_Location='" + LocationID + "'"; 
        ds = DbCon.GetConnection;
        MaxRows = ds.Tables[0].Rows.Count;

        if (MaxRows >= 1)
            return true;
        else
            return false;

    }

    public bool DescriptionInEnteredLocation(string LocationID, string ProductID)
    {
        if (LocationID == "" || ProductID == "")
            return false;


        DbCon = new DatabaseConnection();
        conString = DbCon.GetPhillyConString();
        DbCon.connection_String = conString;
        DbCon.Sql = "SELECT Product_id from Product_Master where Short_Description='" + ProductID + "' and Piece_Pick_Location='" + LocationID + "'";
        ds = DbCon.GetConnection;
        ds = DbCon.GetConnection;
        MaxRows = ds.Tables[0].Rows.Count;

        if (MaxRows >= 1)
            return true;
        else
            return false;
    }



    public bool IsValidQuantity(string shipmentID, string OrderNumber, string LocationID, string Quantity)
    {
        if (LocationID == "" || shipmentID == ""||Quantity==""||Quantity=="0")
            return false;


        DbCon = new DatabaseConnection();
        conString = DbCon.GetPhillyConString();
        DbCon.connection_String = conString;
        DbCon.Sql = "SELECT _rid_ from move_queue where from_location='"+LocationID+"' and shipment_id='"+shipmentID+"' and order_number='"+OrderNumber+"' and quantity_Required='"+Quantity+"'";
        ds = DbCon.GetConnection;
        MaxRows = ds.Tables[0].Rows.Count;

        if (MaxRows >= 1)
            return true;
        else
            return false;
    }

    public void CreateUser(string UserName, string Password, string FIrstName, string LastName)
    {
        DbCon = new DatabaseConnection();
        conString = DbCon.GetTestConString();
        DbCon.connection_String = conString;
        DbCon.Sql = "insert into WMS_Users(User_ID,Password,First_Name,Last_Name,qc_required) values('"+UserName+"','"+Password+"','"+FIrstName+"','"+LastName+"',1)";
        ds = DbCon.GetConnection;
    }

    public string GtinToPruductID(string GTIN)
    {
        string ProductID="";
        DbCon=new DatabaseConnection();
        SqlConnection Con = new SqlConnection(DbCon.GetPhillyConString());
        SqlDataReader MyReader=null;
        SqlCommand SqlCommand = new SqlCommand("SELECT PRODUCT_ID FROM GTIN_XREF with(NOLOCK) WHERE GTIN = '" + GTIN + "'",Con);
        Con.Open();
        MyReader = SqlCommand.ExecuteReader();
        while (MyReader.Read())
        {
            ProductID= MyReader["Product_ID"].ToString();
        }
        Con.Close();
        return ProductID;
    }

    public bool inNumeric(string Product)
    {
        int n;
        bool Numeric = int.TryParse(Product, out n);
        return Numeric;
    }

    public bool AllPicksDone(string shipmentID, string OrderNumber, string PickZone)
    {
        DbCon = new DatabaseConnection();
        conString = DbCon.GetPhillyConString();
        DbCon.connection_String = conString;
        DbCon.Sql = "SELECT _rid_ from move_queue where shipment_id='" + shipmentID + "' and order_number='" + OrderNumber + "' and from_zone='" + PickZone + "' and type='PICK'";
        ds = DbCon.GetConnection;
        MaxRows = ds.Tables[0].Rows.Count;

        if (MaxRows >= 1)
            return false;
        else
            return true;
    }

    public bool OrderDone(string shipmentID, string OrderNumber)
    {
        DbCon = new DatabaseConnection();
        conString = DbCon.GetPhillyConString();
        DbCon.connection_String = conString;
        DbCon.Sql = "SELECT _rid_ from move_queue where shipment_id='" + shipmentID + "' and order_number='" + OrderNumber + "' and type='PICK'";
        ds = DbCon.GetConnection;
        MaxRows = ds.Tables[0].Rows.Count;

        if (MaxRows >= 1)
            return false;
        else
            return true;
    }

    public bool IsQCRequired(string UserID)
    {
        bool QCRequired;
        int QCExpired;
        DbCon = new DatabaseConnection();
        SqlConnection Con = new SqlConnection(DbCon.GetPhillyConString());
        SqlDataReader MyReader = null;
        SqlCommand SqlCommand = new SqlCommand("SELECT QC_Required, DateDiff(d,getdate(), ISNULL(QC_Expire_Date,Getdate()+1)) AS qc_expire FROM Afx_Users WHERE Upper(user_id) = '"+UserID+"'", Con);
        Con.Open();
        MyReader = SqlCommand.ExecuteReader();
        while (MyReader.Read())
        {
            QCRequired=Convert.ToBoolean(MyReader["QC_Required"]);
            QCExpired=Convert.ToInt32(MyReader["qc_expire"]);
            if (QCRequired == true && QCExpired > 0)
            {
                Con.Close();
                return true;
            }
            else
            {
                Con.Close();
                return false;
            }
        }
        Con.Close();
        return true;

    }



    internal bool IsThereAPickInThisZone(string FromZone, string ShipmentID, string OrderNumber)
    {
        DbCon = new DatabaseConnection();
        conString = DbCon.GetPhillyConString();
        DbCon.connection_String = conString;
        DbCon.Sql = "select _RID_ from move_queue where from_zone='"+FromZone+"' and shipment_ID='"+ShipmentID+"' and order_number='"+OrderNumber+"'";
        ds = DbCon.GetConnection;
        MaxRows = ds.Tables[0].Rows.Count;

        if (MaxRows > 0)
        {
            return true;
        }
        return false;
    }
}