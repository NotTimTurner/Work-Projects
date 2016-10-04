using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Host_Order_Copy
{
    class HostOrderDetail
    {
        public string EISOrderID { get; set; }
        public short EISOrderLine { get; set; }
        public string Type { get; set; }
        public string ProductID { get; set; }
        public string QCCategory { get; set; }
        public float? OrderedUnits { get; set; }
        public string UnitOfMesure { get; set; }
        public DateTime? ScheduledDate { get; set; }
        public string Notes { get; set; }
        public decimal? EISOrderLinePrice { get; set; }
        public decimal? EISUnitPrice { get; set; }
        public DateTime? LastUpdated { get; set; }
        public string LastUser { get; set; }
        public string LastModule { get; set; }



        public HostOrderDetail(string InOrderID, int InOrderLine)
        {
            using(WMSEntities wms=new WMSEntities())
            {
                var data = from HOD in wms.HOST_ORDER_DETAIL where HOD.EIS_ORDER_ID == InOrderID && HOD.EIS_ORDER_LINE == InOrderLine select HOD;

                foreach (var Detail in data)
                {
                    EISOrderID = Detail.EIS_ORDER_ID;
                    EISOrderLine = Detail.EIS_ORDER_LINE;
                    Type = Detail.TYPE;
                    ProductID = Detail.PRODUCT_ID;
                    QCCategory = Detail.QC_CATEGORY;
                    OrderedUnits = Detail.ORDERED_UNITS;
                    UnitOfMesure = Detail.UOM;
                    ScheduledDate = Detail.SCHEDULED_DATE;
                    Notes = Detail.NOTES;
                    EISOrderLinePrice = Detail.EIS_ORDLINE_PRICE;
                    EISUnitPrice = Detail.EIS_UNIT_PRICE;
                    LastUpdated = Detail.C_LAST_UPDATED_;
                    LastUser = Detail.C_LAST_USER_;
                    LastModule = LastModule;

                }
            }
        }

        public HostOrderDetail()
        {
            // TODO: Complete member initialization
        }


        public void AddToDatabase()
        {
            var wms = new WMSEntities();
            var HOD = new HOST_ORDER_DETAIL
            {
                EIS_ORDER_ID=EISOrderID,
                EIS_ORDER_LINE=EISOrderLine,
                TYPE=Type,
                PRODUCT_ID=ProductID,
                QC_CATEGORY=QCCategory,
                ORDERED_UNITS=OrderedUnits,
                UOM=UnitOfMesure,
                SCHEDULED_DATE=ScheduledDate,
                NOTES=Notes,
                EIS_ORDLINE_PRICE=EISOrderLinePrice,
                EIS_UNIT_PRICE=EISUnitPrice,
                C_LAST_UPDATED_=LastUpdated,
                C_LAST_USER_=LastUser,
                C_LAST_MODULE_=LastModule


            };
            wms.HOST_ORDER_DETAIL.Add(HOD);
            wms.SaveChanges();
        }



    }
}
