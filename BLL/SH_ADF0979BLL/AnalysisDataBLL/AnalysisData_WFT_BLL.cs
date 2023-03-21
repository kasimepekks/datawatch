﻿using DAL.SH_ADF0979DAL;
using IBLL.SH_ADF0979IBLL;
using IDAL.SH_ADF0979IDAL;
using Microsoft.EntityFrameworkCore;
using MysqlforDataWatch;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools.MyConfig;
using static Tools.FileOperation.CSVFileImport;

namespace BLL.SH_ADF0979BLL
{
    public class AnalysisData_WFT_BLL : BaseBLL<SatictisAnalysisdataWft>, IAnalysisData_WFT_IBLL
    {
        private readonly IAnalysisData_WFT_IDAL _AnalysisData_WFT_DAL;
        
        public AnalysisData_WFT_BLL(IAnalysisData_WFT_IDAL AnalysisData_WFT_DAL)
        {
            this._AnalysisData_WFT_DAL = AnalysisData_WFT_DAL;
            base.CurrentDal = AnalysisData_WFT_DAL;
            //_DB = DB;
        }
        public override void SetCurrentDal()
        {
            base.CurrentDal = this._AnalysisData_WFT_DAL;
        }
       
        public async Task<bool> ReadandMergeWFTDataperHalfHour(string filepath, string vehicleid, VehicleIDPara vehicleIDPara)
        {
            return await _AnalysisData_WFT_DAL.ReadandMergeWFTDataperHalfHour(filepath, vehicleid, vehicleIDPara);
        }

        public string JudgeandMergeWFTDataperHalfHour(FileInfo file, LoadCSVDataStructforWFTImport importstruct, ref List<SatictisAnalysisdataWft> blist, string vehicleid, in VehicleIDPara vehicleIDPara)
        {
            return _AnalysisData_WFT_DAL.JudgeandMergeWFTDataperHalfHour(file, importstruct, ref blist, vehicleid, in vehicleIDPara);
        }
        public async Task<IQueryable> LoadWFTDamage(DateTime sd, DateTime ed, string vehicleid)
        {
           
            var damagelist = await Task.Run(() => _AnalysisData_WFT_DAL.LoadEntities(a => a.Datadate >= sd && a.Datadate <= ed && a.VehicleId == vehicleid, vehicleid).GroupBy(x =>
                 x.Chantitle).Select(x => new
                 {
                     chantitle = x.Key,
                     damage = x.Sum(a => a.Damage),
                     max = x.Max(a => a.Max),
                     min = x.Min(a => a.Min)
                 }).Where(a => a.chantitle.Contains("WFTF")).ToList().OrderBy(b => b.chantitle));
            return damagelist.AsQueryable();
        }

        public async Task<IQueryable> LoadWFTDamageCumulation(DateTime sd, DateTime ed,string vehicleid)
        {
            //这里考虑按每一天进行求和损伤
            var damagelist = await Task.Run(() => _AnalysisData_WFT_DAL.LoadEntities(a => a.Datadate >= sd && a.Datadate <= ed.AddDays(1) && a.VehicleId == vehicleid, vehicleid).GroupBy(x => new
            {
                x.Chantitle,
                x.Datadate.Value.Year,
                x.Datadate.Value.Month,
                x.Datadate.Value.Day
            }
            ).Select(x => new
            {
                chantitle = x.Key.Chantitle,
                datetime = x.Key.Year.ToString() + "-" + x.Key.Month.ToString() + "-" + x.Key.Day.ToString(),
                damage = Math.Round((double)x.Sum(a => a.Damage), 0),

            }).Where(a => a.chantitle.Contains("WFTF")).ToList().OrderBy(b => b.chantitle).ThenBy(b => b.datetime));

            return damagelist.AsQueryable();
        }
    }
}
