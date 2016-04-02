﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Sim
{
    class Analysis
    {
        /// <summary>
        /// returns the turnaround time for a specific process
        /// </summary>
        /// <param name="arrival"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public int turnaroundTime(int arrival, int endTime)
        {
            int turnaroundTime = endTime - arrival;
            return turnaroundTime;
        }

        /// <summary>
        /// computes the average start time for all processes from datafile
        /// </summary>
        /// <param name="logList"></param>
        /// <returns></returns>
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

        /// <summary>
        /// computes the average finishing times for all the process from the datafile
        /// </summary>
        /// <param name="logList"></param>
        /// <returns></returns>
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

        /// <summary>
        /// computes the average turnaround time for all the process freom the datafile
        /// this utilizes the turnaround function from above
        /// </summary>
        /// <param name="logList"></param>
        /// <returns></returns>
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

        /// <summary>
        /// computes the average response time for all processes in datafile
        /// </summary>
        /// <param name="logList"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logList"></param>
        /// <returns></returns>
        //public double AverageContactSwitchTime(List<Metadata> logList)
        //{
        //    double average = 0;
        //    for (int i = 0; i < logList.Count; i++)
        //    {
        //        double turn = System.Convert.ToDouble(logList[i].numContactSwitch);
        //        average += turn;
        //    }
        //    average /= logList.Count;
        //    return average;
        //}

        /// <summary>
        /// function that computes and outputs all results of simulation
        /// </summary>
        /// <param name="logInfo"></param>
        public void DisplayAverages(Dictionary<int, ProcessControlBLock> logInfo)
        {
            List<KeyValuePair<int, ProcessControlBLock>> temp = new List<KeyValuePair<int, ProcessControlBLock>>();
            temp.Clear();
            temp = logInfo.ToList();

            List<Metadata> logList = new List<Metadata>();
            for (int i = 0; i < logInfo.Count; i++)
            {
                logList.Add(temp[i].Value.log);
            }

            double responseAvg = AverageResponseTime(logList);
            double turnAroundAvg = AverageTurnaroundTime(logList);
            double startAvg = AverageStartTime(logList);
            double endAvg = AverageEndTime(logList);

            // write results to results.txt
            string mydocpath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            using (StreamWriter outputFile = new StreamWriter(mydocpath + @"\results.txt"))
            {
                outputFile.WriteLine("Averages\n");
                outputFile.WriteLine("Response Time: {0}, turnaround Time: {1}, Start Time: {2}, end Time: {3}",
                    responseAvg, turnAroundAvg, startAvg, endAvg);
            }
        }

    }
}
