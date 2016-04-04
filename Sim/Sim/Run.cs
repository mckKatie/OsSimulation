using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace Sim
{
    public class Run
    {
        Strategy strat;
        int numProcessors;
        List<int> quantums;


        string dataFile;
        double responseAvg = 0, turnAroundAvg = 0, startAvg = 0, waitAvg = 0, endAvg = 0;
        double throughput = 0, contactSwitches = 0, burstsPerProcess = 0, endTime = 0;
        public Run(Strategy _strat, string _dataFile, Dictionary<int, ProcessControlBlock> procs, int _numProcessors, int _endTime, List<int> _quantums)
        {
            quantums = _quantums;
            endTime = _endTime;
            strat = _strat;
            dataFile = _dataFile;
            numProcessors = _numProcessors;
            throughput = endTime / (double)procs.Count;
            computeAverages(procs);
        }
        private void computeAverages(Dictionary<int, ProcessControlBlock> procs)
        {
            List<double> avgs = new List<double>();
            avgs = Analysis.DisplayAverages(procs);

            responseAvg = avgs[0];
            turnAroundAvg = avgs[1];
            startAvg = avgs[2];
            endAvg = avgs[3];
            contactSwitches = avgs[4];
            burstsPerProcess = avgs[5];
            waitAvg = avgs[6];
        }

        public Tuple<Strategy, int> getKey() //this is used identify the key for the multimap
        {
            return new Tuple<Strategy, int>(strat, numProcessors);
        }
        

        public void outputInfo()
        {
            //write results to results.txt
            string mydocpath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\results.txt";
            //string mydocpath = @"\results.txt";

            using (StreamWriter outputFile = File.AppendText(mydocpath))
            {
                outputFile.WriteLine("\nFor datafile {0} and scheduling algorith {1}...", dataFile, strat.ToString());
                outputFile.Write(String.Format("Response Avgerage: \t\t\t{0:0.00}\nTurnaround Average: \t\t{1:0.00}\nStart Average: \t\t\t\t{2:0.00}\nEnding Time Average: \t\t{3:0.00}\n" +
                    "Average Contact Switches: \t{4:0.00}\n" + "Average CPU Allocations Per Process: \t{5:0.00}\n\n",
                    responseAvg, turnAroundAvg, startAvg, endAvg, contactSwitches, burstsPerProcess));
                outputFile.Close();
            }

           // Console.WriteLine("check your documents file for results.txt");
        }

        public void setQuantums(List<int> q)
        {
            quantums = q;
        }
        //////////////////////////
        ////// Get Functions//////
        //////////////////////////
        public List<int> getQuantum() { return quantums; }
        public double getTurnaround() { return turnAroundAvg; }
        public double getWait() { return waitAvg; }
        public double getResponse() { return responseAvg; }
        public double getThroughput() { return throughput; }
        public double getSimulation() { return endTime; }
    }
}
