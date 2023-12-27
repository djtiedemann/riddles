using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Riddles.Probability.Application;

namespace Riddles.Tests.Probability.Application
{
    public class CityBlockCarPursuitProbabilitySolverTest
    {
        private double epsilon = 0.00000000001;

        private Dictionary<int, (int, int, double, double, double)> _finiteGridTestCaseDictionary
            = new Dictionary<int, (int, int, double, double, double)>
            {
                { 1, (0, 1, 0.25, 0.25, 1.0)},
                { 2, (0, 0, 0.25, 0.25, 0.0)},
                { 3, (1, 0, 0.25, 0.25, 0.0)},
                { 4, (1, 1, 0.25, 0.25, 0.75)},
                { 5, (2, 1, 0.25, 0.25, 0.625)},
                { 6, (3, 1, 0.25, 0.25, 0.546875)},
                { 7, (4, 1, 0.25, 0.25, 63.0/128.0)},
            };

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)]
        [TestCase(7)]
        public void TestCalculateOddsOfEscaping(int testCaseId)
        {
            var cityBlockCarPursuitProbabilitySolver = new CityBlockCarPursuitProbabilitySolver();
            var testCase = this._finiteGridTestCaseDictionary[testCaseId];
            var actualProbability = cityBlockCarPursuitProbabilitySolver.CalculateOddsOfEscaping(
                testCase.Item1,
                testCase.Item2,
                testCase.Item3,
                testCase.Item4
            );
            Assert.LessOrEqual( Math.Abs(actualProbability - testCase.Item5), epsilon );
        }
    }
}
