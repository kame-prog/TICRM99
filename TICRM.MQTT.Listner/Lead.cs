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
    
    public partial class Lead
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Lead()
        {
            this.LeadAccounts = new HashSet<LeadAccount>();
        }
    
        public System.Guid LeadId { get; set; }
        public string Name { get; set; }
        public Nullable<System.Guid> LeadTypeId { get; set; }
        public Nullable<System.Guid> LeadSourceId { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public Nullable<System.Guid> AddressId { get; set; }
        public Nullable<System.Guid> IndustryId { get; set; }
        public string Description { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<System.Guid> StatusId { get; set; }
        public Nullable<System.Guid> AssignedUser { get; set; }
        public Nullable<System.Guid> AssignedTeam { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
    
        public virtual Address Address { get; set; }
        public virtual Industry Industry { get; set; }
        public virtual LeadSource LeadSource { get; set; }
        public virtual LeadType LeadType { get; set; }
        public virtual Status Status { get; set; }
        public virtual Team Team { get; set; }
        public virtual User User { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LeadAccount> LeadAccounts { get; set; }
    }
}
