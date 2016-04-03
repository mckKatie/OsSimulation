using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum Strategy { FCFS, RR, SPN, STR }

namespace Sim
{
    public class Run
    {
        Strategy strat;
        int numProcessors;
        int quantum;


        string dataFile;
        double responseAvg = 0, turnAroundAvg = 0, startAvg = 0;
        double endAvg = 0, contactSwitches = 0;

        public Run(Strategy _strat, string _dataFile, Dictionary<int, ProcessControlBlock> procs)
        {
            strat = _strat;
            dataFile = _dataFile;
            computeAverages(procs);
        }
        public void computeAverages(Dictionary<int, ProcessControlBlock> procs)
        {
            List<double> avgs = new List<double>();

            avgs = Analysis.DisplayAverages(procs);
            responseAvg = avgs[0];
            turnAroundAvg = avgs[1];
            startAvg = avgs[2];
            endAvg = avgs[3];
            contactSwitches = avgs[4];
        }

        public void outputInfo()
        {
            Console.WriteLine("\nFor datafile {0} and scheduling algorith {1}...", dataFile, strat.ToString());
            Console.Write("Response Avgerage: \t{0}\nTurnaround Average: \t{1}\nStart Average: \t{2}\nEnding Time Average: \t{3}\n" +
                "Average Contact Switches: \t{4}\n\n",
                responseAvg, turnAroundAvg, startAvg, endAvg, contactSwitches);
        }
    }
}
