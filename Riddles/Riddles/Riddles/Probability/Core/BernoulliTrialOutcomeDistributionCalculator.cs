using Riddles.Combinatorics.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Riddles.Probability.Core
{
    public class BernoulliTrialOutcomeDistributionCalculator
    {
        private bool _useCache;
        private Dictionary<(int, int, double), double> _cache;
        private BinomialTheoremCalculator _binomialTheoremCalculator;
        public BernoulliTrialOutcomeDistributionCalculator(
            bool useCache=true
        )
        {
            this._useCache = useCache;
            this._cache = new Dictionary<(int, int, double), double>();
            this._binomialTheoremCalculator = new BinomialTheoremCalculator();
        }

        public double CalculateOddsOfAnOutcomeOfAtLeastKSuccesses(
            int numTrials,
            int numSuccesses,
            double probabilityOfSuccess)
        {
            if(!this._useCache || !this._cache.ContainsKey(
                (numTrials, numSuccesses, probabilityOfSuccess)))
            {
                // input sanitzation
                if(numTrials < 0 || numSuccesses < 0 
                    || numSuccesses > numTrials)
                {
                    return 0.0;
                }
                // for efficiency - if the number of successes requires
                // iterating over more than half of the outcomes,
                // reverse the search. ask for the probability of failures
                // and reverse the outcome.
                var shouldCheckForFailures
                    = numSuccesses < ((double)numTrials / 2);
                if (shouldCheckForFailures)
                {
                    numSuccesses = numTrials - numSuccesses + 1;
                    probabilityOfSuccess = 1 - probabilityOfSuccess;
                }
                // find the probability of exactly k successes
                var probabilityOfExactNumberOfSuccesses 
                    = this._binomialTheoremCalculator.CalculateBinomialCoefficient(
                            numTrials,
                            numSuccesses
                        ) * Math.Pow(probabilityOfSuccess, numSuccesses)
                        * Math.Pow((1 - probabilityOfSuccess), numTrials - numSuccesses);
                // find the probability of more than k successes
                var probabilityOfMoreSuccesses =
                    this.CalculateOddsOfAnOutcomeOfAtLeastKSuccesses(
                        numTrials,
                        numSuccesses + 1,
                        probabilityOfSuccess
                    );
                var cumulativeProbability = 
                    probabilityOfExactNumberOfSuccesses + probabilityOfMoreSuccesses;
                if (shouldCheckForFailures)
                {
                    cumulativeProbability = 1 - cumulativeProbability;
                }
                if (!this._useCache)
                {
                    return cumulativeProbability;
                }
                this._cache[(numTrials, numSuccesses, probabilityOfSuccess)]
                    = cumulativeProbability;
            }
            return this._cache[(numTrials, numSuccesses, probabilityOfSuccess)];        
        }
    }
}
