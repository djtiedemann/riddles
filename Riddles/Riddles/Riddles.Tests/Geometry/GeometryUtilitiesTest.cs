using NUnit.Framework;
using Riddles.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Riddles.Tests
{
    public class GeometryUtilitiesTest
    {
        const double Epsilon = 0.000001;

        public Dictionary<int, List<Point>> PointsForCenterOfMassDictionary = new Dictionary<int, List<Point>>
        {
            {1, new List<Point>{ new Point { X = 3, Y = 0 } } },
            {2, new List<Point>{ new Point { X = 3, Y = 0 }, new Point { X = -1, Y = 2 }, new Point { X = -1, Y = -3 } } },
            {3, new List<Point> {
                new Point { X = 1, Y = 0 } , new Point { X = 0.86602, Y = 0.5 }, new Point { X = 0.5, Y = 0.86602 },
                new Point { X = 0, Y = 1 } , new Point { X = -0.5, Y = 0.86602 }, new Point { X = -0.86602, Y = 0.5 },
                new Point { X = -1, Y = 0 } , new Point { X = -0.86602, Y = -0.5 }, new Point { X = -0.5, Y = -0.86602 },
                new Point { X = 0, Y = -1 } ,  new Point { X = 0.5, Y = -0.86602 }, new Point { X = 0.86602, Y = -0.5 }} }
        };

        [SetUp]
        public void Setup()
        {
        }

        [TestCase(1, 1, 1, null, null, Description = "x^2 + x + 1, no solutions")]
        [TestCase(1, 0, 0, 0, null, Description = "x^2, solution: x = 0")]
        [TestCase(1, 4, 4, -2, null, Description = "x^2 + 4x + 4, solution: x = -2")]
        [TestCase(1, -2, -3, -1, 3, Description = "x^2 - 2x - 3, solution: x = -1, 3")]
        [TestCase(8, -2, -3, -0.5, 0.75, Description = "(2x + 1)(4x - 3) = 8x^2 - 2x - 3")]
        public void TestQuadraticFormula(double a, double b, double c, double? solution1, double? solution2)
        {
            GeometryUtilities utilities = new GeometryUtilities();
            var quadraticFormulaSolutions = utilities.QuadraticFormula(a, b, c);
            if(solution1 == null)
			{
                Assert.AreEqual(quadraticFormulaSolutions.Count, 0);
                return;
			}
            if(solution2 == null)
			{
                Assert.AreEqual(quadraticFormulaSolutions.Count, 1);
                Assert.LessOrEqual(Math.Abs(solution1.Value - quadraticFormulaSolutions[0]), Epsilon);
                return;
			}
            Assert.AreEqual(quadraticFormulaSolutions.Count, 2);
            Assert.LessOrEqual(Math.Abs(solution1.Value - quadraticFormulaSolutions[0]), Epsilon);
            Assert.LessOrEqual(Math.Abs(solution2.Value - quadraticFormulaSolutions[1]), Epsilon);
        }

        [TestCase(1, 3, 0, Description = "Single Point")]
        [TestCase(2, 0.333333, -0.333333, Description = "Multiple points, not centered on (0, 0)")]
        [TestCase(3, 0, 0, Description = "points equally spaced among unit circle")]
        public void TestCenterOfMass(int testCaseNum, double expectedCenterOfMassX, double expectedCenterOfMassY)
		{
            GeometryUtilities utilities = new GeometryUtilities();
            var actualCenterOfMass = utilities.CalculateCenterOfMass(PointsForCenterOfMassDictionary[testCaseNum]);
            Assert.LessOrEqual(Math.Abs(expectedCenterOfMassX - actualCenterOfMass.X), Epsilon);
            Assert.LessOrEqual(Math.Abs(expectedCenterOfMassY - actualCenterOfMass.Y), Epsilon);

        }
    }
}