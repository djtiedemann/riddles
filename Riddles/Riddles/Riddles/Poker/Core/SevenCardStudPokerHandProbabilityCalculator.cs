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
                = this._binomialTheoremCalculator.CalculateBinomialCoefficient(52, 5);
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
                    return 0;
                case HandType.StraightFlush:
                    return 0;
                case HandType.RoyalFlush: 
                    return 0;
            }
            throw new NotImplementedException();
        }
    }
}
