
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



enum state {running, ioready, io, ready}

namespace Sim
{
    class ProcessControlBLock
    {
        int PID;
        state currentState;
        public List<int> bursts;
        
        public Metadata log;
        ProcessControlBLock(int submitTime, int _PID, List<int> _bursts)

        {
            PID = _PID;
            currentState = state.ready;
            bursts = _bursts;   //this is referentially assigned
            log = new Metadata(submitTime);
        }
        public state getState() { return currentState; }
        public int getNextBurst() { return bursts[0]; }
        public void ProcessorInitiate(int currentTime)
        {
            log.UpdateLog(currentState, currentTime);
            if (log.getResponse() == -1)
            {
                log.setResponse(currentTime);
            }
            currentState = state.running;
        }

        public void CPUFinish(int currentTime)
        {
            log.UpdateLog(currentState, currentTime);
            bursts.RemoveAt(0);
            currentState = state.ioready;
        }

        public void CPUInterrupt(int currentTime)
        {
            int workDone = log.UpdateLog(currentState, currentTime);
            bursts[0] -= workDone;
            currentState = state.ready;
        }

        public void IOFinish(int currentTime)
        {
            log.UpdateLog(currentState, currentTime);
            bursts.RemoveAt(0);
            currentState = state.ready;
        }
    }
}

