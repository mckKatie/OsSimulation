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
                dataInfo.getInfoFromFile();

                /////////// FCFS
                //RunFCFS(ref dataInfo, dataFiles);
           

                /////////// RR
                RunRR(ref dataInfo, dataFiles);
              

                
                dataFiles--;
            }
        }

        static public void RunFCFS(ref DataFile dataInfo, int dataFiles)
        {
            FCFS algo1 = new FCFS(1);
            algo1.getInfo(dataInfo.getDictionary(), dataInfo.getSubTimes());
            algo1.RunSimulation();

            Run newRun = new Run("FCFS", dataFiles);
            newRun.outputInfo(ref dataInfo);
            runs.Add(newRun);
        }

        static public void RunRR(ref DataFile dataInfo, int dataFiles)
        {
            RR algo2 = new RR(1);
            algo2.getInfo(dataInfo.getDictionary(), dataInfo.getSubTimes());
            algo2.RunSimulation();

            Run newRun = new Run("RR", dataFiles);
            newRun.outputInfo(ref dataInfo);
            runs.Add(newRun);
        }

    }
}
