using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IBLL.SH_ADF0979IBLL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MysqlforDataWatch;
using Tools.MyConfig;

namespace RLDA_VehicleData_Watch.Controllers.ADF0979
{
    public class PredictionController : Controller
    {
        private readonly ILogger<PredictionController> _logger;
        private IAnalysisData_WFT_IBLL _IAnalysisData_WFT_Service;
        private readonly DbContext _db;
       
        private readonly IMemoryCache _memoryCache;//内存缓存
        private static GetVehicleParafromSql _getVehicleParafromSql;
        private static Dictionary<string, VehicleIDPara> paradict = new Dictionary<string, VehicleIDPara>();//存储每个车的里程
        
        public PredictionController(IAnalysisData_WFT_IBLL IAnalysisData_WFT_Service,  ILogger<PredictionController> logger,IMemoryCache memoryCache, DbContext db, GetVehicleParafromSql getVehicleParafromSql)
        {
            
            _IAnalysisData_WFT_Service = IAnalysisData_WFT_Service;
            
            this._logger = logger;
            _db = db;
           
            _memoryCache = memoryCache;
            _getVehicleParafromSql = getVehicleParafromSql;
        }
        public string GetVehicleParafromSql(string vehicleid)
        {
            //先用内存缓存来存储车辆参数
            var re = _memoryCache.GetOrCreate<VehicleIDPara>(vehicleid + "para", value =>
            {
                return _getVehicleParafromSql.GetDatafromSql(vehicleid); ;
            });

            if (re != null)
            {
                //初始化车辆参数字典，由于字典是静态字段，不同实例可以共享
                if (paradict.Count != 0)
                {
                    if (!paradict.ContainsKey(vehicleid))
                    {
                        paradict.Add(vehicleid, re);
                    }
                }
                else
                {
                    paradict.Add(vehicleid, re);
                }
                //_vehicleIDPara = re;
                if (re.predictaccess == 1)
                {
                    return "yes";
                }
                else
                {
                    return "此车辆没有分析权限！请先添加！";
                }
            }
            else
            {
                return "数据库未添加此车辆！";
            }
        }
        //public string GetVehiclePara(string vehicleid)
        //{
        //    var t_vehiclemaster = _db.Set<t_vehiclemaster>().AsNoTracking().Where(a => a.vehicleid == vehicleid).FirstOrDefault();
        //    var t_vehicleimportpara = _db.Set<t_vehicleimportpara>().AsNoTracking().Where(a => a.vehicleid == vehicleid).FirstOrDefault();
        //    var t_vehiclecomputepara = _db.Set<t_vehiclecomputepara>().AsNoTracking().Where(a => a.vehicleid == vehicleid).FirstOrDefault();
        //    if (t_vehicleimportpara != null && t_vehiclecomputepara != null && t_vehiclemaster != null)
        //    {
        //        _vehicleIDPara = MyConfigforVehicleID.GetVehicleConfigurationfromsql(ref _vehicleIDPara, t_vehiclemaster, t_vehicleimportpara, t_vehiclecomputepara);
        //        if (_vehicleIDPara.predictaccess == 1)
        //        {
        //            return "yes";
        //        }
        //        else
        //        {
        //            return "此车辆没有预测权限！请先添加！";
        //        }
        //    }
        //    else
        //    {
        //        return "数据库未添加此车辆！";
        //    }
        //}
        /// <summary>
        /// 预测损伤达到GD水平的时间
        /// </summary>
        /// <param name="startdate"></param>
        /// <param name="enddate"></param>
        /// <returns></returns>
        public async Task<IActionResult> WFTDamageCumulation(string startdate, string enddate,string vehicleid)
        {
            var cash = await _memoryCache.GetOrCreateAsync<JsonResult>("WFTDamageCumulation" + startdate + enddate + vehicleid, async value =>
            {

               
                    if (paradict[vehicleid].predictaccess==1)
                    {
                        var sd = Convert.ToDateTime(startdate);
                        var ed = Convert.ToDateTime(enddate);
                        var damagelist = await _IAnalysisData_WFT_Service.LoadWFTDamageCumulation(sd, ed, vehicleid);
                        return Json(damagelist);
                    }
                    else
                    {
                        return null;
                    }
                
              
            });
            if (cash != null)
            {
                return cash;
            }
            else
            {
                return Json("No");
            }

        }
    }
}
