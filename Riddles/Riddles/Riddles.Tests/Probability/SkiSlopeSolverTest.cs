using System;
using System.Collections.Generic;
using System.Text;
using Riddles.Probability;
using NUnit.Framework;

namespace Riddles.Tests.Probability
{
	public class SkiSlopeSolverTest
	{
		double epsilon = 0.0001;

		[TestCase(1, 1.0)]
		[TestCase(2, 0.75)]
		[TestCase(3, 0.5833333)]
		[TestCase(4, 0.46875)]
		[TestCase(10, 0.19980468)]
		[TestCase(30, 0.066666)]
		public void TestGetProbabilityOfVictory(int numSkiers, double expectedProbability)
		{
			var skiSlopeSolver = new SkiSlopeSolver();
			var actualProbability = skiSlopeSolver.GetProbabilityOfVictory(numSkiers);
			Assert.LessOrEqual(Math.Abs(expectedProbability - actualProbability), epsilon);
		}
	}
}
