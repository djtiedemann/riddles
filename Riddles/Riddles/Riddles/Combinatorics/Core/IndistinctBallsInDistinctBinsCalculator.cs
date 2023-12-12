using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Riddles.Combinatorics.Core
{
    public class IndistinctBallsInDistinctBinsCalculator
    {
        private Dictionary<(string, int), List<string>> _cache;
        private Dictionary<(int, int), int> _numWaysCache;
        public IndistinctBallsInDistinctBinsCalculator() { 
            this._cache= new Dictionary<(string, int), List<string>>();
            this._numWaysCache= new Dictionary<(int, int), int>();
        }
        
        public int CountNumWaysToPlaceBallsInBins(int numBalls, int numBins)
        {
            if(numBalls == 0 || numBins == 0)
            {
                return 0;
            }
            // if there is 1 bin, must put all balls in that bin
            if (numBins == 1)
            {
                return 1;
            }
            // if there is 1 ball left, can place it in each of the bins
            if (numBalls == 1)
            {
                return numBins;
            }
            // two options:
            // 1) place a ball in the first bin, continue with 1 fewer ball
            // 2) discard the first bin, continue with 1 fewer bin
            if(!this._numWaysCache.ContainsKey((numBalls, numBins))) {
                this._numWaysCache[(numBalls, numBins)] =
                    this.CountNumWaysToPlaceBallsInBins(numBalls - 1, numBins) +
                        this.CountNumWaysToPlaceBallsInBins(numBalls, numBins - 1);
            }
            return this._numWaysCache[(numBalls, numBins)];
        }

        public List<string> DetermineWaysToPlaceBallsInBins(
            int numBalls,
            List<char> binLabels
        )
        {
            if(numBalls == 0) { 
                var result = new List<string> { "" };
                return result;
            }
            if (binLabels.Count == 1) {
                var result = new List<string>
                {
                    new string(Enumerable.Range(0, numBalls)
                            .Select(x => binLabels.Single()).ToArray())
                };
                return result;
            }
            var placements = new List<string>();
            for(int i=0; i<=numBalls; i++)
            {
                var newBinLabels = binLabels.ToList();
                var prefix = new string(Enumerable.Range(0, i)
                    .Select(i => binLabels[0]).ToArray());
                newBinLabels.RemoveAt(0);
                var newNumBalls = numBalls - i;
                var newBinLabelsString = new string(newBinLabels.ToArray());
                if (!this._cache.ContainsKey((newBinLabelsString, newNumBalls))) {
                    this._cache[(newBinLabelsString, newNumBalls)] =
                        this.DetermineWaysToPlaceBallsInBins(
                            newNumBalls, newBinLabels);
                }
                var result = this._cache[(newBinLabelsString, newNumBalls)];
                placements.AddRange(
                    result.Select(r => prefix + r)    
                );
            }
            return placements;
        }
    }
}
