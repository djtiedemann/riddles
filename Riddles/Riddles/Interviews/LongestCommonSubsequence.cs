using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Interviews
{
	public class LongestCommonSubsequence
	{
		public string GetLongestCommonSubsequence(string x, string y)
		{
			var memoTable = new Tuple<int, int, int>[x.Length, y.Length];
			this.FillOutLongestCommonSubsequenceMemoTable(x, y, 0, 0, memoTable);
			var commonSubsequence = this.FindCommonSubsequenceFromMemoTable(x, y, memoTable, 0, 0, "");
			return commonSubsequence;
		}

		private string FindCommonSubsequenceFromMemoTable(string x, string y, Tuple<int, int, int>[,] memo, int xIndex, int yIndex, string commonSequence)
		{
			if(xIndex >= x.Length || yIndex >= y.Length)
			{
				return commonSequence;
			}
			var newX = memo[xIndex, yIndex].Item1;
			var newY = memo[xIndex, yIndex].Item2;
			if (memo[xIndex, yIndex].Item3 == 0 || (newX == xIndex && newY == yIndex))
			{
				return commonSequence;
			}
			if (newX == xIndex + 1 && newY == yIndex + 1) {
				commonSequence += x[xIndex];
			}
			var result = this.FindCommonSubsequenceFromMemoTable(x, y, memo, newX, newY, commonSequence);
			return result;
		}

		private int FillOutLongestCommonSubsequenceMemoTable(string x, string y, int i, int j, Tuple<int, int, int>[,] memo)
		{
			if(i >= x.Length && j >= y.Length)
			{
				return 0;
			}
			if(i >= x.Length)
			{
				return y.Length - j;
			}
			if(j >= y.Length)
			{
				return x.Length - i; 
			}
			if(memo[i,j] != null)
			{
				return memo[i, j].Item3;
			}
			List<Tuple<int, int, int>> possibleNextStatesWithCost = new List<Tuple<int, int, int>>();
			if(x[i] == y[j])
			{
				var longestSubsequenceSizeDeletingFromBoth = this.FillOutLongestCommonSubsequenceMemoTable(x, y, i + 1, j + 1, memo);
				possibleNextStatesWithCost.Add(Tuple.Create(i + 1, j + 1, longestSubsequenceSizeDeletingFromBoth));
			}			
			var longestSubsequenceSizeDeletingFromX = 1 + this.FillOutLongestCommonSubsequenceMemoTable(x, y, i + 1, j, memo);
			possibleNextStatesWithCost.Add(Tuple.Create(i + 1, j, longestSubsequenceSizeDeletingFromX));
			var longestSubsequenceSizeDeletingFromY = 1 + this.FillOutLongestCommonSubsequenceMemoTable(x, y, i, j + 1, memo);
			possibleNextStatesWithCost.Add(Tuple.Create(i, j + 1, longestSubsequenceSizeDeletingFromY));

			var minCost = int.MaxValue;
			Tuple<int, int, int> minState = null;
			foreach(var possibleNextState in possibleNextStatesWithCost)
			{
				if(possibleNextState.Item3 < minCost)
				{
					minCost = possibleNextState.Item3;
					minState = possibleNextState;
				}
			}
			memo[i, j] = minState;
			return memo[i, j].Item3;
		}
	}
}
