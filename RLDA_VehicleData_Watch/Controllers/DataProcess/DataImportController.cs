using IBLL.SH_ADF0979IBLL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using MysqlforDataWatch;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Tools;
using Tools.DateTimeOperation;
using Tools.FileOperation;
using Tools.ListOperation.StatisticAccListOperation;
using Tools.ListOperation.WFTListOperation;
using Tools.MyConfig;

namespace RLDA_VehicleData_Watch.Controllers.DataProcess
{
    public class DataImportController : Controller
    {
        private readonly ILogger<DataImportController> _logger;
        private readonly IAnalysisData_ACC_IBLL _IAnalysisData_ACC_Service;
        private readonly IAnalysisData_WFT_IBLL _IAnalysisData_WFT_Service;
        private readonly IBrakeDistribution_IBLL _IBrakeDistribution_IBLL;
        private readonly ISpeedDistribution_ACC_IBLL _ISpeedDistribution_ACC_IBLL;
        private readonly ISteeringDistribution_IBLL _ISteeringDistribution_IBLL;
        private readonly IThrottleDistribution_IBLL _IThrottleDistribution_IBLL;
        private readonly IGPSRecord_IBLL _IGPSRecord_IBLL;
        private readonly IBumpDistribution_IBLL _IBumpDistribution_IBLL;
        private readonly IMemoryCache _memoryCache;//内存缓存
        private readonly DbContext _db;
        private static GetVehicleParafromSql _getVehicleParafromSql;
        private static Dictionary<string, VehicleIDPara> paradict = new Dictionary<string, VehicleIDPara>();//存储每个车的里程
        private static string inputpath;
        private static string resultpath;
        
       

        public DataImportController(IAnalysisData_ACC_IBLL IAnalysisData_ACC_Service, IAnalysisData_WFT_IBLL IAnalysisData_WFT_Service,
            IBrakeDistribution_IBLL brakeDistribution_IBLL , ISpeedDistribution_ACC_IBLL speedDistribution_ACC_IBLL , ISteeringDistribution_IBLL steeringDistribution_IBLL,
           IThrottleDistribution_IBLL throttleDistribution_IBLL , IGPSRecord_IBLL gPSRecord_IBLL, IBumpDistribution_IBLL bumpDistribution_IBLL, ILogger<DataImportController> logger, IMemoryCache memoryCache, DbContext db, GetVehicleParafromSql getVehicleParafromSql)
        {
            _IAnalysisData_ACC_Service = IAnalysisData_ACC_Service;
            _IAnalysisData_WFT_Service = IAnalysisData_WFT_Service;
            _IBrakeDistribution_IBLL = brakeDistribution_IBLL;
            _ISpeedDistribution_ACC_IBLL=speedDistribution_ACC_IBLL;
            _ISteeringDistribution_IBLL=steeringDistribution_IBLL;
            _IThrottleDistribution_IBLL=throttleDistribution_IBLL;
            _IGPSRecord_IBLL=gPSRecord_IBLL;
            _IBumpDistribution_IBLL=bumpDistribution_IBLL;
            _logger = logger;
            _memoryCache= memoryCache;
            _db = db;
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
                        paradict[vehicleid]=re;
                    }
                }
                else
                {
                    paradict.Add(vehicleid, re);
                }
                //_vehicleIDPara = re;
                if (re.importaccess == 1)
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
        //public string GetVehiclePara(string vehicleid)
        //{
        //    var t_vehiclemaster = _db.Set<t_vehiclemaster>().AsNoTracking().Where(a => a.vehicleid == vehicleid).FirstOrDefault();

        //    var t_vehicleimportpara = _db.Set<t_vehicleimportpara>().AsNoTracking().Where(a => a.vehicleid == vehicleid).FirstOrDefault();
        //    var t_vehiclecomputepara = _db.Set<t_vehiclecomputepara>().AsNoTracking().Where(a => a.vehicleid == vehicleid).FirstOrDefault();
        //    if (t_vehicleimportpara != null && t_vehiclecomputepara != null && t_vehiclemaster != null)
        //    {
        //        _vehicleIDPara = MyConfigforVehicleID.GetVehicleConfigurationfromsql(ref _vehicleIDPara, t_vehiclemaster, t_vehicleimportpara, t_vehiclecomputepara);
        //        if (_vehicleIDPara.importaccess == 1)
        //        {
        //            return "yes";
        //        }
        //        else
        //        {
        //            return "此车辆没有导入权限！请先添加！";
        //        }
        //    }
        //    else
        //    {
        //        return "数据库未添加此车辆！";
        //    }
        //}
        public async Task<string> DataImport(string startdate, string enddate, string vehicleid)
        {
            if (paradict[vehicleid].importaccess == 1)//判断是否允许导入数据
            {
                
                if (startdate != null && enddate != null)
                {
                    int span = DateTimeOperation.DateDiff(startdate, enddate);
                    inputpath = paradict[vehicleid].importinputpath;
                    resultpath = paradict[vehicleid].importresultpath;
                    string importresult = "";
                    try
                    {
                        var para = paradict[vehicleid];
                        for (int i = 0; i < span + 1; i++)
                        {
                            string date = FileOperator.DatetoName(Convert.ToDateTime(startdate).AddDays(i).ToString("yyyy-MM-dd")).Substring(5);//当前计算的日期文件夹名称
                            string inputfiletimeinfo = Path.Combine(inputpath, date);
                            string outputfiletimeinfo = Path.Combine(resultpath, date);
                            //判断input子文件夹里是否有导入的flag
                            if (!FileOperator.DataImportCompleteFlag(inputfiletimeinfo))
                            {
                                //判断是否有数据上传完整
                                if (FileOperator.DataTransferCompleteFlag(inputfiletimeinfo, date))
                                {
                                    _logger.LogInformation(date + "的input数据开始手动导入" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                    FileInfo[] filelist = FileOperator.Isfileexist(inputfiletimeinfo);
                                    if (filelist != null && filelist.Length > 0)
                                    {
                                        List<SatictisAnalysisdataAcc> stalist=new List<SatictisAnalysisdataAcc>();
                                        List<Brakerecognition> brklist = new List<Brakerecognition>();
                                        List<Bumprecognition> bmplist = new List<Bumprecognition>();
                                        List<Gpsrecord> gpslist = new List<Gpsrecord>();
                                        List<Speeddistribution> spdlist = new List<Speeddistribution>();
                                        List<Streeringrecognition> strlist = new List<Streeringrecognition>();
                                        List<Throttlerecognition> thrlist = new List<Throttlerecognition>();
                                        bool canbeimported = true;
                                        //先判断是否要导入gps，如果要导入，先去数据库创建此车辆的gps表，如数据库有此车的gps表，则不创建（可以自动创建了，不需要再去数据库创建）
                                        if (paradict[vehicleid].GPSImport == 1)
                                        {
                                            await _IGPSRecord_IBLL.CreateTable(vehicleid);
                                        }
                                        Console.WriteLine("开始读取数据"+ DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                        foreach (var file in filelist)
                                        {
                                            //读取单个csv文件的数据并存储到t
                                            var t =await CSVFileImport.LoadCSVData(file, paradict[vehicleid]);
                                            var infostatisticresult = _IAnalysisData_ACC_Service.JudgeandMergeStatisticDataperHalfHour(file, t, ref stalist, vehicleid, in para);
                                            var infogpsresult =  _IGPSRecord_IBLL.JudgeandMergeGPSDataperHalfHour(file, t, ref gpslist, vehicleid, in para);
                                            var infobrkresult = _IBrakeDistribution_IBLL.JudgeandMergeBrkDataperHalfHour(file, t, ref brklist, vehicleid, in para);
                                            var infobmpresult = _IBumpDistribution_IBLL.JudgeandMergeBmpDataperHalfHour(file, t, ref bmplist, vehicleid, in para);
                                            var infospdresult = _ISpeedDistribution_ACC_IBLL.JudgeandMergeSpdDataperHalfHour(file, t, ref spdlist, vehicleid, in para);
                                            var infostrresult = _ISteeringDistribution_IBLL.JudgeandMergeSteeringDataperHalfHour(file, t, ref strlist, vehicleid, in para);
                                            var infothrresult = _IThrottleDistribution_IBLL.JudgeandMergeThrottleDataperHalfHour(file, t, ref thrlist, vehicleid, in para);
                                            if (infostatisticresult != null)
                                            {
                                                _logger.LogInformation(infostatisticresult);
                                                importresult += infostatisticresult;
                                                canbeimported = false;
                                                break;
                                            }
                                            if (infobrkresult != null)
                                            {
                                                _logger.LogInformation(infobrkresult);
                                                importresult += infobrkresult;
                                                canbeimported = false;
                                                break;
                                            }
                                            if (infobmpresult != null)
                                            {
                                                _logger.LogInformation(infobmpresult);
                                                importresult += infobmpresult;
                                                canbeimported = false;
                                                break;
                                            }
                                            if (infogpsresult != null)
                                            {
                                                _logger.LogInformation(infogpsresult);
                                                importresult += infogpsresult;
                                                canbeimported = false;
                                                break;
                                            }
                                            if (infospdresult != null)
                                            {
                                                _logger.LogInformation(infospdresult);
                                                importresult += infospdresult;
                                                canbeimported = false;
                                                break;
                                            }
                                            if (infostrresult != null)
                                            {
                                                _logger.LogInformation(infostrresult);
                                                importresult += infostrresult;
                                                canbeimported = false;
                                                break;
                                            }
                                            if (infothrresult != null)
                                            {
                                                _logger.LogInformation(infothrresult);
                                                importresult += infothrresult;
                                                canbeimported = false;
                                                break;
                                            }
                                        }
                                        Console.WriteLine("读取数据完毕" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                        Console.WriteLine("开始存入数据" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                        if (canbeimported)
                                        {
                                            _logger.LogInformation("****************" + date + "  InputStart****************");
                                            if (gpslist.Count > 0)
                                            {
                                                _IGPSRecord_IBLL.AddAllEntity(gpslist, vehicleid);
                                                //_db.BulkInsert(gpslist);
                                            }
                                            _logger.LogInformation(vehicleid + "日期为" + date + "input里GPS数据有" + gpslist.Count + "条");
                                            if (brklist.Count > 0)
                                            {
                                                _IBrakeDistribution_IBLL.AddAllEntity(brklist, vehicleid);
                                                //_db.BulkInsert(brklist);
                                            }
                                            _logger.LogInformation(vehicleid + "日期为" + date + "input里制动数据有" + brklist.Count + "条");
                                            if (bmplist.Count > 0)
                                            {
                                                _IBumpDistribution_IBLL.AddAllEntity(bmplist, vehicleid);
                                                //_db.BulkInsert(bmplist);
                                            }
                                            _logger.LogInformation(vehicleid + "日期为" + date + "input里冲击数据有" + bmplist.Count + "条");
                                            if (strlist.Count > 0)
                                            {
                                                _ISteeringDistribution_IBLL.AddAllEntity(strlist, vehicleid);
                                                //_db.BulkInsert(strlist);
                                            }
                                            _logger.LogInformation(vehicleid + "日期为" + date + "input里转向数据有" + strlist.Count + "条");
                                            if (thrlist.Count > 0)
                                            {
                                                _IThrottleDistribution_IBLL.AddAllEntity(thrlist, vehicleid);
                                                //_db.BulkInsert(thrlist);
                                            }
                                            _logger.LogInformation(vehicleid + "日期为" + date + "input里油门数据有" + thrlist.Count + "条");
                                            if (stalist.Count > 0)
                                            {
                                                var statisticfinallist = StatisticCombined.statisticcombine(stalist, vehicleid);
                                                _IAnalysisData_ACC_Service.AddAllEntity(statisticfinallist, vehicleid);
                                                //_db.BulkInsert(stalist);
                                            }
                                            _logger.LogInformation(vehicleid + "日期为" + date + "input里统计值数据有" + stalist.Count + "条");
                                            if (spdlist.Count > 0)
                                            {
                                                var speedfinallist = SpeedCombined.speedcombine(spdlist, vehicleid);
                                                _ISpeedDistribution_ACC_IBLL.AddAllEntity(speedfinallist, vehicleid);
                                                //_db.BulkInsert(spdlist);
                                            }
                                            _logger.LogInformation(vehicleid + "日期为" + date + "input里速度里程数据有" + spdlist.Count + "条");
                                            importresult += vehicleid + "日期为" + date + "的input数据导入成功，详细情况请查看日志" + System.Environment.NewLine;
                                            _logger.LogInformation("****************" + date + "  InputEnd****************");
                                        }
                                        Console.WriteLine("存入数据完毕" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                    }
                                    else
                                    {
                                        importresult += vehicleid + "日期为" + date + "的input路径下无文件" + System.Environment.NewLine;
                                    }
                                }
                                else
                                {
                                    _logger.LogInformation(vehicleid + "日期为" + date + "的input数据还没有上传完毕");
                                    importresult += vehicleid + "日期为" + date + "的input数据还没有上传完毕" + System.Environment.NewLine;
                                }

                            }
                            else
                            {
                                _logger.LogInformation(vehicleid + "日期为" + date + "的input数据已有导入flag，不需要再次导入");
                                importresult += vehicleid + "日期为" + date + "的input数据已有导入flag，不需要再次导入" + System.Environment.NewLine;
                            }

                            if (!FileOperator.DataImportCompleteFlag(outputfiletimeinfo))
                            {
                                //判断result子文件里数据是否上传完整
                                if (FileOperator.DataTransferCompleteFlag(outputfiletimeinfo, date))
                                {
                                    _logger.LogInformation(date + "的result数据开始手动导入" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                                    //开始result上传
                                    FileInfo[] filelist = FileOperator.Isfileexist(outputfiletimeinfo);
                                    if (filelist != null && filelist.Length > 0)
                                    {
                                        List<SatictisAnalysisdataWft>wftlist = new List<SatictisAnalysisdataWft>();
                                        bool canbeimported = true;//标识是否可以导入，一旦数据有问题，就赋予false，并且跳出循环
                                        foreach (var file in filelist)
                                        {
                                            var t = await CSVFileImport.LoadCSVWFTData(file, paradict[vehicleid]);
                                            var infowftesult = _IAnalysisData_WFT_Service.JudgeandMergeWFTDataperHalfHour(file, t, ref wftlist, vehicleid, in para);
                                            if(infowftesult != null)
                                            {
                                                _logger.LogInformation(infowftesult);
                                                canbeimported = false;
                                                break;
                                            }
                                        }
                                        if (canbeimported)
                                        {
                                            _logger.LogInformation("****************" + date + "  ResultStart****************");

                                            var wftfinallist = WFTCombined.wftcombine(wftlist, vehicleid);
                                            if (wftfinallist.Count > 0)
                                            {
                                                _db.BulkInsert(wftfinallist);
                                            }
                                            _logger.LogInformation(vehicleid + "日期为" + date + "result里WFT数据有" + wftfinallist.Count + "条");
                                            importresult += vehicleid + "日期为" + date + "的result数据导入成功，详细情况请查看日志" + System.Environment.NewLine;

                                            _logger.LogInformation("****************" + date + "  ResultEnd****************");

                                        }
                                        else
                                        {
                                            importresult += vehicleid + "日期为" + date + "的result数据有错误，无法导入" + System.Environment.NewLine;
                                        }

                                    }


                                }
                                else
                                {
                                    _logger.LogInformation(vehicleid + "日期为" + date + "的result数据还没有上传完毕");
                                    importresult += vehicleid + "日期为" + date + "的result数据还没有上传完毕" + System.Environment.NewLine;
                                }
                            }
                            else
                            {
                                _logger.LogInformation(vehicleid + "日期为" + date + "的result数据已有导入flag，不需要再次导入");
                                importresult += vehicleid + "日期为" + date + "的result数据已有导入flag，不需要再次导入" + System.Environment.NewLine;
                            }

                        }
                        var memoryCacheALL = (MemoryCache)_memoryCache;//先转成MemoryCache，否则无法使用Compact方法
                        memoryCacheALL.Compact(1.0);//清除所有缓存
                                                    //_memoryCache.Compact(1.0);//一旦有新的数据导入到数据库中，就执行一次内存重置，防止还用之前的缓存来展示数据
                    }
                    catch (Exception ex)
                    {
                        importresult = "程序有错误，请查看日志！";
                        _logger.LogInformation(ex + "错误发生在" + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss"));
                    }
                    return importresult;
                }
                else
                {
                    return vehicleid + "日期选择为空";

                }
               
            }
            else
            {
                return "未开启允许导入设置";
            }

        }
           


        

        //数据库中查询出最新数据的日期返回给前端显示
        public string FinishedDate(string vehicleid)
        {
                var isnull = _ISpeedDistribution_ACC_IBLL.LoadEntities(a => a.VehicleId == vehicleid, vehicleid).OrderBy(a => a.Datadate).Select(a => a.Datadate).LastOrDefault();
                if (isnull != null)
                {
                    var finisheddate = (DateTime)isnull;
                    return finisheddate.AddDays(1).ToShortDateString();
                }
                else
                {
                    return "此车辆目前还没有数据";
                }

        }

        public string ShowConfiguration(string vehicleid)
        {
            string wft = paradict[vehicleid].WFTImport == 1 ? "true  " : "false  ";
            string gps = paradict[vehicleid].GPSImport ==1 ? "true  " : "false  ";
            string brake = paradict[vehicleid].BrakeImport ==1 ? "true  " : "false  ";
            string throttle = paradict[vehicleid].ThrottleImport == 1 ? "true  " : "false  ";
            string steering = paradict[vehicleid].SteeringImport == 1 ? "true  " : "false  ";
            string speed = paradict[vehicleid].SpeedImport == 1 ? "true  " : "false  ";
            string bump = paradict[vehicleid].BumpImport == 1 ? "true  " : "false  ";
            string statistic = paradict[vehicleid].StatisticImport == 1 ? "true" : "false";
            var  importresult= "WFT:" + wft + System.Environment.NewLine+
                "GPS:"+ gps + System.Environment.NewLine 
                +"Brake:"+ brake + System.Environment.NewLine
                +"Throttle:"+ throttle + System.Environment.NewLine
                + "Steering:" + steering + System.Environment.NewLine
                + "Speed:" + speed + System.Environment.NewLine
                + "Bump:" +bump + System.Environment.NewLine
                + "Statistic:" + statistic;
            return importresult;
        }
        


    }
}
