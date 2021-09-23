using System;
using System.Collections.Generic;
using System.Text;
using Riddles.Geometry;
using NUnit.Framework;

namespace Riddles.Tests.Geometry
{
	public class CircleTest
	{
		double epsilon = 0.0001;
		public Dictionary<int, List<Point>> expectedPointsDictionary = new Dictionary<int, List<Point>>
		{
			{1, new List<Point>{ new Point(1, 0) } },
			{2, new List<Point>{ new Point(2, 0) , new Point(-2, 0) } },
			{3, new List<Point>{ new Point(3, 1), new Point(-1, 1) } },
			{4, new List<Point>{ new Point(0, 3), new Point(-3, 0),
				new Point(0, -3), new Point(3, 0) } },
			{5, new List<Point> {
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
				new Point(0.86602, -0.5), }}
		};
		
		[OneTimeSetUp]
		public void OneTimeSetup()
		{
			
		}

		[TestCase(1, 1, 0, 0, 1, 0, Description = "Simple case, 1 point, unit circle")]
		[TestCase(2, 2, 0, 0, 2, 0, Description = "2 points, circle at (0,0) radius 2")]
		[TestCase(3, 2, 1, 1, 2, 0, Description = "2 points, circle at (1,1) radius 2")]
		[TestCase(4, 3, 0, 0, 4, Math.PI/2.0, Description = "4 points, circle at (0,0) radius 3, starting at Pi/2")]
		[TestCase(5, 1, 0, 0, 12, 0, Description = "Twelve points, equal distance around unit cicle")]
		public void TestGenerateNPointsEvenlyAroundCircle(int testCaseId, int radius, double initialX, double initialY, int numPoints, double initialAngle)
		{
			var circle = new Circle { Center = new Point(initialX, initialY), Radius = radius };
			var points = circle.GenerateNPointsEvenlyAroundCircle(numPoints, initialAngle).ToArray();
			var expectedPoints = expectedPointsDictionary[testCaseId].ToArray();
			Assert.AreEqual(expectedPoints.Length, points.Length);
			for(int i=0; i<numPoints; i++)
			{
				Assert.LessOrEqual(Math.Abs(points[i].X - expectedPoints[i].X), epsilon);
				Assert.LessOrEqual(Math.Abs(points[i].Y - expectedPoints[i].Y), epsilon);
			}
		}
	}
}
