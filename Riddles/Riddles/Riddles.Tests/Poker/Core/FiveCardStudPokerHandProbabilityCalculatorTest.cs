using NUnit.Framework;
using Riddles.Poker.Core;
using Riddles.Poker.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Riddles.Tests.Poker.Core
{
    public class FiveCardStudPokerHandProbabilityCalculatorTest
    {
        private double Epsilon = 0.00000001;
        private static double NumHands = 2_598_960;
        private Dictionary<int, PokerHandProbabilityCalculatorTestTestCase> _testCaseDictionary =
            new Dictionary<int, PokerHandProbabilityCalculatorTestTestCase>
            {
                {1, new PokerHandProbabilityCalculatorTestTestCase{ 
                    NumSuits = 4,
                    NumDistinctValues = 13,
                    HandType = HandType.RoyalFlush,
                    ExpectedProbability = 4 / NumHands
                } },
                {2, new PokerHandProbabilityCalculatorTestTestCase{
                    NumSuits = 4,
                    NumDistinctValues = 13,
                    HandType = HandType.StraightFlush,
                    ExpectedProbability = 36 / NumHands
                } },
                {3, new PokerHandProbabilityCalculatorTestTestCase{
                    NumSuits = 4,
                    NumDistinctValues = 13,
                    HandType = HandType.FourOfAKind,
                    ExpectedProbability = 624 / NumHands
                } },
                {4, new PokerHandProbabilityCalculatorTestTestCase{
                    NumSuits = 4,
                    NumDistinctValues = 13,
                    HandType = HandType.FullHouse,
                    ExpectedProbability = 3_744 / NumHands
                } },
                {5, new PokerHandProbabilityCalculatorTestTestCase{
                    NumSuits = 4,
                    NumDistinctValues = 13,
                    HandType = HandType.Flush,
                    ExpectedProbability = 5_108 / NumHands
                } },
                {6, new PokerHandProbabilityCalculatorTestTestCase{
                    NumSuits = 4,
                    NumDistinctValues = 13,
                    HandType = HandType.Straight,
                    ExpectedProbability = 10_200 / NumHands
                } },
                {7, new PokerHandProbabilityCalculatorTestTestCase{
                    NumSuits = 4,
                    NumDistinctValues = 13,
                    HandType = HandType.ThreeOfAKind,
                    ExpectedProbability = 54_912 / NumHands
                } },
                {8, new PokerHandProbabilityCalculatorTestTestCase{
                    NumSuits = 4,
                    NumDistinctValues = 13,
                    HandType = HandType.TwoPair,
                    ExpectedProbability = 123_552 / NumHands
                } },
                {9, new PokerHandProbabilityCalculatorTestTestCase{
                    NumSuits = 4,
                    NumDistinctValues = 13,
                    HandType = HandType.Pair,
                    ExpectedProbability = 1_098_240 / NumHands
                } },
                {10, new PokerHandProbabilityCalculatorTestTestCase{
                    NumSuits = 4,
                    NumDistinctValues = 13,
                    HandType = HandType.HighCard,
                    ExpectedProbability = 1_302_540 / NumHands
                } },
            };
        [TestCase(1, Description = "Royal Flush, standard deck")]
        [TestCase(2, Description = "Straight Flush, standard deck")]
        [TestCase(3, Description = "Four of a Kind, standard deck")]
        [TestCase(4, Description = "Full House, standard deck")]
        [TestCase(5, Description = "Flush, standard deck")]
        [TestCase(6, Description = "Straight, standard deck")]
        [TestCase(7, Description = "Three of a Kind, standard deck")]
        [TestCase(8, Description = "Two Pair, standard deck")]
        [TestCase(9, Description = "One Pair, standard deck")]
        [TestCase(10, Description = "High Card, standard deck")]
        public void TestCalculateOddsOfHand(int testCaseId)
        {
            var testCase = this._testCaseDictionary[testCaseId];
            var pokerHandProbabilityCalculator = new FiveCardStudPokerHandProbabilityCalculator();
            var actualProbability = pokerHandProbabilityCalculator.CalculateOddsOfHand(
                testCase.HandType
            );
            Assert.LessOrEqual(
                Math.Abs(actualProbability - testCase.ExpectedProbability),
                Epsilon
            );
        }

        private class PokerHandProbabilityCalculatorTestTestCase
        {
            public int NumSuits { get; set; }
            public int NumDistinctValues { get; set; }
            public HandType HandType { get; set; }
            public double ExpectedProbability { get; set; }
        }
    }
}
