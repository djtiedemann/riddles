using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Riddles.Optimization;

namespace Riddles.Tests.Optimization
{
	public class BattingAverageFinderTest
	{
		[TestCase(1000, 4, 239)]
		public void FindLargestNumberOfGamesWhereBattingAverageCantEqualNumGamesTest(int maxNumGames, int numAtBatsPerGame, int expectedNumGames)
		{
			var battingAverageFinder = new BattingAverageFinder();
			var actualNumGames = battingAverageFinder.FindLargestNumberOfGamesWhereBattingAverageCantEqualNumGames(maxNumGames, numAtBatsPerGame);
			Assert.AreEqual(expectedNumGames, actualNumGames);
		}
	}
}
