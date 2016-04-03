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
            int dataFiles = 1;
            while (dataFiles > 0)
            {
                DataFile dataInfo = new DataFile();
                dataInfo.MakeDataFile();
                dataInfo.getInfoFromFile();

                /////////// FCFS
                //RunFCFS(ref dataInfo, dataFiles, 1);
                //dataInfo.getInfoFromFile();

                //////////// FCFS multiprocessor
                //RunFCFS(ref dataInfo, dataFiles, 4);
                //dataInfo.getInfoFromFile();

                /////////// RR
                //RunRR(ref dataInfo, dataFiles, 1);
                //dataInfo.getInfoFromFile();

                /////////// RR multiprocessor
                //RunRR(ref dataInfo, dataFiles, 4);
                //dataInfo.getInfoFromFile();

                /////////// SPN 
                //RunSPN(ref dataInfo, dataFiles, 1);
                //dataInfo.getInfoFromFile();

                /////////// SPN multiprocessor
                //RunSPN(ref dataInfo, dataFiles, 4);
                //dataInfo.getInfoFromFile();

                /////////// STR 
                RunSTR(ref dataInfo, dataFiles, 1);
                //dataInfo.getInfoFromFile();

                /////////// STR multiprocessor
                //RunSTR(ref dataInfo, dataFiles, 4);
                //dataInfo.getInfoFromFile();

                dataFiles--;
            }
        }

        static public void RunFCFS(ref DataFile dataInfo, int dataFiles, int processors)
        {
            FCFS algo1 = new FCFS(processors);
            algo1.getInfo(dataInfo.getDictionary(), dataInfo.getSubTimes());
            algo1.RunSimulation();

            Run newRun = new Run("FCFS", dataFiles);
            newRun.outputInfo(ref dataInfo);
            runs.Add(newRun);
        }

        static public void RunRR(ref DataFile dataInfo, int dataFiles, int processors)
        {
            RR algo2 = new RR(processors);
            algo2.getInfo(dataInfo.getDictionary(), dataInfo.getSubTimes());
            algo2.RunSimulation();

            Run newRun = new Run("RR", dataFiles);
            newRun.outputInfo(ref dataInfo);
            runs.Add(newRun);
        }

        static public void RunSPN(ref DataFile dataInfo, int dataFiles, int processors)
        {
            SPN algo = new SPN(processors);
            algo.getInfo(dataInfo.getDictionary(), dataInfo.getSubTimes());
            algo.RunSimulation();

            Run newRun = new Run("SPN", dataFiles);
            newRun.outputInfo(ref dataInfo);
            runs.Add(newRun);
        }

        static public void RunSTR(ref DataFile dataInfo, int dataFiles, int processors)
        {
            STR algo = new STR(processors);
            algo.getInfo(dataInfo.getDictionary(), dataInfo.getSubTimes());
            algo.RunSimulation();

            Run newRun = new Run("STR", dataFiles);
            newRun.outputInfo(ref dataInfo);
            runs.Add(newRun);
        }

    }
}
