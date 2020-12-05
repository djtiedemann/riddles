using System;
using System.Collections.Generic;
using System.Text;

namespace Riddles.LinearAlgebra
{
	public class LinearTerm
	{
		private double _coefficient { get; set; }
		private int _variableId { get; set; }

		public LinearTerm(double coefficient, int variableId)
		{
			this._coefficient = coefficient;
			this._variableId = variableId;
		}

		public double Coefficient
		{
			get { return this._coefficient; }
		}
		public int VariableId
		{
			get { return this._variableId; }
		}
	}
}
