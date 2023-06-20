using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools.ErrorMessage;
using Tools.MyConfig;

namespace Tools.FileOperation
{
    public static class CSVFileImport
    {
        //这个list用于返回错误时确认是哪个通道有问题
        public static List<string> names = new List<string>()
        {
            "time",
            "spd",
             "lat",
             "lon",
             "acczwhllf",
             "acczwhlrf",
             "acczwhllr",
             "accxbody",
             "brakecolumnname",
             "throttlecolumnname",
             "accybody",
             "whlangcolumnname",
             "whlanggrcolumnname"

        };
        public static List<string> statusfail = new List<string>()
        {
            "正常",
            "文件被占用",
             "文件无数据",
             "time列有问题",
             "speed列有问题",
        };
        public static List<string> collumnstatusfail = new List<string>()
        {
            "正常",
            "无此数据列",
            "数据为nan",
             "数据超出极限",
        };

        public struct BaseCSVDataforImport
        {
            public byte status;//标识数据导入的状态参数，如为0则正常，为1则是文件被占用，为2则是csv文件无数据，为3则是time列有问题，为4则是spd列有问题
            public List<List<double>> lists;
            public string[] headers;
            public string name;
            public string datetime;
        }
        public struct LoadCSVDataStructforImport
        {
            public byte status;//标识数据导入的状态参数，如为0则正常，为1则是文件被占用，为2则是csv文件无数据，为3则是time列有问题，为4则是spd列有问题
            public byte collumnstatus;//标识数据列的状态参数，便于定位
            public List<List<double>> lists;
            public List<double> spd;
            public int columncount;
            public string[] tablehead;
            public string name;
            public string datetime;
            public List<bool> boolcan;
            public DataTable datatable;
            public bool Iscontinue;//判断计算程序是否继续
        }
        public struct LoadCSVDataStructforWFTImport
        {
            public byte status;//标识数据导入的状态参数，如为0则正常，为1则是文件被占用，为2则是csv文件无数据，3是数据有空行或者nan，4是数据不为数字
            public int columncount;
            public string[] tablehead;
            public string name;
            public string datetime;
            public DataTable datatable;
            public bool Iscontinue;//判断计算程序是否继续
        }
        public struct LoadCSVDataStructforMileageImport
        {
            public byte status;//标识数据导入的状态参数，如为0则正常，为1则是文件被占用，为2则是csv文件无数据，为3则是time列有问题，为4则是spd列有问题
            public byte collumnstatus;//标识数据列的状态参数，便于定位
            public List<List<double>> lists;
            public string datetime;
            public int columncount;
            public string[] tablehead;
            public string name;
           
            public List<bool> boolcan;
          
            public bool Iscontinue;//判断计算程序是否继续
        }
        /// <summary>
        /// 读取csv文件中某些列的数据到alllist和dt，在后面再进行计算（读取一个文件的方法）
        /// </summary>
        /// <param name="file"></param>
        /// <param name="vehicleIDPara"></param>
        /// <returns></returns>
        public static async Task<LoadCSVDataStructforImport> LoadCSVData(FileInfo file,  VehicleIDPara vehicleIDPara)
        {
            try
            {
                //先创建一个结构体，默认都为true
                var structforimport = new LoadCSVDataStructforImport()
                {
                    status = 0,
                    collumnstatus = 0,
                    name = file.Name.Split('.')[0],

                    Iscontinue = true,
                    boolcan = new List<bool>() { true, true, true, true, true, true, true, true, true, true, true, true, true, true },//按照顺序依次是time，spd，lat，lon，acczwhllf，acczwhlrf，acczwhllr，accxbody，brakecolumnname，throttlecolumnname，accybody，whlangcolumnname，whlanggrcolumnname，enginespeed
                };
                if (!FileOperator.FileIsUsed(file.FullName))
                {
                    await Task.Run(() => {
                        string[] filestring = file.Name.Split('-');
                        string date = filestring[0].Replace("_", "-");//date是拿出来的日期，如“2021-07-03”
                        string oldtime = filestring[1];//oldtime是拿出来的时间，如“10_07_03”
                        string[] timestring = oldtime.Split('_');//把小时，分钟，秒数放到这个数组里
                        string newminute = (int.Parse(timestring[1]) / 30 * 30).ToString();//把所有的分钟改为小于30分的都是0分，大于30分都是30分
                        string newtime = timestring[0] + ":" + newminute + ":" + "00";
                        string datetime = date + " " + newtime;
                        Encoding encoding = Encoding.Default;
                        FileStream fs = new FileStream(file.FullName, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                        StreamReader sr = new StreamReader(fs, encoding, false, 8192);
                        DataTable dt = new DataTable();
                        if (file.Length != 0)
                        {
                            structforimport.datetime = datetime;
                            List<List<double>> AllList = new List<List<double>>();
                            for (int i = 0; i < 14; i++)
                            {
                                AllList.Add(new List<double>());
                            }
                            for (int i = 0; i < AllList.Count; i++)
                            {
                                AllList[i].Add(0);//所有通道每次都在开始加一个0数据
                            }
                            AllList[0].Clear();//time通道清除0数据
                            AllList[1].Clear();//spd通道清除0数据
                            AllList[2].Clear();//lat通道清除0数据
                            AllList[3].Clear();//lon通道清除0数据
                            AllList[13].Clear();//engspd通道清除0数据
                            string strLine = "";
                            //记录每行记录中的各字段内容
                            string[] aryLine = null;
                            string[] tableHead = null;
                            int columnCount = 0;
                            List<int> index = new List<int>();//存储需要计算的通道在csv文件中的字段位置
                                                              //List<int> negindex = new List<int>();//存储没有在csv文件中的字段中找到的通道
                                                              //标示是否是读取的第一行
                            bool IsFirst = true;

                            byte reset = vehicleIDPara.Reductiontimesforaccimport;
                            while ((strLine = sr.ReadLine()) != null)
                            {
                                if (IsFirst == true)
                                {
                                    //改为小写，去掉下划线，空格和N
                                    tableHead = strLine.Replace("_", "").Replace("N", "").Replace(" ", "").ToLower().Split(',');
                                    IsFirst = false;
                                    columnCount = tableHead.Length;
                                    structforimport.columncount = columnCount;
                                    structforimport.tablehead = tableHead;
                                    //获得csv文件中各个通道的列号
                                    index.Add(tableHead.ToList().IndexOf("time"));
                                    index.Add(tableHead.ToList().IndexOf(vehicleIDPara.speedcolumnname));
                                    index.Add(tableHead.ToList().IndexOf("lat"));
                                    index.Add(tableHead.ToList().IndexOf("lon"));
                                    index.Add(tableHead.ToList().IndexOf(vehicleIDPara.acczwhllf));
                                    index.Add(tableHead.ToList().IndexOf(vehicleIDPara.acczwhlrf));
                                    index.Add(tableHead.ToList().IndexOf(vehicleIDPara.acczwhllr));
                                    index.Add(tableHead.ToList().IndexOf(vehicleIDPara.accxbody));
                                    index.Add(tableHead.ToList().IndexOf(vehicleIDPara.brakecolumnname));
                                    index.Add(tableHead.ToList().IndexOf(vehicleIDPara.throttlecolumnname));
                                    index.Add(tableHead.ToList().IndexOf(vehicleIDPara.accybody));
                                    index.Add(tableHead.ToList().IndexOf(vehicleIDPara.whlangcolumnname));
                                    index.Add(tableHead.ToList().IndexOf(vehicleIDPara.whlanggrcolumnname));
                                    index.Add(tableHead.ToList().IndexOf(vehicleIDPara.enginespeed));
                                    //创建列
                                    for (int i = 0; i < columnCount; i++)
                                    {
                                        DataColumn dc = new DataColumn(tableHead[i]);
                                        dt.Columns.Add(dc);
                                    }
                                }
                                else
                                {
                                    aryLine = strLine.Split(',');
                                    //判断是否需要读取数据，因为有降低采样数
                                    if (reset == vehicleIDPara.Reductiontimesforaccimport)
                                    {
                                        for (int i = 0; i < index.Count; i++)
                                        {
                                            if (index[i] != -1)//判断csv文件里有没有找到对应的数据库中的通道名
                                            {
                                                var success = double.TryParse(aryLine[index[i]], out double number);
                                                if ((!double.IsNaN(number)))
                                                {
                                                    if (i == 1)//判断速度数据是否有问题
                                                    {
                                                        if (number > 200 || number < 0)
                                                        {
                                                            AllList[i].Add(0);
                                                            structforimport.collumnstatus = 3;//数值超出极限
                                                        }
                                                        else
                                                        {
                                                            AllList[i].Add(number);
                                                        }
                                                    }
                                                    else if (i == 2)//判断纬度数据是否有问题，超过90度就有问题
                                                    {
                                                        if (number >= 90 || number <= -90)
                                                        {
                                                            AllList[i].Add(0);
                                                            structforimport.collumnstatus = 3;
                                                        }
                                                        else
                                                        {
                                                            AllList[i].Add(number);
                                                        }
                                                    }
                                                    else if (i == 3)//判断经度数据是否有问题，超过180度就有问题
                                                    {
                                                        if (number >= 180 || number <= -180)
                                                        {
                                                            AllList[i].Add(0);
                                                            structforimport.collumnstatus = 3;
                                                        }
                                                        else
                                                        {
                                                            AllList[i].Add(number);
                                                        }
                                                    }
                                                    else if (i == 11)//判断转向角度数据是否有问题，超过600度就有问题
                                                    {
                                                        if (number > 600 || number < -600)
                                                        {
                                                            AllList[i].Add(0);
                                                            structforimport.collumnstatus = 3;
                                                        }
                                                        else
                                                        {
                                                            AllList[i].Add(number);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        AllList[i].Add(number);
                                                    }


                                                }
                                                else
                                                {
                                                    structforimport.boolcan[i] = false;
                                                    structforimport.collumnstatus = 2;//有nan
                                                }
                                            }
                                            else
                                            {
                                                structforimport.boolcan[i] = false;
                                                structforimport.collumnstatus = 1;
                                            }
                                        }
                                        DataRow dr = dt.NewRow();
                                        for (int j = 0; j < columnCount; j++)
                                        {
                                            var transfersuceess = double.TryParse(aryLine[j], out double value);

                                            if (transfersuceess)
                                            {
                                                if (!double.IsNaN(value))
                                                {
                                                    if (j == index[1])//index[1]是指spd的所在列号
                                                    {
                                                        if (value > 200 || value < 0)
                                                        {
                                                            dr[j] = 0.0;
                                                        }
                                                        else
                                                        {
                                                            dr[j] = value;
                                                        }
                                                    }
                                                    else if (j == index[11])//index[11]是指转向角所在列号
                                                    {
                                                        if (value > 600 || value < -600)
                                                        {
                                                            dr[j] = 0.0;
                                                        }
                                                        else
                                                        {
                                                            dr[j] = value;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        dr[j] = value;
                                                    }

                                                }
                                                else
                                                {
                                                    dr[j] = 0.0;
                                                }
                                            }
                                            else
                                            {
                                                dr[j] = 0.0;
                                            }
                                        }
                                        dt.Rows.Add(dr);
                                        reset = 1;
                                    }
                                    else
                                    {
                                        reset++;
                                    }
                                }
                            }

                            sr.Close();
                            sr.Dispose();
                            fs.Close();
                            fs.Dispose();
                            for (int i = 0; i < AllList.Count; i++)
                            {
                                if (i > 3 && i != 13)
                                {
                                    AllList[i].Add(0);//所有通道除了2个gps,spd和第一个time，每次都在最后加一个0数据
                                }
                            }

                            //structforimport.spd = AllList[1];//专门复制一份spd，给其他制动，转向等计算工况用。因为它们的spd数据数量必须与其他通道一致
                            structforimport.spd = new List<double>(AllList[1].ToArray());
                            structforimport.spd.Insert(0, 0);//最前面加0
                            structforimport.spd.Add(0);//最后面加零
                                                       //如果time列有问题，则不计算
                            if (!structforimport.boolcan[0])
                            {
                                structforimport.status = 3;
                                structforimport.Iscontinue = false;
                            }
                            //如果spd列有问题，则不计算
                            if (!structforimport.boolcan[1])
                            {
                                structforimport.status = 4;
                                structforimport.Iscontinue = false;
                            }
                            structforimport.lists = AllList;
                            structforimport.datatable = dt;
                        }
                        else
                        {
                            structforimport.Iscontinue = false;
                            structforimport.status = 2;//csv无数据
                        }
                    });
                }
                else
                {
                    structforimport.Iscontinue = false;
                    structforimport.status = 1;//文件被占用
                }
                return structforimport;
            }
            catch (Exception ex)
            {
                errormessage.getErrormessage(file, ex.Message);
                throw;
            }
        
        }
        public static async Task<LoadCSVDataStructforWFTImport> LoadCSVWFTData(FileInfo file, VehicleIDPara vehicleIDPara)
        {
            try
            {
                var structforimport = new LoadCSVDataStructforWFTImport()
                {
                    status = 0,
                    name = file.Name.Split('.')[0],
                    Iscontinue = true,
                };
                if (!FileOperator.FileIsUsed(file.FullName))
                {
                    await Task.Run(() => {
                        string[] filestring = file.Name.Split('-');
                        string date = filestring[0].Replace("_", "-");//date是拿出来的日期，如“2021-07-03”
                        string oldtime = filestring[1];//oldtime是拿出来的时间，如“10_07_03”
                        string[] timestring = oldtime.Split('_');//把小时，分钟，秒数放到这个数组里
                        string newminute = (int.Parse(timestring[1]) / 30 * 30).ToString();//把所有的分钟改为小于30分的都是0分，大于30分都是30分
                        string newtime = timestring[0] + ":" + newminute + ":" + "00";
                        string datetime = date + " " + newtime;
                        Encoding encoding = Encoding.Default;
                        FileStream fs = new FileStream(file.FullName, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                        StreamReader sr = new StreamReader(fs, encoding);
                        DataTable dt = new DataTable();
                        if (file.Length != 0)
                        {
                            structforimport.datetime = datetime;
                            string strLine = "";
                            //记录每行记录中的各字段内容
                            string[] aryLine = null;
                            string[] tableHead = null;
                            int columnCount = 0;
                            bool IsFirst = true;
                            byte reset = vehicleIDPara.Reductiontimesforwftimport;
                            while ((strLine = sr.ReadLine()) != null)
                            {
                                if (IsFirst == true)
                                {
                                    //改为小写，去掉下划线，空格和N
                                    tableHead = strLine.Replace("_", "").Replace("N", "").Replace(" ", "").ToLower().Split(',');
                                    IsFirst = false;
                                    columnCount = tableHead.Length;
                                    structforimport.columncount = columnCount;
                                    structforimport.tablehead = tableHead;

                                    //创建列
                                    for (int i = 0; i < columnCount; i++)
                                    {
                                        DataColumn dc = new DataColumn(tableHead[i]);
                                        dt.Columns.Add(dc);
                                    }
                                }
                                else
                                {
                                    aryLine = strLine.Split(',');
                                    if(aryLine.Length == columnCount)
                                    {
                                        //判断是否需要读取数据，因为有降低采样数
                                        if (reset == vehicleIDPara.Reductiontimesforwftimport)
                                        {

                                            DataRow dr = dt.NewRow();
                                            for (int j = 0; j < columnCount; j++)
                                            {
                                                var transfersuceess = double.TryParse(aryLine[j], out double value);
                                                if (transfersuceess)
                                                {
                                                    if (!double.IsNaN(value))
                                                    {
                                                        dr[j] = value;
                                                    }
                                                    else
                                                    {
                                                        dr[j] = 0.0;
                                                        structforimport.status = 3;
                                                        structforimport.Iscontinue = false;
                                                    }
                                                }
                                                else
                                                {
                                                    dr[j] = 0.0;
                                                    structforimport.status = 4;
                                                    structforimport.Iscontinue = false;
                                                }
                                            }
                                            dt.Rows.Add(dr);
                                            reset = 1;
                                        }
                                        else
                                        {
                                            reset++;
                                        }
                                    }
                                    else
                                    {
                                        errormessage.getErrormessage(file, "数据列数与标题列数不符，有空列"); 
                                    }
                               
                                }
                            }
                            sr.Close();
                            sr.Dispose();
                            fs.Close();
                            fs.Dispose();

                            structforimport.datatable = dt;
                        }
                        else
                        {
                            structforimport.Iscontinue = false;
                            structforimport.status = 2;//csv无数据
                        }
                    });
                }
                else
                {
                    structforimport.Iscontinue = false;
                    structforimport.status = 1;//文件被占用
                }

                return structforimport;
            }
            catch (Exception ex)
            {
                errormessage.getErrormessage(file, ex.Message);
                throw;
            }
            //先创建一个结构体，默认都为true
           
        }

        public static async Task<LoadCSVDataStructforMileageImport> LoadCSVMileageData(FileInfo file, VehicleIDPara vehicleIDPara)
        {
            try
            {
                //先创建一个结构体，默认都为true
                var structforimport = new LoadCSVDataStructforMileageImport()
                {
                    status = 0,
                    collumnstatus = 0,
                    name = file.Name.Split('.')[0],

                    Iscontinue = true,
                    boolcan = new List<bool>() { true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true },//按照顺序依次是time，spd，throttlecolumnname，brakecolumnname，throttlecolumnname，whlangcolumnname
                };
                if (!FileOperator.FileIsUsed(file.FullName))
                {
                    await Task.Run(() => {
                        string[] filestring = file.Name.Split('-');
                        string date = filestring[0].Replace("_", "-");//date是拿出来的日期，如“2021-07-03”
                        Encoding encoding = Encoding.Default;
                        FileStream fs = new FileStream(file.FullName, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                        StreamReader sr = new StreamReader(fs, encoding, false, 8192);
                        if (file.Length != 0)
                        {
                            structforimport.datetime = date;
                            List<List<double>> AllList = new List<List<double>>();
                            for (int i = 0; i < 17; i++)
                            {
                                AllList.Add(new List<double>());
                            }

                            string strLine = "";
                            //记录每行记录中的各字段内容
                            string[] aryLine = null;
                            string[] tableHead = null;
                            int columnCount = 0;
                            List<int> index = new List<int>();//存储需要计算的通道在csv文件中的字段位置
                                                              //List<int> negindex = new List<int>();//存储没有在csv文件中的字段中找到的通道
                                                              //标示是否是读取的第一行
                            bool IsFirst = true;
                            byte reset = vehicleIDPara.Reductiontimesforaccimport;
                            while ((strLine = sr.ReadLine()) != null)
                            {
                                if (IsFirst == true)
                                {
                                    //改为小写，去掉下划线，空格和N
                                    tableHead = strLine.Replace("_", "").Replace("N", "").Replace(" ", "").ToLower().Split(',');
                                    IsFirst = false;
                                    columnCount = tableHead.Length;
                                    structforimport.columncount = columnCount;
                                    structforimport.tablehead = tableHead;
                                    //获得csv文件中各个通道的列号
                                    index.Add(tableHead.ToList().IndexOf("time"));
                                    index.Add(tableHead.ToList().IndexOf(vehicleIDPara.speedcolumnname));

                                    index.Add(tableHead.ToList().IndexOf(vehicleIDPara.throttlecolumnname));
                                    index.Add(tableHead.ToList().IndexOf(vehicleIDPara.brakecolumnname));
                                    index.Add(tableHead.ToList().IndexOf(vehicleIDPara.whlangcolumnname));

                                    index.Add(tableHead.ToList().IndexOf("wftfxlf"));
                                    index.Add(tableHead.ToList().IndexOf("wftfxrf"));
                                    index.Add(tableHead.ToList().IndexOf("wftfxlr"));
                                    index.Add(tableHead.ToList().IndexOf("wftfxrr"));
                                    index.Add(tableHead.ToList().IndexOf("wftfylf"));
                                    index.Add(tableHead.ToList().IndexOf("wftfyrf"));
                                    index.Add(tableHead.ToList().IndexOf("wftfylr"));
                                    index.Add(tableHead.ToList().IndexOf("wftfyrr"));
                                    index.Add(tableHead.ToList().IndexOf("wftfzlf"));
                                    index.Add(tableHead.ToList().IndexOf("wftfzrf"));
                                    index.Add(tableHead.ToList().IndexOf("wftfzlr"));
                                    index.Add(tableHead.ToList().IndexOf("wftfzrr"));

                                }
                                else
                                {
                                    aryLine = strLine.Split(',');
                                    //判断是否需要读取数据，因为有降低采样数
                                    if (reset == vehicleIDPara.Reductiontimesforaccimport)
                                    {
                                        for (int i = 0; i < index.Count; i++)
                                        {
                                            if (index[i] != -1)//判断csv文件里有没有找到对应的数据库中的通道名
                                            {
                                                var success = double.TryParse(aryLine[index[i]], out double number);
                                                if ((!double.IsNaN(number)))
                                                {
                                                    if (i == 1)//判断速度数据是否有问题
                                                    {
                                                        if (number > 200 || number < 0)
                                                        {
                                                            AllList[i].Add(0);
                                                            structforimport.collumnstatus = 3;//数值超出极限
                                                        }
                                                        else
                                                        {
                                                            AllList[i].Add(number);
                                                        }
                                                    }
                                                    else if (i == 4)//判断转向角度数据是否有问题，超过600度就有问题
                                                    {
                                                        if (number > 600 || number < -600)
                                                        {
                                                            AllList[i].Add(0);
                                                            structforimport.collumnstatus = 3;
                                                        }
                                                        else
                                                        {
                                                            AllList[i].Add(number);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        AllList[i].Add(number);
                                                    }
                                                }
                                                else
                                                {
                                                    structforimport.boolcan[i] = false;
                                                    structforimport.collumnstatus = 2;//有nan
                                                }
                                            }
                                            else
                                            {
                                                structforimport.boolcan[i] = false;
                                                structforimport.collumnstatus = 1;
                                            }
                                        }
                                        reset = 1;
                                    }
                                    else
                                    {
                                        reset++;
                                    }
                                }
                            }
                            sr.Close();
                            sr.Dispose();
                            fs.Close();
                            fs.Dispose();
                            //如果time列有问题，则不计算
                            if (!structforimport.boolcan[0])
                            {
                                structforimport.status = 3;
                                structforimport.Iscontinue = false;
                            }
                            //如果spd列有问题，则不计算
                            if (!structforimport.boolcan[1])
                            {
                                structforimport.status = 4;
                                structforimport.Iscontinue = false;
                            }
                            structforimport.lists = AllList;
                        }
                        else
                        {
                            structforimport.Iscontinue = false;
                            structforimport.status = 2;//csv无数据
                        }
                    });
                }
                else
                {
                    structforimport.Iscontinue = false;
                    structforimport.status = 1;//文件被占用
                }
                return structforimport;
            }
            catch (Exception ex)
            {
                errormessage.getErrormessage(file, ex.Message);
                throw;
            }
    
        }

        /// <summary>
        /// 从csv文件读取所有列的数据的基本方法
        /// </summary>
        /// <param name="file"></param>
        /// <param name="datetimetype">判断是否是里程导入模式</param>
        /// <returns></returns>
        public static async Task<BaseCSVDataforImport> getDatafromCSVforImport(FileInfo file,Func<bool>datetimetype)
        {
            var basestruct = new BaseCSVDataforImport()
            {
                status = 0,
                name = file.Name.Split('.')[0],
            };
            string[] filestring = file.Name.Split('-');
            string date = filestring[0].Replace("_", "-");
            await Task.Run(() => {
                if (!FileOperator.FileIsUsed(file.FullName))
                {
                    if (datetimetype())
                    {
                        basestruct.datetime = date;
                    }
                    else
                    {
                        string oldtime = filestring[1];//oldtime是拿出来的时间，如“10_07_03”
                        string[] timestring = oldtime.Split('_');//把小时，分钟，秒数放到这个数组里
                        string newminute = (int.Parse(timestring[1]) / 30 * 30).ToString();//把所有的分钟改为小于30分的都是0分，大于30分都是30分
                        string newtime = timestring[0] + ":" + newminute + ":" + "00";
                        string datetime = date + " " + newtime;
                        basestruct.datetime = datetime; 
                    }
                    Encoding encoding = Encoding.Default;
                    FileStream fs = new FileStream(file.FullName, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                    StreamReader sr = new StreamReader(fs, encoding, false, 8192);
                    string strLine = "";
                    //记录每行记录中的各字段内容
                    string[] aryLine = null;

                    int columnCount = 0;
                    //标示是否是读取的第一行
                    bool IsFirst = true;
                    List<List<double>> datalist = new List<List<double>>();
                    while ((strLine = sr.ReadLine()) != null)
                    {
                        if (IsFirst == true)
                        {
                            //改为小写，去掉下划线，空格和N
                            basestruct.headers = strLine.Replace("_", "").Replace("N", "").Replace(" ", "").ToLower().Split(',');
                            IsFirst = false;
                            columnCount = basestruct.headers.Length;
                            for (int i = 0; i < columnCount; i++)
                            {
                                datalist.Add(new List<double>());
                            }
                        }
                        else
                        {
                            aryLine = strLine.Split(',');
                            for (int j = 0; j < columnCount; j++)
                            {
                                var transfersuceess = double.TryParse(aryLine[j], out double value);

                                if (transfersuceess)
                                {
                                    if (!double.IsNaN(value))
                                    {
                                        datalist[j].Add(value);
                                    }
                                    else
                                    {
                                        datalist[j].Add(0);
                                        basestruct.status = 1;//有空行或nan
                                    }
                                }
                                else
                                {
                                    datalist[j].Add(0);
                                    basestruct.status = 2;//不是数字
                                }
                            }
                        }
                    }
                    sr.Close();
                    sr.Dispose();
                    fs.Close();
                    fs.Dispose();
                }
                else
                {
                    basestruct.status = 3;//文件被占用
                }
            });
            return basestruct;
        }
    }
}
