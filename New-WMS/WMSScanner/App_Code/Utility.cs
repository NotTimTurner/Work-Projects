﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

/// <summary>
/// Summary description for Utility
/// </summary>
public class Utility
{
    DatabaseConnection DbCon = new DatabaseConnection();
    string conString;
    DataSet ds;

    public Utility()
    {
        //
        // TODO: Add constructor logic here
        //
    }


    public string GetOrderLine(string shipmentID, string OrderNumber, string OrderedUnits, string LocationID)
    {

        Int16 OrderLine = 0;
        DbCon = new DatabaseConnection();
        SqlConnection Con = new SqlConnection(DbCon.GetPhillyConString());
        SqlDataReader MyReader = null;
        SqlCommand SqlCommand = new SqlCommand("Select top 1 Order_line from Move_queue WHERE shipment_id='" + shipmentID + "' and Order_number='" + OrderNumber + "' and Quantity_Required='" + OrderedUnits + "' and From_location='" + LocationID + "'", Con);
        Con.Open();
        MyReader = SqlCommand.ExecuteReader();
        while (MyReader.Read())
        {
            OrderLine = Convert.ToInt16(MyReader["Order_Line"]);
        }
        Con.Close();
        return OrderLine.ToString();

    }



    public void UpdateOBSDLineStatus(string shipmentID, string OrderNumber, string orderLine)
    {

        DbCon = new DatabaseConnection();
        conString = DbCon.GetTestConString();
        DbCon.connection_String = conString;
        DbCon.Sql = "update outbound_shipment_details set Status='C' where shipment_id='" + shipmentID + "' and order_number='" + OrderNumber + "' and  order_line='" + orderLine + "'";
        ds = DbCon.GetConnection;
    }

    public void UpdateOoutboundShipments(string shipmentID, string OrderNumber)
    {
        DbCon = new DatabaseConnection();
        conString = DbCon.GetPhillyConString();
        DbCon.connection_String = conString;
        DbCon.Sql = "update outbound_shipments set Status='C' where shipment_id='" + shipmentID + "' and order_number='" + OrderNumber + "'";
        ds = DbCon.GetConnection;


    }

    public void SetQCRequired(string ShipmentID, string OrderNumber)
    {
        DbCon = new DatabaseConnection();
        conString = DbCon.GetPhillyConString();
        DbCon.connection_String = conString;
        DbCon.Sql = "UPDATE Outbound_Shipments WITH (ROWLOCK) SET QC_Required = 'Y' where shipment_id='" + ShipmentID + "' and order_number='" + OrderNumber + "'";
        ds = DbCon.GetConnection;
    }

    public void DeleteInventoryAllocation(string shipmentID, string OrderNumber, string orderLine)
    {
        DbCon = new DatabaseConnection();
        conString = DbCon.GetPhillyConString();
        DbCon.connection_String = conString;
        DbCon.Sql = "delete from inventory_allocation where shipment_id='" + shipmentID + "' and order_number='" + OrderNumber + "'and order_line='" + orderLine + "'";
        ds = DbCon.GetConnection;
    }

    public void deleteMoveQueue(string shipmentID, string OrderNumber, string orderLine)
    {
        DbCon = new DatabaseConnection();
        conString = DbCon.GetPhillyConString();
        DbCon.connection_String = conString;
        DbCon.Sql = "delete from Move_Queue where shipment_id='" + shipmentID + "' and order_number='" + OrderNumber + "'and order_line='" + orderLine + "'";
        ds = DbCon.GetConnection;
    }

    public string GetInventoryRID(string shipmentID, string OrderNumber, string orderLine, string Quantity)
    {
        string InvRid = "";
        DbCon = new DatabaseConnection();
        SqlConnection Con = new SqlConnection(DbCon.GetPhillyConString());
        SqlDataReader MyReader = null;
        SqlCommand SqlCommand = new SqlCommand("Select top 1 Inv_rid from Move_queue WHERE shipment_id='" + shipmentID + "' and Order_number='" + OrderNumber + "' and Quantity_Required='" + Quantity + "' and order_line='" + orderLine + "'", Con);
        Con.Open();
        MyReader = SqlCommand.ExecuteReader();
        while (MyReader.Read())
        {
            InvRid = MyReader["Inv_rid"].ToString();
        }
        Con.Close();
        return InvRid;
    }

    public void UpdateInventory(string UserID, string Quantity, string InventoryRID)
    {
        DbCon = new DatabaseConnection();
        conString = DbCon.GetPhillyConString();
        DbCon.connection_String = conString;
        DbCon.Sql = "UPDATE INVENTORY WITH (ROWLOCK) SET Shipment_ID = NULL, Order_Number = NULL, Order_Line = NULL, Quantity = ISNULL(Quantity, 0) - " + Quantity + ", Reservation_Type = NULL, Reserved = 0, _LAST_USER_ = '" + UserID + "', _LAST_UPDATED_ = GetDate(), _LAST_MODULE_ = 'RFPIECEPICK' WHERE (_RID_ = " + InventoryRID + ") ";
        ds = DbCon.GetConnection;
    }

    public void MoveToQCEmpty(string InventoryRID, string ShipmentId, string Order_Number, string OrderLine)
    {
        DbCon = new DatabaseConnection();
        conString = DbCon.GetPhillyConString();
        DbCon.connection_String = conString;
        DbCon.Sql = "UPDATE INVENTORY WITH (ROWLOCK) SET Data2str = Location_ID, Location_ID = 'QCEMPTY' , Shipment_id = '" + ShipmentId + "' , Order_Number = '" + Order_Number + "' , Order_Line = '" + OrderLine + "' WHERE _RID_ = " + InventoryRID + " AND ISNULL(QUANTITY, 0) = 0 ";
        ds = DbCon.GetConnection;
    }

    public void InsertIntoAuditInventory(string UserID, string Quantity, string shipmentID, string OrderNumber, string orderLine, string ProductID, string LocationID, string InventoryRID)
    {
        int AudInvRID = GetNextAuditInventoryRID();
        if (AudInvRID != 0)
        {
            //string InvRID=GetInventoryRID(shipmentID,OrderNumber,orderLine,Quantity);
            string InvTrace = GetInventoryTrace(InventoryRID);
            string CurrentQTY = CurrentInvQuantity(InventoryRID);
            int QtyBeforePick = (Convert.ToInt32(CurrentQTY)) + (Convert.ToInt32(Quantity));
            string ToLocation = GetToLocation(shipmentID, OrderNumber, orderLine, Quantity);
            DbCon = new DatabaseConnection();
            conString = DbCon.GetPhillyConString();
            DbCon.connection_String = conString;



            DbCon.Sql = "INSERT INTO AUDIT_INVENTORY (_RID_, TRANTYPE, SUBTYPE, TRANSACTION_TIME, USER_ID, INV_RID, PRODUCT_ID, INV_TRACE, INV_QTY, INV_QTY_OLD, UOM,LOCATION_ID, LOCATION_ID_OLD, _LAST_UPDATED_, _LAST_USER_, _LAST_MODULE_) VALUES (" + AudInvRID + " , 'MOVE', 'PICK', GetDate() ,'" + UserID + "', '" + InventoryRID + "', '" + ProductID + "', '" + InvTrace + "', '" + CurrentQTY + "','" + QtyBeforePick + "', 'EA', '" + ToLocation + "', '" + LocationID + "', GetDate(), '" + UserID + "', 'RFPIECEPICK') ";
            ds = DbCon.GetConnection;

            inserIntoAuditInventoryDetail(AudInvRID, InventoryRID, shipmentID, OrderNumber, orderLine, ProductID, InvTrace, UserID);

        }
    }

    private void inserIntoAuditInventoryDetail(int AudInvRID, string InvRID, string shipmentID, string OrderNumber, string orderLine, string ProductID, string InvTrace, string UserID)
    {
        int AudInvDetailRID = GetNextAuditInventoryDetailRID();
        string CustomerID = GetCustomerID(shipmentID);
        DbCon = new DatabaseConnection();
        conString = DbCon.GetPhillyConString();
        DbCon.connection_String = conString;



        DbCon.Sql = "INSERT INTO AUDIT_INVENTORY_DETAIL (_RID_, AUDIT_INVENTORY_RID, INV_ID, CUSTOMER_ID, SHIPMENT_ID, ORDER_NUMBER, ORDER_LINE, PRODUCT_ID, STATUS, INV_TRACE, _LAST_UPDATED_, _LAST_USER_, _LAST_MODULE_)  Values ('" + AudInvDetailRID + "', '" + AudInvRID + "', '" + InvRID + "', '" + CustomerID + "', '" + shipmentID + "', '" + OrderNumber + "', '" + orderLine + "', '" + ProductID + "', 'PICKED', '" + InvTrace + "', GetDate(), '" + UserID + "', 'RFPIECEPICK')";


        ds = DbCon.GetConnection;
    }

    private string GetCustomerID(string shipmentID)
    {
        string CustomerID = "";
        DbCon = new DatabaseConnection();
        SqlConnection Con = new SqlConnection(DbCon.GetPhillyConString());
        SqlDataReader MyReader = null;
        SqlCommand SqlCommand = new SqlCommand("Select CUSTOMER_ID From OUTBOUND_SHIPMENTS(NOLOCK) Where SHIPMENT_ID = '" + shipmentID + "' Group By CUSTOMER_ID ", Con);
        Con.Open();
        MyReader = SqlCommand.ExecuteReader();
        while (MyReader.Read())
        {
            CustomerID = MyReader["CUSTOMER_ID"].ToString();
        }
        Con.Close();
        return CustomerID;
    }

    private int GetNextAuditInventoryDetailRID()
    {
        DbCon = new DatabaseConnection();
        conString = DbCon.GetPhillyConString();
        DbCon.connection_String = conString;
        DbCon.Sql = "Update SystemNextRID_AUDINVD Set NextRID = NextRID + 1 ";
        ds = DbCon.GetConnection;

        int Rid = 0;
        DbCon = new DatabaseConnection();
        SqlConnection Con = new SqlConnection(DbCon.GetPhillyConString());
        SqlDataReader MyReader = null;
        SqlCommand SqlCommand = new SqlCommand("Select NextRID From SystemNextRID_AUDINVD", Con);
        Con.Open();
        MyReader = SqlCommand.ExecuteReader();
        while (MyReader.Read())
        {
            Rid = Convert.ToInt32(MyReader["NextRID"]);
        }
        Con.Close();
        return Rid;
    }

    private string GetToLocation(string shipmentID, string OrderNumber, string orderLine, string Quantity)
    {
        string ToLocation = "";
        DbCon = new DatabaseConnection();
        SqlConnection Con = new SqlConnection(DbCon.GetPhillyConString());
        SqlDataReader MyReader = null;
        SqlCommand SqlCommand = new SqlCommand("Select To_Location from Move_Queue WHERE shipment_id='" + shipmentID + "' and Order_number='" + OrderNumber + "' and Quantity_Required='" + Quantity + "' and order_line='" + orderLine + "'", Con);
        Con.Open();
        MyReader = SqlCommand.ExecuteReader();
        while (MyReader.Read())
        {
            ToLocation = MyReader["Data1str"].ToString();
        }
        Con.Close();
        return ToLocation;
    }

    private string CurrentInvQuantity(string InvRID)
    {
        string quantity = "";
        DbCon = new DatabaseConnection();
        SqlConnection Con = new SqlConnection(DbCon.GetPhillyConString());
        SqlDataReader MyReader = null;
        SqlCommand SqlCommand = new SqlCommand("Select quantity from inventory where _rid_='" + InvRID + "'", Con);
        Con.Open();
        MyReader = SqlCommand.ExecuteReader();
        while (MyReader.Read())
        {
            quantity = MyReader["quantity"].ToString();
        }
        Con.Close();
        return quantity;
    }

    private string GetInventoryTrace(string InvRID)
    {
        string InvTrace = "";
        DbCon = new DatabaseConnection();
        SqlConnection Con = new SqlConnection(DbCon.GetPhillyConString());
        SqlDataReader MyReader = null;
        SqlCommand SqlCommand = new SqlCommand("Select top 1 Data1str from inventory where _rid_='" + InvRID + "'", Con);
        Con.Open();
        MyReader = SqlCommand.ExecuteReader();
        while (MyReader.Read())
        {
            InvTrace = MyReader["Data1str"].ToString();
        }
        Con.Close();
        return InvTrace;
    }

    private int GetNextAuditInventoryRID()
    {

        DbCon = new DatabaseConnection();
        conString = DbCon.GetPhillyConString();
        DbCon.connection_String = conString;
        DbCon.Sql = "Update SystemNextRID_AUDINV Set NextRID = NextRID + 1 ";
        ds = DbCon.GetConnection;

        int Rid = 0;
        DbCon = new DatabaseConnection();
        SqlConnection Con = new SqlConnection(DbCon.GetPhillyConString());
        SqlDataReader MyReader = null;
        SqlCommand SqlCommand = new SqlCommand("Select NextRID From SystemNextRID_AUDINV", Con);
        Con.Open();
        MyReader = SqlCommand.ExecuteReader();
        while (MyReader.Read())
        {
            Rid = Convert.ToInt32(MyReader["NextRID"]);
        }
        Con.Close();
        return Rid;
    }

    public void InsertPickIntoInventory(string Quantity, string shipmentID, string OrderNumber, string orderLine, string UserID, string InventoryRID)
    {
        int NewInvRID = GetNextInventoryRID();
        string CustomerID = GetCustomerID(shipmentID);
        DbCon = new DatabaseConnection();
        conString = DbCon.GetPhillyConString();
        DbCon.connection_String = conString;
        DbCon.Sql = "Insert Into INVENTORY Select " + NewInvRID + ", ISNULL(CASELABEL, NULL) CASELABEL, ISNULL(PALLETTOTELABEL, NULL) PALLETTOTELABEL, ISNULL(CONTAINERLABEL, NULL) CONTAINERLABEL, ISNULL(TRAILERRAILBOATLABEL, NULL) TRAILERRAILBOATLABEL, ISNULL(PRODUCT_ID, NULL) PRODUCT_ID, ISNULL(TRACE_ID, NULL) TRACE_ID, ISNULL(OWNER_ID, NULL) OWNER_ID, 'CONVEYOR', '" + Quantity + "', ISNULL(QC_CATEGORY, NULL) QC_CATEGORY, ISNULL(EXPIRATION_DATE, NULL) EXPIRATION_DATE, ISNULL(MFG_DATETIME, NULL) MFG_DATETIME, NULL, 1, 0, ISNULL(RESERVATION_TYPE, NULL) RESERVATION_TYPE, '" + shipmentID + "', '" + OrderNumber + "', '" + orderLine + "', ISNULL(DATA1DATE, NULL) DATA1DATE, DATA1NUM, DATA1STR, DATA2DATE, DATA2NUM, LOCATION_ID, DATA3DATE, DATA3NUM, DATA3STR, GetDate(), '" + UserID + "', 'RFPIECEPICK' From Inventory INV (NOLOCK) Where INV._RID_ = '" + InventoryRID + "'";
        ds = DbCon.GetConnection;
    }

    private int GetNextInventoryRID()
    {
        DbCon = new DatabaseConnection();
        conString = DbCon.GetPhillyConString();
        DbCon.connection_String = conString;
        DbCon.Sql = "Update SystemNextRID Set NextRID = NextRID + 1 where GenericName='Inventory'";
        ds = DbCon.GetConnection;

        int Rid = 0;
        DbCon = new DatabaseConnection();
        SqlConnection Con = new SqlConnection(DbCon.GetPhillyConString());
        SqlDataReader MyReader = null;
        SqlCommand SqlCommand = new SqlCommand("Select NextRID From SystemNextRID where GenericName='Inventory'", Con);
        Con.Open();
        MyReader = SqlCommand.ExecuteReader();
        while (MyReader.Read())
        {
            Rid = Convert.ToInt32(MyReader["NextRID"]);
        }
        Con.Close();
        return Rid;
    }

    public string GetProductID(string Input)
    {
        string ID = "";
        DbCon = new DatabaseConnection();
        SqlConnection Con = new SqlConnection(DbCon.GetPhillyConString());
        SqlDataReader MyReader = null;
        SqlCommand SqlCommand = new SqlCommand("select Product_id from Product_master where Product_id='" + Input + "' or Short_description='" + Input + "'", Con);
        Con.Open();
        MyReader = SqlCommand.ExecuteReader();
        while (MyReader.Read())
        {
            ID = MyReader["Product_id"].ToString();
        }
        Con.Close();
        return ID;






    }

    public string GetNextZone(string CurrentZone, string ShipmentID, string OrderNumber)
    {
        string[] zones = new string[26];
        int count = 0;
        int currentPickOrder = -1;
        string NextZone = "";
        Validation Valid = new Validation();
        DbCon = new DatabaseConnection();
        SqlConnection Con = new SqlConnection(DbCon.GetPhillyConString());
        SqlDataReader MyReader = null;
        SqlCommand SqlCommand = new SqlCommand("select zone from Zone_order order by pick_order", Con);
        Con.Open();
        MyReader = SqlCommand.ExecuteReader();
        while (MyReader.Read())
        {
            zones[count] = MyReader["zone"].ToString();
            count++;
        }


        for (int i = 0; i < count; i++)
        {
            if (zones[i] == CurrentZone)
            {
                currentPickOrder = i;
            }
        }

        if (currentPickOrder != -1)
        {
            for (int i = currentPickOrder; i < count; i++)
            {
                if (Valid.IsThereAPickInThisZone(zones[i], ShipmentID, OrderNumber) == true)
                {
                    NextZone = zones[i];
                }
            }
            if (NextZone == "")
            {
                //check to see if a zone was missed missed a zone
                for (int i = 0; i < currentPickOrder; i++)
                {
                    if (Valid.IsThereAPickInThisZone(zones[i], ShipmentID, OrderNumber) == true)
                    {
                        NextZone = zones[i];
                    }
                }
            }
           

        }
        else
        {
            NextZone = "error";
        }

        Con.Close();
        return NextZone;
    }

}