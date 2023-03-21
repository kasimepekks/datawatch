using MysqlforDataWatch;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tools.FileOperation;
using static Tools.FileOperator;

namespace IDAL.SH_ADF0979IDAL
{
    public interface IRealTimeI_ACC_IDAL : IBaseDAL<RealtimeTempdataAcc>
    {
         Task<CsvFileReturnAllListV2> ReadCSVFiletoList(string filefullpath, string filename, string csvcollumnname, byte monitortimes,int intertime);
    }
}
