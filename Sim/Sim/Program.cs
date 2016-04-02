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
            DataFile dataInfo = new DataFile();
            dataInfo.getInfoFromFile();
            FCFS algo1 = new FCFS(1);
            algo1.getInfo(dataInfo.getDictionary(), dataInfo.getSubTimes());
            algo1.RunSimulation();
            Analysis.DisplayAverages(dataInfo.getDictionary());
        }
    }
}
