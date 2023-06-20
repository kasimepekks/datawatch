using IDAL.SH_ADF0979IDAL;
using Microsoft.EntityFrameworkCore;
using MysqlforDataWatch;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools;
using Tools.AddDistance;
using Tools.FileOperation;
using Tools.GPSCal;
using Tools.ListOperation.BumpListOperation;
using Tools.MyConfig;
using static Tools.FileOperation.CSVFileImport;

namespace DAL.SH_ADF0979DAL
{
    public class GPSRecord_DAL : BaseDAL<Gpsrecord>, IGPSRecord_IDAL
    {
        /// <summary>
        /// DB会传给基类的构造函数，并调用基类的构造函数
        /// </summary>
        /// <param name="DB"></param>
        public GPSRecord_DAL(datawatchContext DB) : base(DB)
        {

        }
        public string JudgeandMergeGPSDataperHalfHour(FileInfo file, LoadCSVDataStructforImport importstruct, ref List<Gpsrecord> blist, string vehicleid, in VehicleIDPara vehicleIDPara)
        {
            _DB.vehicleid = vehicleid;
            var time = Convert.ToDateTime(file.Name.Split('-')[0].Replace("_", "-"));
            if (vehicleIDPara.GPSImport == 1)
            {
                var sqllist = _DB.Set<Gpsrecord>().Where(a => a.Datadate > time && a.Datadate < time.AddDays(1) && a.VehicleId == vehicleid).Select(a => a.VehicleId).AsNoTracking().FirstOrDefault();
                if (string.IsNullOrEmpty(sqllist))
                {
                    if (importstruct.Iscontinue)
                    {
                        if (importstruct.boolcan[2]& importstruct.boolcan[3])
                        {
                            var list = AddGPS.addgpslist(importstruct.lists[2], importstruct.lists[3], importstruct.lists[1], vehicleid, importstruct.name, importstruct.datetime, in vehicleIDPara);
                            if (list.Count > 0)
                            {
                                blist = blist.Concat(list).ToList();
                            }

                            return null;
                        }
                        else
                        {
                            return ($"此文件{file.Name}进行经纬度计算时序号为{names[2]}:{importstruct.boolcan[2]}{names[3]}:{importstruct.boolcan[3]}这一列出现了问题，数据状态为{importstruct.status}！{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
                        }
                    }
                    else
                    {
                        return ($"此文件{file.Name}进行经纬度计算时出现了问题，数据状态为{importstruct.status}！{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
                    }
                }
                else
                {
                    return ($"此文件{file.Name}进行经纬度计算时已导入过，须删除后再重新导入！{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
                }
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 根据车辆号来清空对应的gps的表，清空意味着清除所有的数据，并且自增主键值归1
        /// </summary>
        /// <param name="vehicle"></param>
        /// <returns></returns>
        public int TruncateTable( string vehicle)
        {
            string ta = $"gpsrecord_{vehicle}";
            var result= _DB.Database.ExecuteSqlRaw($"truncate table {ta}");
            return result;
            //return DB.SaveChanges() > 0;
        }
        /// <summary>
        /// 根据车辆号来创建对应的gps的表（如数据库中没有）
        /// </summary>
        /// <param name="vehicle"></param>
        /// <returns></returns>
        public async Task<int> CreateTable(string vehicle)
        {
            _DB.vehicleid = vehicle;//这里也需要赋值，因为在DataImportController里它是最先执行的数据库操作
            string fs = $@"CREATE TABLE IF NOT EXISTS `gpsrecord_{vehicle}` (
  `key` bigint(20) NOT NULL AUTO_INCREMENT,
  `id` varchar(64) NOT NULL,
  `VehicleID` varchar(64) DEFAULT NULL,
  `filename` varchar(64) DEFAULT NULL,
  `datadate` datetime DEFAULT NULL,
  `Lat` double(64,10) NOT NULL,
  `Lon` double(64,10) NOT NULL,
  `Speed` double(64,0) NOT NULL,
  PRIMARY KEY (`key`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8;
";
            var result = await _DB.Database.ExecuteSqlRawAsync(fs);
            return result;
            //return DB.SaveChanges() > 0;
        }
      
    }
}
