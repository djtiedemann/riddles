using System;
using System.Collections.Generic;
using System.Text;
using Riddles.Probability.Application;
using NUnit.Framework;

namespace Riddles.Tests.Probability.Application
{
    public class DungeonsAndDragonsDiceRollerCalculatorTest
    {
        private Dictionary<int, (int, int[], Dictionary<int, double>, double)> testCaseDictionary
            = new Dictionary<int, (int, int[], Dictionary<int, double>, double)> {
                { 1, (1, new int[] { 4, 6, 8, 10, 12, 20 }, 
                    new Dictionary<int, double> { { 1, 1.0 } }, 1.0)},
                { 2, (2, new int[] { 4, 6, 8, 10, 12, 20 },
                    new Dictionary<int, double> { 
                        { 1, 0.093749999999999958 },
                        { 2, 0.90625 },
                    }, 1.906249999999999958)},
                { 3, (3, new int[] { 4, 6, 8, 10, 12, 20 },
                    new Dictionary<int, double> {
                        { 1, 0.010530156893004114 },
                        { 2, 0.24965952932098856 },
                        { 3, 0.73981031378605544 }
                    }, 2.729280156893147554 )},
            };

        private double epsilon = 0.00000000001;

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void TestCalculateProbabilitiesOfEachOutcome(int testCaseId)
        {
            var testCase = testCaseDictionary[testCaseId];
            var dungeonsAndDragonsDiceRollerCalculator =
                new DungeonsAndDragonsDiceRollerCalculator();
            var (actualDictionary, expectedValue) = dungeonsAndDragonsDiceRollerCalculator
                .CalculateOddsOfEachCombination(testCase.Item1, testCase.Item2);
            var expectedDictionary = testCase.Item3;
            Assert.AreEqual(expectedDictionary.Keys.Count, actualDictionary.Keys.Count);
            foreach(var key in expectedDictionary.Keys)
            {
                Assert.LessOrEqual(
                    Math.Abs(expectedDictionary[key] - actualDictionary[key]),
                    epsilon
                );
            }
            Assert.LessOrEqual(Math.Abs(expectedValue - testCase.Item4), epsilon);
        }
    }
}
