using System;
using System.Collections.Generic;
using System.Text;
using Riddles.Combinatorics.Core.Permutations;
using System.Linq;

namespace Riddles.Combinatorics.Core.Sets
{
    public class SubsetCalculator
    {
        private PermutationWithoutRepetitionGenerator _permutationGenerator;
        public SubsetCalculator() { 
            this._permutationGenerator = new PermutationWithoutRepetitionGenerator();
        }

        // split the initial members into groups of the following sizes
        // note: can generate a more flexible outcome with more members than groups
        // if necessary
        // could be made to work for more than 10 groups by applying some abstractions
        // and using lists of ints instead of strings in PermutationGenerator
        public List<List<HashSet<int>>> CalculateSubsets(
            List<int> groupSizes)
        {
            var elementNums = Enumerable.Range(0, groupSizes.Sum());
            var initialPermutation = groupSizes.SelectMany((x, i) => 
                Enumerable.Range(0, x).Select(x => i)).ToArray();
            var allPermutations = this._permutationGenerator.GenerateAllPermutations(initialPermutation);
            
            var groupings = allPermutations.Select(p => 
                elementNums.GroupBy(x => p[x]).Select(g => g.ToHashSet()).ToList()
            ).ToList();
            return groupings;
        }
    }
}
