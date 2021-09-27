using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Riddles.Graphs.Core
{
	public class BreadthFirstSearch
	{
		public List<T> PerformBreadthFirstSearch<T>(T startingPosition, Func<T, List<T>> getNextPositions, Func<T, bool> isFinalLocation) {
			var pathLookup = new Dictionary<T, T>();
			pathLookup[startingPosition] = default;
			var positionsQueue = new List<T> { startingPosition };
			T currentLocation = default;
			T finalLocation = default;
			while (positionsQueue.Count > 0)
			{
				currentLocation = positionsQueue[0];
				if (isFinalLocation(currentLocation))
				{
					finalLocation = currentLocation;
					break;
				}
				positionsQueue.RemoveAt(0);
				var nextLocations = getNextPositions(currentLocation);
				nextLocations = nextLocations.Where(l => !pathLookup.ContainsKey(l)).ToList();

				foreach (var nextLocation in nextLocations)
				{
					pathLookup[nextLocation] = currentLocation;
					positionsQueue.Add(nextLocation);
				}
			}
			currentLocation = finalLocation;
			var path = new List<T>();
			while (currentLocation != null)
			{
				path.Add(currentLocation);
				currentLocation = pathLookup[currentLocation];
			}
			path.Reverse();
			return path;
		}
	}
}
