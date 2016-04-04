using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace Sim
{
    class DataFile
    {
        string mydocpath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        Dictionary<int, ProcessControlBlock> importantInfo = new Dictionary<int, ProcessControlBlock>();
        List<Tuple<int, int>> submitTimes = new List<Tuple<int, int>>();

        public Dictionary<int, ProcessControlBlock> getDictionary()
        {
            return importantInfo;
        }
        public List<Tuple<int, int>> getSubTimes()
        {
            return submitTimes;
        }

        /// <summary>
        /// make the data files for the program. Short sweet, to the point...datafile
        /// the order is PID, arrivalTime, then the bursts
        /// </summary>
        public string MakeDataFile(int fileIndex, Tuple<int,int> cpuBurst, 
            Tuple<int,int> ioBurst, int numBursts, int arrivalT, int numProc)
        {

            string mydocpath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string filePath = mydocpath + @"\input" + fileIndex + ".txt";
            //string filePath = @"\input" + fileIndex + ".txt";
            using (StreamWriter data = new StreamWriter(filePath))
            {
                Random inputs = new Random();
                int processes = inputs.Next(1, numProc);
                for (int i = 0; i < processes; i++)
                {
                    // PID is i, 10 random bursts
                    // random number for arrival time
                    data.Write(i + " ");
                    int time = inputs.Next(1, arrivalT);
                    data.Write(time + " "); // arrival time
                    int bursts = inputs.Next(1, numBursts);
                    for (int j = 0; j < bursts; j = j + 2)
                    {
                        time = inputs.Next(cpuBurst.Item1, cpuBurst.Item2);
                        data.Write(time + " ");
                        time = inputs.Next(ioBurst.Item1, ioBurst.Item2);
                    }
                    data.Write("\n");
                }
                data.Close();

            }
            return filePath;
        }
        /// <summary>
        /// Info is stored in PCB, it's constructor asks for all 
        /// the info.  Then all the PCB's are stored into a dictionary
        /// with the PID as the key.
        /// Lastly, this function makes the pairs for the ready queue of the submit times and the PID
        /// </summary>
        public void getInfoFromFile(int fileIndex)
        {

            importantInfo.Clear();
            submitTimes.Clear();
            string[] lines = System.IO.File.ReadAllLines(mydocpath + @"\input" + fileIndex + ".txt");
            foreach (string line in lines)
            {
                string[] difValues = line.Split(' ');

                // for the PID
                int PID = Convert.ToInt32(difValues[0]);

                // for submitted time
                int submitted = Convert.ToInt32(difValues[1]);

                // for the burst times
                List<int> bursts = new List<int>();
                bursts.Clear();
                for (int i = 2; i < difValues.Length - 1; i++)
                {
                    bursts.Add(Convert.ToInt32(difValues[i]));
                }

                //////////// pid is stored twice, once in PCB and once in the dictionary
                ///////////// might be able to remove it from PCB
                ProcessControlBlock newProcess = new ProcessControlBlock(submitted, PID, bursts);
                importantInfo.Add(PID, newProcess);
                submitTimes.Add(new Tuple<int, int>(submitted, PID));

                //Console.Write("Pid: {0}, submitted: {1}, bursts: ", PID, submitted);
                //foreach (int b in bursts)
                //{
                //    Console.Write(b + " ");
                //}
                //Console.Write("\n");

            }
        }

        public void outputInfoToFile(List<StrategyInfo> endInfo)
        {
            string mydocpath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string filePath = mydocpath + @"\results.txt";

            using (StreamWriter outputFile = File.AppendText(filePath))
            {
                foreach (StrategyInfo si in endInfo)
                {
                    outputFile.WriteLine("For {0} on {1} processor(s):\n", si.getStrategy(), si.getNumProcessors());
                    outputFile.Write("Quantum(s):\t\t\t");
                    foreach (int i in si.getQuantums())
                    {
                        outputFile.Write(i + "\t");
                    }
                    outputFile.WriteLine(String.Format("\nTurnaround Time: \t{0:0.000}\nWait Time: \t\t\t{1:0.000} \nSimulation Time: \t{2:0.000}\n" +
                        "Throughput: \t\t{3:0.000}\nResponse Time: \t\t{4:0.000}\nNumber of Runs: \t{5:0.000}\n\n", si.getTurnaroundTime(),
                        si.getWaitTime(), si.getSimulationTime(), si.getThroughput(), si.getResponseTime(), si.getNumRuns()));
                }
            }
        }
    }
}
