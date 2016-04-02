using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

enum Pstate { busy, open, stop, swapping}

namespace Sim
{
    class Processor
    {
        Pstate state;
        int PID;
        int burstCompletionTime; // either because burst is completed or quantum reached

        Processor(){
            state = Pstate.open;
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

        public void AssignProcess(Tuple<int, int> PID_BurstCompletionTime) // burst completion time needs to be set to sooner of burst time and quantum in os strategy
        {
            PID = PID_BurstCompletionTime.Item1;
            burstCompletionTime = PID_BurstCompletionTime.Item2;
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
