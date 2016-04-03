using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public enum Strategy { FCFS, RR, SPN, STR, HRRN, MLFB }

namespace Sim
{
    public class FCFS : SimManager
    {
        Queue<int> readyQueue;

        public FCFS(string filePath, int numProcessors, List<int> quantum)
            : base(filePath, numProcessors, Strategy.FCFS, quantum)
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
        public override void UpdateReadyQueue() { }      
        override public bool ReadyQueueEmpty()
        {
            if (readyQueue.Count == 0)
                return true;
            return false;
        }
        public override void MarkInterrupts() {}
        public override void AddTierMapping(int id) { }

    }

    public class RR : SimManager
    {
        Queue<int> readyQueue;
        List<int> processorQuantumEnd;
        public RR(string filePath, int numProcessors, List<int> quantum) : base(filePath, numProcessors, Strategy.RR, quantum)
        {
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
            processorQuantumEnd[id] = (quantum[0] + clock);
            return new Tuple<int, int>(burstTime + clock, pid);
        }
        override public void ProcessReadyQueue(int PID)
        {
            readyQueue.Enqueue(PID);
        }
        public override void UpdateReadyQueue(){}
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
        public override void AddTierMapping(int id) { }

    }

    public class SPN : SimManager
    {
        List<Tuple<int, int>> readyList; //burstTime, PID
        public SPN(string filePath, int numProcessors, List<int> quantum) : base(filePath, numProcessors, Strategy.SPN, quantum)
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
        }
        public override void UpdateReadyQueue()
        {
            readyList.Sort();
        }
        override public bool ReadyQueueEmpty()
        {
            if (readyList.Count == 0)
                return true;
            return false;
        }
        override public void MarkInterrupts() { }
        public override void AddTierMapping(int id) { }

    }

    public class STR : SimManager
    {
        List<Tuple<int, int>> readyList; //burstTime, PID
        public STR(string filePath, int numProcessors, List<int> quantum) : base(filePath, numProcessors, Strategy.STR, quantum)
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
        }
        public override void UpdateReadyQueue()
        {
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
        public override void AddTierMapping(int id) { }

    }

    public class HRRN : SimManager
    {
        struct ReadyQueueEntry
        {
            double value;
            int arrivalTime;
            int burstTime;
            int PID;

            public ReadyQueueEntry(int _PID, int _burstTime, int currentTime)
            {
                PID = _PID;
                arrivalTime = currentTime;
                burstTime = _burstTime;
                value = 0;
                ComputeRatio(currentTime);
            }
            public void ComputeRatio(int currentTime)
            {
                value = ((burstTime + currentTime - arrivalTime) / (double) burstTime);
            }
            public int getPID() { return PID; }
            public int getBurstTime(){return burstTime;}
            public double getValue() { return value; }
        }
        List<ReadyQueueEntry> readyList;
        public HRRN(string filePath, int numProcessors, List<int>quantum): base(filePath, numProcessors, Strategy.HRRN, quantum)
        {
            readyList = new List<ReadyQueueEntry>();
        }
        
        override public Tuple<int, int> ProcessOpenProcessor(int id) // returns <PID, endAllocatedTime> , takes processor id
        {
            int pid = readyList[0].getPID();
            int burstTime = readyList[0].getBurstTime();
            readyList.RemoveAt(0);
            ProcessControlBlock temp = getProcessByID(pid);
            temp.ProcessorInitiate(clock);
            return new Tuple<int, int>(burstTime + clock, pid);
        }
        override public bool ReadyQueueEmpty()
        {
            if (readyList.Count == 0)
                return true;
            return false;
        }
        override public void ProcessReadyQueue(int PID)
        {
            ProcessControlBlock temp = getProcessByID(PID);
            int burstTime = temp.getNextBurst();
            readyList.Add(new ReadyQueueEntry(PID, burstTime, clock));
        }
        public override void UpdateReadyQueue()
        {
            foreach(ReadyQueueEntry rqe in readyList)
            {
                rqe.ComputeRatio(clock);
            }
            readyList = readyList.OrderByDescending(v => v.getValue()).ToList();
        }
        override public void MarkInterrupts() { }
        public override void AddTierMapping(int id){ }
       
    }
    public class MLFB : SimManager
    {
        Dictionary<int, int> processTierMap;
        List<Queue<int>> queueList;
        List<int> processorQuantumEnd;

        public MLFB(string filePath, int numProcessors, List<int> quantum)
            : base(filePath, numProcessors, Strategy.MLFB, quantum)
        {

            queueList = new List<Queue<int>>();
            for (int i = 0; i < quantum.Count + 1; i++)
                queueList.Add(new Queue<int>());

            processorQuantumEnd = new List<int>();
            for (int i = 0; i < processors.Count; i++)
            {
                processorQuantumEnd.Add(0);
            }
            processTierMap = new Dictionary<int, int>();

        }

        private int getQueueVolume()
        {
            int num = 0;
            foreach (Queue<int> q in queueList)
            {
                num += q.Count;
            }
            return num;
        }
        override public Tuple<int, int> ProcessOpenProcessor(int id) //processor id
        {
            int tier = 0;
            for (; tier < queueList.Count; tier++)
            {
                if (queueList[tier].Count != 0)
                {
                    int pid = queueList[tier].Dequeue();
                    ProcessControlBlock temp = getProcessByID(pid);
                    temp.ProcessorInitiate(clock);
                    int burstTime = temp.getNextBurst();
                    int quantumDuration;
                    if (tier == quantum.Count)
                        quantumDuration = -1;
                    else
                        quantumDuration = quantum[tier];
                    processorQuantumEnd[id] = (quantumDuration + clock);
                    return new Tuple<int, int>(burstTime + clock, pid);
                }
            }
            return null;
        }
        override public void ProcessReadyQueue(int PID)//finished
        {
            queueList[processTierMap[PID]].Enqueue(PID);
        }
        public override void UpdateReadyQueue() { }
        override public bool ReadyQueueEmpty() //finished
        {
            foreach (Queue<int> q in queueList)
            {
                if (q.Count != 0)
                    return false;
            }
            return true;
        }
        override public void MarkInterrupts()
        {
            int numWaiting = getQueueVolume();
            for (int i = 0; i < processors.Count; i++)
            {
                if (processorQuantumEnd[i] == clock && processors[i].isBusy() && numWaiting > 0)
                {
                    processTierMap[processors[i].getPID()] = processTierMap[processors[i].getPID()] + 1;
                    processors[i].InterruptProcess();
                    numWaiting--;
                }
            }
        }
        override public  void AddTierMapping(int id)
        {
            processTierMap.Add(id, 0);
        }
    }
}
