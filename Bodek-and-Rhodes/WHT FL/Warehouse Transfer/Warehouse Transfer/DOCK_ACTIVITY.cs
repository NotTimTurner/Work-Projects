//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WarehouseTransfer
{
    using System;
    using System.Collections.Generic;
    
    public partial class DOCK_ACTIVITY
    {
        public int C_RID_ { get; set; }
        public string TRANTYPE { get; set; }
        public string STATUS { get; set; }
        public System.DateTime APPOINTMENT_DATE { get; set; }
        public string DOOR_ID { get; set; }
        public Nullable<System.DateTime> DATE_MADE { get; set; }
        public Nullable<int> DURATION { get; set; }
        public string CARRIER_ID { get; set; }
        public string SHIPMENT_ID { get; set; }
        public Nullable<System.DateTime> ORIGINAL_DATE { get; set; }
        public string ORIGINAL_DOOR { get; set; }
        public Nullable<int> ORIGINAL_DURATION { get; set; }
        public Nullable<System.DateTime> ARRIVAL_DATE { get; set; }
        public Nullable<System.DateTime> DRIVER_ASSIGNED_TIME { get; set; }
        public Nullable<System.DateTime> PICK_START_TIME { get; set; }
        public Nullable<System.DateTime> PICK_COMPLETE_TIME { get; set; }
        public Nullable<System.DateTime> DEPARTURE_TIME { get; set; }
        public bool ALLOWPICKING { get; set; }
        public bool ALLOWLINETOTRUCK { get; set; }
        public bool ALLOWXDOCKING { get; set; }
        public Nullable<System.DateTime> C_LAST_UPDATED_ { get; set; }
        public string C_LAST_USER_ { get; set; }
        public string C_LAST_MODULE_ { get; set; }
    }
}