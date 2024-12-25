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
    
    public partial class sp_AllCompDevices_Get_Result
    {
        public System.Guid DeviceId { get; set; }
        public string Name { get; set; }
        public string Mac { get; set; }
        public string EMEINumber { get; set; }
        public Nullable<System.DateTime> RegistrationDate { get; set; }
        public Nullable<System.DateTime> ServiceDate { get; set; }
        public bool ServiceDateFlag { get; set; }
        public Nullable<decimal> Latitude { get; set; }
        public Nullable<decimal> Longitude { get; set; }
        public Nullable<System.Guid> AccountId { get; set; }
        public Nullable<System.Guid> CustomerAssetId { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<System.Guid> StatusId { get; set; }
        public string CloudServices { get; set; }
        public string CloudData { get; set; }
        public string Maintenance { get; set; }
        public Nullable<System.Guid> AssignedUser { get; set; }
        public Nullable<System.Guid> AssignedTeam { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public bool IsGateway { get; set; }
        public Nullable<System.DateTime> LastMessage { get; set; }
        public Nullable<System.Guid> GatewayReference { get; set; }
        public string Data { get; set; }
        public Nullable<System.Guid> Company { get; set; }
    }
}
