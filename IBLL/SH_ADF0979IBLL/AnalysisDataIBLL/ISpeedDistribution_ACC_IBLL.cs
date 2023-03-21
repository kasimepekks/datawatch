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
   public interface ISpeedDistribution_ACC_IBLL : IBaseIBLL<Speeddistribution>
    {
        //void ReadandMergeSpeedDistributionAcc(string filepath);
        Task<IQueryable> LoadSpeedDistribution(DateTime sd, DateTime ed, string vehicleid);
        Task<IQueryable> LoadSpeedDistributionperday(DateTime sd, DateTime ed, string vehicleid);
        Task<IQueryable> LoadSpeedDistributionperhour(DateTime sd, DateTime ed, string vehicleid);
        Task<List<double>> LoadTextRecord(DateTime sd, DateTime ed, string vehicleid);
        string JudgeandMergeSpdDataperHalfHour(FileInfo file, LoadCSVDataStructforImport importstruct, ref List<Speeddistribution> blist, string vehicleid, in VehicleIDPara vehicleIDPara);


    }
}
