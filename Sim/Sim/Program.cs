using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sim
{
    class Program
    {

        static void Main(string[] args)
        {
            List<Run> runs = new List<Run>();
            int dataFiles = 1;
            while (dataFiles != 0)
            {
                Run newRun = new Run("FCFS", dataFiles);
                DataFile dataInfo = new DataFile();
                dataInfo.getInfoFromFile();
                FCFS algo1 = new FCFS(1);
                algo1.getInfo(dataInfo.getDictionary(), dataInfo.getSubTimes());
                algo1.RunSimulation();
                newRun.outputInfo(ref dataInfo);
                runs.Add(newRun);
                //Analysis.DisplayAverages(dataInfo.getDictionary());
                dataFiles--;
            }
        }
    }
}
