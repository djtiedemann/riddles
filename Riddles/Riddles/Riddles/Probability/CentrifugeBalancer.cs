using System;
using System.Collections.Generic;
using System.Text;
using Riddles.Geometry;
using System.Linq;

namespace Riddles.Probability
{
	public class CentrifugeBalancer
	{
		public GeometryUtilities geometryUtilities;
		public double Epsilon = 0.000001;

		public CentrifugeBalancer()
		{
			geometryUtilities = new GeometryUtilities();
		}

		public List<List<Point>> FindConfigurationThatBalancesCentrifuge(int numSlots, int numSlotsFilled)
		{
			if(numSlotsFilled == 0)
			{
				return new List<List<Point>> { new List<Point>() };
			}
			var circle = new Circle { Center = new Point(new CartesianCoordinate2D(0, 0)), Radius = 1 };
			var positionsAroundCircle = circle.GenerateNPointsEvenlyAroundCircle(numSlots, 0).ToArray();
			var successfulPositions = new List<List<Point>>();

			var numConfigurations = Math.Pow(2, numSlots);
			var configurations = Enumerable.Range(0, (int)numConfigurations)
				.Select(id => Convert.ToString(id, 2).PadLeft(numSlots, '0').ToCharArray().Select(a => a - '0').ToArray()).ToList();
			var configurationsContainingCorrectNumSlotsFilled = configurations.Where(c => c.Sum() == numSlotsFilled).ToList();
			foreach(var configuration in configurationsContainingCorrectNumSlotsFilled)
			{
				var filledPositionsInCentrifuge = positionsAroundCircle.Where((point, i) => configuration[i] == 1).ToList();
				var centroid = geometryUtilities.CalculateCenterOfMass(filledPositionsInCentrifuge);
				if(Math.Abs(centroid.CartesianCoordinate.X) < this.Epsilon && Math.Abs(centroid.CartesianCoordinate.Y) < this.Epsilon)
				{
					successfulPositions.Add(filledPositionsInCentrifuge);
				}
			}
			return successfulPositions;

		}
	}
}
