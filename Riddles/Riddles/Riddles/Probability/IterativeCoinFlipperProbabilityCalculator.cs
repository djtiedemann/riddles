using Riddles.Probability.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Riddles.Probability
{
    // https://thefiddler.substack.com/p/can-you-flip-the-coins-exactly-as
    // Answer: https://thefiddler.substack.com/p/can-you-race-around-the-monopoly
    public class IterativeCoinFlipperProbabilityCalculator
    {
        private IndependentTrialOutcomeDistributionCalculator
            _independentTrialOutcomeDistributionCalculator;
        public IterativeCoinFlipperProbabilityCalculator()
        {
            this._independentTrialOutcomeDistributionCalculator =
                new IndependentTrialOutcomeDistributionCalculator();
        }

        public Dictionary<string, double> CalculateOddsOfGettingExactOutcome(
            Dictionary<char, double> outcomeProbabilities,
            int numCoinFlips,
            int numIterationsOfExperiment
        )
        {
            var distribution = this._independentTrialOutcomeDistributionCalculator
                .CalculateOrderIndependentOutcomeProbabilities(
                    numCoinFlips,
                    outcomeProbabilities
                ).OrderByDescending(x => x)
                .Select((x, i) => ((i.ToString().ToCharArray().First()), x.Item2))
                .ToDictionary(x => x.Item1, x => x.Item2);

            var secondDistribution = this._independentTrialOutcomeDistributionCalculator
                .CalculateOrderIndependentOutcomeProbabilities(
                    numIterationsOfExperiment,
                    distribution
                ).ToDictionary(x => x.Item1, x => x.Item2);

            return secondDistribution;
        }

        public Dictionary<string, double> CalculateOrderedTupleProbabilities(
            Dictionary<char, double> outcomeProbabilities,
            int numCoinFlips,
            int numIterationsOfExperiment
        )
        {
            var odds = this.CalculateOddsOfGettingExactOutcome(
                outcomeProbabilities,
                numCoinFlips,
                numIterationsOfExperiment
            );
            var potentialOutcomes = odds.Keys
                .SelectMany(x => x.ToCharArray()).Distinct().ToList();
            var transformedOutcomes = new Dictionary<string, double>();
            foreach (var outcome in odds.Keys)
            {
                var transformedOutcome = 
                    this.TransformOutcome(outcome, potentialOutcomes);
                if (transformedOutcomes.ContainsKey(transformedOutcome))
                {
                    transformedOutcomes[transformedOutcome] +=
                        odds[outcome];
                } else
                {
                    transformedOutcomes[transformedOutcome] = odds[outcome];
                }
            }
            return transformedOutcomes;
        }

        private string TransformOutcome(string outcome, List<char> potentialOutcomes)
        {
            var outcomeCharArray = outcome.ToCharArray();
            List<int> consolidatedOutcomes = new List<int>();
            foreach(var key in potentialOutcomes)
            {
                consolidatedOutcomes.Add(
                    outcomeCharArray.Where(c => c == key).Count()
                );
            }
            return consolidatedOutcomes.OrderByDescending(c => c)
                .Aggregate("", (agg, x) => $"{agg}{x}");
        }
    }
}
