using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Riddles.Combinatorics.Application;

namespace Riddles.Probability.Application
{
    public class TenziCalculator
    {
        private MostCommonDieRolledDistributionCalculator _distributionCalculator;
        public TenziCalculator() { 
            this._distributionCalculator = new MostCommonDieRolledDistributionCalculator();
        }

        public double CalculateExpectedNumDiceSetAside(int numDice, int numSides)
        {
            var distribution = this._distributionCalculator
                .CalculateDistributionOfMostCommonValuesRolled(
                    numDice: numDice,
                    numSides: numSides
                );
            var totalOutcomes = distribution.Sum(v => v.Value);
            var expectedValue =
                distribution.Select(x => x.Key * x.Value / totalOutcomes)
                .Sum();
            return expectedValue;
        }
    }
}
