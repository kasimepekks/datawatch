using MysqlforDataWatch;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Tools.FileOperation.CSVFileImport;
using Tools.MyConfig;

namespace IBLL.SH_ADF0979IBLL
{
   public interface IGPSRecord_IBLL : IBaseIBLL<Gpsrecord>
    {
        Task<List<Gpsrecord>> LoadGPSRecord(DateTime sd, DateTime ed, string vehicleid, int reducetimes);
        string JudgeandMergeGPSDataperHalfHour(FileInfo file, LoadCSVDataStructforImport importstruct, ref List<Gpsrecord> blist, string vehicleid, in VehicleIDPara vehicleIDPara);
        Task<int> CreateTable(string vehicle);
        int TruncateTable(string vehicle);
    }
}
