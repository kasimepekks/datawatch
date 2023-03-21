using System;
using System.Collections.Generic;
using System.Text;

namespace Tools.MyConfig
{
    public class VehicleWatchPara
    {
        public string vehicleid { get; set; }
        public string filewatcherpath { get; set; }
        public byte monitorreducetimes { get; set; }
        public int intertimes { get; set; }
        public string csvcollumnname { get; set; }
        public string title { get; set; }
        public string[] echarttitle { get; set; }
        public string[] echartdata { get; set; }


    }
}
