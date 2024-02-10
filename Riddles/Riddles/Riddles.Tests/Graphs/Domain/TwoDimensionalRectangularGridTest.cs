using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Riddles.Graphs.Domain;

namespace Riddles.Tests.Graphs.Domain
{
    public class TwoDimensionalRectangularGridTest
    {
        [Test]
        public void TestGenerate4x4Grid()
        {
            var twoDimensionalRectangularGrid = new TwoDimensionalRectangularGrid();
            var grid = twoDimensionalRectangularGrid.GenerateGrid(4, 4)
                .OrderBy(x => x.Id)
                .ToList();
            Assert.AreEqual(grid.Count, 16);
            Assert.AreEqual(grid[11].Id, 11);
            Assert.AreEqual(grid[6].Id, 6);
            var square3Neighbors = grid[3].GetAdjacentLocations()
                .OrderBy(x => x.Id)
                .ToList();
            Assert.AreEqual(square3Neighbors.Count, 2);
            Assert.AreEqual(square3Neighbors[0].Id, 2);
            Assert.AreEqual(square3Neighbors[1].Id, 7);
            var square5Neighbors = grid[5].GetAdjacentLocations()
                .OrderBy(x => x.Id)
                .ToList();
            Assert.AreEqual(square5Neighbors.Count, 4);
            Assert.AreEqual(square5Neighbors[0].Id, 1);
            Assert.AreEqual(square5Neighbors[1].Id, 4);
            Assert.AreEqual(square5Neighbors[2].Id, 6);
            Assert.AreEqual(square5Neighbors[3].Id, 9);
            var square13Neighbors = grid[13].GetAdjacentLocations()
                .OrderBy(x => x.Id)
                .ToList();
            Assert.AreEqual(square13Neighbors.Count, 3);
            Assert.AreEqual(square13Neighbors[0].Id, 9);
            Assert.AreEqual(square13Neighbors[1].Id, 12);
            Assert.AreEqual(square13Neighbors[2].Id, 14);
        }
    }
}
