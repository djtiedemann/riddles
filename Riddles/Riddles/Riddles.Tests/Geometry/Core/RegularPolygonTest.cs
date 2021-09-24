using System;
using System.Collections.Generic;
using System.Text;
using Riddles.Geometry.Core;
using NUnit.Framework;

namespace Riddles.Tests.Geometry.Core
{
	public class RegularPolygonTest
	{
		const double Epsilon = 0.000001;

		[TestCase(3, 60)]
		[TestCase(4, 90)]
		[TestCase(5, 108)]
		[TestCase(6, 120)]
		[TestCase(9, 140)]
		public void TestInternalAngle(int numSides, double expectedInternalAngleDegrees) {
			var polygon = new RegularPolygon(numSides);
			Assert.LessOrEqual(Math.Abs(expectedInternalAngleDegrees - polygon.InternalAngle.Degrees), Epsilon);
		}

		[TestCase(3, 120)]
		[TestCase(4, 90)]
		[TestCase(5, 72)]
		[TestCase(6, 60)]
		[TestCase(9, 40)]
		public void TestExternalAngle(int numSides, double expectedExternalAngleDegrees) {
			var polygon = new RegularPolygon(numSides);
			Assert.LessOrEqual(Math.Abs(expectedExternalAngleDegrees - polygon.ExternalAngle.Degrees), Epsilon);
		}
	}
}
