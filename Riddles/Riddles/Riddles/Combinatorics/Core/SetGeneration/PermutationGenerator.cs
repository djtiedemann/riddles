using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Riddles.Combinatorics.Core.Domain;
using Riddles.Combinatorics.Core;

namespace Riddles.Combinatorics.Core.SetGeneration
{
	public class PermutationGenerator
	{
		private FactorialCalculator _factorialCalculator;
		public PermutationGenerator()
		{
			this._factorialCalculator = new FactorialCalculator();
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

		public Permutation GeneratePermutationFromKey(int key, int n)
		{
			var permutation = Enumerable.Range(1, n).ToArray();
			for(int i=n-1; i>0; i--) {
				var iFactorial = this._factorialCalculator.Factorial(i);
				var indexToSwap = key / iFactorial;
				var temp = permutation[indexToSwap];
				permutation[indexToSwap] = permutation[i];
				permutation[i] = temp;
				key = key % iFactorial;
			}
			return new Permutation(permutation);
		}
	}
}
