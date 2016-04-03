using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sim
{
        public class Metadata
        {
            public int submitted;
            public int completed;
            public int CPUBurstCount;
            public int response;
            public int execution;
            public int wait;
            public int io;
            public int timesSwapped;
            public int burstMarker;
            public Metadata(int submitTime)
            {
                submitted = submitTime;
                execution = 0;
                wait = 0;
                io = 0;
                timesSwapped = 0;
                CPUBurstCount = 0;
                response = -1;
                burstMarker = submitted;
            }

            public int UpdateLog(ProcessControlBlock pcb, int currentTime)
            {
                int prevBurstDuration = getDuration(currentTime);
                burstMarker = currentTime;  // reassign burstmarker to start tracking the next burst/wait period

                if (pcb.isReady())
                {
                    wait += prevBurstDuration;
                }
                else if (pcb.isRunning())
                {
                    execution += prevBurstDuration;
                }
                else if (pcb.isIO())
                {
                    io += prevBurstDuration;
                }
                return prevBurstDuration;
            }
            public void ClearLog() 
            {
                execution = 0;
                wait = 0;
                io = 0;
                timesSwapped = 0;
                CPUBurstCount = 0;
                response = -1;
                burstMarker = submitted;
                completed = 0;
            }

            private int getDuration(int currentTime)  { return currentTime - burstMarker;}
            public int getResponse(){return response; }
            public void setResponse(int currentTime){response = currentTime - submitted; }
            public void setCompleted(int currentTime) {completed = currentTime; }
            public int getCompleted() { return completed; }

        }
    
}
