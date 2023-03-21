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
using Tools.ListOperation;
using Tools.ListOperation.BrakeListOperation;
using Tools.ListOperation.BumpListOperation;
using Tools.MyConfig;
using static Tools.FileOperation.CSVFileImport;

namespace DAL.SH_ADF0979DAL
{
    public class BumpDistribution_DAL : BaseDAL<Bumprecognition>, IBumpDistribution_IDAL
    {
        public BumpDistribution_DAL(datawatchContext DB) : base(DB)
        {

        }

        public string JudgeandMergeBmpDataperHalfHour(FileInfo file, LoadCSVDataStructforImport importstruct, ref List<Bumprecognition> blist, string vehicleid, in VehicleIDPara vehicleIDPara)
        {
            
            var time = Convert.ToDateTime(file.Name.Split('-')[0].Replace("_", "-"));
            if (vehicleIDPara.BumpImport == 1)
            {
                var sqllist = _DB.Bumprecognitions.Where(a => a.Datadate > time && a.Datadate < time.AddDays(1)).Select(a => a.VehicleId).AsNoTracking().FirstOrDefault();
                if (string.IsNullOrEmpty(sqllist))
                {
                    if (importstruct.Iscontinue)
                    {
                        if (importstruct.boolcan[4]&& importstruct.boolcan[5] && importstruct.boolcan[6])
                        {
                            var list = AddBump.addbumplist(importstruct.lists[4], importstruct.lists[5], importstruct.lists[6], importstruct.spd, vehicleid, importstruct.name, importstruct.datetime, in vehicleIDPara);
                            if (list.Count > 0)
                            {
                                blist = blist.Concat(list).ToList();
                            }
                            return null;
                        }
                        else
                        {
                            return ($"此文件{file.Name}进行冲击计算时序号为{names[4]}:{importstruct.boolcan[4]}{names[5]}:{importstruct.boolcan[5]}{names[6]}:{importstruct.boolcan[6]}这一列出现了问题，数据状态为{importstruct.status}！{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
                        }
                    }
                    else
                    {
                        return ($"此文件{file.Name}进行冲击计算时出现了问题，数据状态为{importstruct.status}！{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
                    }
                }
                else
                {
                    return ($"此文件{file.Name}进行冲击计算时已导入过，须删除后再重新导入！{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");

                }
            }
           
            else
            {
                return null;
            }
        }

    }
}
