using MysqlforDataWatch;
using System;
using System.Collections.Generic;
using System.Text;
using Tools.AddDistance;

namespace Tools.ListOperation.EngineSpeedListOperation
{
    public class AddEngineSpeedDistanceDistribution
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
        public static List<EngineRpmDistribution_Distance> addEngSpdDis(List<double> speed,List<double> engspd, List<double> time, string vehicleid, string name, string datetime)
        {

            List<EngineRpmDistribution_Distance> engspddissqllist = new List<EngineRpmDistribution_Distance>();

            var singledistance = AddDistanceList.ReturnSingleDistance(speed, time);
            var engspddistribution = EngineSpeedDistanceDistribution.CalEngSpdDistribution(engspd, singledistance);

            EngineRpmDistribution_Distance entity = new EngineRpmDistribution_Distance();
            entity.Id = vehicleid + "-" + name + "-EngSpdDisDistribution";
            entity.VehicleId = vehicleid;
            entity.Datadate = Convert.ToDateTime(datetime);

            //entity.Datatime = TimeSpan.Parse(time);
            entity._01000 = engspddistribution[0];
            entity._10001500 = engspddistribution[1];
            entity._15002000 = engspddistribution[2];
            entity._20002500 = engspddistribution[3];
            entity._25003000 = engspddistribution[4];
            entity._30004000 = engspddistribution[5];
            entity._40005000 = engspddistribution[6];
            entity._50006000 = engspddistribution[7];
            entity._Above6000 = engspddistribution[8];

            engspddissqllist.Add(entity);

            return engspddissqllist;
        }

    }
}
