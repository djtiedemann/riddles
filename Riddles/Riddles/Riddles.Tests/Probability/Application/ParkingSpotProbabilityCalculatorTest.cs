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
                // either 1 car arriving or 1 car leaving
                { 8, (0, 1, 1, 0.5)},
                { 9, (0, 2, 1, 2.0/3.0)},
                { 10, (0, 1, 2, 1.0/3.0)},
                { 11, (0, 1, 3, 1.0/4.0)},
                { 12, (0, 3, 1, 3.0/4.0)},
                // test cases with multiple cars arriving and leaving
                { 13, (0, 2, 2, 7.0/12.0)},
                { 14, (0, 3, 2, 14.0/20.0)},
                { 15, (0, 2, 3, 14.0/30.0)},
                { 16, (0, 3, 3, 38.0/60.0)},
                // actual problem
                { 17, (0, 3, 9, 0.29343434343434349)}
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
        [TestCase(11)]
        [TestCase(12)]
        [TestCase(13)]
        [TestCase(14)]
        [TestCase(15)]
        [TestCase(16)]
        [TestCase(17)]
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
