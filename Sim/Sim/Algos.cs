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
            ProcessControlBLock temp = getProcessByID(pid);
            temp.ProcessorInitiate(clock);
            int burstTime = temp.getNextBurst();
            return new Tuple<int, int>(burstTime + clock, pid);
        }

        public void ProcessReadyQueue(int PID)
        {
            readyQueue.Enqueue(PID);
        }
    }

    class RR : SimManager
    {
        Queue<int> readyQueue;
        int quantum;
        RR() : base()
        {
            numProcessors = 1;
            quantum = 20;
            readyQueue = new Queue<int>();
        }
        public Tuple<int, int> ProcessOpenProcessor()
        {
            int pid = readyQueue.First();
            readyQueue.Dequeue();
            ProcessControlBLock temp = getProcessByID(pid);
            temp.ProcessorInitiate(clock);
            int burstTime = temp.getNextBurst();
            int allocatedTime = Math.Min(burstTime, quantum);
            return new Tuple<int, int>(allocatedTime + clock, pid);
        }
        public void ProcessReadyQueue(int PID)
        {
            readyQueue.Enqueue(PID);
        }
    }

    class SPN : SimManager
    {
        List<Tuple<int, int>> readyList; //burstTime, PID
        SPN() : base()
        {
            numProcessors = 1;
            readyList = new List<Tuple<int, int>>();
        }
        public Tuple<int, int> ProcessOpenProcessor()
        {
            Tuple<int, int> processData = readyList.First();
            readyList.RemoveAt(0);
            ProcessControlBLock temp = getProcessByID(processData.Item2);
            temp.ProcessorInitiate(clock);
            return processData;
        }

        public void ProcessReadyQueue(int PID)
        {
            ProcessControlBLock temp = getProcessByID(PID);
            int burstTime = temp.getNextBurst();
            readyList.Add(new Tuple<int, int>(burstTime, PID));
            readyList.Sort();
        }
    }

    class STR : SimManager
    {
        List<Tuple<int, int>> readyList; //burstTime, PID
        STR() : base()
        {
            numProcessors = 1;
            readyList = new List<Tuple<int, int>>();
        }
        public Tuple<int, int> ProcessOpenProcessor()
        {

        }
        public void ProcessReadyQueue(int PID)
        {
            ProcessControlBLock temp = getProcessByID(PID);
            int burstTime = temp.getNextBurst();
            readyList.Add(new Tuple<int, int>(burstTime, PID));
            readyList.Sort();
        }
    }
}
