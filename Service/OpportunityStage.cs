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
    
    public partial class OpportunityStage
    {
        public OpportunityStage()
        {
            this.Opportunities = new HashSet<Opportunity>();
        }
    
        public System.Guid OpportunityStageId { get; set; }
        public string Name { get; set; }
    
        public virtual ICollection<Opportunity> Opportunities { get; set; }
    }
}
