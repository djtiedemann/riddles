using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NUnit.Framework;
using Riddles.Optimization;
using Operation = Riddles.Optimization.OrderOfOperationsMaximizer.Operation;

namespace Riddles.Tests.Optimization
{
    public class OrderOfOperationsMaximizerTest
    {
        private double epsilon = 0.00000000001;
        private Dictionary<int, (List<Operation>, List<double>, double)>
            _testCaseDictionary = new Dictionary<int, (List<Operation>, List<double>, double)>
            {
                { 1, (new List<Operation> { }, new List<double>{ 1.0 }, 1.0)},
                { 2, (new List<Operation> { Operation.Addition }, new List<double>{ 1.0, 5.0 }, 6.0)},
                { 3, (new List<Operation> { Operation.Subtraction }, new List<double>{ 1.0, 5.0 }, 4.0)},
            };

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void TestCalculateMaximumValue(int testCaseId) { 
            var testCase = _testCaseDictionary[testCaseId];
            var orderOfOperationsMaximizer = new OrderOfOperationsMaximizer();
            var actual = orderOfOperationsMaximizer
                .CalculateMaximumValue(testCase.Item2, testCase.Item1);
            Assert.LessOrEqual(Math.Abs(actual.Item1 - testCase.Item3), epsilon);
        }
    }
}
