using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sim
{
    public static class ReducedAnalysis
    {
        struct StrategyInfo
        {
            Strategy strat;
            int numProcessors;
            List<int> quantums;
            double turnaroundTime;
            double waitTime;
            double simulationTime;
            double throughput;
            double responseTime;
        }
        public static void ComputeReducedAverages(List<Run> runs)
        {

        }
    }
}
