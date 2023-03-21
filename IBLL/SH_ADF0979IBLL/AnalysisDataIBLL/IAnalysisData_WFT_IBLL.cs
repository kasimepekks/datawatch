using MysqlforDataWatch;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools.MyConfig;
using static Tools.FileOperation.CSVFileImport;

namespace IBLL.SH_ADF0979IBLL
{
   public interface IAnalysisData_WFT_IBLL : IBaseIBLL<SatictisAnalysisdataWft>
    {
        Task<bool> ReadandMergeWFTDataperHalfHour(string filepath, string vehicleid, VehicleIDPara vehicleIDPara);
        string JudgeandMergeWFTDataperHalfHour(FileInfo file, LoadCSVDataStructforWFTImport importstruct, ref List<SatictisAnalysisdataWft> blist, string vehicleid, in VehicleIDPara vehicleIDPara);
        Task<IQueryable> LoadWFTDamage(DateTime sd, DateTime ed,string vehicleid);
        Task<IQueryable> LoadWFTDamageCumulation(DateTime sd, DateTime ed, string vehicleid);
    }
}
