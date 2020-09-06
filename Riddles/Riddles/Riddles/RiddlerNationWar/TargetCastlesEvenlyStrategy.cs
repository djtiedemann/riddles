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
		private int _numPointsAvailable;

		// the idea behind this strategy is to pick a subset of castles to target and to place all of your troops attacking those castles
		public TargetCastlesEvenlyStrategy(int numCastles, int numTroops)
		{
			this._numCastles = numCastles;
			this._numTroops = numTroops;
			this._numPointsAvailable = Enumerable.Range(1, numCastles).Sum();
		}

		public List<List<int>> GenerateTroopPlacements(bool restrictToStrategiesTargetingMajorityOfPoints, int valueToAssignPerNonTargetedCastle)
		{
			var troopPlacements = new List<List<int>>();
			var numberOfCombinations = Math.Pow(2, this._numCastles);
			// note: i = 0 means you target no castles
			for(int i=1; i<numberOfCombinations; i++)
			{
				var troopPlacement = this.GenerateSingleTroopPlacement(i, valueToAssignPerNonTargetedCastle);
				if (restrictToStrategiesTargetingMajorityOfPoints)
				{
					var numPointsTargeted = this.NumPointsTargeted(i);
					if(numPointsTargeted >= Math.Ceiling((this._numPointsAvailable + 1) / 2.0))
					{
						troopPlacements.Add(troopPlacement);
					}
					continue;
				}
				troopPlacements.Add(troopPlacement);
			} 
			return troopPlacements;
		}

		public List<int> GenerateSingleTroopPlacement(int seed, int valueToAssignPerNonTargetedCastle)
		{
			string binaryString = Convert.ToString(seed, 2);
			var castlePlacements = binaryString.PadLeft(this._numCastles, '0').ToCharArray().Select(a => a - '0');
			var numCastlesTargeted = castlePlacements.Where(c => c == 1).ToList().Count;
			var numCastlesNotTargeted = this._numCastles - numCastlesTargeted;
			var numTroopsToAllocateImmediatelyPerTargetedCastle 
				= (this._numTroops - valueToAssignPerNonTargetedCastle * numCastlesNotTargeted) / numCastlesTargeted;
			var leftoverTroops = (this._numTroops - valueToAssignPerNonTargetedCastle * numCastlesNotTargeted) % numCastlesTargeted;

			var troopPlacement = castlePlacements.Select(c => c == 1 
				? numTroopsToAllocateImmediatelyPerTargetedCastle 
				: valueToAssignPerNonTargetedCastle
			).ToArray();
			for (int i=0; i<this._numCastles; i++)
			{
				if(troopPlacement[i] > 0 && leftoverTroops > 0)
				{
					troopPlacement[i]++;
					leftoverTroops--;
				}
			}
			var troopPlacementList = troopPlacement.ToList();
			troopPlacementList.Reverse();
			return troopPlacementList;
		}

		public int NumPointsTargeted(int seed)
		{
			string binaryString = Convert.ToString(seed, 2);
			var castlePlacements = binaryString.PadLeft(this._numCastles, '0').ToCharArray().Select(a => a - '0').ToList();
			castlePlacements.Reverse();
			castlePlacements = castlePlacements.ToList();
			var pointsTargeted = castlePlacements.Select((isTargeted, castleNum) => isTargeted == 1 ? castleNum + 1 : 0).Sum();
			return pointsTargeted;
		}
	}
}
