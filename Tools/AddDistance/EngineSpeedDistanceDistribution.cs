using System;
using System.Collections.Generic;
using System.Text;

namespace Tools.AddDistance
{
    public class EngineSpeedDistanceDistribution
    {
        /// <summary>
        /// 计算各个转速分布下的总里程
        /// </summary>
        /// <param name="speedlist"></param>
        /// <param name="singledistance"></param>
        /// <returns></returns>
        public static List<double> CalEngSpdDistribution(List<double> engspd, List<double> singledistance)
        {
            List<double> engspddistribution = new List<double>();

            double distance01000 = 0, distance10001500 = 0, distance15002000 = 0, distance20002500 = 0, distance25003000 = 0, distance30004000 = 0, distance40005000 = 0,
                distance50006000 = 0, distanceup6000 = 0;
            for (int i = 0; i < engspd.Count; i++)
            {
                if (engspd[i] < 1000)
                {
                    distance01000 += singledistance[i];
                }
                else if (engspd[i] < 1500)
                {
                    distance10001500 += singledistance[i];
                }
                else if (engspd[i] < 2000)
                {
                    distance15002000 += singledistance[i];
                }
                else if (engspd[i] < 2500)
                {
                    distance20002500 += singledistance[i];
                }
                else if (engspd[i] < 3000)
                {
                    distance25003000 += singledistance[i];
                }
                else if (engspd[i] < 4000)
                {
                    distance30004000 += singledistance[i];
                }
                else if (engspd[i] < 5000)
                {
                    distance40005000 += singledistance[i];
                }
                else if (engspd[i] < 6000)
                {
                    distance50006000 += singledistance[i];
                }
                else if (engspd[i] < 10000)
                {
                    distanceup6000 += singledistance[i];
                }
              
            }

            engspddistribution.Add(distance01000);
            engspddistribution.Add(distance10001500);
            engspddistribution.Add(distance15002000);
            engspddistribution.Add(distance20002500);
            engspddistribution.Add(distance25003000);
            engspddistribution.Add(distance30004000);
            engspddistribution.Add(distance40005000);
            engspddistribution.Add(distance50006000);
            engspddistribution.Add(distanceup6000);
            
            return engspddistribution;
        }
    }
}
