using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sim
{
    abstract class SimManager
    {
        int clock;
        Dictionary<int, ProcessControlBLock> processes;
        List<Tuple<int, int>> subTimes;
        List<Tuple<int, int>> IOList; //dont know what to call this <outTime, PID>
        List<Processor> processors;


        public void CheckProcessorStatus()
        {
            foreach (Processor p in processors)
            {
                p.CheckStatus(clock); // this will toggle processor status to stop if limit is reached
                if(p.getState() == Pstate.stop)
                {
                    int id = p.getID();
                    ProcessControlBLock temp;
                    processes.TryGetValue(id, out temp);
                    if (temp.getState() == state.ioready)   //if burst finished (need to have logic about finishing in PCB)
                    {
                        StartIO(id);
                    }
                    else if(temp.getState() == state.running)   //if interrupted
                    {
                        ProcessReadyQueue(id); // need logic in PCB to adjust bursts vec
                    }
                    p.SwapContexts();  //set state to swapping, busy for one tick
                }
            }
        }

        public void FinishSwaps()//sets processor state to free now that content has been saved
        {
            foreach (Processor p in processors)
            {
                if (p.getState() == Pstate.swapping)
                {
                    p.FreeProcessor();
                }
            }
        }
        public void CheckIOStatus() //check procList for processes ready to be placed in wait queue
        {
            while(true)
            {
                if(IOList[0].Item1 == clock)
                {
                    ProcessReadyQueue(IOList[0].Item2);
                    IOList.RemoveAt(0);
                    continue;
                }
                break;
            }
        }

        public void StartIO(int PID) // looks up processes and places id and time of io completion into list, then sorts
        {
            ProcessControlBLock temp;
            processes.TryGetValue(PID, out temp);
            int burstDuration = temp.getNextBurst();
            int burstCompletionTime = clock + burstDuration;

            IOList.Add(new Tuple<int, int>(burstCompletionTime, PID));
            IOList.Sort();
        }

        public void AssignFreeProcessors()
        {
            foreach (Processor p in processors)
            {
                if( p.getState() == Pstate.open)
                {
                    p.AssignProcess(ProcessOpenProcessor());
                }
            }
        }

        abstract public void ProcessReadyQueue(int PID);// pushes PID into ready queue, depends on strategy so will be overloaded in subclasses
        // this will need to set state of process
        abstract public Tuple<int, int> ProcessOpenProcessor();//returns PID of process to get processor time
    }
 
}
