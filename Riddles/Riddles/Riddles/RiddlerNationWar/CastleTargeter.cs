using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Riddles.RiddlerNationWar
{
	public class CastleTargeter
	{
		public CastleTargeter()
		{

		}

		public List<CastlesTargetedInfo> GetPossibleTargetingStrategies(int numCastles, bool onlyConsiderOptionsWhichTargetMajorityOfPoints){
			var numberOfCombinations = Math.Pow(2, numCastles);
			List<CastlesTargetedInfo> strategies = new List<CastlesTargetedInfo>();
			// note: i = 0 means you target no castles
			for(int i=1; i<numberOfCombinations; i++)
			{
				var castlesTargetedInfo = GetCastlesToTargetForPermuation(numCastles, i);

				var numPointsTargeted = castlesTargetedInfo.CastlesTargeted.Sum();
				var numPointsAvailable = Enumerable.Range(1, numCastles).Sum();
				if (numPointsTargeted >= Math.Ceiling((numPointsAvailable + 1) / 2.0))
				{
					strategies.Add(castlesTargetedInfo);
				}
			}
			return strategies;
		}

		public CastlesTargetedInfo GetCastlesToTargetForPermuation(int numCastles, int seed)
		{
			string binaryString = Convert.ToString(seed, 2);
			var castlePlacements = binaryString.PadLeft(numCastles, '0').ToCharArray().Select(a => a - '0').ToList();
			castlePlacements.Reverse();
			var castlesTargeted = castlePlacements.Select((isTargeted, index) => new { CastleNum = index + 1, IsTargeted = isTargeted })
				.Where(c => c.IsTargeted == 1)
				.Select(c => c.CastleNum)
				.ToList();
			var castlesNotTargeted = castlePlacements.Select((isTargeted, index) => new { CastleNum = index + 1, IsTargeted = isTargeted })
				.Where(c => c.IsTargeted == 0)
				.Select(c => c.CastleNum)
				.ToList();
			return new CastlesTargetedInfo
			{
				CastlesTargeted = castlesTargeted,
				CastlesNotTargeted = castlesNotTargeted
			};
		}

		public class CastlesTargetedInfo
		{
			public List<int> CastlesTargeted { get; set; }
			public List<int> CastlesNotTargeted { get; set; }
		}
	}
}
