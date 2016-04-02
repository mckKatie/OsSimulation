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
        SimManager simManager = new SimManager();

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
                int processes = inputs.Next(1, 10);
                for (int i = 0; i < processes; i++)
                {
                    // PID is i, 10 random bursts
                    // random number for arrival time
                    data.Write(i + " ");
                    int time = inputs.Next(1, 10);
                    data.Write(time + " "); // arrival time
                    int bursts = inputs.Next(1, 5);
                    for (int j = 0; j < bursts; j++)
                    {
                        time = inputs.Next(1, 10);
                        data.Write(time + " ");
                    }
                    data.Write("\n");
                }

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
                for (int i = 2; i < difValues.Length; i++)
                {
                    bursts.Add(Convert.ToInt32(difValues[i]));
                }

                ProcessControlBLock newProcess = new ProcessControlBLock(submitted,PID,bursts);


            }
        }
    }
}
