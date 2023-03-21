using MysqlforDataWatch;
using System;
using System.Collections.Generic;
using System.Text;
using Tools.MyConfig;

namespace Tools.ListOperation.BumpListOperation
{
   public static class AddBump
    {
        public static List<Bumprecognition> addbumplist(List<double> l2_AccZWhlLF, List<double> l3_AccZWhlRF, List<double> l4_AccZWhlLR, List<double> l1_VehSpdAvg,  string vehicleid, string name, string datetime,  in VehicleIDPara vehicleIDPara)
        {

            List<Bumprecognition> bumpsqllist = new List<Bumprecognition>();
            if (vehicleIDPara.BumpImport == 1)
            {

                BumpZero.DoZero(l2_AccZWhlLF, l3_AccZWhlRF, l4_AccZWhlLR, in vehicleIDPara);
                PeakSelect.GetPeak(l2_AccZWhlLF, l3_AccZWhlRF, l4_AccZWhlLR, l1_VehSpdAvg,in vehicleIDPara, out List<double> WFT_AZ_LFPeakList, out List<double> WFT_AZ_RFPeakList, out List<double> WFT_AZ_LRPeakList, out List<int> WFT_AZ_LFPeakTimeList, out List<int> WFT_AZ_RFPeakTimeList, out List<int> WFT_AZ_LRPeakTimeList, out List<double> speedpeaklist);

                var bumpnocombinelist = BumpReconize.GetBump(WFT_AZ_LFPeakList, WFT_AZ_RFPeakList, WFT_AZ_LRPeakList, WFT_AZ_LFPeakTimeList, WFT_AZ_RFPeakTimeList, WFT_AZ_LRPeakTimeList, speedpeaklist, l1_VehSpdAvg, in vehicleIDPara, out List<int> OuttimeListnocombine);
                if (bumpnocombinelist.Count > 0)
                {
                    var bumpcombinedlist = CombineList.CombineListMethod(bumpnocombinelist, OuttimeListnocombine, in vehicleIDPara, out List<int> OuttimeList);

                    //int n =1;
                    for (int i = 0; i < bumpcombinedlist.Count; i++)
                    {
                        Bumprecognition bump = new Bumprecognition();
                        bump.Id = vehicleid + "-" + name + "-Bump-" + i;

                        bump.VehicleId = vehicleid;
                        bump.Datadate = Convert.ToDateTime(datetime);
                        bump.Filename = name;
                        bump.BumpAcc = bumpcombinedlist[i];
                        bumpsqllist.Add(bump);
                    }

                }
            }

            return bumpsqllist;

        }
    }
}
