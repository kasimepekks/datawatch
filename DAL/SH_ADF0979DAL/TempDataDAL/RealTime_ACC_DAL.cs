﻿using IDAL.SH_ADF0979IDAL;
using Microsoft.EntityFrameworkCore;
using MysqlforDataWatch;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools;



namespace DAL.SH_ADF0979
{
   public class RealTime_ACC_DAL : BaseDAL<RealtimeTempdataAcc>, IRealTimeI_ACC_IDAL
    {
        
        public RealTime_ACC_DAL(datawatchContext DB) :base(DB){
           
        }
        public async Task<FileOperator.CsvFileReturnAllListV2> ReadCSVFiletoList(string filefullpath, string filename, string csvcollumnname, byte monitortimes,int intertime)
        {
            return await FileOperator.ReadCSVFiletoList(filefullpath, filename, csvcollumnname, monitortimes,intertime);
        }
    }
}
