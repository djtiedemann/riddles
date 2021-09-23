using System;
using System.Collections.Generic;
using System.Text;

namespace Riddles.Geometry
{
    public class Point
    {
        private CartesianCoordinate2D _cartesianCoordinate;
        private PolarCoordinate _polarCoordinate;
        public Point(double x, double y) {
            this._cartesianCoordinate = new CartesianCoordinate2D(x, y);
        }

        public Point(double r, Angle theta)
		{
            this._polarCoordinate = new PolarCoordinate(r, theta);
		}
        
        public double X
		{
            get
            {
                if (this._cartesianCoordinate == null)
                {
                    this._cartesianCoordinate = this._polarCoordinate.ToCartesianCoordinate2D();
                }
                return this._cartesianCoordinate.X;
            }
        }

        public double Y
		{
            get
            {
                if (this._cartesianCoordinate == null)
                {
                    this._cartesianCoordinate = this._polarCoordinate.ToCartesianCoordinate2D();
                }
                return this._cartesianCoordinate.Y;
            }
        }

        public double R
		{
			get
			{
                if(this._polarCoordinate == null)
				{
                    this._polarCoordinate = this._cartesianCoordinate.ToPolarCoordinate();
				}
                return this._polarCoordinate.R;
			}
		}

        public Angle Theta
        {
            get
            {
                if (this._polarCoordinate == null)
                {
                    this._polarCoordinate = this._cartesianCoordinate.ToPolarCoordinate();
                }
                return this._polarCoordinate.Theta;
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
                return new PolarCoordinate(r: 0, theta: new Angle { Radians = 0 });
			}
            var r = Math.Sqrt(this.X * this.X + this.Y * this.Y);
            var theta = new Angle { Radians = ((this.Y < 0) ? Math.PI : 0) + Math.Acos(this.X / r) };
            return new PolarCoordinate(r: r, theta: theta);
		}
    }

    public class PolarCoordinate {
        public PolarCoordinate(double r, Angle theta)
		{
            this.R = r;
            this.Theta = theta;
		}

        public double R { get; }
        public Angle Theta { get; }

        public CartesianCoordinate2D ToCartesianCoordinate2D()
		{
            return new CartesianCoordinate2D(x: R * Math.Cos(this.Theta.Radians), y: R * Math.Sin(this.Theta.Radians));
		}
    }
}
