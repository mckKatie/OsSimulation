using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public enum Strategy { FCFS, RR, SPN, STR, HRRN }

namespace Sim
{
    public class FCFS : SimManager
    {
        Queue<int> readyQueue;

        public FCFS(string filePath, int numProcessors)
            : base(filePath, numProcessors, Strategy.FCFS)
        {
            readyQueue = new Queue<int>();
        }

        override public Tuple<int, int> ProcessOpenProcessor(int id) // returns <PID, endAllocatedTime> , takes processor id
        {
            int pid = readyQueue.First();
            readyQueue.Dequeue();
            ProcessControlBlock temp = getProcessByID(pid);
            temp.ProcessorInitiate(clock);
            int burstTime = temp.getNextBurst();
            return new Tuple<int, int>(burstTime + clock, pid);
        }

        override public void ProcessReadyQueue(int PID)
        {
            readyQueue.Enqueue(PID);
        }

        override public bool ReadyQueueEmpty()
        {
            if (readyQueue.Count == 0)
                return true;
            return false;
        }
        public override void MarkInterrupts() {}
        public override void AddAdditionalMetadata(Run run) { }
    }

    public class RR : SimManager
    {
        Queue<int> readyQueue;
        List<int> processorQuantumEnd;
        int quantum;
        public RR(string filePath, int numProcessors, int _quantum) : base(filePath, numProcessors, Strategy.RR)
        {
            quantum = _quantum;
            readyQueue = new Queue<int>();
            processorQuantumEnd = new List<int>();
            for(int i = 0; i < processors.Count; i++ )
            {
                processorQuantumEnd.Add(0);
            }
        }
        override public Tuple<int, int> ProcessOpenProcessor(int id) //processor id
        {
            int pid = readyQueue.First();
            readyQueue.Dequeue();
            ProcessControlBlock temp = getProcessByID(pid);
            temp.ProcessorInitiate(clock);
            int burstTime = temp.getNextBurst();
            processorQuantumEnd[id] = (quantum + clock);
            return new Tuple<int, int>(burstTime + clock, pid);
        }
        override public void ProcessReadyQueue(int PID)
        {
            readyQueue.Enqueue(PID);
        }
        override public bool ReadyQueueEmpty()
        {
            if (readyQueue.Count == 0)
                return true;
            return false;
        }
        override public void MarkInterrupts()
        {
            int numWaiting = readyQueue.Count;
            for(int i = 0; i < processors.Count; i++)
            {
                if(processorQuantumEnd[i] == clock && processors[i].isBusy() && numWaiting > 0)
                {
                    processors[i].InterruptProcess();
                    numWaiting--;
                }
            }
        }
        override public void AddAdditionalMetadata(Run run)
        {
            run.setQuantum(quantum);
        }
    }

    public class SPN : SimManager
    {
        List<Tuple<int, int>> readyList; //burstTime, PID
        public SPN(string filePath, int numProcessors) : base(filePath, numProcessors, Strategy.SPN)
        {
            readyList = new List<Tuple<int, int>>();
        }
        override public Tuple<int, int> ProcessOpenProcessor(int id)
        {
            Tuple<int, int> processData = readyList.First();
            readyList.RemoveAt(0);
            ProcessControlBlock temp = getProcessByID(processData.Item2);
            temp.ProcessorInitiate(clock);
            return new Tuple<int,int>(processData.Item1 + clock, processData.Item2);
        }

        override public void ProcessReadyQueue(int PID)
        {
            ProcessControlBlock temp = getProcessByID(PID);
            int burstTime = temp.getNextBurst();
            readyList.Add(new Tuple<int, int>(burstTime, PID));
            readyList.Sort();
        }
        override public bool ReadyQueueEmpty()
        {
            if (readyList.Count == 0)
                return true;
            return false;
        }
        override public void MarkInterrupts() { }
        override public void AddAdditionalMetadata(Run run) { }
    }

    public class STR : SimManager
    {
        List<Tuple<int, int>> readyList; //burstTime, PID
        public STR(string filePath, int numProcessors) : base(filePath, numProcessors, Strategy.STR)
        {
            readyList = new List<Tuple<int, int>>();
        }
        override public Tuple<int, int> ProcessOpenProcessor(int id) //returns <PID, endAllocatedTime> , takes processor id
        {
            Tuple<int, int> processData = readyList.First();
            readyList.RemoveAt(0);
            ProcessControlBlock temp = getProcessByID(processData.Item2);
            temp.ProcessorInitiate(clock);
            return new Tuple<int, int>(processData.Item1 + clock, processData.Item2);
        }
        override public void ProcessReadyQueue(int PID)
        {
            ProcessControlBlock temp = getProcessByID(PID);
            int burstTime = temp.getNextBurst();
            readyList.Add(new Tuple<int, int>(burstTime, PID));
            readyList.Sort();
        }
        override public bool ReadyQueueEmpty()
        {
            if (readyList.Count == 0)
                return true;
            return false;
        }
        override public void MarkInterrupts()
        {
            List<int> completionTimes = new List<int>();
            foreach (Processor p in processors)
            {
                if (p.isBusy())
                {
                    completionTimes.Add(p.getCompletionTime());
                }
            }
            for(int i = 0; i < processors.Count && readyList.Count > i; i++)
            {
                completionTimes.Add(readyList[i].Item1 + clock);
            }
            completionTimes.Sort();
            if (completionTimes.Count > processors.Count)
            {
                int interruptMarker = completionTimes[processors.Count - 1];
                foreach (Processor p in processors)
                {
                    if (p.getCompletionTime() > interruptMarker && p.isBusy())
                    {
                        p.InterruptProcess();
                    }
                }
            }
        }
        override public void AddAdditionalMetadata(Run run) { }
    }

    public class HRRN : SimManager
    {
        List<Tuple<double, int, int>> readyList; //ratio, pid, readyListArrivalTime
        public HRRN(string filePath, int numProcessors): base(filePath, numProcessors, Strategy.HRRN)
        {
            readyList = new List<Tuple<double, int, int>>();
        }
        override public Tuple<int, int> ProcessOpenProcessor(int id) // returns <PID, endAllocatedTime> , takes processor id
        {
            int pid = readyList[0].Item2;
            readyList.RemoveAt(0);
            ProcessControlBlock temp = getProcessByID(pid);
            temp.ProcessorInitiate(clock);
            int burstTime = temp.getNextBurst();
            return new Tuple<int, int>(burstTime + clock, pid);
        }
        override public bool ReadyQueueEmpty()
        {
            if (readyList.Count == 0)
                return true;
            return false;
        }
        public override void MarkInterrupts() { }
    }
}
