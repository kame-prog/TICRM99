//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Service
{
    using System;
    using System.Collections.Generic;
    
    public partial class WorkOrder
    {
        public System.Guid WorkOrderId { get; set; }
        public string Title { get; set; }
        public Nullable<decimal> NTE { get; set; }
        public Nullable<System.Guid> WorkOrderStageId { get; set; }
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
        public virtual User User { get; set; }
        public virtual WorkStage WorkStage { get; set; }
    }
}