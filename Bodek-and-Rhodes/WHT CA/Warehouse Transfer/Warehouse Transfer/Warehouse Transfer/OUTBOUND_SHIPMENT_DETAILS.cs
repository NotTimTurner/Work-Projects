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
    
    public partial class OUTBOUND_SHIPMENT_DETAILS
    {
        public string SHIPMENT_ID { get; set; }
        public string ORDER_NUMBER { get; set; }
        public short ORDER_LINE { get; set; }
        public string TYPE { get; set; }
        public string PRODUCT_ID { get; set; }
        public string QC_CATEGORY { get; set; }
        public string UOM { get; set; }
        public Nullable<float> UOM_WEIGHT { get; set; }
        public Nullable<float> ORDERED_UNITS { get; set; }
        public Nullable<float> STAGED_UNITS { get; set; }
        public Nullable<float> SHIPPED_UNITS { get; set; }
        public string STATUS { get; set; }
        public Nullable<System.DateTime> SCHEDULED_TIME { get; set; }
        public Nullable<System.DateTime> PICK_ASSIGNED_TIME { get; set; }
        public Nullable<System.DateTime> PICK_START_TIME { get; set; }
        public Nullable<System.DateTime> PICK_COMPLETE_TIME { get; set; }
        public Nullable<decimal> UNIT_PRICE_EA { get; set; }
        public string NOTES { get; set; }
        public Nullable<System.DateTime> C_LAST_UPDATED_ { get; set; }
        public string C_LAST_USER_ { get; set; }
        public string C_LAST_MODULE_ { get; set; }
        public string ALLOCATION_STATUS { get; set; }
    }
}