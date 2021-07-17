using System;
using System.Collections.Generic;
using System.Text;

namespace Interviews
{
	public class CoinChangeSolver
	{
		public int FindMinimumNumberOfCoins(int amount, int[] coins)
		{			
			var memo = new int?[amount];
			var result = this.FindMinimumNumberOfCoinsInternal(amount, coins, memo);
			return result.HasValue && result != int.MaxValue ? result.Value : -1;
		}

		private int? FindMinimumNumberOfCoinsInternal(int amount, int[] coins, int?[] memo)
		{
			if (amount == 0) { return 0; }
			if (amount < 0) { return int.MaxValue; }
			if (memo[amount-1] != default(int?))
			{
				return memo[amount-1];
			}

			var minNumCoins = int.MaxValue;
			foreach (var coin in coins) {
				var numCoinsAfterThisOne = this.FindMinimumNumberOfCoinsInternal(amount - coin, coins, memo);
				if(numCoinsAfterThisOne == int.MaxValue) { continue; }
				var numCoins = 1 + numCoinsAfterThisOne;
				if (numCoins.HasValue && numCoins < minNumCoins) { 
					minNumCoins = numCoins.Value; 
				}
			}
			memo[amount-1] = minNumCoins;
			return minNumCoins;
		}
	}
}
