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
    
    public partial class SensorData
    {
        public long SensorDataId { get; set; }
        public Nullable<System.Guid> DeviceSensorId { get; set; }
        public Nullable<double> SensorValue { get; set; }
        public Nullable<System.DateTime> RecordDate { get; set; }
    
        public virtual DeviceSensor DeviceSensor { get; set; }
    }
}
