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
   public interface IAnalysisData_PUMA_IBLL : IBaseIBLL<Pumapermileage>
    {
        string JudgeandMergeMileageData(FileInfo file, LoadCSVDataStructforMileageImport importstruct, ref List<Pumapermileage> blist, string vehicleid, in VehicleIDPara vehicleIDPara);
        //Task<IQueryable> LoadACCandDisData(DateTime sd, DateTime ed, string vehicleid);
        //string JudgeandMergeStatisticDataperHalfHour(FileInfo file, LoadCSVDataStructforImport importstruct, ref List<Pumapermileage> _SatictisAnalysisdataAccList, string vehicleid, in VehicleIDPara vehicleIDPara);
    }
}
