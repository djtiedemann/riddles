using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Interviews
{
	public class MatrixMultiplicationOrder
	{
		public List<int> GetMultiplicationOrder(Tuple<int, int>[] matrixDimensions) {
			var memo = new Tuple<int, int>[matrixDimensions.Length, matrixDimensions.Length];
			var result = this.GetMatrixMultiplicationOrderInternal(matrixDimensions, 0, matrixDimensions.Length - 1, memo);
			return this.BacktrackResults(memo, 0, matrixDimensions.Length - 1);
		}

		private List<int> BacktrackResults(Tuple<int, int>[,] memo, int min, int max)
		{
			if(max - min <= 1)
			{
				return new List<int> { };
			}
			var split = memo[min, max].Item2;
			var leftList = this.BacktrackResults(memo, min, split);
			var rightList = this.BacktrackResults(memo, split + 1, max);
			return new List<int> { split }.Concat(leftList).Concat(rightList).ToList();
			  
		}

		private Tuple<int, int> GetMatrixMultiplicationOrderInternal(Tuple<int, int>[] matrixDimensions, int min, int max, Tuple<int, int>[,] memo) {
			if (memo[min, max] != null)
			{
				return memo[min, max];
			}
			if (max - min == 0)
			{
				var result = Tuple.Create(0, min);
				memo[min, max] = result;
				return memo[min, max];
			}
			if (max- min == 1)
			{
				var cost = this.GetNumMultiplicationsNeeded(matrixDimensions[min].Item1, matrixDimensions[min].Item2,
					matrixDimensions[max].Item1, matrixDimensions[max].Item2);
				memo[min, max] = Tuple.Create(cost, min);
				return memo[min, max];
			}
			var bestCost = int.MaxValue;
			var bestSplitVal = min;
			for (int i=min; i<max; i++)
			{
				var costLeftSubproblem = this.GetMatrixMultiplicationOrderInternal(matrixDimensions, min, i, memo).Item1;
				var costRightSubproblem = this.GetMatrixMultiplicationOrderInternal(matrixDimensions, i + 1, max, memo).Item1;
				var costThisOperation = this.GetNumMultiplicationsNeeded(matrixDimensions[min].Item1, matrixDimensions[i].Item2,
					matrixDimensions[i + 1].Item1, matrixDimensions[max].Item2);
				var cost = costLeftSubproblem + costRightSubproblem + costThisOperation;
				if(cost < bestCost)
				{
					bestCost = cost;
					bestSplitVal = i;
				}
			}
			memo[min, max] = Tuple.Create(bestCost, bestSplitVal);
			return memo[min, max];
		}

		private int GetNumMultiplicationsNeeded(int firstMatrixRows, int firstMatrixCols, int secondMatrixRows, int secondMatrixCols)
		{
			if(firstMatrixCols != secondMatrixRows) { return int.MaxValue;  }
			return firstMatrixRows * firstMatrixCols * secondMatrixCols;
		}
	}
}
