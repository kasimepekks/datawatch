using MysqlforDataWatch;
using System;
using System.Collections.Generic;
using System.Text;
using Tools.AddTime;

namespace Tools.ListOperation.EngineSpeedListOperation
{
    public class AddEngineSpeedTimeDistribution
    {
        /// <summary>
        ///  添加转速里程分布数据库数据
        /// </summary>
        /// <param name="speed"></param>
        /// <param name="engspd"></param>
        /// <param name="time"></param>
        /// <param name="vehicleid"></param>
        /// <param name="name"></param>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static List<EngineRpmDistribution_Time> addEngSpdTime(List<double> speed,List<double> engspd, List<double> time, string vehicleid, string name, string datetime)
        {

            List<EngineRpmDistribution_Time> engspdtimesqllist = new List<EngineRpmDistribution_Time>();

      
            var engspdtimedistribution = EngineSpeedTimeDistribution.CalEngSpdTimeDistribution(engspd);

            EngineRpmDistribution_Time entity = new EngineRpmDistribution_Time();
            entity.Id = vehicleid + "-" + name + "-EngSpdTimeDistribution";
            entity.VehicleId = vehicleid;
            entity.Datadate = Convert.ToDateTime(datetime);

            //entity.Datatime = TimeSpan.Parse(time);
            entity._01000 = engspdtimedistribution[0];
            entity._10001500 = engspdtimedistribution[1];
            entity._15002000 = engspdtimedistribution[2];
            entity._20002500 = engspdtimedistribution[3];
            entity._25003000 = engspdtimedistribution[4];
            entity._30004000 = engspdtimedistribution[5];
            entity._40005000 = engspdtimedistribution[6];
            entity._50006000 = engspdtimedistribution[7];
            entity._Above6000 = engspdtimedistribution[8];

            engspdtimesqllist.Add(entity);

            return engspdtimesqllist;
        }

    }
}
