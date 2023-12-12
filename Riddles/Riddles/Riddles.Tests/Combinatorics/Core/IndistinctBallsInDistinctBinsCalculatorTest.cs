using NUnit.Framework;
using Riddles.Combinatorics.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Riddles.Tests.Combinatorics.Core
{
    public class IndistinctBallsInDistinctBinsCalculatorTest
    {
        private Dictionary<int, (List<char>, int, int)> _testCaseDictionary =
            new Dictionary<int, (List<char>, int, int)> {
                {1, (new List<char> { '0' }, 3, 1) },
                {2, (new List<char> { 'H', 'T' }, 3, 4) },
                {3, (new List<char> { '0', '1', '2' }, 2, 6) },
                {4, (new List<char> { '0', '1', '2'}, 3, 10) },
                {5, (new List<char> { '0', '1', '2'}, 4, 15) },
                {6, (new List<char> { '0', '1', '2'}, 5, 21) },
                {7, (new List<char> { '0', '1', '2', '3' }, 2, 10) },
                {8, (new List<char> { '0', '1', '2', '3' }, 3, 20) },
                {9, (new List<char> { '0', '1', '2', '3' }, 4, 35) },
                {10, (new List<char> { '0', '1', '2', '3', '4' }, 2, 15) },
                {11, (new List<char> { '0', '1', '2', '3', '4' }, 3, 35) },
            };

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)]
        [TestCase(7)]
        [TestCase(8)]
        [TestCase(9)]
        [TestCase(10)]
        [TestCase(11)]
        public void TestDetermineWaysToPlaceBallsInBins(int testCaseId)
        {
            (List<char> bins, int numBalls, int expectedNumOutcomes) =
                this._testCaseDictionary[testCaseId];

            var indistinctBallsInDistinctBinsCalculator =
                new IndistinctBallsInDistinctBinsCalculator();
            var actualOutcomes = indistinctBallsInDistinctBinsCalculator
                .DetermineWaysToPlaceBallsInBins(
                    numBalls,
                    bins
                );
            var actualNumOutcomes = indistinctBallsInDistinctBinsCalculator
                .CountNumWaysToPlaceBallsInBins(numBalls, bins.Count);
            Assert.AreEqual( expectedNumOutcomes, actualOutcomes.Count );
            Assert.AreEqual(expectedNumOutcomes, actualNumOutcomes);
        }
    }
}
