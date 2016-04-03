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
                RunFCFS(ref dataInfo, dataFiles, 1);
           

                /////////// RR
                RunRR(ref dataInfo, dataFiles, 1, 2);
              

                
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

    }
}
