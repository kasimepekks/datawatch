using System;
using System.Collections.Generic;

#nullable disable

namespace MysqlforDataWatch
{
    public partial class EngineRpmDistribution_Time
    {
        public string Id { get; set; }
        public string VehicleId { get; set; }
        public DateTime? Datadate { get; set; }
        public int? _01000 { get; set; }
        public int? _10001500 { get; set; }
        public int? _15002000 { get; set; }
        public int? _20002500 { get; set; }
        public int? _25003000 { get; set; }
        public int? _30004000 { get; set; }
        public int? _40005000 { get; set; }
        public int? _50006000 { get; set; }
        public int? _Above6000 { get; set; }

    }
}
