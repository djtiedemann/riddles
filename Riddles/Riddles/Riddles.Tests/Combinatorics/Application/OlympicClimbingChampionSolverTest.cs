using System;
using System.Collections.Generic;
using System.Text;
using Riddles.Combinatorics.Core.Domain;
using Riddles.Combinatorics.Application;
using NUnit.Framework;

namespace Riddles.Tests.Combinatorics.Application
{
	public class OlympicClimbingChampionSolverTest
	{
		private Dictionary<int, Permutation[]> permutationTestCaseDictionary = new Dictionary<int, Permutation[]>
		{
			{ 1, new Permutation[]{
				new Permutation(new int[]{ 3, 4, 1, 2 }),
				new Permutation(new int[]{ 4, 1, 2, 3 }),
				new Permutation(new int[]{ 1, 2, 3, 4 }),
			} },
			{ 2, new Permutation[]{
				new Permutation(new int[]{ 4, 3, 2, 1 }),
				new Permutation(new int[]{ 1, 4, 3, 2 }),
				new Permutation(new int[]{ 4, 2, 1, 3 }),
			} },
			{ 3, new Permutation[]{
				new Permutation(new int[]{ 2, 1, 4, 3 }),
				new Permutation(new int[]{ 3, 2, 1, 4 }),
				new Permutation(new int[]{ 1, 3, 4, 2 }),
			} },
			{ 4, new Permutation[]{
				new Permutation(new int[]{ 2, 1, 4, 3 }),
				new Permutation(new int[]{ 3, 1, 2, 4 }),
				new Permutation(new int[]{ 1, 3, 4, 2 }),
			} }
		};

		[TestCase(1, new int[] { 2 }, 6)]
		[TestCase(2, new int[] { 2, 3 }, 6)]
		[TestCase(3, new int[] { 0, 1 }, 6)]
		[TestCase(4, new int[] { 1 }, 3)]
		public void TestFindWinningScore(int testCaseId, int[] expectedWinningPlayers, int expectedWinningScore)
		{
			var olympicClimbingChampionSolver = new OlympicClimbingChampionSolver();
			var winningScore = olympicClimbingChampionSolver.FindWinningScore(permutationTestCaseDictionary[testCaseId]);
			var winningPlayers = winningScore.WinningPlayerIds.ToArray();
			Assert.AreEqual(expectedWinningScore, winningScore.Score);
			Assert.AreEqual(expectedWinningPlayers.Length, winningPlayers.Length);

			for(int i=0; i<expectedWinningPlayers.Length; i++)
			{
				Assert.AreEqual(expectedWinningPlayers[i], winningPlayers[i]);
			}
		}
	}
}
