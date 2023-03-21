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
    public interface IAnalysisData_WFT_IDAL : IBaseDAL<SatictisAnalysisdataWft>
    {
        Task<bool> ReadandMergeWFTDataperHalfHour(string filepath, string vehicleid, VehicleIDPara vehicleIDPara);
        string JudgeandMergeWFTDataperHalfHour(FileInfo file, LoadCSVDataStructforWFTImport importstruct, ref List<SatictisAnalysisdataWft> blist, string vehicleid, in VehicleIDPara vehicleIDPara);
    }
}
