using System;
using System.Collections.Generic;
using System.Text;

namespace Riddles.Geometry.Core
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
                sumX += point.X;
                sumY += point.Y;
			}
            return new Point(x: sumX / (double)numPoints, y: sumY / (double)numPoints);
		}

        public double CalculateCartesianDistance(Point point1, Point point2) {
            return Math.Sqrt(Math.Pow((point2.X - point1.X), 2) + Math.Pow((point2.Y - point1.Y), 2));
        }

        public double CalculateManhattanDistance(double r, double theta)
        {
            return r * (Math.Abs(Math.Cos(theta)) + Math.Abs(Math.Sin(theta)));
        }

        // in this case, there is a manhattan grid, with diagnoal paths connecting opposite corners
        // see https://fivethirtyeight.com/features/can-you-make-a-speedy-delivery/
        public double CalculateManhattanDistanceWithDiagonals(double r, double theta)
        {
            var verticalDistance = Math.Abs(Math.Sin(theta));
            var horizontalDistance = Math.Abs(Math.Cos(theta));
            if (horizontalDistance >= verticalDistance)
            {
                return r * (horizontalDistance + (Math.Sqrt(2) - 1) * verticalDistance);
            }
            return r * (verticalDistance + (Math.Sqrt(2) - 1) * horizontalDistance);
        }

    }
}
