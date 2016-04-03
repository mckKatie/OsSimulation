using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sim
{
    public class ReducedAnalysis
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
            int numRuns;
            public StrategyInfo(Strategy _strat, int _numProc, List<int> _q)
            {
                strat = _strat;
                numProcessors = _numProc;
                quantums = _q;
                turnaroundTime = 0;
                waitTime = 0;
                simulationTime = 0;
                throughput = 0;
                responseTime = 0;
                numRuns = 0;
            }
            public void Reduced(List<Run> runs)
            {
                foreach (Run r in runs)
                {
                    turnaroundTime += r.getTurnaround();
                    waitTime += r.getWait();
                    simulationTime += r.getSimulation();
                    responseTime += r.getResponse();
                    throughput += r.getThroughput();
                }
                turnaroundTime /= runs.Count;
                waitTime /= runs.Count;
                simulationTime /= runs.Count;
                responseTime /= runs.Count;
                throughput /= runs.Count;
                numRuns = runs.Count;
            }
        }
        MultiMap<Run> runsGrouped;
        List<StrategyInfo> reducedData;
        public ReducedAnalysis()
        {
            runsGrouped = new MultiMap<Run>();
            reducedData = new List<StrategyInfo>();
        }
        public List<StrategyInfo> getReducedData()
        {
            return reducedData;
        }
        public void ComputeReducedAverages(List<Run> runs)
        {
            foreach (Run r in runs)
            {
                Tuple<Strategy, int, List<int>> key = r.getKey();
                runsGrouped.Add(key,r);
            }
            foreach(Tuple<Strategy, int, List<int>> key in runsGrouped.Keys)
            {
                StrategyInfo temp = new StrategyInfo(key.Item1, key.Item2, key.Item3);
                temp.Reduced(runsGrouped[key]);
                reducedData.Add(temp);
            }
        }
    }
}
