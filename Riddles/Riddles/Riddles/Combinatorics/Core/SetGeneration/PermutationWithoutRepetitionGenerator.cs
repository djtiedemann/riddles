using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Riddles.Combinatorics.Core.Domain;
using Riddles.Combinatorics.Core;

namespace Riddles.Combinatorics.Core.SetGeneration
{
	public class PermutationWithoutRepetitionGenerator
	{
		private FactorialCalculator _factorialCalculator;
		private Random _random;
		public PermutationWithoutRepetitionGenerator()
		{
			this._factorialCalculator = new FactorialCalculator();
			this._random = new Random();
		}

		public List<Permutation> GenerateAllPermutations(int n)
		{
			var initialPermutation = Enumerable.Range(1, n).ToArray();
			var nextPermutation = new Permutation(initialPermutation);
			var permutations = new List<Permutation> { nextPermutation };
			bool hasNextPermutation = true;
			while (hasNextPermutation)
			{
				nextPermutation = this.GenerateNextPermutation(nextPermutation);
				hasNextPermutation = nextPermutation != null;
				if (hasNextPermutation)
				{
					permutations.Add(nextPermutation);
				}				
			}
			return permutations;
		}

        public Permutation GenerateNextPermutation(Permutation currentPermutation)
        {
            var innerPermutation = currentPermutation.GetPermutation();
            // if there's only 1 element in the permutation, there isn't a next permutation
            if (innerPermutation.Length <= 1)
            {
                return null;
            }
            var newPermutation = new int[innerPermutation.Length];
            for (int i = 0; i < innerPermutation.Length; i++)
            {
                newPermutation[i] = innerPermutation[i];
            }

            for (int i = newPermutation.Length - 1; i > 0; i--)
            {
                // the first time you find a digit that's less than the next digit, find the smallest digit to the right of the digit being swapped
                // that's larger than the digit being swapped. swap those two digits, then sort the remaining digits to the right of the digit swapped
                // for instance: 12534 becomes 12453, 21543 becomes 23145
                if (newPermutation[i] > newPermutation[i - 1])
                {
                    var minDigitToSwap = newPermutation[i];
                    var indexToSwap = i;
                    for (var j = i + 1; j < newPermutation.Length; j++)
                    {
                        if (newPermutation[j] < minDigitToSwap && newPermutation[j] > newPermutation[i - 1])
                        {
                            minDigitToSwap = newPermutation[j];
                            indexToSwap = j;
                        }
                    }

                    var temp = newPermutation[indexToSwap];
                    newPermutation[indexToSwap] = newPermutation[i - 1];
                    newPermutation[i - 1] = temp;

                    var digitsToSort = new int[newPermutation.Length - i];
                    for (var j = i; j < newPermutation.Length; j++)
                    {
                        digitsToSort[j - i] = newPermutation[j];
                    }
                    var sortedDigits = digitsToSort.OrderBy(i => i).ToArray();

                    for (var j = i; j < newPermutation.Length; j++)
                    {
                        newPermutation[j] = sortedDigits[j - i];
                    }
                    return new Permutation(newPermutation);
                }
            }
            return null;
        }

        public Permutation GenerateRandomPermutation(int n)
		{
			var nFactorial = this._factorialCalculator.Factorial(n);
			var randKey = this._random.Next(0, nFactorial);
			return this.GeneratePermutationFromKey(randKey, n);
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
