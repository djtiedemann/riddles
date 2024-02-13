using Riddles.Combinatorics.Core;
using Riddles.Poker.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Riddles.Poker.Core
{
    public class FiveCardStudPokerHandProbabilityCalculator
    {
        private BinomialTheoremCalculator _binomialTheoremCalculator;
        public FiveCardStudPokerHandProbabilityCalculator() { 
            this._binomialTheoremCalculator = new BinomialTheoremCalculator();
        }

        public double CalculateOddsOfHand(
            HandType handType,
            int numSuits = 4,
            int numRanks = 13
        )
        {
            var numTotalHands 
                = this._binomialTheoremCalculator
                    .CalculateBinomialCoefficient(numSuits * numRanks, 5);
            var frequency = this.CalculateFrequency(handType, numSuits, numRanks);
            return frequency / numTotalHands;
        }

        private double CalculateFrequency(
            HandType handType,
            int numSuits,
            int numRanks
        )
        {
            switch (handType)
            {
                case HandType.HighCard:
                    return
                        // pick 5 distinct ranks
                        (this._binomialTheoremCalculator
                            .CalculateBinomialCoefficient(numRanks, 5)
                        // subtract straights - the top 3 cards can't form straights
                        - (numRanks - 3))
                        // assign suits to each of the cards
                        // subtract the flushes: 1 per suit
                        * (Math.Pow(numSuits, 5) - numSuits);
                        // subtract the odds of all other hand types
                case HandType.Pair:
                    return
                        // pick the value of the pair
                        this._binomialTheoremCalculator
                            .CalculateBinomialCoefficient(numRanks, 1) *
                        // pick the suits of the pair
                        this._binomialTheoremCalculator
                            .CalculateBinomialCoefficient(numSuits, 2) *
                        // pick the value of the remaining 3 cards
                        this._binomialTheoremCalculator
                            .CalculateBinomialCoefficient(numRanks - 1, 3) *
                        // pick the suits of the remaining 3 cards
                        Math.Pow(this._binomialTheoremCalculator
                        .CalculateBinomialCoefficient(numSuits, 1), 3);
                case HandType.TwoPair:
                    return
                        // pick the value of each pair
                        this._binomialTheoremCalculator
                            .CalculateBinomialCoefficient(numRanks, 2) *
                        // pick the suits of the first pair
                        this._binomialTheoremCalculator
                            .CalculateBinomialCoefficient(numSuits, 2) *
                        // pick the suits of the second pair
                        this._binomialTheoremCalculator
                            .CalculateBinomialCoefficient(numSuits, 2) *
                        // pick the value of the last card
                        this._binomialTheoremCalculator
                            .CalculateBinomialCoefficient(numRanks - 2, 1) *
                        // pick the suit of the last card
                        this._binomialTheoremCalculator
                            .CalculateBinomialCoefficient(numSuits, 1);
                case HandType.ThreeOfAKind:
                    return
                        // pick the value of the card that's 3 of a kind
                        this._binomialTheoremCalculator
                            .CalculateBinomialCoefficient(numRanks, 1) *
                        // pick the suits of the 3 of a kind
                        this._binomialTheoremCalculator
                            .CalculateBinomialCoefficient(numSuits, 3) *
                        // pick the values of the next 2 cards
                        this._binomialTheoremCalculator
                            .CalculateBinomialCoefficient(numRanks - 1, 2) *
                        // pick the suit of the last two cards (calculation is same)
                        Math.Pow(this._binomialTheoremCalculator
                            .CalculateBinomialCoefficient(numSuits, 1), 2);
                case HandType.Straight:
                    return
                        // pick the starting card of the straight.
                        // the highest 3 cards (J, Q, K) cannot start straights
                        this._binomialTheoremCalculator
                            .CalculateBinomialCoefficient(numRanks - 3, 1) *
                        // pick the suit of each card. the calculation is the same for 5 cards
                        Math.Pow(this._binomialTheoremCalculator
                            .CalculateBinomialCoefficient(numSuits, 1), 5)
                        // subtract the odds of a straight flush or royal flush
                        - (numRanks - 3) * numSuits;    
                case HandType.Flush:
                    return
                        // pick the suit of the flush
                        this._binomialTheoremCalculator
                            .CalculateBinomialCoefficient(numSuits, 1) *
                        this._binomialTheoremCalculator
                            .CalculateBinomialCoefficient(numRanks, 5)
                        // subtract the odds of a straight flush or royal flush
                        - this._binomialTheoremCalculator
                            .CalculateBinomialCoefficient(numRanks - 3, 1)
                        * this._binomialTheoremCalculator
                            .CalculateBinomialCoefficient(numSuits, 1);
                case HandType.FullHouse:
                    return
                        // pick the value of the card we will have 3 of
                        this._binomialTheoremCalculator
                            .CalculateBinomialCoefficient(numRanks, 1) *
                        // pick the value of the card we will have 2 of
                        this._binomialTheoremCalculator
                            .CalculateBinomialCoefficient(numRanks - 1, 1) *
                        // pick the 3 cards of the first value
                        this._binomialTheoremCalculator
                            .CalculateBinomialCoefficient(numSuits, 3) *
                        // pick the 2 cards of the second value
                        this._binomialTheoremCalculator
                            .CalculateBinomialCoefficient(numSuits, 2);
                case HandType.FourOfAKind:
                    return
                        // if there are more than 4 suits consider chances of getting
                        // 5 of a kind
                        this._binomialTheoremCalculator
                            .CalculateBinomialCoefficient(numRanks, 1) *
                        this._binomialTheoremCalculator
                            .CalculateBinomialCoefficient(numSuits, 5)

                        // pick the number that we're getting 4 of a kind from
                        + this._binomialTheoremCalculator
                            .CalculateBinomialCoefficient(numRanks, 1) *
                        // pick 4 cards of that number
                        this._binomialTheoremCalculator
                            .CalculateBinomialCoefficient(numSuits, 4) *
                        // pick any remaining card
                        this._binomialTheoremCalculator
                            .CalculateBinomialCoefficient(numSuits * numRanks - numSuits, 1);
                case HandType.StraightFlush:
                    // pick the suit
                    return this._binomialTheoremCalculator.
                        CalculateBinomialCoefficient(numSuits, 1) *
                    // pick the number starting the straight.
                    // the highest 3 cards (J, Q, K) cannot start a straight.
                    // the next highest (10) starts a royal flush
                    this._binomialTheoremCalculator
                        .CalculateBinomialCoefficient(numRanks - 4, 1);
                case HandType.RoyalFlush:
                    // pick the suit. once the suit is picked the cards are deterministic
                    return this._binomialTheoremCalculator.
                        CalculateBinomialCoefficient(numSuits, 1);
            }
            throw new NotImplementedException();
        }
    }
}
