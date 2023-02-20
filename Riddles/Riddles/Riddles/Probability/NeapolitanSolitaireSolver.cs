using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Riddles.Caching;
using static Riddles.MarkovChains.BottlesOfBeerSolver;

namespace Riddles.Probability
{
    public class NeapolitanSolitaireSolver
    {

        public NeapolitanSolitaireSolver() { 
            
        }
        public double CalculateOddsOfSuccess(
            Dictionary<string, int> cardCount, 
            Dictionary<string, HashSet<int>> restrictions)
        {
            var numCardsRemaining = cardCount.Keys.Sum(x => cardCount[x]);
            var solitaireState = new SolitaireState(cardCount, numCardsRemaining);
            var cache = new Dictionary<SolitaireState, double>();
            return this.CalculateOddsOfSuccess(solitaireState, restrictions, cache, 0);
        }

        private double CalculateOddsOfSuccess(
            SolitaireState state,
            Dictionary<string, HashSet<int>> restrictions,
            Dictionary<SolitaireState, double> cache,
            int numCardsDrawn)
        {
            if (!cache.ContainsKey(state))
            {
                if(state.NumCardsRemaining == 0)
                {
                    cache[state] = 1;
                }
                else
                {
                    var cumulativeProbability = 0.0;
                    foreach(var key in state.RemainingCardsOfType.Dictionary.Keys)
                    {
                        if (restrictions.ContainsKey(key) && 
                            restrictions[key].Contains(numCardsDrawn))
                        {
                            continue;
                        }
                        
                        var probabilityOfCardDrawn 
                            = (double)state.RemainingCardsOfType.Dictionary[key]
                            / (double)state.NumCardsRemaining;
                        
                        var remainingCardsOfTypeClone =
                            state.RemainingCardsOfType.Clone();
                        remainingCardsOfTypeClone.Dictionary[key]--;
                        if (remainingCardsOfTypeClone.Dictionary[key] == 0)
                        {
                            remainingCardsOfTypeClone.Dictionary.Remove(key);
                        }
                        cumulativeProbability += probabilityOfCardDrawn 
                            * this.CalculateOddsOfSuccess(
                                new SolitaireState(
                                    remainingCardsOfTypeClone.Dictionary,
                                    state.NumCardsRemaining - 1
                                ),
                                restrictions,
                                cache,
                                numCardsDrawn+1
                            );
                    }
                    cache[state] = cumulativeProbability;
                }
            }
            return cache[state];
        }
    }

    public class SolitaireState
    {
        public CacheableDictionary<string, int> RemainingCardsOfType { get; }
        public int NumCardsRemaining { get; }
        public SolitaireState(Dictionary<string, int> remainingCardsOfType, int numCardsRemaining)
        {
            this.RemainingCardsOfType = new CacheableDictionary<string, int>(remainingCardsOfType);
            this.NumCardsRemaining = numCardsRemaining;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is SolitaireState))
            {
                return false;
            }
            // note, don't need to consider numCardsRemaining because it can be derived from
            // RemainingCardsOfType
            return ((SolitaireState)obj).RemainingCardsOfType.Equals(this.RemainingCardsOfType);
        }

        public override int GetHashCode()
        {
            return this.RemainingCardsOfType.GetHashCode();
        }
    }
}
