using MysqlforDataWatch;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static Tools.FileOperation.CSVFileImport;
using Tools.MyConfig;

namespace IDAL.SH_ADF0979IDAL
{
   public interface IEngineSpeedTimeDistribution_IDAL : IBaseDAL<EngineRpmDistribution_Time>
    {
        string JudgeandMergeEngSpdTimeDataperHalfHour(FileInfo file, LoadCSVDataStructforImport importstruct, ref List<EngineRpmDistribution_Time> blist, string vehicleid, in VehicleIDPara vehicleIDPara);

    }
}
