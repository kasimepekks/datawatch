using MysqlforDataWatch;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static Tools.FileOperation.CSVFileImport;
using Tools.MyConfig;

namespace IDAL.SH_ADF0979IDAL
{
  public  interface IBumpDistribution_IDAL : IBaseDAL<Bumprecognition>
    {
        string JudgeandMergeBmpDataperHalfHour(FileInfo file, LoadCSVDataStructforImport importstruct, ref List<Bumprecognition> blist, string vehicleid, in VehicleIDPara vehicleIDPara);

    }
}
