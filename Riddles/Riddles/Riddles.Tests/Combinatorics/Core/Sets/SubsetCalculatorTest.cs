using System;
using System.Collections.Generic;
using System.Text;
using Riddles.Combinatorics.Core.Sets;
using NUnit.Framework;

namespace Riddles.Tests.Combinatorics.Core.Sets
{
    public class SubsetCalculatorTest
    {
        private Dictionary<int, (List<int>, int)> _calculateSubsetsTestCaseDictionary = new Dictionary<int, (List<int>, int)> {
            { 1, (new List<int> { 4, 4 }, 70 )}
        };

        [TestCase(1)]
        public void TestCalculateSubsets(int testCaseId)
        {
            var subsetCalculator = new SubsetCalculator();
            var testCase = this._calculateSubsetsTestCaseDictionary[testCaseId];
            var allPermutations = subsetCalculator.CalculateSubsets(testCase.Item1);
        }
    }
}
