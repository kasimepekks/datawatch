using MysqlforDataWatch;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static Tools.FileOperation.CSVFileImport;
using Tools.MyConfig;

namespace IDAL.SH_ADF0979IDAL
{
   public interface ISpeedDistribution_ACC_IDAL : IBaseDAL<Speeddistribution>
    {
        string JudgeandMergeSpdDataperHalfHour(FileInfo file, LoadCSVDataStructforImport importstruct, ref List<Speeddistribution> blist, string vehicleid, in VehicleIDPara vehicleIDPara);

    }
}
