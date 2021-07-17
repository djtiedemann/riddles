using System;
using System.Collections.Generic;
using System.Text;

namespace Interviews
{
	public class QuickSort
	{
		Random random;
		public QuickSort()
		{
			random = new Random();
		}

		public void Sort(int[] array)
		{
			this.SortInternal(array, 0, array.Length - 1);			
		}

		private void SortInternal(int[] array, int minIndexInclusive, int maxIndexInclusive)
		{
			if(minIndexInclusive >= maxIndexInclusive)
			{
				return;
			}
			var pivot = random.Next(maxIndexInclusive - minIndexInclusive + 1) + minIndexInclusive;
			// put the pivot in the last position
			var temp = array[pivot];
			array[pivot] = array[maxIndexInclusive];
			array[maxIndexInclusive] = temp;
			var i = minIndexInclusive;
			// move all values less than or equal to the pivot to the left, all values greater than or equal to the pivot right
			for(int j=minIndexInclusive; j<maxIndexInclusive; j++)
			{
				if(array[j] <= array[maxIndexInclusive])
				{
					temp = array[i];
					array[i] = array[j];
					array[j] = temp;
					i++;
				}
			}
			temp = array[maxIndexInclusive];
			array[maxIndexInclusive] = array[i];
			array[i] = temp;
			this.SortInternal(array, minIndexInclusive, i - 1);
			this.SortInternal(array, i + 1, maxIndexInclusive);
		}
	}
}
