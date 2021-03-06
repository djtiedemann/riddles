using System;
using System.Collections.Generic;
using System.Text;

namespace Riddles.Optimization
{
	public class BattingAverageFinder
	{
		// https://fivethirtyeight.com/features/can-you-bat-299-in-299-games/
		public int? FindLargestNumberOfGamesWhereBattingAverageCantEqualNumGames(int maximumNumGames, int numAtBatsPerGame)
		{
			for (int numGames = maximumNumGames; numGames > 0; numGames--) {
				// you want numHits / (numGames * numAtBatsPerGame) = numGames / 1000
				var idealBattingAverage = numGames / 1000.0;

				// we need to do rounding here, so in order to match the decimal, we need to check the ideal value rounded down, and rounded up
				int numHitsToAcheiveBattingAverage = (int)(numGames * numAtBatsPerGame * idealBattingAverage);
				var battingAverageRoundedDown = (double)numHitsToAcheiveBattingAverage / (numGames * numAtBatsPerGame);
				var battingAverageRoundedUp = ((double)(numHitsToAcheiveBattingAverage + 1)) / (numGames * numAtBatsPerGame);
				var truncatedBattingAverageRoundedDown = Math.Round(battingAverageRoundedDown * 1000.0) / 1000.0;
				var truncatedBattingAverageRoundedUp = Math.Round(battingAverageRoundedUp * 1000.0) / 1000.0;
				if (truncatedBattingAverageRoundedDown == idealBattingAverage) 
				{
					continue;
				}
				if (truncatedBattingAverageRoundedUp == idealBattingAverage)
				{
					continue;
				}
				return numGames;
			}
			return null;
		}
	}
}
