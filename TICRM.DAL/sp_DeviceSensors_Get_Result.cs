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
    
    public partial class sp_DeviceSensors_Get_Result
    {
        public System.Guid DeviceSensorGraphId { get; set; }
        public Nullable<System.Guid> DeviceId { get; set; }
        public Nullable<System.Guid> SensorId { get; set; }
        public Nullable<System.Guid> GraphId { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public string Channel { get; set; }
        public string Network { get; set; }
        public string Data { get; set; }
        public Nullable<int> TankLevel { get; set; }
        public Nullable<System.Guid> Company { get; set; }
    }
}
