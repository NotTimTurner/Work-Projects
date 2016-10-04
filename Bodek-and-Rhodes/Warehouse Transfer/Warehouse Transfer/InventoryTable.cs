using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseTransfer
{
    class InventoryTable
    {
        DebugLog log = new DebugLog();
        public int RID { get; set; }
        public string CaseLabel { get; set; }
        public string ContainerLabel { get; set; }
        public string PalletToTeLabel { get; set; }
        public string TrailerRailBoatLevel { get; set; }
        public string ProductID { get; set; }
        public string TraceID { get; set; }
        public string OwnerID { get; set; }
        public string LocationID { get; set; }
        public float? Quantity { get; set; }
        public string QcCategory { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public DateTime? MfgDateTime { get; set; }
        public DateTime ?ReciptDate { get; set; }
        public bool Reserved { get; set; }
        public bool Hold { get; set; }
        public string ResvervationType { get; set; }
        public string ShipmentID { get; set; }
        public string OrderNumber { get; set; }
        public short? OrderLine { get; set; }
        public DateTime? Data1Date { get; set; }
        public float? Data1Num { get; set; }
        public string Data1Str { get; set; }
        public DateTime? Data2Date { get; set; }
        public float? Data2Num { get; set; }
        public string Data2Str { get; set; }
        public DateTime? Data3Date { get; set; }
        public float? Data3Num { get; set; }
        public string Data3Str { get; set; }
        public DateTime? LastUpdate { get; set; }
        public string LastUser { get; set; }
        public string LastModule { get; set; }


        public InventoryTable()
        {
           

        }

        public InventoryTable(int InRId)
        {
            using (WMSEntities wms = new WMSEntities())
            {
                IQueryable<INVENTORY> Item = from In in wms.INVENTORies where In.C_RID_ == InRId select In;
                foreach (INVENTORY inv in Item)
                {
                    RID = inv.C_RID_;
                    CaseLabel = inv.CASELABEL;
                    ContainerLabel = inv.CONTAINERLABEL;
                    PalletToTeLabel = inv.PALLETTOTELABEL;
                    TrailerRailBoatLevel = inv.TRAILERRAILBOATLABEL;
                    ProductID = inv.PRODUCT_ID;
                    TraceID = inv.TRACE_ID;
                    OwnerID = inv.OWNER_ID;
                    LocationID = inv.LOCATION_ID;
                    Quantity = inv.QUANTITY;
                    QcCategory = inv.QC_CATEGORY;
                    ExpirationDate = inv.EXPIRATION_DATE;
                    MfgDateTime = inv.MFG_DATETIME;
                    ReciptDate = inv.RECEIPT_DATE;
                    Reserved = inv.RESERVED;
                    Hold = inv.HOLD;
                    ResvervationType = inv.RESERVATION_TYPE;
                    ShipmentID = inv.SHIPMENT_ID;
                    OrderNumber = inv.ORDER_NUMBER;
                    OrderLine = inv.ORDER_LINE;
                    Data1Date = inv.DATA1DATE;
                    Data1Num = inv.DATA1NUM;
                    Data1Str = inv.DATA1STR;
                    Data2Date = inv.DATA2DATE;
                    Data2Num = inv.DATA2NUM;
                    Data2Str = inv.DATA2STR;
                    Data3Date = inv.DATA3DATE;
                    Data3Num = inv.DATA3NUM;
                    Data3Str = inv.DATA3STR;
                    LastUpdate = inv.C_LAST_UPDATED_;
                    LastModule = inv.C_LAST_MODULE_;
                    LastUser = inv.C_LAST_USER_;

                }
            }
             
        }

        public void PrintInventoryTable()
        {
            log.newLine();
            log.Write("INFORMANTION FOR BOX :" + RID);
            log.Write("RID: " + RID);
            log.Write("Case Label: " + CaseLabel);
            log.Write("Container Label: " + ContainerLabel);
            log.Write("Pallet Label: " + PalletToTeLabel);
            log.Write("Trailer Label: " + TrailerRailBoatLevel);
            log.Write("Product ID " + ProductID);
            log.Write("Trace ID: " + TraceID);
            log.Write("Owner ID: " + OwnerID);
            log.Write("Location ID: " + LocationID);
            log.Write("Quantity :" + Quantity);
            log.Write("QC Caragoty: " + QcCategory);
            log.Write("Expiration Date: " + ExpirationDate);
            log.Write("MFG DateTime: " + MfgDateTime);
            log.Write("Recipt Date: " + ReciptDate);
            log.Write("Reserved: " + Reserved);
            log.Write("Hold: " + Hold);
            log.Write("Reservation Type: " + ResvervationType);
            log.Write("shipment ID: " + ShipmentID);
            log.Write("Order Number: " + OrderNumber);
            log.Write("Order Line: " + OrderLine);
            log.Write("data1date: " + Data1Date);
            log.Write("data1Num: " + Data1Num);
            log.Write("data1str: " + Data1Str);
            log.Write("data2date: " + Data2Date);
            log.Write("data2Num: " + Data2Num);
            log.Write("data2str: " + Data2Str);
            log.Write("data3date: " + Data3Date);
            log.Write("data3Num: " + Data3Num);
            log.Write("data3str: " + Data3Str);
            log.Write("last Updated: " + LastUpdate);
            log.Write("Last User: " + LastUser);
            log.Write("Last Module: " + LastModule);
            log.Write("****************************");

            
        }

        public void updateReservationType(string Value)
        {
            using (WMSEntities wms = new WMSEntities())
            {
                IQueryable<INVENTORY> Data = from Inv in wms.INVENTORies where Inv.C_RID_ == RID select Inv;

                foreach (INVENTORY InventoryData in Data)
                {
                    InventoryData.RESERVATION_TYPE = Value;
                }
                wms.SaveChanges();
            }

        }

        public void UpdateLastUpdated()
        {
            using (WMSEntities wms = new WMSEntities())
            {
                IQueryable<INVENTORY> Data = from Inv in wms.INVENTORies where Inv.C_RID_ == RID select Inv;

                foreach (INVENTORY InventoryData in Data)
                {
                    InventoryData.C_LAST_UPDATED_ = DateTime.Now;
                }
                wms.SaveChanges();
            }

        }

        public void UpdateLastModule(string Value)
        {
            using (WMSEntities wms = new WMSEntities())
            {
                IQueryable<INVENTORY> Data = from Inv in wms.INVENTORies where Inv.C_RID_ == RID select Inv;

                foreach (INVENTORY InventoryData in Data)
                {
                    InventoryData.C_LAST_MODULE_ = Value;
                }
                wms.SaveChanges();
            }

        }

        public void UpdateLastUser(string Value)
        {
            using (WMSEntities wms = new WMSEntities())
            {
                IQueryable<INVENTORY> Data = from Inv in wms.INVENTORies where Inv.C_RID_ == RID select Inv;

                foreach (INVENTORY InventoryData in Data)
                {
                    InventoryData.C_LAST_USER_ = Value;
                }
                wms.SaveChanges();
            }

        }

    }
}
