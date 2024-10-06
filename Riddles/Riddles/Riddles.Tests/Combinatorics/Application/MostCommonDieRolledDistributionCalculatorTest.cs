using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Riddles.Combinatorics.Application;

namespace Riddles.Tests.Combinatorics.Application
{
    public class MostCommonDieRolledDistributionCalculatorTest
    {
        private Dictionary<int, (int, int, Dictionary<int, double>)> _testCaseDictionary =
            new Dictionary<int, (int, int, Dictionary<int, double>)> {
                { 1, (10, 6, new Dictionary<int, double> {
                    { 2, 4082400 },
                    { 3, 32004000 },
                    { 4, 18774000 },
                    { 5, 4721220 },
                    { 6, 787500 },
                    { 7, 90000 },
                    { 8, 6750 },
                    { 9, 300 },
                    { 10, 6 },
                }) },
                { 2, (3, 6, new Dictionary<int, double>
                {
                    { 1, 120 },
                    { 2, 90 },
                    { 3, 6 }
                })}
            };

        [TestCase(1)]
        [TestCase(2)]
        public void TestCalculateDistributionOfMostCommonValuesRolled(
            int testCaseId)
        {
            var testCase = this._testCaseDictionary[testCaseId];
            var mostCommonDieRolledDistributionCalculator =
                new MostCommonDieRolledDistributionCalculator();
            var actual = mostCommonDieRolledDistributionCalculator
                .CalculateDistributionOfMostCommonValuesRolled(
                    testCase.Item1,
                    testCase.Item2
                );
            var expected = testCase.Item3;
            Assert.AreEqual(expected.Keys.Count, actual.Keys.Count);
            Assert.AreEqual(expected.Values.Sum(),
                Math.Pow(testCase.Item2, testCase.Item1));
            foreach(var key in actual.Keys)
            {
                Assert.AreEqual(expected[key], actual[key]);
            }
        }
    }
}
