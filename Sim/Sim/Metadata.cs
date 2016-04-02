﻿using System;
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

        public int response;
        public int execution;
        public int wait;
        public int io;

        public int burstMarker;

        public Metadata(int submitTime)
        {
            submitted = submitTime;
            execution = 0;
            wait = 0;
            io = 0;

            response = -1;

            burstMarker = submitted;
        }

        public int UpdateLog(state currentState, int currentTime)
        {
            int prevBurstDuration = getDuration(currentTime);
            burstMarker = currentTime;  // reassign burstmarker to start tracking the next burst/wait period

            if (currentState == state.ready)
            {
                wait += prevBurstDuration;
            }
            else if (currentState == state.running)
            {
                execution += prevBurstDuration;
            }
            else if (currentState == state.io)
            {
                io += prevBurstDuration;
            }
            return prevBurstDuration; 
        }

        private int getDuration(int currentTime)
        {
            return currentTime - burstMarker;
        }



        public int getResponse()
        {
            return response;
        }
        public void setResponse(int currentTime)
        {
            response = currentTime - submitted;
        }
        public void setCompleted(int currentTime)
        {
            completed = currentTime;
        }
        public int getCompleted()
        {
            return completed;
        }
    }
}
