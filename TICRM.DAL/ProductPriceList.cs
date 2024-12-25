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
    
    public partial class ProductPriceList
    {
        public System.Guid ProductPriceId { get; set; }
        public Nullable<System.Guid> CurrencyId { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<System.Guid> ProductId { get; set; }
        public Nullable<System.Guid> StatusId { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
    
        public virtual Currency Currency { get; set; }
        public virtual ProductCatelog ProductCatelog { get; set; }
        public virtual Status Status { get; set; }
    }
}
