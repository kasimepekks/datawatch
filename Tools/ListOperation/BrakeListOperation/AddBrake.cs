using MysqlforDataWatch;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tools.MyConfig;

namespace Tools.ListOperation.BrakeListOperation
{
   public static class AddBrake
    {
        public static List<Brakerecognition> addbrakelist(List<double> l7_BrkPdlDrvrAp, List<double> l5_AccXSTLF, List<double> l1_VehSpdAvg,string vehicleid,string name,string datetime,  in VehicleIDPara vehicleIDPara)
        {
            
            List<Brakerecognition> brakesqllist = new List<Brakerecognition>();
            if (vehicleIDPara.BrakeImport == 1)
            {

                BrakeZero.DoZero(l7_BrkPdlDrvrAp, in vehicleIDPara);
                var brakeacclist = BrakeReconize.GetBrake(l7_BrkPdlDrvrAp, l5_AccXSTLF, l1_VehSpdAvg, in vehicleIDPara);
                if (brakeacclist.Count > 0)
                {
                    for (int i = 0; i < brakeacclist.Count; i++)
                    {
                        Brakerecognition brake = new Brakerecognition();
                        brake.Id = vehicleid + "-" + name + "-Brake-" + i;
                        brake.VehicleId = vehicleid;
                        brake.Datadate = Convert.ToDateTime(datetime);
                        brake.Filename = name;
                        brake.BrakeAcc = brakeacclist[i];
                        brakesqllist.Add(brake);
                    }

                }
            }
            return brakesqllist;
        }
    }
}
