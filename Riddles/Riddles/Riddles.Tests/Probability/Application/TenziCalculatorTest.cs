using System;
using System.Collections.Generic;
using System.Text;
using Riddles.Probability.Application;
using NUnit.Framework;

namespace Riddles.Tests.Probability.Application
{
    public class TenziCalculatorTest
    {
        private double epsilon = 0.00000000001;
        private Dictionary<int, (int, int, double)> _testCaseDictionary
            = new Dictionary<int, (int, int, double)>
            {
                { 1, (10, 6, 3.4447466960702133)},
                { 2, (3, 6, 53.0/36.0)}
            };

        [TestCase(1)]
        [TestCase(2)]
        public void TestCalculateExpectedNumDiceSetAside(int testCaseId)
        {
            var tenziCalculator = new TenziCalculator();
            var testCase = this._testCaseDictionary[testCaseId];
            var expectedValue = tenziCalculator.CalculateExpectedNumDiceSetAside(
                numDice: testCase.Item1,
                numSides: testCase.Item2
            );
            Assert.LessOrEqual( 
                Math.Abs(expectedValue - testCase.Item3),
                epsilon);
        }
    }
}
