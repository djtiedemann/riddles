using NUnit.Framework;
using Riddles.Probability.Application;
using System;
using System.Collections.Generic;
using System.Text;

namespace Riddles.Tests.Probability.Application
{
    public class SockMatchSolverTest
    {
        private double epsilon = 0.00000000001;

        private Dictionary<int, (int, int, double)> _testCaseDictionary =
            new Dictionary<int, (int, int, double)> {
                { 1, (1, 1, 1.0) },
                { 2, (2, 1, 1.0/3.0) },
                { 3, (3, 1, (1.0/5.0)*(1.0/3.0)) },
                { 4, (2, 2, 1.0) },
                { 5, (3, 2, 1.0/5.0 + 2.0/5.0)},
                { 6, (14, 9, 0.70048993497640688)}
        };

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)]
        public void TestCalculateProbabilityOfMatchingSocksSuccessfully(int testCaseId)
        {
            var sockMatcherSolver = new SockMatcherSolver();
            var testCase = this._testCaseDictionary[testCaseId];
            var actualProbability = sockMatcherSolver
                .CalculateProbabilityOfMatchingSocksSuccessfully(testCase.Item1, testCase.Item2);
            Assert.LessOrEqual(Math.Abs(actualProbability - testCase.Item3) / actualProbability,
                this.epsilon);
        }
    }
}
