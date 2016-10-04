using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data.Entity.Validation;
using System.Diagnostics;

namespace WarehouseTransfer
{
    public class InventoryAllocation
    {
        DebugLog log = new DebugLog();
        private OutboundShipmentDetails OBSD;
        private string PiecePickLocation;
        private string ProductID1;
        private int? QuantityToAllocate;
        private string p;
        public int RID { get; set; }
        public string ProductID { get; set; }
        public string LocationID { get; set; }
        public string QcCatigory { get; set; }
        public string TraceID { get; set; }
        public int? Quantity { get; set; }
        public string AllocationType { get; set; }
        public string ShipmentID { get; set; }
        public string OrderNumber { get; set; }
        public int OrderLine { get; set; }
        public DateTime LastUpdated { get; set; }
        public string LastUser { get; set; }
        public string LastModule { get; set; }

       

        public InventoryAllocation(ProductMaster ProductMasterInfo, Locations location, OutboundShipmentDetails OBSD, string InAllocationType)
        {
            RID = GetNextRID();
            ProductID = ProductMasterInfo.ProductID;
            LocationID = location.LocationID;
            QcCatigory=OBSD.QCCategory;
            TraceID="";
            Quantity = OBSD.OrderedUnits;
            AllocationType=InAllocationType;
            ShipmentID=OBSD.ShipmentID;
            OrderNumber=OBSD.OrderNumber;
            OrderLine=OBSD.OrderLine;
            LastUpdated=DateTime.Now;
            LastUser=OBSD.LastUser;
            LastModule = OBSD.LastModule;
            //Tim uncomment
            AddToDatabase();

        }

        public InventoryAllocation()
        {

        }

        public InventoryAllocation(OutboundShipmentDetails OBSD, string PiecePickLocation, string ProdID, int? QuantityToAllocate, string InAllocationType)
        {

            RID = GetNextRID();
            ProductID = ProdID;
            LocationID = PiecePickLocation;
            QcCatigory = OBSD.QCCategory;
            TraceID = "";
            Quantity = OBSD.OrderedUnits;
            AllocationType = InAllocationType;
            ShipmentID = OBSD.ShipmentID;
            OrderNumber = OBSD.OrderNumber;
            OrderLine = OBSD.OrderLine;
            LastUpdated = DateTime.Now;
            LastUser = OBSD.LastUser;
            LastModule = OBSD.LastModule;
            AddToDatabase();


           
        }


        private int GetNextRID()
        {
            int RID = 0;

            using (WMSEntities wms = new WMSEntities())
            {
                IQueryable<SYSTEMNEXTRID> Item = from NewRid in wms.SYSTEMNEXTRIDs where NewRid.GENERICNAME == "Inventory_Allocation" select NewRid;
                foreach (SYSTEMNEXTRID New in Item)
                {
                    New.NEXTRID++;
                    RID = New.NEXTRID;
                    wms.SaveChanges();
                }
            }
            
            return RID;
        }

        private void AddToDatabase()
        {
            var WMS = new WMSEntities();

            var InvAll = new INVENTORY_ALLOCATION();

            InvAll.C_RID_ = RID;
            InvAll.PRODUCT_ID = ProductID;
            InvAll.LOCATION_ID = LocationID;
            InvAll.QC_CATEGORY = QcCatigory;
            InvAll.TRACE_ID = TraceID;
            InvAll.QUANTITY = Quantity;
            InvAll.ALLOCATION_TYPE = AllocationType;
            InvAll.SHIPMENT_ID = ShipmentID;
            InvAll.ORDER_NUMBER = OrderNumber;
            InvAll.ORDER_LINE = OrderLine;
            InvAll.C_LAST_UPDATED_ = LastUpdated;
            InvAll.C_LAST_USER_ = LastUser;
            InvAll.C_LAST_MODULE_ = LastModule;

            WMS.INVENTORY_ALLOCATION.Add(InvAll);
            WMS.SaveChanges();
            //log.Write("RID: " + RID);

        }

        internal void DeleteInventoryAllocation(int InRID)
        {
            using (WMSEntities wms = new WMSEntities())
            {
                IQueryable<INVENTORY_ALLOCATION> inv = from IA in wms.INVENTORY_ALLOCATION where IA.C_RID_ == InRID select IA;

                foreach (INVENTORY_ALLOCATION allocate in inv)
                {
                    wms.INVENTORY_ALLOCATION.Remove(allocate);
                }
              //  wms.SaveChanges();

            }
        }
    }
}
