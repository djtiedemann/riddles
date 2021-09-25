using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Riddles.Geometry.Core
{
	public class RegularPolygon
	{
		private Angle _internalAngle;
		private Angle _externalAngle;
		private List<Point> _points;
		public RegularPolygon(int numSides, double sideLength, Point centroid, Angle rotation)
		{
			if(numSides < 3)
			{
				throw new InvalidOperationException("Polygons must have at least 3 sides");
			}
			this.NumSides = numSides;
			this.SideLength = sideLength;
			this.Centroid = centroid;
			this.Rotation = rotation;
		}

		public RegularPolygon(int numSides)
		{
			if (numSides < 3)
			{
				throw new InvalidOperationException("Polygons must have at least 3 sides");
			}
			this.NumSides = numSides;
			this.SideLength = 1;
			this.Centroid = new Point(0, 0);
			this.Rotation = new Angle(0, MeasurementType.Radians);
		}

		public Angle InternalAngle { 
			get
			{
				if(this._internalAngle == null)
				{
					this._internalAngle = new Angle(Math.PI - (2 * Math.PI / this.NumSides), MeasurementType.Radians);
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
					this._externalAngle = new Angle(2 * Math.PI / NumSides, MeasurementType.Radians);
				}
				return this._externalAngle;
			}
		}

		public List<Point> Vertices { 
			get	{
				if(this._points == null)
				{
					var radius = (double)this.SideLength / (2.0 * Math.Sin(Math.PI / (double)this.NumSides));
					var circle = new Circle(new Point(this.Centroid.X, this.Centroid.Y), radius);
					this._points = circle.GenerateNPointsEvenlyAroundCircle(this.NumSides, new Angle(0, MeasurementType.Radians));
				}
				return this._points;
			} 
		}

		public int NumSides { get; }
		public double SideLength { get; }
		public Point Centroid { get; }
		public Angle Rotation { get; }
	}
}
