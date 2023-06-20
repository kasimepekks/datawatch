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
using Tools.ListOperation.StatisticAccListOperation;
using Tools.MyConfig;
using static Tools.FileOperation.CSVFileImport;

namespace DAL.SH_ADF0979DAL
{
    public class SpeedDistribution_ACC_DAL : BaseDAL<Speeddistribution>, ISpeedDistribution_ACC_IDAL
    {
        public SpeedDistribution_ACC_DAL(datawatchContext DB) : base(DB)
        {

        }
        public string JudgeandMergeSpdDataperHalfHour(FileInfo file, LoadCSVDataStructforImport importstruct, ref List<Speeddistribution> blist, string vehicleid, in VehicleIDPara vehicleIDPara)
        {
            var time = Convert.ToDateTime(file.Name.Split('-')[0].Replace("_", "-"));
            if (vehicleIDPara.SpeedImport == 1)
            {
                var sqllist = _DB.Speeddistributions.Where(a => a.Datadate > time && a.Datadate < time.AddDays(1)&&a.VehicleId==vehicleid).Select(a => a.VehicleId).AsNoTracking().FirstOrDefault();
                if (string.IsNullOrEmpty(sqllist))
                {
                    if (importstruct.Iscontinue)
                    {
                        if (true)
                        {
                            var list = AddSpeedDistribution.addspeedlist(importstruct.lists[1], importstruct.lists[0], vehicleid, importstruct.name, importstruct.datetime);
                            if (list.Count > 0)
                            {
                                blist = blist.Concat(list).ToList();
                            }
                            return null;
                        }
                    }
                    else
                    {
                        return ($"此文件{file.Name}进行速度里程计算时出现了问题，数据状态为{importstruct.status}！{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
                    }
                }
                else
                {
                    return ($"此文件{file.Name}进行速度里程计算时已导入过，须删除后再重新导入！{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");

                }
            }
           
            else
            {
                return null;
            }
        }

    }
}
