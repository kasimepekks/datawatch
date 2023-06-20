using DAL.SH_ADF0979DAL;
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
using static Tools.FileOperation.CSVFileImport;
using Tools.MyConfig;

namespace BLL.SH_ADF0979BLL
{
    public class EngineSpeedTimeDistribution_BLL : BaseBLL<EngineRpmDistribution_Time>, IEngineSpeedTimeDistribution_IBLL
    {
        private readonly IEngineSpeedTimeDistribution_IDAL _IEngineSpeedTimeDistribution_IDAL;

        //private readonly datawatchContext _DB;
        public EngineSpeedTimeDistribution_BLL(IEngineSpeedTimeDistribution_IDAL IEngineSpeedTimeDistribution_IDAL)
        {
            this._IEngineSpeedTimeDistribution_IDAL = IEngineSpeedTimeDistribution_IDAL;
            base.CurrentDal = IEngineSpeedTimeDistribution_IDAL;//构造函数把实例传入给basebll里的currentdal
            //_DB = DB;
        }
        public override void SetCurrentDal()
        {
            base.CurrentDal = this._IEngineSpeedTimeDistribution_IDAL;
        }

        public async Task<IQueryable> LoadEngspdTimeDistribution(DateTime sd, DateTime ed, string vehicleid)
        {

            var engspdtimedistributionlist = await Task.Run(() => _IEngineSpeedTimeDistribution_IDAL.LoadEntities(a => a.Datadate >= sd && a.Datadate <= ed && a.VehicleId == vehicleid, vehicleid).GroupBy(x => new { }).Select(q => new
            {
                Time = sd + "-" + ed,
                sum0_1000 = q.Sum(x => x._01000),
                sum1000_1500 = q.Sum(x => x._10001500),
                sum1500_2000 = q.Sum(x => x._15002000),
                sum2000_2500 = q.Sum(x => x._20002500),
                sum2500_3000 = q.Sum(x => x._25003000),
                sum3000_4000 = q.Sum(x => x._30004000),
                sum4000_5000 = q.Sum(x => x._40005000),
                sum5000_6000 = q.Sum(x => x._50006000),
                sumabove6000 = q.Sum(x => x._Above6000),


            }).ToList());

            return engspdtimedistributionlist.AsQueryable();


        }
        public string JudgeandMergeEngSpdTimeDataperHalfHour(FileInfo file, LoadCSVDataStructforImport importstruct, ref List<EngineRpmDistribution_Time> blist, string vehicleid, in VehicleIDPara vehicleIDPara)
        {
            return _IEngineSpeedTimeDistribution_IDAL.JudgeandMergeEngSpdTimeDataperHalfHour(file, importstruct, ref blist, vehicleid, in vehicleIDPara);
        }

    }
}
