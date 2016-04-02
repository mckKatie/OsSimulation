
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



enum state {running, ready, io}

namespace Sim
{
    public class ProcessControlBLock
    {
        int PID;
        state currentState;
        public List<int> bursts;
        public Metadata log;

        ProcessControlBLock(int currentTime, int _PID, List<int> _bursts)
        {
            PID = _PID;
            currentState = state.ready;
            bursts = _bursts;   //this is referentially assigned
            log = new Metadata(currentTime);
        }

        public int ProcessorInitiate(int currentTime)
        {
            log.UpdateLog(currentState, currentTime);
            if (log.getResponse() == -1)
            {
                log.setResponse(currentTime);
            }
            currentState = state.running;
            return bursts[0];
        }

        public int IOInitiate(int currentTime)
        {
            return bursts[0];
        }

        public void FinishBurst(int currentTime)
        {
            log.UpdateLog(currentState, currentTime);
            bursts.RemoveAt(0);
            if(currentState == state.running)
            {
                currentState = state.io;
            }
            if(currentState == state.io)
            {
                currentState = state.ready;
            }
        }

        public void InterruptCPUBurst(int currentTime)
        {
            int workDone = log.UpdateLog(currentState, currentTime);
            bursts[0] -= workDone;
            currentState = state.ready;
        }
    }
}

