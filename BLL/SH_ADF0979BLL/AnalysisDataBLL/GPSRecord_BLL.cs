using DAL.SH_ADF0979DAL;
using IBLL.SH_ADF0979IBLL;
using IDAL.SH_ADF0979IDAL;
using MysqlforDataWatch;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Tools.FileOperation.CSVFileImport;
using Tools.MyConfig;
using Microsoft.EntityFrameworkCore;

namespace BLL.SH_ADF0979BLL
{
    public class GPSRecord_BLL : BaseBLL<Gpsrecord>, IGPSRecord_IBLL
    {
        private readonly IGPSRecord_IDAL _GPSRecordDAL;
        //private readonly datawatchContext _DB;
        public GPSRecord_BLL(IGPSRecord_IDAL GPSRecordDAL)
        {
            this._GPSRecordDAL = GPSRecordDAL;
            base.CurrentDal = GPSRecordDAL;
            //_DB = DB;
        }
        public override void SetCurrentDal()
        {
            base.CurrentDal = this._GPSRecordDAL;
        }
        //返回GPS数据给前端，这里的reducetimes是指需要几个点到前端去，不是英文的意思
        public async Task<List<Gpsrecord>> LoadGPSRecord(DateTime sd, DateTime ed, string vehicleid,int reducetimes)
        {
            
             var gpsdistributionlist = await Task.Run(() => _GPSRecordDAL.LoadEntities(a => a.Datadate >= sd && a.Datadate <= ed && a.VehicleId == vehicleid, vehicleid).OrderBy(a=>a.Datadate).ThenBy(a=>a.Key));
            //这里必须要转成list，否则会有内存溢出的问题
            var list=gpsdistributionlist.ToList();
            var maxspeedrecord = list.OrderByDescending(a => a.Speed).FirstOrDefault();
            int listcount = 0;
            listcount = list.Count() > reducetimes ? list.Count() / reducetimes:  1;
         
            //int listcount = list.Count()/ reducetimes;

            List<Gpsrecord> newlist = new List<Gpsrecord>();

            for (int i = 0; i < list.Count()/ listcount; i++)
            {
                newlist.Add(list.Skip(i * listcount).Take(1).FirstOrDefault());

            }
            newlist.Add(maxspeedrecord);
            return newlist;
        }

        public string JudgeandMergeGPSDataperHalfHour(FileInfo file, LoadCSVDataStructforImport importstruct, ref List<Gpsrecord> blist, string vehicleid, in VehicleIDPara vehicleIDPara)
        {
            return _GPSRecordDAL.JudgeandMergeGPSDataperHalfHour(file, importstruct, ref blist, vehicleid, in vehicleIDPara);

        }
        public int TruncateTable(string vehicle)
        {
            return  _GPSRecordDAL.TruncateTable(vehicle); 
        }
        public async Task<int> CreateTable(string vehicle)
        {
            return await _GPSRecordDAL.CreateTable(vehicle);
        }
    }
}
