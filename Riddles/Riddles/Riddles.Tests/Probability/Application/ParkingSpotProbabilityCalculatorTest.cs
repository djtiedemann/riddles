using NUnit.Framework;
using Riddles.Probability.Application;
using System;
using System.Collections.Generic;
using System.Text;

namespace Riddles.Tests.Probability.Application
{
    public class ParkingSpotProbabilityCalculatorTest
    {
        private Dictionary<int, (int, int, int, double)> _testCaseDictionary =
            new Dictionary<int, (int, int, int, double)> {
                // no cars
                { 1, (0, 0, 0, 0) },
                // equal number of spaces to people arriving
                { 2, (1, 1, 1, 1.0) },
                // more spaces than people arriving
                { 3, (5, 4, 4, 1.0 )},
                { 4, (10, 4, 5, 1.0 )},
                // no cars leaving
                { 5, (2, 0, 2, 1.0) },
                { 6, (1, 0, 2, 0.5) },
                { 7, (0, 0, 2, 0.0) },
                // non-trivial cases
                { 8, (0, 1, 1, 0.5)},
                { 9, (0, 2, 1, 2.0/3.0)},
                { 10, (0, 1, 2, 1.0/3.0)},
            };

        private double _epsilon = 0.00000000001;

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
        public void TestCalculateProbabilityOfFindingSpot(int testCaseId)
        {
            var testCase = this._testCaseDictionary[testCaseId];
            var parkingSpotProbabilityCalculator = new ParkingSpotProbabilityCalculator();
            var expected = testCase.Item4;
            var actual = parkingSpotProbabilityCalculator.CalculateProbabilityOfFindingSpot(
                    testCase.Item1,
                    testCase.Item2,
                    testCase.Item3
                );

            Assert.LessOrEqual(Math.Abs(actual - expected) / expected, this._epsilon);
        }
    }
}
