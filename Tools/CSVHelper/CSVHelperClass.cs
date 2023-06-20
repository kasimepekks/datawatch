

using Csv;
using MysqlforDataWatch;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;


namespace Tools.CSVHelper
{
    public class CSVHelperClass
    {
        public static void saveCSVFile(List<Pumapermileage> list)
        {
            //序列化对象为json字符串
            string csv = CsvSerializer.Serialize(list, withHeaders: true);
            string path = Assembly.GetExecutingAssembly().Location;
            string directory = Path.GetDirectoryName(path);
            string writepath = Path.Combine(directory, "mileagecsv.csv");
            File.WriteAllText(writepath, csv);
        }
    }
}
