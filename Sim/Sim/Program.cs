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
                RunFCFS(ref dataInfo, dataFiles, 1);
                dataInfo.getInfoFromFile();

                /////////// RR
                RunRR(ref dataInfo, dataFiles, 1);
                dataInfo.getInfoFromFile();

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

            Run newRun2 = new Run("RR", dataFiles);
            newRun2.outputInfo(ref dataInfo);
            runs.Add(newRun2);
        }

    }
}
