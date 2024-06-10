using NUnit.Framework;
using Riddles.Probability.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Riddles.Tests.Probability.Core
{
    public class BernoulliTrialOutcomeDistributionCalculatorTest
    {
        private double epsilon = 0.00000000001;

        private Dictionary<int, (int, int, double, double)>
            _testCaseDictionary = new Dictionary<int, (int, int, double, double)> {
                { 1, (4, 0, 0.5, 1.0 )},
                { 2, (4, 1, 0.5, 15.0/16.0 )},
                { 3, (4, 2, 0.5, 11.0/16.0 )},
                { 4, (4, 3, 0.5, 5.0/16.0 )},
                { 5, (4, 4, 0.5, 1.0/16.0 )},
                { 6, (4, 5, 0.5, 0.0/16.0 )},
                { 7, (4, 0, 0.75, 1.0 )},
                { 8, (4, 1, 0.75, 255.0/256.0 )},
                { 9, (4, 2, 0.75, 243.0/256.0 )},
                { 10, (4, 3, 0.75, 189.0/256.0 )},
                { 11, (4, 4, 0.75, 81.0/256.0 )},
                { 12, (4, 5, 0.75, 0.0/256.0 )},
                { 13, (4, 0, 25, 1.0 )},
                { 14, (4, 1, 0.25, 175.0/256.0 )},
                { 15, (4, 2, 0.25, 67.0/256.0 )},
                { 16, (4, 3, 0.25, 13.0/256.0 )},
                { 17, (4, 4, 0.25, 1.0/256.0 )},
                { 18, (4, 5, 0.25, 0.0/256.0 )},
                { 19, (101, 51, 0.5, 0.5 )},
                { 20, (3, 2, 0.5, 0.5 )},
                { 21, (3, 3, 0.5, 1.0/8.0 )}
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
        [TestCase(12)]
        [TestCase(13)]
        [TestCase(14)]
        [TestCase(15)]
        [TestCase(16)]
        [TestCase(17)]
        [TestCase(18)]
        [TestCase(19)]
        [TestCase(20)]
        [TestCase(21)]
        public void TestCalculateOddsOfAnOutcomeOfAtLeastKSuccesses(int testCaseId)
        {
            var testCase = this._testCaseDictionary[testCaseId];
            var bernoulliTrialOutcomeDistributionCalculator =
                new BernoulliTrialOutcomeDistributionCalculator();
            var actualProbability = bernoulliTrialOutcomeDistributionCalculator
                .CalculateOddsOfAnOutcomeOfAtLeastKSuccesses(
                    testCase.Item1,
                    testCase.Item2,
                    testCase.Item3
                );
            Assert.LessOrEqual(Math.Abs(actualProbability - testCase.Item4),
                this.epsilon);
        }
    }
}
