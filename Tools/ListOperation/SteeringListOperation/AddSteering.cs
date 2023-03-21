using MysqlforDataWatch;
using System;
using System.Collections.Generic;
using System.Text;
using Tools.MyConfig;

namespace Tools.ListOperation.SteeringListOperation
{
   public static class AddSteering
    {
        public static List<Streeringrecognition> addsteeringlist(List<double> l9_StrgWhlAng, List<double> l10_StrgWhlAngGr,  List<double> l1_VehSpdAvg, List<double> l6_AccYSTLF, string vehicleid, string name, string datetime, in VehicleIDPara vehicleIDPara)
        {

            List<Streeringrecognition> steeringsqllist = new List<Streeringrecognition>();
            if (vehicleIDPara.SteeringImport == 1)
            {

                SteeringZero.DoZero(l9_StrgWhlAng, in vehicleIDPara);

                var SteeringAccList = SteeringReconize.GetSteering(l9_StrgWhlAng, l10_StrgWhlAngGr, l1_VehSpdAvg, l6_AccYSTLF, in vehicleIDPara);
                if (SteeringAccList.Count > 0)
                {
                    for (int i = 0; i < SteeringAccList.Count; i++)
                    {
                        Streeringrecognition steering = new Streeringrecognition();
                        steering.Id = vehicleid + "-" + name + "-Steering-" + i;
                        steering.VehicleId = vehicleid;
                        steering.Datadate = Convert.ToDateTime(datetime);
                        steering.Filename = name;
                        steering.SteeringAcc = SteeringAccList[i].SteeringStrenth;
                        steering.StrgWhlAng = SteeringAccList[i].Angle;
                        steering.Speed = SteeringAccList[i].Speed;
                        steering.AngularAcc = SteeringAccList[i].AngularAcc;
                        //判断转向方向
                        steering.SteeringDirection = (sbyte)(SteeringAccList[i].Angle > 0 ? 1 : -1);
                        steeringsqllist.Add(steering);

                    }

                }

            }
            return steeringsqllist;
        }
    }
}
