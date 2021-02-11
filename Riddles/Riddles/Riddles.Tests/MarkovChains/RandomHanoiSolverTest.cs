using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Riddles.MarkovChains;

namespace Riddles.Tests.MarkovChains
{
	public class RandomHanoiSolverTest
	{
		const double Epsilon = 0.000001;
		[TestCase(1, 1)]
		[TestCase(2, 10.6666666)]
		[TestCase(3, 70.77777778)]
		public void TestGetExpectedNumMovesToSolveTowerOfHanoiRandomly(int numRings, double expectedNumMoves)
		{
			var randomHanoiSolver = new RandomHanoiSolver();
			var actualNumMoves = randomHanoiSolver.GetExpectedNumMovesToSolveTowerOfHanoiRandomly(numRings);
			Assert.LessOrEqual(Math.Abs(actualNumMoves.Value - expectedNumMoves), Epsilon);
		}
	}
}
