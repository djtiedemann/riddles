using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Riddles.Probability;

namespace Riddles.Tests.Probability
{
	public class HatProblemSolverTest
	{
		[TestCase(1, 1.0)]
		[TestCase(2, 0.5)]
		[TestCase(3, 0.6667)]
		[TestCase(4, 0.625)]
		[TestCase(5, 0.8)]
		public void TestCalculateProbabilityAtLeastOnePersonGetsTheirOwnHat(int numHats, double expectedProbabilitySomeoneDrawsOwnHat)
		{
			double epsilon = 0.0001;
			var hatProblemSolver = new HatProblemSolver();
			var actualProbabilitySomeoneDrawsOwnHat = hatProblemSolver.CalculateProbabilityAtLeastOnePersonGetsTheirOwnHat(numHats);
			Assert.LessOrEqual(Math.Abs(expectedProbabilitySomeoneDrawsOwnHat - actualProbabilitySomeoneDrawsOwnHat), epsilon);
		}
	}
}
