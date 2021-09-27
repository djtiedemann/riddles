using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Riddles.Combinatorics.Core.Domain
{
	public class Permutation
	{
		private int[] _permutation;
		public Permutation(int[] permutation)
		{
			this._permutation = permutation;
		}

		public int[] GetPermutation() { return this._permutation; }

		public Permutation GenerateNextPermutation()
		{
			// if there's only 1 element in the permutation, there isn't a next permutation
			if(this._permutation.Length <= 1)
			{
				return null;
			}
			var newPermutation = new int[this._permutation.Length];
			for(int i=0; i<this._permutation.Length; i++)
			{
				newPermutation[i] = this._permutation[i];
			}

			for(int i = newPermutation.Length - 1; i>0; i--)
			{
				// the first time you find a digit that's less than the next digit, find the smallest digit to the right of the digit being swapped
				// that's larger than the digit being swapped. swap those two digits, then sort the remaining digits to the right of the digit swapped
				// for instance: 12534 becomes 12453, 21543 becomes 23145
				if(newPermutation[i] > newPermutation[i - 1])
				{
					var minDigitToSwap = newPermutation[i];
					var indexToSwap = i;
					for(var j=i+1; j<newPermutation.Length; j++)
					{
						if(newPermutation[j] < minDigitToSwap && newPermutation[j] > newPermutation[i - 1])
						{
							minDigitToSwap = newPermutation[j];
							indexToSwap = j;
						}
					}

					var temp = newPermutation[indexToSwap];
					newPermutation[indexToSwap] = newPermutation[i - 1];
					newPermutation[i - 1] = temp;

					var digitsToSort = new int[newPermutation.Length - i];
					for(var j=i; j<newPermutation.Length; j++)
					{
						digitsToSort[j - i] = newPermutation[j];
					}
					var sortedDigits = digitsToSort.OrderBy(i => i).ToArray();

					for(var j=i; j<newPermutation.Length; j++)
					{
						newPermutation[j] = sortedDigits[j - i];
					}
					return new Permutation(newPermutation);
				}
			}
			return null;
		}

		/// <summary>
		/// Used to solve problems similar to the hat problem where we check if the permutation has at least index where the value is equal to the index.
		/// In this case we use a 1-indexed array
		/// </summary>
		/// <returns></returns>
		public bool ContainsAtLeastOneFixedPoint()
		{
			return this._permutation.Where((p, i) => p == i + 1).Any();
		}
	}
}
