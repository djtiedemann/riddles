using System;
using System.Collections.Generic;
using System.Text;
using Riddles.Geometry;
using NUnit.Framework;

namespace Riddles.Tests.Geometry
{
	public class VectorUtilitiesTest
	{
		const double Epsilon = 0.000001;

		// simple unit circle test cases
		[TestCase(new double[] { 1, 0}, new double[] { 1, 0 }, 0)]
		[TestCase(new double[] { 1, 0 }, new double[] { 0.8660254, 0.5 }, Math.PI/6)]
		[TestCase(new double[] { 1, 0 }, new double[] { 0.707106781, 0.707106781 }, Math.PI/4)]
		[TestCase(new double[] { 1, 0 }, new double[] { 0.5, 0.8660254 }, Math.PI / 3)]
		[TestCase(new double[] { 1, 0 }, new double[] { 0, 1 }, Math.PI / 2)]
		[TestCase(new double[] { 1, 0 }, new double[] { -0.5, 0.8660254 }, 2*Math.PI / 3)]
		[TestCase(new double[] { 1, 0 }, new double[] { -0.707106781, 0.707106781 }, 3*Math.PI / 4)]
		[TestCase(new double[] { 1, 0 }, new double[] { -0.8660254, 0.5 }, 5*Math.PI / 6)]
		[TestCase(new double[] { 1, 0 }, new double[] { -1, 0 }, Math.PI)]
		[TestCase(new double[] { 1, 0 }, new double[] { -0.8660254, -0.5 }, 5 * Math.PI / 6)]
		[TestCase(new double[] { 1, 0 }, new double[] { -0.707106781, -0.707106781 }, 3 * Math.PI / 4)]
		[TestCase(new double[] { 1, 0 }, new double[] { -0.5, -0.8660254 }, 2 * Math.PI / 3)]
		[TestCase(new double[] { 1, 0 }, new double[] { 0, -1 }, Math.PI / 2)]
		[TestCase(new double[] { 1, 0 }, new double[] { 0.5, -0.8660254 }, Math.PI / 3)]
		[TestCase(new double[] { 1, 0 }, new double[] { 0.707106781, -0.707106781 }, Math.PI / 4)]
		[TestCase(new double[] { 1, 0 }, new double[] { 0.8660254, -0.5 }, Math.PI / 6)]
		// test more complicated vector
		[TestCase(new double[] { -0.707106781, 0.707106781 }, new double[] { 0.5, -0.8660254 }, 22*Math.PI / 24.0)]
		[TestCase(new double[] { 0.5, -0.8660254 }, new double[] { -0.707106781, 0.707106781 }, 22 * Math.PI / 24.0)]
		// test non-unit vectors
		[TestCase(new double[] { 3*0.5, 3*-0.8660254 }, new double[] { 7*-0.707106781, 7*0.707106781 }, 22 * Math.PI / 24.0)]
		[TestCase(new double[] { 14, 0 }, new double[] { 0, -5 }, Math.PI/2)]
		public void TestGetAngleBetweenVectors(double[] v1, double[] v2, double expectedAngleRadians)
		{
			var vector1 = new Vector(v1);
			var vector2 = new Vector(v2);
			var vectorUtilities = new VectorUtilities();
			var angleBetweenVectors = vectorUtilities.CalculateAngleBetweenVectors(vector1, vector2);
			Assert.LessOrEqual(Math.Abs(angleBetweenVectors.Radians - expectedAngleRadians), Epsilon);
		}
	}
}
