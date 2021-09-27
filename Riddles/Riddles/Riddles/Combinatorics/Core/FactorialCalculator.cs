using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Riddles.Combinatorics.Core
{
	public class FactorialCalculator
	{
		private Dictionary<int, int> _factorialCache;
		public FactorialCalculator()
		{
			this._factorialCache = new Dictionary<int, int>();
		}

		public int Factorial(int n)
		{
			if (!this._factorialCache.ContainsKey(n))
			{
				this._factorialCache[n] = Enumerable.Range(1, n).Aggregate(1, (agg, num) => agg * num);
			}
			return this._factorialCache[n];
		}
	}
}
