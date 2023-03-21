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
using Tools.ListOperation.SteeringListOperation;
using Tools.ListOperation.ThrottleListOperation;
using Tools.MyConfig;
using static Tools.FileOperation.CSVFileImport;

namespace DAL.SH_ADF0979DAL
{
    public class ThrottleDistribution_DAL : BaseDAL<Throttlerecognition>, IThrottleDistribution_IDAL
    {
        public ThrottleDistribution_DAL(datawatchContext DB) : base(DB)
        {

        }
        public string JudgeandMergeThrottleDataperHalfHour(FileInfo file, LoadCSVDataStructforImport importstruct, ref List<Throttlerecognition> blist, string vehicleid, in VehicleIDPara vehicleIDPara)
        {
            var time = Convert.ToDateTime(file.Name.Split('-')[0].Replace("_", "-"));
            if (vehicleIDPara.ThrottleImport == 1)
            {
                var sqllist = _DB.Throttlerecognitions.Where(a => a.Datadate > time && a.Datadate < time.AddDays(1)).Select(a => a.VehicleId).AsNoTracking().FirstOrDefault();
                if (string.IsNullOrEmpty(sqllist))
                {
                    if (importstruct.Iscontinue)
                    {
                        if (importstruct.boolcan[7] && importstruct.boolcan[9])
                        {
                            var list = AddThrottle.addthrottlelist(importstruct.lists[9], importstruct.lists[7], importstruct.spd, vehicleid, importstruct.name, importstruct.datetime, in vehicleIDPara);
                            if (list.Count > 0)
                            {
                                blist = blist.Concat(list).ToList();
                            }
                            return null;
                        }
                        else
                        {
                            return ($"此文件{file.Name}进行油门计算时{names[7]}:{importstruct.boolcan[7]}{names[9]}:{importstruct.boolcan[9]}出现了问题，数据状态为{importstruct.status}！{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
                        }
                    }
                    else
                    {
                        return ($"此文件{file.Name}进行油门计算时出现了问题，数据状态为{importstruct.status}！{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
                    }
                }
                else
                {
                    return ($"此文件{file.Name}进行油门计算时已导入过，须删除后再重新导入！{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");

                }
            }
           
            else
            {
                return null;
            }
        }

    }
}
