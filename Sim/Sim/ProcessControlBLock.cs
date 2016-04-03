
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
        state currentState;
        List<int> bursts;
        
        public Metadata log;
        public ProcessControlBlock(int submitTime, int _PID, List<int> _bursts)
        {
            PID = _PID;
            currentState = state.ready;
            bursts = _bursts;   //this is referentially assigned
            log = new Metadata(submitTime);
        }
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
        }

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
            log.timesSwapped++;
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

        private void Terminate(int currentTime)    //compute final values
        {
            log.setCompleted(currentTime);
        }


    }
}

