using MysqlforDataWatch;
using System;
using System.Collections.Generic;
using System.Text;
using static Tools.FileOperation.CSVFileImport;
using System.Threading.Tasks;
using Tools.MyConfig;
using System.IO;

namespace IDAL.SH_ADF0979IDAL
{
    public interface IBrakeDistribution_IDAL : IBaseDAL<Brakerecognition>
    {
        //Task<List<double>> LoadBrakeDistribution(DateTime sd, DateTime ed, string vehicleid);
        string JudgeandMergeBrkDataperHalfHour(FileInfo file, LoadCSVDataStructforImport importstruct, ref List<Brakerecognition> _SatictisAnalysisdataAccList, string vehicleid, in VehicleIDPara vehicleIDPara);

    }
}
