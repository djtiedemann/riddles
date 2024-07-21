using System;
using System.Collections.Generic;
using System.Text;
using Riddles.Probability.Core;
using NUnit.Framework;

namespace Riddles.Tests.Probability.Core
{
    public class CumulativeTrialOutcomeDistributionCalculatorTest
    {
        private double epsilon = 0.00000000001;
        private Dictionary<int, (List<Dictionary<int, double>>, Dictionary<int, double>)>
            _testCaseDictionary =
                new Dictionary<int, (List<Dictionary<int, double>>, Dictionary<int, double>)>
                {
                    { 1, (
                        new List<Dictionary<int, double>>{
                            new Dictionary<int, double>{
                                { 1, 0.75 },
                                { 2, 0.25 },
                            },
                            new Dictionary<int, double>{
                                { 3, 0.75 },
                                { 4, 0.25 },
                            },
                            new Dictionary<int, double>{ 
                                { 5, 0.75 }, 
                                { 6, 0.25 },
                            },
                        },
                        new Dictionary<int, double>{
                            { 9, 27.0/64.0 },
                            { 10, 27.0/64.0 },
                            { 11, 9.0/64.0 },
                            { 12, 1.0/64.0 }
                        }
                    )},
                    { 2, (
                        new List<Dictionary<int, double>>{
                            new Dictionary<int, double>{
                                { 4, 1.0/8.0 },
                                { 3, 1.0/4.0 },
                                { 2, 5.0/8.0 }
                            },
                            new Dictionary<int, double>{
                                { 4, 1.0/8.0 },
                                { 3, 1.0/4.0 },
                                { 2, 5.0/8.0 }
                            },
                            new Dictionary<int, double>{
                                { -4, 1.0/8.0 },
                                { -3, 1.0/4.0 },
                                { -2, 5.0/8.0 }
                            },
                        },
                        new Dictionary<int, double>{
                            { 0, 0.048828125 },
                            { 1, 0.13671875 },
                            { 2, 0.349609375 },
                            { 3, 0.2578125 },
                            { 4, 0.154296875 },
                            { 5, 0.04296875 },
                            { 6, 0.009765625 }
                        }
                    )},
                };

        [TestCase(1)]
        [TestCase(2)]
        public void TestCalculateCumulativeOutcomeDistributionForTrials(
            int testCaseId)
        {
            var (trials, expected) = _testCaseDictionary[testCaseId];
            var outcomeDistributionCalculator = new
                CumulativeTrialOutcomeDistributionCalculator();
            var actual = outcomeDistributionCalculator
                .CalculateCumulativeOutcomeDistributionForTrials(
                    (int x, int y) => x + y,
                    trials,
                    0,
                    1.0
                );
            Assert.AreEqual(expected.Count, actual.Count);
            foreach(var key in actual.Keys)
            {
                Assert.LessOrEqual(Math.Abs(actual[key] - expected[key]),
                    epsilon);
            }
        }
    }
}
