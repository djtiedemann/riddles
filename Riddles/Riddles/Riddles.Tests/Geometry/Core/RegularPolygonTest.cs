using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Riddles.Geometry.Core;
using NUnit.Framework;

namespace Riddles.Tests.Geometry.Core
{
	public class RegularPolygonTest
	{
		const double Epsilon = 0.000001;
		public Dictionary<int, List<Angle>> numSidesToPointOrientationDictionary = new Dictionary<int, List<Angle>>
		{
			{12, new List<Angle> {
				new Angle(0, MeasurementType.Radians),
				new Angle(Math.PI/6, MeasurementType.Radians),
				new Angle(Math.PI/3, MeasurementType.Radians),
				new Angle(Math.PI/2, MeasurementType.Radians),
				new Angle(2*Math.PI/3, MeasurementType.Radians),
				new Angle(5*Math.PI/6, MeasurementType.Radians),
				new Angle(Math.PI, MeasurementType.Radians),
				new Angle(7*Math.PI/6, MeasurementType.Radians),
				new Angle(4*Math.PI/3, MeasurementType.Radians),
				new Angle(3*Math.PI/2, MeasurementType.Radians),
				new Angle(5*Math.PI/3, MeasurementType.Radians),
				new Angle(11*Math.PI/6, MeasurementType.Radians),
			}}
		};

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

		[TestCase(12, 2)]
		public void TestGetVertices(int numSides, int sideLength)
		{
			var geometryUtilities = new GeometryUtilities();
			var polygon = new RegularPolygon(numSides, sideLength, new Point(0, 0), new Angle(0, MeasurementType.Radians));
			var actualPoints = polygon.Vertices.ToArray();
			var expectedPoints = numSidesToPointOrientationDictionary[numSides].ToArray();
			Assert.AreEqual(expectedPoints.Length, actualPoints.Length);
			for(int i=0; i<actualPoints.Length; i++)
			{
				Assert.LessOrEqual(Math.Abs(expectedPoints[0].Radians - actualPoints[0].Theta.Radians), Epsilon);
				Assert.LessOrEqual(
					Math.Abs(geometryUtilities.CalculateCartesianDistance(actualPoints[i], actualPoints[(i + 1) % numSides]) - 2),
					Epsilon
				);
				Assert.LessOrEqual(
					Math.Abs(actualPoints[i].R - (double)sideLength / (2 * Math.Sin(Math.PI / (double)numSides))),
					Epsilon
				);
			}
		}
	}
}
