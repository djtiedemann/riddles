using Riddles.Combinatorics.Core.Permutations;
using Riddles.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Riddles.Probability
{
    public class MultipleVariablesUniformDistributionProbabilityCalculator
    {
        private MutuallyExclusiveCollectivelyExhaustiveRangeCreator _meceRangeCreator;
        private PermutationWithRepetitionGenerator _outcomeGenerator;
        public MultipleVariablesUniformDistributionProbabilityCalculator()
        {
            this._meceRangeCreator 
                = new MutuallyExclusiveCollectivelyExhaustiveRangeCreator();
            this._outcomeGenerator = new PermutationWithRepetitionGenerator();
        }
        public double[] CalculateProbabilityTeamHasHighestValue(
            List<List<(double, double)>> distributions
        )
        {
            if(distributions == null || distributions.Count == 0)
            {
                return null;
            }
            var numDistributions = distributions.Count;
            var meceRanges = this._meceRangeCreator.CreateMeceRanges(distributions);
            var numRanges = meceRanges.First().Count;
            // calculate size of each distribution
            // used to determine the probability a certain distribution has a
            // value within the range
            var rangeSizes =
                meceRanges.Select(
                    r => r.Aggregate(
                        0.0,
                        (agg, v) => agg + (
                            v.Item3 
                                ? (v.Item2 - v.Item1)
                                : 0.0
                        )
                    )
                ).ToList();

            // go through each possible combination of ranges
            // calculate the probability that this range is selected
            // calculate the probability of each team winning based on that probability
            var currentOutcome = distributions.Select(d => 0).ToArray();
            var probabilities = distributions.Select(d => 0.0).ToArray();
            while(currentOutcome != null)
            {
                // calculate the probability this combination occurs
                var probability = 1.0;
                for(int i=0; i<numDistributions; i++)
                {
                    var range = meceRanges[i][currentOutcome[i]];
                    probability *= 
                        range.Item3 
                            ? (range.Item2 - range.Item1) / rangeSizes[i] : 0.0;
                }

                // calculate the team that wins in this combination
                if(probability > 0)
                {
                    var maxRange = currentOutcome.Max(x => x);
                    var winningTeams = currentOutcome.Select(
                        (rangeNum, index) => (rangeNum, index)
                    ).Where(x => x.rangeNum == maxRange)
                        .Select(x => x.index)
                        .ToList();

                    var numWinningTeams = winningTeams.Count;
                    foreach(var winningTeam in winningTeams) {
                        probabilities[winningTeam] += (probability / numWinningTeams);
                    }
                }

                // advance to the next outcome
                currentOutcome = this._outcomeGenerator.GenerateNextOutcome(
                    currentOutcome,
                    0,
                    numRanges - 1
                );
            }
            return probabilities;
        }
    }
}
