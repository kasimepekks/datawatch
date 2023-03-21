using MysqlforDataWatch;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static Tools.FileOperation.CSVFileImport;
using Tools.MyConfig;
using System.Threading.Tasks;
using System.Linq;

namespace IDAL.SH_ADF0979IDAL
{
    public interface IGPSRecord_IDAL : IBaseDAL<Gpsrecord>
    {
        string JudgeandMergeGPSDataperHalfHour(FileInfo file, LoadCSVDataStructforImport importstruct, ref List<Gpsrecord> blist, string vehicleid, in VehicleIDPara vehicleIDPara);
        Task<int> CreateTable(string vehicle);
        int TruncateTable(string vehicle);
    }
}
