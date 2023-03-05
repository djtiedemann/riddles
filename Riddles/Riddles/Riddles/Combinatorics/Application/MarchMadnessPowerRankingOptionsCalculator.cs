using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Riddles.Combinatorics.Application
{
    // https://fivethirtyeight.com/features/how-many-brackets-can-you-bust/ 
    // https://docs.google.com/document/d/1pnmNARUuVb0AGc95-xyJDHboSB_MsS41U9Sl7w0uFvk/edit#
    // Note: this problem has a recursive structure, described in document
    public class MarchMadnessPowerRankingOptionsCalculator
    {
        private ListInterleaver _listInterleaver;
        private Dictionary<int, double> _cache;
        public MarchMadnessPowerRankingOptionsCalculator() { 
            this._listInterleaver = new ListInterleaver();
            this._cache = new Dictionary<int, double>(); 
        }

        public double CalculateNumPossiblePowerRankings(int numRounds)
        {
            if (numRounds < 0)
            {
                return 0;
            }
            if(!this._cache.ContainsKey(numRounds)) { 
                if(numRounds <= 1)
                {
                    this._cache[numRounds] = 1;
                }
                else
                {
                    var numWaysToInterleave =
                    this._listInterleaver.CountNumberOfWaysToInterleaveLists(
                        Enumerable.Range(0, numRounds)
                        .Select(i => (int)Math.Pow(2, i))
                        .ToList()
                    );
                    var numWaysToOrder = Enumerable.Range(0, numRounds)
                        .Select(i => this.CalculateNumPossiblePowerRankings(i))
                        .Aggregate(1.0, (agg, v) => agg * v);
                    this._cache[numRounds] = numWaysToOrder * numWaysToInterleave;
                }
                
            }
            return this._cache[numRounds];
        }
    }
}
