using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseTransfer
{
    public class CarrierTable
    {
        DebugLog log = new DebugLog();
        public string CarrierID { get; set; }
        public string PoolPoint { get; set; }
        public string Name { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZIP { get; set; }
        public string ContactName { get; set; }
        public string ContactPhone { get; set; }
        public string Comments1 { get; set; }
        public string Comments2 { get; set; }
        public string Comments3 { get; set; }
        public string Comments4 { get; set; }
        public string AccountNumber { get; set; }
        public string ICCNumber { get; set; }
        public DateTime? LastUpdated { get; set; }
        public string LastUser { get; set; }
        public string LastModule { get; set; }
        public string CompanyName { get; set; }
        public string Type { get; set; }
        public string ShipModeDefault { get; set; }



        public CarrierTable(string ID)
        {
            using (WMSEntities wms = new WMSEntities())
            {
                IQueryable<CARRIER> Data = from Cr in wms.CARRIERS where Cr.CARRIER_ID == ID select Cr;
                foreach (CARRIER Carr in Data)
                {
                    CarrierID = Carr.CARRIER_ID;
                    PoolPoint = Carr.POOL_POINT;
                    Name = Carr.NAME;
                    Address1 = Carr.ADDRESS_1;
                    Address2 = Carr.ADDRESS_2;
                    City = Carr.CITY;
                    State = Carr.STATE;
                    ZIP = Carr.ZIP;
                    ContactName = Carr.CONTACT_NAME;
                    ContactPhone = Carr.CONTACT_PHONE;
                    Comments1 = Carr.COMMENTS_1;
                    Comments2 = Carr.COMMENTS_2;
                    Comments3 = Carr.COMMENTS_3;
                    Comments4 = Carr.COMMENTS_4;
                    AccountNumber = Carr.ACCOUNT_NUMBER;
                    ICCNumber = Carr.ACCOUNT_NUMBER;
                    LastUpdated = Carr.C_LAST_UPDATED_;
                    LastUser = Carr.C_LAST_USER_;
                    LastModule = Carr.C_LAST_MODULE_;
                    CompanyName = Carr.COMPANY_NAME;
                    Type = Carr.TYPE;
                    ShipModeDefault = Carr.SHIP_MODE_DEFAULT;

                }
            }

        }

        public void PrintCarrierTable()
        {
            log.Write("Carrier ID: " + CarrierID);
            log.Write("Pool Point: " + PoolPoint);
            log.Write("Name: " + Name);
            log.Write("Address 1: " + Address1);
            log.Write("Address 2: " + Address2);
            log.Write("City: " + City);
            log.Write("State: " + State);
            log.Write("Zip: " + ZIP);
            log.Write("Contact Name: " + ContactName);
            log.Write("Contact Phone: " + ContactPhone);
            log.Write("Comments 1: " + Comments1);
            log.Write("Comments 2: " + Comments2);
            log.Write("comments 3: " + Comments3);
            log.Write("comments 4: " + Comments4);
            log.Write("Account Number: " + AccountNumber);
            log.Write("ICC Number: " + ICCNumber);
            log.Write("last Updated: " + LastUpdated);
            log.Write("last User: " + LastUser);
            log.Write("Last Module: " + LastModule);
            log.Write("Company Name: " + CompanyName);
            log.Write("Type: " + Type);
            log.Write("ship Mode Default: " + ShipModeDefault);
        }
    }
}
