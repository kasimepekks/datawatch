using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using MysqlforDataWatch;
using RLDA_VehicleData_Watch.Models;
using System.Diagnostics;
using System.Linq;

namespace RLDA_VehicleData_Watch.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DbContext _db;
        private readonly IMemoryCache _memoryCache;//内存缓存

        public HomeController(ILogger<HomeController> logger, DbContext db, IMemoryCache memoryCache)
        {
            _logger = logger;
            _db = db;
            _memoryCache = memoryCache;

        }

        public IActionResult Chat()
        {
            return View();
        }
        public IActionResult Login()
        {

            return View();
        }

        [Authorize]
        public IActionResult T_VehicleMaster()
        {
            ViewBag.User = HttpContext.Session.GetString("UserID");

            return View();
        }
        [Authorize]
        public IActionResult T_VehicleMonitorPara()
        {
            ViewBag.User = HttpContext.Session.GetString("UserID");

            return View();
        }

        [Authorize]
        public IActionResult T_VehicleImportPara()
        {
            ViewBag.User = HttpContext.Session.GetString("UserID");

            return View();
        }
        [Authorize]
        public IActionResult T_VehicleComputePara()
        {
            ViewBag.User = HttpContext.Session.GetString("UserID");

            return View();
        }
     

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        /// <summary>
        /// 去掉左边导航条
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public IActionResult HomePage()
        {
            ViewBag.User = HttpContext.Session.GetString("UserID");
            return View();
        }

        //给 _Layout_DisplayAll.cshtml用
        [Authorize]
        public IActionResult GetVehicleStateID()
        {
            var vehiclelist = _db.Set<t_vehiclemaster>().Where(a => a.state == 1).Select(b => b.vehicleid);
            return Json(vehiclelist);
        }

  
        /// <summary>
        /// 给前端T_VehicleMonitorPara.cshtml显示车辆副表参数
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [Authorize]
        public IActionResult GetT_VehicleMonitorParaSetup(int page, int limit)
        {
            var vehiclelist = _db.Set<t_vehiclemonitorpara>().OrderBy(a => a.id).Skip((page - 1) * limit).Take(limit);
            var count = vehiclelist.Count();
            return Json(new { code = 0, count = count, data = vehiclelist, msg = "" });
        }

        /// <summary>
        /// 给前端T_VehicleMaster.cshtml显示车辆主表参数
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [Authorize]
        public IActionResult GetT_VehicleMasterSetup(int page, int limit)
        {
            var vehiclelist = _db.Set<t_vehiclemaster>().OrderBy(a => a.id).Skip((page - 1) * limit).Take(limit);
            var count = vehiclelist.Count();
            return Json(new { code = 0, count = count, data = vehiclelist, msg = "" });
        }

        [Authorize]
        public IActionResult GetT_VehicleImportParaSetup(int page, int limit)
        {
            var vehiclelist = _db.Set<t_vehicleimportpara>().OrderBy(a => a.id).Skip((page - 1) * limit).Take(limit);
            var count = vehiclelist.Count();
            return Json(new { code = 0, count = count, data = vehiclelist, msg = "" });
        }

        [Authorize]
        public IActionResult GetT_VehicleComputeParaSetup(int page, int limit)
        {
            var vehiclelist = _db.Set<t_vehiclecomputepara>().OrderBy(a => a.id).Skip((page - 1) * limit).Take(limit);
            var count = vehiclelist.Count();
            return Json(new { code = 0, count = count, data = vehiclelist, msg = "" });
        }
        [Authorize]
        public IActionResult AddorEditSingleT_VehicleMaster(int id, string method, string vehicleidtext, string countrytext, byte statecheck, string remarkstext, string areatext,
          int samplerate, int numberpoints, int displaygpspoints, byte importaccesscheck, byte analysisaccesscheck, byte predictaccesscheck)
        {


            if (method == "edit")
            {
                t_vehiclemaster vehicle = new t_vehiclemaster()
                {
                    id = id,
                    vehicleid = vehicleidtext,
                    country = countrytext,
                    state = statecheck,
                    remarks = remarkstext,
                    area = areatext,
                    samplerate = samplerate,
                    numberpoints = numberpoints,
                    displaygpspoints = displaygpspoints,
                    importaccess = importaccesscheck,
                    analysisaccess = analysisaccesscheck,
                    predictaccess = predictaccesscheck,
                };
                _db.Entry<t_vehiclemaster>(vehicle).State = EntityState.Modified;
                if (_db.SaveChanges() > 0)
                {
                    return Content("编辑成功！");
                }
                else
                {
                    return Content("编辑失败! ");
                }

            }
            else
            {
                t_vehiclemaster vehicle = new t_vehiclemaster()
                {

                    vehicleid = vehicleidtext,
                    country = countrytext,
                    state = statecheck,
                    remarks = remarkstext,
                    area = areatext,
                    samplerate = samplerate,
                    numberpoints = numberpoints,
                    displaygpspoints = displaygpspoints,
                    importaccess = importaccesscheck,
                    analysisaccess = analysisaccesscheck,
                    predictaccess = predictaccesscheck,
                };
                _db.Set<t_vehiclemaster>().Add(vehicle);
                if (_db.SaveChanges() > 0)
                {
                    return Content("添加成功！");
                }
                else
                {
                    return Content("添加失败! ");
                }

            }
        }
        [Authorize]
        public IActionResult AddorEditSingleT_VehicleMonitorPara(int id, string method, string vehicleidtext, string monitorinputpath, string monitorcsvcollumnname, byte monitorreductiontimes, string echart1channelname
            , string echart1title, string echart2channelname, string echart2title, string echart3channelname, string echart3title, string echart4channelname, string echart4title, string echart5channelname, string echart5title,
            string echart6channelname, string echart6title)
        {
            if (method == "edit")
            {
                t_vehiclemonitorpara vehicle = new t_vehiclemonitorpara()
                {
                    id = id,
                    vehicleid = vehicleidtext,
                    monitorinputpath = monitorinputpath,
                    monitorcsvcollumnname = monitorcsvcollumnname,
                    monitorreductiontimes = monitorreductiontimes,
                    echart1channelname = echart1channelname,
                    echart1title = echart1title,
                    echart2channelname = echart2channelname,
                    echart2title = echart2title,
                    echart3channelname = echart3channelname,
                    echart3title = echart3title,
                    echart4channelname = echart4channelname,
                    echart4title = echart4title,
                    echart5channelname = echart5channelname,
                    echart5title = echart5title,
                    echart6channelname = echart6channelname,
                    echart6title = echart6title

                };
                _db.Entry<t_vehiclemonitorpara>(vehicle).State = EntityState.Modified;
                if (_db.SaveChanges() > 0)
                {
                    return Content("编辑成功！");
                }
                else
                {
                    return Content("编辑失败! ");
                }

            }
            else
            {
                t_vehiclemonitorpara vehicle = new t_vehiclemonitorpara()
                {
                    vehicleid = vehicleidtext,
                    monitorinputpath = monitorinputpath,
                    monitorcsvcollumnname = monitorcsvcollumnname,
                    monitorreductiontimes = monitorreductiontimes,
                    echart1channelname = echart1channelname,
                    echart1title = echart1title,
                    echart2channelname = echart2channelname,
                    echart2title = echart2title,
                    echart3channelname = echart3channelname,
                    echart3title = echart3title,
                    echart4channelname = echart4channelname,
                    echart4title = echart4title,
                    echart5channelname = echart5channelname,
                    echart5title = echart5title,
                    echart6channelname = echart6channelname,
                    echart6title = echart6title
                };
                _db.Set<t_vehiclemonitorpara>().Add(vehicle);
                if (_db.SaveChanges() > 0)
                {
                    return Content("添加成功！");
                }
                else
                {
                    return Content("添加失败! ");
                }

            }
        }
        [Authorize]
        public IActionResult AddorEditSingleT_VehicleImportPara(int id, string method, string vehicleidtext, string importinputpath,  string importresultpath, byte statisticimportcheck, byte gpsimportcheck,
            byte speedimportcheck, byte throttleimportcheck, byte brakeimportcheck, byte steeringimportcheck, byte bumpimportcheck, byte wftimportcheck, byte importgpsreductiontimes, byte importaccreductiontimes, byte importwftreductiontimes
            ,string speedcolumnname,string throttlecolumnname,string brakecolumnname,string whlangcolumnname,string whlanggrcolumnname,string acczwhllf,string acczwhlrf
            ,string acczwhllr,string accybody,string accxbody)
        {
            if (method == "edit")
            {
                t_vehicleimportpara vehicle = new t_vehicleimportpara()
                {
                    id = id,
                    vehicleid = vehicleidtext,
                    importinputpath = importinputpath, 
                    importresultpath = importresultpath,
                    importaccreductiontimes = importaccreductiontimes,
                    importgpsreductiontimes = importgpsreductiontimes,
                    importwftreductiontimes = importwftreductiontimes,
                    importgps = gpsimportcheck,
                    importspeed = speedimportcheck,
                    importbrake = brakeimportcheck,
                    importthrottle = throttleimportcheck,
                    importsteering = steeringimportcheck,
                    importstatistic = statisticimportcheck,
                    importimpact = bumpimportcheck,
                    importwft = wftimportcheck,
                    speedcolumnname= speedcolumnname,
                    throttlecolumnname = throttlecolumnname,
                    brakecolumnname = brakecolumnname,
                    whlangcolumnname = whlangcolumnname,
                    acczwhllf = acczwhllf,
                    acczwhlrf = acczwhlrf,
                    accybody = accybody,
                    accxbody = accxbody,
                    acczwhllr = acczwhllr,
                    whlanggrcolumnname = whlanggrcolumnname
                };
                _db.Entry<t_vehicleimportpara>(vehicle).State = EntityState.Modified;
                if (_db.SaveChanges() > 0)
                {
                    return Content("编辑成功！");
                }
                else
                {
                    return Content("编辑失败! ");
                }

            }
            else
            {
                t_vehicleimportpara vehicle = new t_vehicleimportpara()
                {
                    vehicleid = vehicleidtext,
                    importinputpath = importinputpath,
                   
                    importresultpath = importresultpath,
                    importaccreductiontimes = importaccreductiontimes,
                    importgpsreductiontimes = importgpsreductiontimes,
                    importwftreductiontimes = importwftreductiontimes,
                    importgps = gpsimportcheck,
                    importspeed = speedimportcheck,
                    importbrake = brakeimportcheck,
                    importthrottle = throttleimportcheck,
                    importsteering = steeringimportcheck,
                    importstatistic = statisticimportcheck,
                    importimpact = bumpimportcheck,
                    importwft = wftimportcheck,
                    speedcolumnname = speedcolumnname,
                    throttlecolumnname = throttlecolumnname,
                    brakecolumnname = brakecolumnname,
                    whlangcolumnname = whlangcolumnname,
                    acczwhllf = acczwhllf,
                    acczwhlrf = acczwhlrf,
                    accybody = accybody,
                    accxbody = accxbody,
                    acczwhllr = acczwhllr,
                    whlanggrcolumnname = whlanggrcolumnname
                };
                _db.Set<t_vehicleimportpara>().Add(vehicle);
                if (_db.SaveChanges() > 0)
                {
                    return Content("添加成功！");
                }
                else
                {
                    return Content("添加失败! ");
                }

            }
        }
        [Authorize]
        public IActionResult AddorEditT_VehicleComputePara(int id, string method, string vehicleidtext,float wheelbaselower, float wheelbaseupper, byte bumpzerostandard, 
          byte bumpmaxspeed, byte bumptimegap, byte accvaluegap, byte acctimegap, byte brakezerostandard, byte brakelastingpoints, byte steeringzerostandard, byte steeringlastingpoints
           , byte throttlezerostandard, byte throttlelastingpoints)
        {
            if (method == "edit")
            {
                t_vehiclecomputepara vehicle = new t_vehiclecomputepara()
                {
                    id = id,
                    vehicleid = vehicleidtext,
                    wheelbaselower = wheelbaselower,
                    wheelbaseupper = wheelbaseupper,
                    bumpzerostandard = bumpzerostandard,
                    bumpmaxspeed = bumpmaxspeed,
                    bumptimegap = bumptimegap,
                    accvaluegap = accvaluegap,
                    acctimegap = acctimegap,
                    brakezerostandard = brakezerostandard,
                    brakelastingpoints = brakelastingpoints,
                    steeringzerostandard = steeringzerostandard,
                    steeringlastingpoints = steeringlastingpoints,
                    throttlezerostandard = throttlezerostandard,
                    throttlelastingpoints = throttlelastingpoints
             
                };
                _db.Entry<t_vehiclecomputepara>(vehicle).State = EntityState.Modified;
                if (_db.SaveChanges() > 0)
                {
                    return Content("编辑成功！");
                }
                else
                {
                    return Content("编辑失败! ");
                }

            }
            else
            {
                t_vehiclecomputepara vehicle = new t_vehiclecomputepara()
                {

                    vehicleid = vehicleidtext,
                    wheelbaselower = wheelbaselower,
                    wheelbaseupper = wheelbaseupper,
                    bumpzerostandard = bumpzerostandard,
                    bumpmaxspeed = bumpmaxspeed,
                    bumptimegap = bumptimegap,
                    accvaluegap = accvaluegap,
                    acctimegap = acctimegap,
                    brakezerostandard = brakezerostandard,
                    brakelastingpoints = brakelastingpoints,
                    steeringzerostandard = steeringzerostandard,
                    steeringlastingpoints = steeringlastingpoints,
                    throttlezerostandard = throttlezerostandard,
                    throttlelastingpoints = throttlelastingpoints
                };
                _db.Set<t_vehiclecomputepara>().Add(vehicle);
                if (_db.SaveChanges() > 0)
                {
                    return Content("添加成功！");
                }
                else
                {
                    return Content("添加失败! ");
                }

            }
        }

        /// <summary>
        /// 删除t_vehiclemaster表里对应的数据，此操作会删除其余表里相对应的数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        public IActionResult DeleteSingleT_VehicleMaster(int id)
        {
            var vehicle = _db.Set<t_vehiclemaster>().Where(a => a.id == id).FirstOrDefault();
            var v_monitor = _db.Set<t_vehiclemonitorpara>().Where(a => a.vehicleid == vehicle.vehicleid).FirstOrDefault();
            var v_import = _db.Set<t_vehicleimportpara>().Where(a => a.vehicleid == vehicle.vehicleid).FirstOrDefault();
            var v_compute = _db.Set<t_vehiclecomputepara>().Where(a => a.vehicleid == vehicle.vehicleid).FirstOrDefault();
            _db.Entry<t_vehiclemaster>(vehicle).State = EntityState.Deleted;
            _db.Entry<t_vehiclemonitorpara>(v_monitor).State = EntityState.Deleted;
            _db.Entry<t_vehicleimportpara>(v_import).State = EntityState.Deleted;
            _db.Entry<t_vehiclecomputepara>(v_compute).State = EntityState.Deleted;
            if (_db.SaveChanges() > 0)
            {
                return Content("删除成功！");
            }
            else
            {
                return Content("删除失败! ");
            }
        }
        [Authorize]
        public IActionResult DeleteSingleT_VehicleMonitorPara(int id)
        {
            var vehicle = _db.Set<t_vehiclemonitorpara>().Where(a => a.id == id).FirstOrDefault();
            _db.Entry<t_vehiclemonitorpara>(vehicle).State = EntityState.Deleted;

            if (_db.SaveChanges() > 0)
            {
                return Content("删除成功！");
            }
            else
            {
                return Content("删除失败! ");
            }
        }
        [Authorize]
        public IActionResult DeleteSingleT_VehicleImportPara(int id)
        {
            var vehicle = _db.Set<t_vehicleimportpara>().Where(a => a.id == id).FirstOrDefault();
            _db.Entry<t_vehicleimportpara>(vehicle).State = EntityState.Deleted;

            if (_db.SaveChanges() > 0)
            {
                return Content("删除成功！");
            }
            else
            {
                return Content("删除失败! ");
            }
        }
        [Authorize]
        public IActionResult DeleteSingleT_VehicleComputePara(int id)
        {
            var vehicle = _db.Set<t_vehiclecomputepara>().Where(a => a.id == id).FirstOrDefault();
            _db.Entry<t_vehiclecomputepara>(vehicle).State = EntityState.Deleted;

            if (_db.SaveChanges() > 0)
            {
                return Content("删除成功！");
            }
            else
            {
                return Content("删除失败! ");
            }
        }
      
        /// <summary>
        /// 删除监控的参数内存
        /// </summary>
        /// <param name="vehicleidtext"></param>
        /// <returns></returns>
        [Authorize]
        public IActionResult DeleteMonitorMemory(string vehicleidtext)
        {
            _memoryCache.Remove(vehicleidtext + "watchpara");
            _logger.LogInformation(vehicleidtext + "监控参数原内存数据已删除");
            return Ok();
        }
     
        [Authorize]
        public IActionResult DeleteOtherMemory(string vehicleidtext)
        {
            _memoryCache.Remove(vehicleidtext + "para");
            _logger.LogInformation(vehicleidtext + "其他参数原内存数据已删除");
            return Ok();
        }
    }
}
