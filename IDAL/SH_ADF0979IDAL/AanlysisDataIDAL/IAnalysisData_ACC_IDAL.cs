using MysqlforDataWatch;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Tools.MyConfig;
using static Tools.FileOperation.CSVFileImport;

namespace IDAL.SH_ADF0979IDAL
{
   public interface IAnalysisData_ACC_IDAL : IBaseDAL<SatictisAnalysisdataAcc>
    {
        //bool ReadFilesForAnalysisDataAcc(string filepath);
        string JudgeandMergeStatisticDataperHalfHour(FileInfo file, LoadCSVDataStructforImport importstruct, ref List<SatictisAnalysisdataAcc> _SatictisAnalysisdataAccList, string vehicleid, in VehicleIDPara vehicleIDPara);
    }
}
