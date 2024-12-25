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
    
    public partial class WorkFlow
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public WorkFlow()
        {
            this.EmailTemplates = new HashSet<EmailTemplate>();
            this.WorkFlowMappings = new HashSet<WorkFlowMapping>();
            this.WorkFlowReports = new HashSet<WorkFlowReport>();
        }
    
        public System.Guid WorkFlowId { get; set; }
        public string Name { get; set; }
        public string TriggerCondition { get; set; }
        public Nullable<System.DateTime> TriggerIn { get; set; }
        public Nullable<System.DateTime> TriggerOut { get; set; }
        public string TargetOn { get; set; }
        public string Description { get; set; }
        public string WorkFlowStatus { get; set; }
        public string AppliedTo { get; set; }
        public Nullable<int> Frequency { get; set; }
        public Nullable<int> FrequencyOut { get; set; }
        public Nullable<int> Priority { get; set; }
        public Nullable<System.Guid> AssignedUser { get; set; }
        public Nullable<System.Guid> AssignedTeam { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string WorkFlowDesign { get; set; }
        public string Action { get; set; }
        public string AccountId { get; set; }
        public string Threshold { get; set; }
        public string DeviceName { get; set; }
        public string DeviceMac { get; set; }
        public string Cloud { get; set; }
        public Nullable<System.Guid> RelatedToId { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EmailTemplate> EmailTemplates { get; set; }
        public virtual Team Team { get; set; }
        public virtual User User { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WorkFlowMapping> WorkFlowMappings { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WorkFlowReport> WorkFlowReports { get; set; }
    }
}