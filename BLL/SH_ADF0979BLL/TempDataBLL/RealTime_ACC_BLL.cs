﻿using IBLL.SH_ADF0979IBLL;
using IDAL.SH_ADF0979IDAL;
using Microsoft.EntityFrameworkCore;
using MysqlforDataWatch;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools.FileOperation;
using static Tools.FileOperator;

namespace BLL.SH_ADF0979BLL
{
   public class RealTime_ACC_BLL:BaseBLL<RealtimeTempdataAcc>, IRealTime_ACC_IBLL
    {
        private IRealTimeI_ACC_IDAL _RealTime_ACC_Dal { get; set; }
        //private readonly datawatchContext _DB;
        public RealTime_ACC_BLL(IRealTimeI_ACC_IDAL RealTime_ACC_Dal)
        {
            this._RealTime_ACC_Dal = RealTime_ACC_Dal;
            base.CurrentDal = RealTime_ACC_Dal;
          
        }

        public override void SetCurrentDal()
        {
            //base.CurrentDal = this._RealTime_ACC_Dal;
        }
        public async Task<CsvFileReturnAllListV2> ReadCSVFiletoList(string filefullpath, string filename, string csvcollumnname,byte monitortimes,int intertime)
        {
            return await _RealTime_ACC_Dal.ReadCSVFiletoList(filefullpath, filename, csvcollumnname, monitortimes, intertime);
        }



    }
}
