using NUnit.Framework;
using Riddles.Combinatorics.Application;
using System;
using System.Collections.Generic;
using System.Text;

namespace Riddles.Tests.Combinatorics.Application
{
    public class MarchMadnessPowerRankingOptionsCalculatorTest
    {
        [TestCase(0, 1)]
        [TestCase(1, 1)]
        [TestCase(2, 3)]
        [TestCase(3, 315)]
        [TestCase(4, 638_512_875)]
        public void TestCalculateNumPossiblePowerRankings(int numRounds, int expectedNumPossiblePowerRankings)
        {
            var marchMadnessPowerRankingOptionsCalculator = new MarchMadnessPowerRankingOptionsCalculator();
            var actualNumPossiblePowerRankings = marchMadnessPowerRankingOptionsCalculator.CalculateNumPossiblePowerRankings(numRounds);
            Assert.AreEqual(expectedNumPossiblePowerRankings, actualNumPossiblePowerRankings);
        }
    }
}
