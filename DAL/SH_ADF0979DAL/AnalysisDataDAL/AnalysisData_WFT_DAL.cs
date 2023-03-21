﻿using IDAL.SH_ADF0979IDAL;
using Microsoft.EntityFrameworkCore;
using MysqlforDataWatch;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Tools;
using Tools.AddDamage;
using Tools.ListOperation.ThrottleListOperation;
using Tools.ListOperation.WFTListOperation;
using Tools.MyConfig;
using static Tools.FileOperation.CSVFileImport;

namespace DAL.SH_ADF0979DAL
{
    public class AnalysisData_WFT_DAL : BaseDAL<SatictisAnalysisdataWft>, IAnalysisData_WFT_IDAL
    {
        public AnalysisData_WFT_DAL(datawatchContext DB) : base(DB)
        {

        }
       //目前不用此方法
        public async Task<bool> ReadandMergeWFTDataperHalfHour(string filepath,string vehicleid, VehicleIDPara vehicleIDPara)
        {
            FileInfo[] filelist = FileOperator.Isfileexist(filepath);//获得指定文件下的所有csv文件
            Encoding encoding = Encoding.Default;
            bool can = false;
            await Task.Run(() => {

                if (filelist.Length > 0)
                {
                    List<SatictisAnalysisdataWft> List = new List<SatictisAnalysisdataWft>();
                    var wftmysqllist = _DB.Set<SatictisAnalysisdataWft>().Select(a => a.Id).ToList();

                    foreach (var file in filelist)
                    {
                        if (file.Length != 0)
                        {
                            string[] filestring = file.Name.Split('-');//把文件名每隔“-”拿出来放在这个string数组里
                            string date = filestring[0].Replace("_", "-");//date是拿出来的日期，如“2021-07-03”
                            string oldtime = filestring[1];//oldtime是拿出来的时间，如“10_07_03”
                            string[] timestring = oldtime.Split('_');//把小时，分钟，秒数放到这个数组里
                            string newminute = (int.Parse(timestring[1]) / 30 * 30).ToString();//把所有的分钟改为小于30分的都是0分，大于30分都是30分
                            string newtime = timestring[0] + ":" + newminute + ":" + "00";

                            string datetime = date + " " + newtime;
                            var datestart = Convert.ToDateTime(datetime);


                            string name = file.Name.Split('.')[0];


                            FileStream fs = new FileStream(file.FullName, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                            StreamReader sr = new StreamReader(fs, encoding);
                            DataTable dt = new DataTable();
                            string strLine = "";
                            //记录每行记录中的各字段内容
                            string[] aryLine = null;
                            string[] tableHead = null;
                            int resample = 0;//设置一个计数

                            //标示列数
                            int columnCount = 0;
                            //标示是否是读取的第一行
                            bool IsFirst = true;
                            //逐行读取CSV中的数
                            while ((strLine = sr.ReadLine()) != null)
                            {

                                if (IsFirst == true)
                                {
                                    tableHead = strLine.Split(',');
                                    IsFirst = false;
                                    columnCount = tableHead.Length;

                                    for (int i = 0; i < columnCount; i++)
                                    {
                                        var t = tableHead[i].Replace("_", "");
                                        tableHead[i] = t.Replace(" ", "");
                                    }

                                    //创建列
                                    for (int i = 0; i < columnCount; i++)
                                    {
                                        DataColumn dc = new DataColumn(tableHead[i]);
                                        dt.Columns.Add(dc);
                                    }
                                }
                                else
                                {
                                    resample += 1;//每读取一行就加1，直到加到采样率标准就读取数据
                                    //判断是否等于采样率标准
                                    if (resample == vehicleIDPara.Reductiontimesforwftimport)
                                    {
                                        aryLine = strLine.Split(',');
                                        DataRow dr = dt.NewRow();
                                        for (int j = 0; j < columnCount; j++)
                                        {
                                            dr[j] = aryLine[j];
                                        }
                                        dt.Rows.Add(dr);
                                        resample = 0;//重新计数
                                    }
                                      
                                }
                            }
                            sr.Close();
                            sr.Dispose();

                            fs.Close();
                            fs.Dispose();
                            if (aryLine != null && aryLine.Length > 0)
                            {
                                if (vehicleIDPara.WFTImport == 1)
                                {
                                    var wftlist = AddWFT.addwft(columnCount, tableHead, dt, vehicleid, name, datetime);
                                    if (wftlist .Count>0)
                                    {
                                        Console.WriteLine(file.Name+"里的WFT数据有"+wftlist.Count);
                                        List = List.Concat(wftlist).ToList();
                                    }
                                }
                               

                            }
                        }

                        else
                        {
                            continue;
                        }

                    }


                    var wftfinallist = WFTCombined.wftcombine(List, vehicleid);


                    if (vehicleIDPara.WFTImport == 1)
                    {
                        if (wftfinallist.Count>0)
                        {
                            _DB.BulkInsert(wftfinallist);
                        }
                        Console.WriteLine("WFT数据有" + wftfinallist.Count + "条");
                    }
                    else
                    {
                        Console.WriteLine("WFT没有开启导入");
                    }
                       
                    
                    _DB.SaveChanges();
                    can = true;

                }

            });
            

            return can;


        }

        public string JudgeandMergeWFTDataperHalfHour(FileInfo file, LoadCSVDataStructforWFTImport importstruct, ref List<SatictisAnalysisdataWft> blist, string vehicleid, in VehicleIDPara vehicleIDPara)
        {
            
            var time = Convert.ToDateTime(file.Name.Split('-')[0].Replace("_", "-"));
            if (vehicleIDPara.WFTImport == 1)
            {
                var sqllist = _DB.SatictisAnalysisdataWfts.Where(a => a.Datadate > time && a.Datadate < time.AddDays(1)).Select(a => a.VehicleId).AsNoTracking().FirstOrDefault();
                if (string.IsNullOrEmpty(sqllist))
                {
                    if (importstruct.Iscontinue)
                    {
                        var list = AddWFT.addwft(importstruct.columncount, importstruct.tablehead, importstruct.datatable, vehicleid, importstruct.name, importstruct.datetime);
                        if (list.Count > 0)
                        {
                            blist = blist.Concat(list).ToList();
                        }
                        return null;
                    }
                    else
                    {
                        return ($"此文件{file.Name}进行WFT计算时出现了问题，数据状态为{importstruct.status}！{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
                    }
                }
                else
                {
                    return ($"此文件{file.Name}进行WFT计算时已导入过或没开启导入权限，须删除后再重新导入！{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
                }
            }
            else
            {
                return null;
            }
        }

    }
}
