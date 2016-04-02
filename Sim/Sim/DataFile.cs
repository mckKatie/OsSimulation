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
        public void MakeDataFile()
        {

            string path = @"Z:\CS401\dataFile\dataFile\dataFile.txt";
            if (File.Exists(path))
            {
                using (StreamWriter data = File.CreateText(path))
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

        }
    }
}
