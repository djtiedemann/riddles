using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Riddles.RiddlerNationWar
{
	public enum TroopAllocationStrategy
	{
		// in this case, troops will be evenly distributed across castles
		EvenDistribution = 1,
		// in this case, troops will be distributed across castles based on their point values 
		// (a 10 point castle will get roughly 10x troops as a 1 point one)
		ProportionalDistribution = 2
	};

	public class TargetCastlesStrategy
	{
		private int _numCastles;
		private int _numTroops;
		private CastleTargeter _castleTargeter;

		// the idea behind this strategy is to pick castles that you want to try and win and castles you don't plan to win, and spread your troops accordingly
		public TargetCastlesStrategy(int numCastles, int numTroops, CastleTargeter castleTargeter)
		{
			this._numCastles = numCastles;
			this._numTroops = numTroops;
			this._castleTargeter = castleTargeter;
		}

		public List<List<int>> GenerateTroopPlacements(
			bool restrictToStrategiesTargetingMajorityOfPoints, 
			int valueToAssignPerNonTargetedCastle,
			TroopAllocationStrategy allocationStrategyForTargetedCastles,
			TroopAllocationStrategy allocationStrategyForNonTargetedCastles)
		{
			var subsetsOfCastlesToTarget = this._castleTargeter.GetPossibleTargetingStrategies(this._numCastles, restrictToStrategiesTargetingMajorityOfPoints);

			var troopPlacements = new List<List<int>>();
			foreach (var castlesToTarget in subsetsOfCastlesToTarget)
			{
				var troopPlacement = this.GenerateSingleTroopPlacement(
					castlesToTarget, 
					valueToAssignPerNonTargetedCastle, 
					allocationStrategyForTargetedCastles,
					allocationStrategyForNonTargetedCastles);
				troopPlacements.Add(troopPlacement);
			}
			return troopPlacements;
		}

		public List<int> GenerateSingleTroopPlacement(
			CastleTargeter.CastlesTargetedInfo castleTargetingInfo, 
			int valueToAssignPerNonTargetedCastle,
			TroopAllocationStrategy allocationStrategyForTargetedCastles,
			TroopAllocationStrategy allocationStrategyForNonTargetedCastles)
		{
			var numCastlesTargeted = castleTargetingInfo.CastlesTargeted.Count;
			var numCastlesNotTargeted = castleTargetingInfo.CastlesNotTargeted.Count;
			var numTroopsToAllocateForNonTargetedCastles = valueToAssignPerNonTargetedCastle * numCastlesNotTargeted;
			var numTroopsToAllocateForTargetedCastles = this._numTroops - numTroopsToAllocateForNonTargetedCastles;

			var troopPlacementForTargetedCastles = new List<CastleTroopPlacement>();
			var troopPlacementForNonTargetedCastles = new List<CastleTroopPlacement>();

			switch (allocationStrategyForTargetedCastles)
			{
				case TroopAllocationStrategy.EvenDistribution:
					troopPlacementForTargetedCastles = this.AllocateTroopsToCastlesEvenly(
						castleTargetingInfo.CastlesTargeted,
						numTroopsToAllocateForTargetedCastles
					);
					break;
				case TroopAllocationStrategy.ProportionalDistribution:
					troopPlacementForTargetedCastles = this.AllocateTroopsToCastlesProportionally(
						castleTargetingInfo.CastlesTargeted,
						numTroopsToAllocateForTargetedCastles
					);
					break;
			}

			switch (allocationStrategyForNonTargetedCastles)
			{
				case TroopAllocationStrategy.EvenDistribution:
					troopPlacementForNonTargetedCastles = this.AllocateTroopsToCastlesEvenly(
						castleTargetingInfo.CastlesNotTargeted,
						numTroopsToAllocateForNonTargetedCastles
					);
					break;
				case TroopAllocationStrategy.ProportionalDistribution:
					troopPlacementForNonTargetedCastles = this.AllocateTroopsToCastlesProportionally(
						castleTargetingInfo.CastlesNotTargeted,
						numTroopsToAllocateForNonTargetedCastles
					);
					break;
			}
			var troopPlacementList = troopPlacementForTargetedCastles.ToList().Concat(troopPlacementForNonTargetedCastles.ToList());
			return troopPlacementList.OrderBy(t => t.CastleNum).Select(t => t.NumTroops).ToList();
		}

		public List<CastleTroopPlacement> AllocateTroopsToCastlesEvenly(
			List<int> castles, 
			int numTroopsToAllocate)
		{
			if(castles.Count == 0)
			{
				return new List<CastleTroopPlacement>();
			}
			var numTroopsToAllocateImmediately = numTroopsToAllocate / castles.Count;
			var numTroopsLeftover = numTroopsToAllocate % castles.Count;

			var troopPlacement = castles
				.Select(c => new CastleTroopPlacement { CastleNum = c, NumTroops = numTroopsToAllocateImmediately }).ToArray();

			for (int i = troopPlacement.Length - 1; i > troopPlacement.Length - 1 - numTroopsLeftover; i--)
			{
				troopPlacement[i].NumTroops++;
			}
			return troopPlacement.ToList();
		}

		public List<CastleTroopPlacement> AllocateTroopsToCastlesProportionally(List<int> castles, int numTroopsToAllocate)
		{
			var numPointsAvailable = castles.Sum();
			var troopPlacements = castles.Select(castleNum => new CastleTroopPlacement { 
				CastleNum = castleNum,
				NumTroops = (int)castleNum * numTroopsToAllocate / numPointsAvailable,
			}).ToList();
			var remainders = castles.Select((castleNum, index) =>
				(double)castleNum * (double)numTroopsToAllocate / (double)numPointsAvailable - troopPlacements[index].NumTroops)
				.ToList();
			var numTroopsPlaced = troopPlacements.Sum(t => t.NumTroops);
			var numTroopsRemaining = numTroopsToAllocate - numTroopsPlaced;
			var orderedRemainders = remainders.Select((remainder, index) => new { Remainder = remainder, Index = index });
			for (int i = 0; i < numTroopsRemaining; i++)
			{
				orderedRemainders = orderedRemainders.OrderByDescending(r => r.Remainder).ToList();
				// find the highest remainder, and allocate a troop. Then set that remainder to 0, moving it to the back of the line
				// since we've already allocated the base number of troops not all castles will be assigned an additional troop in this stage

				troopPlacements[orderedRemainders.First().Index].NumTroops++;
				orderedRemainders = orderedRemainders.Select((r, index) => index == 0 ? new { Remainder = 0.0, Index = r.Index } : r).ToList();
			}
			return troopPlacements;
		}

		public class CastleTroopPlacement
		{
			public int CastleNum { get; set; }
			public int NumTroops { get; set; }
		}
	}
}
