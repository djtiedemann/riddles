using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Riddles.Graphs.Core;
using Riddles.Graphs.Model;

namespace Riddles.Tests.Graphs.Core
{
    public class GridCycleFinderTest
    {
        [TestCase(3, 3, 13)]
        [TestCase(4, 4, 213)]
        public void TestFindCyclesStartingAtLocation(int length, int width, int expected)
        {
            var gridCycleFinder = new GridCycleFinder();
            var twoDimensionalGridGenerator = new TwoDimensionalRectangularGridGenerator();
            var twoDimensionalGrid = twoDimensionalGridGenerator.GenerateGrid(length, width);
            var cycles = gridCycleFinder.FindCycles(length, width, twoDimensionalGrid);
            var actual = cycles.Count;
            Assert.AreEqual(expected, actual);
        }
    }
}
