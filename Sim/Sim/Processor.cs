using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

enum Pstate { busy, open, swapping, interrupted}

namespace Sim
{
    public class Processor
    {
        Pstate state;
        int processorID;
        int PID;
        int burstCompletionTime; //time at which burst will be completed

        public Processor(int id){
            state = Pstate.open;
            processorID = id;
        }

        ///////////////////////////
        ////// Functionality //////
        ///////////////////////////
        public bool BurstCompleteCheck(int currentTime)
        {
            if (burstCompletionTime == currentTime && state == Pstate.busy)
            {
                state = Pstate.swapping;
                return true;
            }
            return false;
        }
        public void AssignProcess(Tuple<int, int> assignment) // assignment = <BurstCompletionTime, PID>
        {
            PID = assignment.Item2;
            burstCompletionTime = assignment.Item1;
            state = Pstate.busy;
        }

        //////////////////////////
        /////// Basic Gets ///////
        //////////////////////////
        public int getPID(){ return PID; }
        public int getProcID() { return processorID; }
        public int getCompletionTime() { return burstCompletionTime; }
        
        /////////////////////////////////
        ///////// State Queries /////////
        /////////////////////////////////
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
        public bool isInterrupted()
        {
            if (state == Pstate.interrupted)
                return true;
            return false;
        }

        //////////////////////////////////////////////
        ///////// State Transition Functions /////////
        //////////////////////////////////////////////
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
