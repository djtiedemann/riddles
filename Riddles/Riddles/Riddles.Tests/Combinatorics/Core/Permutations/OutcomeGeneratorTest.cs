using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Riddles.Combinatorics.Core.SetGeneration;
using System.Linq;

namespace Riddles.Tests.Combinatorics.Core.SetGeneration
{
    public class OutcomeGeneratorTest
    {
        private Dictionary<int, (int, int, int, int[], int)> _testCaseDictionary =
            new Dictionary<int, (int, int, int, int[], int)> {
                { 1, (0, 9, 4, new int[] { 3, 9, 5, 0}, 3950) },
                { 2, (0, 9, 3, new int[] { 0, 9, 1}, 91) },
                { 3, (0, 9, 3, null, 1627) },
            };

        [TestCase(1, 1)]
        [TestCase(3, 2)]
        [TestCase(2, 3)]
        [TestCase(5, 3)]
        public void TestGenerateAllPossibleGroupAssignmentsForDistinctGroupsAndDistinctMembers(int numOutcomes, int numTrials)
        {
            var permutationGenerator = new PermutationWithRepetitionGenerator();
            var outcomes = permutationGenerator.GenerateAllOutcomes(numTrials, numOutcomes, '1');
            Assert.AreEqual(outcomes.Count, Math.Pow(numOutcomes, numTrials));
            var maxScore = 0;
            // note, this only works for n < 10
            foreach (var outcome in outcomes)
            {
                var score = int.Parse(outcome.Aggregate("", (score, i) => $"{score}{i}"));
                Assert.Greater(score, maxScore);
                maxScore = score;
            }
        }

        [TestCase(1)] // non-trivial case with 4 digits
        [TestCase(2)] // non-trivial case with 3 digits
        [TestCase(3)] // try and generate too many combinations
        public void TestGenerateNextOutcome(int testCaseId)
        {
            var testCase = this._testCaseDictionary[testCaseId];
            var outcomeGenerator = new PermutationWithRepetitionGenerator();
            var firstOutcome = testCase.Item1;
            var lastOutcome = testCase.Item2;
            var numTrials = testCase.Item3;
            var targetOutcomeArray = testCase.Item4;
            var expectedNumGenerations = testCase.Item5;
            var currentOutcome 
                = Enumerable.Range(0, numTrials).Select(c => firstOutcome).ToArray();
            for(int i=0; i<expectedNumGenerations; i++)
            {
                currentOutcome = outcomeGenerator.GenerateNextOutcome(
                    currentOutcome,
                    firstOutcome,
                    lastOutcome
                );
            }
            if(currentOutcome == null || targetOutcomeArray == null)
            {
                Assert.AreEqual(currentOutcome, targetOutcomeArray);
            }
            else
            {
                Assert.AreEqual(currentOutcome.Length, targetOutcomeArray.Length);
                for(int i=0; i<currentOutcome.Length; i++)
                {
                    Assert.AreEqual(currentOutcome[i], targetOutcomeArray[i]);
                }
            }
        }
    }
}
