using NUnit.Framework;
using Riddles.Combinatorics.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Riddles.Tests.Combinatorics.Core
{
    public class DerangementCalculatorTest
    {
        [TestCase(1, 0)]
        [TestCase(2, 1)]
        [TestCase(3, 2)]
        [TestCase(4, 9)]
        [TestCase(5, 44)]
        [TestCase(6, 265)]
        [TestCase(7, 1_854)]
        [TestCase(8, 14_833)]
        public void TestCalculateDerangement(int n, int expected)
        {
            var derangementCalculator = new DerangementCalculator();
            var actual = derangementCalculator.CalculateNumDerangements(n);
            Assert.AreEqual(expected, actual);
        }
    }
}
