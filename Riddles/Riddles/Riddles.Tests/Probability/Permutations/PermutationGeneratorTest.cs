using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using NUnit.Framework;
using Riddles.Combinatorics.Core.SetGeneration;
using Riddles.Combinatorics.Core;

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

		[TestCase(1)]
		[TestCase(2)]
		[TestCase(3)]
		[TestCase(4)]
		[TestCase(5)]
		public void TestGeneratePermutationFromKey(int numObjects)
		{
			var factorial = new FactorialCalculator().Factorial(numObjects);
			var permutationGenerator = new PermutationGenerator();
			var permutationSet = new HashSet<string>();
			for(int i=0; i<factorial; i++)
			{
				var permutation = permutationGenerator.GeneratePermutationFromKey(i, numObjects).GetPermutation();
				var permutationString = permutation.Aggregate("", (agg, digit) => $"{agg}{digit}");
				Assert.False(permutationSet.Contains(permutationString));
				permutationSet.Add(permutationString);
			}
			Assert.AreEqual(factorial, permutationSet.Count);
			var randomPermutation = permutationGenerator.GenerateRandomPermutation(numObjects);
		}
	}
}
