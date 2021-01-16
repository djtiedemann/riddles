using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Riddles.Optimization
{
	public class MysteriousNumberHunt
	{
		public int[,] SolveMysteryNumbers(int[] bottomRowSolutions, int[] rightSideSolutions)
		{
			var solution = new int[rightSideSolutions.Length, bottomRowSolutions.Length];
			var numColumns = bottomRowSolutions.Length;
			int numRows = rightSideSolutions.Length;
			solution[0,0] = 1;
			var lastIndexPlaced = 0;
			bool provedSolutionImpossible = false;
			while(!provedSolutionImpossible)
			{
				(int lastRowPlaced, int lastColPlaced) = (lastIndexPlaced / numColumns, lastIndexPlaced % numColumns);
				bool isSolutionStillValid = this.isSolutionStillValid(solution, bottomRowSolutions, rightSideSolutions, lastRowPlaced, lastColPlaced);
				if (isSolutionStillValid)
				{
					if(lastIndexPlaced == numRows*numColumns - 1)
					{
						if(this.isSolutionFinal(solution, bottomRowSolutions, rightSideSolutions))
						{
							return solution;
						}
						throw new InvalidOperationException("The solution should have been valid, but the validator is saying it is not");
					}
					this.advanceValidSolution(solution, lastIndexPlaced, numColumns);
					lastIndexPlaced++;
				}
				else
				{
					lastIndexPlaced = this.backtrackInvalidSolution(solution, lastIndexPlaced, numColumns);
					provedSolutionImpossible = lastIndexPlaced < 0;
				}
			}
			// if the solution is impossible, return null
			return null;
		}

		public int backtrackInvalidSolution(int[,] solution, int lastIndexPlaced, int numColumns)
		{
			while (lastIndexPlaced >= 0)
			{
				(int currRow, int currCol) = (lastIndexPlaced / numColumns, lastIndexPlaced % numColumns);
				// if we can try a new solution by increasing the number, do that
				if(solution[currRow, currCol] != 9)
				{
					solution[currRow, currCol]++;
					return lastIndexPlaced;
				}
				// otherwise, clear this row, and keep backtracking
				solution[currRow, currCol] = 0;
				lastIndexPlaced--;
			}
			return lastIndexPlaced;
		}

		public int advanceValidSolution(int[,] solution, int lastIndexPlaced, int numColumns)
		{
			var indexToPlace = lastIndexPlaced + 1;
			(int nextRow, int nextCol) = (indexToPlace / numColumns, indexToPlace % numColumns);
			solution[nextRow, nextCol] = 1;
			return indexToPlace;
		}

		public bool isSolutionStillValid(int[,] solution, int[] bottomRowSolutions, int[] rightSideSolutions, int lastRowPlaced, int lastColumnPlaced)
		{
			// If none of the previous digits broke the solution, we need to make sure this digit doesn't break the solution
			// In order to be a potentially valid solution, the following conditions must hold
			// 1) if a row or column is complete, the digits placed in that row or column must multiply to the solution
			// 2) if a row or column is not complete, the digits placed in that row or column must divide evenly into the solution
			// 3) if a row or column is not complete, the current product can't be so low that adding in 9s for every other digit will still be too small
			//
			// If any of those conditions aren't met, the solution isn't valid and we need to change one of the existing digits
			// Otherwise, we can add another digit
			var rowProduct = Enumerable.Range(0, lastColumnPlaced + 1).Aggregate(1, (agg,c) => agg*solution[lastRowPlaced,c]);
			if(rightSideSolutions[lastRowPlaced] % rowProduct != 0)	{
				return false;
			} 
			if(lastColumnPlaced == bottomRowSolutions.Length - 1 && rowProduct != rightSideSolutions[lastRowPlaced])
			{
				return false;
			}
			if (lastColumnPlaced != bottomRowSolutions.Length - 1)
			{
				var highestPossibleProduct = ((int)Math.Pow(9, bottomRowSolutions.Length - 1 - lastColumnPlaced)) * rowProduct;
				if(rightSideSolutions[lastRowPlaced] > highestPossibleProduct)
				{
					return false;
				}
			}
			var colProduct = Enumerable.Range(0, lastRowPlaced + 1).Aggregate(1, (agg, r) => agg * solution[r, lastColumnPlaced]);
			if (bottomRowSolutions[lastColumnPlaced] % colProduct != 0)
			{
				return false;
			}
			if (lastRowPlaced == rightSideSolutions.Length - 1 && colProduct != bottomRowSolutions[lastColumnPlaced])
			{
				return false;
			}
			if (lastRowPlaced != rightSideSolutions.Length - 1)
			{
				var highestPossibleProduct = ((int)Math.Pow(9, rightSideSolutions.Length - 1 - lastRowPlaced)) * colProduct;
				if (bottomRowSolutions[lastColumnPlaced] > highestPossibleProduct)
				{
					return false;
				}
			}
			return true;
		}

		// the solution is valid if every row and column multiply to the correct values
		public bool isSolutionFinal(int[,] solution, int[] bottomRowSolutions, int[] rightSideSolutions)
		{
			for(int row = 0; row<rightSideSolutions.Length; row++)
			{
				var rowProduct = Enumerable.Range(0, bottomRowSolutions.Length - 1).Aggregate(1, (agg, c) => agg * solution[row, c]);
				if(rowProduct != rightSideSolutions[row])
				{
					return false;
				}
			}
			for (int col = 0; col < bottomRowSolutions.Length; col++)
			{
				var colProduct = Enumerable.Range(0, rightSideSolutions.Length - 1).Aggregate(1, (agg, r) => agg * solution[r, col]);
				if (colProduct != bottomRowSolutions[col])
				{
					return false;
				}
			}
			return true;
		}
	}
}
