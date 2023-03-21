using Coravel.Cache;
using IBLL.SH_ADF0979IBLL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MysqlforDataWatch;
using RLDA_VehicleData_Watch.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Tools;
using Tools.MyConfig;

namespace RLDA_VehicleData_Watch.Controllers.Base
{
    public class FileWatchController : Controller
    {
        //private readonly IStatistic_ACC_IBLL _IStatistic_ACC_Service;
        private readonly IRealTime_ACC_IBLL _IRealTime_ACC_Service;
        private readonly IHubContext<MyHub> _hubContext;//signalr
        private readonly IConfiguration _configuration;
        private readonly ILogger<FileWatchController> _logger;
        //private static string filewatcherpath;
        private static string filewatcherneed;
        private readonly DbContext _db;
        private static GetVehicleParafromSql _getVehicleParafromSql;
        //private static byte monitorreducetimes;
        //private static int intertimes;
        //private static string csvcollumnname;
        //private static string title;
        //private static string[] echarttitle;
        //private static string[] echartdata;
      
        private readonly IMemoryCache _memoryCache;//内存缓存
        //private static string vehicleid;
        //private readonly string datefile = FileOperator.DatetoName(DateTime.Now.ToString("MM-dd"));//日期文件夹名

        private static Dictionary<string, VehicleWatchPara> paradict = new Dictionary<string, VehicleWatchPara>();//存储监控参数
        private static Dictionary<string, double> vehicledistance = new Dictionary<string, double>();//存储每个车的里程
        private static Dictionary<string, double> vehiclecumdistance = new Dictionary<string, double>();//存储每个车的里程
        private static Dictionary<string, string> filename = new Dictionary<string, string>();//存储每个文件名
        private static Dictionary<string, int> readtimeseveryday = new Dictionary<string, int>();//存储每个文件名
        private static Dictionary<string, string> vehiclefilepath = new Dictionary<string, string>();//存储每个车辆的路径，用于区分监控的文件路径
        public FileWatchController( IRealTime_ACC_IBLL IRealTime_ACC_Service, IHubContext<MyHub> hubContext, IConfiguration configuration,ILogger<FileWatchController> logger, DbContext db, IMemoryCache memoryCache, GetVehicleParafromSql getVehicleParafromSql)
        {
            //_IStatistic_ACC_Service = IStatistic_ACC_Service;
            _IRealTime_ACC_Service = IRealTime_ACC_Service;
            //_IStatistic_WFT_Service = IStatistic_WFT_Service;
            _configuration = configuration;
            this._logger = logger;
            _hubContext = hubContext;
            _db = db;
            _memoryCache = memoryCache;
            _getVehicleParafromSql = getVehicleParafromSql;
            filewatcherneed = _configuration["DataMonitor:MonitorRequired"];
        }
        /// <summary>
        /// 判断是否需要filewatcher类，如不需要，直接return OK，页面加载会执行一次
        /// </summary>
        /// <param name="_vehicleID"></param>
        /// <returns></returns>
        public string FileWatch(string vehicleid)
        {
            //先用内存缓存来存储车辆参数
            var re = _memoryCache.GetOrCreate<VehicleWatchPara>(vehicleid + "watchpara", value =>
            {
                return _getVehicleParafromSql.GetWatchDatafromSql(vehicleid);
            });
            if (re != null)
            {
                //初始化车辆参数字典，由于字典是静态字段，不同实例可以共享
                if (paradict.Count != 0)
                {
                    //如果字典里没有此车辆则添加
                    if (!paradict.ContainsKey(vehicleid))
                    {
                        paradict.Add(vehicleid, re);
                    }
                    //如果字典里有此车辆则覆盖
                    else
                    {
                        paradict[vehicleid] = re;
                    }
                }
                else
                {
                    paradict.Add(vehicleid, re);
                }
                //为false就是启动filewatch监控，为true就是启动定时监控
                if (filewatcherneed == "false")
                {
                    _logger.LogWarning("启动了" + vehicleid + "的FileWatch类监控页面" + paradict[vehicleid].filewatcherpath);
                    //把每个车辆的路径放入这个字典中，由于_memoryCache无法从value获得key，所以用Dictionary来代替
                    if (readtimeseveryday.Count != 0)
                    {
                        //每次刷新网页后看看字典里相同的车辆是不是有2个以上的key，也就是说有2个以上的天数的数据，所以删除除最后一个的数据
                        if (readtimeseveryday.Count > 1)
                        {
                            List<string> key = new List<string>();
                            foreach (var item in readtimeseveryday)
                            {
                                if (item.Key.Contains( vehicleid ))//筛选特定车辆号的字典
                                {
                                    key.Add(item.Key);
                                }
                            }
                            if (key.Count > 1)
                            {
                                for (int i = 0; i < key.Count - 1; i++)
                                {
                                    readtimeseveryday.Remove(key[i]);
                                }
                            }
                        }
                        if (!readtimeseveryday.ContainsKey(vehicleid +"每天次数"+ FileOperator.DatetoName(DateTime.Now.ToString("MM-dd"))))
                        {
                            readtimeseveryday.Add(vehicleid + "每天次数" + FileOperator.DatetoName(DateTime.Now.ToString("MM-dd")), 0);
                        }
                    }
                    else
                    {
                        readtimeseveryday.Add(vehicleid + "每天次数" + FileOperator.DatetoName(DateTime.Now.ToString("MM-dd")), 0);
                    }
                    if (vehiclefilepath.Count != 0)
                    {
                        if (!vehiclefilepath.ContainsKey(vehicleid))
                        {
                            vehiclefilepath.Add(vehicleid, paradict[vehicleid].filewatcherpath);
                        }
                        else
                        {
                            vehiclefilepath[vehicleid] = paradict[vehicleid].filewatcherpath;
                        }
                    }
                    else
                    {
                        vehiclefilepath.Add(vehicleid, paradict[vehicleid].filewatcherpath);
                    }
                    if (vehiclecumdistance.Count != 0)
                    {
                        if (vehiclecumdistance.Count > 1)
                        {
                            List<string> key = new List<string>();
                            foreach (var item in vehiclecumdistance)
                            {
                                if (item.Key.Contains(vehicleid))//筛选特定车辆号的字典
                                {
                                    key.Add(item.Key);
                                }
                            }
                            if (key.Count > 1)
                            {
                                for (int i = 0; i < key.Count - 1; i++)
                                {
                                    vehiclecumdistance.Remove(key[i]);
                                }
                            }
                        }
                        if (!vehiclecumdistance.ContainsKey(vehicleid + "累计里程" + FileOperator.DatetoName(DateTime.Now.ToString("MM-dd"))))
                        {
                            vehiclecumdistance.Add(vehicleid + "累计里程" + FileOperator.DatetoName(DateTime.Now.ToString("MM-dd")), 0);
                            
                        }
                    }
                    else
                    {
                        vehiclecumdistance.Add(vehicleid + "累计里程" + FileOperator.DatetoName(DateTime.Now.ToString("MM-dd")), 0);

                    }
                    if (vehicledistance.Count != 0)
                    {
                        if (vehicledistance.Count > 1)
                        {
                            List<string> key = new List<string>();
                            foreach (var item in vehicledistance)
                            {
                                if (item.Key.Contains(vehicleid))//筛选特定车辆号的字典
                                {
                                    key.Add(item.Key);
                                }
                            }
                            if (key.Count > 1)
                            {
                                for (int i = 0; i < key.Count - 1; i++)
                                {
                                    vehicledistance.Remove(key[i]);
                                }
                            }
                        }
                        if (!vehicledistance.ContainsKey(vehicleid + "每天里程" + FileOperator.DatetoName(DateTime.Now.ToString("MM-dd"))))
                        {
                            vehicledistance.Add(vehicleid + "每天里程" + FileOperator.DatetoName(DateTime.Now.ToString("MM-dd")), 0);

                        }

                    }
                    else
                    {
                        vehicledistance.Add(vehicleid + "每天里程" + FileOperator.DatetoName(DateTime.Now.ToString("MM-dd")), 0);

                    }
                    if (filename.Count != 0)
                    {
                        if (!filename.ContainsKey("input"))
                        {
                            filename.Add("input", "");

                        }
                        else if (!filename.ContainsKey("result"))
                        {
                            filename.Add("result", "");
                        }
                    }
                    else
                    {
                        filename.Add("input", "");
                        filename.Add("result", "");

                    }
                    try
                    {
                        WatcherStrat(paradict[vehicleid].filewatcherpath, "*.csv");
                    }
                    catch (Exception ex)
                    {

                        _logger.LogInformation(ex.Message + "。错误源为：" + ex.Source); ;
                    }
                }
                else
                {
                    _logger.LogInformation("启动了" + vehicleid + "的定时监控页面");
                }
                if (paradict[vehicleid].title != null)
                {
                    return paradict[vehicleid].title;
                }
                else
                {
                    return "请先添加此车辆监控标题并刷新页面！";
                }
            }
            else
            {
                _memoryCache.Remove(vehicleid + "watchpara");
                return "数据库未添加此车辆！";
            }
         
           
            
        }
        [Authorize]
        public IActionResult EditFilemonitorpathMemory(string vehicleidtext,string newwatchpath)
        {
            _memoryCache.Set<string>(vehicleidtext + "monitorinputpath",newwatchpath);
            if (vehiclefilepath.ContainsKey(vehicleidtext))
            {
                vehiclefilepath[vehicleidtext]=newwatchpath;
            }
            _logger.LogInformation(vehicleidtext + "监控路径原内存数据已更新");
            return Ok();
        }

        public  void WatcherStrat(string path, string filter)
        {
           
            FileSystemWatcher watcher = new FileSystemWatcher();
           
            watcher.Path = path;
            watcher.Filter = filter;
            watcher.IncludeSubdirectories = true;
            //watcher.NotifyFilter = NotifyFilters.Size;
            watcher.Created += new FileSystemEventHandler(OnProcess);
            //watcher.Changed += new FileSystemEventHandler(OnProcess);
            watcher.EnableRaisingEvents = true;
           
        }
        private  void OnProcess(object source, FileSystemEventArgs e)
        {


            if (e.ChangeType == WatcherChangeTypes.Created)
            {
                Task.Run(()=>OnCreated(source, e));
            }


        }
        private void Waiting(string path)

        {

            try
            {
                FileInfo fi;
                fi = new FileInfo(path);
                long len1, len2;
                len2 = fi.Length;
                do
                {
                    len1 = len2;
                    Thread.Sleep(1000);//等待1秒钟
                    fi.Refresh();//这个语句不能漏了
                    len2 = fi.Length;
                } while (len1 < len2);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.Message);
            }
        }
    

        private  async Task OnCreated(object source, FileSystemEventArgs e)
        {
            try
                {
                var vepath=((System.IO.FileSystemWatcher)source).Path;//获得实际的监控到文件的路径，因为可能会打开很多监控页面，需要判断是哪个车辆的监控路径下有新增的文件，然后再获取这个车辆号，传送给前端，前端根据车辆号来判断是否需要显示
                var whichvehicle = vehiclefilepath.Where(q => q.Value == vepath).FirstOrDefault().Key;//根据实际的监控到的文件路径获得是哪个车
                if (e.FullPath.Contains("input") && filename["input"] != e.Name && e.Name.Contains(FileOperator.DatetoName(DateTime.Now.ToString("MM-dd"))))
                {
                    filename["input"] = e.Name;
                    Waiting(e.FullPath);

                    //_logger.LogWarning(e.Name + "已被监控发现");
                    await Task.Run(async () =>
                    {

                        var structall = await _IRealTime_ACC_Service.ReadCSVFiletoList(e.FullPath, e.Name, paradict[whichvehicle].csvcollumnname, paradict[whichvehicle].monitorreducetimes, paradict[whichvehicle].intertimes);
                       
                        switch (structall.status)
                        {
                            case 0:
                                if (vehicledistance.ContainsKey(whichvehicle + "每天里程" + FileOperator.DatetoName(DateTime.Now.ToString("MM-dd"))))
                                {
                                    vehicledistance[whichvehicle + "每天里程" + FileOperator.DatetoName(DateTime.Now.ToString("MM-dd"))] = Math.Round(structall.sdistance, 2);
                                    vehiclecumdistance[whichvehicle + "累计里程" + FileOperator.DatetoName(DateTime.Now.ToString("MM-dd"))] += vehicledistance[whichvehicle + "每天里程" + FileOperator.DatetoName(DateTime.Now.ToString("MM-dd"))];

                                }
                                else
                                {
                                    _logger.LogWarning($"实时里程字典里没有{whichvehicle + FileOperator.DatetoName(DateTime.Now.ToString("MM-dd"))}的数据");

                                }
                                if (readtimeseveryday.ContainsKey(whichvehicle + "每天次数" + FileOperator.DatetoName(DateTime.Now.ToString("MM-dd"))))
                                {
                                    readtimeseveryday[whichvehicle + "每天次数" + FileOperator.DatetoName(DateTime.Now.ToString("MM-dd"))] += 1;

                                    foreach (var i in MyHub.user)
                                    {
                                        await _hubContext.Clients.Group(i).SendAsync("SpeedtoDistance", whichvehicle, vehiclecumdistance[whichvehicle + "累计里程" + FileOperator.DatetoName(DateTime.Now.ToString("MM-dd"))], structall.Speed, structall.Lat, structall.Lon, readtimeseveryday[whichvehicle + "每天次数" + FileOperator.DatetoName(DateTime.Now.ToString("MM-dd"))]);
                                        await _hubContext.Clients.Group(i).SendAsync("ReloadDataACC", whichvehicle, structall.Time, structall.name, structall.otherchannels, structall.channelorderlist, paradict[whichvehicle].echarttitle, paradict[whichvehicle].echartdata);
                                    }

                                }
                                else
                                {
                                    _logger.LogWarning($"次数字典里没有{whichvehicle + FileOperator.DatetoName(DateTime.Now.ToString("MM-dd"))}的数据");
                                }
                               
                                //_logger.LogWarning(filename["input"] + "已传送数据");
                                break;
                            case 1:
                                _logger.LogWarning(filename["input"] + "数据库字段名数量少于1");
                                break;
                            case 2:
                                _logger.LogWarning(filename["input"] + "文件被占用");
                                break;
                            case 3:
                                _logger.LogWarning(filename["input"] + "文件大小为0");
                                break;
                        }

                    });

                }

            }
            catch (Exception ex)
                {
                    _logger.LogWarning(ex.Message);
                }
        }
    }
}
