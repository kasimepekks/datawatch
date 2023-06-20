using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Tools.ErrorMessage
{
    public class errormessage
    {
        /// <summary>
        /// 把error输出到tools_log.txt
        /// </summary>
        /// <param name="file"></param>
        /// <param name="ex"></param>
        public static void getErrormessage(FileInfo file, string message)
        {
            var content = "处理此文件名：" + file.Name + "发生了错误：" +message;
            string path = Assembly.GetExecutingAssembly().Location;
            string directory = Path.GetDirectoryName(path);
            string writepath = Path.Combine(directory, "tools_log.txt");
            Console.WriteLine(directory);
            File.AppendAllText(writepath, content);
        }

    }
}
