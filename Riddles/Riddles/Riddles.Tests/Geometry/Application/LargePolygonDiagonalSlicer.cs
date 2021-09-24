using System;
using System.Collections.Generic;
using System.Text;
using Riddles.Geometry.Core;

namespace Riddles.Geometry.Application
{
	public class LargePolygonDiagonalSlicer
	{
		// https://fivethirtyeight.com/features/can-you-bake-the-radish-pie/
		// you have a large polygon, you want to slice the longest diagonal (opposite corners, or opposite corner to opposite side)
		// with all of the diagonals that are perpendicular and find the product of the slices
		public void CalculateProductOfDiagonalSlices(int numSlices, int sideLength ) {
			if(sideLength < 3)
			{
				throw new InvalidOperationException("A polygon must have at least 3 sides");
			}
			
			var unitCircle = new Circle(new Point(0, 0), 1);
			var pointsAroundCircle = unitCircle.GenerateNPointsEvenlyAroundCircle(numSlices, new Angle(0, MeasurementType.Radians));

		}
	}
}
