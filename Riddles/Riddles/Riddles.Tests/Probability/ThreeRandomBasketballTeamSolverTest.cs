using System;
using System.Collections.Generic;
using System.Text;
using Riddles.Probability;
using NUnit.Framework;

namespace Riddles.Tests.Probability
{
	public class ThreeRandomBasketballTeamSolverTest
	{
		[TestCase(1, 0.0)]
		[TestCase(2, 0.375)]
		[TestCase(3, 0.333333)]
		[TestCase(5, 0.24)]
		public void TestFindProbabilityOfEvenTeamsUsingEqualPopulationsEqualProbabilities(int population, double expectedProbability)
		{
			double epsilon = 0.0001;
			var threeRandomBasketballTeamSolver = new ThreeRandomBasketballTeamSolver();
			var actualProbability = threeRandomBasketballTeamSolver.FindProbabilityOfEvenTeamsUsingEqualPopulationsEqualProbabilities(population);
			Assert.LessOrEqual(Math.Abs(expectedProbability - actualProbability), epsilon);
		}
	}	
}
