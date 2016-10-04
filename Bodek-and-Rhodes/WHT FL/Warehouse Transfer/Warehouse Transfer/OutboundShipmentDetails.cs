using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseTransfer
{
   public class OutboundShipmentDetails
    {
       DebugLog Log = new DebugLog();
        public string ShipmentID { get; set; }
        public string OrderNumber { get; set; }
        public short OrderLine { get; set; }
        public string Type { get; set; }
        public string ProductID { get; set; }
        public string QCCategory { get; set; }
        public string UnitOfMesure { get; set; }
        public float? UOMWeight { get; set; }
        public int? OrderedUnits { get; set; }
        public int StagedUnits { get; set; }
        public int ShippedUnits { get; set; }
        public string Status { get; set; }
        public DateTime ScheduledTime { get; set; }
        public DateTime PickAssignedTime { get; set; }
        public decimal? UnitPriceEach { get; set; }
        public string Notes { get; set; }
        public DateTime LastUpdated { get; set; }
        public string LastUser { get; set; }
        public string LastModule { get; set; }
        public string AllocationStatus { get; set; }

        public OutboundShipmentDetails()
        {
            ProductID = null;
        }

        //constructor for a case 
        public OutboundShipmentDetails(int InOrderLine, OrderLine orderLine, int index, OutboundShipments OBS)
        {
            ShipmentID = OBS.ShipmentID;
            OrderNumber = OBS.OrderNumber;
            OrderLine = (short)InOrderLine;
            Type = OBS.Type;
            UnitOfMesure = "CSE";
            QCCategory = "";
            ShippedUnits = 0;
            LastUpdated = OBS._Last_Updated_;
            LastUser = OBS._Last_User_;
            LastModule = OBS._last_module_;
            ProductID = orderLine.ProductInfo.ProductID;
            UOMWeight = orderLine.ProductInfo.CaseWeight;
            OrderedUnits = GetCaseQuantity(ProductID);
             UnitPriceEach = (decimal?)orderLine.ProductInfo.PricePerEach;
            Status = "B";
            AllocationStatus = "N";
            StagedUnits = 0;
            //AddToDatabase();

        }

        private int? GetCaseQuantity(string ProductID)
        {
            try
            {
                int quantity = 0;
                using (WMSEntities wms = new WMSEntities())
                {
                    var info = (from PM in wms.PRODUCT_MASTER where PM.PRODUCT_ID == ProductID select PM.BASE_UOM_PER_CASELEVEL);

                    foreach (var data in info)
                    {
                        quantity = (int)data.Value;
                    }
                }
                return quantity;
            }
            catch
            {
                Log.Write("No Case Quantity for Product: " + ProductID);
                return 0;
            }
            
        }

      
        internal void AddToDatabase()
        {
            var WMS = new WMSEntities();

            var OBSD = new OUTBOUND_SHIPMENT_DETAILS()
            {
                SHIPMENT_ID = ShipmentID,
                ORDER_NUMBER = OrderNumber,
                ORDER_LINE = OrderLine,
                TYPE = Type,
                UOM = UnitOfMesure,
                QC_CATEGORY = QCCategory,
                SHIPPED_UNITS =ShippedUnits,
                C_LAST_UPDATED_ = LastUpdated,
                NOTES = Notes,
                C_LAST_USER_ = LastUser,
                C_LAST_MODULE_ = LastModule,
                PRODUCT_ID = ProductID,
                UOM_WEIGHT = UOMWeight,
                UNIT_PRICE_EA = UnitPriceEach,
                ORDERED_UNITS = OrderedUnits,
                STATUS = Status,
                ALLOCATION_STATUS = AllocationStatus,
                STAGED_UNITS = StagedUnits

            };
            WMS.OUTBOUND_SHIPMENT_DETAILS.Add(OBSD);
            //Tim Uncomment 
             WMS.SaveChanges();
            PrintOutboundShipmentDetail();
        }

        private void PrintOutboundShipmentDetail()
        {
            Log.newLine();
            Log.Write("Information being inserted into outbound shipment details");
            Log.Write("Shipment ID: " + ShipmentID);
            Log.Write("ORDER_NUMBER: " + OrderNumber);
            Log.Write("ORDER_LINE: " + OrderLine);
            Log.Write("TYPE: " + Type);
            Log.Write("UOM: " + UnitOfMesure);
            Log.Write("QC_CATEGORY: " + QCCategory);
            Log.Write("SHIPPED_UNITS: " + ShippedUnits);
            Log.Write(" _LAST_UPDATED_: " + LastUpdated);
            Log.Write("_LAST_USER_: " + LastUser);
            Log.Write("_LAST_MODULE_: " + LastModule);
            Log.Write(" PRODUCT_ID: " + ProductID);
            Log.Write(" UOM_WEIGHT: " + UOMWeight);
            Log.Write("UNIT_PRICE_EA: " + UnitPriceEach);
            Log.Write("ORDERED_UNITS: " + OrderedUnits);
            Log.Write("STATUS: " + Status);
            Log.Write("ALLOCATION_STATUS: " + AllocationStatus);
            Log.Write("STAGED_UNITS: " + StagedUnits);
        }
       

        public void updateAllocationStatus(string Value)
        {
            if (AllocationStatus != Value)
            {
                AllocationStatus = Value;
                using (WMSEntities wms = new WMSEntities())
                {
                    IQueryable<OUTBOUND_SHIPMENT_DETAILS> Data = from OBSD in wms.OUTBOUND_SHIPMENT_DETAILS
                                                                 where
                                                                     OBSD.SHIPMENT_ID == ShipmentID && OBSD.ORDER_NUMBER == OrderNumber && OBSD.ORDER_LINE == OrderLine
                                                                 select OBSD;

                    foreach (OUTBOUND_SHIPMENT_DETAILS OBSDData in Data)
                    {
                        OBSDData.ALLOCATION_STATUS = Value;
                    }
                    wms.SaveChanges();
                }
            }

        }

        public void UpdateLastUpdated()
        {
            LastUpdated = DateTime.Now;
            using (WMSEntities wms = new WMSEntities())
            {
                IQueryable<OUTBOUND_SHIPMENT_DETAILS> Data = from OBSD in wms.OUTBOUND_SHIPMENT_DETAILS
                                                             where
                                                                 OBSD.SHIPMENT_ID == ShipmentID && OBSD.ORDER_NUMBER == OrderNumber && OBSD.ORDER_LINE == OrderLine
                                                             select OBSD;

                foreach (OUTBOUND_SHIPMENT_DETAILS OBSDData in Data)
                {
                    OBSDData.C_LAST_UPDATED_ = LastUpdated;
                }
                wms.SaveChanges();
            }

        }

        public void UpdateLastModule(string Value)
        {
            if (LastModule != Value)
            {
                LastModule = Value;
                using (WMSEntities wms = new WMSEntities())
                {
                    IQueryable<OUTBOUND_SHIPMENT_DETAILS> Data = from OBSD in wms.OUTBOUND_SHIPMENT_DETAILS
                                                                 where
                                                                     OBSD.SHIPMENT_ID == ShipmentID && OBSD.ORDER_NUMBER == OrderNumber && OBSD.ORDER_LINE == OrderLine
                                                                 select OBSD;

                    foreach (OUTBOUND_SHIPMENT_DETAILS OBSDData in Data)
                    {
                        OBSDData.C_LAST_MODULE_ = LastModule;
                    }
                    wms.SaveChanges();
                }
            }

        }

        public void UpdateLastUser(string Value)
        {
            if (LastUser != Value)
            {
                LastUser = Value;

                using (WMSEntities wms = new WMSEntities())
                {
                    IQueryable<OUTBOUND_SHIPMENT_DETAILS> Data = from OBSD in wms.OUTBOUND_SHIPMENT_DETAILS
                                                                 where
                                                                     OBSD.SHIPMENT_ID == ShipmentID && OBSD.ORDER_NUMBER == OrderNumber && OBSD.ORDER_LINE == OrderLine
                                                                 select OBSD;

                    foreach (OUTBOUND_SHIPMENT_DETAILS OBSDData in Data)
                    {
                        OBSDData.C_LAST_USER_ = LastUser;
                    }
                    wms.SaveChanges();
                }
            }
        }

        public void UpdateOrderQuantity(int Value)
        {
            if (OrderedUnits != Value)
            {
                OrderedUnits = Value;
                using (WMSEntities wms = new WMSEntities())
                {
                    IQueryable<OUTBOUND_SHIPMENT_DETAILS> Data = from OBSD in wms.OUTBOUND_SHIPMENT_DETAILS
                                                                 where
                                                                     OBSD.SHIPMENT_ID == ShipmentID && OBSD.ORDER_NUMBER == OrderNumber && OBSD.ORDER_LINE == OrderLine
                                                                 select OBSD;

                    foreach (OUTBOUND_SHIPMENT_DETAILS OBSDData in Data)
                    {
                        OBSDData.ORDERED_UNITS = OrderedUnits;
                    }
                    wms.SaveChanges();
                }
            }
                

            
        }

        internal void DeleteOBSDEntry()
        {
            using (WMSEntities wms = new WMSEntities())
            {
                IQueryable<OUTBOUND_SHIPMENT_DETAILS> obsd = from OD in wms.OUTBOUND_SHIPMENT_DETAILS
                                                             where
                                                                 OD.SHIPMENT_ID == ShipmentID && OD.ORDER_NUMBER == OrderNumber && OD.ORDER_LINE == OrderLine
                                                             select OD;

                foreach (OUTBOUND_SHIPMENT_DETAILS allocate in obsd)
                {
                    wms.OUTBOUND_SHIPMENT_DETAILS.Remove(allocate);
                }
                wms.SaveChanges();

            }
        }

        internal double GetTotalWeightofLine()
        {
            if (UOMWeight == null)
            {
                UOMWeight = 0;
            }

            return (double)UOMWeight *(double)OrderedUnits;
        }

        internal void UpdateStatus(string Value)
        {
            if (Status != Value)
            {
                Status = Value;
                using (WMSEntities wms = new WMSEntities())
                {
                    IQueryable<OUTBOUND_SHIPMENT_DETAILS> Data = from OBSD in wms.OUTBOUND_SHIPMENT_DETAILS
                                                                 where
                                                                     OBSD.SHIPMENT_ID == ShipmentID && OBSD.ORDER_NUMBER == OrderNumber && OBSD.ORDER_LINE == OrderLine
                                                                 select OBSD;

                    foreach (OUTBOUND_SHIPMENT_DETAILS OBSDData in Data)
                    {
                        OBSDData.STATUS = Value;
                    }
                    wms.SaveChanges();
                }
            }
        }

        internal void updateUnitOfMesure(string Value)
        {
            if (UnitOfMesure != Value)
            {
                UnitOfMesure = Value;
                using (WMSEntities wms = new WMSEntities())
                {
                    IQueryable<OUTBOUND_SHIPMENT_DETAILS> Data = from OBSD in wms.OUTBOUND_SHIPMENT_DETAILS
                                                                 where
                                                                     OBSD.SHIPMENT_ID == ShipmentID && OBSD.ORDER_NUMBER == OrderNumber && OBSD.ORDER_LINE == OrderLine
                                                                 select OBSD;

                    foreach (OUTBOUND_SHIPMENT_DETAILS OBSDData in Data)
                    {
                        OBSDData.UOM = UnitOfMesure;
                    }
                    wms.SaveChanges();
                }
            }
        }

        public string getOrderNumber(int count)
        {
            string number = "";
            int TotalLength = 4;
            double length = (Math.Floor(Math.Log10(count) + 1));

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
    }
}
