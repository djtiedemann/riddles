using System;
using System.Collections.Generic;
using System.Text;
using Riddles.Geometry.Core;
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
			var circle = new Circle(new Point(0, 0), 1);
			var positionsAroundCircle = circle.GenerateNPointsEvenlyAroundCircle(
				numSlots, new Angle(0, MeasurementType.Radians)).ToArray();
			var successfulPositions = new List<List<Point>>();

			var numConfigurations = Math.Pow(2, numSlots);
			var configurations = Enumerable.Range(0, (int)numConfigurations)
				.Select(id => Convert.ToString(id, 2).PadLeft(numSlots, '0').ToCharArray().Select(a => a - '0').ToArray()).ToList();
			var configurationsContainingCorrectNumSlotsFilled = configurations.Where(c => c.Sum() == numSlotsFilled).ToList();
			foreach(var configuration in configurationsContainingCorrectNumSlotsFilled)
			{
				var filledPositionsInCentrifuge = positionsAroundCircle.Where((point, i) => configuration[i] == 1).ToList();
				var centroid = geometryUtilities.CalculateCenterOfMass(filledPositionsInCentrifuge);
				if(Math.Abs(centroid.X) < this.Epsilon && Math.Abs(centroid.Y) < this.Epsilon)
				{
					successfulPositions.Add(filledPositionsInCentrifuge);
				}
			}
			return successfulPositions;

		}
	}
}
