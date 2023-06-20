using System;
using System.Collections.Generic;

#nullable disable

namespace MysqlforDataWatch
{
    public partial class t_vehicleimportpara
    {
        public int id { get; set; }
        public string vehicleid { get; set; }
        public string importinputpath { get; set; }
        public string importresultpath { get; set; }
      
        public byte importgpsreductiontimes { get; set; }
        public byte importaccreductiontimes { get; set; }
        public byte importwftreductiontimes { get; set; }
        public byte importgps { get; set; }
        public byte importbrake { get; set; }
        public byte importthrottle { get; set; }
        public byte importspeed { get; set; }
        public byte importstatistic { get; set; }
        public byte importimpact { get; set; }
        public byte importsteering { get; set; }
        public byte importwft { get; set; }
        public byte importengspd { get; set; }

        public byte importpuma { get; set; }
        public string speedcolumnname { get; set; }
        public string throttlecolumnname { get; set; }
        public string brakecolumnname { get; set; }
        public string whlangcolumnname { get; set; }
        public string whlanggrcolumnname { get; set; }
        public string acczwhllf { get; set; }
        public string acczwhlrf { get; set; }
        public string acczwhllr { get; set; }
        public string accybody { get; set; }
        public string accxbody { get; set; }
        public string enginespeed { get; set; }
    }
}
