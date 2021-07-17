using System;
using System.Collections.Generic;
using System.Text;

namespace Interviews
{
	public class KthLargestElementSolver
	{
		Random _random;
		public KthLargestElementSolver()
		{
			this._random = new Random();
		}
		public int FindKthLargestElement(int[] array, int k)
		{
			return this.FindKthLargestElementInternal(array, array.Length - k, 0, array.Length-1);
		}

		public int FindKthLargestElementInternal(int[] array, int targetIndex, int minIndexInclusive, int maxIndexInclusive)
		{
			if(targetIndex == minIndexInclusive && targetIndex == maxIndexInclusive) { return array[targetIndex]; }
			var pivot = this._random.Next(minIndexInclusive, maxIndexInclusive + 1);

			// swap pivot with max value
			var temp = array[pivot];
			array[pivot] = array[maxIndexInclusive];
			array[maxIndexInclusive] = temp;

			int i = minIndexInclusive;
			for(int j=minIndexInclusive; j<=maxIndexInclusive; j++)
			{
				if(array[j] <= array[maxIndexInclusive])
				{
					temp = array[i];
					array[i] = array[j];
					array[j] = temp;
					i++;
				}
			}
			i--;
			if(i == targetIndex) { return array[targetIndex]; }
			if(targetIndex<i) { return this.FindKthLargestElementInternal(array, targetIndex, minIndexInclusive, i - 1);  }
			else { return this.FindKthLargestElementInternal(array, targetIndex, i + 1, maxIndexInclusive);  }
		}
	}
}
