using System;
using System.Collections.Generic;

#nullable disable

namespace MysqlforDataWatch
{
    public partial class t_vehiclecomputepara
    {
        public int id { get; set; }
        public string vehicleid { get; set; }
        public float wheelbaselower { get; set; }
        public float wheelbaseupper { get; set; }
        public byte bumpzerostandard { get; set; }
        public byte bumpmaxspeed { get; set; }
        public byte bumptimegap { get; set; }
        public byte accvaluegap { get; set; }
        public byte acctimegap { get; set; }
        public byte brakezerostandard { get; set; }
        public byte brakelastingpoints { get; set; }
        public byte steeringzerostandard { get; set; }
        public byte steeringlastingpoints { get; set; }
        public byte throttlezerostandard { get; set; }
        public byte throttlelastingpoints { get; set; }
    }
}
