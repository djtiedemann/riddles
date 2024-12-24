using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Riddles.Combinatorics.Core.Model;
using Riddles.Combinatorics.Core.Permutations;

namespace Riddles.Combinatorics.Application
{
	public class OlympicClimbingChampionSolver
	{
		private PermutationWithRepetitionGenerator _permutationGenerator;
		public OlympicClimbingChampionSolver()
		{
			this._permutationGenerator = new PermutationWithRepetitionGenerator();
		}

		public int FindMinimumWinningScore(int numContests, int numAthletes)
		{
			var maximumSuccessfulScore = 0;
			var maximumAttemptedScore = 0;
			var scoresForSingleAthlete = this.GenerateDistinctPossiblePlacementsForSingleAthlete(numContests, numAthletes);
			foreach(var outcome in scoresForSingleAthlete)
			{
				var score = outcome.Aggregate(1, (agg, v) => agg * v);
				var canWin = this.CanAthleteWinWithOutcome(numContests, numAthletes, outcome);
				if (canWin)
				{
					maximumSuccessfulScore = score;
				}
				maximumAttemptedScore = score;
			}
			return maximumSuccessfulScore;
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
				numContests, numAthletes, firstPlayerScores, challengerScores, targetScore, playerToIncrease, currentResult
			);
			if (targetChallengerScores == null) { return null; }
			int rowToIncrease = Enumerable.Range(1, numContests - 1)
				.Where(i => targetChallengerScores[i] != challengerScores[i]).Min();
			int colToIncrease = playerToIncrease;
			int targetVal = targetChallengerScores[rowToIncrease];
			while (true)
			{
				var availableValuesToSwap = 
					currentResult[rowToIncrease].Where((val, i) => i > colToIncrease && val >= targetVal).ToList();
				if (availableValuesToSwap.Any()) {
					targetVal = availableValuesToSwap.Min();
					break;
				}
				if (colToIncrease > 1) { colToIncrease--; }
				else { 
					rowToIncrease--; 
					colToIncrease = numAthletes - 1;
					if (rowToIncrease == 0) {
						return null;
					}
				}
				targetVal = currentResult[rowToIncrease][colToIncrease] + 1;
			}

			// now, we need to increase the index to the appropriate value, and fill everything else in
			var nextResult = Enumerable.Range(0, numContests).Select(i => new int[numAthletes]).ToList();
			for (int i = 0; i < numContests; i++) {
				for(int j=0; j<numAthletes; j++)
				{
					if (i < rowToIncrease || (i == rowToIncrease && j < colToIncrease) || j == 0)
					{
						nextResult[i][j] = currentResult[i][j];
					}
					else if (i == rowToIncrease && j == colToIncrease)
					{
						nextResult[i][j] = targetVal;
					}
				}
				var numbersToFill = Enumerable.Range(1, numAthletes).Except(nextResult[i].Distinct()).OrderBy(i => i).ToList();
				var numbersToFillIndex = 0;
				for(int j=0; j<numAthletes; j++)
				{
					if (nextResult[i][j] == 0)
					{
						nextResult[i][j] = numbersToFill[numbersToFillIndex++];
					}
				}
			}
			return nextResult;
		}

		private int[] FindStartingPointForNextResult(
			int numContests, int numAthletes, int[] firstPlayerScores, int[] challengerScores, int targetScore, int challengerId,
			List<int[]> currentResult
		)
		{
			var currentOutcome = challengerScores;
			while (true)
			{
				var nextOutcome = this._permutationGenerator.GenerateNextOutcome(currentOutcome, 1, numAthletes, doesOrderMatter: true);
				if (nextOutcome == null || nextOutcome[0] != challengerScores[0]){ return null; }
				var challengerScore = nextOutcome.Aggregate(1, (agg, o) => agg * o);
				var comesInSamePositionAsPlayer =
					Enumerable.Range(1, numContests-1).Where(i => firstPlayerScores[i] == nextOutcome[i]).Any();
				if(challengerScore >= targetScore && !comesInSamePositionAsPlayer) { 
					return nextOutcome; 
				}
				currentOutcome = nextOutcome;

			}
		}

		public List<int[]> GenerateDistinctPossiblePlacementsForSingleAthlete(int numContests, int numAthletes)
		{
			var outcomes = this._permutationGenerator
				.GenerateAllOutcomes(numContests, numAthletes, '1', true)
				.Select(o => new string(o.ToCharArray().OrderBy(e => e).ToArray()))
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
