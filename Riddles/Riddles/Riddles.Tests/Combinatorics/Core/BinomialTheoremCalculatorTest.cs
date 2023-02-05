﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Riddles.Combinatorics.Core;

namespace Riddles.Tests.Combinatorics.Core
{
    public class BinomialTheoremCalculatorTest
    {
        [TestCase(3, -1, 0)]
        [TestCase(3, 4, 0)]
        [TestCase(-1, -4, 0)]
        [TestCase(4, 0, 1)]
        [TestCase(4, 1, 4)]
        [TestCase(4, 2, 6)]
        [TestCase(4, 3, 4)]
        [TestCase(4, 4, 1)]
        public void TestCalculateBinomialCoefficient(int n, int k, int expected)
        {
            var binomialCalculator = new BinomialTheoremCalculator();
            var result = binomialCalculator.CalculateBinomialCoefficient(n, k);
            Assert.AreEqual(expected, result);
        }
    }
}