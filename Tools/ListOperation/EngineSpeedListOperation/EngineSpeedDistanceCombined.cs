using MysqlforDataWatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tools.ListOperation.EngineSpeedListOperation
{
    public static class EngineSpeedDistanceCombined
    {
        /// <summary>
        /// 按30分钟来合并转速里程的数据
        /// </summary>
        /// <param name="orlist"></param>
        /// <param name="vehicleid"></param>
        /// <returns></returns>
        public static List<EngineRpmDistribution_Distance> engspddisCombine(List<EngineRpmDistribution_Distance> orlist,string vehicleid)
        {
            if (orlist != null)
            {
                var DListperHalfHour = orlist.GroupBy(x => new
                {

                    x.Datadate.Value.Year,
                    x.Datadate.Value.Month,
                    x.Datadate.Value.Day,
                    x.Datadate.Value.Hour,
                    x.Datadate.Value.Minute
                }
               ).Select(x => new
               {
                   Id = x.Min(a => a.Id),
                   VehicleID = vehicleid,
                   Datadate = x.Min(a => a.Datadate),


                   _01000 = x.Sum(a => a._01000),
                   _10001500 = x.Sum(a => a._10001500),
                   _15002000 = x.Sum(a => a._15002000),
                   _20002500 = x.Sum(a => a._20002500),
                   _25003000 = x.Sum(a => a._25003000),
                   _30004000 = x.Sum(a => a._30004000),
                   _40005000 = x.Sum(a => a._40005000),
                   _50006000 = x.Sum(a => a._50006000),
                   _Above6000 = x.Sum(a => a._Above6000),
                 


               }).OrderBy(b => b.Datadate).ToList();

               
                List<EngineRpmDistribution_Distance> DListperHalfHourConvert = DListperHalfHour.Select(a => new EngineRpmDistribution_Distance
                {
                    Id = a.Id,
                    VehicleId = a.VehicleID,
                    Datadate = a.Datadate,
                    _01000 = a._01000,
                    _10001500 = a._10001500,
                    _15002000 = a._15002000,
                    _20002500 = a._20002500,
                    _25003000 = a._25003000,
                    _30004000 = a._30004000,
                    _40005000 = a._40005000,
                    _50006000 = a._50006000,
                    _Above6000 = a._Above6000,
                 

                }).ToList();

                
                return DListperHalfHourConvert;
            }
            else
            {
                return null;
            }

        }
    }
}
