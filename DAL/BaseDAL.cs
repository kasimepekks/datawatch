using Microsoft.EntityFrameworkCore;
using MysqlforDataWatch;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Tools.Cash;
using Tools.FileOperation;
using Tools.ListOperation.SteeringListOperation;
using Tools.MyConfig;

namespace DAL
{
    //此基类并没有IOC
    public class BaseDAL<T> where T : class, new()
    {
        public readonly datawatchContext _DB;
        public BaseDAL(datawatchContext db)
        {
            _DB = db;
        }
        public IQueryable<T> LoadEntities
          (System.Linq.Expressions.Expression<Func<T, bool>> whereLambda,string vehicle)
        {
            _DB.vehicleid = vehicle;
            return _DB.Set<T>().AsNoTracking().Where<T>(whereLambda);
        }
       
        public bool DeleteEntity(T entity)
        {
            _DB.Entry<T>(entity).State = EntityState.Deleted;
            return true;
            //return DB.SaveChanges() > 0;
        }
        /// <summary>
        /// 删除所选的数据
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        public bool DeleteAllEntity(System.Linq.Expressions.Expression<Func<T, bool>> whereLambda, string vehicle)
        {
            _DB.vehicleid = vehicle;
            var list=_DB.Set<T>().Where<T>(whereLambda);
            if (list.Count() > 0)
            {
                _DB.Set<T>().RemoveRange(list);
                return true;
            }
            else
            {
                return false;
            }
            
            
            //return DB.SaveChanges() > 0;
        }
        public bool EditEntity(T entity)
        {
            _DB.Entry<T>(entity).State = EntityState.Modified;
            return true;
            //return DB.SaveChanges() > 0;
        }
      
        public bool AddEntity(T entity)
        {
            _DB.Set<T>().Add(entity);
            //DB.SaveChanges();
            return true;
        }
        public void AddAllEntity(IEnumerable<T> entity,string vehicle)
        {
            _DB.vehicleid = vehicle;
            _DB.BulkInsert<T>(entity);
            
        }
        public bool SaveChanges()
        {
            return _DB.SaveChanges()>0;
         }

        /// <summary>
        /// 判断有么有打开某个工况分析的权限以及判断是否数据库里已经存在
        /// </summary>
        /// <param name="file"></param>
        /// <param name="vehicleIDPara"></param>
        /// <returns></returns>
        public async Task <bool> IsSqlDataExitBase(FileInfo file, VehicleIDPara vehicleIDPara,string vehicle)
        {
            bool t = true;
            Type type = typeof(T);
            var time=Convert.ToDateTime( file.Name.Split('-')[0].Replace("_","-"));
            if (type == typeof(Brakerecognition))
            {
                if (vehicleIDPara.BrakeImport == 1)
                {
                    await Task.Run(() => {
                       
                        var sqllist = _DB.Brakerecognitions.Where(a => a.Datadate> time && a.Datadate < time.AddDays(1)).Select(a => a.VehicleId).AsNoTracking().FirstOrDefault();
                        if (string.IsNullOrEmpty(sqllist))
                        {
                            t = false;
                        }
                        else
                        {
                            t = true;
                        }
                    });
                }
            }
            else if (type == typeof(Bumprecognition))
            {
                if (vehicleIDPara.BumpImport == 1)
                {
                    await Task.Run(() => {
                        var sqllist = _DB.Bumprecognitions.Where(a => a.Datadate > time && a.Datadate < time.AddDays(1)).Select(a => a.VehicleId).AsNoTracking().FirstOrDefault();
                        if (string.IsNullOrEmpty(sqllist))
                        {
                            t = false;
                        }
                        else
                        {
                            t = true;
                        }
                    });
                }
            }
            else if (type == typeof(Gpsrecord))
            {
                if (vehicleIDPara.GPSImport == 1)
                {
                    _DB.vehicleid = vehicle;
                    await Task.Run(() => {
                        var sqllist = _DB.Gpsrecords.Where(a => a.Datadate > time && a.Datadate < time.AddDays(1)).Select(a => a.VehicleId).AsNoTracking().FirstOrDefault();
                        if (string.IsNullOrEmpty(sqllist))
                        {
                            t = false;
                        }
                        else
                        {
                            t = true;
                        }
                    });
                }
            }
            else if (type == typeof(Speeddistribution))
            {
                if (vehicleIDPara.SpeedImport == 1)
                {
                    await Task.Run(() => {
                        var sqllist = _DB.Speeddistributions.Where(a => a.Datadate > time && a.Datadate < time.AddDays(1)).Select(a => a.VehicleId).AsNoTracking().FirstOrDefault();
                        if (string.IsNullOrEmpty(sqllist))
                        {
                            t = false;
                        }
                        else
                        {
                            t = true;
                        }
                    });
                }
            }
            else if (type == typeof(SteeringDistribute))
            {
                if (vehicleIDPara.SteeringImport == 1)
                {
                    await Task.Run(() => {
                        var sqllist = _DB.Streeringrecognitions.Where(a => a.Datadate > time && a.Datadate < time.AddDays(1)).Select(a => a.VehicleId).AsNoTracking().FirstOrDefault();
                        if (string.IsNullOrEmpty(sqllist))
                        {
                            t = false;
                        }
                        else
                        {
                            t = true;
                        }
                    });
                }
            }
            else if (type == typeof(Throttlerecognition))
            {
                if (vehicleIDPara.ThrottleImport == 1)
                {
                    await Task.Run(() => {
                        var sqllist = _DB.Throttlerecognitions.Where(a => a.Datadate > time && a.Datadate < time.AddDays(1)).Select(a => a.VehicleId).AsNoTracking().FirstOrDefault();
                        if (string.IsNullOrEmpty(sqllist))
                        {
                            t = false;
                        }
                        else
                        {
                            t = true;
                        }
                    });
                }
            }
            else if (type == typeof(SatictisAnalysisdataAcc))
            {
                if (vehicleIDPara.StatisticImport == 1)
                {
                    await Task.Run(() => {
                        var sqllist = _DB.SatictisAnalysisdataAccs.Where(a => a.Datadate > time && a.Datadate < time.AddDays(1)).Select(a => a.VehicleId).AsNoTracking().FirstOrDefault();
                        if (string.IsNullOrEmpty(sqllist))
                        {
                            t = false;
                        }
                        else
                        {
                            t = true;
                        }
                    });
                }
            }
            else if (type == typeof(SatictisAnalysisdataWft))
            {
                if (vehicleIDPara.WFTImport == 1)
                {
                    await Task.Run(() => {
                        var sqllist = _DB.SatictisAnalysisdataWfts.Where(a => a.Datadate > time && a.Datadate < time.AddDays(1)).Select(a => a.VehicleId).AsNoTracking().FirstOrDefault();
                        if (string.IsNullOrEmpty(sqllist))
                        {
                            t = false;
                        }
                        else
                        {
                            t = true;
                        }
                    });
                }
            }
            return t;
        }



        /// <summary>
        /// 已弃用。用一个方法读取csv文件里的时域数据及统计数据进行展示，不要分开用2个方法了
        /// </summary>
        /// <param name="filefullpath"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public async Task<CsvFileReturnAllList<T>> ReadCSVFileAll(string filefullpath, string filename, byte monitortimes)
        {
            return await CSVFileOperation<T>.ReadCSVFileAll(filefullpath, filename ,monitortimes);
        }
    }
}
