using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Sim
{
    static class Analysis
    {
        /// <summary>
        /// returns the turnaround time for a specific process
        /// </summary>
        /// <param name="arrival"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public static int turnaroundTime(int arrival, int endTime)
        {
            int turnaroundTime = endTime - arrival;
            return turnaroundTime;
        }

        /// <summary>
        /// computes the average start time for all processes from datafile
        /// </summary>
        /// <param name="logList"></param>
        /// <returns></returns>
        public static double AverageStartTime(List<Metadata> logList)
        {
            double average = 0;
            for (int i = 0; i < logList.Count; i++)
            {
                average += System.Convert.ToDouble(logList[i].response);
            }
            average /= logList.Count;
            return average;
        }

        /// <summary>
        /// computes the average finishing times for all the process from the datafile
        /// </summary>
        /// <param name="logList"></param>
        /// <returns></returns>
        public static double AverageEndTime(List<Metadata> logList)
        {
            double average = 0;
            for (int i = 0; i < logList.Count; i++)
            {
                average += System.Convert.ToDouble(logList[i].completed);
            }
            average /= logList.Count;
            return average;
        }

        /// <summary>
        /// computes the average turnaround time for all the process freom the datafile
        /// this utilizes the turnaround function from above
        /// </summary>
        /// <param name="logList"></param>
        /// <returns></returns>
        public static double AverageTurnaroundTime(List<Metadata> logList)
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

        /// <summary>
        /// computes the average response time for all processes in datafile
        /// </summary>
        /// <param name="logList"></param>
        /// <returns></returns>
        public static double AverageResponseTime(List<Metadata> logList)
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

        public static double AverageWaitTime(List<Metadata> logList)
        {
            double average = 0;
            for (int i = 0; i < logList.Count; i++)
            {
                double turn = System.Convert.ToDouble(logList[i].wait);
                average += turn;
            }
            average /= logList.Count;
            return average;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logList"></param>
        /// <returns></returns>
        public static double AverageContactSwitchTime(List<Metadata> logList)
        {
            double average = 0;
            for (int i = 0; i < logList.Count; i++)
            {
                double turn = System.Convert.ToDouble(logList[i].timesSwapped);
                average += turn;
            }
            average /= logList.Count;
            return average;
        }

        public static double AverageCPUAllocationsPerProcess(List<Metadata> logList)
        {
            double average = 0;
            for (int i = 0; i < logList.Count; i++)
            {
                double count = System.Convert.ToDouble(logList[i].CPUBurstCount);
                average += count;
            }
            average /= logList.Count;
            return average;
        }

        /// <summary>
        /// function that computes and outputs all results of simulation
        /// </summary>
        /// <param name="logInfo"></param>
        public static List<double> DisplayAverages(Dictionary<int, ProcessControlBlock> logInfo)
        {
            List<KeyValuePair<int, ProcessControlBlock>> temp = new List<KeyValuePair<int, ProcessControlBlock>>();
            temp.Clear();
            temp = logInfo.ToList();

            List<Metadata> logList = new List<Metadata>();
            for (int i = 0; i < logInfo.Count; i++)
            {
                logList.Add(temp[i].Value.log);
            }

            List<double> averages = new List<double>();
            double responseAvg = AverageResponseTime(logList);
            averages.Add(responseAvg);
            double turnAroundAvg = AverageTurnaroundTime(logList);
            averages.Add(turnAroundAvg);
            double startAvg = AverageStartTime(logList);
            averages.Add(startAvg);
            double endAvg = AverageEndTime(logList);
            averages.Add(endAvg);
            double contactSwitch = AverageContactSwitchTime(logList);
            averages.Add(contactSwitch);
            double burstCountPerProcess = AverageCPUAllocationsPerProcess(logList);
            averages.Add(burstCountPerProcess);
            double waitAvg = AverageWaitTime(logList);
            averages.Add(waitAvg);
            // write results to results.txt
            //string mydocpath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            //using (StreamWriter outputFile = new StreamWriter(mydocpath + @"\results.txt"))
            //{
            //    outputFile.WriteLine("Averages\n");
            //    outputFile.WriteLine("Response Time: {0}, turnaround Time: {1}, Start Time: {2}, end Time: {3}",
            //        responseAvg, turnAroundAvg, startAvg, endAvg);
            //}
            return averages;
        }

    }
}
