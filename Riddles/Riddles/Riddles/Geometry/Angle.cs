using System;
using System.Collections.Generic;
using System.Text;

namespace Riddles.Geometry
{
	public class Angle
	{
		private double _degrees;
		private double _radians;
		public double Degrees {
			get { 
				return this._degrees; 
			}
			set
			{
				this._degrees = value;
				this._radians = this._degrees * Math.PI * 2 / 360;
			}
		}

		public double Radians
		{
			get
			{
				return this._radians;	
			}
			set 
			{
				this._radians = value;
				this._degrees = this._radians * 360 / (2 * Math.PI);
			}
		}
	}
}
