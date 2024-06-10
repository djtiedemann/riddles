using NUnit.Framework;
using Riddles.Probability;
using System;
using System.Collections.Generic;
using System.Text;

namespace Riddles.Tests.Probability
{
    public class BasketballComebackProbabilityCalculatorTest
    {
        private double epsilon = 0.00000000001;

        private Dictionary<int, (int, double, double, double)>
        _testCaseDictionary = new Dictionary<int, (int, double, double, double)> {
            { 1, (5, 0.5, 0.75, 3.0/16.0) },
            { 2, (21, 0.5, 0.9, 0.053374931489523214) },
            { 3, (51, 0.5, 0.9, 0.069130088731669037) },
            { 4, (101, 0.5, 0.9, 0.0774198767763915) },
            { 5, (201, 0.5, 0.9, 0.08367378584949936) },
            { 6, (251, 0.5, 0.9, 0.085326607074818761) },
            { 7, (269, 0.5, 0.9, 0.085793402181987832) },
        };


        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)]
        [TestCase(7)]
        public void TestCalculateProbabilityOfComeback(int testCaseId)
        {
            var basketballComebackProbabilityCalculator =
                new BasketballComebackProbabilityCalculator();
            var testCase = this._testCaseDictionary[testCaseId];
            var actualProbability =
                basketballComebackProbabilityCalculator
                    .CalculateProbabilityOfComeback(
                        testCase.Item1,
                        testCase.Item2,
                        testCase.Item3
                    );
            Assert.LessOrEqual(Math.Abs(actualProbability - testCase.Item4),
                this.epsilon);
        }
    }
}
