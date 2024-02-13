using Riddles.Graphs.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Riddles.Graphs.Core
{
    // https://thefiddler.substack.com/p/how-many-loops-can-you-slither-around
    // You're a snake making a cycle on a 4x4 grid. Count the number of unique
    // cycles.
    public class GridCycleFinder
    {
        public GridCycleFinder()
        {
        }

        public List<List<TGraphLocation>> FindCycles<TGraphLocation>(int length, int width, IEnumerable<TGraphLocation> grid)
            where TGraphLocation : IGraphLocation
        {
            return grid.SelectMany(sl =>
                FindCyclesStartingAtLocation(sl, sl, new List<TGraphLocation> { }).ToList()
            ).ToList();
        }


        private List<List<TGraphLocation>> FindCyclesStartingAtLocation<TGraphLocation>(
            TGraphLocation currentLocation,
            TGraphLocation startingLocation,
            IEnumerable<TGraphLocation> currentPath)
            where TGraphLocation : IGraphLocation
        {
            // Copy the path and add the current location to it.
            // This ensures that it cannot be modified by another branch
            var pathCopy = currentPath.ToList();
            pathCopy.Add(currentLocation);

            // Once you reach the starting node, you have found a cycle.
            if (currentLocation.Id == startingLocation.Id && currentPath.Count() >= 4)
            {
                // The second to last node cannot be less than the second node.
                // This prevents duplicate cycles in opposite directions.
                if (currentPath.ElementAt(1).Id > currentPath.ElementAt(currentPath.Count() - 1).Id)
                {
                    return new List<List<TGraphLocation>>();
                }
                return new List<List<TGraphLocation>> { pathCopy };
            }
            var nextPossibleLocations = currentLocation.GetAdjacentLocations()
                .Where(
                    // You can never visit a node smaller than the node starting the circle.
                    // This prevents duplicate cycles with different starting points.
                    // For instance 1, 2, 3, 4 is the same as 2, 3, 4, 1.
                    // The lowest number node will come first in the cycle
                    l => l.Id >= startingLocation.Id
                    // You can never visit the node that you immediately came from.
                    // This prevents "cycles" where you simply turn around.
                    && l.Id != currentLocation.Id
                    // You can never visit a non-starting node that you've visited before.
                    && !currentPath.Select((x, i) => (x, i)).Any(x => x.i > 0 && x.x.Id == l.Id)
                );

            var allPaths = new List<List<TGraphLocation>>();
            foreach (var nextPossibleLocation in nextPossibleLocations)
            {
                var pathsFromLocation = FindCyclesStartingAtLocation(
                    (TGraphLocation)nextPossibleLocation,
                    startingLocation,
                    pathCopy
                );
                allPaths.AddRange(pathsFromLocation);
            }
            return allPaths;
        }
    }
}
