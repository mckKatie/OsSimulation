using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sim
{
    class SimManager
    {
        int clock;
        Dictionary<int, ProcessControlBLock> processes;
        List<Tuple<int, int>> subTimes;


        List<Tuple<int, int>> IOList; //dont know what to call this <outTime, PID>

        public void CheckIOStatus() //check procList for processes ready to be placed in wait queue
        {
            while(true)
            {
                if(IOList[0].Item1 == clock)
                {
                    ProcReadyQueue(IOList[0].Item2);
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
            int burstDuration = temp.bursts[0];
            int burstCompletionTime = clock + burstDuration;

            IOList.Add(new Tuple<int, int>(burstCompletionTime, PID));
            IOList.Sort();
        }

        abstract public void ProcReadyQueue(int PID){} // pushes PID into ready queue, depends on strategy so will be overloaded in subclasses


    }
 
}
