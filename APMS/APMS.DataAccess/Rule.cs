//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace APMS.DataAccess
{
    using System;
    using System.Collections.Generic;
    
    public partial class Rule
    {
        public string SensorId { get; set; }
        public int State { get; set; }
        public int OperatorType { get; set; }
        public double Value { get; set; }
    
        public virtual Sensor Sensor { get; set; }
    }
}
