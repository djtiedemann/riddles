using Riddles.Combinatorics.Core.Permutations;
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
        private PermutationWithoutRepetitionGenerator _permutationGenerator;
        public RingStackConfigCounter() {
            this._permutationGenerator = new PermutationWithoutRepetitionGenerator();
        }

        public int CountNumberOfRingConfigurations(int numRings)
        {
            var rings = Enumerable.Range(0, numRings).ToArray();
            var numConfigurations = 0;
            for(int permutationSize=1; permutationSize<=numRings; permutationSize++)
            {
                var permutationState = this._permutationGenerator
                    .GenerateNextPermutation(
                        new PermutationWithoutRepetitionGenerator.PermutationState(
                            null,
                            null,
                            rings.ToArray(),
                            permutationSize
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
            return numConfigurations;
        }
    }
}
