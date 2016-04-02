﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sim
{
    public class FCFS : SimManager
    {
        Queue<int> readyQueue;

        public FCFS(int numProcessors)
            : base(numProcessors)
        {
            readyQueue = new Queue<int>();
        }

        override public Tuple<int, int> ProcessOpenProcessor(int id) // returns <PID, endAllocatedTime> , takes processor id
        {
            int pid = readyQueue.First();
            readyQueue.Dequeue();
            ProcessControlBLock temp = getProcessByID(pid);
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
        public override void MarkInterrupts(int currentTime) {}
    }

    class RR : SimManager
    {
        Queue<int> readyQueue;
        List<int> processorQuantumEnd;
        int quantum;
        public RR(int numProcessors)
            : base(numProcessors)
        {
            quantum = 20;
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
            ProcessControlBLock temp = getProcessByID(pid);
            temp.ProcessorInitiate(clock);
            int burstTime = temp.getNextBurst();
            processorQuantumEnd[id].Equals(quantum + clock);
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
        override public void MarkInterrupts(int currentTime)
        {
            for(int i = 0; i < processors.Count; i++)
            {
                if(processorQuantumEnd[i] == currentTime)
                {
                    processors[i].InterruptProcess();
                }
            }
        }
    }

    class SPN : SimManager
    {
        List<Tuple<int, int>> readyList; //burstTime, PID
        SPN(int numProcessors)
            : base(numProcessors)
        {
            readyList = new List<Tuple<int, int>>();
        }
        override public Tuple<int, int> ProcessOpenProcessor(int id)
        {
            Tuple<int, int> processData = readyList.First();
            readyList.RemoveAt(0);
            ProcessControlBLock temp = getProcessByID(processData.Item2);
            temp.ProcessorInitiate(clock);
            return processData;
        }

        override public void ProcessReadyQueue(int PID)
        {
            ProcessControlBLock temp = getProcessByID(PID);
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
        override public void MarkInterrupts(int currentTime) { }
    }

    //class STR : SimManager
    //{
    //    List<Tuple<int, int>> readyList; //burstTime, PID
    //    STR() : base()
    //    {
    //        numProcessors = 1;
    //        readyList = new List<Tuple<int, int>>();
    //    }
    //    override public Tuple<int, int> ProcessOpenProcessor()
    //    {

    //    }
    //    override public void ProcessReadyQueue(int PID)
    //    {
    //        ProcessControlBLock temp = getProcessByID(PID);
    //        int burstTime = temp.getNextBurst();
    //        readyList.Add(new Tuple<int, int>(burstTime, PID));
    //        readyList.Sort();
    //    }
    //    //public void CheckToReplaceProcess()
    //    //{
    //    //    foreach (Processor p in processors)
    //    //    {
    //    //        if
    //    //    }
    //    //}
    //}
}
