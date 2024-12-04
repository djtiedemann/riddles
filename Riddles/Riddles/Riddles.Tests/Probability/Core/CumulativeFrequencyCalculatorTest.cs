using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Riddles.Probability.Core;

namespace Riddles.Tests.Probability.Core
{
    public class CumulativeFrequencyCalculatorTest
    {
        private double _epsilon = 0.00000000001;

        Dictionary<int, (Func<int, int, bool>, Dictionary<int, double>, Dictionary<int, double>)>
            _testCaseDictionary = new Dictionary<int, (Func<int, int, bool>, Dictionary<int, double>, Dictionary<int, double>)>
            {
                {
                    1,
                    (
                        (x, y) => x <= y,
                        new Dictionary<int, double> {
                            { 1, 0.2 },
                            { 2, 0.4 },
                            { 3, 0.1 },
                            { 4, 0.1 },
                            { 5, 0.2 },
                        },
                        new Dictionary<int, double> {
                            { 1, 0.2 },
                            { 2, 0.6 },
                            { 3, 0.7 },
                            { 4, 0.8 },
                            { 5, 1.0 },
                        })
                },
                {
                    2,
                    (
                        (x, y) => x >= y,
                        new Dictionary<int, double> {
                            { 1, 0.2 },
                            { 2, 0.4 },
                            { 3, 0.1 },
                            { 4, 0.1 },
                            { 5, 0.2 },
                        },
                        new Dictionary<int, double> {
                            { 1, 1.0 },
                            { 2, 0.8 },
                            { 3, 0.4 },
                            { 4, 0.3 },
                            { 5, 0.2 },
                        })
                }
            };

        [TestCase(1)]
        [TestCase(2)]
        public void TestCalculateCumulativeFrequencyAscending(int testCaseId)
        {
            var cumulativeFrequencyCalculator = 
                new CumulativeFrequencyCalculator();
            var testCase = this._testCaseDictionary[testCaseId];

            var actual = cumulativeFrequencyCalculator
                .CalculateCumulativeFrequency(
                    testCase.Item2,
                    testCase.Item1
                );

            Assert.AreEqual(actual.Keys.Count, testCase.Item3.Keys.Count);
            foreach(var key in actual.Keys ) {
                Assert.LessOrEqual(
                    Math.Abs(actual[key] - testCase.Item3[key]),
                    this._epsilon
                );
            }
        }
    }
}
