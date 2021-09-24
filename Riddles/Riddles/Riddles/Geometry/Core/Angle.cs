using System;
using System.Collections.Generic;
using System.Text;

namespace Riddles.Geometry.Core
{
	public enum MeasurementType
	{
		Radians = 1,
		Degrees = 2
	}

	public class Angle
	{
		public Angle(double measurement, MeasurementType measurementType)
		{
			if (measurementType == MeasurementType.Radians) { 
				this.Radians = measurement;
				this.Degrees = this.Radians * 360 / (2 * Math.PI);
			}
			else { 
				this.Degrees = measurement;
				this.Radians = this.Degrees * Math.PI * 2 / 360;
			}
		}

		public double Degrees {
			get;
		}

		public double Radians
		{
			get;
		}
	}
}
