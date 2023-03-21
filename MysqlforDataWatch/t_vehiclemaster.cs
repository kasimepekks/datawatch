using System;
using System.Collections.Generic;

#nullable disable

namespace MysqlforDataWatch
{
    public partial class t_vehiclemaster
    {
        public int id { get; set; }
        public string vehicleid { get; set; }
        public int samplerate { get; set; }
        public int numberpoints { get; set; }
        public byte importaccess { get; set; }
        public byte analysisaccess { get; set; }
        public byte predictaccess { get; set; }
        public int displaygpspoints { get; set; }
        public string area { get; set; }
        public string country { get; set; }
        public byte state { get; set; }
        public string remarks { get; set; }
    }
}
