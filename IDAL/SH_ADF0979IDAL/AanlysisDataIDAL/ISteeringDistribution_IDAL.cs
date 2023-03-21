using MysqlforDataWatch;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static Tools.FileOperation.CSVFileImport;
using Tools.MyConfig;

namespace IDAL.SH_ADF0979IDAL
{
    public interface ISteeringDistribution_IDAL : IBaseDAL<Streeringrecognition>
    {
        string JudgeandMergeSteeringDataperHalfHour(FileInfo file, LoadCSVDataStructforImport importstruct, ref List<Streeringrecognition> blist, string vehicleid, in VehicleIDPara vehicleIDPara);

    }
}
