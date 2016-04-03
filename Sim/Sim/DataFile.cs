using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace Sim
{

    class Run
    {
        int dataFileID;
        string type;
        double ResponseAvg = 0, TurnAroundAvg = 0, StartAvg = 0;
        double EndAvg = 0, contactSwitch = 0;

        public Run(string algo, int dataFile)
        {
            type = algo;
            dataFileID = dataFile;
        }
        public void getAverages(ref DataFile dataInfo)
        {
            List<double> avgs = new List<double>();

            avgs = Analysis.DisplayAverages(dataInfo.getDictionary());
            ResponseAvg = avgs[0];
            TurnAroundAvg = avgs[1];
            StartAvg = avgs[2];
            EndAvg = avgs[3];
            contactSwitch = avgs[4];
        }

        public void outputInfo(ref DataFile dataInfo)
        {
            getAverages(ref dataInfo);
            Console.WriteLine("\nFor datafile {0} and scheduling algorith {1}...", dataFileID, type);
            Console.Write("Response Avgerage: \t{0}\nTurnaround Average: \t{1}\nStart Average: \t\t{2}\nEnding Time Average: \t{3}\n" + 
                "Average Contact Switches: \t{4}\n\n",
                ResponseAvg, TurnAroundAvg, StartAvg, EndAvg, contactSwitch);
        }
    }

    class DataFile
    {
        string mydocpath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        Dictionary<int, ProcessControlBLock> importantInfo = new Dictionary<int, ProcessControlBLock>();
        List<Tuple<int, int>> submitTimes = new List<Tuple<int, int>>();

        public Dictionary<int, ProcessControlBLock> getDictionary()
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
        public void MakeDataFile()
        {

            string mydocpath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            using (StreamWriter data = new StreamWriter(mydocpath + @"\results.txt"))
            {
                Random inputs = new Random();
                int processes = inputs.Next(1, 15);
                for (int i = 0; i < processes; i++)
                {
                    // PID is i, 10 random bursts
                    // random number for arrival time
                    data.Write(i + " ");
                    int time = inputs.Next(1, 10);
                    data.Write(time + " "); // arrival time
                    int bursts = inputs.Next(1, 10);
                    for (int j = 0; j < bursts; j++)
                    {
                        time = inputs.Next(1, 10);
                        data.Write(time + " ");
                    }
                    data.Write("\n");
                }
                data.Close();

            }
        }
        /// <summary>
        /// Info is stored in PCB, it's constructor asks for all 
        /// the info.  Then all the PCB's are stored into a dictionary
        /// with the PID as the key.
        /// Lastly, this function makes the pairs for the ready queue of the submit times and the PID
        /// </summary>
        public void getInfoFromFile()
        {
            
            importantInfo.Clear();
            submitTimes.Clear();
            string[] lines = System.IO.File.ReadAllLines(mydocpath + @"\results.txt");
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
                ProcessControlBLock newProcess = new ProcessControlBLock(submitted,PID,bursts); 
                importantInfo.Add(PID, newProcess);
                submitTimes.Add(new Tuple<int,int>(submitted, PID));

                //Console.Write("Pid: {0}, submitted: {1}, bursts: ", PID, submitted);
                //foreach (int b in bursts)
                //{
                //    Console.Write(b + " ");
                //}
                //Console.Write("\n");

            }
        }
    }
}
