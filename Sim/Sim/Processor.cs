using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum Pstate { busy, open, stop, swapping}


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
        public int getID(){ return PID; }
        public Pstate getState(){ return state;}
        public void CheckStatus(int currentTime)
        {
            if (burstCompletionTime == currentTime)
            {
                state = Pstate.stop;
            }
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
    }
}
