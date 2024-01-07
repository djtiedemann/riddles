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
        private Dictionary<int, (List<Operation>, List<double>, bool, double, int)>
            _testCaseDictionary = new Dictionary<int, (List<Operation>, List<double>, bool, double, int)>
            {
                { 1, (new List<Operation> { }, new List<double>{ 1.0 }, false, 1.0, 1)},
                { 2, (new List<Operation> { Operation.Addition }, new List<double>{ 1.0, 5.0 }, false, 6.0, 2)},
                { 3, (new List<Operation> { Operation.Subtraction }, new List<double>{ 1.0, 5.0 }, false, 4.0, 2)},
                { 4, (new List<Operation> { Operation.Addition, Operation.Multiplication }, new List<double>{ 1.0, 5.0, 9.0 }, false, 54.0, 24)},
                { 5, (
                    new List<Operation> { 
                        Operation.Addition, 
                        Operation.Multiplication,
                        Operation.Subtraction,
                    }, new List<double>{ 1.0, 2.0, 3.0, 4.0 }, false, 19.0, 720)},
                { 6, (
                    new List<Operation> {
                        Operation.Addition,
                        Operation.Multiplication,
                        Operation.Subtraction,
                        Operation.Division
                    }, new List<double>{ 1.0, 2.0, 3.0, 4.0, 5.0 }, true, 35.0, 10_080)},
            };

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)]
        public void TestCalculateMaximumValue(int testCaseId) { 
            var testCase = _testCaseDictionary[testCaseId];
            var orderOfOperationsMaximizer = new OrderOfOperationsMaximizer();
            var actual = orderOfOperationsMaximizer
                .CalculateMaximumValue(testCase.Item2, testCase.Item1, testCase.Item3);
            Assert.LessOrEqual(Math.Abs(actual.Item1 - testCase.Item4), epsilon);
            Assert.AreEqual(actual.Item3, testCase.Item5);
        }
    }
}
