using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Riddles.Probability.Core
{
    public class CumulativeTrialOutcomeDistributionCalculator
    {
        // if you have independent trials each with a probability distribution
        // and the outcomes are aggregated, what is the probability of each
        // outcome
        public Dictionary<T, double> 
            CalculateCumulativeOutcomeDistributionForTrials<T>(
                Func<T, T, T> aggregation,
                List<Dictionary<T, double>> distributions,
                T startingValue,
                double startingProbability
            )
        {
            var result = new Dictionary<T, double>();
            this.CalculateCumulativeOutcomeDistributionForTrials(
                aggregation,
                distributions,
                startingValue,
                startingProbability,
                result
            );
            return result;
        }

        private void CalculateCumulativeOutcomeDistributionForTrials<T>(
                Func<T, T, T> aggregation,
                List<Dictionary<T, double>> distributions,
                T value,
                double probability,
                Dictionary<T, double> result
            )
        {
            if(distributions.Count == 0)
            {
                if (!result.ContainsKey(value))
                {
                    result[value] = 0;
                }
                result[value] += probability;
                return;
            }
            var distribution = distributions[0];
            var distributionsClone = distributions.ToList();
            distributionsClone.RemoveAt(0);
            foreach(var key in distribution.Keys)
            {
                this.CalculateCumulativeOutcomeDistributionForTrials(
                    aggregation,
                    distributionsClone,
                    aggregation(value, key),
                    probability * distribution[key],
                    result
                );
            }
        }

    }
}