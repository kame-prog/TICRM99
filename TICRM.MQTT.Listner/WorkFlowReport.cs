//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TICRM.MQTT.Listner
{
    using System;
    using System.Collections.Generic;
    
    public partial class WorkFlowReport
    {
        public System.Guid WorkFlowReportId { get; set; }
        public Nullable<System.Guid> WorkFlowId { get; set; }
        public string WorkFlowStatus { get; set; }
        public string Action { get; set; }
        public string WorkFlowActionStatus { get; set; }
        public string WorkFlowDesign { get; set; }
        public string AppliedTo { get; set; }
        public Nullable<int> Frequency { get; set; }
        public Nullable<int> Priority { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public string AccountId { get; set; }
        public string DeviceName { get; set; }
    
        public virtual WorkFlow WorkFlow { get; set; }
    }
}
