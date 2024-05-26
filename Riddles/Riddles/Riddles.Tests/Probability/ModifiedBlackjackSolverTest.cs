using NUnit.Framework;
using Riddles.Probability;
using System;
using System.Collections.Generic;
using System.Text;

namespace Riddles.Tests.Probability
{
    public class ModifiedBlackjackSolverTest
    {
        private double epsilon = 0.00000000001;
        private Dictionary<int, (List<int>, int, bool, double)> _testCaseDictionary
            = new Dictionary<int, (List<int>, int, bool, double)> {
                { 1, (new List<int> { 1, 2, 3, 4, 5 }, 6, false, 0.3)},
                { 2, (new List<int> { 1, 2, 3, 4, 5 }, 6, true, 0.05)},
                { 3, (new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, 21, 
                    true, (10 * 5_040 + 30 * 720 + 24 * 120) /3_628_800.0)},
                // 0.02063492063
                { 4, (new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, 21,
                    false, 0.175)},

        };

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public void TestCalculateOddsOfHitting21Exactly(int testCaseId)
        {
            var testCase = this._testCaseDictionary[testCaseId];
            var modifiedBlackjackSolver = new ModifiedBlackjackSolver();
            var actualProbability = modifiedBlackjackSolver
                .CalculateOddsOfHitting21Exactly(
                    testCase.Item1,
                    testCase.Item3,
                    testCase.Item2
                );
            Assert.LessOrEqual(Math.Abs(testCase.Item4 - actualProbability),
                epsilon);
        }
    }
}
