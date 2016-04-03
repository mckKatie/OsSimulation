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
<<<<<<< HEAD
                RunRR(ref dataInfo, dataFiles, 1, 2);
              
=======
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
>>>>>>> f7041d9915e529d4a358d95efb95a4193664da53

                dataFiles--;
            }
        }

        static public void RunFCFS(ref DataFile dataInfo, int dataFiles, int processors)
        {
            FCFS algo1 = new FCFS(processors);
            algo1.getInfo(dataInfo.getDictionary(), dataInfo.getSubTimes());
            Run newRun =  algo1.RunSimulation();
            newRun.outputInfo();
            runs.Add(newRun);
        }

        static public void RunRR(ref DataFile dataInfo, int dataFiles, int processors, int quantum)
        {
            RR algo2 = new RR(processors, quantum);
            algo2.getInfo(dataInfo.getDictionary(), dataInfo.getSubTimes());
            Run newRun = algo2.RunSimulation();
            newRun.outputInfo();
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
