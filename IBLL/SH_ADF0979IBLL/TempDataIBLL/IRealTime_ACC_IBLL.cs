using MysqlforDataWatch;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tools.FileOperation;
using static Tools.FileOperator;

namespace IBLL.SH_ADF0979IBLL
{
   public interface IRealTime_ACC_IBLL:IBaseIBLL<RealtimeTempdataAcc>
    {
        Task<CsvFileReturnAllListV2> ReadCSVFiletoList(string filefullpath, string filename, string csvcollumnname,  byte monitortimes, int intertime);
   }
}
