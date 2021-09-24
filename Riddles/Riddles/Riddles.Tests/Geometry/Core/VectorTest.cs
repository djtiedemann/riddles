using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Riddles.Geometry.Core;

namespace Riddles.Tests.Geometry.Core
{
	public class VectorTest
	{
		const double Epsilon = 0.000001;

		[TestCase(new double[] { 1, 1, 1 }, 1.73205081)]
		[TestCase(new double[] { 3, 4 }, 5)]
		[TestCase(new double[] { 5, 12 }, 13)]
		[TestCase(new double[] { 4, 0 }, 4)]
		[TestCase(new double[] { 4, 2 }, 4.472135955)]
		public void TestMagnitude(double[] components, double expectedMagnitude)
		{
			var vector = new Vector(components);
			Assert.LessOrEqual(Math.Abs(expectedMagnitude - vector.Magnitude), Epsilon);
			var unitVector = vector.ToUnitVector();
			Assert.LessOrEqual(Math.Abs(unitVector.Magnitude - 1), Epsilon);
		}
	}
}
