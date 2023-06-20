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
    public class AnalysisData_PUMA_BLL : BaseBLL<Pumapermileage>, IAnalysisData_PUMA_IBLL
    {
        private readonly IAnalysisData_PUMA_IDAL _AnalysisData_PUMA_DAL;
        //private readonly DbContext _DB;
        public AnalysisData_PUMA_BLL(IAnalysisData_PUMA_IDAL AnalysisData_PUMA_DAL)
        {
            this._AnalysisData_PUMA_DAL = AnalysisData_PUMA_DAL;
            base.CurrentDal = AnalysisData_PUMA_DAL;//构造函数把实例传入给basebll里的currentdal

            
            //_DB = DB;
        }
        public override void SetCurrentDal()
        {
            //base.CurrentDal = this._AnalysisData_ACC_DAL;
        }


        public string JudgeandMergeMileageData(FileInfo file, LoadCSVDataStructforMileageImport importstruct, ref List<Pumapermileage> blist, string vehicleid, in VehicleIDPara vehicleIDPara)
        {
            return _AnalysisData_PUMA_DAL.JudgeandMergeMileageData(file, importstruct, ref blist, vehicleid, vehicleIDPara);
        }


    }
}
