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
    
    public partial class ReadingUnit
    {
        public ReadingUnit()
        {
            this.Readings = new HashSet<Reading>();
        }
    
        public System.Guid ReadingUnitId { get; set; }
        public string Name { get; set; }
        public Nullable<System.Guid> Type { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
    
        public virtual ICollection<Reading> Readings { get; set; }
        public virtual ReadingType ReadingType { get; set; }
    }
}
