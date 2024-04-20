using System;
using System.Collections.Generic;
using System.Text;

namespace Riddles.Probability.Application
{
    public class CollaborativeCardFlippingGameSolver
    {
        private Dictionary<(int, int), double> _cache;
        public CollaborativeCardFlippingGameSolver() { 
            this._cache = new Dictionary<(int, int), double>();
        }

        public double CalculateProbabilityOfWinning(int numCardsInEachDeck)
        {
            return this.CalculateProbabilityOfWinning(
                numCardsInEachDeck,
                0
            );
        }

        private double CalculateProbabilityOfWinning(
            int numUniqueCardsInEachDeck, 
            int numSharedCardsInEachDeck)
        {
            if(numUniqueCardsInEachDeck < 0 || numSharedCardsInEachDeck < 0)
            {
                return 0;
            }
            if (numUniqueCardsInEachDeck == 0)
            {
                return 1.0;
            }
            if (numSharedCardsInEachDeck == 0 && numUniqueCardsInEachDeck == 1)
            {
                return 0.0;
            }
            if (!this._cache.ContainsKey(
                (numUniqueCardsInEachDeck, numSharedCardsInEachDeck)
            ))
            {
                var numCardsInEachDeck = numUniqueCardsInEachDeck +
                    numSharedCardsInEachDeck;
                // you could each draw a shared card
                var oddsOfBothDrawingSharedCard = 
                    (double)numSharedCardsInEachDeck / numCardsInEachDeck
                    * numSharedCardsInEachDeck / numCardsInEachDeck;
                var pWinBothShared = this.CalculateProbabilityOfWinning(
                    numUniqueCardsInEachDeck,
                    numSharedCardsInEachDeck - 1
                );

                // one of you could draw a unique card and the other could
                // draw a shared card
                var oddsOfDrawingOneUniqueAndOneSharedCard =
                    ((double)numSharedCardsInEachDeck / numCardsInEachDeck
                    * numUniqueCardsInEachDeck / numCardsInEachDeck) * 2;
                var pWinOneUniqueOneShared = this.CalculateProbabilityOfWinning(
                    numUniqueCardsInEachDeck - 1,
                    numSharedCardsInEachDeck
                );
                // you could each draw a non-matching unique card
                var oddsOfBothDrawingUniqueNonMatchingCard =
                    (double)numUniqueCardsInEachDeck / numCardsInEachDeck 
                    * (numUniqueCardsInEachDeck - 1) / numCardsInEachDeck;
                var pWinBothUnique = this.CalculateProbabilityOfWinning(
                    numUniqueCardsInEachDeck - 2,
                    numSharedCardsInEachDeck + 1
                );

                // you could each draw the same card and lose
                var oddsOfImmediatelyLosing = 
                    (double)numUniqueCardsInEachDeck /(numCardsInEachDeck)
                    * 1 / numCardsInEachDeck;

                this._cache[(numUniqueCardsInEachDeck, numSharedCardsInEachDeck)]
                    = oddsOfBothDrawingSharedCard * pWinBothShared
                    + oddsOfDrawingOneUniqueAndOneSharedCard * pWinOneUniqueOneShared
                    + oddsOfBothDrawingUniqueNonMatchingCard * pWinBothUnique;
            }
            return this._cache[(numUniqueCardsInEachDeck, numSharedCardsInEachDeck)]; 
        }
    }
}
