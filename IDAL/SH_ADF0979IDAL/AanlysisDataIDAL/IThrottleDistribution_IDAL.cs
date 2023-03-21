using MysqlforDataWatch;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static Tools.FileOperation.CSVFileImport;
using Tools.MyConfig;

namespace IDAL.SH_ADF0979IDAL
{
    public interface IThrottleDistribution_IDAL : IBaseDAL<Throttlerecognition>
    {
        string JudgeandMergeThrottleDataperHalfHour(FileInfo file, LoadCSVDataStructforImport importstruct, ref List<Throttlerecognition> blist, string vehicleid, in VehicleIDPara vehicleIDPara);

    }
}
