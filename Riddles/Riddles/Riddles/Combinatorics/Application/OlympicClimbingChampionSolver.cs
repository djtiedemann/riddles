using System;
using System.Collections.Generic;
using System.Text;
using Riddles.Combinatorics.Core.Domain;

namespace Riddles.Combinatorics.Application
{
	public class OlympicClimbingChampionSolver
	{
		public OlympicClimbingChampionSolver()
		{
			
		}

		public WinningScore FindWinningScore(Permutation[] outcomes)
		{
			if (outcomes.Length == 0) { return null; }
			var bestScore = int.MaxValue;
			List<int> bestPlayerIds = new List<int> { };
			for (int i = 0; i < outcomes[0].GetPermutation().Length; i++)
			{
				var score = 1;
				for (int j = 0; j < outcomes.Length; j++)
				{
					score *= outcomes[j].GetPermutation()[i];
				}				
				if (score == bestScore) {
					bestPlayerIds.Add(i);
				}
				else if (score < bestScore)
				{
					bestScore = score;
					bestPlayerIds = new List<int> { i };
				}
			}
			return new WinningScore(playerIds: bestPlayerIds, score: bestScore);
		}

		public class WinningScore{
			public WinningScore(List<int> playerIds, int score)
			{
				this.WinningPlayerIds = playerIds;
				this.Score = score;
			}

			public List<int> WinningPlayerIds { get; }
			public int Score { get; }
		}


	}
}
