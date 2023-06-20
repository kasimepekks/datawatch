using System.Collections.Generic;
using System.Text;

namespace Tools.AddTime
{
    public class EngineSpeedTimeDistribution
    {
        /// <summary>
        /// 计算各个转速分布下的累计点数
        /// </summary>
        /// <param name="engspd"></param>
        /// <returns></returns>
        public static List<int> CalEngSpdTimeDistribution(List<double> engspd)
        {
            List<int> engspddistribution = new List<int>();

            int time01000 = 0, time10001500 = 0, time15002000 = 0, time20002500 = 0, time25003000 = 0, time30004000 = 0, time40005000 = 0,
                time50006000 = 0, timeup6000 = 0;
            for (int i = 0; i < engspd.Count; i++)
            {
                if (engspd[i] < 1000)
                {
                    time01000 += 1;
                }
                else if (engspd[i] < 1500)
                {
                    time10001500 += 1;
                }
                else if (engspd[i] < 2000)
                {
                    time15002000 += 1;
                }
                else if (engspd[i] < 2500)
                {
                    time20002500 += 1;
                }
                else if (engspd[i] < 3000)
                {
                    time25003000 += 1;
                }
                else if (engspd[i] < 4000)
                {
                    time30004000 += 1;
                }
                else if (engspd[i] < 5000)
                {
                    time40005000 += 1;
                }
                else if (engspd[i] < 6000)
                {
                    time50006000 += 1;
                }
                else if (engspd[i] < 10000)
                {
                    timeup6000 += 1;
                }

            }

            engspddistribution.Add(time01000);
            engspddistribution.Add(time10001500);
            engspddistribution.Add(time15002000);
            engspddistribution.Add(time20002500);
            engspddistribution.Add(time25003000);
            engspddistribution.Add(time30004000);
            engspddistribution.Add(time40005000);
            engspddistribution.Add(time50006000);
            engspddistribution.Add(timeup6000);

            return engspddistribution;
        }
    }
}
