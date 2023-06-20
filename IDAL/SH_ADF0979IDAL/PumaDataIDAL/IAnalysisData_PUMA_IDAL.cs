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
   public interface IAnalysisData_PUMA_IDAL : IBaseDAL<Pumapermileage>
    {
        string JudgeandMergeMileageData(FileInfo file, LoadCSVDataStructforMileageImport importstruct, ref List<Pumapermileage> blist, string vehicleid, in VehicleIDPara vehicleIDPara);
        //bool ReadFilesForAnalysisDataAcc(string filepath);
        //string JudgeandMergeStatisticDataperHalfHour(FileInfo file, LoadCSVDataStructforImport importstruct, ref List<SatictisAnalysisdataAcc> _SatictisAnalysisdataAccList, string vehicleid, in VehicleIDPara vehicleIDPara);
    }
}
