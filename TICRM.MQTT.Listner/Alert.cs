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
    
    public partial class Alert
    {
        public System.Guid AlertId { get; set; }
        public string Title { get; set; }
        public System.Guid UrgencyId { get; set; }
        public string Description { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<System.Guid> StatusId { get; set; }
        public Nullable<System.Guid> AssignedUser { get; set; }
        public Nullable<System.Guid> AssignedTeam { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
    
        public virtual Status Status { get; set; }
        public virtual Team Team { get; set; }
        public virtual Urgency Urgency { get; set; }
        public virtual User User { get; set; }
    }
}
