using MysqlforDataWatch;
using System;
using System.Collections.Generic;
using System.Text;
using Tools.MyConfig;

namespace Tools.ListOperation.ThrottleListOperation
{
   public static class AddThrottle
    {
        public static List<Throttlerecognition> addthrottlelist(List<double> l8_EPTAccelActu, List<double> l5_AccXSTLF, List<double> l1_VehSpdAvg,   string vehicleid, string name, string datetime, in VehicleIDPara vehicleIDPara)
        {

            List<Throttlerecognition> throttlesqllist = new List<Throttlerecognition>();
            if (vehicleIDPara.ThrottleImport == 1)
            {
                ThrottleZero.DoZero(l8_EPTAccelActu, in vehicleIDPara);
                var throttleList = ThrottleReconize.GetThrottle(l8_EPTAccelActu, l5_AccXSTLF, l1_VehSpdAvg, in vehicleIDPara);
                if (throttleList.Count > 0)
                {


                    for (int i = 0; i < throttleList.Count; i++)
                    {
                        Throttlerecognition throttlerecognition = new Throttlerecognition();
                        throttlerecognition.Id = vehicleid + "-" + name + "-Throttle-" + i;
                        throttlerecognition.VehicleId = vehicleid;
                        throttlerecognition.Datadate = Convert.ToDateTime(datetime);
                        throttlerecognition.Filename = name;
                        throttlerecognition.Accelerograph = throttleList[i].throttle;
                        throttlerecognition.LastingTime = throttleList[i].lastingtime;
                        throttlerecognition.Speed = throttleList[i].Speed;
                        throttlerecognition.ThrottleAcc = throttleList[i].accxst;
                        throttlerecognition.Reverse = throttleList[i].direction;
                        throttlesqllist.Add(throttlerecognition);


                    }

                }

            }


            return throttlesqllist;
        }
    }
}
