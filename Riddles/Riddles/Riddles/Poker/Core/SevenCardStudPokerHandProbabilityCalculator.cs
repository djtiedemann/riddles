using Riddles.Combinatorics.Core;
using Riddles.Poker.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Riddles.Poker.Core
{
    public class SevenCardStudPokerHandProbabilityCalculator
    {
        private int _numSuits = 4;
        private int _numDistinctValues = 13;
        private int _numCards;
        private int _numCardsDrawn = 7;
        private BinomialTheoremCalculator _binomialTheoremCalculator;

        public SevenCardStudPokerHandProbabilityCalculator()
        {
            this._binomialTheoremCalculator = new BinomialTheoremCalculator();
            this._numCards = this._numSuits * this._numDistinctValues;
        }

        public double CalculateOddsOfHand(
            HandType handType
        )
        {
            var numTotalHands
                = this._binomialTheoremCalculator.CalculateBinomialCoefficient(52, 7);
            var frequency = this.CalculateFrequency(handType);
            return frequency / numTotalHands;
        }

        private double CalculateFrequency(
           HandType handType
        )
        {
            switch (handType)
            {
                case HandType.HighCard:
                    return 0;
                case HandType.Pair:
                    return 0;
                case HandType.TwoPair: 
                    return 0;
                case HandType.ThreeOfAKind: 
                    return 0;
                case HandType.Straight:
                    return 0;
                case HandType.Flush: 
                    return 0;
                case HandType.FullHouse: 
                    return 0;
                case HandType.FourOfAKind:
                    // pick the number that we're getting 4 of a kind from
                    return this._binomialTheoremCalculator
                        .CalculateBinomialCoefficient(this._numDistinctValues, 1) *
                    // pick 4 cards of that number
                    this._binomialTheoremCalculator
                        .CalculateBinomialCoefficient(this._numSuits, 4) *
                    // pick any remaining 3 cards
                    this._binomialTheoremCalculator
                        .CalculateBinomialCoefficient(this._numCards - 4, 3);
                case HandType.StraightFlush:
                    return 0;
                case HandType.RoyalFlush:
                    // pick the suit. once the suit is picked the 5 cards in the royal flush
                    // are deterministic
                    return this._binomialTheoremCalculator.
                        CalculateBinomialCoefficient(this._numSuits, 1) *
                    // pick any 2 other cards
                    this._binomialTheoremCalculator.
                        CalculateBinomialCoefficient(this._numCards - 5, 2);
            }
            throw new NotImplementedException();
        }
    }
}
