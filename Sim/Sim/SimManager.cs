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


        public SimManager(int numProcessors)
        {
            processes = new Dictionary<int,ProcessControlBlock>();
            subTimes = new List<Tuple<int,int>>();
            IOList = new List<Tuple<int,int>>();
            processors = new List<Processor>();
            clock = 0;
            for(int i = 0; i < numProcessors; i++)
            {
                processors.Add(new Processor(i));
            }
            // Feel free to add functions here to seed data structures

        }

        public void getInfo(Dictionary<int, ProcessControlBlock> procs, List<Tuple<int, int>> subs) // need to hard copy data if running in parallel
        {
            processes = procs;
            subTimes = subs;
            subTimes.Sort();
        }

        public void RunSimulation()
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
                MarkInterrupts(clock);
                //handle interrupted processes
                HandleInterrupts();
                //submit new processes
                CheckForProcesses(subTimes);
                //assign to free processors
                AssignFreeProcessors();
                if(subTimes.Count != 0)
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
        }
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
        abstract public void MarkInterrupts(int currentTime);
        abstract public bool ReadyQueueEmpty();
    }
 
}
