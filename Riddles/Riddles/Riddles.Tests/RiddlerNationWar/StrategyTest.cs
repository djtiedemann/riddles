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
		[TestCase(1, new int[] {100, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, 1, 10, 100)]
		[TestCase(560, new int[] { 0, 0, 0, 0, 33, 33, 0, 0, 0, 34 }, 21, 10, 100)]
		[TestCase(452, new int[] { 0, 0, 25, 0, 0, 0, 25, 25, 25, 0}, 27, 10, 100)]
		[TestCase(1023, new int[] { 10, 10, 10, 10, 10, 10, 10, 10, 10, 10 }, 55, 10, 100)]
		public void TestTargetCastlesEvenlyStrategy(int seed, int[] expectedTroopPlacement, int expectedNumPointsTargeted, int numCastles, int numTroops)
		{
			var strategy = new TargetCastlesEvenlyStrategy(10, 100);
			var troopPlacements = strategy.GenerateSingleTroopPlacement(seed);
			for(int i=0; i<troopPlacements.Count; i++)
			{
				Assert.AreEqual(expectedTroopPlacement[i], troopPlacements[i]);
			}
			var numPointsTargeted = strategy.NumPointsTargeted(seed);
			Assert.AreEqual(expectedNumPointsTargeted, numPointsTargeted);
		}

		//[TestCase(10, 100, false)]
		[TestCase(10, 100, true)]
		public void GenerateTrainingDataTargetAllCastlesEvenlyStrategy(int numCastles, int numTroops, bool restrictToStrategiesTargetingMajorityOfPoints)
		{
			var strategy = new TargetCastlesEvenlyStrategy(numCastles, numTroops);
			var trainingData = strategy.GenerateTroopPlacements(restrictToStrategiesTargetingMajorityOfPoints);

			var warSimulator = new WarSimulator(numCastles, numTroops);
			var results = warSimulator.SimulateWars(trainingData).ToArray();
			var winningStrategyInTrainingSet = results[0];
			var secondBestStrategyInTrainingSet = results[1];
		}
	}
}
