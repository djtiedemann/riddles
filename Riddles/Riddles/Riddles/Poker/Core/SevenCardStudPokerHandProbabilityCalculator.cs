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
                    // flush can either have 5, 6, or 7 cards of the same suit
                    // consider these separately
                    //
                    // 7 cards of the same suit
                    // first pick the suit
                    return this._binomialTheoremCalculator
                        .CalculateBinomialCoefficient(this._numSuits, 1) *
                        // choose any 7 cards of that suit
                        (this._binomialTheoremCalculator
                            .CalculateBinomialCoefficient(this._numDistinctValues, 7)
                          // subtract the flushes where the 7 cards are all perfectly ordered
                          // the top 5 cards cannot start such a flush - the others can
                          - (this._numDistinctValues - 5)
                          // subtract the flushes that have exactly 6 ordered cards
                          // and one unordered card
                          // the top 4 cards cannot start such a flush
                          - (2*(this._numDistinctValues - 7)
                            +(this._numDistinctValues-6)*(this._numDistinctValues-8))
                          // subtract the flushes where exactly 5 cards are ordered
                          - (2*this._binomialTheoremCalculator
                            .CalculateBinomialCoefficient(this._numDistinctValues - 6, 2) + 
                            (this._numDistinctValues-5)*this._binomialTheoremCalculator
                            .CalculateBinomialCoefficient(this._numDistinctValues - 7, 2))
                        )
                    // 6 cards of the same suit, 1 card of a different suit
                    // first pick the suit
                    + this._binomialTheoremCalculator
                        .CalculateBinomialCoefficient(this._numSuits, 1) *
                        (
                            // pick the 6 cards in the suit
                            this._binomialTheoremCalculator
                                .CalculateBinomialCoefficient(this._numDistinctValues, 6)
                            // subtract instances where 6 cards are in a row
                            // the top 4 cards can't form such a straight
                            - (this._numDistinctValues - 4)
                            // subtract instances where 5 cards are in a row
                            - (2 * (this._numDistinctValues - 6) + 
                            ((this._numDistinctValues - 5) * (this._numDistinctValues - 7)))

                        ) *
                        // pick the last card, ensure it is of a different suit
                        this._binomialTheoremCalculator
                            .CalculateBinomialCoefficient(this._numCards - this._numDistinctValues
                            , 1)
                    // 5 cards of the same suit, any 2 other cards
                    +
                    // first pick the suit
                    +this._binomialTheoremCalculator
                        .CalculateBinomialCoefficient(this._numSuits, 1) *
                        (
                            // pick the 5 cards in the suit
                            this._binomialTheoremCalculator
                                .CalculateBinomialCoefficient(this._numDistinctValues, 5)
                            // remove the cases where the 5 cards are in a row
                            - (this._numDistinctValues - 3)
                        ) *
                        // the final 2 cards can be any card of a different suit
                        this._binomialTheoremCalculator
                            .CalculateBinomialCoefficient
                                (this._numCards - this._numDistinctValues, 2);

                case HandType.FullHouse:
                    // 3 ways to get a full house: AAAKKQJ, AAAKKQQ, AAAKKKQ
                    //
                    // For AAAKKQJ 
                    // pick the value of the card we will have 3 of
                    return this._binomialTheoremCalculator
                        .CalculateBinomialCoefficient(this._numDistinctValues, 1) *
                    // pick the value of the card we will have 2 of
                    this._binomialTheoremCalculator
                        .CalculateBinomialCoefficient(this._numDistinctValues - 1, 1) *
                    // pick the 3 suits of the first value
                    this._binomialTheoremCalculator
                        .CalculateBinomialCoefficient(this._numSuits, 3) *
                    // pick the 2 suits of the second value
                    this._binomialTheoremCalculator
                        .CalculateBinomialCoefficient(this._numSuits, 2) *
                    // pick the values of the last 2 cards
                    this._binomialTheoremCalculator
                        .CalculateBinomialCoefficient(this._numDistinctValues - 2, 2) *
                    // pick the suit of the last 2 cards
                    Math.Pow(this._binomialTheoremCalculator
                        .CalculateBinomialCoefficient(this._numSuits, 1), 2)
                    + 
                    // For AAAKKQQ
                    // pick the value of the card we will have 3 of
                    this._binomialTheoremCalculator
                        .CalculateBinomialCoefficient(this._numDistinctValues, 1) *
                    // pick the value of the 2 cards we will have 2 of
                    this._binomialTheoremCalculator
                        .CalculateBinomialCoefficient(this._numDistinctValues - 1, 2) *
                    // pick the 3 suits of the first value
                    this._binomialTheoremCalculator
                        .CalculateBinomialCoefficient(this._numSuits, 3) *
                    // pick the 2 suits of the pairs
                    Math.Pow(this._binomialTheoremCalculator
                        .CalculateBinomialCoefficient(this._numSuits, 2), 2)
                    +
                    // For AAAKKKQ
                    // pick the values of the cards we will have 3 of
                    this._binomialTheoremCalculator
                        .CalculateBinomialCoefficient(this._numDistinctValues, 2) *
                    // pick the value of the card we will have 1 of
                    this._binomialTheoremCalculator
                        .CalculateBinomialCoefficient(this._numDistinctValues - 2, 1) *
                    // pick the suits of the cards we have 3 of
                    Math.Pow(this._binomialTheoremCalculator
                        .CalculateBinomialCoefficient(this._numSuits, 3), 2) *
                    // pick the suit of the card we have 1 of
                    this._binomialTheoremCalculator.CalculateBinomialCoefficient(this._numSuits, 1)
                    ;

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
                    // pick the suit of the straight flush
                    return this._binomialTheoremCalculator.
                        CalculateBinomialCoefficient(this._numSuits, 1) *
                    // pick the number starting the straight.
                    // the highest 3 cards (J, Q, K) cannot start a straight.
                    // the next highest (10) starts a royal flush
                    this._binomialTheoremCalculator
                        .CalculateBinomialCoefficient(this._numDistinctValues - 4, 1) *
                    // pick any other 2 cards, but you cannot pick the next card of the straight
                    // if you do, you double count the next straight flush which contains the 
                    // lower card
                    this._binomialTheoremCalculator
                        .CalculateBinomialCoefficient(this._numCards - 6, 2);
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
