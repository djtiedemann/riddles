using System;
using System.Collections.Generic;
using System.Text;
using Riddles.Geometry.Core;
using Riddles.Geometry.Application;
using NUnit.Framework;

namespace Riddles.Tests.Geometry.Application
{
	public class LargePolygonDiagonalSlicerTest
	{
		private double Epsilon = 0.00000001;

		[TestCase(5, 2, 2.2360679774997894)]
		[TestCase(6, 2, 2)]
		[TestCase(1000, 2, 2)]
		[TestCase(1001, 2, 31.638584039074495)]
		public void TestCalculateProductOfDiagonalSlices(int numSides, double sideLength, double expectedProduct ) {
			var largePolygonDiagonalSlicer = new LargePolygonDiagonalSlicer();
			var product = largePolygonDiagonalSlicer.CalculateProductOfDiagonalSlices(numSides, sideLength);
			Assert.LessOrEqual(expectedProduct - product, Epsilon);
		}
	}
}
