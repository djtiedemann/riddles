using Riddles.Combinatorics.Core.SetGeneration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Riddles.Combinatorics.Core.Sets
{
    public class PowerSetCalculator
    {
        private PermutationWithRepetitionGenerator _permutationWithRepititionGenerator;
        public PowerSetCalculator() { 
            this._permutationWithRepititionGenerator
                = new PermutationWithRepetitionGenerator();
        }

        public (List<T>, int[]) CalculateNextSubset<T>(T[] set, int[] lastSubset)
        {
            if(set.Length == 0)
            {
                if (lastSubset == null)
                {
                    return (new List<T> { }, new int[] { });
                }
                else
                {
                    return (null, null);
                }
            }
            var nextSubset = lastSubset != null ?
                this._permutationWithRepititionGenerator
                    .GenerateNextOutcome(
                        lastSubset, 0, 1
                    ) :
                    set.Select(s => 0).ToArray();
            var result = nextSubset != null ?
                nextSubset
                    .Select((v, i) => (v, i))
                    .Where(x => x.v == 1)
                    .Select(x => set[x.i])
                    .ToList()
                : null;
            return (result, nextSubset);
        }
    }
}
