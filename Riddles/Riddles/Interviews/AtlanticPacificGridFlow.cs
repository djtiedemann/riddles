using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Interviews
{
	public class AtlanticPacificGridFlow
	{
		public List<Location> SolveAtlanticPacificFlow(int[,] gridFlow) {
			WaterFlow?[,] reachableLocations = new WaterFlow?[gridFlow.GetLength(0), gridFlow.GetLength(1)];
			List<Location> cellsThatCanReachBothOceans = new List<Location>();
			HashSet<Location> locationsSeen = new HashSet<Location>();
			for(int row=0; row < gridFlow.GetLength(0); row++)
			{
				for(int col=0; col<gridFlow.GetLength(1); col++)
				{
					this.SolveAtlanticPacificFlowInternal(
						gridFlow, reachableLocations, cellsThatCanReachBothOceans, new Location(row, col), locationsSeen
					);
				}
			}
			return cellsThatCanReachBothOceans;
		}

		private void SolveAtlanticPacificFlowInternal(
			int[,] gridFlow, WaterFlow?[,] reachableLocations, List<Location> cellsThatCanReachBothOceans, Location currentLocation, 
			HashSet<Location> locationsSeen)
		{
			if (reachableLocations[currentLocation.Row, currentLocation.Col] != null) { return; }
			if (locationsSeen.Contains(currentLocation)){ return; }
			locationsSeen.Add(currentLocation);

			// anything that's on the border of the pacific and atlantic can reach both
			if ((currentLocation.Row == 0 && currentLocation.Col == gridFlow.GetLength(1)) 
				|| (currentLocation.Col == 0 && currentLocation.Row == gridFlow.GetLength(0))) {
				reachableLocations[currentLocation.Row, currentLocation.Col] = WaterFlow.Both;
				cellsThatCanReachBothOceans.Add(currentLocation);
				return;
			}
			bool canReachAtlantic = currentLocation.Row == gridFlow.GetLength(0) || currentLocation.Col == gridFlow.GetLength(1);
			bool canReachPacific = currentLocation.Row == 0 || currentLocation.Col == 0;

			var locationsWhereWaterCanFlow = this.GetLocationsWhereWaterCanFlow(currentLocation, gridFlow, locationsSeen);
			foreach (var adjacentLocation in locationsWhereWaterCanFlow) {
				this.SolveAtlanticPacificFlowInternal(gridFlow, reachableLocations, cellsThatCanReachBothOceans, adjacentLocation, locationsSeen);
				var waterFlowTypeAdjacent = reachableLocations[adjacentLocation.Row, adjacentLocation.Col];
				if (waterFlowTypeAdjacent == WaterFlow.Atlantic || waterFlowTypeAdjacent == WaterFlow.Both) { canReachAtlantic = true; }
				if (waterFlowTypeAdjacent == WaterFlow.Pacific || waterFlowTypeAdjacent == WaterFlow.Both) { canReachPacific = true; }
			}
			var waterFlowType = canReachAtlantic && canReachPacific
				? WaterFlow.Both
				: canReachAtlantic
					? WaterFlow.Atlantic
					: canReachPacific
						? WaterFlow.Pacific
						: WaterFlow.None;
			if (waterFlowType == WaterFlow.Both) { cellsThatCanReachBothOceans.Add(currentLocation); }
			reachableLocations[currentLocation.Row, currentLocation.Col] = waterFlowType;			
		}

		private List<Location> GetLocationsWhereWaterCanFlow(Location currentLocation, int[,] gridFlow, HashSet<Location> locationsSeen)
		{
			var adjacentLocations = new List<Location> {
				new Location (currentLocation.Row - 1, currentLocation.Col ),
				new Location (currentLocation.Row, currentLocation.Col - 1 ),
				new Location (currentLocation.Row + 1, currentLocation.Col),
				new Location (currentLocation.Row, currentLocation.Col + 1),
			};

			var validAdjacentLocations = adjacentLocations.Where(l => l.Row >= 0 && l.Row < gridFlow.GetLength(0)
				&& l.Col >= 0 && l.Col < gridFlow.GetLength(1) && !locationsSeen.Contains(l));
			return validAdjacentLocations.Where(l => gridFlow[currentLocation.Row, currentLocation.Col] >= gridFlow[l.Row, l.Col]).ToList();
		}

		public enum WaterFlow {
			Unknown = -1,
			None = 0,
			Atlantic = 1,
			Pacific = 2,
			Both = 3
		}

		public class Location : Tuple<int, int> {
			public Location(int item1, int item2) : base(item1, item2) {
				
			}
			public int Row { get { return base.Item1;  } }
			public int Col { get { return base.Item2; } }
		}
	}
}
