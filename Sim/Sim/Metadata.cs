using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sim
{
        public class Metadata
        {
            int submitted;
            int response;

            int burstMarker; //holds value of clock at beginning of current state period
            int execution;
            int wait;
            int io;
            
            int completed;

            int CPUBurstCount; //number of times process assigned to processor
            int timesSwapped; //number of times this process was interrupted
            
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

                //increment respective log value
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
            private int getDuration(int currentTime)  { return currentTime - burstMarker;}
           
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
                     
            public void Swap() { timesSwapped++; }
            public void CPUBurst() { CPUBurstCount++; }
            /////////////////////////////////
            ///////// Set Functions /////////
            /////////////////////////////////
            public void setResponse(int currentTime){response = currentTime - submitted; }
            public void setCompleted(int currentTime) {completed = currentTime; }

            ///////////////////////////////////////
            ///////// Basic Get Functions /////////
            ///////////////////////////////////////
            public int getSubmitted() { return submitted; }
            public int getCompleted() { return completed; }
            public int getCPUBurstCount() { return CPUBurstCount; }
            public int getResponse() { return response; }
            public int getExecution() { return execution; }
            public int getWait() { return wait; }
            public int getIO() { return io; }
            public int getTimesSwapped() { return timesSwapped; }

        }
    
}
