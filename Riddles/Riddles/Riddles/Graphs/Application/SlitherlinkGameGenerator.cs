using Riddles.Combinatorics.Core.Permutations;
using Riddles.Graphs.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Riddles.Graphs.Application
{
    /// <summary>
    /// Find the number of Slitherlink games with a single solution can be generated on
    /// a nxn grid
    /// </summary>
    public class SlitherlinkGameGenerator
    {
        private TwoDimensionalGridCycleFinder _gridCycleFinder;
        private TwoDimensionalRectangularGrid _gridGenerator;
        private PermutationWithRepetitionGenerator _permutationGenerator;

        public SlitherlinkGameGenerator() { 
            this._gridCycleFinder = new TwoDimensionalGridCycleFinder();
            this._gridGenerator = new TwoDimensionalRectangularGrid();
            this._permutationGenerator = new PermutationWithRepetitionGenerator();
        }

        // General idea is:
        // 1) Find all cycles
        // 2) Convert them to a game with all edges specified
        // 3) For each fully specified game, also generate all possible games that could lead to the solution
        // 4) Turn those games into strings, and count how many different cycles can be generated with those strings
        // 5) The games that are only present in one cycle have exactly one solution.
        public int GenerateNumValidSlitherlinkGames(int length, int width)
        {
            Dictionary<string, int> numSolutionsPerGame = new Dictionary<string, int>();
            var grid = this._gridGenerator.GenerateGrid(length, width);
            var gridCycles = this._gridCycleFinder.FindCycles(length, width);
            var permutations = this._permutationGenerator.GenerateAllOutcomes((length - 1) * (width - 1), new List<char> { '0', '1' });
            var fullySpecifiedGames = 
                gridCycles.Select(c => 
                    this.GetEdgeCountForCycle(length, width, c)
                ).ToList();
            foreach(var fullySpecifiedGame in fullySpecifiedGames)
            {
                var games = this.GenerateCanonicalPuzzleRepresentation(
                    fullySpecifiedGame,
                    permutations
                );
                foreach(var game in games)
                {
                    if (!numSolutionsPerGame.ContainsKey(game))
                    {
                        numSolutionsPerGame[game] = 0;
                    }
                    numSolutionsPerGame[game]++;
                }
            }
            var gamesWithOneSolution = numSolutionsPerGame.Keys.Where(
                k => numSolutionsPerGame[k] == 1
            );
            return gamesWithOneSolution.Count();
        }

        private List<string> GenerateCanonicalPuzzleRepresentation(int[] edgeCountsForCycle, List<string> permutations)
        {
            var canonicalRepresentations = permutations.Select(
                p => edgeCountsForCycle.Select(
                    (e, i) => p.ToCharArray()[i] == '1' ? $"{e}" : "_")
                        .Aggregate("", (agg, v) => $"{agg}{v}")
            ).ToList();
            return canonicalRepresentations;
        }

        private int[] GetEdgeCountForCycle(int length, int width, List<TwoDimensionalRectangularGrid.Location> cycle)
        {
            var edgeCount = Enumerable.Range(0, (length - 1) * (width - 1)).ToArray();
            for(int i=0; i<cycle.Count - 1; i++)
            {
                var location1 = cycle[i];
                var location2 = cycle[i+1];
                // either the X location must match or the Y location must match
                // each edge is associated with 1 or 2 boxes depending on whether or not it is on the edge of the grid
                if(location1.X == location2.X)
                {
                    var minY = Math.Min(location1.Y, location2.Y);
                    if (location1.X != 0)
                    {
                        edgeCount[(location1.X - 1) * width + minY]++;
                    }
                    if (location1.X != length - 1)
                    {
                        edgeCount[(location1.X * width) + minY]++;
                    }
                }
                else
                {
                    var minX = Math.Min(location1.X, location2.X);
                    if(location1.Y != 0)
                    {
                        edgeCount[(minX * width) - location1.Y]++;
                    }
                    if(location1.Y != width - 1)
                    {
                        edgeCount[(minX * width ) + location1.Y]++;
                    }
                }
            }
            return edgeCount;
        }
    }
}
