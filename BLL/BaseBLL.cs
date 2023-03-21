using IDAL;
using Microsoft.EntityFrameworkCore;
using MysqlforDataWatch;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Tools.FileOperation;
using Tools.MyConfig;

namespace BLL
{
    public abstract class BaseBLL<T> where T : class, new()
    {
        
        public IBaseDAL<T> CurrentDal { get; set; }
        public abstract void SetCurrentDal();
        public BaseBLL()
        {
                       
            SetCurrentDal();//子类一定要实现抽象方法，一旦创建抽象子类的实例时，就会调用此方法
        }

        public IQueryable<T> LoadEntities
         (System.Linq.Expressions.Expression<Func<T, bool>> whereLambda, string vehicle)
        {
            return CurrentDal.LoadEntities(whereLambda,vehicle);
        }

        public bool DeleteEntity(T entity)
        {
            CurrentDal.DeleteEntity(entity);
            return CurrentDal.SaveChanges();
        }
        public bool DeleteAllEntity(System.Linq.Expressions.Expression<Func<T, bool>> whereLambda, string vehicle)
        {
            CurrentDal.DeleteAllEntity(whereLambda,vehicle);
            return CurrentDal.SaveChanges();
        }
        public bool EditEntity(T entity)
        {
            CurrentDal.EditEntity(entity);
            return CurrentDal.SaveChanges();
        }
        public bool AddEntity(T entity)
        {
            CurrentDal.AddEntity(entity);

            return CurrentDal.SaveChanges();
        }
        public void AddAllEntity(IEnumerable<T> entity, string vehicle)
        {
            CurrentDal.AddAllEntity(entity,vehicle);
        }
        public async Task<bool> IsSqlDataExitBase(FileInfo file, VehicleIDPara vehicleIDPara, string vehicle)
        {
            return await CurrentDal.IsSqlDataExitBase(file, vehicleIDPara, vehicle);
        }
        public async Task<CsvFileReturnAllList<T>> ReadCSVFileAll(string filefullpath, string filename,byte monitortimes)
        {
            return await CurrentDal.ReadCSVFileAll(filefullpath, filename,monitortimes);
        }

    }
}
