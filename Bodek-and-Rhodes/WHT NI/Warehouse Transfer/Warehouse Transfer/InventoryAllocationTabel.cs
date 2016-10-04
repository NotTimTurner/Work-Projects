using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseTransfer
{
    class InventoryAllocationTabel
    {
        DebugLog log = new DebugLog();
        public int RID { get; set; }
        public string  ProductID { get; set; }
        public string LocationID { get; set; }
        public string QcCategory { get; set; }
        public string TraceID { get; set; }
        public float? Quantity { get; set; }
        public string AllocationType { get; set; }
        public string ShipmentID { get; set; }
        public string OrderNumber { get; set; }
        public int? OrderLine { get; set; }
        public DateTime? LastUpdated { get; set; }
        public string LastUser { get; set; }
        public string LastModule { get; set; }

        public InventoryAllocationTabel(int InRID)
        {
            using (WMSEntities wms = new WMSEntities())
            {
                IQueryable<INVENTORY_ALLOCATION> Item = from Inv in wms.INVENTORY_ALLOCATION where Inv.C_RID_ == InRID select Inv;
                foreach (INVENTORY_ALLOCATION invA in Item)
                {
                    RID = invA.C_RID_;
                    ProductID = invA.PRODUCT_ID;
                    LocationID = invA.LOCATION_ID;
                    QcCategory = invA.QC_CATEGORY;
                    TraceID = invA.TRACE_ID;
                    Quantity = invA.QUANTITY;
                    AllocationType = invA.ALLOCATION_TYPE;
                    ShipmentID = invA.SHIPMENT_ID;
                    OrderNumber = invA.ORDER_NUMBER;
                    OrderLine = invA.ORDER_LINE;
                    LastUpdated = invA.C_LAST_UPDATED_;
                    LastUser = invA.C_LAST_USER_;
                    LastModule = invA.C_LAST_MODULE_;

                }
            }
        }

        public void printInventoryAllocationTable()
        {
            log.Write("RID: " + RID);
            log.Write("Product ID " + ProductID);
            log.Write("Location ID: " + LocationID);
            log.Write("QC Caragoty: " + QcCategory);
            log.Write("Trace ID: " + TraceID);
            log.Write("Quantity :" + Quantity);
            log.Write("Allocation Type: " + AllocationType);
            log.Write("shipment ID: " + ShipmentID);
            log.Write("Order Number: " + OrderNumber);
            log.Write("Order Line: " + OrderLine);
            log.Write("last Updated: " + LastUpdated);
            log.Write("Last User: " + LastUser);
            log.Write("Last Module: " + LastModule);
            log.Write("****************************");

        }
    }
}
