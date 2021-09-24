using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Riddles.Geometry.Core
{
    public class Circle
    {
        public Point Center { get; set; }
        public double Radius { get; set; }

        public List<Point> GenerateNPointsEvenlyAroundCircle(int n, double initialAngle)
		{
            var increment = Math.PI * 2 / n;
            var angles = Enumerable.Range(0, n).Select(i => initialAngle + i * increment);
            return angles.Select(a => new Point(
                x: this.Center.X + Math.Cos(a) * this.Radius, 
                y: this.Center.Y + Math.Sin(a) * this.Radius
            )
            ).ToList();
		}
    }
}
