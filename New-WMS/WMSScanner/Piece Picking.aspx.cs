using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Speech.Synthesis;



public partial class Piece_Picking : System.Web.UI.Page
{
    DatabaseConnection objCOnnect;
    Validation Valid;
    string conString;
    string BoxID = "";
    string CurrentZone = "";
    DataSet ds;
    DataRow dRow;

    int MaxRows;
    int inc = 0;


    protected void Page_Load(object sender, EventArgs e)
    {
        Valid = new Validation();

        BoxScannTextBox.Attributes.Add("onfocus", "this.value=''");
        LocationTextBox.Attributes.Add("onfocus", "this.value=''");
        ProductTextBox.Attributes.Add("onfocus", "this.value=''");
        QuantityTextBox.Attributes.Add("onfocus", "this.value=''");
        if (Panel1.Visible == true)
        {
            StartLocationBox.Focus();
        }
        if (BoxScannTextBox.Text != "")
        {
            BoxID = BoxScannTextBox.Text;
            if (CurrentLocationTextBox.Text != "")
                CurrentZone = CurrentLocationTextBox.Text;
            //cant use these because of update pannel, have to call the function in the query
           // string shipmentID = Valid.GetShipmentID(BoxID);
           // string orderNumber = Valid.GetOrderNumber(BoxID);

            if (Panel2.Visible == true && Valid.IsValidOrder(BoxID) == true && CheckBox1.Checked == false)
            {
                PickTicketDataSource.SelectCommand = "SELECT PRODUCT_MASTER.DESCRIPTION, PRODUCT_MASTER.SHORT_DESCRIPTION, PRODUCT_MASTER.PIECE_PICK_LOCATION as From_Location, OUTBOUND_SHIPMENT_DETAILS.ORDER_LINE, OUTBOUND_SHIPMENT_DETAILS.Ordered_units as quantity FROM OUTBOUND_SHIPMENT_DETAILS INNER JOIN PRODUCT_MASTER ON OUTBOUND_SHIPMENT_DETAILS.PRODUCT_ID = PRODUCT_MASTER.PRODUCT_ID where OUTBOUND_SHIPMENT_DETAILS.shipment_id='" + Valid.GetShipmentID(BoxID) + "' and OUTBOUND_SHIPMENT_DETAILS.order_Number='" + Valid.GetOrderNumber(BoxID) + "'and OUTBOUND_SHIPMENT_DETAILS.UOM='EA'";
            }
            if (Panel2.Visible == true && Valid.IsValidOrder(BoxID) == true && CheckBox1.Checked == true)
            {

                PickTicketDataSource.SelectCommand = "select MQ.order_line, MQ.FROM_LOCATION,PM.DESCRIPTION,PM.short_description,  Sum(MQ.QUANTITY_REQUIRED) as quantity From Move_queue MQ (NOLOCK),  inventory INV (NOLOCK), Locations L (NOLOCK), Product_Master PM (NOLOCK) Where (INV._RID_ = MQ.INV_RID) And (PM.PRODUCT_ID = INV.PRODUCT_ID) And (MQ.FROM_LOCATION = L.LOCATION_ID) And (MQ.TYPE = 'PICK') And (MQ.LABELLEVEL = 'EA') And  L.PICKING_ZONE = '" + CurrentZone + "' And (MQ.SHIPMENT_ID = '" + Valid.GetShipmentID(BoxID) + "') And (MQ.ORDER_NUMBER = '" + Valid.GetOrderNumber(BoxID) + "') Group By MQ.SHIPMENT_ID, MQ.ORDER_NUMBER, MQ.ORDER_LINE, MQ.LABELLEVEL, ISNULL(INV.CASELABEL, ''), INV.PRODUCT_ID,PM.description, PM.SHORT_DESCRIPTION, L.PICKING_ZONE, L.LOCATION_GROUP, MQ.FROM_LOCATION, PICK_SEQUENCE, TO_LOCATION Order By L.LOCATION_GROUP, FROM_LOCATION";
            }

        }



    }
    protected void btnRandom_Click(object sender, EventArgs e)
    {
        Valid = new Validation();

        if (StartLocationBox.Text != "")
        {


            if (Valid.IsValidLocation(StartLocationBox.Text) == true)
            {

                StartLocationBox.ReadOnly = true;
                LocationGroup.Attributes["class"] = "form-group  has-success";

                // CurrentLocationBox.BackColor = System.Drawing.Color.LightGray;
                //  btnRandom.Visible = false;

                //  System.Threading.Thread.Sleep(2000);
                Panel1.Visible = false;
                Panel2.Visible = true;
                CurrentLocationTextBox.Text = (Valid.GetZone(StartLocationBox.Text));
                CurrentZone = Valid.GetZone(StartLocationBox.Text);
                BoxScannTextBox.Focus();

            }
            else
            {

                StartLocationBox.Text = "";
                StartLocationBox.Attributes["placeholder"] = "This Location Was Not Valid";
                LocationGroup.Attributes["class"] = "form-group has-error";

            }
        }
        else
        {
            StartLocationBox.Text = "";
            StartLocationBox.Attributes["placeholder"] = "Please Enter a Location";
            LocationGroup.Attributes["class"] = "form-group has-error";

        }

    }


    protected void LinkButton2_Click(object sender, EventArgs e)
    {

        Valid = new Validation();
        if (BoxID != "")
        {

            if (Valid.IsValidOrder(BoxID) == true)
            {

                BoxScanGroup.Attributes["class"] = "form-group has-success";
                CartonUpdatePanel.Visible = true;
                ProductPanel.Visible = true;
                LocationPanel.Visible = true;
                QuantityPanel.Visible = true;
                CartonPanel.Visible = true;
               // string shipmentID = Valid.GetShipmentID(BoxID);
               // string orderNumber = Valid.GetOrderNumber(BoxID);
                LocationTextBox.Focus();

                if (Panel2.Visible == true && Valid.IsValidOrder(BoxID) == true && CheckBox1.Checked == false)
                {
                    PickTicketDataSource.SelectCommand = "SELECT PRODUCT_MASTER.DESCRIPTION, PRODUCT_MASTER.SHORT_DESCRIPTION, PRODUCT_MASTER.PIECE_PICK_LOCATION as from_location, OUTBOUND_SHIPMENT_DETAILS.ORDER_LINE, OUTBOUND_SHIPMENT_DETAILS.Ordered_units as quantity FROM OUTBOUND_SHIPMENT_DETAILS INNER JOIN PRODUCT_MASTER ON OUTBOUND_SHIPMENT_DETAILS.PRODUCT_ID = PRODUCT_MASTER.PRODUCT_ID where OUTBOUND_SHIPMENT_DETAILS.shipment_id='" + Valid.GetShipmentID(BoxID) + "' and OUTBOUND_SHIPMENT_DETAILS.order_Number='" + Valid.GetOrderNumber(BoxID) + "'and OUTBOUND_SHIPMENT_DETAILS.UOM='EA' order by Product_master.Piece_pick_location";
                }
                if (Panel2.Visible == true && Valid.IsValidOrder(BoxID) == true && CheckBox1.Checked == true)
                {
                    PickTicketDataSource.SelectCommand = "select MQ.order_line, MQ.FROM_LOCATION,PM.DESCRIPTION,PM.short_description,  Sum(MQ.QUANTITY_REQUIRED) as quantity From Move_queue MQ (NOLOCK),  inventory INV (NOLOCK), Locations L (NOLOCK), Product_Master PM (NOLOCK) Where (INV._RID_ = MQ.INV_RID) And (PM.PRODUCT_ID = INV.PRODUCT_ID) And (MQ.FROM_LOCATION = L.LOCATION_ID) And (MQ.TYPE = 'PICK') And (MQ.LABELLEVEL = 'EA') And  L.PICKING_ZONE = '" + CurrentZone + "' And (MQ.SHIPMENT_ID = '" + Valid.GetShipmentID(BoxID) + "') And (MQ.ORDER_NUMBER = '" + Valid.GetOrderNumber(BoxID) + "') Group By MQ.SHIPMENT_ID, MQ.ORDER_NUMBER, MQ.ORDER_LINE, MQ.LABELLEVEL, ISNULL(INV.CASELABEL, ''), INV.PRODUCT_ID,PM.description, PM.SHORT_DESCRIPTION, L.PICKING_ZONE, L.LOCATION_GROUP, MQ.FROM_LOCATION, PICK_SEQUENCE, TO_LOCATION Order By L.LOCATION_GROUP, FROM_LOCATION"; 
                }



                CartonUpdatePanel.Update();
                //  PickUpdatePannel.Update();
            }
            else
            {
                BoxScannTextBox.Text = "";
                BoxScannTextBox.Attributes["placeholder"] = "Shipment ID is not valid";
                BoxScannTextBox.Text = "";
                BoxScanGroup.Attributes["class"] = "form-group has-error";
            }
        }
        else
        {
            BoxScannTextBox.Text = "";
            BoxScannTextBox.Attributes["placeholder"] = "Please Scan an order";
            BoxScanGroup.Attributes["class"] = "form-group has-error";
        }



    }
    protected void CartonTimer_Tick(object sender, EventArgs e)
    {

        CartonUpdatePanel.Update();
        GridView1.DataBind();


    }
    protected void PickTimer_Tick(object sender, EventArgs e)
    {

    }

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        Valid = new Validation();
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (Valid.ShipmentStillHasMoveQs(BoxID) == true)
            {


                string orderLine = e.Row.Cells[0].Text;
                string PiecePickLocation = e.Row.Cells[1].Text;
                string PiecePickZone = Valid.GetZone(e.Row.Cells[1].Text);

                if (PiecePickZone == CurrentZone)
                {
                    e.Row.CssClass = "success";
                }
                else if (CheckBox1.Checked == true)
                {
                    e.Row.Visible = false;
                }

                if (Valid.IsThereAPriority30ReplenComingHere(PiecePickLocation) == true)
                {
                    e.Row.CssClass = "warning";
                }
                if (Valid.ExistsInMoveQueue(orderLine, BoxID) != true)
                {
                    e.Row.CssClass = "info";
                }

                if (Valid.IsThereAPriority1ReplenComingHere(PiecePickLocation) == true)
                {
                    e.Row.CssClass = "danger";
                }


            }

        }
    }
    protected void CheckBox1_CheckedChanged(object sender, EventArgs e)
    {
        // PickUpdatePannel.Update();
    }

    protected void ProductCheckButton_Click(object sender, EventArgs e)
    {
        Valid = new Validation();

        string GTIN = ProductTextBox.Text;
        string ProductID = Valid.GtinToPruductID(GTIN);
        string LocationID = LocationTextBox.Text;
        string shipmentID = Valid.GetShipmentID(BoxID);
        string OrderNumber = Valid.GetOrderNumber(BoxID);

        if (Valid.inNumeric(GTIN) == true)
            ProductID = Valid.GtinToPruductID(GTIN);
        else
            ProductID = GTIN;



        if (ProductID != "" && LocationID != "")
        {

            if (Valid.ProductIsInTHisOrder(Valid.GetShipmentID(BoxID), Valid.GetOrderNumber(BoxID), ProductID) == true || Valid.DescriptionInThisOrder(Valid.GetShipmentID(BoxID), Valid.GetOrderNumber(BoxID), ProductID, LocationID) == true)
            {
                if (Valid.ProductInEnteredLocation(Valid.GetShipmentID(BoxID), Valid.GetOrderNumber(BoxID), LocationID, ProductID) == true || Valid.DescriptionInEnteredLocation(LocationID, ProductID) == true)
                {
                    if (Valid.IsThereAPriority1ReplenComingHere(LocationID) != true)
                    {

                        ProductTextBox.Text = ProductID.ToUpper();

                        LocationTextBox.Focus();
                        ProductInputGroup.Attributes["class"] = "form-group has-success";
                        QuantityTextBox.Focus();
                    }
                    else
                    {
                        ProductTextBox.Text = "";
                        ProductTextBox.Attributes["placeholder"] = "Replen Can Not Pick";
                        ProductInputGroup.Attributes["class"] = "form-group has-error";
                    }

                }
                else
                {
                    ProductTextBox.Text = "";
                    ProductTextBox.Attributes["placeholder"] = "Item not in This Location";
                    ProductInputGroup.Attributes["class"] = "form-group has-error";
                }
            }
            else
            {
                ProductTextBox.Text = "";
                ProductTextBox.Attributes["placeholder"] = "Item not in order";
                ProductInputGroup.Attributes["class"] = "form-group has-error";
            }
        }
        else
        {
            ProductTextBox.Text = "";
            ProductTextBox.Attributes["placeholder"] = "Enter Product";
            ProductInputGroup.Attributes["class"] = "form-group has-error";
        }



    }

    protected void LocationButton_Click(object sender, EventArgs e)
    {
        Valid = new Validation();
        string LocationID = LocationTextBox.Text;
        string shipmentID = Valid.GetShipmentID(BoxID);
        string OrderNumber = Valid.GetOrderNumber(BoxID);

        if (LocationID != "")
        {
            if (Valid.IsValidLocation(LocationID) == true)
            {
                if (Valid.LocationInZone(LocationID, CurrentZone) == true)
                {
                    if (Valid.IsLocationInOrder(LocationID, shipmentID, OrderNumber) == true)
                    {
                        if (Valid.IsThereAPriority1ReplenComingHere(LocationID) != true)
                        {



                            LocationTextBox.Text = LocationID.ToUpper();
                            LocationInputGroup.Attributes["class"] = "form-group has-success";
                            ProductTextBox.Focus();
                            PickingUpdatePanel.Update();
                        }
                        else
                        {
                            LocationTextBox.Text = "";
                            LocationTextBox.Attributes["placeholder"] = "No Inventory Can't Pick";
                            LocationInputGroup.Attributes["class"] = "form-group has-error";
                        }

                    }
                    else
                    {
                        LocationTextBox.Text = "";
                        LocationTextBox.Attributes["placeholder"] = "Not In this Order";
                        LocationInputGroup.Attributes["class"] = "form-group has-error";
                    }
                }
                else
                {
                    LocationTextBox.Text = "";
                    LocationTextBox.Attributes["placeholder"] = "Not In Your Zone";
                    LocationInputGroup.Attributes["class"] = "form-group has-error";
                }
            }
            else
            {
                LocationTextBox.Text = "";
                LocationTextBox.Attributes["placeholder"] = "Not A Valid Location";
                LocationInputGroup.Attributes["class"] = "form-group has-error";
            }
        }
        else
        {
            LocationTextBox.Text = "";
            LocationTextBox.Attributes["placeholder"] = "Enter Location";
            LocationInputGroup.Attributes["class"] = "form-group has-error";
        }



    }
    protected void QuantityButton_Click(object sender, EventArgs e)
    {
        Valid = new Validation();
        string UserID = "Tturner";
        Utility UT = new Utility();
        string PickZone = CurrentZone;
        string ProductID = UT.GetProductID(ProductTextBox.Text);
        string LocationID = LocationTextBox.Text;
        string shipmentID = Valid.GetShipmentID(BoxID);
        string OrderNumber = Valid.GetOrderNumber(BoxID);
        string Quantity = QuantityTextBox.Text;
        int NumberOfMoveQueues = 0;
        int n;
        string orderLine;
        string InventoryRID;
        bool isNumeric = int.TryParse(Quantity, out n);


        if (isNumeric == true)
        {
            int EnteredQuantity = Convert.ToInt16(Quantity);
            if (Valid.IsValidQuantity(shipmentID, OrderNumber, ProductID, Quantity) == true)
            {
                QuantityTextBox.Text = "";
                LocationTextBox.Text = "";
                ProductTextBox.Text = "";
                LocationTextBox.Focus();
                QuantityInputGroup.Attributes["class"] = "form-group has-success";
                orderLine = UT.GetOrderLine(shipmentID, OrderNumber, Quantity, ProductID);

                NumberOfMoveQueues = UT.GetNumberOfMoveQueues(shipmentID, OrderNumber, orderLine);

                int NQuantity = Convert.ToInt16(Quantity);
                for (int i = 0; i < NumberOfMoveQueues; i++)
                {
                    string MoveQRID = UT.GetMoveQueueRID(shipmentID, OrderNumber, orderLine);
                    int moveQQuantity = UT.GetMoveQueueQuantity(MoveQRID);
                    InventoryRID = UT.GetInventoryRID(MoveQRID);
                    UT.InsertPickIntoInventory(moveQQuantity.ToString(), shipmentID, OrderNumber, orderLine, UserID, InventoryRID);

                    UT.deleteMoveQueue(MoveQRID);
                    if (InventoryRID != "")
                    {
                        UT.UpdateInventory(UserID, moveQQuantity.ToString(), InventoryRID);
                        UT.MoveToQCEmpty(InventoryRID, shipmentID, OrderNumber, orderLine);
                        UT.InsertIntoAuditInventory(UserID, moveQQuantity.ToString(), shipmentID, OrderNumber, orderLine, ProductID, LocationID, InventoryRID);
                    }
                    else
                    {
                        //error message
                    }
                }

                if (Valid.OrderDone(shipmentID, OrderNumber) == true)
                {
                    UT.UpdateOoutboundShipments(shipmentID, OrderNumber);
                }

                if (Valid.IsQCRequired(UserID) == true)
                {
                    UT.SetQCRequired(shipmentID, OrderNumber);
                }

                UT.DeleteInventoryAllocation(shipmentID, OrderNumber, orderLine);

                QuantityInputGroup.Attributes["class"] = "form-group";
                LocationInputGroup.Attributes["class"] = "form-group";

                ProductInputGroup.Attributes["class"] = "form-group";

                //check to see if we need more inventory from another 
                //updateLocationStatus


            }
            else
            {
                QuantityTextBox.Text = "";
                QuantityTextBox.Attributes["placeholder"] = "Incorrect Quantity";
                QuantityTextBox.Focus();

                QuantityInputGroup.Attributes["class"] = "form-group has-error";
            }


        }
        else
        {
            QuantityTextBox.Text = "";
            BoxScannTextBox.Attributes["placeholder"] = "Please enter a number";
            QuantityInputGroup.Attributes["class"] = "form-group has-error";
        }
    }

  
    protected void ToGoTimer_Tick(object sender, EventArgs e)
    {
        string NextZone = "";
        Utility UT = new Utility();
        ToDoLabel.Visible = false;    

            BoxID = BoxScannTextBox.Text;
            if (CartonPanel.Visible == false || BoxScannTextBox.Text == "" )
            {
                ToDoLabel.Visible = false;
            }

            else if (Panel2.Visible == true && Valid.OrderDone(Valid.GetShipmentID(BoxID), Valid.GetOrderNumber(BoxID)) == true)
            {
                ToDoLabel.Visible = true;
                ToDoLabel.Text = "ORDER DONE SEND TO SHIPPING";
            }
            else if (Panel2.Visible == true && Valid.AllPicksDone(Valid.GetShipmentID(BoxID), Valid.GetOrderNumber(BoxID), CurrentZone) == true)
            {
                ToDoLabel.Visible = true;
                NextZone = UT.GetNextZone(CurrentZone, Valid.GetShipmentID(BoxID), Valid.GetOrderNumber(BoxID));
                ToDoLabel.Text = "PICK DONE! SEND TO <span style='color: red;'>"+NextZone+"</span> ZONE";
            }
            ToGoUpdatePannel.Update();
        
    }
}