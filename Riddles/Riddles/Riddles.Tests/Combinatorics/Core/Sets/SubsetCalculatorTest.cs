using System;
using System.Collections.Generic;
using System.Text;
using Riddles.Combinatorics.Core.Sets;
using NUnit.Framework;
using System.Linq;

namespace Riddles.Tests.Combinatorics.Core.Sets
{
    public class SubsetCalculatorTest
    {
        private Dictionary<int, (List<int>, int)> _calculateSubsetsTestCaseDictionary = new Dictionary<int, (List<int>, int)> {
            { 1, (new List<int> { 4, 4 }, 70 )},
            { 2, (new List<int> { 3, 4 }, 35 )},
            { 3, (new List<int> { 3, 3 }, 20 )},
            { 4, (new List<int> { 3, 2 }, 10 )},
        };

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public void TestCalculateSubsets(int testCaseId)
        {
            var subsetCalculator = new SubsetCalculator();
            var testCase = this._calculateSubsetsTestCaseDictionary[testCaseId];
            var allPermutations = subsetCalculator.CalculateSubsets(testCase.Item1);
            HashSet<string> subsetStringRepresentations = new HashSet<string>();
            foreach (var subset in allPermutations) {
                var stringRepresentation = subset.Aggregate(
                    string.Empty,
                    (agg, x) => $"{agg}|{this.TurnHashSetIntoString(x)}"
                );
                subsetStringRepresentations.Add(stringRepresentation);
            }
            Assert.AreEqual(testCase.Item2, allPermutations.Count);
            Assert.AreEqual(testCase.Item2, subsetStringRepresentations.Count);
        }

        private string TurnHashSetIntoString(HashSet<int> hashSet)
        {
            return hashSet.OrderBy(x => x).Aggregate(string.Empty, (agg, x) => $"{agg}{x}");
        }
    }
}
