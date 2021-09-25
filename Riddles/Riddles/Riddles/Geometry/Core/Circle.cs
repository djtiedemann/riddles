using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Riddles.Geometry.Core
{
    public class Circle
    {
        public Circle(Point center, double radius) {
            this.Center = center;
            this.Radius = radius;
        }

        public Point Center { get; }
        public double Radius { get; }

        public List<Point> GenerateNPointsEvenlyAroundCircle(int n, Angle initialAngle)
		{
            var increment = Math.PI * 2 / n;
            var angles = Enumerable.Range(0, n).Select(i => initialAngle.Radians + i * increment);
            return angles.Select(a => new Point(
                x: this.Center.X + Math.Cos(a) * this.Radius, 
                y: this.Center.Y + Math.Sin(a) * this.Radius
            )).ToList();
		}
    }
}
