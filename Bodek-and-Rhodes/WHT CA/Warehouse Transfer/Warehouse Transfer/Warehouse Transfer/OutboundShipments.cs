using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Diagnostics;

namespace WarehouseTransfer
{
    public class OutboundShipments
    {
        DebugLog Log = new DebugLog();
        public string ShipmentID { get; set; }
        public string OrderNumber { get; set; }
        public string Type { get; set; }
        public string Customer_ID { get; set; }
        public string Name { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address_3 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Phone { get; set; }
        public string PrepaidCollect { get; set; }
        public string Purchase_order { get; set; }
        public DateTime PurchasOrderDate { get; set; }
        public DateTime ScheduledShipDate { get; set; }
        public string OrigionalCarrier { get; set; }
        public string Carrier_id { get; set; }
        public string Original_ship_mode { get; set; }
        public string Ship_mode { get; set; }
        public bool Exceed_ship_by { get; set; }
        public bool Substitution_override { get; set; }
        public bool Special_Handeling { get; set; }
        public bool AllowCOPL { get; set; }
        public string Payment_terms { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }
        public string AllocationStatus { get; set; }
        public float? Deposit_amount { get; set; }
        public string Freight_bill_to_account { get; set; }
        public string Freight_bill_To_Name { get; set; }
        public string Freight_bill_to_address_1 { get; set; }
        public string Freight_bill_to_address_2 { get; set; }
        public string Freight_bill_to_address_3 { get; set; }
        public string Freight_bill_to_city { get; set; }
        public string Freight_bill_to_state { get; set; }
        public string freight_bill_to_Zip { get; set; }
        public string QC_Codes { get; set; }
        public DateTime? Deliver_By_Date { get; set; }
        public string CartonType { get; set; }
        public int Wave_id { get; set; }
        public float? Total_weight { get; set; }
        public int Total_CSN { get; set; }
        public short? Stop_of { get; set; }
        public Int16 Stop { get; set; }
        public Int16 Sequence { get; set; }
        public decimal Freight_charge { get; set; }
        public decimal COD_COST { get; set; }
        public decimal Carton_Freight { get; set; }
        public DateTime _Last_Updated_ { get; set; }
        public string _Last_User_ { get; set; }
        public string _last_module_ { get; set; }
        public string COD_Indicator { get; set; }
        public string QCRequired { get; set; }






        public OutboundShipments()
        {
            OrderNumber = "0001";
            Exceed_ship_by = false;
            Substitution_override = false;
            Special_Handeling = false;
            AllowCOPL = false;
            Status = "B";
            AllocationStatus = "N";
            Wave_id = 0;
            Total_weight = 0;
            Total_CSN = 0;
            Stop_of = 0;
            Stop = 0;
            Sequence = 0;
            Freight_charge = 0;
            COD_COST = 0;
            Carton_Freight = 0;
            _Last_Updated_ = DateTime.Now;
            Purchase_order = "0";


        }
        
        //constructor for a case
        public OutboundShipments(int BaseOrderNumber, string OrderID, string InCartonType)
        {
            using (WMSEntities wms = new WMSEntities())
            {
                IQueryable<HOST_ORDERS> Item = from HO in wms.HOST_ORDERS where HO.EIS_ORDER_ID == OrderID select HO;
                foreach (HOST_ORDERS h in Item)
                {
                    ShipmentID = h.EIS_ORDER_ID;
                    Type = h.TYPE;
                    Customer_ID = h.CUSTOMER_ID;
                    Name = h.NAME;
                    Address1 = h.ADDRESS_1;
                    Address2 = h.ADDRESS_2;
                    Address_3 = h.ADDRESS_3;
                    City = h.CITY;
                    State = h.STATE;
                    Zip = h.ZIP;
                    Phone = h.PHONE;
                    PrepaidCollect = h.FREIGHT_BILLING_OPTION;
                    Purchase_order = h.PURCHASE_ORDER;
                    PurchasOrderDate = h.ORDER_DATE;
                    ScheduledShipDate = DateTime.Now;
                    OrigionalCarrier = h.CARRIER_ID;
                    Carrier_id = h.CARRIER_ID;
                    Original_ship_mode = h.ORIGINAL_SHIP_MODE;
                    Ship_mode = h.ORIGINAL_SHIP_MODE;
                    Payment_terms = h.TERMS.ToString();
                    Notes = h.NOTES;
                    Deposit_amount = h.DEPOSIT_AMOUNT;
                    Freight_bill_to_account = h.FREIGHT_BILL_TO_ACCOUNT;
                    Freight_bill_To_Name = h.FREIGHT_BILL_TO_NAME;
                    Freight_bill_to_address_1 = h.FREIGHT_BILL_TO_ADDRESS_1;
                    Freight_bill_to_address_2 = h.FREIGHT_BILL_TO_ADDRESS_2;
                    Freight_bill_to_address_3 = h.FREIGHT_BILL_TO_ADDRESS_3;
                    Freight_bill_to_city = h.FREIGHT_BILL_TO_CITY;
                    Freight_bill_to_state = h.FREIGHT_BILL_TO_STATE;
                    freight_bill_to_Zip = h.FREIGHT_BILL_TO_ZIP;
                    QC_Codes ="";
                    Deliver_By_Date = h.DELIVER_BY_DATE;
                    _Last_User_ = Environment.MachineName;
                    _last_module_ = "CARTONIZE";
                    _Last_Updated_ = DateTime.Now;
                    Exceed_ship_by = false;
                    Substitution_override = false;
                    Special_Handeling = false;
                    AllowCOPL = false;
                    Status = "B";
                    AllocationStatus = "N";
                    Wave_id = 0;
                    Total_weight = 0;
                    Total_CSN = 0;
                    Stop_of = 0;
                    Stop = 0;
                    Sequence = 0;
                    Freight_charge = 0;
                    COD_COST = 0;
                    Carton_Freight = 0;
                    QCRequired = h.QC_REQUIRED;
                   
                   


                    if (h.COD_INDICATOR == "")
                        COD_Indicator = "0";
                    else
                        COD_Indicator = h.COD_INDICATOR;

                    if (h.CARRIER_ID == "Active FM")
                        Address_3 = h.NAME;
                    BaseOrderNumber++;
                    OrderNumber = getOrderNumber(BaseOrderNumber);
                    CartonType = InCartonType;
                  // AddToDatabase();
                  // PrintOutboundShipmentData();
                
                }

            }
        }
        //adds a new line into the outbound Shipments table
        public void AddToDatabase()
        {

           
            try
            {
                var WMS = new WMSEntities();

                var OBS = new OUTBOUND_SHIPMENTS();

                OBS.SHIPMENT_ID = ShipmentID;
                OBS.ORDER_NUMBER = OrderNumber;
                OBS.TYPE = Type;
                OBS.CUSTOMER_ID = Customer_ID;
                OBS.NAME = Name;
                OBS.ADDRESS_1 = Address1;
                OBS.ADDRESS_2 = Address2;
                OBS.ADDRESS_3 = Address_3;
                OBS.CITY = City;
                OBS.STATE = State;
                OBS.ZIP = Zip;
                OBS.PHONE = Phone;
                OBS.PREPAID_COLLECT = PrepaidCollect;
                OBS.SCHEDULED_SHIP_DATE = ScheduledShipDate;
                OBS.ORIGINAL_CARRIER = OrigionalCarrier;
                OBS.CARRIER_ID = Carrier_id;
                OBS.ORIGINAL_SHIP_MODE = Original_ship_mode;
                OBS.SHIP_MODE = Ship_mode;
                OBS.EXCEED_SHIP_BY = Exceed_ship_by;
                OBS.SUBSTITUTION_OVERRIDE = Substitution_override;
                OBS.SPECIAL_HANDLING = Special_Handeling;
                OBS.ALLOWCOPL = AllowCOPL;
                OBS.PAYMENT_TERMS = Payment_terms;
                OBS.STATUS = Status;
                OBS.NOTES = Notes;
                OBS.ALLOCATION_STATUS = AllocationStatus;
                OBS.DEPOSIT_AMOUNT =Convert.ToDecimal(Deposit_amount);
                OBS.FREIGHT_BILL_TO_ACCOUNT = Freight_bill_to_account;
                OBS.FREIGHT_BILL_TO_ADDRESS_1 = Freight_bill_to_address_1;
                OBS.FREIGHT_BILL_TO_ADDRESS_2 = Freight_bill_to_address_2;
                OBS.FREIGHT_BILL_TO_ADDRESS_3 = Freight_bill_to_address_3;
                OBS.FREIGHT_BILL_TO_CITY = Freight_bill_to_city;
                OBS.FREIGHT_BILL_TO_ZIP = freight_bill_to_Zip;
                OBS.FREIGHT_BILL_TO_NAME = Freight_bill_To_Name;
                OBS.FREIGHT_BILL_TO_STATE = Freight_bill_to_state;
                OBS.QC_CODES = QC_Codes;
                OBS.DELIVER_BY_DATE = Deliver_By_Date;
                OBS.WAVE_ID = Wave_id;
                OBS.TOTAL_WEIGHT = Convert.ToDecimal(Total_weight);
                OBS.STOP_OF = Stop_of;
                OBS.STOP = Stop;
                OBS.SEQUENCE = Sequence;
                OBS.FREIGHT_CHARGE = Freight_charge;
                OBS.COD_COST = COD_COST;
                OBS.CARTON_FREIGHT = Carton_Freight;
                OBS.C_LAST_UPDATED_ = _Last_Updated_;
                OBS.C_LAST_USER_ = _Last_User_;
                OBS.C_LAST_MODULE_ = _last_module_;
                OBS.COD_INDICATOR = COD_Indicator;
                OBS.PURCHASE_ORDER = Purchase_order;
                OBS.PURCHASE_ORDER_DATE = PurchasOrderDate;
                OBS.TOTAL_CSN = Total_CSN;
                OBS.CARTON_TYPE = CartonType;
                WMS.OUTBOUND_SHIPMENTS.Add(OBS);
                //Tim Uncomment 
               WMS.SaveChanges();

            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
            }
        }

        //prints the contents on this objct to the debug log 
        public void PrintOutboundShipmentData()
        {
            Log.newLine();
            Log.Write("OUTBOUND SHIPMENT DATA TO BE INSERTED INTO THE DATABASE:");
            Log.Write("Shipment ID: " + ShipmentID);
            Log.Write("OrderNumber: " + OrderNumber);
            Log.Write("Type: " + Type);
            Log.Write("Stop: " + Stop);
            Log.Write("Stop Of: " + Stop_of);
            Log.Write("Sequence: " + Sequence);
            Log.Write("Customer_ID: " + Customer_ID);
            Log.Write("Name: " + Name);
            Log.Write("Address 1: " + Address1);
            Log.Write("Address 2: " + Address2);
            Log.Write("City: " + City);
            Log.Write("State: " + State);
            Log.Write("Zip: " + Zip);
            Log.Write("Phone " + Phone);
            Log.Write("Purchas Order Date: " + PurchasOrderDate);
            Log.Write("Scheduled Ship Date: " + ScheduledShipDate);
            Log.Write("Original Carrier: " + OrigionalCarrier);
            Log.Write("Origional Ship Mode: " + Original_ship_mode);
            Log.Write("Carrier ID: " + Carrier_id);
            Log.Write("Ship Mode: " + Ship_mode);
            Log.Write("Total CNS: " + Total_CSN);
            Log.Write("Total Weight" + Total_weight);
            Log.Write("Status: " + Status);
            Log.Write("PrePaid Collect: " + PrepaidCollect);
            Log.Write("Exceed Ship By: " + Exceed_ship_by);
            Log.Write("Substitution Override: " + Substitution_override);
            Log.Write("Special Handeling: " + Special_Handeling);
            Log.Write("Notes: " + Notes);
            Log.Write("Allow CopL: " + AllowCOPL);
            Log.Write("Payment_Terms: " + Payment_terms);
            Log.Write("Freight Charge: " + Freight_charge);
            Log.Write("Allocation status: " + AllocationStatus);
            Log.Write("COD Indicator: " + COD_Indicator);
            Log.Write("Carton Freight: " + Carton_Freight);
            Log.Write("COD Cost: " + COD_COST);
            Log.Write("Wave ID: " + Wave_id);
            Log.Write("Deposit Ammount: " + Deposit_amount);
            Log.Write("Freight Bill To Account: " + Freight_bill_to_account);
            Log.Write("Freight Bill To Name: " + Freight_bill_To_Name);
            Log.Write("Freight Bill to Address 1: " + Freight_bill_to_address_1);
            Log.Write("Freight BIll to Address 2: " + Freight_bill_to_address_2);
            Log.Write("Freight Bill to Address 3: " + Freight_bill_to_address_3);
            Log.Write("Freight Bill to City: " + Freight_bill_to_city);
            Log.Write("Freight Bill to Zip: " + freight_bill_to_Zip);
            Log.Write("Last Updated: " + _Last_Updated_);
            Log.Write("last User: " + _Last_User_);
            Log.Write("Last Module: " + _last_module_);
            Log.Write("QC Codes: " + QC_Codes);
            Log.Write("Deliver by Date: " + Deliver_By_Date);
            Log.Write("Address 3: " + Address_3);
            Log.Write("Carton Type: " + CartonType);
           
           

            
            

        }
        //used to get us the order number
        //if passed in a 1 it will return 0001 if passed in a 567 it will return 0567
        public string getOrderNumber(int count)
        {
            string number="";
            int TotalLength = 4;
            double length =(Math.Floor(Math.Log10(count) + 1));

           // Log.Write("Length of string: " + length);
           // Log.Write("0s to insert: " + (TotalLength - length));
            for (int i = 0; i < (TotalLength - length); i++)
            {
                number += "0";
            }
            number += count;
            Log.Write("created order number: " + number);
                return number;
        }

        public void updateAllocationStatus(string Value)
        {
            if (AllocationStatus != Value)
            {
                AllocationStatus = Value;
                using (WMSEntities wms = new WMSEntities())
                {
                    IQueryable<OUTBOUND_SHIPMENTS> Data = from OBSD in wms.OUTBOUND_SHIPMENTS
                                                                 where
                                                                     OBSD.SHIPMENT_ID == ShipmentID && OBSD.ORDER_NUMBER == OrderNumber
                                                                 select OBSD;

                    foreach (OUTBOUND_SHIPMENTS OBSData in Data)
                    {
                        OBSData.ALLOCATION_STATUS = Value;
                    }
                    wms.SaveChanges();
                }
            }

        }


        internal void DeleteOBSEntry()
        {
            using (WMSEntities wms = new WMSEntities())
            {
                IQueryable<OUTBOUND_SHIPMENTS> obsd = from OD in wms.OUTBOUND_SHIPMENTS
                                                             where
                                                                 OD.SHIPMENT_ID == ShipmentID && OD.ORDER_NUMBER == OrderNumber 
                                                             select OD;

                foreach (OUTBOUND_SHIPMENTS allocate in obsd)
                {
                    wms.OUTBOUND_SHIPMENTS.Remove(allocate);
                }
                wms.SaveChanges();
            }

        }

        internal void UpdateCatrtonType(string Value)
        {
            if (CartonType != Value)
            {
                CartonType = Value;
                using (WMSEntities wms = new WMSEntities())
                {
                    IQueryable<OUTBOUND_SHIPMENTS> Data = from OBSD in wms.OUTBOUND_SHIPMENTS
                                                          where
                                                              OBSD.SHIPMENT_ID == ShipmentID && OBSD.ORDER_NUMBER == OrderNumber
                                                          select OBSD;

                    foreach (OUTBOUND_SHIPMENTS OBSData in Data)
                    {
                        OBSData.CARTON_TYPE = Value;
                    }
                    wms.SaveChanges();
                }
            }
        }

        internal void UpdateLastUpdated()
        {
            _Last_Updated_ = DateTime.Now;
            using (WMSEntities wms = new WMSEntities())
            {
                IQueryable<OUTBOUND_SHIPMENTS> Data = from OBS in wms.OUTBOUND_SHIPMENTS
                                                             where
                                                                 OBS.SHIPMENT_ID == ShipmentID && OBS.ORDER_NUMBER == OrderNumber
                                                             select OBS;

                foreach (OUTBOUND_SHIPMENTS OBSData in Data)
                {
                    OBSData.C_LAST_UPDATED_ = _Last_Updated_;
                }
                wms.SaveChanges();
            }
        }

        internal void UpdateStatus(string Value)
        {
            if (Status != Value)
            {
                Status = Value;
                using (WMSEntities wms = new WMSEntities())
                {
                    IQueryable<OUTBOUND_SHIPMENTS> Data = from OBS in wms.OUTBOUND_SHIPMENTS
                                                                 where
                                                                     OBS.SHIPMENT_ID == ShipmentID && OBS.ORDER_NUMBER == OrderNumber
                                                                 select OBS;

                    foreach (OUTBOUND_SHIPMENTS OBSData in Data)
                    {
                        OBSData.STATUS = Value;
                    }
                    wms.SaveChanges();
                }
            }
        }

        

        internal void UpdateQCRequired(string Value)
        {
            if (QCRequired != Value)
            {
                QCRequired = Value;
                using (WMSEntities wms = new WMSEntities())
                {
                    IQueryable<OUTBOUND_SHIPMENTS> Data = from OBSD in wms.OUTBOUND_SHIPMENTS
                                                          where
                                                              OBSD.SHIPMENT_ID == ShipmentID && OBSD.ORDER_NUMBER == OrderNumber
                                                          select OBSD;

                    foreach (OUTBOUND_SHIPMENTS OBSData in Data)
                    {
                        OBSData.QC_REQUIRED = Value;
                    }
                    wms.SaveChanges();
                }
            }
        }

        internal void UpdateQCCodes(string Value)
        {
            if (QC_Codes != Value)
            {
                QC_Codes = Value;
                using (WMSEntities wms = new WMSEntities())
                {
                    IQueryable<OUTBOUND_SHIPMENTS> Data = from OBSD in wms.OUTBOUND_SHIPMENTS
                                                          where
                                                              OBSD.SHIPMENT_ID == ShipmentID && OBSD.ORDER_NUMBER == OrderNumber
                                                          select OBSD;

                    foreach (OUTBOUND_SHIPMENTS OBSData in Data)
                    {
                        OBSData.QC_CODES = Value;
                    }
                    wms.SaveChanges();
                }
            }
        }

        internal void UpdatePurchaseOrder(string Value)
        {
            if (Purchase_order != Value)
            {
                Purchase_order = Value;
                using (WMSEntities wms = new WMSEntities())
                {
                    IQueryable<OUTBOUND_SHIPMENTS> Data = from OBSD in wms.OUTBOUND_SHIPMENTS
                                                          where
                                                              OBSD.SHIPMENT_ID == ShipmentID && OBSD.ORDER_NUMBER == OrderNumber
                                                          select OBSD;

                    foreach (OUTBOUND_SHIPMENTS OBSData in Data)
                    {
                        OBSData.PURCHASE_ORDER = Value;
                    }
                    wms.SaveChanges();
                }
            }
        }
    }
}
