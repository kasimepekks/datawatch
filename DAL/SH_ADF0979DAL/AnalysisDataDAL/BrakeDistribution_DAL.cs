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
using Tools.ListOperation.BrakeListOperation;
using Tools.ListOperation.StatisticAccListOperation;
using Tools.MyConfig;
using static Tools.FileOperation.CSVFileImport;

namespace DAL.SH_ADF0979DAL
{
    public class BrakeDistribution_DAL : BaseDAL<Brakerecognition>, IBrakeDistribution_IDAL
    {
        public BrakeDistribution_DAL(datawatchContext DB) : base(DB)
        {

        }
        public string JudgeandMergeBrkDataperHalfHour(FileInfo file, LoadCSVDataStructforImport importstruct, ref List<Brakerecognition> blist, string vehicleid, in VehicleIDPara vehicleIDPara)
        {
          
            var time = Convert.ToDateTime(file.Name.Split('-')[0].Replace("_", "-"));
            if (vehicleIDPara.BrakeImport == 1)
            {
                var sqllist = _DB.Brakerecognitions.Where(a => a.Datadate > time && a.Datadate < time.AddDays(1)).Select(a => a.VehicleId).AsNoTracking().FirstOrDefault();
                if (string.IsNullOrEmpty(sqllist))
                {
                    if (importstruct.Iscontinue)
                    {
                        if (importstruct.boolcan[7]&& importstruct.boolcan[8])//判断是否通道序号7和8是否有问题
                        {
                            var list = AddBrake.addbrakelist(importstruct.lists[8], importstruct.lists[7], importstruct.spd, vehicleid, importstruct.name, importstruct.datetime, in vehicleIDPara);
                            if (list.Count > 0)
                            {
                                blist = blist.Concat(list).ToList();
                            }
                            return null;
                        }
                        else
                        {
                            return ($"此文件{file.Name}进行制动计算时序号为{names[7]}:{importstruct.boolcan[7]}{names[8]}:{importstruct.boolcan[8]}出现了问题，数据状态为{importstruct.status}！{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
                        }
                    }
                    else
                    {
                        return ($"此文件{file.Name}进行制动计算时出现了问题，数据状态为{importstruct.status}！{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
                    }
                }
                else
                {
                    return ($"此文件{file.Name}进行制动计算时已导入过，须删除后再重新导入！{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");

                }
            }
            else
            {
                return null;
            }
          
        }

    }
}
