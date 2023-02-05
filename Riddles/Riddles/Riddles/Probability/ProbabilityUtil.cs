using Riddles.Combinatorics.Core;
using Riddles.Combinatorics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Riddles.Tests.Probability
{
    public class ProbabilityUtil
    {
        private BinomialTheoremCalculator _binomialTheoremCalculator;
        private CombinatoricsUtil _combinatoricsUtil;

        public ProbabilityUtil() { 
            this._binomialTheoremCalculator = new BinomialTheoremCalculator();
            this._combinatoricsUtil = new CombinatoricsUtil();
        }
        public double CalculateOddsOfDrawingIndistinctObjectsWithoutReplacement(IEnumerable<SetInfo> sets)
        {
            var numerator = this._combinatoricsUtil.CalculateNumWaysToSelectIndistinctItemsFromSet(sets);
            var totalNumElements = sets.Sum(s => s.NumElements);
            var totalNumDesired = sets.Sum(s => s.NumDesired);
            var denominator = this._binomialTheoremCalculator.CalculateBinomialCoefficient(totalNumElements, totalNumDesired);
            return numerator / denominator;
        }
    }
}
