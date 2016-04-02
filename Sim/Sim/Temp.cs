using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sim
{
    class Analysis
    {
        public int turnaroundTime(int arrival, int endTime)
        {
            int turnaroundTime = endTime - arrival;
            return turnaroundTime;
        }

        public double AverageStartTime(List<Metadata> logList)
        {
            double average = 0;
            for (int i = 0; i < logList.Count; i++)
            {
                average += System.Convert.ToDouble(logList[i].response);
            }
            average /= logList.Count;
            return average;
        }

        public double AverageEndTime(List<Metadata> logList)
        {
            double average = 0;
            for (int i = 0; i < logList.Count; i++)
            {
                average += System.Convert.ToDouble(logList[i].completed);
            }
            average /= logList.Count;
            return average;
        }

        public double AverageTurnaroundTime(List<Metadata> logList)
        {
            double average = 0;
            for (int i = 0; i < logList.Count; i++)
            {
                double turn = System.Convert.ToDouble(turnaroundTime(logList[i].submitted, logList[i].completed));
                average += turn;
            }
            average /= logList.Count;
            return average;
        }

        public double AverageResponseTime(List<Metadata> logList)
        {
            double average = 0;
            for (int i = 0; i < logList.Count; i++)
            {
                double turn = System.Convert.ToDouble(logList[i].response);
                average += turn;
            }
            average /= logList.Count;
            return average;
        }

        public void DisplayAverages(Dictionary<int, Metadata> logInfo)
        {
            List<KeyValuePair<int,Metadata>> temp = new List<KeyValuePair<int,Metadata>>();
            temp.Clear();
            temp = logInfo.ToList();

            List<Metadata> logList = new List<Metadata>();
            for (int i = 0; i < logInfo.Count; i++)
            {
                logList.Add(temp[i].Value);
            }

            double responseAvg = AverageResponseTime(logList);
            double turnAroundAvg = AverageTurnaroundTime(logList);
            double startAvg = AverageStartTime(logList);
            double endAvg = AverageEndTime(logList);

            // convert to write to file
            Console.WriteLine("Averages\n");
            Console.WriteLine("Response Time: {0}, turnaround Time: {1}, Start Time: {2}, end Time: {3}", 
                responseAvg, turnAroundAvg, startAvg, endAvg);
        }
    }
}
