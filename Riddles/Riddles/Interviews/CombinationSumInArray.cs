using System;
using System.Collections.Generic;
using System.Text;

namespace Interviews
{
	public class CombinationSumInArray
	{
		public int FindNumberOfCombinationsSummingToValue(int[] searchArray, int target) {
			var memo = new int?[target];
			return this.FindNumberOfCombinationsSummingToValueInternal(searchArray, target, memo);
		}

		private int FindNumberOfCombinationsSummingToValueInternal(int[] searchArray, int target, int?[] memo) {
			if (target == 0) {
				return 0;
			}
			if(memo[target-1] != default(int?))
			{
				return memo[target - 1].Value;
			}
			var numCombinations = 0;
			for(int i=0; i<searchArray.Length; i++)
			{
				if (target > searchArray[i]) {
					numCombinations += this.FindNumberOfCombinationsSummingToValueInternal(searchArray, target - searchArray[i], memo);
				}
				if(target == searchArray[i])
				{
					numCombinations++;
				}
			}
			memo[target-1] = numCombinations;
			return memo[target - 1].Value;
		}
	}
}
