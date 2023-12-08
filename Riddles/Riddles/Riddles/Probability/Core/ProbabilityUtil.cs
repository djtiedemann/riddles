using Riddles.Combinatorics.Core;
using Riddles.Combinatorics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Riddles.Probability.Core
{
    public class ProbabilityUtil
    {
        private BinomialTheoremCalculator _binomialTheoremCalculator;
        private CombinatoricsUtil _combinatoricsUtil;

        public ProbabilityUtil()
        {
            _binomialTheoremCalculator = new BinomialTheoremCalculator();
            _combinatoricsUtil = new CombinatoricsUtil();
        }
        public double CalculateOddsOfDrawingIndistinctObjectsWithoutReplacement(IEnumerable<SetInfo> sets)
        {
            var numerator = _combinatoricsUtil.CalculateNumWaysToSelectIndistinctItemsFromSet(sets);
            var totalNumElements = sets.Sum(s => s.NumElements);
            var totalNumDesired = sets.Sum(s => s.NumDesired);
            var denominator = _binomialTheoremCalculator.CalculateBinomialCoefficient(totalNumElements, totalNumDesired);
            return numerator / denominator;
        }
    }
}
