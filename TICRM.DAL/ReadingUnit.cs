//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TICRM.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class ReadingUnit
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ReadingUnit()
        {
            this.Readings = new HashSet<Reading>();
        }
    
        public System.Guid ReadingUnitId { get; set; }
        public string Name { get; set; }
        public Nullable<System.Guid> Type { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Reading> Readings { get; set; }
        public virtual ReadingType ReadingType { get; set; }
    }
}
