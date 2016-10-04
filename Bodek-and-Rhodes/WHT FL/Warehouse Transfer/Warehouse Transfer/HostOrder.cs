using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseTransfer
{
    public class HostOrder
    {
        public string EisOrderId { get; set; }
        public string status { get; set; }
        public string Type { get; set; }
        public DateTime OrderDate { get; set; }
        public string CustomerID { get; set; }
        public string Name { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Phone { get; set; }
        public string PurchasOrder { get; set; }
        public string OrigionalShipMode { get; set; }
        public float? Terms { get; set; }
        public string CODIndicator { get; set; }
        public int? EstNumBoxes { get; set; }
        public string CarrierID { get; set; }
        public decimal? Freight { get; set; }
        public decimal? DepositAMount { get; set; }
        public string FreightBIllingOptions { get; set; }
        public string ExpediteIndicator { get; set; }
        public string Notes { get; set; }
        public DateTime? ImportTime { get; set; }
        public string FreightBillToAccount { get; set; }
        public string FreightBillToName { get; set; }
        public string FreightBillToAddress1 { get; set; }
        public string FreightBillToAddress2 { get; set; }
        public string FreightBillToAddress3 { get; set; }
        public string FreightBillToCity { get; set; }
        public string FreightBillToState { get; set; }
        public string FreightBillToZip { get; set; }
        public DateTime? LastUpdated { get; set; }
        public string LastUser { get; set; }
        public string LastModule { get; set; }
        public string QcCodes { get; set; }
        public DateTime? DeliverByDate { get; set; }
        public string QcRequired { get; set; }
        public string ExternalCartonized { get; set; }
        public bool? FirstBoxShipped { get; set; }



        public HostOrder(string InEISOrderID)
        {
            using (WMSEntities wms = new WMSEntities())
            {
                var data = from HO in wms.HOST_ORDERS where HO.EIS_ORDER_ID == InEISOrderID select HO;

                foreach (var Order in data)
                {
                    EisOrderId = Order.EIS_ORDER_ID;
                    status = Order.STATUS;
                    Type = Order.TYPE;
                    OrderDate = Order.ORDER_DATE;
                    CustomerID = Order.CUSTOMER_ID;
                    Name = Order.NAME;
                    Address1 = Order.ADDRESS_1;
                    Address2 = Order.ADDRESS_2;
                    Address3 = Order.ADDRESS_3;
                    City = Order.CITY;
                    State = Order.STATE;
                    Zip = Order.ZIP;
                    Phone = Order.PHONE;
                    PurchasOrder = Order.PURCHASE_ORDER;
                    OrigionalShipMode = Order.ORIGINAL_SHIP_MODE;
                    Terms = Order.TERMS;
                    CODIndicator = Order.COD_INDICATOR;
                    EstNumBoxes = Order.EST_NUM_BOXES;
                    CarrierID = Order.CARRIER_ID;
                    Freight = Order.FREIGHT;
                    DepositAMount = Order.DEPOSIT_AMOUNT;
                    FreightBIllingOptions = Order.FREIGHT_BILLING_OPTION;
                    ExpediteIndicator = Order.EXPEDITE_IND;
                    Notes = Order.NOTES;
                    ImportTime = Order.IMPORT_TIME;
                    FreightBillToAccount = Order.FREIGHT_BILL_TO_ACCOUNT;
                    FreightBillToAddress1 = Order.FREIGHT_BILL_TO_ADDRESS_1;
                    FreightBillToAddress2 = Order.FREIGHT_BILL_TO_ADDRESS_2;
                    FreightBillToAddress3 = Order.FREIGHT_BILL_TO_ADDRESS_3;
                    FreightBillToCity = Order.FREIGHT_BILL_TO_CITY;
                    FreightBillToName = Order.FREIGHT_BILL_TO_NAME;
                    FreightBillToState = Order.FREIGHT_BILL_TO_STATE;
                    FreightBillToZip = Order.FREIGHT_BILL_TO_ZIP;
                    LastUpdated = Order.C_LAST_UPDATED_;
                    LastUser = Order.C_LAST_USER_;
                    LastModule = Order.C_LAST_MODULE_;
                    QcCodes = Order.QC_CODES;
                    DeliverByDate = Order.DELIVER_BY_DATE;
                    QcRequired = Order.QC_REQUIRED;
             
                    FirstBoxShipped = Order.FIRSTBOXSHIPPED;
                    
                }
            }

        }

        public HostOrder()
        {
            // TODO: Complete member initialization
        }




        internal void UpdateStatus(string Value)
        {
            if (status != Value)
            {
                status = Value;
                using (WMSEntities wms = new WMSEntities())
                {
                    IQueryable<HOST_ORDERS> Data = from HO in wms.HOST_ORDERS
                                                                 where
                                                                     HO.EIS_ORDER_ID==EisOrderId
                                                                 select HO;

                    foreach (HOST_ORDERS HOData in Data)
                    {
                        HOData.STATUS = Value;
                    }
                    wms.SaveChanges();
                }
            }
        }

        internal void UpdateLastUpdates()
        {
            
            {
                LastUpdated = DateTime.Now;
                using (WMSEntities wms = new WMSEntities())
                {
                    IQueryable<HOST_ORDERS> Data = from HO in wms.HOST_ORDERS
                                                   where
                                                       HO.EIS_ORDER_ID == EisOrderId
                                                   select HO;

                    foreach (HOST_ORDERS HOData in Data)
                    {
                        HOData.C_LAST_UPDATED_ = LastUpdated;
                    }
                    wms.SaveChanges();
                }
            }
        }

        internal void UpdateLastModule(string Value)
        {
            if (LastModule != Value)
            {
                LastModule = Value;
                using (WMSEntities wms = new WMSEntities())
                {
                    IQueryable<HOST_ORDERS> Data = from HO in wms.HOST_ORDERS
                                                   where
                                                       HO.EIS_ORDER_ID == EisOrderId
                                                   select HO;

                    foreach (HOST_ORDERS HOData in Data)
                    {
                        HOData.C_LAST_MODULE_ = Value;
                    }
                    wms.SaveChanges();
                }
            }
        }

        internal void UpdateLastUser(string Value)
        {
            if (LastUser != Value)
            {
                LastUser = Value;
                using (WMSEntities wms = new WMSEntities())
                {
                    IQueryable<HOST_ORDERS> Data = from HO in wms.HOST_ORDERS
                                                   where
                                                       HO.EIS_ORDER_ID == EisOrderId
                                                   select HO;

                    foreach (HOST_ORDERS HOData in Data)
                    {
                        HOData.C_LAST_USER_ = Value;
                    }
                    wms.SaveChanges();
                }
            }
        }
    }
}
