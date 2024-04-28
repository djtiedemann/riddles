using Riddles.Tests.Probability.Application;
using System;
using System.Collections.Generic;
using System.Text;

namespace Riddles.Probability.Application
{
    /// <summary>
    /// https://thefiddler.substack.com/p/can-you-win-the-collaborative-card
    /// You and a friend have 2 52 card decks. You flip a card at the same time
    /// If the card is the same you both lose, otherwise you continue.
    /// 
    /// What are the odds that you win? What about if you shuffled the decks
    /// together before you started?
    /// </summary>
    public class CollaborativeCardFlippingGameSolver
    {
        private Dictionary<(int, int), double> _cache;
        private DuplicateDeckShufflerAndDivider _duplicateDeckShufflerAndDivider;
        public CollaborativeCardFlippingGameSolver() { 
            this._cache = new Dictionary<(int, int), double>();
            this._duplicateDeckShufflerAndDivider = new 
                DuplicateDeckShufflerAndDivider();
        }

        public double CalculateProbabilityOfWinningShuffledDeck(int numCards)
        {
            var distributions = this._duplicateDeckShufflerAndDivider
                .CalculateDistributionsWithProbability(
                    numCards
                );
            var cumulativeProbability = 0.0;
            foreach(var distribution in distributions)
            {
                cumulativeProbability +=
                    distribution.Item2 * this.CalculateProbabilityOfWinning(
                        distribution.Item1.Item1,
                        distribution.Item1.Item2
                    );
            }
            return cumulativeProbability;
        }

        public double CalculateProbabilityOfWinning(
            int numSharedCardsInEachDeck, 
            int numUniqueCardsInEachDeck)
        {
            // you've hit an impossible scenario with probability = 0
            if(numSharedCardsInEachDeck < 0 || numUniqueCardsInEachDeck < 0)
            {
                return 0;
            }
            // if you don't have any cards in common, you've won
            if (numSharedCardsInEachDeck == 0)
            {
                return 1.0;
            }
            // if you have exactly one of the same card, you've lost
            if (numUniqueCardsInEachDeck == 0 && numSharedCardsInEachDeck == 1)
            {
                return 0.0;
            }
            if (!this._cache.ContainsKey(
                (numSharedCardsInEachDeck, numUniqueCardsInEachDeck)
            ))
            {
                var numCardsInEachDeck = numSharedCardsInEachDeck +
                    numUniqueCardsInEachDeck;
                // you could each draw a unique card
                var oddsOfBothDrawingUniqueCard = 
                    (double)numUniqueCardsInEachDeck / numCardsInEachDeck
                    * numUniqueCardsInEachDeck / numCardsInEachDeck;
                var pWinBothUnique = this.CalculateProbabilityOfWinning(
                    numSharedCardsInEachDeck,
                    numUniqueCardsInEachDeck - 1
                );

                // one of you could draw a unique card and the other could
                // draw a shared card
                var oddsOfDrawingOneUniqueAndOneSharedCard =
                    ((double)numUniqueCardsInEachDeck / numCardsInEachDeck
                    * numSharedCardsInEachDeck / numCardsInEachDeck) * 2;
                var pWinOneUniqueOneShared = this.CalculateProbabilityOfWinning(
                    numSharedCardsInEachDeck - 1,
                    numUniqueCardsInEachDeck
                );
                // you could each draw a non-matching shared card
                var oddsOfBothDrawingSharedNonMatchingCard =
                    (double)numSharedCardsInEachDeck / numCardsInEachDeck 
                    * (numSharedCardsInEachDeck - 1) / numCardsInEachDeck;
                var pWinBothShared = this.CalculateProbabilityOfWinning(
                    numSharedCardsInEachDeck - 2,
                    numUniqueCardsInEachDeck + 1
                );

                // you could each draw the same card and lose
                var oddsOfImmediatelyLosing = 
                    (double)numSharedCardsInEachDeck/(numCardsInEachDeck)
                    * 1 / numCardsInEachDeck;

                this._cache[(numSharedCardsInEachDeck, numUniqueCardsInEachDeck)]
                    = oddsOfBothDrawingUniqueCard * pWinBothUnique
                    + oddsOfDrawingOneUniqueAndOneSharedCard * pWinOneUniqueOneShared
                    + oddsOfBothDrawingSharedNonMatchingCard * pWinBothShared;
            }
            return this._cache[(numSharedCardsInEachDeck, numUniqueCardsInEachDeck)]; 
        }
    }
}
