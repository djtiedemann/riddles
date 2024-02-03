using NUnit.Framework;
using Riddles.Optimization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Riddles.Tests.Optimization
{
    public class RecursiveDigitSummerTest
    {
        //[TestCase("0", 0)]
        [TestCase("10", 1)]
        [TestCase("19", 2)]
        [TestCase("199", 3)]
        [TestCase("19999999999999999999999", 4)]
        public void TestCalculateDepth(string n, int expected)
        {
            var recursiveDigitSummer = new RecursiveDigitSummer();
            var actual = recursiveDigitSummer.CalculateDepth(n);
            Assert.AreEqual(actual, expected);
        }

        [TestCase(3, 10_000, 945)]
        public void TestCalculateNumNumbersSummingToN(int n, int numValues, int expected)
        {
            var recursiveDigitSummer = new RecursiveDigitSummer();
            var actual = recursiveDigitSummer.CalculateNumNumbersSummingToN(numValues, n);
            Assert.AreEqual(actual, expected);
        }
    }
}
