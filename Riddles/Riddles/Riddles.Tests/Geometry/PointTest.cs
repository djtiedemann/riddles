using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Riddles.Geometry; 

namespace Riddles.Tests.Geometry
{
	public class PointTest
	{
		double epsilon = 0.000001;

		[TestCase(0, 0, 0, 0)]
		[TestCase(4*1, 0, 4*1, 0)]
		[TestCase(9*0.866025404, 9*0.5, 9*1, Math.PI / 6)]
		[TestCase(16*0.707106781, 16*0.707106781, 16*1, Math.PI / 4)]
		[TestCase(0.5, 0.866025404, 1, Math.PI / 3)]
		[TestCase(0, 1, 1, Math.PI/2)]
		[TestCase(-0.5, 0.866025404, 1, 2*Math.PI / 3)]
		[TestCase(-0.707106781, 0.707106781, 1, 3*Math.PI / 4)]
		[TestCase(-0.866025404, 0.5, 1, 5*Math.PI / 6)]
		[TestCase(4*-1, 0, 4*1, Math.PI)]
		[TestCase(-0.866025404, -0.5, 1, 7 * Math.PI / 6)]
		[TestCase(-0.707106781, -0.707106781, 1, 5 * Math.PI / 4)]
		[TestCase(-0.5, -0.866025404, 1, 4 * Math.PI / 3)]
		[TestCase(0, -1, 1, 3*Math.PI / 2)]
		[TestCase(25*0.5, 25*-0.866025404, 25*1, 5 * Math.PI / 3)]
		[TestCase(9*0.707106781, 9*-0.707106781, 9*1, 7 * Math.PI / 4)]
		[TestCase(0.866025404, -0.5, 1, 11 * Math.PI / 6)]
		public void TestCoordinateConversion(double x, double y, double r, double thetaRadians) {
			var point1 = new Point(x, y);
			var point2 = new Point(r, new Angle(thetaRadians, MeasurementType.Radians));
			Assert.LessOrEqual(Math.Abs(point1.X - point2.X), epsilon);
			Assert.LessOrEqual(Math.Abs(point1.Y - point2.Y), epsilon);
			Assert.LessOrEqual(Math.Abs(point1.R - point2.R), epsilon);
			Assert.LessOrEqual(Math.Abs(point1.Theta.Degrees - point2.Theta.Degrees), epsilon);
			Assert.LessOrEqual(Math.Abs(point1.Theta.Radians - point2.Theta.Radians), epsilon);
		}
	}
}
