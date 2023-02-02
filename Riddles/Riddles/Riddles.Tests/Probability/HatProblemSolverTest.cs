using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using NUnit.Framework;
using Riddles.Probability;
using Riddles.Combinatorics.Core.SetGeneration;

namespace Riddles.Tests.Probability
{
	public class HatProblemSolverTest
	{
		[TestCase(1, 1.0)]
		[TestCase(2, 0.5)]
		[TestCase(3, 0.6667)]
		[TestCase(4, 0.625)]
		[TestCase(5, 0.63333)]
		public void TestCalculateProbabilityAtLeastOnePersonGetsTheirOwnHat(int numHats, double expectedProbabilitySomeoneDrawsOwnHat)
		{
			double epsilon = 0.0001;
			var hatProblemSolver = new HatProblemSolver();
			var actualProbabilitySomeoneDrawsOwnHat = hatProblemSolver.CalculateProbabilityAtLeastOnePersonGetsTheirOwnHat(numHats);

			var permutationGenerator = new PermutationWithoutRepetitionGenerator();
			var permutations = permutationGenerator.GenerateAllPermutations(numHats, numHats);
			var actualProbabilitySolvedThroughPermutations =
				(double)permutations.Where(p => p.ContainsAtLeastOneFixedPoint()).Count() / (double)permutations.Count();

			Assert.LessOrEqual(Math.Abs(expectedProbabilitySomeoneDrawsOwnHat - actualProbabilitySomeoneDrawsOwnHat), epsilon);
			Assert.LessOrEqual(Math.Abs(expectedProbabilitySomeoneDrawsOwnHat - actualProbabilitySolvedThroughPermutations), epsilon);
		}
	}
}
