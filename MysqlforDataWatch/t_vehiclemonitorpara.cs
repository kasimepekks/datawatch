using System;
using System.Collections.Generic;

#nullable disable

namespace MysqlforDataWatch
{
    public partial class t_vehiclemonitorpara
    {
        public int id { get; set; }
        public string vehicleid { get; set; }
        public byte monitorreductiontimes { get; set; }
        public string monitorinputpath { get; set; }
        public string monitorcsvcollumnname { get; set; }
        public string echart1channelname { get; set; }
        public string echart1title { get; set; }
        public string echart2channelname { get; set; }
        public string echart2title { get; set; }
        public string echart3channelname { get; set; }
        public string echart3title { get; set; }
        public string echart4channelname { get; set; }
        public string echart4title { get; set; }
        public string echart5channelname { get; set; }
        public string echart5title { get; set; }
        public string echart6channelname { get; set; }
        public string echart6title { get; set; }
    }
}
