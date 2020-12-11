using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using NUnit.Framework;
using Riddles.Probability.Permutations;

namespace Riddles.Tests.Probability.Permutations
{
	public class PermutationGeneratorTest
	{
		[TestCase(1)]
		[TestCase(2)]
		[TestCase(3)]
		[TestCase(4)]
		[TestCase(5)]
		public void TestGeneratePermutations(int n)
		{
			var permutationGenerator = new PermutationGenerator();
			var permutations = permutationGenerator.GeneratePermutationsOfNObjects(n);
			int nFactorial = Enumerable.Range(1, n).Aggregate(1, (p, item) => p * item);
			Assert.AreEqual(nFactorial, permutations.Count);
			var maxScore = 0;
			// note, this only works for n < 10
			foreach (var permutation in permutations) {
				var score = int.Parse(permutation.GetPermutation().Aggregate("", (score, i) => $"{score}{i}"));
				Assert.Greater(score, maxScore);
				maxScore = score;
			}
		}
	}
}
