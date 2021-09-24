using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Riddles.Geometry.Core
{
    public class Vector
    {
        private double? _magnitude;
        public Vector(double[] vector) {
            this.Components = vector;
        }
    
        public double[] Components { get; }

        public double Magnitude
		{
            get
			{                
				if (!this._magnitude.HasValue) {
                    this._magnitude = Math.Sqrt(
                        this.Components.Select(c => c*c).Sum()
                    );
                }
                return this._magnitude.Value;
			}
		}

        public Vector ToUnitVector()
		{
            return new Vector(this.Components.Select(c => c / this.Magnitude).ToArray());
		}
    }
}