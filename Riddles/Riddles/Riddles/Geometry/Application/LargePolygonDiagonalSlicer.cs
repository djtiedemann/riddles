using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Riddles.Geometry.Core;

namespace Riddles.Geometry.Application
{
	public class LargePolygonDiagonalSlicer
	{
		private double Epsilon = 0.00000001;
		private GeometryUtilities _geometryUtilities;
		private VectorUtilities _vectorUtilities;
		public LargePolygonDiagonalSlicer()
		{
			this._geometryUtilities = new GeometryUtilities();
			this._vectorUtilities = new VectorUtilities();
		}

		// https://fivethirtyeight.com/features/can-you-bake-the-radish-pie/
		// you have a large polygon, you want to slice the longest diagonal (opposite corners, or opposite corner to opposite side)
		// with all of the diagonals that are perpendicular and find the product of the slices
		public double CalculateProductOfDiagonalSlices(int numSides, double sideLength)
		{
			if (numSides < 3)
			{
				throw new InvalidOperationException("A polygon must have at least 3 sides");
			}
			if (sideLength <= 0)
			{
				throw new InvalidOperationException("The length of the sides of the polygon must be > 0");
			}
			var polygon = new RegularPolygon(numSides, sideLength, new Point(0, 0), new Angle(0, MeasurementType.Radians));
			var vertices = polygon.Vertices;
			var verticesInUpperQuadrant = vertices.Where(v => v.Y >= -1*Epsilon).ToArray();
			var rollingProduct = 1.0;
			var vectorNegativeX = new Vector(new double[] { -1, 0 });
			// to solve for the total product, break the horizontal along the x-axis into segments based on the vertices
			// for each pair of vertices:
			// 1) project the first point down to the x-axis
			// 2) calculate the angle between the line segment connecting those two points and the x-axis
			// 3) find the distance between the second point and the projected point
			// 4) the horizontal distance between the points = Abs(d*cos(theta))
			for (int i = 0; i < verticesInUpperQuadrant.Length - 1; i++) {
				var firstPointProjectedDown = new Point(verticesInUpperQuadrant[i].X, 0);
				var distanceBetweenPoints = this._geometryUtilities.CalculateCartesianDistance(
					verticesInUpperQuadrant[i + 1], firstPointProjectedDown);
				var vectorBetweenPoints = new Vector(new double[] {
					verticesInUpperQuadrant[i+1].X - firstPointProjectedDown.X,
					verticesInUpperQuadrant[i+1].Y - firstPointProjectedDown.Y
				});
				var angleBetweenVectors = this._vectorUtilities.CalculateAngleBetweenVectors(vectorNegativeX, vectorBetweenPoints);
				var segmentDistance = Math.Abs(distanceBetweenPoints * Math.Cos(angleBetweenVectors.Radians));
				rollingProduct *= segmentDistance;
			}
			return rollingProduct;
		}
	}
}
