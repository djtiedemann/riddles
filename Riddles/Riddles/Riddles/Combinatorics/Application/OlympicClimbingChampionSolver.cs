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

		public int FindMinimumWinningScore(int numContests, int numAthletes)
		{
			var maximumSuccessfulScore = 0;
			var maximumAttemptedScore = 0;
			var scoresForSingleAthlete = this.GenerateDistinctPossiblePlacementsForSingleAthlete(numContests, numAthletes);
			foreach(var outcome in scoresForSingleAthlete)
			{
				var score = outcome.Aggregate(1, (agg, v) => agg * v);
				if (score > maximumAttemptedScore && maximumAttemptedScore > maximumSuccessfulScore) {
					return maximumSuccessfulScore;
				}
				if (score == maximumSuccessfulScore) { continue; }
				var canWin = this.CanAthleteWinWithOutcome(numContests, numAthletes, outcome);
				if (canWin)
				{
					maximumSuccessfulScore = score;
				}
				maximumAttemptedScore = score;
			}
			return scoresForSingleAthlete.Select(s => s.Aggregate(1, (agg, v) => agg * v)).Max();
		}

		public bool CanAthleteWinWithOutcome(int numContests, int numAthletes, int[] outcome)
		{
			var firstResult = outcome.Select(o => new List<int> { o }
				.Concat(Enumerable.Range(1, numAthletes - 1).Select(x => x < o ? x : x + 1)).ToArray()).ToList();
			var targetScore = outcome.Aggregate(1, (agg, v) => agg * v);
			var result = firstResult;
			while(result != null)
			{
				var scores = this.FindScoresForContest(result).ToList();
				if (scores.Single(s => s.PlayerId == 0).Score == scores.Min(s => s.Score)) {
					return true;
				}
				result = this.GenerateNextResult(numContests, numAthletes, result, scores, outcome, targetScore);
			}
			return false;
		}

		public List<int[]> GenerateNextResult(
			int numContests, 
			int numAthletes, 
			List<int[]> currentResult, 
			List<PlayerScore> scores, 
			int[] firstPlayerScores, 
			int targetScore
		)
		{
			var playerToIncrease = scores.Where(s => s.Score < targetScore).Min(s => s.PlayerId);
			var challengerScores = currentResult.Select(r => r[playerToIncrease]).ToArray();
			var targetChallengerScores = this.FindStartingPointForNextResult(
				numContests, numAthletes, firstPlayerScores, challengerScores, targetScore
			);
			if (targetChallengerScores == null) { return null; }
			int rowToIncrease = Enumerable.Range(1, numContests - 1)
				.Where(i => targetChallengerScores[i] != challengerScores[i]).Min();
			int colToIncrease = playerToIncrease;
			int targetVal = targetChallengerScores[rowToIncrease];
			while (true)
			{
				var availableValuesToSwap = 
					currentResult[rowToIncrease].Where((val, i) => i > colToIncrease && val >= targetVal);
				if (availableValuesToSwap.Any()) {
					targetVal = availableValuesToSwap.Min();
					break;
				}
				if (colToIncrease > 0) { colToIncrease--; }
				else { 
					rowToIncrease--; 
					colToIncrease = numAthletes - 1;
					if (rowToIncrease == 0) {
						return null;
					}
				}
				targetVal = currentResult[rowToIncrease][colToIncrease] + 1;
			}

			// now, we need to increase the index to the appropriate value, set the challengers to what it should be 
			// and fill everything else in
			var nextResult = Enumerable.Range(0, numContests).Select(i => new int[numAthletes]).ToList();
			for (int i = 0; i < numContests; i++) {
				var possibleResultsForRow = Enumerable.Range(1, numAthletes)
					.Where(v => v != nextResult[i][0] && v != nextResult[i][playerToIncrease]
						&& (i != rowToIncrease || v != nextResult[i][colToIncrease])).OrderBy(i => i).ToList();
				var indexInPossibleResultsForRow = 0;
				for(int j=0; j<numAthletes; j++)
				{
					if(i < rowToIncrease || (i == rowToIncrease && j < colToIncrease) || j == 0)
					{
						nextResult[i][j] = targetVal;
					}
					else if(i == rowToIncrease && j == colToIncrease)
					{
						nextResult[i][j] = targetVal;
					}
					else if(j == playerToIncrease)
					{
						nextResult[i][j] = targetChallengerScores[i - 1];
					}
					else
					{
						nextResult[i][j] = possibleResultsForRow[indexInPossibleResultsForRow];
						indexInPossibleResultsForRow++;
					}
				}
			}
			return nextResult;
		}

		private int[] FindStartingPointForNextResult(
			int numContests, int numAthletes, int[] firstPlayerScores, int[] challengerScores, int targetScore
		)
		{
			var currentOutcome = challengerScores;
			var canTerminate = false;
			while (true)
			{
				var nextOutcome = this._outcomeGenerator.GenerateNextOutcome(challengerScores, 0, numAthletes - 1);
				if (nextOutcome == null || nextOutcome[0] != challengerScores[0]){ return null; }
				var challengerScore = nextOutcome.Aggregate(1, (agg, o) => agg * o);
				var comesInSamePositionAsPlayer =
					Enumerable.Range(1, numContests-1).Where(i => firstPlayerScores[i] == nextOutcome[i]).Any();
				if(challengerScore >= targetScore && !comesInSamePositionAsPlayer) { return nextOutcome; }
				currentOutcome = nextOutcome;

			}
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

		public IEnumerable<PlayerScore> FindScoresForContest(List<int[]> results)
		{
			for (int i = 0; i < results[0].Length; i++)
			{
				var score = 1;
				for (int j = 0; j < results.Count; j++)
				{
					score *= results[j][i];
				}
				yield return new PlayerScore(playerId: i, score: score);
			}
		}

		public class PlayerScore{
			public PlayerScore(int playerId, int score)
			{
				this.PlayerId = playerId;
				this.Score = score;
			}

			public int PlayerId { get; }
			public int Score { get; }
		}
	}
}
