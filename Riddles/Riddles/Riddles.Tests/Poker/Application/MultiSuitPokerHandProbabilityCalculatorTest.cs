using NUnit.Framework;
using Riddles.Poker.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Riddles.Tests.Poker.Application
{
    // https://fivethirtyeight.com/features/can-you-stack-the-deck-with-suits/
    public class MultiSuitPokerHandProbabilityCalculatorTest
    {
        private double Epsilon = 0.00000001;

        [TestCase(16, 4.0336134453781511, 4.0345368916797488)]
        [TestCase(17, 5.5510204081632653, 5.4631083202511777)]
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

        [TestCase(7, 2.4637318387114329, 2.590349663668214)]
        [TestCase(8, 6.87833705174947, 6.6875965358035341)]
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
    }
}
