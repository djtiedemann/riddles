using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Riddles.Combinatorics.Core.Model;
using Riddles.Combinatorics.Application;
using NUnit.Framework;

namespace Riddles.Tests.Combinatorics.Application
{
	public class OlympicClimbingChampionSolverTest
	{
		[TestCase(3, 8, 48)]
		public void TestFindMinimumWinningScore(int numContests, int numAthletes, int expectedMinScore)
		{
			var olympicClimbingChampionSolver = new OlympicClimbingChampionSolver();
			var minimumScore = olympicClimbingChampionSolver.FindMinimumWinningScore(numContests, numAthletes);
			Assert.AreEqual(expectedMinScore, minimumScore);
		}
	}
}
