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
   public interface IThrottleDistribution_IBLL : IBaseIBLL<Throttlerecognition>
    {
        Task<IQueryable> LoadThrottleDistribution(DateTime sd, DateTime ed, string vehicleid);
        Task<int> GetThrottleCount(DateTime sd, DateTime ed, string vehicleid);
        string JudgeandMergeThrottleDataperHalfHour(FileInfo file, LoadCSVDataStructforImport importstruct, ref List<Throttlerecognition> blist, string vehicleid, in VehicleIDPara vehicleIDPara);
    }
}
