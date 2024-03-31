using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Riddles.Optimization
{
    public class WaterPouringSolver
    {
        public WaterPouringSolver() { 
        }

        public (int, int) CalculateNumberOfMovesForSlowestValue(List<int> pitcherSizes)
        {
            var solutionDictionary = this.CalculateNumMovesToFindExactlyNLitersOfWater(
                pitcherSizes
            );
            var slowestPairSorted = solutionDictionary.Select(k => k)
                .OrderBy(k => k.Key);
            var slowestValuePair = solutionDictionary.Select(k => k)
                .OrderByDescending(k => k.Value).First();
            return (slowestValuePair.Key, slowestValuePair.Value);
        }

        public Dictionary<int, int> CalculateNumMovesToFindExactlyNLitersOfWater(
            List<int> pitcherSizes
        )
        {
            var pitchers = pitcherSizes.Select(s => new WaterPitcher(s, 0)).ToList();
            Dictionary<int, int> numMovesRequiredToFindNLiters = new Dictionary<int, int>();
            HashSet<string> cache = new HashSet<string>();
            List<(List<WaterPitcher>, int)> stateQueue 
                = new List<(List<WaterPitcher>, int)> ();
            this.AddToQueueIfNewState(
                pitchers,
                stateQueue,
                cache,
                0
            );
            while (stateQueue.Count > 0 )
            {
                var state = stateQueue[0];
                var currentPitchers = state.Item1;
                // update the dictionary for the current state being processed
                foreach (var pitcher in currentPitchers)
                {
                    if (!numMovesRequiredToFindNLiters.ContainsKey(pitcher.Amount))
                    {
                        numMovesRequiredToFindNLiters[pitcher.Amount] = state.Item2;
                    }
                }

                for(int i=0; i<currentPitchers.Count(); i++)
                {
                    // fill i
                    if (currentPitchers[i].Amount != currentPitchers[i].Capacity)
                    {
                        var newPitcherState =
                            currentPitchers.Select(
                                (p, x) => x == i
                                    ? new WaterPitcher(p.Capacity, p.Capacity)
                                    : new WaterPitcher(p.Capacity, p.Amount)
                            ).ToList();
                        this.AddToQueueIfNewState(
                                newPitcherState,
                                stateQueue,
                                cache,
                                state.Item2 + 1
                            );
                    }

                    // empty i
                    if (currentPitchers[i].Amount != 0)
                    {
                        var newPitcherState =
                            currentPitchers.Select(
                                (p, x) => x == i
                                    ? new WaterPitcher(p.Capacity, 0)
                                    : new WaterPitcher(p.Capacity, p.Amount)
                            ).ToList();
                        this.AddToQueueIfNewState(
                                newPitcherState,
                                stateQueue,
                                cache,
                                state.Item2 + 1
                            );
                    }

                    for(int j=0; j<currentPitchers.Count(); j++)
                    {
                        // pour the contents of i into j
                        if(i != j)
                        {
                            var changeInCapacity = Math.Min(
                                currentPitchers[i].Amount,
                                currentPitchers[j].Capacity - currentPitchers[j].Amount
                            );
                            var newPitcherState = state.Item1.Select(
                                (p, x) =>
                                {
                                    if (x == i)
                                    {
                                        return new WaterPitcher(p.Capacity,
                                            p.Amount - changeInCapacity);
                                    }
                                    else if (x == j)
                                    {
                                        return new WaterPitcher(p.Capacity,
                                            p.Amount + changeInCapacity);
                                    }
                                    return new WaterPitcher(p.Capacity, p.Amount);
                                }
                            ).ToList();
                            this.AddToQueueIfNewState(
                                newPitcherState,
                                stateQueue,
                                cache,
                                state.Item2 + 1
                            );
                        }
                    }
                }
                stateQueue.RemoveAt(0);
            }

            return numMovesRequiredToFindNLiters;
        }

        private void AddToQueueIfNewState(
            List<WaterPitcher> currentState,
            List<(List<WaterPitcher>, int)> queue,
            HashSet<string> cache, 
            int numMoves)
        {
            var cacheKey = currentState.Aggregate("", (key, p) => $"{key}{p.Amount}_");
            if (!cache.Contains(cacheKey))
            {
                cache.Add(cacheKey);
                queue.Add((currentState, numMoves));
            }
        }

        public class WaterPitcher
        {
            public WaterPitcher(int capacity, int amount)
            {
                this.Capacity = capacity;
                this.Amount = amount;
            }

            public int Capacity { get; private set; }
            public int Amount { get; private set; }
        }
    }
}