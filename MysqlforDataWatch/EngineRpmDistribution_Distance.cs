using System;
using System.Collections.Generic;

#nullable disable

namespace MysqlforDataWatch
{
    public partial class EngineRpmDistribution_Distance
    {
        public string Id { get; set; }
        public string VehicleId { get; set; }
        public DateTime? Datadate { get; set; }
        public double? _01000 { get; set; }
        public double? _10001500 { get; set; }
        public double? _15002000 { get; set; }
        public double? _20002500 { get; set; }
        public double? _25003000 { get; set; }
        public double? _30004000 { get; set; }
        public double? _40005000 { get; set; }
        public double? _50006000 { get; set; }
        public double? _Above6000 { get; set; }

    }
}
