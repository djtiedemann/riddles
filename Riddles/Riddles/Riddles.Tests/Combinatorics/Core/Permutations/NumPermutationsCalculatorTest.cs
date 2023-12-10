using NUnit.Framework;
using Riddles.Combinatorics.Core.Permutations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Riddles.Tests.Combinatorics.Core.Permutations
{
    public class NumPermutationsCalculatorTest
    {
        private Dictionary<int, Dictionary<string, int>> _testCaseDictionary = new Dictionary<int, Dictionary<string, int>> {
            {1, new Dictionary<string, int> { { "H", 3 }, { "T", 4} } },
            {2, new Dictionary<string, int> { { "0", 2}, { "1", 2}, { "2", 2} } },
            {3, new Dictionary<string, int> { { "H", 2}, { "T", 1}, } },
            {4, new Dictionary<string, int> { { "0", 3}, { "1", 2} } },
            {5, new Dictionary<string, int> { { "H", 4 }, { "T", 4} } },
            {6, new Dictionary<string, int> { { "b", 1 }, { "a", 4} , { "n", 2 }, { "g", 1 }, { "r", 1 }, { "m", 1 } } },
            {7, new Dictionary<string, int> { } },
            {8, new Dictionary<string, int> { { "1", 4 } } },
            {9, new Dictionary<string, int> { { "1", 4 }, { "2", 1 } } },
        };

        [TestCase(1, 35)]
        [TestCase(2, 90)]
        [TestCase(3, 3)]
        [TestCase(4, 10)]
        [TestCase(5, 70)]
        [TestCase(6, 75600)]
        [TestCase(7, 0)]
        [TestCase(8, 1)]
        [TestCase(9, 5)]
        public void CalculateNumPermutations(int testCase, int expected)
        {
            var numPermutationsCalculator = new NumPermutationsCalculator();
            var actual = numPermutationsCalculator.CalculateNumPermutations(this._testCaseDictionary[testCase]);
            Assert.AreEqual(expected, actual);
        }
    }
}
