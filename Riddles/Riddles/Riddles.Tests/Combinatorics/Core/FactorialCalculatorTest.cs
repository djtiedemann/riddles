using System;
using System.Collections.Generic;
using System.Text;
using Riddles.Combinatorics.Core;
using NUnit.Framework;

namespace Riddles.Tests.Combinatorics.Core
{
    public class FactorialCalculatorTest
    {
        [TestCase(0, 1)]
        [TestCase(1, 1)]
        [TestCase(2, 2)]
        [TestCase(3, 6)]
        [TestCase(4, 24)]
        [TestCase(5, 120)]
        [TestCase(6, 720)]
        [TestCase(16, 20922789888000)]
        [TestCase(-5, -120)]
        [TestCase(-6, -720)]
        public void TestCalculateFactorial(int n, double expectedFactorial)
        {
            var factorialCalculator = new FactorialCalculator();
            var actualFactorial = factorialCalculator.Factorial(n);
            Assert.AreEqual(expectedFactorial, actualFactorial);
        }

        [TestCase(1, 2, 0.5)]
        [TestCase(7, 3, 840)]
        public void TestCalculateFactorialDivision(int n, int k, double expected) {
            var factorialCalculator = new FactorialCalculator();
            var actual = factorialCalculator.FactorialDivision(n, k);
            var actual2 = factorialCalculator.FactorialDivision(n, k);
            Assert.AreEqual(expected, actual);
        }
    }
}
