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
    
    public partial class LOCATION_CAPACITY
    {
        public string LOCATION_ID { get; set; }
        public string UOM { get; set; }
        public string PRODUCT_ID { get; set; }
        public Nullable<float> CAPACITY { get; set; }
        public bool USE_PRODUCTSPECIFIC { get; set; }
        public Nullable<float> REPLENISH_LB_QTY { get; set; }
        public string REPLENISH_LB_UOM { get; set; }
        public Nullable<float> REPLENISH_UB_QTY { get; set; }
        public string REPLENISH_UB_UOM { get; set; }
        public Nullable<System.DateTime> C_LAST_UPDATED_ { get; set; }
        public string C_LAST_USER_ { get; set; }
        public string C_LAST_MODULE_ { get; set; }
    }
}