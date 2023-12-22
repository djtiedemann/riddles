using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Riddles.Optimization
{
    public class CollatzSequenceMinNumberFinder
    {
        private Dictionary<(int, int), bool> _cache;
        public CollatzSequenceMinNumberFinder()
        {
            this._cache = new Dictionary<(int, int), bool>();
        }

        public int? FindMinNumberWithValueInCollatzSequence(int searchingValue)
        {
            if (searchingValue <= 0)
            {
                return null;
            }
            if(searchingValue == 1)
            {
                return searchingValue;
            }
            for(int i=1; i<searchingValue; i++)
            {
                if(this.CollatzSequenceContainsNumber(i, searchingValue))
                {
                    return i;
                }
            }
            return searchingValue;
        }

        private bool CollatzSequenceContainsNumber(int currentValue, int searchingValue)
        {
            if (!this._cache.ContainsKey((currentValue, searchingValue)))
            {
                if (currentValue == searchingValue)
                {
                    this._cache[(currentValue, searchingValue)] = true;
                }
                else if(currentValue == 1)
                {
                    this._cache[(currentValue, searchingValue)] = false;
                }
                else if(currentValue % 2 == 0)
                {
                    this._cache[(currentValue, searchingValue)] = 
                        this.CollatzSequenceContainsNumber(currentValue/2, searchingValue);
                }
                else
                {
                    this._cache[(currentValue, searchingValue)] =
                        this.CollatzSequenceContainsNumber(currentValue*3 + 1, searchingValue);
                }
            }
            return this._cache[(currentValue, searchingValue)];
        }
    }
}
