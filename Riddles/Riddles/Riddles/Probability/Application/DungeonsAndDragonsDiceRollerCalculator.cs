using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Riddles.Probability.Application
{
    public class DungeonsAndDragonsDiceRollerCalculator
    {
        public (Dictionary<int, double>, double) CalculateOddsOfEachCombination(
            int numPeople,
            int[] diceSizes)
        {
            var probabilitiesOfEachRoll = new Dictionary<int, double>();
            foreach (var dice in diceSizes) { 
                for(int i=1; i<=dice; i++)
                {
                    if (!probabilitiesOfEachRoll.ContainsKey(i))
                    {
                        probabilitiesOfEachRoll[i] = 0;
                    }
                    probabilitiesOfEachRoll[i] += ((double)1 / (dice * diceSizes.Length));
                }
            }
            var cumulativeProbability = Enumerable.Range(1, numPeople)
                .ToDictionary(k => k, k => 0.0);
            var currentCounts = probabilitiesOfEachRoll.Keys
                .ToDictionary(k => k, k => 0);
            this.CalculateProbabilitiesOfEachOutcome(
                probabilitiesOfEachRoll,
                numPeople,
                currentCounts,
                1.0,
                cumulativeProbability
            );
            var expectedValue = cumulativeProbability.Sum(
                x => x.Key * x.Value);
            return (cumulativeProbability, expectedValue);
        }

        private void CalculateProbabilitiesOfEachOutcome(
            Dictionary<int, double> distribution, 
            int numPeople,
            Dictionary<int, int> currentCounts, 
            double probabilityOfCurrentOutcome,
            Dictionary<int, double> cumulativeProbability)
        {
            if(numPeople == 0)
            {
                cumulativeProbability[currentCounts.Values.Where(v => v > 0).Count()] +=
                    probabilityOfCurrentOutcome;
                return;
            }
            foreach(var outcome in distribution)
            {
                currentCounts[outcome.Key] += 1;
                this.CalculateProbabilitiesOfEachOutcome(
                    distribution,
                    numPeople - 1,
                    currentCounts,
                    probabilityOfCurrentOutcome * outcome.Value,
                    cumulativeProbability
                );
                currentCounts[outcome.Key] -= 1;
            }
        } 
    }
}
