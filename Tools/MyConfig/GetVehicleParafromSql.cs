using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MysqlforDataWatch;

namespace Tools.MyConfig
{
    public  class GetVehicleParafromSql
    {
        private readonly DbContext _db;
        public GetVehicleParafromSql(DbContext db)
        {
            _db=db; 
        }
        public VehicleIDPara GetDatafromSql(string vehicleid)
        {
            
            var t_vehiclemaster = _db.Set<t_vehiclemaster>().AsNoTracking().Where(a => a.vehicleid == vehicleid).FirstOrDefault();
            var t_vehicleimportpara = _db.Set<t_vehicleimportpara>().AsNoTracking().Where(a => a.vehicleid == vehicleid).FirstOrDefault();
            var t_vehiclecomputepara = _db.Set<t_vehiclecomputepara>().AsNoTracking().Where(a => a.vehicleid == vehicleid).FirstOrDefault();
            if(t_vehiclemaster != null&& t_vehicleimportpara!=null&& t_vehiclecomputepara != null)
            {
                VehicleIDPara vehicleidpara = new VehicleIDPara();
                vehicleidpara.AccTimeGap = t_vehiclecomputepara.acctimegap;
                vehicleidpara.AccValueGap = t_vehiclecomputepara.accvaluegap;
                vehicleidpara.StatisticImport = t_vehicleimportpara.importstatistic;
                vehicleidpara.SpeedImport = t_vehicleimportpara.importspeed;
                vehicleidpara.importpuma = t_vehicleimportpara.importpuma;
                vehicleidpara.GPSImport = t_vehicleimportpara.importgps;
                vehicleidpara.BrakeImport = t_vehicleimportpara.importbrake;
                vehicleidpara.BumpImport = t_vehicleimportpara.importimpact;
                vehicleidpara.WFTImport = t_vehicleimportpara.importwft;
                vehicleidpara.ThrottleImport = t_vehicleimportpara.importthrottle;
                vehicleidpara.SteeringImport = t_vehicleimportpara.importsteering;
                vehicleidpara.importengspd = t_vehicleimportpara.importengspd;
                vehicleidpara.BmupZeroStandard = t_vehiclecomputepara.bumpzerostandard;
                vehicleidpara.BumpMaxSpeed = t_vehiclecomputepara.bumpmaxspeed;
                vehicleidpara.BumpTimeGap = t_vehiclecomputepara.bumptimegap;
                vehicleidpara.BrakeZeroStandard = t_vehiclecomputepara.brakezerostandard;
                vehicleidpara.BrakeLastingPoints = t_vehiclecomputepara.brakelastingpoints;
                vehicleidpara.SteeringZeroStandard = t_vehiclecomputepara.steeringzerostandard;
                vehicleidpara.SteeringLastingPoints = t_vehiclecomputepara.steeringlastingpoints;
                vehicleidpara.ThrottleZeroStandard = t_vehiclecomputepara.throttlezerostandard;
                vehicleidpara.ThrottleLastingPoints = t_vehiclecomputepara.throttlelastingpoints;
                vehicleidpara.Reductiontimesforaccimport = t_vehicleimportpara.importaccreductiontimes;
                vehicleidpara.Reductiontimesforwftimport = t_vehicleimportpara.importwftreductiontimes;
                vehicleidpara.Wheelbaselower = t_vehiclecomputepara.wheelbaselower;
                vehicleidpara.Wheelbaseupper = t_vehiclecomputepara.wheelbaseupper;
                vehicleidpara.vehicleid = t_vehicleimportpara.vehicleid;
                vehicleidpara.analysisaccess = t_vehiclemaster.analysisaccess;
                vehicleidpara.importaccess = t_vehiclemaster.importaccess;
                vehicleidpara.predictaccess = t_vehiclemaster.predictaccess;
                vehicleidpara.gpspointsforanalysis = t_vehiclemaster.displaygpspoints;
                vehicleidpara.samplerate = t_vehiclemaster.samplerate;
               vehicleidpara.importengspd= t_vehicleimportpara.importengspd;
                vehicleidpara.reductiontimesforgps = t_vehicleimportpara.importgpsreductiontimes;
                vehicleidpara.importresultpath = t_vehicleimportpara.importresultpath;
                vehicleidpara.importinputpath = t_vehicleimportpara.importinputpath;
                vehicleidpara.accxbody = String.IsNullOrEmpty(t_vehicleimportpara.accxbody) ? "" : t_vehicleimportpara.accxbody.Replace("_", "").Replace("N", "").Replace(" ", "").ToLower();
                vehicleidpara.accybody = String.IsNullOrEmpty(t_vehicleimportpara.accybody) ? "" : t_vehicleimportpara.accybody.Replace("_", "").Replace("N", "").Replace(" ", "").ToLower();
                vehicleidpara.acczwhllf = String.IsNullOrEmpty(t_vehicleimportpara.acczwhllf) ? "" : t_vehicleimportpara.acczwhllf.Replace("_", "").Replace("N", "").Replace(" ", "").ToLower();
                vehicleidpara.acczwhlrf = String.IsNullOrEmpty(t_vehicleimportpara.acczwhlrf) ? "" : t_vehicleimportpara.acczwhlrf.Replace("_", "").Replace("N", "").Replace(" ", "").ToLower();
                vehicleidpara.acczwhllr = String.IsNullOrEmpty(t_vehicleimportpara.acczwhllr) ? "" : t_vehicleimportpara.acczwhllr.Replace("_", "").Replace("N", "").Replace(" ", "").ToLower();
                vehicleidpara.speedcolumnname = String.IsNullOrEmpty(t_vehicleimportpara.speedcolumnname) ? "" : t_vehicleimportpara.speedcolumnname.Replace("_", "").Replace("N", "").Replace(" ", "").ToLower();
                vehicleidpara.throttlecolumnname = String.IsNullOrEmpty(t_vehicleimportpara.throttlecolumnname) ? "" : t_vehicleimportpara.throttlecolumnname.Replace("_", "").Replace("N", "").Replace(" ", "").ToLower();
                vehicleidpara.brakecolumnname = String.IsNullOrEmpty(t_vehicleimportpara.brakecolumnname) ? "" : t_vehicleimportpara.brakecolumnname.Replace("_", "").Replace("N", "").Replace(" ", "").ToLower();
                vehicleidpara.whlanggrcolumnname = String.IsNullOrEmpty(t_vehicleimportpara.whlanggrcolumnname) ? "" : t_vehicleimportpara.whlanggrcolumnname.Replace("_", "").Replace("N", "").Replace(" ", "").ToLower();
                vehicleidpara.whlangcolumnname = String.IsNullOrEmpty(t_vehicleimportpara.whlangcolumnname) ? "" : t_vehicleimportpara.whlangcolumnname.Replace("_", "").Replace("N", "").Replace(" ", "").ToLower();
                vehicleidpara.enginespeed = String.IsNullOrEmpty(t_vehicleimportpara.enginespeed) ? "" : t_vehicleimportpara.enginespeed.Replace("_", "").Replace("N", "").Replace(" ", "").ToLower();
                return vehicleidpara;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 获取车辆监控参数
        /// </summary>
        /// <param name="vehicleid"></param>
        /// <returns></returns>
        public VehicleWatchPara GetWatchDatafromSql(string vehicleid)
        {
            var t_vehiclemaster = _db.Set<t_vehiclemaster>().AsNoTracking().Where(a => a.vehicleid == vehicleid).Select(a=>new { a.vehicleid,a.samplerate,a.remarks,a.numberpoints }).FirstOrDefault();
            var t_vehiclemonitorpara = _db.Set<t_vehiclemonitorpara>().AsNoTracking().Where(a => a.vehicleid == vehicleid).Select(a => new
            {
                a.monitorinputpath, a.monitorcsvcollumnname, a.monitorreductiontimes, 
                et = a.echart1title + ";" + a.echart2title + ";" + a.echart3title + ";" + a.echart4title + ";" + a.echart5title + ";" + a.echart6title,
                ec= a.echart1channelname + ";" + a.echart2channelname + ";" + a.echart3channelname + ";" + a.echart4channelname + ";" + a.echart5channelname + ";" + a.echart6channelname
            }).FirstOrDefault();

            if (t_vehiclemaster != null && t_vehiclemonitorpara != null )
            {
                VehicleWatchPara vehicleidpara = new VehicleWatchPara();
                vehicleidpara.vehicleid = t_vehiclemaster.vehicleid;
                vehicleidpara.title = t_vehiclemaster.remarks;
                vehicleidpara.intertimes = t_vehiclemaster.numberpoints/t_vehiclemaster.samplerate;
                vehicleidpara.csvcollumnname=t_vehiclemonitorpara.monitorcsvcollumnname;
                vehicleidpara.filewatcherpath = t_vehiclemonitorpara.monitorinputpath;
                vehicleidpara.monitorreducetimes = t_vehiclemonitorpara.monitorreductiontimes;
                vehicleidpara.echarttitle = t_vehiclemonitorpara.et.ToString().Replace(" ", "").Replace("n=", "").Replace("{", "").Replace("}", "").Split(";");
                vehicleidpara.echartdata = t_vehiclemonitorpara.ec.ToString().Replace(" ", "").Replace("n=", "").Replace("{", "").Replace("}", "").Split(";");
                return vehicleidpara;
            }
            else
            {
                return null;
            }
        }
    }
}
