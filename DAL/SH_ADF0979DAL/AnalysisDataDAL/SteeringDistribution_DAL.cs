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
using Tools.ListOperation.StatisticAccListOperation;
using Tools.ListOperation.SteeringListOperation;
using Tools.MyConfig;
using static Tools.FileOperation.CSVFileImport;

namespace DAL.SH_ADF0979DAL
{
    public class SteeringDistribution_DAL : BaseDAL<Streeringrecognition>, ISteeringDistribution_IDAL
    {
        public SteeringDistribution_DAL(datawatchContext DB) : base(DB)
        {

        }
        
        public string JudgeandMergeSteeringDataperHalfHour(FileInfo file, LoadCSVDataStructforImport importstruct, ref List<Streeringrecognition> blist, string vehicleid, in VehicleIDPara vehicleIDPara)
        {
            var time = Convert.ToDateTime(file.Name.Split('-')[0].Replace("_", "-"));
            if (vehicleIDPara.SteeringImport == 1)
            {
                var sqllist = _DB.Streeringrecognitions.Where(a => a.Datadate > time && a.Datadate < time.AddDays(1) && a.VehicleId == vehicleid).Select(a => a.VehicleId).AsNoTracking().FirstOrDefault();
                if (string.IsNullOrEmpty(sqllist))
                {
                    if (importstruct.Iscontinue)
                    {
                        if (importstruct.boolcan[11]&& importstruct.boolcan[12])
                        {
                            var list = AddSteering.addsteeringlist(importstruct.lists[11], importstruct.lists[12], importstruct.spd, importstruct.lists[10], vehicleid, importstruct.name, importstruct.datetime, in vehicleIDPara);
                            if (list.Count > 0)
                            {
                                blist = blist.Concat(list).ToList();
                            }
                            return null;
                        }
                        else
                        {
                            return ($"此文件{file.Name}进行转向计算时序号为{names[11]}:{importstruct.boolcan[11]}{names[12]}:{importstruct.boolcan[12]}出现了问题，数据状态为{importstruct.status}！{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
                        }
                    }
                    else
                    {
                        return ($"此文件{file.Name}进行转向计算时出现了问题，数据状态为{importstruct.status}！{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
                    }
                }
                else
                {
                    return ($"此文件{file.Name}进行转向计算时已导入过，须删除后再重新导入！{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");

                }
            }
           
            else
            {
                return null;
            }
        }

    }
}
