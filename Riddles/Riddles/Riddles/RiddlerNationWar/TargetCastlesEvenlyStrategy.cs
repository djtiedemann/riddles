using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Riddles.RiddlerNationWar
{
	public class TargetCastlesEvenlyStrategy
	{
		private int _numCastles;
		private int _numTroops;
		private CastleTargeter _castleTargeter;

		// the idea behind this strategy is to pick a subset of castles to target and to place all of your troops attacking those castles
		public TargetCastlesEvenlyStrategy(int numCastles, int numTroops, CastleTargeter castleTargeter)
		{
			this._numCastles = numCastles;
			this._numTroops = numTroops;
			this._castleTargeter = castleTargeter;
		}

		public List<List<int>> GenerateTroopPlacements(bool restrictToStrategiesTargetingMajorityOfPoints, int valueToAssignPerNonTargetedCastle)
		{
			var subsetsOfCastlesToTarget = this._castleTargeter.GetPossibleTargetingStrategies(this._numCastles, restrictToStrategiesTargetingMajorityOfPoints);

			var troopPlacements = new List<List<int>>();
			foreach (var castlesToTarget in subsetsOfCastlesToTarget)
			{
				var troopPlacement = this.GenerateSingleTroopPlacement(castlesToTarget, valueToAssignPerNonTargetedCastle);
				troopPlacements.Add(troopPlacement);
			}
			return troopPlacements;
		}

		public List<int> GenerateSingleTroopPlacement(CastleTargeter.CastlesTargetedInfo castleTargetingInfo, int valueToAssignPerNonTargetedCastle)
		{
			var numCastlesTargeted = castleTargetingInfo.CastlesTargeted.Count;
			var numCastlesNotTargeted = castleTargetingInfo.CastlesNotTargeted.Count;
			var numTroopsToAllocateForNonTargetedCastles = valueToAssignPerNonTargetedCastle * numCastlesNotTargeted;
			var numTroopsToAllocateForTargetedCastles = this._numTroops - numTroopsToAllocateForNonTargetedCastles;

			var troopPlacementForTargetedCastles = this.AllocateTroopsToCastlesEvenly(
				castleTargetingInfo.CastlesTargeted, 
				numTroopsToAllocateForTargetedCastles
			);
			var troopPlacementForNonTargetedCastles = this.AllocateTroopsToCastlesEvenly(
				castleTargetingInfo.CastlesNotTargeted, 
				numTroopsToAllocateForNonTargetedCastles
			);

			var troopPlacementList = troopPlacementForTargetedCastles.ToList().Concat(troopPlacementForNonTargetedCastles.ToList());
			return troopPlacementList.OrderBy(t => t.CastleNum).Select(t => t.NumTroops).ToList();
		}

		public List<CastleTroopPlacement> AllocateTroopsToCastlesEvenly(List<int> castles, int numTroopsToAllocate)
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

		public class CastleTroopPlacement
		{
			public int CastleNum { get; set; }
			public int NumTroops { get; set; }
		}
	}
}
