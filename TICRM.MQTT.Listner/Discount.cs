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
    
    public partial class Discount
    {
        public System.Guid DiscountId { get; set; }
        public string Name { get; set; }
        public Nullable<decimal> MinQuantity { get; set; }
        public Nullable<decimal> MaxQuantity { get; set; }
        public Nullable<System.Guid> ProductPriceId { get; set; }
        public Nullable<decimal> DiscountAmount { get; set; }
        public Nullable<decimal> DiscountPercentage { get; set; }
        public string Description { get; set; }
        public Nullable<System.Guid> StatusId { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
    }
}
