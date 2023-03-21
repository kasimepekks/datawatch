using MysqlforDataWatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Tools.FileOperation.CSVFileImport;
using Tools.MyConfig;
using System.IO;

namespace IBLL.SH_ADF0979IBLL
{
   public interface IBrakeDistribution_IBLL : IBaseIBLL<Brakerecognition>
    {
        Task<int> GetBrakeCount(DateTime sd, DateTime ed, string vehicleid);
        Task<List<double>> LoadBrakeDistribution(DateTime sd, DateTime ed, string vehicleid);
        string JudgeandMergeBrkDataperHalfHour(FileInfo file, LoadCSVDataStructforImport importstruct, ref List<Brakerecognition> _SatictisAnalysisdataAccList, string vehicleid, in VehicleIDPara vehicleIDPara);
    }
}
