using System;
using System.Collections.Generic;
using System.Text;
using Riddles.Probability;
using NUnit.Framework;

namespace Riddles.Tests.Probability
{
    public class RandomElectionSolverTest
    {
        private double epsilon = 0.00000000001;

        private Dictionary<int, (int, int, double)> _testCaseDictionary = new Dictionary<int, (int, int, double)>
        {
            { 1, (221, 2, 1.0) },
            { 2, (221, 3, 2.0 - 4.889056646318E-07) },
            { 3, (221, 10, 8.999999511094332) },
        };

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void TestCalculateExpectedNumberOfRoundsToRandomlyElect(int testCaseId)
        {
            var testCase = this._testCaseDictionary[testCaseId];
            var randomElectionSolver = new RandomElectionSolver();
            var expectedNumberOfTurns = randomElectionSolver
                .CalculateExpectedNumberOfRoundsToRandomlyElect(
                    testCase.Item1,
                    testCase.Item2
                );
            Assert.LessOrEqual( Math.Abs(expectedNumberOfTurns - testCase.Item3)
                , epsilon );
        }
    }
}
