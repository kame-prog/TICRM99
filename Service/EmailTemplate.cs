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
    
    public partial class EmailTemplate
    {
        public System.Guid EmailTemplateId { get; set; }
        public Nullable<System.Guid> EmailConfigurationId { get; set; }
        public Nullable<System.Guid> WorkFlowId { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
    
        public virtual EmailConfiguration EmailConfiguration { get; set; }
        public virtual WorkFlow WorkFlow { get; set; }
    }
}