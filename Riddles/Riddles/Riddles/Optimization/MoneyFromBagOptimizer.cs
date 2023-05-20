using System;
using System.Collections.Generic;
using System.Text;

namespace Riddles.Optimization
{
    public class MoneyFromBagOptimizer
    {
        private Dictionary<int, double> _cache;

        public MoneyFromBagOptimizer() {
            this._cache = new Dictionary<int, double>();
        }
        public double GetSimulatedNumberOfValuesInFinalBag(int initialValuesInBag, int numDraws)
        {
            var random = new Random();
            int numValuesInBag = initialValuesInBag;
            for(int i=0; i<numDraws; i++)
            {
                var draw = random.Next(1, numValuesInBag+1);
                numValuesInBag += draw;
            }
            return (double)numValuesInBag;
        }

        public double GetMaximumMoneyFromHat(int numRounds)
        {
            // populate cache
            var bestNumAdds = 0;
            var bestResult = double.MinValue;
            for(int numAdds = 0; numAdds<=numRounds; numAdds++)
            {
                var result = (numRounds - numAdds)
                    * (this.GetExpectedItemsInBagAfterNAdds(numAdds) + 1)
                    / 2.0;
                if(result >= bestResult)
                {
                    bestResult = result;
                    bestNumAdds = numAdds;
                }
            }
            return bestResult;
        }

        public double GetExpectedItemsInBagAfterNAdds(int numAdds)
        {
            if(numAdds <= 0) { return 1; }
            if (!this._cache.ContainsKey(numAdds))
            {
                var numItemsInPreviousIteration = this
                    .GetExpectedItemsInBagAfterNAdds(numAdds - 1);
                this._cache[numAdds] = numItemsInPreviousIteration + 
                    (numItemsInPreviousIteration + 1)/2.0;
            }
            return this._cache[numAdds];
        }
    }
}
