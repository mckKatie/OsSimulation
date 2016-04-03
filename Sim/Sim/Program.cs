using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sim
{
    class Program
    {
        static List<Run> runs;
        static void Main(string[] args)
        {
            runs = new List<Run>();
            int fileIndex = 1;
            while (fileIndex > 0)
            {
                DataFile dataInfo = new DataFile();
                string filePath = dataInfo.MakeDataFile(fileIndex);
                dataInfo.getInfoFromFile(fileIndex);

                /////////// FCFS
                //RunFCFS(ref dataInfo, filePath, 1);
                //dataInfo.getInfoFromFile();

                //////////// FCFS multiprocessor
                //RunFCFS(ref dataInfo, dataFiles, 4);
                //dataInfo.getInfoFromFile();

                /////////// RR
                //RunRR(ref dataInfo, filePath, 1, 10);
              
                //RunRR(ref dataInfo, dataFiles, 1);
                //dataInfo.getInfoFromFile();

                /////////// RR multiprocessor
                //RunRR(ref dataInfo, dataFiles, 4);
                //dataInfo.getInfoFromFile();

                /////////// SPN 
                //RunSPN(ref dataInfo, filePath, 1);
                //dataInfo.getInfoFromFile();

                /////////// SPN multiprocessor
                //RunSPN(ref dataInfo, dataFiles, 4);
                //dataInfo.getInfoFromFile();

                /////////// STR 
                //RunSTR(ref dataInfo, filePath, 1);
                //dataInfo.getInfoFromFile();

                /////////// STR multiprocessor
                //RunSTR(ref dataInfo, dataFiles, 4);
                //dataInfo.getInfoFromFile();

                /////////// HRRN
                RunHRRN(ref dataInfo, filePath, 1);
                fileIndex--;
            }
        }

        static public void RunFCFS(ref DataFile dataInfo, string filePath, int processors)
        {
            FCFS algo1 = new FCFS(filePath, processors);
            algo1.getInfo(dataInfo.getDictionary(), dataInfo.getSubTimes());
            Run newRun =  algo1.RunSimulation();
            newRun.outputInfo();
            runs.Add(newRun);
        }

        static public void RunRR(ref DataFile dataInfo, string filePath, int processors, int quantum)
        {
            RR algo2 = new RR(filePath, processors, quantum);
            algo2.getInfo(dataInfo.getDictionary(), dataInfo.getSubTimes());
            Run newRun = algo2.RunSimulation();
            newRun.outputInfo();
            runs.Add(newRun);
        }

        static public void RunSPN(ref DataFile dataInfo, string filePath, int processors)
        {
            SPN algo = new SPN(filePath, processors);
            algo.getInfo(dataInfo.getDictionary(), dataInfo.getSubTimes());
            Run newRun = algo.RunSimulation();
            newRun.outputInfo();
            runs.Add(newRun);
        }

        static public void RunSTR(ref DataFile dataInfo, string filePath, int processors)
        {
            STR algo = new STR(filePath, processors);
            algo.getInfo(dataInfo.getDictionary(), dataInfo.getSubTimes());
            Run newRun = algo.RunSimulation();
            newRun.outputInfo();
            runs.Add(newRun);
        }
        static public void RunHRRN(ref DataFile dataInfo, string filePath, int processors)
        {
            HRRN algo = new HRRN(filePath, processors);
            algo.getInfo(dataInfo.getDictionary(), dataInfo.getSubTimes());
            Run newRun = algo.RunSimulation();
            newRun.outputInfo();
            runs.Add(newRun);
        }
    }
}
