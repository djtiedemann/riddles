using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Riddles.Graphs.Application;

namespace Riddles.Tests.Graphs.Application
{
    public class TwoDimensionalGridCycleFinderTest
    {
        [TestCase(3, 3, 13)]
        [TestCase(4, 4, 213)]
        public void TestFindCyclesStartingAtLocation(int length, int width, int expected)
        {
            var twoDimensionalGridCycleFinder = new TwoDimensionalGridCycleFinder();
            var cycles = twoDimensionalGridCycleFinder.FindCycles(length, width);
            var actual = cycles.Count;
            Assert.AreEqual(expected, actual);
        }
    }
}
