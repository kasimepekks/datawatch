using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using System.Data;
using MysqlforDataWatch;
using Tools.FileOperation;
using System.Threading.Tasks;

namespace Tools
{
  public static  class FileOperator
 {
        public struct CsvFileReturnAllListV2
        {
            public byte status;
            public double Speed;
            public List<double> Time;
            public List<double> Lat;
            public List<double> Lon;
            public string name;
            public double sdistance;
            public List<string> channelorderlist;
            public List<List<double>> otherchannels;
        }

        /// <summary>
        /// 删除文件
        /// </summary>

        public static void DeleteFile(string file)
            {
                File.Delete(file);
            }
     
        /// <summary>
        /// 定位哪个文件夹并返回其文件路径
        /// </summary>
        /// <returns></returns>
        ///  确认是哪个文件路径,date是日期
        public static string DatetoName(string date)
        {
           
            string datereplace = date.Replace("-", "_");//把日期格式2019-07-01改为2019_07_01
          
            return datereplace;
        }
        //创建import的flag文件
        public static void CreateDataImportCompleteFlag(string filePath)
        {
           string filename=filePath+ "/DataImportCompleteFlag.txt";
            if (!File.Exists(filename))
            {
                File.Create(filename).Close();
            }
          
        }


       //返回目标文件夹下的所有子文件夹，不包括子文件夹的子文件夹
        public static DirectoryInfo[] GetSubDirectories(string filePath)
        {
            DirectoryInfo root = new DirectoryInfo(filePath);
            return root.GetDirectories();
        }
        //判断是否有这个txt文件，“DataImportCompleteFlag.txt”，如果有则说明这个文件夹已经处理完了返回true，没有返回false
        public static bool DataImportCompleteFlag(DirectoryInfo root)
        {
            string flag = root.FullName + "/DataImportCompleteFlag.txt";
            return File.Exists(flag);

        }
        /// <summary>
        /// 给手动导入用
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        public static bool DataImportCompleteFlag(string root)
        {
            string flag = root + "/DataImportCompleteFlag.txt";
            return File.Exists(flag);

        }

        //判断是否有这个txt文件，“Done.txt”，如果有则说明数据已经上传完了返回true，没有返回false
        public static bool DataTransferCompleteFlag(DirectoryInfo root)
        {
            string flag = root.FullName + "/"+root.Name+"_Done.txt";
            return File.Exists(flag);

        }
        //给手动导入用
        public static bool DataTransferCompleteFlag(string root,string datename)
        {
            string flag = root + "/" + datename + "_Done.txt";
            return File.Exists(flag);

        }

        //返回目标文件夹下的所有csv文件，并按文件名排序
        public static FileInfo[] Isfileexist(string filePath)
        {
            DirectoryInfo root = new DirectoryInfo(filePath);
            if (!root.Exists)
            {
                root.Create();
            }
            FileInfo[] files = root.GetFiles("*.csv");
            if (files.Length > 0)
            {
                var fc = new FileComparer();
                Array.Sort(files, fc);
            }

            return files;

        }


        //获取最近创建的文件名和创建时间
        //如果没有指定类型的文件，返回null
        public static FileTimeInfo GetLatestFileTimeInfo(string dir, string ext)
        {

            List<FileTimeInfo> list = new List<FileTimeInfo>();
            DirectoryInfo d = new DirectoryInfo(dir);
            if (!d.Exists)
            {
                try
                {
                    d.Create();
                }
                catch (Exception ex)
                {

                    throw ex;
                }
                
            }
            if (d.GetFiles().Length > 0)
            {
                foreach (FileInfo file in d.GetFiles())
                {
                    if (file.Extension.ToUpper() == ext.ToUpper())
                    {
                        list.Add(new FileTimeInfo()
                        {
                            FullFileName = file.FullName,
                            FileCreateTime = file.CreationTime,
                            FileName = file.Name
                        });
                    }
                }
                var f = from x in list
                        orderby x.FileCreateTime
                        select x;
                return f.LastOrDefault();
            }
            else
            {
                return new FileTimeInfo() {
                    FullFileName = "default",
                    FileCreateTime = DateTime.Today,
                    FileName = "default"
                };
            }
        }


        /// <summary>
        /// 判断字符串是否为纯数字
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNumber(string str)
        {
            if (str == null || str.Length == 0)    //验证这个参数是否为空
                return false;                           //是，就返回False
            ASCIIEncoding ascii = new ASCIIEncoding();//new ASCIIEncoding 的实例
            byte[] bytestr = ascii.GetBytes(str);         //把string类型的参数保存到数组里

            foreach (byte c in bytestr)                   //遍历这个数组里的内容
            {
                if ((c < 48 && c!=46 && c != 45) || c > 57)                          //判断是否为数字
                {
                    return false;                              //不是，就返回False
                }
            }
            return true;                                        //是，就返回True
        }


        public static bool FileIsUsed(string fileFullName)
        {
            bool result = false;
            //判断文件是否存在，如果不存在，直接返回 false
            if (!System.IO.File.Exists(fileFullName))
            {
                result = true;
            }//end: 如果文件不存在的处理逻辑
            else
            {//如果文件存在，则继续判断文件是否已被其它程序使用
                //逻辑：尝试执行打开文件的操作，如果文件已经被其它程序使用，则打开失败，抛出异常，根据此类异常可以判断文件是否已被其它程序使用。
                System.IO.FileStream fileStream = null;
                try
                {
                    fileStream = System.IO.File.Open(fileFullName, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.None);
                    result = false;
                }
                catch (System.IO.IOException ex)
                {
                    result = true;
                    Console.WriteLine(ex.Message+ex.Source);
                }
                catch (System.Exception)
                {
                    result = true;
                }
                finally
                {
                    if (fileStream != null)
                    {
                        fileStream.Close();
                    }
                }
            }
            return result;
        }

        public static bool FileIsZero(string fileFullName)
        {
            bool result = false;
            //判断文件是否存在，如果不存在，直接返回 true
            if (!System.IO.File.Exists(fileFullName))
            {
                result = true;
            }//end: 如果文件存在的处理逻辑
            else
            {
                FileInfo fi;
                fi = new FileInfo(fileFullName);
                if (fi.Length/1024 > 0)
                {
                    result = false;
                }
                else
                {
                    result = true;
                }
            }
            return result;
        }
        /// <summary>
        /// 生成ID（数字和字母混和） 
        /// </summary>
        /// <param name="codeCount"></param>
        /// <returns></returns>
        public static string GenerateCheckCode(int codeCount)
        {
            string str = string.Empty;
            int rep = 0;
            long num2 = DateTime.Now.Ticks + rep;
            rep++;
            Random random = new Random(((int)(((ulong)num2) & 0xffffffffL)) | ((int)(num2 >> rep)));
            for (int i = 0; i < codeCount; i++)
            {
                char ch;
                int num = random.Next();
                if ((num % 2) == 0)
                {
                    ch = (char)(0x30 + ((ushort)(num % 10)));
                }
                else
                {
                    ch = (char)(0x41 + ((ushort)(num % 0x1a)));
                }
                str = str + ch.ToString();
            }
            return str;
        }

        /// <summary>
        /// 筛选数据库中填写的csv文件里的字段名来读取数据到List<List<double>>，并降低了采样数
        /// </summary>
        /// <param name="filefullpath">文件路径</param>
        /// <param name="csvcollumnname">数据库里填写的字段名</param>
        /// <param name="watchercollumnname">监控的实际通道名（必须与csvcollumnname数量一致）</param>
        /// <param name="monitortimes">降低采样频率</param>
        /// <param name="intertime">数据时间</param>
        /// <returns></returns>
        public static async Task<CsvFileReturnAllListV2> ReadCSVFiletoList(string filefullpath, string filename, string csvcollumnname, byte monitortimes,int intertime)
        {
            try
            {
                var returnalllist = new CsvFileReturnAllListV2();
                if (!FileIsUsed(filefullpath))
                {
                    if (!FileIsZero(filefullpath))
                    {
                        await Task.Run(() => {
                            //数据库里的监控通道名改为小写，去掉下划线，空格和N
                            var n = csvcollumnname.Replace("_", "").Replace("N", "").Replace(" ", "").ToLower();
                            string[] csvcollumnnamelist = n.Split(',');
                            if (csvcollumnnamelist.Length > 1)
                            {
                                Encoding encoding = Encoding.Default;
                                List<List<double>> AllList = new List<List<double>>();
                                for (int i = 0; i < csvcollumnnamelist.Length; i++)
                                {
                                    AllList.Add(new List<double>());
                                }
                                //string starttime = name.Split('-')[1];
                                using FileStream fs = new FileStream(filefullpath, System.IO.FileMode.Open, System.IO.FileAccess.Read, FileShare.ReadWrite);
                                StreamReader sr = new StreamReader(fs, encoding);
                                DataTable dt = new DataTable();
                                string strLine = "";
                                //记录每行记录中的各字段内容
                                string[] aryLine = null;
                                string[] tableHead = null;
                                int columnCount = 0;
                                ////标示列数
                                //int columnCount = 0;

                                //标示是否是读取的第一行
                                bool IsFirst = true;
                                byte reset = monitortimes;
                                while ((strLine = sr.ReadLine()) != null)
                                {
                                    if (IsFirst == true)
                                    {
                                        //改为小写，去掉下划线，空格和N
                                        tableHead = strLine.Replace("_", "").Replace("N", "").Replace(" ", "").ToLower().Split(',');
                                        IsFirst = false;
                                        columnCount = tableHead.Length;
                                    }
                                    else
                                    {
                                        aryLine = strLine.Split(',');
                                        //判断是否需要读取数据，因为有降低采样数
                                        if (reset == monitortimes)
                                        {
                                            for (int i = 0; i < csvcollumnnamelist.Length; i++)
                                            {
                                                if (tableHead.ToList().IndexOf(csvcollumnnamelist[i]) != -1)
                                                {

                                                    var success = double.TryParse(aryLine[tableHead.ToList().IndexOf(csvcollumnnamelist[i])], out double number);
                                                    if ((!double.IsNaN(number)))
                                                    {
                                                        AllList[i].Add(number);
                                                    }
                                                    else
                                                    {
                                                        AllList[i].Add(-1.0);
                                                    }
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
                                //获得time,spd,lat,lon在AllList里的index
                                var csvlist = csvcollumnnamelist.ToList();
                                var timeindex = csvlist.IndexOf("time");
                                var spdindex = csvlist.FindIndex(a => a.Contains("spd"));//用can里的速度，这里只要数据库的名字里包含spd就行
                                var latindex = csvlist.IndexOf("lat");
                                var lonindex = csvlist.IndexOf("lon");
                                double speedaverage = -0.72;
                                if (spdindex != -1)
                                {
                                    speedaverage = AllList[spdindex].Sum()>0? (AllList[spdindex].Sum()) / (AllList[spdindex].Count):0;
                                }
                                List<double> time = new List<double>();
                                List<double> lat = new List<double>();
                                List<double> lon = new List<double>();
                                int inter = AllList[timeindex].Count / 10;
                                if (timeindex != -1)
                                {
                                    time = AllList[timeindex];
                                }
                                if (latindex != -1 && lonindex != -1)
                                {
                                    for (int i = 0; i < 10; i++)
                                    {

                                        lat.Add(AllList[latindex].Skip(i * inter).Take(1).FirstOrDefault());
                                        lat.Add(AllList[latindex].LastOrDefault());
                                        lon.Add(AllList[lonindex].Skip(i * inter).Take(1).FirstOrDefault());
                                        lon.Add(AllList[lonindex].LastOrDefault());
                                    }
                                }
                                var returnlist = new CsvFileReturnAllListV2()
                                {
                                    status = 0,
                                    Time = time,
                                    Speed = Math.Round(speedaverage, 1),
                                    Lat = lat,
                                    Lon = lon,
                                    name = filename.Split('.')[0],
                                    sdistance = Math.Round(speedaverage / 3600 * intertime, 3),
                                    channelorderlist = csvcollumnnamelist.ToList(),
                                    otherchannels = AllList
                                };
                                returnalllist = returnlist;
                            }
                            else
                            {
                                returnalllist.status = 1;
                            }

                        });
                    }
                    else
                    {
                        returnalllist.status = 3;
                    }
                  
                }
                else
                {
                    returnalllist.status = 2;
                }
                return returnalllist;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
  }
}
