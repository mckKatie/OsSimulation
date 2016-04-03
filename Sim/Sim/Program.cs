using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Sim
{
    class Program
    {
        static List<Run> runs;
        static void Main(string[] args)
        {
            runs = new List<Run>();
            int fileIndex = 1;
            while (fileIndex > 0)
            {
                DataFile dataInfo = new DataFile();
                string filePath = dataInfo.MakeDataFile(fileIndex);
                dataInfo.getInfoFromFile(fileIndex);

                //string mydocpath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\results.txt";
                string mydocpath = Directory.GetCurrentDirectory() + @"\results.txt";

                using (StreamWriter outputFile = File.AppendText(mydocpath))
                {
                    outputFile.WriteLine("\nThese are the results from Datafile number {0}", fileIndex);
                }

                /////////// FCFS
                //RunFCFS(ref dataInfo, filePath, 1);
                //dataInfo.getInfoFromFile();

                //////////// FCFS multiprocessor
                //RunFCFS(ref dataInfo, dataFiles, 4);
                //dataInfo.getInfoFromFile();

                /////////// RR
                //RunRR(ref dataInfo, dataFiles, 1);
                //dataInfo.getInfoFromFile();

                /////////// RR multiprocessor
                //RunRR(ref dataInfo, dataFiles, 4);
                //dataInfo.getInfoFromFile();

                /////////// SPN 
                //RunSPN(ref dataInfo, filePath, 1);
                //dataInfo.getInfoFromFile();

                /////////// SPN multiprocessor
                //RunSPN(ref dataInfo, dataFiles, 4);
                //dataInfo.getInfoFromFile();

                /////////// STR 
                //RunSTR(ref dataInfo, filePath, 1);
                //dataInfo.getInfoFromFile();

                /////////// STR multiprocessor
                //RunSTR(ref dataInfo, dataFiles, 4);
                //dataInfo.getInfoFromFile();

                /////////// HRRN
                //RunHRRN(ref dataInfo, filePath, 1);

                /////////// MLFB
                List<int> qTimes = new List<int>() {5, 10 };
                RunMLFB(ref dataInfo, filePath, 1, qTimes);
                fileIndex--;
            }
            Console.WriteLine("Check your document folder for results.txt and the corresponding datafile");
        }

        static public void RunFCFS(ref DataFile dataInfo, string filePath, int processors)
        {
            FCFS algo1 = new FCFS(filePath, processors);
            algo1.getInfo(dataInfo.getDictionary(), dataInfo.getSubTimes());
            Run newRun = algo1.RunSimulation();
            newRun.outputInfo();
            runs.Add(newRun);
        }

        static public void RunRR(ref DataFile dataInfo, string filePath, int processors, int quantum)
        {
            RR algo2 = new RR(filePath, processors, quantum);
            algo2.getInfo(dataInfo.getDictionary(), dataInfo.getSubTimes());
            Run newRun = algo2.RunSimulation();
            newRun.outputInfo();
            runs.Add(newRun);
        }

        static public void RunSPN(ref DataFile dataInfo, string filePath, int processors)
        {
            SPN algo = new SPN(filePath, processors);
            algo.getInfo(dataInfo.getDictionary(), dataInfo.getSubTimes());
            Run newRun = algo.RunSimulation();
            newRun.outputInfo();
            runs.Add(newRun);
        }

        static public void RunSTR(ref DataFile dataInfo, string filePath, int processors)
        {
            STR algo = new STR(filePath, processors);
            algo.getInfo(dataInfo.getDictionary(), dataInfo.getSubTimes());
            Run newRun = algo.RunSimulation();
            newRun.outputInfo();
            runs.Add(newRun);
        }
        static public void RunHRRN(ref DataFile dataInfo, string filePath, int processors)
        {
            HRRN algo = new HRRN(filePath, processors);
            algo.getInfo(dataInfo.getDictionary(), dataInfo.getSubTimes());
            Run newRun = algo.RunSimulation();
            newRun.outputInfo();
            runs.Add(newRun);
        }
        static public void RunMLFB(ref DataFile dataInfo, string filePath, int processors, List<int> quantumTimes)
        {

            MLFB algo = new MLFB(filePath, processors, quantumTimes);
            algo.getInfo(dataInfo.getDictionary(), dataInfo.getSubTimes());
            Run newRun = algo.RunSimulation();
            newRun.outputInfo();
            runs.Add(newRun);
        }
    }
}
