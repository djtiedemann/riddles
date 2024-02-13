using Riddles.Combinatorics.Core.Permutations;
using Riddles.Graphs.Core;
using Riddles.Graphs.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Riddles.Graphs.Application
{
    /// <summary>
    /// Find the number of Slitherlink games with a single solution can be generated on
    /// a nxn grid. https://thefiddler.substack.com/p/how-many-loops-can-you-slither-around
    /// 
    /// Look up Slitherlink to learn about the game. There must be one solution.
    /// </summary>
    public class SlitherlinkGameGenerator
    {
        private GridCycleFinder _gridCycleFinder;
        private TwoDimensionalRectangularGridGenerator _gridGenerator;
        private PermutationWithRepetitionGenerator _permutationGenerator;

        public SlitherlinkGameGenerator() { 
            this._gridCycleFinder = new GridCycleFinder();
            this._gridGenerator = new TwoDimensionalRectangularGridGenerator();
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
            var gridCycles = this._gridCycleFinder.FindCycles(length, width, grid);
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
            ).ToList();
            return gamesWithOneSolution.Count();
        }

        public List<string> GenerateCanonicalPuzzleRepresentation(int[] edgeCountsForCycle, List<string> permutations)
        {
            var canonicalRepresentations = permutations.Select(
                p => edgeCountsForCycle.Select(
                    (e, i) => p.ToCharArray()[i] == '1' ? $"{e}" : "_")
                        .Aggregate("", (agg, v) => $"{agg}{v}")
            ).ToList();
            return canonicalRepresentations;
        }

        public int[] GetEdgeCountForCycle(int length, int width, List<TwoDimensionalRectangularGridGenerator.Location> cycle)
        {
            var edgeCount = Enumerable.Range(0, (length - 1) * (width - 1)).Select(i => 0).ToArray();
            for(int i=0; i<cycle.Count - 1; i++)
            {
                var indexesForEdge = this.GetIndexesForEdge(
                    cycle[i],
                    cycle[i + 1],
                    length,
                    width
                );
                foreach(var index in indexesForEdge)
                {
                    edgeCount[index]++;
                }
            }
            return edgeCount;
        }

        public IEnumerable<int> GetIndexesForEdge(
            TwoDimensionalRectangularGridGenerator.Location location1,
            TwoDimensionalRectangularGridGenerator.Location location2,
            int length,
            int width
        )
        {
            // either the row location must match or the col location must match
            // each edge is associated with 1 or 2 boxes depending on whether or not it is on the edge of the grid
            var edges = new List<int>() { };
            if (location1.Row == location2.Row)
            {
                var minCol = Math.Min(location1.Column, location2.Column);
                if (location1.Row != length - 1)
                {
                    edges.Add((location1.Row * (width - 1)) + minCol);
                }
                if (location1.Row != 0)
                {
                    edges.Add((location1.Row - 1) * (width - 1) + minCol);
                }
            }
            else
            {
                var minRow = Math.Min(location1.Row, location2.Row);
                var maxRow = Math.Max(location1.Row, location2.Row);
                if (location1.Column != width - 1)
                {
                    edges.Add((minRow * (width - 1)) + location1.Column);
                }
                if (location1.Column != 0)
                {
                    edges.Add((minRow * (width - 1)) + location1.Column - 1);
                }
            }
            return edges;
        }
    }
}