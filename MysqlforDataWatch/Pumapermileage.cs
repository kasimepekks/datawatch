using System;
using System.Collections.Generic;

#nullable disable

namespace MysqlforDataWatch
{
    public partial class Pumapermileage
    {
        public int id { get; set; }
        public string vehicle { get; set; }
        public string filename { get; set; }
        public DateTime filedate { get; set; }
        public double? averagespeed { get; set; }
        public double? mileage { get; set; }
        public double? duration { get; set; }//单个里程的持续时间
        public double? maxthrottle { get; set; }
        public double? maxbrake { get; set; }
        public double? maxangle { get; set; }
        public double? minangle { get; set; }
        public decimal? wftfxlfk3 { get; set; }
        public decimal? wftfxrfk3 { get; set; }
        public decimal? wftfxlrk3 { get; set; }
        public decimal? wftfxrrk3 { get; set; }
        public decimal? wftfylfk3 { get; set; }
        public decimal? wftfyrfk3 { get; set; }
        public decimal? wftfylrk3 { get; set; }
        public decimal? wftfyrrk3 { get; set; }
        public decimal? wftfzlfk3 { get; set; }
        public decimal? wftfzrfk3 { get; set; }
        public decimal? wftfzlrk3 { get; set; }
        public decimal? wftfzrrk3 { get; set; }
        public decimal? wftfxlfk5 { get; set; }
        public decimal? wftfxrfk5 { get; set; }
        public decimal? wftfxlrk5 { get; set; }
        public decimal? wftfxrrk5 { get; set; }
        public decimal? wftfylfk5 { get; set; }
        public decimal? wftfyrfk5 { get; set; }
        public decimal? wftfylrk5 { get; set; }
        public decimal? wftfyrrk5 { get; set; }
        public decimal? wftfzlfk5 { get; set; }
        public decimal? wftfzrfk5 { get; set; }
        public decimal? wftfzlrk5 { get; set; }
        public decimal? wftfzrrk5 { get; set; }
    }
}
