using System;
using System.Collections.Generic;
using System.Text;

namespace Riddles.Geometry.Core
{
	public class VectorUtilities
	{
		public double CalculateDotProduct(Vector v1, Vector v2) { 
			if(v1.Components.Length != v2.Components.Length) { throw new InvalidOperationException("vectors must be same size"); }
			var sum = 0.0;
			for(int i=0; i<v1.Components.Length; i++)
			{
				sum += (v1.Components[i] * v2.Components[i]);
			}
			return sum;
		}

		public Angle CalculateAngleBetweenVectors(Vector v1, Vector v2)
		{
			var dotProduct = this.CalculateDotProduct(v1, v2);
			if(v1.Magnitude == 0 || v2.Magnitude == 0)
			{
				throw new InvalidOperationException("vectors must have non-zero magnitude");
			}
			var angleRadians = Math.Acos(dotProduct / (v1.Magnitude * v2.Magnitude));
			return new Angle(angleRadians, MeasurementType.Radians);
		}
	}
}
