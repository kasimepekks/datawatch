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
   public interface IAnalysisData_ACC_IBLL : IBaseIBLL<SatictisAnalysisdataAcc>
    {
        Task<IQueryable> LoadACCandDisData(DateTime sd, DateTime ed, string vehicleid);
        string JudgeandMergeStatisticDataperHalfHour(FileInfo file, LoadCSVDataStructforImport importstruct, ref List<SatictisAnalysisdataAcc> _SatictisAnalysisdataAccList, string vehicleid, in VehicleIDPara vehicleIDPara);
    }
}
