using System;
using System.Collections.Generic;
using System.Text;

namespace Tools.MyConfig
{
    public  class VehicleIDPara
    {
        public string vehicleid { get; set; }
        public string importinputpath { get; set; }
        public string importresultpath { get; set; }
        public float Wheelbaselower { get; set; }
        public float Wheelbaseupper { get; set; }
        public byte BmupZeroStandard { get; set; }
        public byte AccValueGap { get; set; }
        public byte AccTimeGap { get; set; }
        public byte BumpTimeGap { get; set; }
        public byte BrakeZeroStandard { get; set; }
        public byte BrakeLastingPoints { get; set; }
        public byte SteeringZeroStandard { get; set; }
        public byte SteeringLastingPoints { get; set; }
        public byte ThrottleZeroStandard { get; set; }
        public byte ThrottleLastingPoints { get; set; }
        public byte BumpMaxSpeed { get; set; }
        public byte Reductiontimesforaccimport { get; set; }
        public byte Reductiontimesforwftimport { get; set; }
        public int samplerate { get; set; }
        public byte reductiontimesforgps { get; set; }
        public int gpspointsforanalysis { get; set; }
        public byte GPSImport { get; set; }
        public byte importpuma { get; set; }
        public byte BrakeImport { get; set; }
        public byte ThrottleImport { get; set; }
        public byte SpeedImport { get; set; }
        public byte StatisticImport { get; set; }
        public byte BumpImport { get; set; }
        public byte SteeringImport { get; set; }
        public byte WFTImport { get; set; }
        public byte importengspd { get; set; }
        public byte monitoraccess { get; set; }
        public byte analysisaccess { get; set; }
        public byte importaccess { get; set; }
        public byte predictaccess { get; set; }
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
