using System;
using System.Collections.Generic;
using System.Text;
using Riddles.Geometry;
using NUnit.Framework;

namespace Riddles.Tests.Geometry
{
	public class AngleTest
	{
		const double Epsilon = 0.000001;

		[TestCase(Math.PI, 180)]
		[TestCase(Math.PI*2, 360)]
		[TestCase(Math.PI/2, 90)]
		[TestCase(Math.PI/4, 45)]
		[TestCase(Math.PI/3, 60)]
		[TestCase(Math.PI/6, 30)]
		public void TestRadians(double radians, double expectedDegrees)
		{
			var angle = new Angle { Radians = radians };
			Assert.LessOrEqual(Math.Abs(expectedDegrees - angle.Degrees), Epsilon);
		}

		[TestCase(Math.PI, 180)]
		[TestCase(Math.PI * 2, 360)]
		[TestCase(Math.PI / 2, 90)]
		[TestCase(Math.PI / 4, 45)]
		[TestCase(Math.PI / 3, 60)]
		[TestCase(Math.PI / 6, 30)]
		public void TestDegrees(double expectedRadians, double degrees)
		{
			var angle = new Angle { Degrees = degrees };
			Assert.LessOrEqual(Math.Abs(expectedRadians - angle.Radians), Epsilon);
		}
	}
}
