using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;



    public class MoveQueue
    {
        DatabaseConnection DbCon;
        private string conString;
        private DataSet ds;
        private DataRow dRow;
        public int RID { get; set; }
        public int InvRID { get; set; }
        public string MQType { get; set; }
        public int Priority { get; set; }
        public DateTime CreateTime { get; set; }
        public string Status { get; set; }
        public string LabelLevel { get; set; }
        public string ToZone { get; set; }
        public string ToLocation { get; set; }
        public string FromZone { get; set; }
        public string FromLocation { get; set; }
        public float? OrigionalQuantityRequired { get; set; }
        public float? QuantityRequired { get; set; }
        public float? QuantityInTransit { get; set; }
        public string UserIdInTransit { get; set; }
        public string VehicleInTranset { get; set; }
        public string shipmentID { get; set; }
        public string OrderNumber { get; set; }
        public float? OrderLine { get; set; }
        public DateTime? LastUpdated { get; set; }
        public string LastUser { get; set; }
        public string LastModule { get; set; }

        public MoveQueue()
        {

        }

        public MoveQueue(string InShipmentID, string InOrderNumber, string InLocationID)
        {
            var con = DbCon.GetPhillyConString();
            using (SqlConnection myConnection = new SqlConnection(con))
            {
               // string selectString="select top 1 from Move_queue where shipment"
            }


        }



        //public MoveQueue(int InInvRID, string InShipmentID)
        //{
        //    using (WMSEntities wms = new WMSEntities())
        //    {
        //        var Move = (from MQ in wms.MOVE_QUEUE where MQ.INV_RID == InInvRID && MQ.SHIPMENT_ID == InShipmentID select MQ).SingleOrDefault();

        //        if (Move != null)
        //        {
        //            RID = Move.C_RID_;
        //            InvRID = Move.INV_RID;
        //            MQType = Move.TYPE;
        //            Priority = Move.PRIORITY;
        //            CreateTime = Move.CREATE_TIME;
        //            Status = Move.STATUS;
        //            LabelLevel = Move.LABELLEVEL;
        //            ToZone = Move.TO_ZONE;
        //            ToLocation = Move.TO_LOCATION;
        //            FromLocation = Move.FROM_LOCATION;
        //            OrigionalQuantityRequired = Move.ORIG_QUANTITY_REQUIRED;
        //            QuantityRequired = Move.QUANTITY_REQUIRED;
        //            QuantityInTransit = Move.QUANTITY_INTRANSIT;
        //            UserIdInTransit = Move.USER_ID_INTRANSIT;
        //            VehicleInTranset = Move.VEHICLE_INTRANSIT;
        //            shipmentID = Move.SHIPMENT_ID;
        //            OrderNumber = Move.ORDER_NUMBER;
        //            OrderLine = Move.ORDER_LINE;
        //            LastUpdated = Move.C_LAST_UPDATED_;
        //            LastModule = Move.C_LAST_MODULE_;
        //            LastUser = Move.C_LAST_USER_;


        //        }
        //    }
        //}

        //public MoveQueue()
        //{
        //    // TODO: Complete member initialization
        //}

      


        //internal void CreateMoveQueue(string CarrierID)
        //{

        //    if (MQType != "REPLEN")
        //    {
        //        CarrierTable Carrier = new CarrierTable(CarrierID);
        //        if (Carrier.Type == "Parcel")
        //            ToLocation = "CONVEYOR";
        //        else
        //            ToLocation = "SHIPSTAGE";
        //    }
        //    if (CreateTime == Convert.ToDateTime("1/1/0001"))
        //    {
        //        CreateTime = DateTime.Now;
        //    }
        //    if (Status == null || Status == "")
        //    {
        //        Status = "N";
        //    }

        //    LastUpdated = DateTime.Now;
        //    LastUser = Environment.MachineName;
        //    LastModule = "Warehouse Transfer";
        //    RID = GetNextRID("MOVE_QUEUE");

        //    var WMS = new WMSEntities();

        //    var MoveQ = new MOVE_QUEUE
        //    {
        //        C_RID_ = RID,
        //        INV_RID = InvRID,
        //        TYPE = MQType,
        //        SHIPMENT_ID = shipmentID,
        //        ORDER_NUMBER = OrderNumber,
        //        ORDER_LINE = (short?)OrderLine,
        //        PRIORITY = (short)Priority,
        //        CREATE_TIME = CreateTime,
        //        STATUS = Status,
        //        TO_LOCATION = ToLocation,
        //        FROM_LOCATION = FromLocation,
        //        TO_ZONE = ToZone,
        //        FROM_ZONE = FromZone,
        //        ORIG_QUANTITY_REQUIRED = OrigionalQuantityRequired,
        //        QUANTITY_INTRANSIT = QuantityInTransit,
        //        LABELLEVEL = LabelLevel,
        //        C_LAST_UPDATED_ = LastUpdated,
        //        C_LAST_MODULE_ = LastModule,
        //        C_LAST_USER_ = LastUser



        //    };
        //    WMS.MOVE_QUEUE.Add(MoveQ);
        //    //Tim Uncomment 
        //    WMS.SaveChanges();
        //    // PrintMoveQueue();
        //}

        //private int GetNextRID(string Table)
        //{
        //    int RID = 0;

        //    using (WMSEntities wms = new WMSEntities())
        //    {
        //        IQueryable<SYSTEMNEXTRID> Item = from NewRid in wms.SYSTEMNEXTRIDs where NewRid.GENERICNAME == Table select NewRid;
        //        foreach (SYSTEMNEXTRID New in Item)
        //        {
        //            New.NEXTRID++;
        //            RID = New.NEXTRID;
        //            wms.SaveChanges();
        //        }
        //    }

        //    return RID;
        //}



        //internal void DeleteMoveQueueEntry(int InRID)
        //{
        //    using (WMSEntities wms = new WMSEntities())
        //    {
        //        IQueryable<MOVE_QUEUE> MoveQ = from MQ in wms.MOVE_QUEUE where MQ.C_RID_ == InRID select MQ;

        //        foreach (MOVE_QUEUE move in MoveQ)
        //        {
        //            wms.MOVE_QUEUE.Remove(move);
        //        }
        //        wms.SaveChanges();

        //    }
        //}

        public int GetRequiredQuantity(string shipmentID, string OrderNumber, string LocationID, int EnteredQuantity)
        {



            return 0;
        }
    }


