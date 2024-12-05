using Riddles.Combinatorics.Core;
using Riddles.Combinatorics.Core.Permutations;
using Riddles.Combinatorics.Core.Permutations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Riddles.Probability.Core
{
    public class IndependentTrialOutcomeDistributionCalculator
    {
        private PermutationWithRepetitionGenerator _permutationGenerator;
        private IndistinctBallsInDistinctBinsCalculator
            _indistinctBallsInDistinctBinsCalculator;
        private NumPermutationsCalculator _numPermutationsCalculator;
        public IndependentTrialOutcomeDistributionCalculator() { 
            this._permutationGenerator = new PermutationWithRepetitionGenerator();
            this._indistinctBallsInDistinctBinsCalculator 
                = new IndistinctBallsInDistinctBinsCalculator();
            this._numPermutationsCalculator = new NumPermutationsCalculator();
        }

        public List<(string, double)> CalculateOrderDependentOutcomeProbabilities(
            int numTrials,
            Dictionary<char, double> outcomeProbabilities
        )
        {
            var outcomes = this._permutationGenerator.GenerateAllOutcomes(
                numTrials,
                outcomeProbabilities.Keys.OrderBy(x => x).ToList(),
                isOrdered: true
            );
            return outcomes.Select(
                o => (o, o.ToCharArray().Aggregate(1.0, (agg, v) => agg * outcomeProbabilities[v]))
            ).ToList();
        }

        public List<(string, double)> CalculateOrderIndependentOutcomeProbabilities(
            int numTrials,
            Dictionary<char, double> outcomeProbabilities
        ) {
            var outcomes = 
                this._indistinctBallsInDistinctBinsCalculator
                    .DetermineWaysToPlaceBallsInBins(
                        numTrials,
                        outcomeProbabilities.Keys.OrderBy(c => c).ToList()
                    );
            return outcomes.Select(
                c => (c,
                    c.ToCharArray().Aggregate(1.0, (agg, v) => agg * outcomeProbabilities[v]) *
                    this._numPermutationsCalculator.CalculateNumPermutations(c)
                )
            ).ToList();
        }
    }
}
