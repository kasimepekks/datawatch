using MysqlforDataWatch;
using RainFlowandDamageTool.ComputingProcess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tools.AddDistance;
using static Tools.FileOperation.CSVFileImport;

namespace Tools.ListOperation.PUMAMileageOperation
{
    public static class PumaMileage
    {
        public static List<Pumapermileage> addPumaMileage(LoadCSVDataStructforMileageImport importstruct,string vehicle)
        {
            List<Pumapermileage> list = new List<Pumapermileage>();
            if (!importstruct.Equals(null))
            {
                Pumapermileage data = new Pumapermileage();
                data.vehicle = vehicle;
                data.filename = importstruct.name;
                data.filedate = Convert.ToDateTime(importstruct.datetime);
                data.duration = importstruct.lists[0].Max();
                var singledistance=AddDistanceList.ReturnSingleDistance(importstruct.lists[1], importstruct.lists[0]);
                data.mileage = singledistance.Sum();
                data.averagespeed = importstruct.lists[1].Average();//速度均值
                data.maxthrottle=importstruct.lists[2].Max();
                data.maxbrake = importstruct.lists[3].Max();
                data.maxangle = importstruct.lists[4].Max();
                data.minangle = importstruct.lists[4].Min();
                data.wftfxlfk3 = (decimal)importstruct.lists[5].GetAccumDamagefromList(3, 3);
                data.wftfxrfk3 = (decimal)importstruct.lists[6].GetAccumDamagefromList(3, 3);
                data.wftfxlrk3 = (decimal)importstruct.lists[7].GetAccumDamagefromList(3, 3);
                data.wftfxrrk3 = (decimal)importstruct.lists[8].GetAccumDamagefromList(3, 3);
                data.wftfylfk3 = (decimal)importstruct.lists[9].GetAccumDamagefromList(3, 3);
                data.wftfyrfk3 = (decimal)importstruct.lists[10].GetAccumDamagefromList(3, 3);
                data.wftfylrk3 = (decimal)importstruct.lists[11].GetAccumDamagefromList(3, 3);
                data.wftfyrrk3 = (decimal)importstruct.lists[12].GetAccumDamagefromList(3, 3);
                data.wftfzlfk3 = (decimal)importstruct.lists[13].GetAccumDamagefromList(3, 3);
                data.wftfzrfk3 = (decimal)importstruct.lists[14].GetAccumDamagefromList(3, 3);
                data.wftfzlrk3 = (decimal)importstruct.lists[15].GetAccumDamagefromList(3, 3);
                data.wftfzrrk3 = (decimal)importstruct.lists[16].GetAccumDamagefromList(3, 3);

                data.wftfxlfk5 = (decimal)importstruct.lists[5].GetAccumDamagefromList(5, 5);
                data.wftfxrfk5 = (decimal)importstruct.lists[6].GetAccumDamagefromList(5, 5);
                data.wftfxlrk5 = (decimal)importstruct.lists[7].GetAccumDamagefromList(5, 5);
                data.wftfxrrk5 = (decimal)importstruct.lists[8].GetAccumDamagefromList(5, 5);
                data.wftfylfk5 = (decimal)importstruct.lists[9].GetAccumDamagefromList(5, 5);
                data.wftfyrfk5 = (decimal)importstruct.lists[10].GetAccumDamagefromList(5, 5);
                data.wftfylrk5 = (decimal)importstruct.lists[11].GetAccumDamagefromList(5, 5);
                data.wftfyrrk5 = (decimal)importstruct.lists[12].GetAccumDamagefromList(5, 5);
                data.wftfzlfk5 = (decimal)importstruct.lists[13].GetAccumDamagefromList(5, 5);
                data.wftfzrfk5 = (decimal)importstruct.lists[14].GetAccumDamagefromList(5, 5);
                data.wftfzlrk5 = (decimal)importstruct.lists[15].GetAccumDamagefromList(5, 5);
                data.wftfzrrk5 = (decimal)importstruct.lists[16].GetAccumDamagefromList(5, 5);
                list.Add(data);
            }
            return list;
        }
    }
}
