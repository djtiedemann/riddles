using Riddles.Combinatorics.Core;
using Riddles.Poker.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Riddles.Poker.Core
{
    public class FiveCardStudPokerHandProbabilityCalculator
    {
        private int _numSuits = 4;
        private int _numDistinctValues = 13;
        private int _numCards;
        private int _numCardsDrawn = 5;
        private BinomialTheoremCalculator _binomialTheoremCalculator;
        public FiveCardStudPokerHandProbabilityCalculator() { 
            this._binomialTheoremCalculator = new BinomialTheoremCalculator();
            this._numCards = this._numSuits * this._numDistinctValues;
        }

        public double CalculateOddsOfHand(
            HandType handType
        )
        {
            var numTotalHands 
                = this._binomialTheoremCalculator.CalculateBinomialCoefficient(52, this._numCardsDrawn);
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
                    return
                        // pick any 5 cards
                        this._binomialTheoremCalculator
                            .CalculateBinomialCoefficient(this._numCards, this._numCardsDrawn)
                        - this.CalculateFrequency(HandType.Pair)
                        - this.CalculateFrequency(HandType.TwoPair)
                        - this.CalculateFrequency(HandType.ThreeOfAKind)
                        - this.CalculateFrequency(HandType.Straight)
                        - this.CalculateFrequency(HandType.Flush)
                        - this.CalculateFrequency(HandType.FullHouse)
                        - this.CalculateFrequency(HandType.FourOfAKind)
                        - this.CalculateFrequency(HandType.StraightFlush)
                        - this.CalculateFrequency(HandType.RoyalFlush);
                        // subtract the odds of all other hand types
                case HandType.Pair:
                    return
                        // pick the value of the pair
                        this._binomialTheoremCalculator
                            .CalculateBinomialCoefficient(this._numDistinctValues, 1) *
                        // pick the suits of the pair
                        this._binomialTheoremCalculator
                            .CalculateBinomialCoefficient(this._numSuits, 2) *
                        // pick the value of the remaining 3 cards
                        this._binomialTheoremCalculator
                            .CalculateBinomialCoefficient(this._numDistinctValues - 1, 3) *
                        // pick the suits of the remaining 3 cards
                        Math.Pow(this._binomialTheoremCalculator
                        .CalculateBinomialCoefficient(this._numSuits, 1), 3);
                case HandType.TwoPair:
                    return
                        // pick the value of each pair
                        this._binomialTheoremCalculator
                            .CalculateBinomialCoefficient(this._numDistinctValues, 2) *
                        // pick the suits of the first pair
                        this._binomialTheoremCalculator
                            .CalculateBinomialCoefficient(this._numSuits, 2) *
                        // pick the suits of the second pair
                        this._binomialTheoremCalculator
                            .CalculateBinomialCoefficient(this._numSuits, 2) *
                        // pick the value of the last card
                        this._binomialTheoremCalculator
                            .CalculateBinomialCoefficient(this._numDistinctValues - 2, 1) *
                        // pick the suit of the last card
                        this._binomialTheoremCalculator
                            .CalculateBinomialCoefficient(this._numSuits, 1);
                case HandType.ThreeOfAKind:
                    return
                        // pick the value of the card that's 3 of a kind
                        this._binomialTheoremCalculator
                            .CalculateBinomialCoefficient(this._numDistinctValues, 1) *
                        // pick the suits of the 3 of a kind
                        this._binomialTheoremCalculator
                            .CalculateBinomialCoefficient(this._numSuits, 3) *
                        // pick the values of the next 2 cards
                        this._binomialTheoremCalculator
                            .CalculateBinomialCoefficient(this._numDistinctValues - 1, 2) *
                        // pick the suit of the last two cards (calculation is same)
                        Math.Pow(this._binomialTheoremCalculator
                            .CalculateBinomialCoefficient(this._numSuits, 1), 2);
                case HandType.Straight:
                    return
                        // pick the starting card of the straight.
                        // the highest 3 cards (J, Q, K) cannot start straights
                        this._binomialTheoremCalculator
                            .CalculateBinomialCoefficient(this._numDistinctValues - 3, 1) *
                        // pick the suit of each card. the calculation is the same for 5 cards
                        Math.Pow(this._binomialTheoremCalculator
                            .CalculateBinomialCoefficient(this._numSuits, 1), 5)
                        // subtract the odds of a straight flush or royal flush
                        - this.CalculateFrequency(HandType.StraightFlush)
                        - this.CalculateFrequency(HandType.RoyalFlush);    
                case HandType.Flush:
                    return
                        // pick the suit of the flush
                        this._binomialTheoremCalculator
                            .CalculateBinomialCoefficient(this._numSuits, 1) *
                        this._binomialTheoremCalculator
                            .CalculateBinomialCoefficient(this._numDistinctValues, 5)
                        // subtract the odds of a straight flush or royal flush
                        - this.CalculateFrequency(HandType.StraightFlush)
                        - this.CalculateFrequency(HandType.RoyalFlush);
                case HandType.FullHouse:
                    return
                        // pick the value of the card we will have 3 of
                        this._binomialTheoremCalculator
                            .CalculateBinomialCoefficient(this._numDistinctValues, 1) *
                        // pick the value of the card we will have 2 of
                        this._binomialTheoremCalculator
                            .CalculateBinomialCoefficient(this._numDistinctValues - 1, 1) *
                        // pick the 3 cards of the first value
                        this._binomialTheoremCalculator
                            .CalculateBinomialCoefficient(this._numSuits, 3) *
                        // pick the 2 cards of the second value
                        this._binomialTheoremCalculator
                            .CalculateBinomialCoefficient(this._numSuits, 2);
                case HandType.FourOfAKind:
                    return
                        // pick the number that we're getting 4 of a kind from
                        this._binomialTheoremCalculator
                            .CalculateBinomialCoefficient(this._numDistinctValues, 1) *
                        // pick 4 cards of that number
                        this._binomialTheoremCalculator
                            .CalculateBinomialCoefficient(this._numSuits, 4) *
                        // pick any remaining card
                        this._binomialTheoremCalculator
                            .CalculateBinomialCoefficient(this._numCards - 4, 1);
                case HandType.StraightFlush:
                    // pick the suit
                    return this._binomialTheoremCalculator.
                        CalculateBinomialCoefficient(this._numSuits, 1) *
                    // pick the number starting the straight.
                    // the highest 3 cards (J, Q, K) cannot start a straight.
                    // the next highest (10) starts a royal flush
                    this._binomialTheoremCalculator
                        .CalculateBinomialCoefficient(this._numDistinctValues - 4, 1);
                case HandType.RoyalFlush:
                    // pick the suit. once the suit is picked the cards are deterministic
                    return this._binomialTheoremCalculator.
                        CalculateBinomialCoefficient(this._numSuits, 1);
            }
            throw new NotImplementedException();
        }
    }
}
