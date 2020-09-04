using NUnit.Framework;
using Riddles.Probability;
using System;
using System.Collections.Generic;
using System.Text;

namespace Riddles.Tests.Probability
{
	public class PerfectWarGameSolverTest
	{
		[Test]
		public void GetExpectedNumberOfGamesBeforePerfectWarGame()
		{
			var duplicateCardProbability = new DuplicateCardProbability();
			var perfectWarGameSolver = new PerfectWarGameSolver(duplicateCardProbability);
			// 159,620,172
			var expectedValue = perfectWarGameSolver.GetExpectedNumberOfGamesBeforePerfectWarGame();
		}
	}
}
