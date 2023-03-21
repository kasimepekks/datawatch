using MysqlforDataWatch;
using System;
using System.Collections.Generic;
using System.Text;
using Tools.MyConfig;

namespace Tools.GPSCal
{
   public static class AddGPS
    {
        public static List<Gpsrecord> addgpslist(List<double> l11_Lat, List<double> l12_Lon, List<double> Speed, string vehicleid, string name, string datetime, in VehicleIDPara vehicleIDPara)
        {

            List<Gpsrecord> gpsrecordlist = new List<Gpsrecord>();
            if (vehicleIDPara.GPSImport == 1)
            {
                var lat = GPS.GPSResampling(l11_Lat,in vehicleIDPara);
                var lon = GPS.GPSResampling(l12_Lon, in vehicleIDPara);
                var speed = GPS.GPSResampling(Speed, in vehicleIDPara);
                for (int i = 0; i < lat.Count; i++)
                {
                    if (lat[i] !=0 && lon[i] !=0)
                    {
                        Gpsrecord gpsrecord = new Gpsrecord();
                        gpsrecord.Id = vehicleid + "-" + name + "-GPS-" + i.ToString();
                        gpsrecord.Filename = name;
                        gpsrecord.VehicleId = vehicleid;
                        gpsrecord.Datadate = Convert.ToDateTime(datetime);
                        gpsrecord.Lat = lat[i];
                        gpsrecord.Lon = lon[i];
                        gpsrecord.Speed = speed[i];
                        gpsrecordlist.Add(gpsrecord);
                    }
                }
            }
            return gpsrecordlist;
        }   
    }
}
