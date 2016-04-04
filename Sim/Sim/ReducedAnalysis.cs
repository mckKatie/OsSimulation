using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sim
{
    public class ReducedAnalysis
    {
        
        MultiMap runsGrouped; //used to Reduce data
        List<StrategyInfo> reducedData; //List of Reduced data
        public ReducedAnalysis(List<Run> runsList)
        {
            runsGrouped = new MultiMap();
            reducedData = new List<StrategyInfo>();
            ComputeReducedAverages(runsList);
        }

        private void ComputeReducedAverages(List<Run> runs)
        {
            foreach (Run r in runs)
            {
                Tuple<Strategy, int> key = r.getKey();
                runsGrouped.Add(key,r);
            }
            foreach(Tuple<Strategy, int> key in runsGrouped.Keys)
            {
                StrategyInfo temp = new StrategyInfo(key.Item1, key.Item2, runsGrouped[key][0].getQuantum());
                temp.Reduce(runsGrouped[key]);
                reducedData.Add(temp);
            }
        }
        public List<StrategyInfo> getReducedData()
        {
            return reducedData;
        }
    }
  
    public struct StrategyInfo
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
        
        public StrategyInfo(Strategy _strat, int _numProc, List<int>_q)
        {
            quantums = _q;
            strat = _strat;
            numProcessors = _numProc;
            turnaroundTime = 0;
            waitTime = 0;
            simulationTime = 0;
            throughput = 0;
            responseTime = 0;
            numRuns = 0;
        }
        public void Reduce(List<Run> runs)
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

        ///////////////////////////
        ////// Get Functions //////
        ///////////////////////////
        public string getStrategy(){return strat.ToString();}
        public int getNumProcessors() {return numProcessors;}
        public List<int> getQuantums() { return quantums; }
        public double getTurnaroundTime() { return turnaroundTime; }
        public double getWaitTime() { return waitTime; }
        public double getSimulationTime() { return simulationTime; }
        public double getThroughput() { return throughput; }
        public double getResponseTime() { return responseTime; }
        public int getNumRuns() { return numRuns; }
    }
}
