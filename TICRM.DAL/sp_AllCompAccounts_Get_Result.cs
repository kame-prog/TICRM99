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
    
    public partial class sp_AllCompAccounts_Get_Result
    {
        public string Name { get; set; }
        public Nullable<System.Guid> ShippingAddress { get; set; }
        public Nullable<System.Guid> BillingAddress { get; set; }
        public Nullable<System.Guid> AccountTypeId { get; set; }
        public string PhoneOffice { get; set; }
        public string Email { get; set; }
        public string Fax { get; set; }
        public string WebSite { get; set; }
        public Nullable<System.Guid> AccountSizeId { get; set; }
        public Nullable<System.Guid> IndustryId { get; set; }
        public string Description { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<System.Guid> StatusId { get; set; }
        public Nullable<System.Guid> AssignedUser { get; set; }
        public Nullable<System.Guid> AssignedTeam { get; set; }
        public Nullable<System.Guid> CurrencyId { get; set; }
        public Nullable<System.Guid> Company { get; set; }
        public System.Guid AccountId { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<decimal> Latitude { get; set; }
        public Nullable<decimal> Longitude { get; set; }
        public string CompanyName { get; set; }
    }
}