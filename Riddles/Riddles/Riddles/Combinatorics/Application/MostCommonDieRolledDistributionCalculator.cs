using Riddles.Combinatorics.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Riddles.Combinatorics.Application
{
    // Say you have 10 6 sided dice. You roll them all and set aside
    // all dice with the most commonly rolled value
    // 
    // What's the distribution of the number of dice you set aside
    public class MostCommonDieRolledDistributionCalculator
    {

        private FactorialCalculator _factorialCalculator;
        private BinomialTheoremCalculator _binomialTheoremCalculator;

        public MostCommonDieRolledDistributionCalculator()
        {
            this._factorialCalculator = new FactorialCalculator();
            this._binomialTheoremCalculator = new BinomialTheoremCalculator();
        }

        public Dictionary<int, double>
            CalculateDistributionOfMostCommonValuesRolled(
                int numDice, 
                int numSides
            )
        {
            var cumulativeResults = new List<List<int>>();
            var currentValues = new List<int>();
            this.GenerateAllPossibleDistributions(
                numDice,
                numDice,
                numSides,
                currentValues,
                cumulativeResults
            );
            var distributionFrequencyDictionary 
                = new Dictionary<List<int>, double>();
            foreach(var distribution in cumulativeResults)
            {
                distributionFrequencyDictionary[distribution] =
                    this.CalculateNumOccurrencesOfDistribution(
                        numDice: numDice,
                        numSides: numSides,
                        distribution: distribution
                    );
            }
            var resultsDictionary =
                distributionFrequencyDictionary.GroupBy(
                    x => x.Key.Max()
                ).ToDictionary(x => x.Key, x => x.Sum(y => y.Value));

            var numOutcomes = resultsDictionary.Values.Sum();
            return resultsDictionary;
        }

        private double CalculateNumOccurrencesOfDistribution(
            int numDice,
            int numSides,
            List<int> distribution
        )
        {
            double numOrderings = this._factorialCalculator
                .Factorial(numDice);
            foreach(var item in distribution)
            {
                numOrderings = numOrderings / this._factorialCalculator
                    .Factorial(item);
            }

            var groupings = 
                distribution.GroupBy(x => x).Select(x => x.Count());
            var numWaysToSelectDistinctValues = 1.0;
            var numValuesRemainingToBeSelected = numSides;
            foreach (var grouping in groupings)
            {
                numWaysToSelectDistinctValues *=
                    this._binomialTheoremCalculator
                        .CalculateBinomialCoefficient(
                            numValuesRemainingToBeSelected,
                            grouping
                        );
                numValuesRemainingToBeSelected -= grouping;
            }
            
            return numWaysToSelectDistinctValues * numOrderings;
        }

        private void GenerateAllPossibleDistributions(
            int remainingSum,
            int maximumValueAllowed,
            int numValuesRemaining,
            List<int> currentValues,
            List<List<int>> cumulativeResults
        )
        {
            var minimumValueFoNextValue = (int)
                Math.Ceiling(
                    ((double)remainingSum / numValuesRemaining)
                );
            for(int i=minimumValueFoNextValue; i<= maximumValueAllowed; i++)
            {
                var listClone = currentValues.ToList();
                listClone.Add(i);
                if(numValuesRemaining > 1)
                {
                    this.GenerateAllPossibleDistributions(
                        remainingSum - i,
                        Math.Min(i, remainingSum - i),
                        numValuesRemaining - 1,
                        listClone,
                        cumulativeResults
                    );
                }
                else
                {
                    cumulativeResults.Add(listClone);
                }
            }
        }
    }
}
