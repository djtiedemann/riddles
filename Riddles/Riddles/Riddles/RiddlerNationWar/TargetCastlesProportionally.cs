using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Riddles.RiddlerNationWar
{
	public class TargetCastlesProportionally
	{
		private int _numCastles;
		private int _numTroops;
		public TargetCastlesProportionally(int numCastles, int numTroops)
		{
			this._numCastles = numCastles;
			this._numTroops = numTroops;
		}

		public List<int> GetProportionalTroopPlacement()
		{
			return this.GetProportionalTroopPlacement(Enumerable.Range(1, this._numCastles).ToList());
		}

		public List<int> GetProportionalTroopPlacement(List<int> castlePlacements) {
			var numPointsAvailable = castlePlacements.Sum();
			var troopPlacements = castlePlacements.Select(castleNum => (int)castleNum * this._numTroops / numPointsAvailable).ToList();
			var remainders = castlePlacements.Select((castleNum, index) => 
				(double)castleNum * (double)this._numTroops / (double)numPointsAvailable - troopPlacements[index])
				.ToList();
			var numTroopsPlaced = troopPlacements.Sum();
			var numTroopsRemaining = this._numTroops - numTroopsPlaced;
			var orderedRemainders = remainders.Select((remainder, index) => new { Remainder = remainder, Index = index });
			for (int i=0; i<numTroopsRemaining; i++)
			{
				orderedRemainders = orderedRemainders.OrderByDescending(r => r.Remainder).ToList();
				// find the highest remainder, and allocate a troop. Then set that remainder to 0, moving it to the back of the line
				// since we've already allocated the base number of troops not all castles will be assigned an additional troop in this stage

				troopPlacements[orderedRemainders.First().Index]++;
				orderedRemainders = orderedRemainders.Select((r, index) => index == 0 ? new { Remainder = 0.0, Index = r.Index } : r).ToList();
			}
			return troopPlacements;
		}
	}
}
