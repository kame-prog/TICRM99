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
    
    public partial class SkillLevel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SkillLevel()
        {
            this.ResourceSkills = new HashSet<ResourceSkill>();
        }
    
        public System.Guid SkillLevelId { get; set; }
        public string Name { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ResourceSkill> ResourceSkills { get; set; }
    }
}
