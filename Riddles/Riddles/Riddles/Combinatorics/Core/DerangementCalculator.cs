using System;
using System.Collections.Generic;
using System.Text;

namespace Riddles.Combinatorics.Core
{
    /// <summary>
    /// Calculates the number of arrangements of a sequence where
    /// no item is at its correct position
    /// </summary>
    public class DerangementCalculator
    {
        private Dictionary<int, double> _cache;
        public DerangementCalculator() { 
            this._cache = new Dictionary<int, double>();
        }

        public double CalculateNumDerangements(int n)
        {
            if (!this._cache.ContainsKey(n))
            {
                if (n <= 1)
                {
                    this._cache[n] = 0;
                }
                else if (n == 2)
                {
                    this._cache[n] = 1;
                }
                else
                {
                    // you can either find a derangement of the first n-1 cards 
                    // and swap the nth card with any of them
                    // 
                    // or you can find a derangement of the first n-1 cards with
                    // one match, and swap the nth card with it.
                    //
                    // each case multiplies by n-1 and uses the recursive call
                    this._cache[n] = (n-1) * 
                        (this.CalculateNumDerangements(n - 1) 
                        + this.CalculateNumDerangements(n-2));
                }
            }
            return this._cache[n];
        }
    }
}
