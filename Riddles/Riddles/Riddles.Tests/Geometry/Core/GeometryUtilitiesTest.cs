using NUnit.Framework;
using Riddles.Geometry.Core;
using Riddles.Simulations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Riddles.Tests.Geometry.Core
{
    public class GeometryUtilitiesTest
    {
        const double Epsilon = 0.000001;

        public Dictionary<int, List<Point>> PointsForCenterOfMassDictionary = new Dictionary<int, List<Point>>
        {
            {1, new List<Point>{ new Point(3, 0) } },
            {2, new List<Point>{ 
                new Point(3, 0), 
                new Point(-1, 2), 
                new Point(-1, -3) } 
            },
            {3, new List<Point> {
                new Point(1, 0), 
                new Point(0.86602, 0.5), 
                new Point(0.5, 0.86602),
                new Point(0, 1),
                new Point(-0.5, 0.86602),
                new Point(-0.86602, 0.5),
                new Point(-1, 0),
                new Point(-0.86602, -0.5),
                new Point(-0.5, -0.86602),
                new Point(0, -1),
                new Point(0.5, -0.86602),
                new Point(0.86602, -0.5)} 
            }
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

        [TestCase(4, 7, 1, 3, 5)]
        [TestCase(-7, 6, -19, 1, 13)]
        [TestCase(-7, 3, 5, -2, 13)]
        [TestCase(0, 4, 0, 4, 0)]
        [TestCase(2, 0, 0, 0, 2)]
        public void TestCalculateCartesianDistance(double x1, double y1, double x2, double y2, double expectedDistance)
		{
            var point1 = new Point(x1, y1);
            var point2 = new Point(x2, y2);
            var geometryUtilities = new GeometryUtilities();
            var distance = geometryUtilities.CalculateCartesianDistance(point1, point2);
            Assert.LessOrEqual(Math.Abs(distance - expectedDistance), Epsilon);
		}

        [TestCase(2, 0, 2 * 1)]
        [TestCase(2, Math.PI / 6, 2 * 1.36602540378)] // Sqrt(3)/2 + 1/2
        [TestCase(2, Math.PI / 4, 2 * 1.41421356237)] // Sqrt(2)
        [TestCase(2, Math.PI / 3, 2 * 1.36602540378)] // Sqrt(3)/2 + 1/2
        [TestCase(2, Math.PI / 2, 2 * 1)]
        [TestCase(2, 2 * Math.PI / 3, 2 * 1.36602540378)] // Sqrt(3)/2 + 1/2
        [TestCase(2, 3 * Math.PI / 4, 2 * 1.41421356237)] // Sqrt(2)
        [TestCase(2, 5 * Math.PI / 6, 2 * 1.36602540378)] // Sqrt(3)/2 + 1/2
        [TestCase(2, Math.PI, 2 * 1)]
        [TestCase(2, 7 * Math.PI / 6, 2 * 1.36602540378)] // Sqrt(3)/2 + 1/2
        [TestCase(2, 5 * Math.PI / 4, 2 * 1.41421356237)] // Sqrt(2)
        [TestCase(2, 4 * Math.PI / 3, 2 * 1.36602540378)] // Sqrt(3)/2 + 1/2
        [TestCase(2, 3 * Math.PI / 2, 2 * 1)]
        [TestCase(2, 5 * Math.PI / 3, 2 * 1.36602540378)] // Sqrt(3)/2 + 1/2
        [TestCase(2, 7 * Math.PI / 4, 2 * 1.41421356237)] // Sqrt(2)
        [TestCase(2, 11 * Math.PI / 6, 2 * 1.36602540378)] // Sqrt(3)/2 + 1/2
        [TestCase(2, 2 * Math.PI, 2 * 1)]
        public void TestGetDeliveryManhattanDistance(double r, double theta, double expectedDistance)
        {
            var geometryUtilities = new GeometryUtilities();
            var actualDistance = geometryUtilities.CalculateManhattanDistance(r, theta);
            var diff = Math.Abs(expectedDistance - actualDistance);
            Assert.LessOrEqual(diff, Epsilon);
        }

        [TestCase(2, 0, 2 * 1)]
        [TestCase(2, Math.PI / 6, 2 * 1.073132184970986)] // Sqrt(3)/2 + (Sqrt(2) - 1) * 1/2
        [TestCase(2, Math.PI / 4, 2 * 1)]
        [TestCase(2, Math.PI / 3, 2 * 1.073132184970986)] // Sqrt(3)/2 + (Sqrt(2) - 1) * 1/2
        [TestCase(2, Math.PI / 2, 2 * 1)]
        [TestCase(2, 2 * Math.PI / 3, 2 * 1.073132184970986)] // Sqrt(3)/2 + (Sqrt(2) - 1) * 1/2
        [TestCase(2, 3 * Math.PI / 4, 2 * 1)]
        [TestCase(2, 5 * Math.PI / 6, 2 * 1.073132184970986)] // Sqrt(3)/2 + (Sqrt(2) - 1) * 1/2
        [TestCase(2, Math.PI, 2 * 1)]
        [TestCase(2, 7 * Math.PI / 6, 2 * 1.073132184970986)] // Sqrt(3)/2 + (Sqrt(2) - 1) * 1/2
        [TestCase(2, 5 * Math.PI / 4, 2 * 1)]
        [TestCase(2, 4 * Math.PI / 3, 2 * 1.073132184970986)] // Sqrt(3)/2 + (Sqrt(2) - 1) * 1/2
        [TestCase(2, 3 * Math.PI / 2, 2 * 1)]
        [TestCase(2, 5 * Math.PI / 3, 2 * 1.073132184970986)] // Sqrt(3)/2 + (Sqrt(2) - 1) * 1/2
        [TestCase(2, 7 * Math.PI / 4, 2 * 1)]
        [TestCase(2, 11 * Math.PI / 6, 2 * 1.073132184970986)] // Sqrt(3)/2 + (Sqrt(2) - 1) * 1/2
        [TestCase(2, 2 * Math.PI, 2 * 1)]
        public void TestGetDeliveryDiagonalDistance(double r, double theta, double expectedDistance)
        {
            var geometryUtilities = new GeometryUtilities();
            var actualDistance = geometryUtilities.CalculateManhattanDistanceWithDiagonals(r, theta);
            var diff = Math.Abs(expectedDistance - actualDistance);
            Assert.LessOrEqual(diff, Epsilon);
        }
    }
}