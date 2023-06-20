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
using Tools.ListOperation.BrakeListOperation;
using Tools.ListOperation.EngineSpeedListOperation;
using Tools.ListOperation.StatisticAccListOperation;
using Tools.MyConfig;
using static Tools.FileOperation.CSVFileImport;

namespace DAL.SH_ADF0979DAL
{
    public class EngineSpeedTimeDistribution_DAL : BaseDAL<EngineRpmDistribution_Time>, IEngineSpeedTimeDistribution_IDAL
    {
        public EngineSpeedTimeDistribution_DAL(datawatchContext DB) : base(DB)
        {

        }
        public string JudgeandMergeEngSpdTimeDataperHalfHour(FileInfo file, LoadCSVDataStructforImport importstruct, ref List<EngineRpmDistribution_Time> blist, string vehicleid, in VehicleIDPara vehicleIDPara)
        {
            var time = Convert.ToDateTime(file.Name.Split('-')[0].Replace("_", "-"));
            if (vehicleIDPara.importengspd == 1)
            {
                var sqllist = _DB.EngineRpmDistribution_Time.Where(a => a.Datadate > time && a.Datadate < time.AddDays(1)&&a.VehicleId==vehicleid).Select(a => a.VehicleId).AsNoTracking().FirstOrDefault();
                if (string.IsNullOrEmpty(sqllist))
                {
                    if (importstruct.Iscontinue)
                    {
                        if (true)
                        {
                            var list = AddEngineSpeedTimeDistribution.addEngSpdTime(importstruct.lists[1], importstruct.lists[13],importstruct.lists[0], vehicleid, importstruct.name, importstruct.datetime);
                            if (list.Count > 0)
                            {
                                blist = blist.Concat(list).ToList();
                            }
                            return null;
                        }
                    }
                    else
                    {
                        return ($"此文件{file.Name}进行转速时间计算时出现了问题，数据状态为{importstruct.status}！{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
                    }
                }
                else
                {
                    return ($"此文件{file.Name}进行转速时间计算时已导入过，须删除后再重新导入！{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");

                }
            }
           
            else
            {
                return null;
            }
        }
    }
}
