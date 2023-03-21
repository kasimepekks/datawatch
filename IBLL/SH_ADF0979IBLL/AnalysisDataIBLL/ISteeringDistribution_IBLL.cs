﻿using MysqlforDataWatch;
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
   public interface ISteeringDistribution_IBLL : IBaseIBLL<Streeringrecognition>
    {
        Task<IQueryable> LoadSteeringDistribution(DateTime sd, DateTime ed, string vehicleid);
        Task<int> GetSteeringCount(DateTime sd, DateTime ed, string vehicleid);
        string JudgeandMergeSteeringDataperHalfHour(FileInfo file, LoadCSVDataStructforImport importstruct, ref List<Streeringrecognition> blist, string vehicleid, in VehicleIDPara vehicleIDPara);
    }
}
