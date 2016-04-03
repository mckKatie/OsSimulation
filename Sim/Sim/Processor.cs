using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

enum Pstate { busy, open, stop, swapping, interrupted}
//might be able to replace all instances of stop with swapping

namespace Sim
{
    public class Processor
    {
        Pstate state;
        int processorID;
        int PID;
        int burstCompletionTime; // either because burst is completed or quantum reached

        public Processor(int id){
            state = Pstate.open;
            processorID = id;
        }
        public int getPID(){ return PID; }
        public int getProcID() { return processorID; }
        public bool isSwapping()
        {
            if (state == Pstate.swapping)
                return true;
            return false;
        }
        public bool isBusy()
        {
            if (state == Pstate.busy)
                return true;
            return false;
        }
        public bool isOpen()
        {
            if (state == Pstate.open)
                return true;
            return false;
        }
        public bool isStopped()
        {
            if (state == Pstate.stop)
                return true;
            return false;
        }
        public bool isInterrupted()
        {
            if (state == Pstate.interrupted)
                return true;
            return false;
        }
        public int getCompletionTime() { return burstCompletionTime; }
        public bool BurstCompleteCheck(int currentTime)
        {
            if (burstCompletionTime == currentTime && state == Pstate.busy)
            {
                state = Pstate.swapping;
                return true;
            }
            return false;
        }
        public void AssignProcess(Tuple<int, int> BurstCompletionTime_PID) // burst completion time needs to be set to sooner of burst time and quantum in os strategy
        {
            PID = BurstCompletionTime_PID.Item2;
            burstCompletionTime = BurstCompletionTime_PID.Item1;
            state = Pstate.busy;
        }
        public void SwapContexts()
        {
            state = Pstate.swapping;
        }
        public void FreeProcessor()
        {
            state = Pstate.open;
        }
        public void InterruptProcess()
        {
            state = Pstate.interrupted;
        }
    }
}
