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
    
    public partial class EventNotification
    {
        public System.Guid EventNotificationId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public Nullable<bool> Status { get; set; }
        public string Message { get; set; }
        public string Path { get; set; }
        public string Color { get; set; }
        public string IPAddress { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
    }
}
