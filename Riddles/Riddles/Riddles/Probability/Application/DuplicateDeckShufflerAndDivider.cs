using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Riddles.Combinatorics.Core;

namespace Riddles.Tests.Probability.Application
{
    /// <summary>
    /// The idea here is that we take two identical decks, shuffle them
    /// and deal them out to two players.
    /// 
    /// Calculate the distribution of how many cards are shared by each player
    /// and how many pairs of cards each player has that is unique.
    /// </summary>
    public class DuplicateDeckShufflerAndDivider
    {
        private FactorialCalculator _factorialCalculator;
        private BinomialTheoremCalculator _binomialTheoremCalculator;
        public DuplicateDeckShufflerAndDivider()
        {
            this._factorialCalculator = new FactorialCalculator();
            this._binomialTheoremCalculator = new BinomialTheoremCalculator();
        }

        public List<((int, int), double)> CalculateDistributionsWithProbability(
            int numCardsInEachDeck)
        {
            var probabilities = new List<((int, int), double)> ();
            var numPossibleGroupings = this._binomialTheoremCalculator
                .CalculateBinomialCoefficient(numCardsInEachDeck * 2, numCardsInEachDeck);
            // iterate through the number of pairs that each person is holding
            // if numUniquePairs = 0, they each have the same deck
            // if numUniquePairs = numCardsInEachDeck/2, they have no overlapping
            // cards
            for(int numUniquePairs=0; numUniquePairs <= numCardsInEachDeck/2; numUniquePairs ++)
            {
                var numSharedCards = numCardsInEachDeck - numUniquePairs * 2;
                var numWaysToGenerateThisCombination =
                    // first find the number of ways to pick the cards that will be shared
                    this._binomialTheoremCalculator.CalculateBinomialCoefficient(
                        numCardsInEachDeck,
                        numSharedCards
                    ) * 
                    // for each shared card, their are 2 copies and each could go in each
                    // deck
                    Math.Pow(2, numSharedCards) *
                    // for each pair, find the number of ways to split the 
                    // pairs into each hand
                    this._binomialTheoremCalculator.CalculateBinomialCoefficient(
                        numUniquePairs * 2,
                        numUniquePairs
                    );
                var probability = (double)numWaysToGenerateThisCombination /
                    (double)numPossibleGroupings;
                probabilities.Add(((numSharedCards, numUniquePairs * 2), probability));
            }
            var cumulativeProbability = probabilities.Sum(x => x.Item2);
            return probabilities;
        }
    }
}
