using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Riddles.Combinatorics.Core
{
	public class FactorialCalculator
	{
		private Dictionary<int, double> _factorialCache;
		private Dictionary<(int, int), double> _factorialDivisionCache;
		public FactorialCalculator()
		{
			this._factorialCache = new Dictionary<int, double>();
			this._factorialDivisionCache = new Dictionary<(int, int), double>();
		}

		public double Factorial(int n)
		{
			if (!this._factorialCache.ContainsKey(n))
			{
                var n_positive = n * (n >= 0 ? 1 : -1);
                this._factorialCache[n] = Enumerable.Range(1, n_positive)
					.Aggregate((double)1, (agg, num) => agg * num)
					* (n >= 0 ? 1 : -1);
			}
			return this._factorialCache[n];
		}

		public double FactorialDivision(int n, int k)
		{
			if(this._factorialDivisionCache.ContainsKey((n, k)))
			{
				return this._factorialDivisionCache[(n, k)];
			}
			if(n >= k)
			{
                this._factorialDivisionCache[(n, k)] = Enumerable.Range(k + 1, (n - k)).Aggregate(1.0, (agg, v) => agg * v);
			}
			else
			{
                this._factorialDivisionCache[(n, k)] = this.Factorial(n) / this.Factorial(k);

            }
			return this._factorialDivisionCache[(n, k)];

        }
	}
}
