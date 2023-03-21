using MysqlforDataWatch;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Tools.FileOperation;
using Tools.MyConfig;

namespace IBLL
{
    public interface IBaseIBLL<T> where T : class, new()
    {

        IQueryable<T> LoadEntities
         (System.Linq.Expressions.Expression<Func<T, bool>> whereLambda, string vehicle);
        bool DeleteEntity(T entity);
        bool DeleteAllEntity(System.Linq.Expressions.Expression<Func<T, bool>> whereLambda, string vehicle);
        bool EditEntity(T entity);
        bool AddEntity(T entity);
        void AddAllEntity(IEnumerable<T> entity, string vehicle);
        Task<bool> IsSqlDataExitBase(FileInfo file, VehicleIDPara vehicleIDPara, string vehicle);
        Task<CsvFileReturnAllList<T>> ReadCSVFileAll(string filefullpath, string filename, byte monitortimes);

    }
}
