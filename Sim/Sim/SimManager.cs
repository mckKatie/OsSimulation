using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sim
{
    abstract public class SimManager
    {

        public int clock;
        public Dictionary<int, ProcessControlBlock> processes;
        public List<Tuple<int, int>> subTimes;
        List<Tuple<int, int>> IOList; //dont know what to call this <outTime, PID>
        public List<Processor> processors;
        Strategy strat;
        string inputFileName;

        public SimManager(string filePath, int numProcessors, Strategy _strat)
        {
            processes = new Dictionary<int,ProcessControlBlock>();
            subTimes = new List<Tuple<int,int>>();
            IOList = new List<Tuple<int,int>>();
            processors = new List<Processor>();
            clock = 0;
            strat = _strat;
            inputFileName = filePath;
            for(int i = 0; i < numProcessors; i++)
            {
                processors.Add(new Processor(i));
            }
            // Feel free to add functions here to seed data structures

        }

        //public void getInfo(Dictionary<int, ProcessControlBlock> procs, List<Tuple<int, int>> subs) // need to deep copy data if running in parallel
        //{
        //    foreach (KeyValuePair<int, ProcessControlBlock> kvp in procs)
        //        processes.Add(kvp.Key, new ProcessControlBlock(kvp.Value.getSubmitted(), kvp.Value.getPID(), kvp.Value.getBursts()));
        //    foreach (Tuple<int, int> s in subs)
        //        subTimes.Add(new Tuple<int, int>(s.Item1, s.Item2));
        //    subTimes.Sort();
        //}
        public void getInfo(Dictionary<int, ProcessControlBlock> procs, List<Tuple<int, int>> subs) // need to deep copy data if running in parallel
        {
            processes = procs;
            foreach (Tuple<int, int> p in subs)
            {
                subTimes.Add(new Tuple<int, int>(p.Item1, p.Item2));
            }
            subTimes.Sort();
        }

        public Run RunSimulation()
        {
            while (true)
            {
                //increment clock
                clock++;
                FinishSwaps();
                //finish io
                CheckForProcesses(IOList);
                //check for completed processes, interrupt if necessary
                CheckProcessorStatus();
                MarkInterrupts();
                //handle interrupted processes
                HandleInterrupts();
                //submit new processes
                CheckForProcesses(subTimes);
                //assign to free processors
                UpdateReadyQueue();
                AssignFreeProcessors();
                if(subTimes.Count != 0)
                {
                    continue;
                }
                else if(IOList.Count != 0)
                {
                    continue;
                }
                else if(!ReadyQueueEmpty())
                {
                    continue;
                }
                else if(!ProcessorsAllEmpty())
                {
                    continue;
                }
                break;
            }
            return ComposeResults();
        }

        private Run ComposeResults()   //use this function to make run instance
        {
            Run temp = new Run(getStrategy(), inputFileName, processes, processors.Count);
            AddAdditionalMetadata(temp);
            ResetPCBs();
            return temp;
        }
        private void ResetPCBs()
        {
            foreach (KeyValuePair<int, ProcessControlBlock> kvp in processes)
            {
                kvp.Value.ResetPCB();
            }
        }
        private Strategy getStrategy() { return strat; }
        public ProcessControlBlock getProcessByID(int pid)
        {
            ProcessControlBlock temp;
            processes.TryGetValue(pid, out temp);
            return temp;
        }
        public void CheckProcessorStatus()
        {
            foreach (Processor p in processors)
            {
                if(p.BurstCompleteCheck(clock)) // this will toggle processor status to stop if Burst Cmpleted
                {
                    int id = p.getPID();
                    ProcessControlBlock temp = getProcessByID(id);
                    temp.CPUFinish(clock);
                    if(temp.isIO())
                        StartIO(id);
                    p.SwapContexts();
                }
            }
        }

        public void FinishSwaps()//sets processor state to free now that content has been saved
        {
            foreach (Processor p in processors)
            {
                if (p.isSwapping())
                {
                    p.FreeProcessor();
                }
            }
        }
        public void CheckForProcesses(List<Tuple<int, int>> procList) //check procList for processes ready to be placed in wait queue
        {
            while(procList.Count != 0)
            {
                if(procList[0].Item1 == clock)
                {
                    ProcessControlBlock temp = getProcessByID(procList[0].Item2);
                    if(Object.ReferenceEquals(procList, IOList))
                    {
                        temp.IOFinish(clock);
                    }
                    if(temp.isReady())
                        ProcessReadyQueue(procList[0].Item2);
                    procList.RemoveAt(0);
                    continue;
                }
                break;
            }
        }

        public void StartIO(int PID) // looks up processes and places id and time of io completion into list, then sorts
        {
            ProcessControlBlock temp = getProcessByID(PID);
            int burstDuration = temp.getNextBurst();
            int burstCompletionTime = clock + burstDuration;

            IOList.Add(new Tuple<int, int>(burstCompletionTime, PID));
            IOList.Sort();
        }

        public void AssignFreeProcessors()
        {
            foreach (Processor p in processors)
            {
                if( p.isOpen() && !ReadyQueueEmpty())
                {
                    p.AssignProcess(ProcessOpenProcessor(p.getProcID()));
                }
            }
        }
        public bool ProcessorsAllEmpty()
        {
            foreach(Processor p in processors)
            {
                if (!p.isOpen())
                    return false;
            }
            return true;
        }

        public void InterruptProcessor(Processor p) // changes processor state and has pcb execute interruption handling
        {
            int pid = p.getPID();
            ProcessControlBlock temp = getProcessByID(pid);
            temp.CPUInterrupt(clock);
            ProcessReadyQueue(pid); 
        }
        public void HandleInterrupts()
        {
            foreach (Processor p in processors)
            {
                if(p.isInterrupted())
                {
                    InterruptProcessor(p);
                    p.SwapContexts();
                }
            }
        }

        abstract public void ProcessReadyQueue(int PID);// pushes PID into ready queue, depends on strategy so will be overloaded in subclasses
        // this will need to set state of process
        abstract public Tuple<int, int> ProcessOpenProcessor(int id);//returns PID of process to get processor time// takes in processor id
        abstract public void MarkInterrupts();
        abstract public bool ReadyQueueEmpty();
        abstract public void UpdateReadyQueue();
        abstract public void AddAdditionalMetadata(Run run);

    }
 
}
