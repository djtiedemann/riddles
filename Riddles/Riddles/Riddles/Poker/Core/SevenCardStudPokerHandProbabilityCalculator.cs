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
                    // there are seven ranks. Pick the seven ranks removing straights
                    return (this._binomialTheoremCalculator
                        .CalculateBinomialCoefficient(this._numDistinctValues, 7)
                    // remove the straights
                    - this.CalculateNumberOfWaysToGetAtLeastFiveConsecutiveRanksFromSevenDistinctRanks())
                    // select the suits, removing flushes
                    * (Math.Pow(this._numSuits, 7) 
                    // numSuits ways to give all 7 cards the same suit
                    - this._numSuits
                    // (7 choose 6) ways to pick 6 cards of the same suit
                    // numSuits ways to pick a suit for the 6 cards
                    // numSuits - 1 ways to pick a suit for the remaining card
                    - this._binomialTheoremCalculator
                        .CalculateBinomialCoefficient(7, 6)
                    * this._numSuits
                    * (this._numSuits - 1)
                    // (7 choose 5) ways to pick 5 cards of the same suits
                    // numSuits ways to pick a suit for the 5
                    // numSuits - 1 ways to pick a suit for each of the remaining 2
                    - this._binomialTheoremCalculator
                        .CalculateBinomialCoefficient(7, 5)
                    * this._numSuits
                    * Math.Pow(this._numSuits - 1, 2)
                    );
                case HandType.Pair:
                    // there are six ranks. Pick the six ranks
                    // remove straights
                    return (this._binomialTheoremCalculator
                        .CalculateBinomialCoefficient(this._numDistinctValues, 6)
                        - this.CalculateNumberOfWaysToGetAtLeastFiveConsecutiveRanksFromSixDistinctRanks()
                    )
                    // pick the one rank with the pair
                    * this._binomialTheoremCalculator
                        .CalculateBinomialCoefficient(6, 1)
                    // pick the suits for the pair
                    * this._binomialTheoremCalculator
                        .CalculateBinomialCoefficient(this._numSuits, 2)
                    // pick the suits for the remaining 5 cards avoiding flushes
                    * (Math.Pow(this._numSuits, 5)
                        // there are numSuits ways to get a flush from the non-pairs alone
                        - this._numSuits
                        // there are 2 suits that can match a pair
                        // and (5 choose 4) ways to pick the cards with the matched suit
                        // the last card can have any other suit
                        - 2 * this._binomialTheoremCalculator
                            .CalculateBinomialCoefficient(5, 4)
                            * this._binomialTheoremCalculator
                            .CalculateBinomialCoefficient(this._numSuits - 1, 1)
                    );  
                case HandType.TwoPair: 
                    // two pairs can either include 3 pairs and 1 single
                    // or 2 pairs and 3 singles
                    //
                    // 1. 2 pairs and 3 singles
                    // pick the 5 ranks of the cards, avoiding the straights
                    // the top 3 cards cannot form straights
                    return (this._binomialTheoremCalculator
                        .CalculateBinomialCoefficient(this._numDistinctValues, 5) - 
                        this._binomialTheoremCalculator
                        .CalculateBinomialCoefficient(this._numDistinctValues - 3, 1))
                        * this.CalculateNumberOfTwoPairHandsAvoidingFlushesGivenFiveRanks()
                    // 2. 3 pairs and 1 single
                    // pick the 4 ranks. Straights and flushes are impossible
                    + this._binomialTheoremCalculator
                        .CalculateBinomialCoefficient(this._numDistinctValues, 4)
                    // pick the 3 ranks that will contain the pairs
                    * this._binomialTheoremCalculator
                        .CalculateBinomialCoefficient(4, 3)
                    // assign suits to the 3 pairs
                    * Math.Pow(this._binomialTheoremCalculator
                        .CalculateBinomialCoefficient(this._numSuits, 2), 3)
                    // assign suits to the single
                    * this._binomialTheoremCalculator
                        .CalculateBinomialCoefficient(this._numSuits, 1)
                    ;
                case HandType.ThreeOfAKind: 
                    // three of a kind hands contain 5 ranks: pick them, excluding straights
                    return (this._binomialTheoremCalculator
                        .CalculateBinomialCoefficient(this._numDistinctValues, 5) 
                        - this._binomialTheoremCalculator
                        .CalculateBinomialCoefficient(this._numDistinctValues - 3, 1))
                        // pick the rank of the 3 of a kind
                        * this._binomialTheoremCalculator.CalculateBinomialCoefficient(5, 1)
                        // pick the suits of the 3 of a kind
                        * this._binomialTheoremCalculator
                            .CalculateBinomialCoefficient(this._numSuits, 3)
                        // pick the suits of the remaining cards
                        // there are only 3 options for a flush
                        // - when all the suits match a suit of the 3 of a kind
                        * (Math.Pow(this._numSuits, 4) - 3);
                case HandType.Straight:
                    // straight can have 5, 6, or 7 distinct cards
                    // 1. 7 distinct cards
                    // start with the full set of straights of 7 distinct values
                    return this.CalculateNumberOfWaysToGetAtLeastFiveConsecutiveRanksFromSevenDistinctRanks()
                        * (
                            // start with all suit assignments, then remove the ones that are flushes
                            Math.Pow(this._numSuits, 7)
                            // for each suit, there is 1 way to pick all cards of the same suit
                            - this._numSuits
                            // if there are 6 cards of the same suit
                            // there are 7 ways to pick the card of the different suit
                            // there are numSuits ways to pick the suit of the 6, numSuits - 1 for the other
                            - this._binomialTheoremCalculator.CalculateBinomialCoefficient(7, 6)
                                * this._binomialTheoremCalculator
                                .CalculateBinomialCoefficient(this._numSuits, 1)
                                * this._binomialTheoremCalculator
                                .CalculateBinomialCoefficient(this._numSuits - 1, 1)
                            // if there are 5 cards of the same suit
                            // there are (7 choose 5) ways to pick the cards of the same suit
                            // there are numSuits ways to pick the suit of the 5, and numSuits 01 for the others
                            - this._binomialTheoremCalculator.CalculateBinomialCoefficient(7, 5)
                                * this._binomialTheoremCalculator
                                .CalculateBinomialCoefficient(this._numSuits, 1)
                                * Math.Pow(this._binomialTheoremCalculator
                                .CalculateBinomialCoefficient(this._numSuits - 1, 1), 2)
                        )
                    // 2. 6 distinct cards
                    // (6 choose 1) choices for the rank and (numSuits choose 2) choices for the suits of the pair
                    // (numSuits)^4 ways to suit the others
                    // then need to remove flushes
                    + this.CalculateNumberOfWaysToGetAtLeastFiveConsecutiveRanksFromSixDistinctRanks() *
                    (this._binomialTheoremCalculator.CalculateBinomialCoefficient(6, 1) *
                        this._binomialTheoremCalculator.CalculateBinomialCoefficient(this._numSuits, 2) *
                        (Math.Pow(this._numSuits, 5)
                        // numSuits ways to pick the 5 cards in the same suit
                        - this._numSuits
                        // (5 choose 1) to pick 4 cards (of the non-paired cards) of the same suit
                        // 2 choices for that suit (each pair card)
                        // (numSuits - 1) choices for the suit of the remaining card
                        - this._binomialTheoremCalculator.CalculateBinomialCoefficient(5, 1) *
                            2 * (this._numSuits - 1)
                        ))
                    // 3. 5 distinct cards
                    // This must contain either a 3 of a kind or 2 pair
                    // numDistinctValues - 3 ways to pick the straight
                    // 3.1 Three of a Kind
                    // 5 ways to pick the 3 of a kind
                    // (numSuits choose 3) choices for the triple
                    + (this._numDistinctValues - 3)
                        * this._binomialTheoremCalculator.CalculateBinomialCoefficient(5, 1)
                        * this._binomialTheoremCalculator.CalculateBinomialCoefficient(this._numSuits, 3)
                        // pick the suits of the remaining 4 cards
                        // only a flush if they all match and match one of the suits of the triple
                        * (Math.Pow(this._numSuits, 4) - this._binomialTheoremCalculator
                            .CalculateBinomialCoefficient(3, 1))
                    // 3.2 Two Pair
                    // (numDistinctValues - 3) ways to choose the cards in the straight
                    // then count the suits, avoiding flushes
                    + (this._numDistinctValues - 3)
                        * this.CalculateNumberOfTwoPairHandsAvoidingFlushesGivenFiveRanks();
                    ; 
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
                            .CalculateBinomialCoefficient(this._numDistinctValues, 7) - 
                            // subtract any flushes that contain at least 5 consecutive cards (straight flushes)
                            this.CalculateNumberOfWaysToGetAtLeastFiveConsecutiveRanksFromSevenDistinctRanks() 
                        )
                    // 6 cards of the same suit, 1 card of a different suit
                    // first pick the suit
                    + this._binomialTheoremCalculator
                        .CalculateBinomialCoefficient(this._numSuits, 1) *
                        (
                            // pick the 6 cards in the suit
                            this._binomialTheoremCalculator
                                .CalculateBinomialCoefficient(this._numDistinctValues, 6)
                            - this.CalculateNumberOfWaysToGetAtLeastFiveConsecutiveRanksFromSixDistinctRanks()

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
        private double CalculateNumberOfWaysToGetAtLeastFiveConsecutiveRanksFromSevenDistinctRanks()
        {
            return
                // 1. Exactly 7 cards are consecutive
                // any value except for the highest 5 cards can start a straight of 7 cards
                (this._numDistinctValues - 5)
                // 2. Exactly 6 cards are consecutive
                // the highest 4 cards cannot start such a straight
                // the lowest and highest remaining card can pair with (numDistinctValues - 7) cards
                // the remaining cards can pair with (numDistinctValues - 8 cards)
                + (2 * (this._numDistinctValues - 7)
                + (this._numDistinctValues - 6) * (this._numDistinctValues - 8))
                // 3. Exactly 5 cards are ordered
                // the highest 3 cards cannot start such a straight
                // the lowest and highest remaining cards can be any of (numDistinctValues - 6) cards
                // the remaining cards can be any of (numDistinctValues - 7) cards
                + (2 * this._binomialTheoremCalculator
                    .CalculateBinomialCoefficient(this._numDistinctValues - 6, 2)
                + (this._numDistinctValues - 5) * this._binomialTheoremCalculator
                    .CalculateBinomialCoefficient(this._numDistinctValues - 7, 2));
        }
        private double CalculateNumberOfWaysToGetAtLeastFiveConsecutiveRanksFromSixDistinctRanks()
        {
            // 1. Exactly 6 cards are all consecutive
            // any value except for the highest 4 cards can start a straight of 6 cards
            return (this._numDistinctValues - 4)
            // 2. Exactly 5 cards are consecutive
            // the highest 3 cards cannot start such a straight
            // the lowest and highest remaining card can pair with (numDistinctValues - 6) cards
            // the remaining cards can pair with (numDistinctValues - 7 cards)
            + (2 * (this._numDistinctValues - 6) +
            ((this._numDistinctValues - 5) * (this._numDistinctValues - 7)));
        }

        // given 5 ranks (two of which will be paired)
        // calculate the number of 2-pair combinations without flushes
        private double CalculateNumberOfTwoPairHandsAvoidingFlushesGivenFiveRanks()
        {
            // (5 choose 2) ways to pick the 2 pairs
            // There are (numSuits choose 2) to assign suits to each pair
            // There are (numSuits^3) ways to assign the suits of the 3 non-paired cards
            // Need to consider cases where the two pairs have 0, 1, or 2 overlapping suits
            // If they have 0 overlapping suits (numSuits - 2 choose 2) * (numSuits choose 2)
            // then all numSuits^3 are valid
            // If they have 1 overlapping suit (numSuits - 2) * 2 * (numSuits choose 2)
            // then 1 of the numSuits^3 is invalid (the one with all 3 matching the one suit)
            // If they have 2 overlapping suits 1 * (numSuits choose 2)
            // then 2 of the pairs are invalid (the ones that match the suits of the pairs)
            return this._binomialTheoremCalculator.CalculateBinomialCoefficient(5, 2)
                // 0 overlapping suits in 2 pairs
                * (this._binomialTheoremCalculator
                .CalculateBinomialCoefficient(this._numSuits, 2)
                * this._binomialTheoremCalculator
                .CalculateBinomialCoefficient(this._numSuits - 2, 2)
                * Math.Pow(this._numSuits, 3)
                // 1 overlapping suits in 2 pairs
                + this._binomialTheoremCalculator
                .CalculateBinomialCoefficient(this._numSuits, 2) * 2 * (this._numSuits - 2)
                * (Math.Pow(this._numSuits, 3) - 1)
                // 2 overlapping suits in 2 pairs
                + this._binomialTheoremCalculator
                .CalculateBinomialCoefficient(this._numSuits, 2)
                * (Math.Pow(this._numSuits, 3) - 2)
                );
        }
    }
}
