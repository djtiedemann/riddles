using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Riddles.Combinatorics.Core.Domain;
using Riddles.Combinatorics.Core.SetGeneration;

namespace Riddles.Combinatorics.Application
{
	public class OlympicClimbingChampionSolver
	{
		private OutcomeGenerator _outcomeGenerator;
		public OlympicClimbingChampionSolver()
		{
			this._outcomeGenerator = new OutcomeGenerator();
		}

		public List<int[]> GenerateDistinctPossiblePlacementsForSingleAthlete(int numContests, int numAthletes)
		{
			var outcomes = this._outcomeGenerator
				.GenerateAllOutcomes(numContests, numAthletes, '1')
				.Select(o => new String(o.ToCharArray().OrderBy(e => e).ToArray()))
				.Distinct()
				.Select(o => o.ToCharArray().Select(c => c - '1' + 1).ToArray())
				.OrderBy(o => o.Aggregate(1, (agg, p) => agg*p))
				.ThenBy(o => o.Aggregate("", (agg, p) => $"{agg}{p}"))
				.ToList();

			return outcomes;
		}

		public WinningScore FindWinningScoreForSpecificContest(Permutation[] results)
		{
			if (results.Length == 0) { return null; }
			var bestScore = int.MaxValue;
			List<int> bestPlayerIds = new List<int> { };
			for (int i = 0; i < results[0].GetPermutation().Length; i++)
			{
				var score = 1;
				for (int j = 0; j < results.Length; j++)
				{
					score *= results[j].GetPermutation()[i];
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
