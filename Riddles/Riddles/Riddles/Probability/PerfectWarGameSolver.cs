using System;
using System.Collections.Generic;
using System.Text;

namespace Riddles.Probability
{
	public class PerfectWarGameSolver
	{
		DuplicateCardProbability _duplicateCardProbability;
		public PerfectWarGameSolver(DuplicateCardProbability duplicateCardProbability)
		{
			this._duplicateCardProbability = duplicateCardProbability;
		}

		//https://fivethirtyeight.com/features/can-you-cover-the-globe/
		public double GetExpectedNumberOfGamesBeforePerfectWarGame()
		{
			// if you draw a war, it's impossible to have a perfect game
			var oddsOfNotDrawingAWarInAGame = 1 - this._duplicateCardProbability.FindProbabilityOfDrawingDuplicateCardAfterDrawing2CardsUntilEmpty(
				13, 4);
			// if there are no wars, someone must win the first round, so the first round doesn't matter. that person must then win the remaining
			// 25 rounds, with odds (1/2^25)
			var oddsOfPerfectGameGivenNoWarsDrawn = 1.0 / Math.Pow(2, 25);

			var oddsOfAPerfectWarGame = oddsOfNotDrawingAWarInAGame * oddsOfPerfectGameGivenNoWarsDrawn;
			var expectedNumberOfGames = 1.0 / oddsOfAPerfectWarGame;
			return expectedNumberOfGames;
		}
	}
}
