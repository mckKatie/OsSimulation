
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



enum state {running, finished, io, ready}

namespace Sim
{
    public class ProcessControlBlock
    {
        int PID;
        List<int> burstMemory; //deep copied list of burst values, persists between simulations
        state currentState; //one of above emun values
        List<int> bursts; //list of burst times yet to be completed
        Metadata log; //stores metadata about this process
        public ProcessControlBlock(int submitTime, int _PID, List<int> _bursts)
        {
            PID = _PID;
            currentState = state.ready;
            burstMemory = _bursts;   //this is referentially assigned
            bursts = new List<int>();
            for (int i = 0; i < burstMemory.Count; i++)
            {
                bursts.Add(burstMemory[i]);
            }
            log = new Metadata(submitTime);
        }

        //////////// Reset Called Between Simulation Runs
        //////////// resets burst List and wipes Metadata
        public void ResetPCB()
        {
            bursts = new List<int>();
            for (int i = 0; i < burstMemory.Count; i++)
            {
                bursts.Add(burstMemory[i]);
            }
            currentState = state.ready;
            log.ClearLog();
        }

        ///////////////////////////////////////////
        //////////// PCB state Queries ////////////
        ///////////////////////////////////////////
        public bool isRunning()
        {
            if (currentState == state.running)
                return true;
            return false;
        }
        public bool isFinished()
        {
            if (currentState == state.finished)
                return true;
            return false;
        }
        public bool isIO()
        {
            if (currentState == state.io)
                return true;
            return false;
        }
        public bool isReady()
        {
            if (currentState == state.ready)
                return true;
            return false;
        }

        public int getNextBurst() { return bursts[0]; }
        public void ProcessorInitiate(int currentTime)
        {
            log.UpdateLog(this, currentTime);
            if (log.getResponse() == -1)
            {
                log.setResponse(currentTime);
            }
            currentState = state.running;
            log.CPUBurst();
        }
        ////////////////////////////////////////////////
        //////////// State Change Functions ////////////
        ////////////////////////////////////////////////
        public void CPUFinish(int currentTime)
        {
            log.UpdateLog(this, currentTime);
            bursts.RemoveAt(0);
            if(bursts.Count == 0)
            {
                currentState = state.finished;
                Terminate(currentTime);
            }
            else
                currentState = state.io;
        }
        public void CPUInterrupt(int currentTime)
        {
            int workDone = log.UpdateLog(this, currentTime);
            bursts[0] -= workDone;
            currentState = state.ready;
            log.Swap();
        }
        public void IOFinish(int currentTime)
        {
            log.UpdateLog(this, currentTime);
            bursts.RemoveAt(0);
            if(bursts.Count == 0)
            {
                currentState = state.finished;
                Terminate(currentTime);
            }
            else
                currentState = state.ready;
        }
        private void Terminate(int currentTime)
        {
            log.setCompleted(currentTime);
        }
        ////////////////////////////////////
        //////////// Basic Gets ////////////
        ////////////////////////////////////
        public Metadata getLog() { return log; }
        public int getSubmitted() { return log.getSubmitted(); }
        public int getPID() { return PID; }
    }
}

