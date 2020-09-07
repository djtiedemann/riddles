using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Riddles.RiddlerNationWar;
using System.IO;

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
			var castleTargeter = new CastleTargeter();
			var strategy = new TargetCastlesStrategy(numCastles, numTroops, castleTargeter);
			var castlesToTarget = castleTargeter.GetCastlesToTargetForPermuation(numCastles, seed);
			var troopPlacements = strategy.GenerateSingleTroopPlacement(
				castlesToTarget, 
				valueToAssignPerNonTargetedCastle,
				TroopAllocationStrategy.EvenDistribution,
				TroopAllocationStrategy.EvenDistribution);
			for(int i=0; i<troopPlacements.Count; i++)
			{
				Assert.AreEqual(expectedTroopPlacement[i], troopPlacements[i]);
			}
		}

		[TestCase(10, 100, false, 0)]
		[TestCase(10, 100, true, 0)]
		[TestCase(10, 100, false, 1)]
		[TestCase(10, 100, false, 2)]
		public void GenerateTrainingDataTargetAllCastlesEvenlyStrategy(int numCastles, int numTroops, bool restrictToStrategiesTargetingMajorityOfPoints,
			int valueToAssignPerNonTargetedCastle)
		{
			var castleTargeter = new CastleTargeter();
			var strategy = new TargetCastlesStrategy(numCastles, numTroops, castleTargeter);
			var trainingData = strategy.GenerateTroopPlacements(
				restrictToStrategiesTargetingMajorityOfPoints, 
				valueToAssignPerNonTargetedCastle,
				TroopAllocationStrategy.EvenDistribution,
				TroopAllocationStrategy.EvenDistribution
			);

			var warSimulator = new WarSimulator(numCastles, numTroops);
			var results = warSimulator.SimulateWars(trainingData).ToArray();
			var winningStrategyInTrainingSet = results[0];
			var secondBestStrategyInTrainingSet = results[1];
		}

		[TestCase(1023, 10, 100, new int[] { 2, 4, 5, 7, 9, 11, 13, 15, 16, 18})]
		[TestCase(859, 10, 100, new int[] { 3, 5, 0, 11, 13, 0, 18, 0, 24, 26 })]
		public void TestTargetCastlesProportionally(int seed, int numCastles, int numTroops, int[] expectedResults)
		{
			var castleTargeter = new CastleTargeter();
			var strategy = new TargetCastlesStrategy(numCastles, numTroops, castleTargeter);
			var castlesToTarget = castleTargeter.GetCastlesToTargetForPermuation(numCastles, seed);
			var actualTroopPlacements = strategy.GenerateSingleTroopPlacement(
				castlesToTarget,
				0,
				TroopAllocationStrategy.ProportionalDistribution,
				TroopAllocationStrategy.EvenDistribution).ToArray();

			Assert.AreEqual(expectedResults.Length, actualTroopPlacements.Length);
			for(int i=0; i<expectedResults.Length; i++)
			{
				Assert.AreEqual(expectedResults[i], actualTroopPlacements[i]);
			}
		}

		[TestCase(10, 100)]
		public void GenerateDataSetForOptimization(int numCastles, int numTroops)
		{
			var castleTargeter = new CastleTargeter();
			var strategyGenerator = new TargetCastlesStrategy(numCastles, numTroops, castleTargeter);
			var strategy1 = strategyGenerator.GenerateTroopPlacements(false, 0, TroopAllocationStrategy.EvenDistribution, TroopAllocationStrategy.EvenDistribution);
			var strategy2 = strategyGenerator.GenerateTroopPlacements(false, 1, TroopAllocationStrategy.EvenDistribution, TroopAllocationStrategy.EvenDistribution);
			var strategy3 = strategyGenerator.GenerateTroopPlacements(false, 2, TroopAllocationStrategy.EvenDistribution, TroopAllocationStrategy.EvenDistribution);
			var strategy4 = strategyGenerator.GenerateTroopPlacements(true, 0, TroopAllocationStrategy.EvenDistribution, TroopAllocationStrategy.EvenDistribution);
			var strategy5 = strategyGenerator.GenerateTroopPlacements(true, 1, TroopAllocationStrategy.EvenDistribution, TroopAllocationStrategy.EvenDistribution);
			var strategy6 = strategyGenerator.GenerateTroopPlacements(true, 2, TroopAllocationStrategy.EvenDistribution, TroopAllocationStrategy.EvenDistribution);
			var strategy7 = strategyGenerator.GenerateTroopPlacements(false, 0, TroopAllocationStrategy.ProportionalDistribution, TroopAllocationStrategy.EvenDistribution);
			var strategy8 = strategyGenerator.GenerateTroopPlacements(false, 1, TroopAllocationStrategy.ProportionalDistribution, TroopAllocationStrategy.EvenDistribution);
			var strategy9 = strategyGenerator.GenerateTroopPlacements(false, 2, TroopAllocationStrategy.ProportionalDistribution, TroopAllocationStrategy.EvenDistribution);
			var strategy10 = strategyGenerator.GenerateTroopPlacements(true, 0, TroopAllocationStrategy.ProportionalDistribution, TroopAllocationStrategy.EvenDistribution);
			var strategy11 = strategyGenerator.GenerateTroopPlacements(true, 1, TroopAllocationStrategy.ProportionalDistribution, TroopAllocationStrategy.EvenDistribution);
			var strategy12 = strategyGenerator.GenerateTroopPlacements(true, 2, TroopAllocationStrategy.ProportionalDistribution, TroopAllocationStrategy.EvenDistribution);

			var trainingData = new List<List<int>>();
			trainingData.AddRange(strategy1);
			trainingData.AddRange(strategy2);
			trainingData.AddRange(strategy3);
			trainingData.AddRange(strategy4);
			trainingData.AddRange(strategy5);
			trainingData.AddRange(strategy6);
			trainingData.AddRange(strategy7);
			trainingData.AddRange(strategy8);
			trainingData.AddRange(strategy9);
			trainingData.AddRange(strategy10);
			trainingData.AddRange(strategy11);
			trainingData.AddRange(strategy12);
			trainingData = trainingData.ToList();

			var warSimulator = new WarSimulator(numCastles, numTroops);
			var results = warSimulator.SimulateWars(trainingData);
		}

		// passed in parameters that would generate something similar to winner's strategy
		[TestCase(@"C:\Users\djt74_000\Projects\riddles\Riddles\Riddles\Riddles.Tests\RiddlerNationWar\Data\submissions-1.txt",
			new int[] { 3, 6, 9, 13, 16, 2, 22, 25, 2, 2 })]
		// came in first, naive version
		[TestCase(@"C:\Users\djt74_000\Projects\riddles\Riddles\Riddles\Riddles.Tests\RiddlerNationWar\Data\submissions-1.txt",
			new int[] { 0, 0, 0, 0, 18, 21, 0, 29, 32, 0 })]
		// came in second, naive version
		[TestCase(@"C:\Users\djt74_000\Projects\riddles\Riddles\Riddles\Riddles.Tests\RiddlerNationWar\Data\submissions-1.txt",
			new int[] { 1, 1, 1, 1, 17, 20, 1, 27, 30, 1 })]
		// came in third, naive version
		[TestCase(@"C:\Users\djt74_000\Projects\riddles\Riddles\Riddles\Riddles.Tests\RiddlerNationWar\Data\submissions-1.txt",
			new int[] { 1, 1, 1, 1, 17, 20, 23, 1, 1, 34 })]
		public void TestAgainstTrainingData(string filename, int[] testSubmission)
		{
			List<WarSimulator.SimulationEntry> testData = new List<WarSimulator.SimulationEntry>();
			using (StreamReader sr = new StreamReader(filename))
			{
				// ignore the header line
				var submission = sr.ReadLine();				
				while (sr.Peek() >= 0)
				{
					submission = sr.ReadLine();
					var troopDeployment = submission.Split(',');
					troopDeployment = troopDeployment.Where((t, index) => index < 10).ToArray();
					int n;
					if(troopDeployment.Any(t => !int.TryParse(t, out n))){
						continue;
					}
					var troopDeploymentSanitized = troopDeployment.Select(c => int.Parse(c)).ToList();
					if(troopDeploymentSanitized.Count != 10 || troopDeploymentSanitized.Sum() != 100)
					{
						continue;
					}
					testData.Add(new WarSimulator.SimulationEntry { 
						Id = null,
						IsTestSubmission = false,
						TroopAllocation = troopDeploymentSanitized
					});
				}
			}
			
			var castleTargeter = new CastleTargeter();
			var castlesToTarget = castleTargeter.GetCastlesToTargetForPermuation(10, 223);
			var strategyGenerator = new TargetCastlesStrategy(10, 100, castleTargeter);
			testData.Add(new WarSimulator.SimulationEntry { Id = 1, IsTestSubmission = true, TroopAllocation = testSubmission });
			var warSimulator = new WarSimulator(10, 100);
			var results = warSimulator.SimulateWars(testData);
			var resultsWithRank = results.Select((r, index) => new { Id = r.Id, TroopAllocation = r.Submission, NumWins = r.NumWins, Rank = index + 1 });
			var submissionResult = resultsWithRank.Single(r => r.Id == 1);
		}
	}
}
