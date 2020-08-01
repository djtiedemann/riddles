using System;
using System.Collections.Generic;
using System.Text;

namespace Riddles.Geometry
{
    public class GeometryUtilities
    {
        public List<double> QuadraticFormula(double a, double b, double c)
        {
            if(4*a*c > Math.Pow(b, 2))
            {
                return new List<double>();
            }

            double maxYValue = (-1 * b + Math.Sqrt(Math.Pow(b, 2) - 4 * a * c)) / (2 * a);
            List<double> solution = new List<double>();
            solution.Add(maxYValue);
            if (4*a*c < Math.Pow(b, 2))
            {
                double minYValue = (-1 * b + Math.Sqrt(Math.Pow(b, 2) - 4 * a * c)) / (2 * a);
                solution.Add(minYValue);
            }
            return solution;
        }
    }
}
