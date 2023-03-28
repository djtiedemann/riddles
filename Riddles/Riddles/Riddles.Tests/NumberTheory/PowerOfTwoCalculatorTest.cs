using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Riddles.NumberTheory.Core;

namespace Riddles.Tests.NumberTheory
{
    public class PowerOfTwoCalculatorTest
    {
        [TestCase(-11, 1)]
        [TestCase(0, 1)]
        [TestCase(1, 1)]
        [TestCase(2, 2)]
        [TestCase(3, 4)]
        [TestCase(4, 4)]
        [TestCase(5, 8)]
        [TestCase(6, 8)]
        [TestCase(7, 8)]
        [TestCase(8, 8)]
        [TestCase(9, 16)]
        [TestCase(10, 16)]
        [TestCase(11, 16)]
        [TestCase(12, 16)]
        [TestCase(13, 16)]
        [TestCase(14, 16)]
        [TestCase(15, 16)]
        [TestCase(16, 16)]
        [TestCase(17, 32)]
        [TestCase(64, 64)]
        [TestCase(68, 128)]
        public void TestFindNextPowerOfTwoInclusive(int n, int expected)
        {
            var powersOfTwoCalculator = new PowerOfTwoCalculator();
            var actual = powersOfTwoCalculator.FindNextPowerOfTwoInclusive(n);
            Assert.AreEqual(expected, actual);
        }
    }
}
