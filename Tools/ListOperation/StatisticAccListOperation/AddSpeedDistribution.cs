using MysqlforDataWatch;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tools.AddDistance;
using Tools.MyConfig;

namespace Tools.ListOperation.StatisticAccListOperation
{
   public static class AddSpeedDistribution
    {
        /// <summary>
        /// 添加速度里程分布数据库数据
        /// </summary>
        /// <param name="speed"></param>
        /// <param name="l0_Time"></param>
        /// <param name="vehicleid"></param>
        /// <param name="name"></param>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static List<Speeddistribution> addspeedlist(List<double> speed, List<double> l0_Time, string vehicleid,string name,string datetime)
        {
            
            List<Speeddistribution> speedsqllist = new List<Speeddistribution>();

            var singledistance = AddDistanceList.ReturnSingleDistance(speed, l0_Time);
            var speeddistribution = SpeedDistribution.CalSpeedDistribution(speed, singledistance);

            Speeddistribution entity = new Speeddistribution();
            entity.Id = vehicleid + "-" + name + "-Distribution";
            entity.VehicleId = vehicleid;
            entity.Datadate = Convert.ToDateTime(datetime);

            //entity.Datatime = TimeSpan.Parse(time);
            entity._010 = speeddistribution[0];
            entity._1020 = speeddistribution[1];
            entity._2030 = speeddistribution[2];
            entity._3040 = speeddistribution[3];
            entity._4050 = speeddistribution[4];
            entity._5060 = speeddistribution[5];
            entity._6070 = speeddistribution[6];
            entity._7080 = speeddistribution[7];
            entity._8090 = speeddistribution[8];
            entity._90100 = speeddistribution[9];
            entity._100110 = speeddistribution[10];
            entity._110120 = speeddistribution[11];
            entity.Above120 = speeddistribution[12];
            speedsqllist.Add(entity);

            return speedsqllist;
        }
    }
}
