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
   public interface IEngineSpeedTimeDistribution_IBLL : IBaseIBLL<EngineRpmDistribution_Time>
    {
        Task<IQueryable> LoadEngspdTimeDistribution(DateTime sd, DateTime ed, string vehicleid);
      
        string JudgeandMergeEngSpdTimeDataperHalfHour(FileInfo file, LoadCSVDataStructforImport importstruct, ref List<EngineRpmDistribution_Time> blist, string vehicleid, in VehicleIDPara vehicleIDPara);


    }
}
