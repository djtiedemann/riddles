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
        public RecursiveDigitSummer() { 
            this._cache = new Dictionary<string, int>();
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
