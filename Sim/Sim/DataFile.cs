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
        /// <summary>
        /// make the data files for the program. Short sweet, to the point...datafile
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

        }
    }
}
