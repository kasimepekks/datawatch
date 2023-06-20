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
   public interface IEngineSpeedDisDistribution_IBLL : IBaseIBLL<EngineRpmDistribution_Distance>
    {
        Task<IQueryable> LoadEngspdDisDistribution(DateTime sd, DateTime ed, string vehicleid);
    
        string JudgeandMergeEngSpdDisDataperHalfHour(FileInfo file, LoadCSVDataStructforImport importstruct, ref List<EngineRpmDistribution_Distance> blist, string vehicleid, in VehicleIDPara vehicleIDPara);


    }
}
