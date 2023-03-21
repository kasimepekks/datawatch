using IDAL.SH_ADF0979IDAL;
using Microsoft.EntityFrameworkCore;
using MysqlforDataWatch;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Tools;
using Tools.AddDamage;
using Tools.AddDistance;
using Tools.Cash;
using Tools.FileOperation;
using Tools.GPSCal;
using Tools.ListOperation;
using Tools.ListOperation.BrakeListOperation;
using Tools.ListOperation.BumpListOperation;
using Tools.ListOperation.StatisticAccListOperation;
using Tools.ListOperation.SteeringListOperation;
using Tools.ListOperation.ThrottleListOperation;
using Tools.MyConfig;
using static Tools.FileOperation.CSVFileImport;

namespace DAL.SH_ADF0979DAL
{
  public  class AnalysisData_ACC_DAL : BaseDAL<SatictisAnalysisdataAcc>, IAnalysisData_ACC_IDAL
  {
       
        public AnalysisData_ACC_DAL(datawatchContext DB) : base(DB)
        {

        }

        /// <summary>
        /// 针对每个csv文件数据进行判断，先判断是否开了导入权限，再判断数据库里是否导入过，然后再进行计算获得计算后需要导入的数据，最后返回导入结果的信息
        /// 把判断和计算统一成一个方法
        /// </summary>
        /// <param name="file"></param>
        /// <param name="importstruct"></param>
        /// <param name="_SatictisAnalysisdataAccList"></param>
        /// <param name="vehicleid"></param>
        /// <param name="vehicleIDPara"></param>
        /// <returns></returns>
        public string JudgeandMergeStatisticDataperHalfHour(FileInfo file, LoadCSVDataStructforImport importstruct, ref List<SatictisAnalysisdataAcc> _SatictisAnalysisdataAccList, string vehicleid, in VehicleIDPara vehicleIDPara)
        {
            var time = Convert.ToDateTime(file.Name.Split('-')[0].Replace("_", "-"));
            if (vehicleIDPara.StatisticImport == 1)
            {
                _DB.vehicleid = vehicleid;
                var sqllist = _DB.SatictisAnalysisdataAccs.Where(a => a.Datadate > time && a.Datadate < time.AddDays(1)).Select(a => a.VehicleId).AsNoTracking().FirstOrDefault();
                if (string.IsNullOrEmpty(sqllist))
                {
                    if (importstruct.Iscontinue)
                    {
                        if (true)//由于统计值计算不需要特定的通道，所以不进行判断
                        {
                            var statisticlist = AddStatisticacc.addstatisticlist(importstruct.columncount, importstruct.tablehead, importstruct.datatable, vehicleid, importstruct.name, importstruct.datetime);
                            if (statisticlist.Count > 0)
                            {
                                _SatictisAnalysisdataAccList = _SatictisAnalysisdataAccList.Concat(statisticlist).ToList();
                            }
                            return null;
                        }
                    }
                    else
                    {
                        return ($"此文件{file.Name}进行统计值计算时出现了问题，数据状态为{importstruct.status}！{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
                    }
                }
                else
                {
                    return ($"此文件{file.Name}进行统计值计算时已导入过，须删除后再重新导入！{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
                }
            }
            else
            {
                return null;
            }
        }

    }
}
