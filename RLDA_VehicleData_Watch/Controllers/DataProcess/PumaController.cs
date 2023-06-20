using IBLL.SH_ADF0979IBLL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using MysqlforDataWatch;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tools.CSVHelper;
using Tools.MyConfig;

namespace RLDA_VehicleData_Watch.Controllers.DataProcess
{
    public class PumaController : Controller
    {
        private readonly ILogger<PumaController> _logger;
       
        private readonly IAnalysisData_PUMA_IBLL _IAnalysisData_PUMA_Service;
        private readonly IMemoryCache _memoryCache;//内存缓存
        private static Dictionary<string, VehicleIDPara> paradict = new Dictionary<string, VehicleIDPara>();

        private static GetVehicleParafromSql _getVehicleParafromSql;
        public PumaController(ILogger<PumaController> logger, IAnalysisData_PUMA_IBLL iAnalysisData_PUMA_Service, IMemoryCache memoryCache, GetVehicleParafromSql getVehicleParafromSql)
        {
            _logger = logger;
            _IAnalysisData_PUMA_Service = iAnalysisData_PUMA_Service;
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
                    else
                    {
                        paradict[vehicleid] = re;
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
                //如果是null也要删除缓存数据，否则新添加的车辆就没法用了，会一直显示数据库未添加此车辆！
                _memoryCache.Remove(vehicleid + "para");
                return "数据库未添加此车辆！";
            }
        }
        [Authorize]
        public IActionResult getPumamileagedata(string vehicle, int page, int limit)
        {
            var textcash = _memoryCache.GetOrCreate<JsonResult>("getPumamileagedata" + vehicle+page+limit,  value =>
            {

                if (paradict[vehicle].analysisaccess == 1)
                {
                    var alllist = _IAnalysisData_PUMA_Service.LoadEntities(a => a.vehicle == vehicle, vehicle).OrderBy(a => a.id);
                    var pumadatalist = alllist.Skip((page - 1) * limit).Take(limit).ToList();
                    var count = alllist.Count();
                    return Json(new { code = 0, count = count, data = pumadatalist, msg = "" });

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

        [Authorize]
        public IActionResult saveAllPumamileagedata(string vehicle)
        {
            var listcash = _memoryCache.GetOrCreate<List<Pumapermileage>>("savePumamileagealldata" + vehicle , value =>
            {

                if (paradict[vehicle].analysisaccess == 1)
                {
                    var alllist = _IAnalysisData_PUMA_Service.LoadEntities(a => a.vehicle == vehicle, vehicle).OrderBy(a => a.id).ToList();
                   
                    return alllist;
                }
                else
                {
                    return null;
                }
            });
            if (listcash != null)
            {
                CSVHelperClass.saveCSVFile(listcash);
                return Json("Yes");
            }
            else
            {
                return Json("No");
            }
        }
    }
}
