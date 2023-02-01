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
