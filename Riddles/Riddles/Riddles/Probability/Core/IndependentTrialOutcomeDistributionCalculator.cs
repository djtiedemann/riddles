using Riddles.Combinatorics.Core.SetGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Riddles.Probability.Core
{
    public class IndependentTrialOutcomeDistributionCalculator
    {
        private PermutationWithRepetitionGenerator _permutationGenerator;
        public IndependentTrialOutcomeDistributionCalculator() { 
            this._permutationGenerator = new PermutationWithRepetitionGenerator();
        }

        public List<(string, double)> CalculateOrderDependentOutcomeProbabilities(
            int numTrials,
            Dictionary<char, double> outcomeProbabilities
        )
        {
            var outcomes = this._permutationGenerator.GenerateAllOutcomes(
                numTrials,
                outcomeProbabilities.Keys.OrderBy(x => x).ToList()
            );
            return outcomes.Select(
                o => (o, o.ToCharArray().Aggregate(1.0, (agg, v) => agg * outcomeProbabilities[v]))
            ).ToList();
        }

        public List<(string, double)> CalculateOrderIndependentOutcomeProbabilities(
            int numTrials,
            Dictionary<char, double> outcomeProbabilities
        ) {
            return this.CalculateOrderDependentOutcomeProbabilities(
                numTrials,
                outcomeProbabilities
            ).GroupBy(g => new string(g.Item1.ToCharArray().OrderBy(v => v).ToArray()))
            .Select(g => (g.Key, g.Sum(x => x.Item2))).ToList();
        }
    }
}
