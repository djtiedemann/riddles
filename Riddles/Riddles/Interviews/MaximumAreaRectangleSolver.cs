using System;
using System.Collections.Generic;
using System.Text;

namespace Interviews
{
	public class MaximumAreaRectangleSolver
	{
		public int FindMaxiumumSizeRectangle(int[,] matrix)
		{
			int[] currentHistogram = new int[matrix.GetLength(1)];
			var maxArea = 0;
			for(int i=0; i<matrix.GetLength(0); i++)
			{
				for(int j=0; j<matrix.GetLength(1); j++)
				{
					if(matrix[i, j] == 0)
					{
						currentHistogram[j] = 0;
					} else
					{
						currentHistogram[j] += matrix[i, j];
					}
				}
				var areaForThisRow = this.GetMaxAreaUnderHistogram(currentHistogram);
				if(areaForThisRow > maxArea)
				{
					maxArea = areaForThisRow;
				}
			}
			return maxArea;
		}

		public int GetMaxAreaUnderHistogram(int[] histogram)
		{
			Stack<Tuple<int, int>> stackOfPastRows = new Stack<Tuple<int, int>>();
			var maxArea = 0;
			for(int i=0; i<histogram.Length; i++)
			{
				var indexToUpdate = i;
				while (stackOfPastRows.Count > 0 && stackOfPastRows.Peek().Item1 > histogram[i]){
					var previousElement = stackOfPastRows.Pop();
					var area = previousElement.Item1 * (i - previousElement.Item2);
					if(area > maxArea)
					{
						maxArea = area;
					}
					indexToUpdate--;
				}
				if(histogram[i] > 0 && (stackOfPastRows.Count == 0 || stackOfPastRows.Peek().Item1 < histogram[i]))
				{
					stackOfPastRows.Push(Tuple.Create(histogram[i], indexToUpdate));
				}				
			}

			while(stackOfPastRows.Count > 0)
			{
				var previousElement = stackOfPastRows.Pop();
				var area = previousElement.Item1 * (histogram.Length - previousElement.Item2);
				if (area > maxArea)
				{
					maxArea = area;
				}
			}
			return maxArea;
		}
	}
}
