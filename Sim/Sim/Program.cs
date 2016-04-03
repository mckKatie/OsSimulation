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
            int fileIndex = 10;
            while (fileIndex > 0)
            {
                DataFile dataInfo = new DataFile();
                // order of numbers is cpuburst, ioburst, numBursts, arrivalTime, and number of proceducers
                Tuple<int, int> CPUBurst = new Tuple<int, int>(10, 50);
                Tuple<int, int> IOBurst = new Tuple<int, int>(10, 50);

                string filePath = dataInfo.MakeDataFile(fileIndex, CPUBurst, IOBurst, 150 ,50, 150);
                dataInfo.getInfoFromFile(fileIndex);

                string mydocpath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\results.txt";

                using (StreamWriter outputFile = File.AppendText(mydocpath))
                {
                    outputFile.WriteLine("\nThese are the results from Datafile number {0}", fileIndex);
                }

                #region deploy algorithms
                /////////// FCFS
                RunFCFS(ref dataInfo, filePath, 1);
                //////////// FCFS 2 processors
                RunFCFS(ref dataInfo, filePath, 2);
                //////////// FCFS 4 processors
                RunFCFS(ref dataInfo, filePath, 4);

                /////////// RR w/ differing quantums (1 processor)
                RunRR(ref dataInfo, filePath, 1, 5);
                RunRR(ref dataInfo, filePath, 1, 20);
                RunRR(ref dataInfo, filePath, 1, 40);
                /////////// RR w/ differing quantums (2 processor)
                RunRR(ref dataInfo, filePath, 2, 5);
                RunRR(ref dataInfo, filePath, 2, 20);
                RunRR(ref dataInfo, filePath, 2, 40);
                /////////// RR w/ differing quantums (4 processor)
                RunRR(ref dataInfo, filePath, 4, 5);
                RunRR(ref dataInfo, filePath, 4, 20);
                RunRR(ref dataInfo, filePath, 4, 40);

                /////////// SPN 
                RunSPN(ref dataInfo, filePath, 1);
                /////////// SPN 2 processors
                RunSPN(ref dataInfo, filePath, 2);
                /////////// SPN 4 processors
                RunSPN(ref dataInfo, filePath, 4);

                /////////// STR 
                RunSTR(ref dataInfo, filePath, 1);
                /////////// STR 2 processors
                RunSTR(ref dataInfo, filePath, 2);
                /////////// STR 4 processors
                RunSTR(ref dataInfo, filePath, 4);

                /////////// HRRN
                RunHRRN(ref dataInfo, filePath, 1);
                /////////// HRRN 2 processors
                RunHRRN(ref dataInfo, filePath, 2);
                /////////// HRRN 4 processors
                RunHRRN(ref dataInfo, filePath, 4);

                List<int> qTimes = new List<int>() { 5, 10, 50 };
                List<int> qTimes2 = new List<int>() { 10, 40, 80 };
                List<int> qTimes3 = new List<int>() { 1,2,4,8,16,32,64 };
                /////////// MLFB
                RunMLFB(ref dataInfo, filePath, 1, qTimes);
                RunMLFB(ref dataInfo, filePath, 1, qTimes2);
                RunMLFB(ref dataInfo, filePath, 1, qTimes3);
                /////////// MLFB 2 processors
                RunMLFB(ref dataInfo, filePath, 2, qTimes);
                RunMLFB(ref dataInfo, filePath, 2, qTimes2);
                RunMLFB(ref dataInfo, filePath, 2, qTimes3);
                /////////// MLFB 4 processors
                RunMLFB(ref dataInfo, filePath, 4, qTimes);
                RunMLFB(ref dataInfo, filePath, 4, qTimes2);
                RunMLFB(ref dataInfo, filePath, 4, qTimes3);
                #endregion

                fileIndex--;
            }
            Console.WriteLine("Check your document folder for results.txt and the corresponding datafile");

            ReducedAnalysis endAnalysis = new ReducedAnalysis();
            endAnalysis.completeAnalysis();

        }

        static public void RunFCFS(ref DataFile dataInfo, string filePath, int processors)
        {
            FCFS algo1 = new FCFS(filePath, processors);
            algo1.getInfo(dataInfo.getDictionary(), dataInfo.getSubTimes());
            Run newRun = algo1.RunSimulation();
            //newRun.outputInfo();
            runs.Add(newRun);
        }

        static public void RunRR(ref DataFile dataInfo, string filePath, int processors, int quantum)
        {
            RR algo2 = new RR(filePath, processors, quantum);
            algo2.getInfo(dataInfo.getDictionary(), dataInfo.getSubTimes());
            Run newRun = algo2.RunSimulation();
            //newRun.outputInfo();
            runs.Add(newRun);
        }

        static public void RunSPN(ref DataFile dataInfo, string filePath, int processors)
        {
            SPN algo = new SPN(filePath, processors);
            algo.getInfo(dataInfo.getDictionary(), dataInfo.getSubTimes());
            Run newRun = algo.RunSimulation();
            //newRun.outputInfo();
            runs.Add(newRun);
        }

        static public void RunSTR(ref DataFile dataInfo, string filePath, int processors)
        {
            STR algo = new STR(filePath, processors);
            algo.getInfo(dataInfo.getDictionary(), dataInfo.getSubTimes());
            Run newRun = algo.RunSimulation();
            //newRun.outputInfo();
            runs.Add(newRun);
        }
        static public void RunHRRN(ref DataFile dataInfo, string filePath, int processors)
        {
            HRRN algo = new HRRN(filePath, processors);
            algo.getInfo(dataInfo.getDictionary(), dataInfo.getSubTimes());
            Run newRun = algo.RunSimulation();
            //newRun.outputInfo();
            runs.Add(newRun);
        }
        static public void RunMLFB(ref DataFile dataInfo, string filePath, int processors, List<int> quantumTimes)
        {

            MLFB algo = new MLFB(filePath, processors, quantumTimes);
            algo.getInfo(dataInfo.getDictionary(), dataInfo.getSubTimes());
            Run newRun = algo.RunSimulation();
            //newRun.outputInfo();
            runs.Add(newRun);
        }
    }
}
