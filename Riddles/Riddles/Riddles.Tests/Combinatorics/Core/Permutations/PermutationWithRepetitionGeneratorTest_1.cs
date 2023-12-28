using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Riddles.Combinatorics.Core.Permutations;
using System.Linq;

namespace Riddles.Tests.Combinatorics.Core.Permutations
{
    public class PermutationWithRepetitionGeneratorTest
    {
        private Dictionary<int, (int, int, int, int, int[], int)> _testCaseDictionary =
            new Dictionary<int, (int, int, int, int, int[], int)> {
                { 1, (0, 9, 2, 4, new int[] { 3, 9, 5, 0}, 3950) },
                { 2, (0, 9, 2, 3, new int[] { 0, 9, 1}, 91) },
                { 3, (0, 9, 2, 3, null, 1627) },
            };

        private Dictionary<int, List<char>> setTestCaseDictionary = new Dictionary<int, List<char>> {
            { 1, new List<char> { 'A', 'B', 'L', 'N'} },
            { 2, new List<char> { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9'} }
        };
        private Dictionary<int, List<string>> outputTestCaseDictionary = new Dictionary<int, List<string>> {
            { 1, new List<string>{ 
                "AAA", "AAB", "AAL", "AAN",
                "ABA", "ABB", "ABL", "ABN",
                "ALA", "ALB", "ALL", "ALN",
                "ANA", "ANB", "ANL", "ANN",
                "BAA", "BAB", "BAL", "BAN",
                "BBA", "BBB", "BBL", "BBN",
                "BLA", "BLB", "BLL", "BLN",
                "BNA", "BNB", "BNL", "BNN",
                "LAA", "LAB", "LAL", "LAN",
                "LBA", "LBB", "LBL", "LBN",
                "LLA", "LLB", "LLL", "LLN",
                "LNA", "LNB", "LNL", "LNN",
                "NAA", "NAB", "NAL", "NAN",
                "NBA", "NBB", "NBL", "NBN",
                "NLA", "NLB", "NLL", "NLN",
                "NNA", "NNB", "NNL", "NNN",
            } }
        };

        [TestCase(1, 1, 3)]
        public void TestGenerateAllPossibleGroupAssignmentsForDistinctGroupsAndDistinctMembersSetApi(
            int testCaseId, 
            int outputId, 
            int numTrials)
        {
            var outcomeGenerator = new PermutationWithRepetitionGenerator();
            var outcomes = outcomeGenerator.GenerateAllOutcomes(numTrials, this.setTestCaseDictionary[testCaseId]);
            var expectedOutput = this.outputTestCaseDictionary[outputId];
            Assert.AreEqual(expectedOutput.Count, outcomes.Count);
            for(int i=0; i<outcomes.Count; i++)
            {
                Assert.AreEqual(expectedOutput[i], outcomes[i]);
            }
        }

        [TestCase(1, 1)]
        [TestCase(3, 2)]
        [TestCase(2, 3)]
        [TestCase(5, 3)]
        public void TestGenerateAllPossibleGroupAssignmentsForDistinctGroupsAndDistinctMembers(int numOutcomes, int numTrials)
        {
            var permutationGenerator = new PermutationWithRepetitionGenerator();
            var potentialOutcomes = Enumerable.Range(0, numOutcomes).Select(x => (char)('1' + x)).ToList();
            var outcomes = permutationGenerator.GenerateAllOutcomes(numTrials, potentialOutcomes);
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
            var outcomeSet = this.setTestCaseDictionary[testCase.Item3];
            var numTrials = testCase.Item4;
            var targetOutcomeArray = testCase.Item5;
            var expectedNumGenerations = testCase.Item6;
            var currentOutcome 
                = Enumerable.Range(0, numTrials).Select(c => firstOutcome).ToArray();
            var currentOutcomeSetApi = outcomeGenerator.GenerateNextOutcome(null, outcomeSet, numTrials);
            for(int i=0; i<expectedNumGenerations; i++)
            {
                currentOutcome = outcomeGenerator.GenerateNextOutcome(
                    currentOutcome,
                    firstOutcome,
                    lastOutcome
                );
                currentOutcomeSetApi = outcomeGenerator.GenerateNextOutcome(
                    currentOutcomeSetApi, 
                    outcomeSet, 
                    numTrials
                );
            }
            if(currentOutcome == null || targetOutcomeArray == null)
            {
                Assert.AreEqual(currentOutcome, targetOutcomeArray);
                // Note, do not verify setAPI here because it intentionally handles the case where it is passed in null
            }
            else
            {
                Assert.AreEqual(int.Parse(currentOutcomeSetApi), expectedNumGenerations);
                Assert.AreEqual(currentOutcome.Length, targetOutcomeArray.Length);
                for(int i=0; i<currentOutcome.Length; i++)
                {
                    Assert.AreEqual(currentOutcome[i], targetOutcomeArray[i]);
                }
            }
        }
    }
}
