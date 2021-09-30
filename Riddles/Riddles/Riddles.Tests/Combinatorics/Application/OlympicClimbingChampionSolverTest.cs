using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Riddles.Combinatorics.Core.Domain;
using Riddles.Combinatorics.Application;
using NUnit.Framework;

namespace Riddles.Tests.Combinatorics.Application
{
	public class OlympicClimbingChampionSolverTest
	{
		[TestCase]
		public void TestCanAthleteWinWithResult()
		{
			var olympicClimbingChampionSolver = new OlympicClimbingChampionSolver();
			var outcome = new int[] { 2, 5 };
			//var canWin = olympicClimbingChampionSolver.CanAthleteWinWithOutcome(2, 8, outcome);
		}

		[TestCase]
		public void TestGenerateNextOutcome()
		{

		}

		private Dictionary<int, List<int[]>> permutationTestCaseDictionary = new Dictionary<int, List<int[]>>
		{
			{ 1, new List<int[]>{
				new int[]{ 3, 4, 1, 2 },
				new int[]{ 4, 1, 2, 3 },
				new int[]{ 1, 2, 3, 4 },
			} },
			{ 2, new List<int[]>{
				new int[]{ 4, 3, 2, 1 },
				new int[]{ 1, 4, 3, 2 },
				new int[]{ 4, 2, 1, 3 },
			} },
			{ 3, new List<int[]>{
				new int[]{ 2, 1, 4, 3 },
				new int[]{ 3, 2, 1, 4 },
				new int[]{ 1, 3, 4, 2 },
			} },
			{ 4, new List<int[]>{
				new int[]{ 2, 1, 4, 3 },
				new int[]{ 3, 1, 2, 4 },
				new int[]{ 1, 3, 4, 2 },
			} }
		};

		private Dictionary<int, List<OlympicClimbingChampionSolver.PlayerScore>> winningScoresDictionary =
			new Dictionary<int, List<OlympicClimbingChampionSolver.PlayerScore>>
			{
				{1, new List<OlympicClimbingChampionSolver.PlayerScore> { 
					new OlympicClimbingChampionSolver.PlayerScore(playerId: 0, score: 12),
					new OlympicClimbingChampionSolver.PlayerScore(playerId: 1, score: 8),
					new OlympicClimbingChampionSolver.PlayerScore(playerId: 2, score: 6),
					new OlympicClimbingChampionSolver.PlayerScore(playerId: 3, score: 24),
				} },
				{2, new List<OlympicClimbingChampionSolver.PlayerScore> {
					new OlympicClimbingChampionSolver.PlayerScore(playerId: 0, score: 16),
					new OlympicClimbingChampionSolver.PlayerScore(playerId: 1, score: 24),
					new OlympicClimbingChampionSolver.PlayerScore(playerId: 2, score: 6),
					new OlympicClimbingChampionSolver.PlayerScore(playerId: 3, score: 6),
				} },
				{3, new List<OlympicClimbingChampionSolver.PlayerScore> {
					new OlympicClimbingChampionSolver.PlayerScore(playerId: 0, score: 6),
					new OlympicClimbingChampionSolver.PlayerScore(playerId: 1, score: 6),
					new OlympicClimbingChampionSolver.PlayerScore(playerId: 2, score: 16),
					new OlympicClimbingChampionSolver.PlayerScore(playerId: 3, score: 24),
				} },
				{4, new List<OlympicClimbingChampionSolver.PlayerScore> {
					new OlympicClimbingChampionSolver.PlayerScore(playerId: 0, score: 6),
					new OlympicClimbingChampionSolver.PlayerScore(playerId: 1, score: 3),
					new OlympicClimbingChampionSolver.PlayerScore(playerId: 2, score: 32),
					new OlympicClimbingChampionSolver.PlayerScore(playerId: 3, score: 24),
				} },
			};

		[TestCase(1, new int[] { 2 }, 6)]
		[TestCase(2, new int[] { 2, 3 }, 6)]
		[TestCase(3, new int[] { 0, 1 }, 6)]
		[TestCase(4, new int[] { 1 }, 3)]
		public void TestFindWinningScore(int testCaseId, int[] expectedWinningPlayers, int expectedWinningScore)
		{
			var olympicClimbingChampionSolver = new OlympicClimbingChampionSolver();
			var scores = olympicClimbingChampionSolver.FindScoresForContest(
				permutationTestCaseDictionary[testCaseId]
			).OrderBy(s => s.PlayerId).ToList();
			var expectedScores = this.winningScoresDictionary[testCaseId];
			Assert.AreEqual(expectedScores.Count, scores.Count);
			for(int i=0; i<scores.Count; i++)
			{
				Assert.AreEqual(expectedScores[i].PlayerId, scores[i].PlayerId);
				Assert.AreEqual(expectedScores[i].Score, scores[i].Score);
			}
		}

		private Dictionary<int, List<int[]>> expectedOutcomesForWinningAthlete = new Dictionary<int, List<int[]>> {
			{ 1, new List<int[]> { 
				new int[] { 1, 1, 1 },
				new int[] { 1, 1, 2 },
				new int[] { 1, 1, 3 },
				new int[] { 1, 2, 2 },
				new int[] { 1, 2, 3 },
				new int[] { 2, 2, 2 },
				new int[] { 1, 3, 3 },
				new int[] { 2, 2, 3 },
				new int[] { 2, 3, 3 },
				new int[] { 3, 3, 3 },
			}},
			{ 2, new List<int[]> {
				new int[] { 1, 1 }, new int[] { 1, 2 }, new int[] { 1, 3 }, new int[] { 1, 4 }, new int[] { 2, 2 },
				new int[] { 1, 5 }, new int[] { 1, 6 }, new int[] { 2, 3 }, new int[] { 1, 7 }, new int[] { 1, 8 },
				new int[] { 2, 4 }, new int[] { 3, 3 }, new int[] { 2, 5 }, new int[] { 2, 6 }, new int[] { 3, 4 },
				new int[] { 2, 7 }, new int[] { 3, 5 }, new int[] { 2, 8 }, new int[] { 4, 4 }, new int[] { 3, 6 },
				new int[] { 4, 5 }, new int[] { 3, 7 }, new int[] { 3, 8 }, new int[] { 4, 6 }, new int[] { 5, 5 },
				new int[] { 4, 7 }, new int[] { 5, 6 }, new int[] { 4, 8 }, new int[] { 5, 7 }, new int[] { 6, 6 },
				new int[] { 5, 8 }, new int[] { 6, 7 }, new int[] { 6, 8 }, new int[] { 7, 7 }, new int[] { 7, 8 },
				new int[] { 8, 8 },
			}},
		};

		[TestCase(1, 3, 3)]
		[TestCase(2, 2, 8)]
		public void TestGenerateDistinctPossiblePlacementsForSingleAthlete(int testCaseId, int numContests, int numAthletes) {
			var olympicClimbingChampionSolver = new OlympicClimbingChampionSolver();
			var outcomes = olympicClimbingChampionSolver.GenerateDistinctPossiblePlacementsForSingleAthlete(numContests, numAthletes);
			var expectedOutcomes = expectedOutcomesForWinningAthlete[testCaseId];
			Assert.AreEqual(expectedOutcomes.Count, outcomes.Count);
			for(int i=0; i<outcomes.Count; i++)
			{
				Assert.AreEqual(expectedOutcomes[i], outcomes[i]);
			}
		}
	}
}
