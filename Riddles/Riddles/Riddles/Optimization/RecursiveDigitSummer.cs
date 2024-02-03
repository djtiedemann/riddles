using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Riddles.Optimization
{
    /// <summary>
    /// https://thefiddler.substack.com/p/how-many-times-can-you-add-up-the
    /// You take a number, sum its digits, take that sum, and repeat until you have 
    /// a one digit number. 
    /// </summary>
    public class RecursiveDigitSummer
    {
        private Dictionary<string, int> _cache;
        private Dictionary<int, string> _smallestNumberSummingToNCache;
        public RecursiveDigitSummer() { 
            this._cache = new Dictionary<string, int>();
            this._smallestNumberSummingToNCache = new Dictionary<int, string>();
        }

        public string FindSmallestNumberSummingToN(int n)
        {
            if(n == 0)
            {
                return 0.ToString();
            }
            if(n == 1)
            {
                return 10.ToString();
            }
            if (!this._smallestNumberSummingToNCache.ContainsKey(n)) {
                var targetValue = int.Parse(this.FindSmallestNumberSummingToN(n - 1));
                var numNines = targetValue / 9;
                var precedingNumber = targetValue % 9;
                var nines = Enumerable.Range(0, numNines).Select(n => "9")
                    .Aggregate("", (agg, v) => $"{agg}{v}");
                this._smallestNumberSummingToNCache[n] =
                    (precedingNumber != 0 ? precedingNumber.ToString() : string.Empty)
                    + nines;
            }
            return this._smallestNumberSummingToNCache[n];
        }

        public int CalculateNumNumbersSummingToN(int maximumNum, int n)
        {
            int numValuesMeetingThreshold = 0;
            for(int i=0; i<maximumNum; i++)
            {
                var result = this.CalculateDepth(i.ToString());
                if(result == n)
                {
                    numValuesMeetingThreshold++;
                }
            }
            return numValuesMeetingThreshold;
        }

        public int CalculateDepth(string n)
        {
            if (!this._cache.ContainsKey(n))
            {
                if(n.Length == 1)
                {
                    this._cache[n] = 0;
                }
                else
                {
                    var result = n.Select(c => int.Parse(c.ToString())).Sum();
                    this._cache[n] = 1 + this.CalculateDepth(result.ToString());
                }
            }
            return this._cache[n];
        }
    }
}
