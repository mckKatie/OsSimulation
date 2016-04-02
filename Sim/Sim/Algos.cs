using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sim
{
    class FCFS : SimManager
    {
        Queue<int> readyQueue;




        FCFS() : base()
        {
            numProcessors = 1;
            readyQueue = new Queue<int>();
        }

        public Tuple<int, int> ProcessOpenProcessor() // returns <PID, endAllocatedTime>
        {
            int pid = readyQueue.First();
            readyQueue.Dequeue();
            ProcessControlBLock temp;
            processes.TryGetValue(pid, out temp);
            int burstTime = temp.getNextBurst();
            return new Tuple<int, int>(pid, burstTime+clock);
        }

        public void ProcessReadyQueue(int PID)
        {
            readyQueue.Enqueue(PID);
        }


        
    }
}
