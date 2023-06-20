﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IBLL.SH_ADF0979IBLL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MysqlforDataWatch;
using Tools;
using Tools.AddDistance;
using Tools.MyConfig;

namespace RLDA_VehicleData_Watch.Controllers
{
    /// <summary>
    /// 此控制器主要功能为分析统计数据，在前端选择日期时间进行计算
    /// </summary>
    public class AnalysisController : Controller
    {
        private readonly DbContext _db;
        private readonly ILogger<AnalysisController> _logger;
        private readonly IAnalysisData_WFT_IBLL _IAnalysisData_WFT_Service;
        private readonly IAnalysisData_ACC_IBLL _IAnalysisData_ACC_Service;
        private readonly IBrakeDistribution_IBLL _IBrakeDistribution_Service;
        private readonly IBumpDistribution_IBLL _IBumpDistribution_Service;

        private readonly ISpeedDistribution_ACC_IBLL _ISpeedDistribution_ACC_Service;
        private readonly IThrottleDistribution_IBLL _IThrottleDistribution_Service;
        private readonly ISteeringDistribution_IBLL _ISteeringDistribution_Service;
        private readonly IEngineSpeedDisDistribution_IBLL _IEngineSpeedDisDistribution_IBLL;
        private readonly IEngineSpeedTimeDistribution_IBLL _IEngineSpeedTimeDistribution_IBLL;
        
        private readonly IMemoryCache _memoryCache;//内存缓存

        private readonly IGPSRecord_IBLL _IGPSRecord_Service;
        private static Dictionary<string, VehicleIDPara> paradict = new Dictionary<string, VehicleIDPara>();//存储每个车的里程
       
        private  static GetVehicleParafromSql _getVehicleParafromSql;

        
        public AnalysisController(IAnalysisData_WFT_IBLL IAnalysisData_WFT_Service, IAnalysisData_ACC_IBLL IAnalysisData_ACC_Service, ISpeedDistribution_ACC_IBLL ISpeedDistribution_ACC_Service,
            IBrakeDistribution_IBLL IBrakeDistribution_Service, IBumpDistribution_IBLL IBumpDistribution_Service, 
            ISteeringDistribution_IBLL ISteeringDistribution_Service, IGPSRecord_IBLL IGPSRecord_Service, 
            IEngineSpeedDisDistribution_IBLL IEngineSpeedDisDistribution_IBLL, IEngineSpeedTimeDistribution_IBLL IEngineSpeedTimeDistribution_IBLL,
            IThrottleDistribution_IBLL IThrottleDistribution_Service, ILogger<AnalysisController> logger,IMemoryCache memoryCache, DbContext db, GetVehicleParafromSql getVehicleParafromSql)
        {
            _IAnalysisData_WFT_Service = IAnalysisData_WFT_Service;
            _IAnalysisData_ACC_Service = IAnalysisData_ACC_Service;
            _ISpeedDistribution_ACC_Service = ISpeedDistribution_ACC_Service;
            _IBrakeDistribution_Service = IBrakeDistribution_Service;
            _IBumpDistribution_Service = IBumpDistribution_Service;
            _IThrottleDistribution_Service = IThrottleDistribution_Service;
            _ISteeringDistribution_Service = ISteeringDistribution_Service;
            _IEngineSpeedDisDistribution_IBLL = IEngineSpeedDisDistribution_IBLL;
            _IEngineSpeedTimeDistribution_IBLL = IEngineSpeedTimeDistribution_IBLL;
            _IGPSRecord_Service = IGPSRecord_Service;
            this._logger = logger;
            _memoryCache = memoryCache;
            _db = db;
            _getVehicleParafromSql = getVehicleParafromSql;
            
        }
      
        public string GetVehicleParafromSql(string vehicleid)
        {
            //先用内存缓存来存储车辆参数
            var re= _memoryCache.GetOrCreate<VehicleIDPara>(vehicleid + "para", value =>
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
                if (re.analysisaccess == 1)
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
        /// <summary>
        /// 根据所选的时间范围来传给前端速度的总体分布(利用内存缓存来优化，注意第一次不能查询数据库的时候不能asnotracking(),且查询结果必须先toList(),否则会报数据库错误
        /// </summary>
        /// <param name="startdate"></param>
        /// <param name="enddate"></param>
        /// <returns></returns>
        public async Task<IActionResult> SpeedAnalysis(string startdate,string enddate, string vehicleid)
        {
            
                var speedcash = await _memoryCache.GetOrCreateAsync<IQueryable>("SpeedAnalysis" + startdate + enddate + vehicleid, async value =>
                {
                    if (paradict[vehicleid].analysisaccess == 1)
                    {
                            var sd = Convert.ToDateTime(startdate);
                            var ed = Convert.ToDateTime(enddate);
                            var SpeedList = await _ISpeedDistribution_ACC_Service.LoadSpeedDistribution(sd, ed, vehicleid);
                            return SpeedList;
                    }
                    else
                    {
                        return null;
                    }

                });
                if (speedcash != null)
                {
                    return Json(speedcash);
                }
                else
                {
                    return Json("No");
                }
        }

        public async Task<IActionResult> engspdDisAnalysis(string startdate, string enddate, string vehicleid)
        {

            var engspdcash = await _memoryCache.GetOrCreateAsync<IQueryable>("engspdDisAnalysis" + startdate + enddate + vehicleid, async value =>
            {
                if (paradict[vehicleid].analysisaccess == 1)
                {
                    var sd = Convert.ToDateTime(startdate);
                    var ed = Convert.ToDateTime(enddate);
                    var SpeedList = await _IEngineSpeedDisDistribution_IBLL.LoadEngspdDisDistribution(sd, ed, vehicleid);
                    return SpeedList;
                }
                else
                {
                    return null;
                }

            });
            if (engspdcash != null)
            {
                return Json(engspdcash);
            }
            else
            {
                return Json("No");
            }
        }
        public async Task<IActionResult> engspdTimeAnalysis(string startdate, string enddate, string vehicleid)
        {

            var engspdtimecash = await _memoryCache.GetOrCreateAsync<IQueryable>("engspdTimeAnalysis" + startdate + enddate + vehicleid, async value =>
            {
                if (paradict[vehicleid].analysisaccess == 1)
                {
                    var sd = Convert.ToDateTime(startdate);
                    var ed = Convert.ToDateTime(enddate);
                    var EngspdTimeList = await _IEngineSpeedTimeDistribution_IBLL.LoadEngspdTimeDistribution(sd, ed, vehicleid);
                    return EngspdTimeList;
                }
                else
                {
                    return null;
                }

            });
            if (engspdtimecash != null)
            {
                return Json(engspdtimecash);
            }
            else
            {
                return Json("No");
            }
        }
        /// <summary>
        /// 根据所选的时间范围来传给前端每天的里程分布
        /// </summary>
        /// <param name="startdate"></param>
        /// <param name="enddate"></param>
        /// <returns></returns>
        public async Task<IActionResult> SpeedPerDayAnalysis(string startdate, string enddate, string vehicleid)
        {
            var speedcash = await _memoryCache.GetOrCreateAsync<JsonResult>("SpeedPerDayAnalysis" + startdate + enddate + vehicleid, async value =>
            {
                if (paradict[vehicleid].analysisaccess == 1)
                {
                  
                        var sd = Convert.ToDateTime(startdate);
                        var ed = Convert.ToDateTime(enddate);
                        var SpeedPerDayList = await _ISpeedDistribution_ACC_Service.LoadSpeedDistributionperday(sd, ed, vehicleid);
                        return Json(SpeedPerDayList);
                }
                else
                {
                    return null;
                }

            });
            if (speedcash != null)
            {
                return speedcash;
            }
            else
            {
                return Json("No");
            }





        }
        /// <summary>
        /// 根据所选的时间范围来传给前端每时的里程分布
        /// </summary>
        /// <param name="startdate"></param>
        /// <param name="enddate"></param>
        /// <returns></returns>
        public async Task<IActionResult> SpeedPerHourAnalysis(string startdate, string enddate, string vehicleid)
        {
            var speedcash = await _memoryCache.GetOrCreateAsync<JsonResult>("SpeedPerHourAnalysis" + startdate + enddate + vehicleid, async value => {

                if (paradict[vehicleid].analysisaccess == 1)
                {
                   
                        var sd = Convert.ToDateTime(startdate);
                        var ed = Convert.ToDateTime(enddate);
                        var SpeedPerHourList = await _ISpeedDistribution_ACC_Service.LoadSpeedDistributionperhour(sd, ed, vehicleid);
                        return Json(SpeedPerHourList);
                    
                    
                }
                else
                {
                    return null;
                }


            });

            if (speedcash != null)
            {
                return speedcash;
            }
            else
            {
                return Json("No");
            }




        }

        public async Task<IActionResult> WFTDamageAnalysis(string startdate, string enddate, string vehicleid)
        {
            var wftcash = await _memoryCache.GetOrCreateAsync<JsonResult>("WFTDamageAnalysis" + startdate + enddate + vehicleid, async value =>
            {

                if (paradict[vehicleid].analysisaccess == 1)
                {
                   
                        var sd = Convert.ToDateTime(startdate);
                        var ed = Convert.ToDateTime(enddate);
                        var damagelist = await _IAnalysisData_WFT_Service.LoadWFTDamage(sd, ed, vehicleid);
                        return Json(damagelist);
                }
                else
                {
                    return null;
                }
            });

            if (wftcash != null)
            {
                return wftcash;
            }
            else
            {
                return Json("No");
            }


        }

        public async Task<IActionResult> ACCandDisAnalysis(string startdate, string enddate, string vehicleid)
        {
            var acccash = await _memoryCache.GetOrCreateAsync<JsonResult>("ACCandDisAnalysis" + startdate + enddate + vehicleid, async value =>
            {

                if (paradict[vehicleid].analysisaccess == 1)
                {
                    var sd = Convert.ToDateTime(startdate);
                    var ed = Convert.ToDateTime(enddate);
                    var accanddislist = await _IAnalysisData_ACC_Service.LoadACCandDisData(sd, ed, vehicleid);
                    return Json(accanddislist);
                }
                else
                {
                    return null;
                }
            });

            if (acccash != null)
            {
                return acccash;
            }
            else
            {
                return Json("No");
            }


        }
        public async Task<IActionResult> BrakeDistributionAnalysis(string startdate, string enddate,string vehicleid)
        {
            var brakecash = await _memoryCache.GetOrCreateAsync<JsonResult>("BrakeDistributionAnalysis" + startdate + enddate + vehicleid, async value =>
            {

                if (paradict[vehicleid].analysisaccess == 1)
                {
                    
                        var sd = Convert.ToDateTime(startdate);
                        var ed = Convert.ToDateTime(enddate);
                        var brakelist = await _IBrakeDistribution_Service.LoadBrakeDistribution(sd, ed, vehicleid);
                        return Json(brakelist);
                    
                   
                }
                else
                {
                    return null;
                }
            });
            if (brakecash != null)
            {
                return brakecash;
            }
            else
            {
                return Json("No");
            }



        }
        public async Task<IActionResult> BrakeCountAnalysis(string startdate, string enddate, string vehicleid)
        {
            try
            {
                if (paradict[vehicleid].analysisaccess == 1)
                {
                    
                        var sd = Convert.ToDateTime(startdate);
                        var ed = Convert.ToDateTime(enddate);
                        var brakecount = await _IBrakeDistribution_Service.GetBrakeCount(sd, ed, vehicleid);
                        return Json(brakecount);
                   
                }
                else
                {
                    return Json("No");
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation("AnalysisController中BrakeCountAnalysis方法出现问题：" + ex.Message + "出现时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                return Json("No");
            }
           

         
        }


        public async Task<IActionResult> BumpDistributionAnalysis(string startdate, string enddate, string vehicleid)
        {
            var bumpcash = await _memoryCache.GetOrCreateAsync<JsonResult>("BumpDistributionAnalysis" + startdate + enddate + vehicleid, async value =>
            {

                if (paradict[vehicleid].analysisaccess == 1)
                {
                   
                        var sd = Convert.ToDateTime(startdate);
                        var ed = Convert.ToDateTime(enddate);
                        var bumplist = await _IBumpDistribution_Service.LoadBumpDistribution(sd, ed, vehicleid);
                        return Json(bumplist);
                    
                }
                else
                {
                    return null;
                }
            });
            if (bumpcash != null)
            {
                return bumpcash;
            }
            else
            {
                return Json("No");
            }




        }
        public async Task<IActionResult> BumpCountAnalysis(string startdate, string enddate, string vehicleid)
        {
            try
            {
                if (paradict[vehicleid].analysisaccess == 1)
                {
                    
                        var sd = Convert.ToDateTime(startdate);
                        var ed = Convert.ToDateTime(enddate);
                        var bumpcount = await _IBumpDistribution_Service.GetBumpCount(sd, ed, vehicleid);
                        return Json(bumpcount);
                    
                }
                else
                {
                    return Json("No");
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation("AnalysisController中BumpCountAnalysis方法出现问题：" + ex.Message + "出现时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                return Json("No");
            }

           

        }

        public async Task<IActionResult> ThrottleAnalysis(string startdate, string enddate, string vehicleid)
        {
            var throttlecash = await _memoryCache.GetOrCreateAsync<JsonResult>("ThrottleAnalysis" + startdate + enddate + vehicleid, async value =>
            {

                if (paradict[vehicleid].analysisaccess == 1)
                {
                   
                        var sd = Convert.ToDateTime(startdate);
                        var ed = Convert.ToDateTime(enddate);
                        var ThrottleList = await _IThrottleDistribution_Service.LoadThrottleDistribution(sd, ed, vehicleid);

                        var json = Json(ThrottleList);//这里speedlist是iquerable匿名类型，不知为什么无法传给前端layui表格，所以只能直接传json数据
                        return json;
                  
                }
                else
                {
                    return null;
                }
            });
            if (throttlecash != null)
            {
                return throttlecash;
            }
            else
            {
                return Json("No");
            }

        }

        public async Task<IActionResult> SteeringAnalysis(string startdate, string enddate, string vehicleid)
        {
            var steeringcash = await _memoryCache.GetOrCreateAsync<JsonResult>("SteeringAnalysis" + startdate + enddate + vehicleid, async value =>
            {

                if (paradict[vehicleid].analysisaccess == 1)
                {
                   
                        var sd = Convert.ToDateTime(startdate);
                        var ed = Convert.ToDateTime(enddate);
                        var SteeringList = await _ISteeringDistribution_Service.LoadSteeringDistribution(sd, ed, vehicleid);
                        var json = Json(SteeringList);//这里speedlist是iquerable匿名类型，不知为什么无法传给前端layui表格，所以只能直接传json数据
                        return json;
                   
                }
                else
                {
                    return null;
                }
            });
            if (steeringcash != null)
            {
                return steeringcash;
            }
            else
            {
                return Json("No");
            }

        }

        public async Task<IActionResult> GPSAnalysis(string startdate, string enddate, string vehicleid)
        {
            var GPScash = await _memoryCache.GetOrCreateAsync<JsonResult>("GPSAnalysis" + startdate + enddate + vehicleid, async value =>
            {

                if (paradict[vehicleid].analysisaccess == 1)
                {
                  
                        var sd = Convert.ToDateTime(startdate);
                        var ed = Convert.ToDateTime(enddate);
                        //这里的reducetimeforgps是appsetting里的

                        var GPSList = await _IGPSRecord_Service.LoadGPSRecord(sd, ed, vehicleid, paradict[vehicleid].gpspointsforanalysis);
                        //var lat = GPSList.Select(a => a.Lat);
                        //var lon= GPSList.Select(a => a.Lon);
                        var json = Json(GPSList);//这里speedlist是iquerable匿名类型，不知为什么无法传给前端layui表格，所以只能直接传json数据
                        return json;
                  
                }
                else
                {
                    return null;
                }
            });
            if (GPScash != null)
            {
                return GPScash;
            }
            else
            {
                return Json("No");
            }

        }

        public async Task<IActionResult> TextperDayAnalysis(string startdate, string enddate, string vehicleid)
        {
            var textcash = await _memoryCache.GetOrCreateAsync<JsonResult>("TextperDayAnalysis" + startdate + enddate + vehicleid, async value =>
            {

                if (paradict[vehicleid].analysisaccess == 1)
                {
                   
                        var sd = Convert.ToDateTime(startdate);
                        var ed = Convert.ToDateTime(enddate);
                        var textList = await _ISpeedDistribution_ACC_Service.LoadTextRecord(sd, ed, vehicleid);

                        var json = Json(textList);//这里speedlist是iquerable匿名类型，不知为什么无法传给前端layui表格，所以只能直接传json数据
                        return json;
                   
                }
                else
                {
                    return null;
                }
            });
            if (textcash != null)
            {
                return textcash;
            }
            else
            {
                return Json("No");
            }

        }

      

    }
}
