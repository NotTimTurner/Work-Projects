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
    
    public partial class PURGE_ARCHIVE_GROUP
    {
        public string GROUP_NAME { get; set; }
        public string GROUP_MEMBER { get; set; }
        public string MEMBER_TYPE { get; set; }
        public string SERVER_NAME { get; set; }
        public string ARCHIVE_FILE { get; set; }
        public Nullable<short> MEMBER_PRIORITY { get; set; }
        public Nullable<short> MEMBER_SUB_PRIORITY { get; set; }
        public string WHERE_CONDITION { get; set; }
        public string PA_KEY { get; set; }
        public string FOREIGN_PA_KEY { get; set; }
        public Nullable<System.DateTime> C_LAST_UPDATED_ { get; set; }
        public string C_LAST_USER_ { get; set; }
        public string C_LAST_MODULE_ { get; set; }
        public string PA_DATE_COLUMN { get; set; }
    }
}
