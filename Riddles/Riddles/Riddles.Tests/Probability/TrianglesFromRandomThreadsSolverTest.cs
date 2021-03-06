using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Riddles.Probability;

namespace Riddles.Tests.Probability
{
	public class TrianglesFromRandomThreadsSolverTest
	{
		double epsilon = 0.000001;

		[TestCase(new int[] { 1, 5, 10, 25 }, 0.34375 )]
		public void TestGetProbabilityOfBeingAbleToFormTriangle(int[] coins, double expectedProbability)
		{
			var trianglesFromRandomThreadsSolver = new TrianglesFromRandomThreadsSolver();
			var actualProbability = trianglesFromRandomThreadsSolver.GetProbabilityOfBeingAbleToFormTriangle(coins);
			Assert.LessOrEqual(Math.Abs(expectedProbability - actualProbability), epsilon);
		}
	}
}
