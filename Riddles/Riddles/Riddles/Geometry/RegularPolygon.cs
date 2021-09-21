using System;
using System.Collections.Generic;
using System.Text;

namespace Riddles.Geometry
{
	public class RegularPolygon
	{
		private Angle _internalAngle;
		private Angle _externalAngle;
		public RegularPolygon(int numSides, int sideLength)
		{
			if(numSides < 3)
			{
				throw new InvalidOperationException("Polygons must have at least 3 sides");
			}
			this.NumSides = numSides;
			this.SideLength = sideLength;
		}

		public RegularPolygon(int numSides)
		{
			if (numSides < 3)
			{
				throw new InvalidOperationException("Polygons must have at least 3 sides");
			}
			this.NumSides = numSides;
		}

		public Angle InternalAngle { 
			get
			{
				if(this._internalAngle == null)
				{
					this._internalAngle = new Angle { Radians = Math.PI - (2 * Math.PI / this.NumSides) };
				}
				return this._internalAngle;
			}
		}

		public Angle ExternalAngle
		{
			get
			{
				if (this._externalAngle == null)
				{
					this._externalAngle = new Angle { Radians = 2*Math.PI / NumSides };
				}
				return this._externalAngle;
			}
		}

		public int NumSides { get; }
		public int SideLength { get; }
	}
}
