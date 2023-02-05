using Riddles.Combinatorics.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Riddles.Combinatorics
{
    public class BaseSetInfo : SetInfo
    {
        public BaseSetInfo(int numElements, int numDesired, string label = null) { 
            this.NumElements = numElements;
            this.NumDesired = numDesired;
            this.Label = label;
        }

        public int NumElements { get; }
        public int NumDesired { get; } 
        public string Label { get; }
    }
    public interface SetInfo
    {
        int NumElements { get; }
        int NumDesired { get; }
    }
    public class CombinatoricsUtil
    {
        private BinomialTheoremCalculator _binomialTheoremCalculator;
        public CombinatoricsUtil()
        {
            this._binomialTheoremCalculator = new BinomialTheoremCalculator();
        }

        // Say you have a set of 3 red balls and 5 yellow balls. How many ways are there
        // to select 2 red balls and 1 yellow ball? Primarily used in probability
        public double CalculateNumWaysToSelectIndistinctItemsFromSet(IEnumerable<SetInfo> sets)
        {
            return sets.Select(s =>
            {
                if (s.NumDesired > s.NumElements || s.NumElements < 0 || s.NumDesired < 0)
                {
                    return 0;
                }
                else
                {
                    var result = this._binomialTheoremCalculator.CalculateBinomialCoefficient(
                        s.NumElements,
                        s.NumDesired
                    );
                    return result;
                }
            }).Aggregate((double)1, (agg, v) => agg * v);
        }
    }
}
