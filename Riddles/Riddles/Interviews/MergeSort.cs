using System;
using System.Collections.Generic;
using System.Text;

namespace Interviews
{
	public class MergeSort
	{
		public void Sort(int[] array)
		{
			this.MergeSortIterative(array);
		}

		private void MergeSortHelper(int[] array, int lowIndex, int highIndex) { 
			if(lowIndex == highIndex)
			{
				return;
			}
			var middleIndex = (highIndex + lowIndex) / 2;
			this.MergeSortHelper(array, lowIndex, middleIndex);
			this.MergeSortHelper(array, middleIndex + 1, highIndex);
			this.Merge(array, lowIndex, middleIndex, middleIndex + 1, highIndex);
		}

		private void MergeSortIterative(int[] array)
		{
			var partitionSize = 1;
			while(partitionSize < array.Length)
			{
				partitionSize *= 2;
				int minIndex = 0;
				int maxIndex = Math.Min(partitionSize - 1, array.Length - 1);
				while (minIndex < array.Length)
				{					
					var middleIndex = Math.Min(minIndex + partitionSize/2 - 1, maxIndex);
					this.Merge(array, minIndex, middleIndex, middleIndex + 1, maxIndex);
					minIndex = maxIndex + 1;
					maxIndex = Math.Min(maxIndex + partitionSize, array.Length - 1);
				}				
			}
		}

		private void Merge(int[] array, int lowIndexLeft, int highIndexLeft, int lowIndexRight, int highIndexRight) {
			int[] results = new int[highIndexRight - lowIndexLeft + 1];
			int leftArrayIterator = lowIndexLeft;
			var rightArrayIterator = lowIndexRight;
			for (int i = 0; i < results.Length; i++)
			{
				var leftValue = leftArrayIterator <= highIndexLeft ? array[leftArrayIterator] : int.MaxValue;
				var rightValue = rightArrayIterator <= highIndexRight ? array[rightArrayIterator] : int.MaxValue;
				if (leftValue <= rightValue) {
					results[i] = array[leftArrayIterator];
					leftArrayIterator++;
				} else
				{
					results[i] = array[rightArrayIterator];
					rightArrayIterator++;
				}
			}
			for(int i=0; i<results.Length; i++)
			{
				array[i + lowIndexLeft] = results[i];
			}
		}
	}
}
