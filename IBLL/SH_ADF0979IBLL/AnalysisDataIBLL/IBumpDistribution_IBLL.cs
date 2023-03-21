using MysqlforDataWatch;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using static Tools.FileOperation.CSVFileImport;
using Tools.MyConfig;

namespace IBLL.SH_ADF0979IBLL
{
   public interface IBumpDistribution_IBLL : IBaseIBLL<Bumprecognition>
    {
        Task<List<double>> LoadBumpDistribution(DateTime sd, DateTime ed, string vehicleid);
        Task<int> GetBumpCount(DateTime sd, DateTime ed, string vehicleid);
        string JudgeandMergeBmpDataperHalfHour(FileInfo file, LoadCSVDataStructforImport importstruct, ref List<Bumprecognition> blist, string vehicleid, in VehicleIDPara vehicleIDPara);
    }
}
