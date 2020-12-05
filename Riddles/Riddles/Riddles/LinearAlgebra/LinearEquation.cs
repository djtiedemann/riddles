using System;
using System.Collections.Generic;
using System.Text;

namespace Riddles.LinearAlgebra
{
	public class LinearEquation
	{
		private List<LinearTerm> _terms;
		private double _constant;

		// the combination of the linear terms equals the constant
		// a0*x0 + a1*x1 + ... +an*xn = c
		// terms should have variableIds starting at 0 and increasing
		public LinearEquation(List<LinearTerm> terms, double constant)
		{
			this._terms = terms;
			this._constant = constant;
		}

		public List<LinearTerm> Terms
		{
			get { return this._terms;  }
		}

		public double Constant
		{
			get { return this._constant;  }
		}
	}
}
