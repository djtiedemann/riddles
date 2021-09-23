using System;
using System.Collections.Generic;
using System.Text;

namespace Riddles.Geometry
{
    public class Point
    {
        private CartesianCoordinate2D _cartesianCoordinate;
        private PolarCoordinate _polarCoordinate;
        public Point(CartesianCoordinate2D cartesianCoordinate) {
            this._cartesianCoordinate = cartesianCoordinate;
        }

        public Point(PolarCoordinate polarCoordinate)
		{
            this._polarCoordinate = polarCoordinate;
		}
        
        public CartesianCoordinate2D CartesianCoordinate
		{
            get
			{
                if(this._cartesianCoordinate == null)
				{
                    this._cartesianCoordinate = this._polarCoordinate.ToCartesianCoordinate2D();
				}
                return this._cartesianCoordinate;
			}
		}

        public PolarCoordinate PolarCoordinate
		{
			get
			{
                if(this._polarCoordinate == null)
				{
                    this._polarCoordinate = this._cartesianCoordinate.ToPolarCoordinate();
				}
                return this._polarCoordinate;
			}
		}
    }

    public class CartesianCoordinate2D
    {
        public CartesianCoordinate2D(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }
        public double X { get; }
        public double Y { get; }

        public PolarCoordinate ToPolarCoordinate()
		{
            if(this.X == 0 && this.Y == 0)
			{
                return new PolarCoordinate(r: 0, theta: 0);
			}
            var r = Math.Sqrt(this.X * this.X + this.Y * this.Y);
            var theta = ((this.Y < 0) ? Math.PI : 0) + Math.Acos(this.X / r);
            return new PolarCoordinate(r: r, theta: theta);
		}
    }

    public class PolarCoordinate {
        public PolarCoordinate(double r, double theta)
		{
            this.R = r;
            this.Theta = theta;
		}

        public double R { get; }
        public double Theta { get; }

        public CartesianCoordinate2D ToCartesianCoordinate2D()
		{
            return new CartesianCoordinate2D(x: R * Math.Cos(this.Theta), y: R * Math.Sin(this.Theta));
		}
    }
}
