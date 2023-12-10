using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using NUnit.Framework;
using Riddles.Combinatorics.Core.SetGeneration;
using Riddles.Combinatorics.Core;
using static Riddles.Combinatorics.Core.SetGeneration.PermutationWithoutRepetitionGenerator;

namespace Riddles.Tests.Combinatorics.Core.Permutations
{
    public class PermutationGeneratorTest
    {
        [TestCase(1, 1)]
        [TestCase(2, 2)]
        [TestCase(3, 3)]
        [TestCase(4, 4)]
        [TestCase(5, 5)]
        public void TestGeneratePermutations(int n, int r)
        {
            var permutationGenerator = new PermutationWithoutRepetitionGenerator();
            var permutations = permutationGenerator.GenerateAllPermutations(n, r);
            int nFactorial = Enumerable.Range(1, n).Aggregate(1, (p, item) => p * item);
            Assert.AreEqual(nFactorial, permutations.Count);
            var maxScore = 0;
            // note, this only works for n < 10
            foreach (var permutation in permutations)
            {
                var score = int.Parse(permutation.GetPermutation().Aggregate("", (score, i) => $"{score}{i}"));
                Assert.Greater(score, maxScore);
                maxScore = score;
            }
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        public void TestGeneratePermutationFromKey(int numObjects)
        {
            var factorial = new FactorialCalculator().Factorial(numObjects);
            var permutationGenerator = new PermutationWithoutRepetitionGenerator();
            var permutationSet = new HashSet<string>();
            for (int i = 0; i < factorial; i++)
            {
                var permutation = permutationGenerator.GeneratePermutationFromKey(i, numObjects).GetPermutation();
                var permutationString = permutation.Aggregate("", (agg, digit) => $"{agg}{digit}");
                Assert.False(permutationSet.Contains(permutationString));
                permutationSet.Add(permutationString);
            }
            Assert.AreEqual(factorial, permutationSet.Count);
        }

        [TestCase(new int[] { 1, 3, 5, 7, 9 }, 120)]
        [TestCase(new int[] { 0, 0, 0, 0, 1 }, 5)]
        [TestCase(new int[] { 0, 0, 0, 1, 1 }, 10)]
        [TestCase(new int[] { 0, 0, 1, 1, 1 }, 10)]
        [TestCase(new int[] { 0, 1, 1, 1, 1 }, 5)]
        public void TestGenerateNextPermutation(int[] permutationArray, int expectedNumPermutations)
        {
            var permutationGenerator = new PermutationWithoutRepetitionGenerator();
            var permutationState = new PermutationState(
                null, null, permutationArray, permutationArray.Length

            );
            var hasNextPermutation = true;
            int numPermutations = 0;
            while (hasNextPermutation)
            {
                permutationState = permutationGenerator.GenerateNextPermutation(permutationState);
                if (permutationState.CurrentPermutation == null)
                {
                    break;
                }
                numPermutations++;
            }
            Assert.AreEqual(expectedNumPermutations, numPermutations);
        }
        [TestCase(5, 3, 60)]
        [TestCase(26, 3, 15600)]
        //[TestCase(26, 5, 7893600)] - takes 2.3 seconds, so commented out
        public void TestIncompletePermutation(int n, int r, int expectedNumPermutations)
        {

            var permutationGenerator = new PermutationWithoutRepetitionGenerator();
            var permutationState = new PermutationState(
                null, null, Enumerable.Range(1, n).ToArray(), r

            );
            var hasNextPermutation = true;
            int numPermutations = 0;
            while (hasNextPermutation)
            {
                permutationState = permutationGenerator.GenerateNextPermutation(permutationState);
                if (permutationState.CurrentPermutation == null)
                {
                    break;
                }
                numPermutations++;
            }
            Assert.AreEqual(expectedNumPermutations, numPermutations);
        }

        [TestCase("01100", 10)]
        [TestCase("HTTHHTHT", 70)]
        [TestCase("012021", 90)]
        [TestCase("bananagram", 75600)]
        public void TestGenerateAllAnagrams(string word, int expectedNumAnagrams)
        {
            var permutationGenerator = new PermutationWithoutRepetitionGenerator();
            var permutations = permutationGenerator.GenerateAllPermutationsOfString(word);
            Assert.AreEqual(expectedNumAnagrams, permutations.Count);
        }
    }
}
