using MysqlforDataWatch;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static Tools.FileOperation.CSVFileImport;
using Tools.MyConfig;

namespace IDAL.SH_ADF0979IDAL
{
   public interface IEngineSpeedDisDistribution_IDAL : IBaseDAL<EngineRpmDistribution_Distance>
    {
        string JudgeandMergeEngSpdDisDataperHalfHour(FileInfo file, LoadCSVDataStructforImport importstruct, ref List<EngineRpmDistribution_Distance> blist, string vehicleid, in VehicleIDPara vehicleIDPara);

    }
}
