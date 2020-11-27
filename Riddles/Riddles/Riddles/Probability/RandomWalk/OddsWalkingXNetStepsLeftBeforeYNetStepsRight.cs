using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Riddles.LinearAlgebra;

namespace Riddles.Probability.RandomWalk
{
	public class OddsWalkingXNetStepsLeftBeforeYNetStepsRight
	{
		private Dictionary<int, double[]> _probabilitiesCacheForRangeOfSteps;
		private MatrixUtilities _matrixUtilities;
		public OddsWalkingXNetStepsLeftBeforeYNetStepsRight()
		{
			
			this._probabilitiesCacheForRangeOfSteps = new Dictionary<int, double[]>();
			this._matrixUtilities = new MatrixUtilities();
		}

		/// <summary>
		/// The idea here is that if we're doing a random walk, at some point we will either reach a point where we are x steps to the left
		/// of the origin or y steps to the right of the origin. This computes the odds that the left one will happen first.
		/// 
		/// This basically boils down to a system of linear equations with x + y -1 equations and x + y - 1 unknowns.
		/// If you think of each step as a state, the state where you are one to the right of the space you'll end up in is 
		/// 1/2 + 1/2*(probability when you are 2 steps away)
		/// The probability when you are 2 steps away is 
		/// 1/2*(probability when you are 1 step away) + 1/2*(probability when you are 3 steps away)
		/// The probability when you are 3 steps away is
		/// 1/2*(probability when you are 2 steps away) + (1/2)*probability when you are 4 steps away
		/// 
		/// The final linear equation asserts that all of the probabilities must sum to 1.
		/// 
		/// So you can build a matrix to solve the problem
		/// </summary>
		/// <param name="numStepsLeft"></param>
		/// <param name="numStepsRight"></param>
		/// <returns></returns>
		public double? GetProbabilityOfWalkingXStepsLeftBeforeYStepsRight(int numStepsLeft, int numStepsRight)
		{
			var numStepsWithinRange = numStepsLeft + numStepsRight - 1;
			if (this._probabilitiesCacheForRangeOfSteps.ContainsKey(numStepsWithinRange))
			{
				return this._probabilitiesCacheForRangeOfSteps[numStepsWithinRange][numStepsLeft];
			}

			if (numStepsLeft < 0 || numStepsRight < 0)
			{
				return null;
			}
			if(numStepsLeft == 0 && numStepsRight == 0)
			{
				return null;
			}
			if(numStepsLeft == 0)
			{
				return 1;
			}
			if(numStepsRight == 0)
			{
				return 0;
			}
			if(numStepsLeft == 1 && numStepsRight == 1)
			{
				return 0.5;
			}
			this._probabilitiesCacheForRangeOfSteps[numStepsWithinRange] = this.GetProbabilityDistributionForARangeOfNSteps(numStepsWithinRange);
			return this._probabilitiesCacheForRangeOfSteps[numStepsWithinRange][numStepsLeft];
		}

		public double[] GetProbabilityDistributionForARangeOfNSteps(int numStepsWithinRange)
		{
			if (numStepsWithinRange <= 0)
			{
				return null;
			}
			if(numStepsWithinRange == 1)
			{
				return new double[] { 0.5 };
			}
			var equations = new double[numStepsWithinRange][];
			var constants = new double[numStepsWithinRange];
			// the first equation ensures that all probabilities sum to 1
			equations[0] = Enumerable.Range(1, numStepsWithinRange).Select(s => 1.0).ToArray();
			constants[0] = 1;
			// the second equation: x_1 - (1/2)x_2 = 1/2
			equations[1] = Enumerable.Range(1, numStepsWithinRange).Select(s => 0.0).ToArray();
			equations[1][0] = 1;
			equations[1][1] = -0.5;
			constants[1] = 0.5;

			for (int i = 2; i < numStepsWithinRange; i++)
			{
				constants[i] = 0;
				var equation = new double[numStepsWithinRange];
				for (int j = 0; j < numStepsWithinRange; j++)
				{
					if (j == i - 2 || j == i)
					{
						equation[j] = -0.5;
					}
					else if (j == i - 1)
					{
						equation[j] = 1;
					}
					else
					{
						equation[i] = 0.0;
					}
				}
				equations[i] = equation;
			}
			return this._matrixUtilities.SolveNEquationsNUnknowns(equations, constants);
		}
	}
}
