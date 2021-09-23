using System;
using System.Collections.Generic;
using System.Text;

namespace Riddles.Geometry
{
    public class GeometryUtilities
    {
        /// <summary>
        /// Returns the distinct solutions to ax^2 + bx + c = 0
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public List<double> QuadraticFormula(double a, double b, double c)
        {
            if(4*a*c > Math.Pow(b, 2))
            {
                return new List<double>();
            }

            List<double> solution = new List<double>();
            double minYValue = (-1 * b - Math.Sqrt(Math.Pow(b, 2) - 4 * a * c)) / (2 * a);
            solution.Add(minYValue);
            
            if (4*a*c < Math.Pow(b, 2))
            {
                double maxYValue = (-1 * b + Math.Sqrt(Math.Pow(b, 2) - 4 * a * c)) / (2 * a);
                solution.Add(maxYValue);
            }
            return solution;
        }

        public Point CalculateCenterOfMass(List<Point> points)
		{
            var numPoints = points.Count;
            var sumX = 0.0;
            var sumY = 0.0;
            foreach(var point in points)
			{
                sumX += point.CartesianCoordinate.X;
                sumY += point.CartesianCoordinate.Y;
			}
            return new Point(
                new CartesianCoordinate2D(x: sumX / (double)numPoints, y: sumY / (double)numPoints)
            );
		}

    }
}
