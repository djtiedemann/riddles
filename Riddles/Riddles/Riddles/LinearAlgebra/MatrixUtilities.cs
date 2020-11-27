using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Riddles.LinearAlgebra
{
	public class MatrixUtilities
	{
		/// <summary>
		/// Solves n linear equations with n unknowns by using the matrix elimination method.
		/// This involves eliminating variables in order to get a matrix U where the lower triangle of the matrix is all 0s
		/// The last equation will have only the last variable in the equation and can be solved. The second to last equation adds on
		/// one more variable and so on until the first equation which has all n variables.
		/// 
		/// In order to solve this, the matrix must be invertible. If the matrix isn't invertible, it will return null.
		/// 
		/// The elimination method looks at pivots, which are values along the row = column diagonal. The pivot values must all be non-zero,
		/// so there is some trial and error in how we order the rows.
		/// 
		/// So we solve the pivots one at a time. We mark every row that could be used for a pivot in this location and loop through them.
		/// Once we have a candidate pivot, we will eliminate the pivot variable from all lower rows. We then search for the next pivot.
		/// If at any point we cannot find a candidate pivot, then we go back and try a new pivot in the previous step until we've exhausted all
		/// pivots and determined that the matrix isn't invertible.
		///
		/// Once we've found all of the pivots, we use back substitution to solve for each of the unknowns. 
		/// </summary>
		/// <param name="matrix">The matrix containing n equations and n unknowns</param>
		/// <param name="constants">The constant terms in each of the n equations</param>
		/// <returns></returns>
		public double[] SolveNEquationsNUnknowns(double[][] matrix, double[] constants)
		{
			if(matrix.Length != constants.Length)
			{
				throw new InvalidOperationException("You must input the same number of constants as equations");
			}
			for(int row=0; row<matrix.Length; row++)
			{
				if(matrix[row].Length != matrix.Length)
				{
					throw new InvalidOperationException("Each equation must have the same number of unknowns entered");
				}
			}
			var (resultMatrix, resultConstants) = this.PerformEliminationStartingAtRowIColI(matrix, constants, 0);
			if(resultMatrix == null)
			{
				return null;
			}
			return this.PerformBackSubstitution(resultMatrix, resultConstants);
		}

		private (double[][] resultMatrix, double[] resultConstants) PerformEliminationStartingAtRowIColI(
			double [][] matrix, double [] constants, int pivotRowAndCol)
		{
			if(pivotRowAndCol == matrix.Length - 1)
			{
				// if we've reached the last pivot and there is a non-zero value, we've successfully computed the matrix
				// if it's zero, we need to try new pivots
				if (matrix[pivotRowAndCol][pivotRowAndCol] != 0)
				{
					return (matrix, constants);
				}
				return (null, null);
			}

			// we may need to swap rows in order to get a pivot that works			
			var possiblePivotRows = new List<int> { };
			for(int row=pivotRowAndCol; row < matrix.Length; row++){
				if(matrix[row][pivotRowAndCol] != 0)
				{
					possiblePivotRows.Add(row);
				}
			}

			if (!possiblePivotRows.Any())
			{
				return (null, null);
			}

			foreach(var pivotRow in possiblePivotRows)
			{
				var copiedMatrix = this.CopyMatrix(matrix);
				var copiedConstants = this.CopyConstants(constants);

				// need to swap the rows so that the pivot position isn't zero
				if (pivotRow != pivotRowAndCol)
				{
					var temp = copiedMatrix[pivotRow];
					copiedMatrix[pivotRow] = copiedMatrix[pivotRowAndCol];
					copiedMatrix[pivotRowAndCol] = temp;

					var tempValue = copiedConstants[pivotRow];
					copiedConstants[pivotRow] = copiedConstants[pivotRowAndCol];
					copiedConstants[pivotRowAndCol] = tempValue;
				}

				// perform elimination on the remaining rows
				for(int rowToEliminate=pivotRowAndCol+1; rowToEliminate < matrix.Length; rowToEliminate++)
				{
					var coefficient = (-1 * copiedMatrix[rowToEliminate][pivotRowAndCol]) / copiedMatrix[pivotRowAndCol][pivotRowAndCol];
					for(int col=pivotRowAndCol; col<matrix.Length; col++)
					{
						copiedMatrix[rowToEliminate][col] = copiedMatrix[rowToEliminate][col] + coefficient * copiedMatrix[pivotRowAndCol][col];
					}
					copiedConstants[rowToEliminate] = copiedConstants[rowToEliminate] + coefficient * copiedConstants[pivotRowAndCol];
				}

				// increase the pivot and call this function recursively
				var (resultMatrix, resultConstants) = this.PerformEliminationStartingAtRowIColI(copiedMatrix, copiedConstants, pivotRowAndCol + 1);
				// if this using this pivot led us to a result, return it. Otherwise choose a different pivot
				if(resultMatrix != null)
				{
					return (resultMatrix, resultConstants);
				}
			}
			return (null, null);
		}

		private double[] PerformBackSubstitution(double[][] matrixU, double[] constants)
		{
			double[] solution = new double[constants.Length];

			for(int row = constants.Length-1; row>=0; row--)
			{
				var coefficientAtPivot = matrixU[row][row];
				var resultOfOtherVariables = 0.0;
				for(int col=row+1; col<constants.Length; col++)
				{
					resultOfOtherVariables += matrixU[row][col] * solution[row];
				}
				solution[row] = (constants[row] - resultOfOtherVariables) / coefficientAtPivot;
			}
			return solution;
		}


		private double[][] CopyMatrix(double[][] matrix)
		{
			var copiedMatrix = new double[matrix.Length][];
			for (int row = 0; row < matrix.Length; row++)
			{
				var newRow = new double[matrix.Length];
				for (int col=0; col<matrix.Length; col++)
				{
					newRow[col] = matrix[row][col];
				}
				copiedMatrix[row] = newRow;
			}
			return copiedMatrix;
		}

		private double[] CopyConstants(double[] constants)
		{
			var copiedConstants = new double[constants.Length];
			for(int i=0; i<constants.Length; i++)
			{
				copiedConstants[i] = constants[i];
			}
			return copiedConstants;
		}
	}
}
