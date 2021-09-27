using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Riddles.Combinatorics.Core.Domain;

namespace Riddles.Combinatorics.Core.SetGeneration
{
	public class PermutationGenerator
	{
		public PermutationGenerator()
		{

		}

		public List<Permutation> GeneratePermutationsOfNObjects(int n)
		{
			var initialPermutation = Enumerable.Range(1, n).ToArray();
			var nextPermutation = new Permutation(initialPermutation);
			var permutations = new List<Permutation> { nextPermutation };
			bool hasNextPermutation = true;
			while (hasNextPermutation)
			{
				nextPermutation = nextPermutation.GenerateNextPermutation();
				hasNextPermutation = nextPermutation != null;
				if (hasNextPermutation)
				{
					permutations.Add(nextPermutation);
				}				
			}
			return permutations;
		}
	}
}
