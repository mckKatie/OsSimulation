using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sim
{
    class RoundRobin : SimManager
    {
        int quantumSwitch;
        int quantum = 20;

        int getQuantum()
        {
            return quantum;
        }

        void setQuantum(int x)
        {
            quantum = x;
        }

        void run()
        {
            int quantumSwitch = clock + quantum;
            ProcessControlBLock temp;
            processes.TryGetValue(0, out temp);

        }
    }
}
