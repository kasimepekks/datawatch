using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using MysqlforDataWatch;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tools.MyConfig
{
    //public struct VehiclePara
    //{
    //    public string vehicleid;
    //    public string importinputpath;
    //    public string importresultpath;
     
    //    public  float Wheelbaselower;
    //    public float Wheelbaseupper;
    //    public  byte BmupZeroStandard;
    //    public  byte AccValueGap;
    //    public  byte AccTimeGap;
    //    public  byte BumpTimeGap;
    //    public  byte BrakeZeroStandard;
    //    public  byte BrakeLastingPoints;
    //    public  byte SteeringZeroStandard;
    //    public  byte SteeringLastingPoints;
    //    public  byte ThrottleZeroStandard;
    //    public byte ThrottleLastingPoints;
    //    public  byte BumpMaxSpeed;
    //    public byte Reductiontimesforaccimport;
    //    public byte Reductiontimesforwftimport;
    //    public int samplerate;
    //    public byte reductiontimesforgps;
    //    public int gpspointsforanalysis;
    //    public byte GPSImport;
    //    public byte BrakeImport;
    //    public byte ThrottleImport;
    //    public byte SpeedImport;
    //    public byte StatisticImport;
    //    public byte BumpImport;
    //    public byte SteeringImport;
    //    public byte WFTImport;
    //    public byte monitoraccess;
    //    public byte analysisaccess;
    //    public byte importaccess;
    //    public byte predictaccess;
    //    public string speedcolumnname;
    //    public string throttlecolumnname;
    //    public string brakecolumnname;
    //    public string whlangcolumnname;
    //    public string whlanggrcolumnname;
    //    public string acczwhllf;
    //    public string acczwhlrf;
    //    public string acczwhllr;
    //    public string accybody;
    //    public string accxbody;
    //    //public string IsFileCombined;
    //}
   
    //public static class MyConfigforVehicleID
    //{
        
    //    /// <summary>
    //    /// 利用数据库里的车辆信息来替换配置文件里的信息
    //    /// </summary>
    //    /// <param name="vehicleidpara"></param>
    //    /// <param name="vehicletable"></param>
    //    /// <returns></returns>
    //    public static VehicleIDPara GetVehicleConfigurationfromsql(ref VehicleIDPara vehicleidpara, t_vehiclemaster t_vehiclemaster, t_vehicleimportpara t_vehicleimportpara,t_vehiclecomputepara t_vehiclecomputepara)
    //    {
    //        vehicleidpara.AccTimeGap= t_vehiclecomputepara.acctimegap;
    //        vehicleidpara.AccValueGap= t_vehiclecomputepara.accvaluegap;
    //        vehicleidpara.StatisticImport = t_vehicleimportpara.importstatistic;
    //        vehicleidpara.SpeedImport = t_vehicleimportpara.importspeed ;
    //        vehicleidpara.GPSImport = t_vehicleimportpara.importgps;
    //        vehicleidpara.BrakeImport = t_vehicleimportpara.importbrake;
    //        vehicleidpara.BumpImport = t_vehicleimportpara.importimpact;
    //        vehicleidpara.WFTImport = t_vehicleimportpara.importwft;
    //        vehicleidpara.ThrottleImport = t_vehicleimportpara.importthrottle;
    //        vehicleidpara.SteeringImport = t_vehicleimportpara.importsteering;
    //        vehicleidpara.BmupZeroStandard= t_vehiclecomputepara.bumpzerostandard;
    //        vehicleidpara.BumpMaxSpeed= t_vehiclecomputepara.bumpmaxspeed;
    //        vehicleidpara.BumpTimeGap= t_vehiclecomputepara.bumptimegap;
    //        vehicleidpara.BrakeZeroStandard= t_vehiclecomputepara.brakezerostandard;
    //        vehicleidpara.BrakeLastingPoints= t_vehiclecomputepara.brakelastingpoints;
    //        vehicleidpara.SteeringZeroStandard= t_vehiclecomputepara.steeringzerostandard;
    //        vehicleidpara.SteeringLastingPoints= t_vehiclecomputepara.steeringlastingpoints;
    //        vehicleidpara.ThrottleZeroStandard= t_vehiclecomputepara.throttlezerostandard;
    //        vehicleidpara.ThrottleLastingPoints = t_vehiclecomputepara.throttlelastingpoints;
          
    //        vehicleidpara.Reductiontimesforaccimport = t_vehicleimportpara.importaccreductiontimes;
    //        vehicleidpara.Reductiontimesforwftimport = t_vehicleimportpara.importwftreductiontimes;
    //        vehicleidpara.Wheelbaselower = t_vehiclecomputepara.wheelbaselower;
    //        vehicleidpara.Wheelbaseupper = t_vehiclecomputepara.wheelbaseupper;
    //        vehicleidpara.vehicleid = t_vehicleimportpara.vehicleid;
    //        vehicleidpara.analysisaccess = t_vehiclemaster.analysisaccess;
    //        vehicleidpara.importaccess = t_vehiclemaster.importaccess;
    //        vehicleidpara.predictaccess= t_vehiclemaster.predictaccess;
    //        vehicleidpara.gpspointsforanalysis = t_vehiclemaster.displaygpspoints;
    //        vehicleidpara.samplerate = t_vehiclemaster.samplerate;
    //        //vehicleidpara.reductiontimesformonitor = t_vehiclecomputepara.Reductiontimesformonitor;
    //        vehicleidpara.reductiontimesforgps = t_vehicleimportpara.importgpsreductiontimes;
          
    //        vehicleidpara.importresultpath= t_vehicleimportpara.importresultpath;
    //        vehicleidpara.importinputpath= t_vehicleimportpara.importinputpath;
    //        vehicleidpara.accxbody = String.IsNullOrEmpty(t_vehicleimportpara.accxbody) ? "": t_vehicleimportpara.accxbody.Replace("_", "").Replace("N", "").Replace(" ", "").ToLower();
    //        vehicleidpara.accybody= String.IsNullOrEmpty(t_vehicleimportpara.accybody) ? "" : t_vehicleimportpara.accybody.Replace("_", "").Replace("N", "").Replace(" ", "").ToLower();
    //        vehicleidpara.acczwhllf= String.IsNullOrEmpty(t_vehicleimportpara.acczwhllf) ? "" : t_vehicleimportpara.acczwhllf.Replace("_", "").Replace("N", "").Replace(" ", "").ToLower();
    //        vehicleidpara.acczwhlrf= String.IsNullOrEmpty(t_vehicleimportpara.acczwhlrf) ? "" : t_vehicleimportpara.acczwhlrf.Replace("_", "").Replace("N", "").Replace(" ", "").ToLower();
    //        vehicleidpara.acczwhllr= String.IsNullOrEmpty(t_vehicleimportpara.acczwhllr) ? "" : t_vehicleimportpara.acczwhllr.Replace("_", "").Replace("N", "").Replace(" ", "").ToLower();
    //        vehicleidpara.speedcolumnname = String.IsNullOrEmpty(t_vehicleimportpara.speedcolumnname) ? "" : t_vehicleimportpara.speedcolumnname.Replace("_", "").Replace("N", "").Replace(" ", "").ToLower();

    //        vehicleidpara.throttlecolumnname= String.IsNullOrEmpty(t_vehicleimportpara.throttlecolumnname) ? "" : t_vehicleimportpara.throttlecolumnname.Replace("_", "").Replace("N", "").Replace(" ", "").ToLower();
    //        vehicleidpara.brakecolumnname= String.IsNullOrEmpty(t_vehicleimportpara.brakecolumnname) ? "" : t_vehicleimportpara.brakecolumnname.Replace("_", "").Replace("N", "").Replace(" ", "").ToLower();
    //        vehicleidpara.whlanggrcolumnname= String.IsNullOrEmpty(t_vehicleimportpara.whlanggrcolumnname) ? "" : t_vehicleimportpara.whlanggrcolumnname.Replace("_", "").Replace("N", "").Replace(" ", "").ToLower();
    //        vehicleidpara.whlangcolumnname= String.IsNullOrEmpty(t_vehicleimportpara.whlangcolumnname) ? "" : t_vehicleimportpara.whlangcolumnname.Replace("_", "").Replace("N", "").Replace(" ", "").ToLower();

    //        return vehicleidpara;
    //    }
    //}
}
