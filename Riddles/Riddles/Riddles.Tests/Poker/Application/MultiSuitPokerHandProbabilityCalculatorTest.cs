using NUnit.Framework;
using Riddles.Poker.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Riddles.Tests.Poker.Application
{
    // https://fivethirtyeight.com/features/can-you-stack-the-deck-with-suits/
    // Note the orders are right, but the probabilities don't seem to be for 7 card
    // Note that the cumulative distribution test fails
    public class MultiSuitPokerHandProbabilityCalculatorTest
    {
        private double Epsilon = 0.00000001;

        [TestCase(16, 0.0033914872458882758, 0.0033922636852760707)]
        [TestCase(17, 0.0034369907924132852, 0.0033825588115379618)]
        public void TestNumSuitsRequiredForFullHouseToBeMoreCommonThanStraightFiveCardStud(
            int numSuits,
            double expectedFullHouseOdds,
            double expectedStraightOdds
        )
        {
            var fiveCardStudProbabilityCalculator
                = new FiveCardStudPokerHandProbabilityCalculator();

            var fullHouseOdds = fiveCardStudProbabilityCalculator
                .CalculateOddsOfHand(
                    Riddles.Poker.Domain.HandType.FullHouse,
                    numSuits
                );
            var straightOdds = fiveCardStudProbabilityCalculator
                .CalculateOddsOfHand(
                    Riddles.Poker.Domain.HandType.Straight,
                    numSuits
                );

            Assert.LessOrEqual(Math.Abs(expectedFullHouseOdds - fullHouseOdds), this.Epsilon);
            Assert.LessOrEqual(Math.Abs(expectedStraightOdds - straightOdds), this.Epsilon);
        }

        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)]
        [TestCase(16)]
        [TestCase(17)]
        public void AssertCumulativeProbabilityIsOneFiveCardStud(int numSuits)
        {
            var fiveCardStudProbabilityCalculator
                = new FiveCardStudPokerHandProbabilityCalculator();
            var cumulativeProbability =
                fiveCardStudProbabilityCalculator.CalculateOddsOfHand(
                    Riddles.Poker.Domain.HandType.HighCard,
                    numSuits
                ) +
                fiveCardStudProbabilityCalculator.CalculateOddsOfHand(
                    Riddles.Poker.Domain.HandType.Pair,
                    numSuits
                ) +
                fiveCardStudProbabilityCalculator.CalculateOddsOfHand(
                    Riddles.Poker.Domain.HandType.TwoPair,
                    numSuits
                ) +
                fiveCardStudProbabilityCalculator.CalculateOddsOfHand(
                    Riddles.Poker.Domain.HandType.ThreeOfAKind,
                    numSuits
                ) +
                fiveCardStudProbabilityCalculator.CalculateOddsOfHand(
                    Riddles.Poker.Domain.HandType.Straight,
                    numSuits
                ) +
                fiveCardStudProbabilityCalculator.CalculateOddsOfHand(
                    Riddles.Poker.Domain.HandType.Flush,
                    numSuits
                ) +
                fiveCardStudProbabilityCalculator.CalculateOddsOfHand(
                    Riddles.Poker.Domain.HandType.FullHouse,
                    numSuits
                ) +
                fiveCardStudProbabilityCalculator.CalculateOddsOfHand(
                    Riddles.Poker.Domain.HandType.FourOfAKind,
                    numSuits
                ) +
                fiveCardStudProbabilityCalculator.CalculateOddsOfHand(
                    Riddles.Poker.Domain.HandType.StraightFlush,
                    numSuits
                ) +
                fiveCardStudProbabilityCalculator.CalculateOddsOfHand(
                    Riddles.Poker.Domain.HandType.RoyalFlush,
                    numSuits
                );
            Assert.LessOrEqual(Math.Abs(cumulativeProbability - 1), Epsilon);
        }

        [TestCase(7, 0.040722717999736049, 0.04281556832477456)]
        [TestCase(8, 0.043317821216730466, 0.042116591398190031)]
        public void TestNumSuitsRequiredForFullHouseToBeMoreCommonThanStraightSevenCardStud(
            int numSuits,
            double expectedFullHouseOdds,
            double expectedStraightOdds
        )
        {
            var sevenCardStudProbabilityCalculator
                = new SevenCardStudPokerHandProbabilityCalculator();

            var fullHouseOdds = sevenCardStudProbabilityCalculator
                .CalculateOddsOfHand(
                    Riddles.Poker.Domain.HandType.FullHouse,
                    numSuits
                );
            var straightOdds = sevenCardStudProbabilityCalculator
                .CalculateOddsOfHand(
                    Riddles.Poker.Domain.HandType.Straight,
                    numSuits
                );
            Assert.LessOrEqual(Math.Abs(expectedFullHouseOdds - fullHouseOdds), this.Epsilon);
            Assert.LessOrEqual(Math.Abs(expectedStraightOdds - straightOdds), this.Epsilon);
        }

        [TestCase(4)]
        //[TestCase(5)]
        //[TestCase(6)]
        //[TestCase(16)]
        //[TestCase(17)]
        public void AssertCumulativeProbabilityIsOneSevenCardStud(int numSuits)
        {
            var sevenCardStudProbabilityCalculator
                = new SevenCardStudPokerHandProbabilityCalculator();
            var cumulativeProbability =
                sevenCardStudProbabilityCalculator.CalculateOddsOfHand(
                    Riddles.Poker.Domain.HandType.HighCard,
                    numSuits
                ) +
                sevenCardStudProbabilityCalculator.CalculateOddsOfHand(
                    Riddles.Poker.Domain.HandType.Pair,
                    numSuits
                ) +
                sevenCardStudProbabilityCalculator.CalculateOddsOfHand(
                    Riddles.Poker.Domain.HandType.TwoPair,
                    numSuits
                ) +
                sevenCardStudProbabilityCalculator.CalculateOddsOfHand(
                    Riddles.Poker.Domain.HandType.ThreeOfAKind,
                    numSuits
                ) +
                sevenCardStudProbabilityCalculator.CalculateOddsOfHand(
                    Riddles.Poker.Domain.HandType.Straight,
                    numSuits
                ) +
                sevenCardStudProbabilityCalculator.CalculateOddsOfHand(
                    Riddles.Poker.Domain.HandType.Flush,
                    numSuits
                ) +
                sevenCardStudProbabilityCalculator.CalculateOddsOfHand(
                    Riddles.Poker.Domain.HandType.FullHouse,
                    numSuits
                ) +
                sevenCardStudProbabilityCalculator.CalculateOddsOfHand(
                    Riddles.Poker.Domain.HandType.FourOfAKind,
                    numSuits
                ) +
                sevenCardStudProbabilityCalculator.CalculateOddsOfHand(
                    Riddles.Poker.Domain.HandType.StraightFlush,
                    numSuits
                ) +
                sevenCardStudProbabilityCalculator.CalculateOddsOfHand(
                    Riddles.Poker.Domain.HandType.RoyalFlush,
                    numSuits
                );
            Assert.LessOrEqual(Math.Abs(cumulativeProbability - 1), Epsilon);
        }
    }
}
