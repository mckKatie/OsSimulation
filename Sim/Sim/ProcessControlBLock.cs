
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
        List<int> burstMemory;
        state currentState;
        List<int> bursts;
        
        public Metadata log;
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
            log.CPUBurstCount++;
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


        public int getSubmitted() { return log.submitted; }
        public int getPID() { return PID; }
        public List<int> getBursts()
        {
            List<int> temp = new List<int>();
            for(int i = 0; i < bursts.Count; i++)
            {
                temp.Add(bursts[i]);
            }
            return temp;
        }
    }
}

