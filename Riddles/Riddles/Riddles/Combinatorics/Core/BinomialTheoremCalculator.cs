using System;
using System.Collections.Generic;
using System.Text;

namespace Riddles.Combinatorics.Core
{
    public class BinomialTheoremCalculator
    {
        private bool _useCache = true;
        private Dictionary<(int, int), int> _cache;
        private FactorialCalculator _factorialCalculator;

        public BinomialTheoremCalculator(bool useCache = true)
        {
            this._factorialCalculator = new FactorialCalculator();
            this._useCache = useCache;
            this._cache = new Dictionary<(int, int), int>();
        }

        public int CalculateBinomialCoefficient(int n, int k)
        {
            if(n <= 0 || k <0 || n < k)
            {
                return 0;
            }
            if(this._useCache && this._cache.ContainsKey((n, k)))
            {
                return this._cache[(n, k)];
            }
            var result = this._factorialCalculator.Factorial(n)
                / (this._factorialCalculator.Factorial(k) * 
                this._factorialCalculator.Factorial(n - k));
            if (this._useCache)
            {
                this._cache[(n, k)] = result;
            }
            return result;
        }
    }
}
