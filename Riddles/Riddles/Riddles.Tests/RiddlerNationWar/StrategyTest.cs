using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Riddles.RiddlerNationWar;

namespace Riddles.Tests.RiddlerNationWar
{
	public class StrategyTest
	{
		// only assigning points to targeted castles
		[TestCase(1, new int[] {100, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, 1, 10, 100, 0)]
		[TestCase(560, new int[] { 0, 0, 0, 0, 33, 33, 0, 0, 0, 34 }, 21, 10, 100, 0)]
		[TestCase(452, new int[] { 0, 0, 25, 0, 0, 0, 25, 25, 25, 0}, 27, 10, 100, 0)]
		[TestCase(1023, new int[] { 10, 10, 10, 10, 10, 10, 10, 10, 10, 10 }, 55, 10, 100, 0)]
		// assigning points to non-targeted castles
		[TestCase(560, new int[] { 1, 1, 1, 1, 31, 31, 1, 1, 1, 31 }, 21, 10, 100, 1)]
		public void TestTargetCastlesEvenlyStrategy(int seed, int[] expectedTroopPlacement, int expectedNumPointsTargeted, int numCastles, int numTroops,
			int valueToAssignPerNonTargetedCastle)
		{
			var strategy = new TargetCastlesEvenlyStrategy(10, 100);
			var troopPlacements = strategy.GenerateSingleTroopPlacement(seed, valueToAssignPerNonTargetedCastle);
			for(int i=0; i<troopPlacements.Count; i++)
			{
				Assert.AreEqual(expectedTroopPlacement[i], troopPlacements[i]);
			}
			var numPointsTargeted = strategy.NumPointsTargeted(seed);
			Assert.AreEqual(expectedNumPointsTargeted, numPointsTargeted);
		}

		[TestCase(10, 100, false, 0)]
		[TestCase(10, 100, true, 0)]
		[TestCase(10, 100, false, 1)]
		[TestCase(10, 100, false, 2)]
		public void GenerateTrainingDataTargetAllCastlesEvenlyStrategy(int numCastles, int numTroops, bool restrictToStrategiesTargetingMajorityOfPoints,
			int valueToAssignPerNonTargetedCastle)
		{
			var strategy = new TargetCastlesEvenlyStrategy(numCastles, numTroops);
			var trainingData = strategy.GenerateTroopPlacements(restrictToStrategiesTargetingMajorityOfPoints, valueToAssignPerNonTargetedCastle);

			var warSimulator = new WarSimulator(numCastles, numTroops);
			var results = warSimulator.SimulateWars(trainingData).ToArray();
			var winningStrategyInTrainingSet = results[0];
			var secondBestStrategyInTrainingSet = results[1];
		}

		[TestCase(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10}, 10, 100, new int[] { 2, 4, 5, 7, 9, 11, 13, 15, 16, 18})]
		[TestCase(new int[] { 1, 2, 4, 5, 7, 9, 10 }, 10, 100, new int[] { 3, 5, 11, 13, 18, 24, 26 })]
		public void TestTargetCastlesProportionally(int[] castlesTargeted, int numCastles, int numTroops, int[] expectedResults)
		{
			var strategy = new TargetCastlesProportionally(10, 100);
			var actualTroopPlacements = strategy.GetProportionalTroopPlacement(castlesTargeted.ToList()).ToArray();
			Assert.AreEqual(expectedResults.Length, actualTroopPlacements.Length);
			for(int i=0; i<expectedResults.Length; i++)
			{
				Assert.AreEqual(expectedResults[i], actualTroopPlacements[i]);
			}
		}
	}
}
