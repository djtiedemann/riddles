using Riddles.Combinatorics.Core.SetGeneration;
using Riddles.Combinatorics.Core.Sets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Riddles.Combinatorics.Application
{
    // https://fivethirtyeight.com/features/can-you-make-24/
    public class RingStackConfigCounter
    {
        private PowerSetCalculator _powerSetCalculator;
        private PermutationWithoutRepetitionGenerator _permutationGenerator;
        public RingStackConfigCounter() {
            this._powerSetCalculator = new PowerSetCalculator();
            this._permutationGenerator = new PermutationWithoutRepetitionGenerator();
        }

        public int CountNumberOfRingConfigurations(int numRings)
        {
            var rings = Enumerable.Range(0, numRings).ToArray();
            var (powerSet, indicatorArray) = this._powerSetCalculator.CalculateNextSubset(
                rings,
                null
            );
            var numConfigurations = 0;
            while(powerSet != null)
            {
                if(powerSet.Count() != 0)
                {
                    var permutationState = this._permutationGenerator
                        .GenerateNextPermutation(
                            new PermutationWithoutRepetitionGenerator.PermutationState(
                                null,
                                null,
                                powerSet.ToArray(),
                                powerSet.Count()
                            ));
                    while(permutationState.CurrentPermutation != null)
                    {
                        var result = permutationState.CurrentPermutation.GetPermutation();
                        var isValid = result
                            .Select((x, i) => (x, i))
                            .All(x => x.x >= x.i);
                        if (isValid)
                        {
                            numConfigurations++;
                        }
                        permutationState = this._permutationGenerator
                            .GenerateNextPermutation(permutationState);
                    }
                }
                (powerSet, indicatorArray) = this._powerSetCalculator
                    .CalculateNextSubset(rings, indicatorArray);
            }
            return numConfigurations;
        }
    }
}
