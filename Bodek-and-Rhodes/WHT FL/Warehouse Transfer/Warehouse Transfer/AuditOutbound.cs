using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseTransfer
{
    public class AuditOutbound
    {
        public int RID { get; set; }
        public string  TranType { get; set; }
        public string SubType { get; set; }
        public DateTime TransactionTime { get; set; }
        public DateTime? AppointmentTime { get; set; }
        public DateTime? AppointmentTimeOld { get; set; }
        public string DoorID { get; set; }
        public string DoorIDOld { get; set; }
        public DateTime? MadeTime { get; set; }
        public DateTime? MadeTimeOld { get; set; }
        public int duration { get; set; }
        public int durationOld { get; set; }
        public string CarrierID { get; set; }
        public string  CarrierIdOld { get; set; }
        public string ShipmentID { get; set; }
        public string ShipmentIDOld { get; set; }
        public DateTime? ArrivalTime { get; set; }
        public DateTime ?PickAssignedTime { get; set; }
        public DateTime? PickStartTIme { get; set; }
        public DateTime? PickCompleateTIme { get; set; }
        public DateTime? ExportTimeSys1 { get; set; }
        public DateTime? DepartureTime { get; set; }
        public int ExportLoadNumSys1 { get; set; }
        public DateTime? ExportTimeSys2 { get; set; }
        public int ExportLoadNumSys2 { get; set; }
        public DateTime? ExportTimeSys3 { get; set; }
        public int ExportLoadNumSys3 { get; set; }
        public DateTime? LastUpdated { get; set; }
        public string LastUser { get; set; }
        public string LastModule { get; set; }
        public string Address1 { get; set; }
        public string Address1Old { get; set; }
        public string Address2 { get; set; }
        public string Address2Old { get; set; }
        public string City { get; set; }
        public string CityOld { get; set; }
        public string State { get; set; }
        public string StateOld { get; set; }
        public string Zip { get; set; }
        public string ZipOld { get; set; }
        public string PurchasOrder { get; set; }
        public string PurchasOrderOld { get; set; }
        public DateTime? PurchasOrderDate { get; set; }
        public DateTime? PurchasOrderDateOld { get; set; }
        public string Status { get; set; }
        public string StatusOld { get; set; }
        public string Comments1 { get; set; }
        public string ExportFDM4Batch { get; set; }


        public AuditOutbound(string InTranType,string InSubType)
        {

                RID = GetNextRID("Audit_Outbound");
                TranType = InTranType;
                SubType = InSubType;
                TransactionTime = DateTime.Now;

                AddToDataBase();
        
        }

        private void AddToDataBase()
        {
            using(WMSEntities wms=new WMSEntities())
            {
                var Outbound = new AUDIT_OUTBOUND
                {
                    C_RID_ = RID,
                    TRANTYPE=TranType,
                    SUBTYPE=SubType,
                    TRANSACTION_TIME=TransactionTime,
                    APPOINTMENT_TIME=AppointmentTime,
                    APPOINTMENT_TIME_OLD=AppointmentTimeOld,
                    DOOR_ID=DoorID,
                    DOOR_ID_OLD=DoorIDOld,
                    MADE_TIME=MadeTime,
                    MADE_TIME_OLD=MadeTimeOld,
                    DURATION=duration,
                    DURATION_OLD=durationOld,
                    CARRIER_ID=CarrierID,
                    CARRIER_ID_OLD=CarrierIdOld,
                    SHIPMENT_ID=ShipmentID,
                    SHIPMENT_ID_OLD=ShipmentIDOld,
                    ARRIVAL_TIME=ArrivalTime,
                    PICK_ASSIGNED_TIME=PickAssignedTime,
                    PICK_COMPLETE_TIME=PickCompleateTIme,
                    PICK_START_TIME=PickStartTIme,
                    DEPARTURE_TIME=DepartureTime,
                    EXPORTTIME_SYS1=ExportTimeSys1,
                    EXPORTLOADNUM_SYS1=ExportLoadNumSys1,
                    EXPORTTIME_SYS2=ExportTimeSys2,
                    EXPORTLOADNUM_SYS2=ExportLoadNumSys2,
                    EXPORTTIME_SYS3=ExportTimeSys3,
                    EXPORTLOADNUM_SYS3=ExportLoadNumSys3,
                    C_LAST_UPDATED_=LastUpdated,
                    C_LAST_USER_=LastUser,
                    C_LAST_MODULE_=LastModule,
                    ADDRESS_1=Address1,
                    ADDRESS_1_OLD=Address1Old,
                    ADDRESS_2=Address2,
                    ADDRESS_2_OLD=Address2Old,
                    CITY=City,
                    CITY_OLD=CityOld,
                    STATE=State,
                    STATE_OLD=StateOld,
                    ZIP=Zip,
                    ZIP_OLD=ZipOld,
                    PURCHASE_ORDER=PurchasOrder,
                    PURCHASE_ORDER_OLD=PurchasOrderOld,
                    PURCHASE_ORDER_DATE=PurchasOrderDate,
                    PURCHASE_ORDER_DATE_OLD=PurchasOrderDateOld,
                    STATUS=Status,
                    STATUS_OLD=StatusOld,
                    COMMENTS_1=Comments1,
                    EXPORTFDM4BATCH_SYS1=ExportFDM4Batch
                };
                wms.AUDIT_OUTBOUND.Add(Outbound);
                wms.SaveChanges();
            }
        }

        private int GetNextRID(string Table)
        {
            int RID = 0;

            using (WMSEntities wms = new WMSEntities())
            {
                IQueryable<SYSTEMNEXTRID> Item = from NewRid in wms.SYSTEMNEXTRIDs where NewRid.GENERICNAME == Table select NewRid;
                foreach (SYSTEMNEXTRID New in Item)
                {
                    New.NEXTRID++;
                    RID = New.NEXTRID;
                    wms.SaveChanges();
                }
            }

            return RID;
        }



        internal void UpdateTransactionTime(DateTime Value)
        {
            TransactionTime = Value;
             using (WMSEntities wms = new WMSEntities())
             {
                 IQueryable<AUDIT_OUTBOUND> Data = from AO in wms.AUDIT_OUTBOUND where AO.C_RID_ == RID select AO;

                 foreach (AUDIT_OUTBOUND AOData in Data)
                 {
                     AOData.TRANSACTION_TIME = TransactionTime;
                 }
                 wms.SaveChanges();
             }
        }

        internal void UpdateShipmentID(string Value)
        {
            if (ShipmentID != Value)
            {
                ShipmentID = Value;
                using (WMSEntities wms = new WMSEntities())
                {
                     IQueryable<AUDIT_OUTBOUND> Data = from AO in wms.AUDIT_OUTBOUND where AO.C_RID_ == RID select AO;

                     foreach (AUDIT_OUTBOUND AOData in Data)
                     {

                         AOData.SHIPMENT_ID = ShipmentID;
                     }
                    wms.SaveChanges();
                }
            }
        }

        internal void UpdateStatus(string Value)
        {
            if (Status != Value)
            {
                Status = Value;
                using (WMSEntities wms = new WMSEntities())
                {
                    IQueryable<AUDIT_OUTBOUND> Data = from AO in wms.AUDIT_OUTBOUND where AO.C_RID_ == RID select AO;

                    foreach (AUDIT_OUTBOUND AOData in Data)
                    {

                        AOData.STATUS = Status;
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
                    IQueryable<AUDIT_OUTBOUND> Data = from AO in wms.AUDIT_OUTBOUND where AO.C_RID_ == RID select AO;

                    foreach (AUDIT_OUTBOUND AOData in Data)
                    {

                        AOData.C_LAST_MODULE_ = LastModule;
                    }
                    wms.SaveChanges();
                }
            }
        }
    }
}
