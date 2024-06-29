using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Riddles.Combinatorics.Application;

namespace Riddles.Tests.Combinatorics.Application
{
    public class TiltedSquareTilerTest
    {
        [TestCase(0, 0)]
        [TestCase(1, 1)]
        [TestCase(2, 2)]
        [TestCase(3, 5)]
        [TestCase(4, 14)]
        [TestCase(5, 42)]
        [TestCase(6, 132)]
        [TestCase(7, 429)]
        public void TestCountNumberOfWaysToTileSteps(int numSteps, float expected)
        {
            var tiltedSquareTiler = new TiltedSquareTiler();
            var actual = tiltedSquareTiler.CountNumberOfUniqueWaysToTileSteps(numSteps);
            Assert.AreEqual(expected, actual);
        }

        [TestCase(0, 0)]
        [TestCase(1, 1)]
        [TestCase(2, 2)]
        [TestCase(3, 7)]
        [TestCase(4, 38)]
        [TestCase(5, 274)]
        [TestCase(6, 2_350)]
        [TestCase(7, 22_531)]
        public void TestCountNumberOfWaysToTilePyramid(int height, float expected)
        {
            var tiltedSquareTiler = new TiltedSquareTiler();
            var actual = tiltedSquareTiler
                .CountNumberOfUniqueWaysToTilePyramid(height);
            Assert.AreEqual(expected, actual);
        }

        [TestCase(0, 0)]
        [TestCase(1, 1)]
        [TestCase(2, 2)]
        [TestCase(3, 9)]
        [TestCase(4, 106)]
        [TestCase(5, 3_002)]
        [TestCase(6, 153_432)]
        [TestCase(7, 11_209_105)]
        public void TestCountNumberOfWaysToTileTiltedSquare(
            int distanceFromOrigin, float expected)
        {
            var tiltedSquareTiler = new TiltedSquareTiler();
            var actual = tiltedSquareTiler
                .CountNumberOfUniqueWaysToTileTiltedSquare(distanceFromOrigin);
            Assert.AreEqual(expected, actual);
        }
    }
}
