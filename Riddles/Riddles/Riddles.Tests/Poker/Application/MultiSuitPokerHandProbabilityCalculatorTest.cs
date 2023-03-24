using Riddles.Poker.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Riddles.Tests.Poker.Application
{
    public class MultiSuitPokerHandProbabilityCalculatorTest
    {
        public void TestNumSuitsRequiredForFullHouseToBeMoreCommonThanStraightFiveCardStud()
        {
            var fiveCardStudProbabilityCalculator
                = new FiveCardStudPokerHandProbabilityCalculator();

            for(int numSuits = 4; numSuits<100; numSuits++)
            {

            }
        }

        public void TestNumSuitsRequiredForFullHouseToBeMoreCommonThanStraightSevenCardStud()
        {

        }
    }
}
