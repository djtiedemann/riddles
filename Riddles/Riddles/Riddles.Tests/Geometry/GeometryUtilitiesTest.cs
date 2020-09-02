using NUnit.Framework;
using Riddles.Geometry;
using System;

namespace Riddles.Tests
{
    public class GeometryUtilitiesTest
    {
        const double Epsilon = 0.000001;

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
    }
}