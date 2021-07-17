using System;
using System.Collections.Generic;
using System.Text;

namespace Interviews
{
	public class MiniumumValueInRotatedArray
	{
		public int FindMinimumValueInArray(int[] rotatedSortedArray)
		{
			if (rotatedSortedArray.Length == 0) { return -1;  }
			if (rotatedSortedArray.Length == 1) { return rotatedSortedArray[0]; }
			var min = 0;
			var max = rotatedSortedArray.Length - 1;
			while (min < max)
			{
				var midPoint = min + (max - min) / 2;
				if (rotatedSortedArray[midPoint] < rotatedSortedArray[min]) {
					min = min + 1;
					max = midPoint;
				} else if(rotatedSortedArray[max] < rotatedSortedArray[midPoint])
				{
					min = midPoint + 1;					
				} else
				{
					return rotatedSortedArray[min];
				}				
			}
			return rotatedSortedArray[min];
		}



		//private int FindMinimumValueInRotatedSortedArrayInternalInternal(int[] rotatedSortedArray, int minIndex, int maxIndex) {
		//	if(maxIndex - minIndex <= 1)
		//	{
		//		return Math.Min(rotatedSortedArray[minIndex], rotatedSortedArray[maxIndex]);
		//	}
		//	var lowerIndexToCheck = minIndex + (maxIndex - minIndex + 1) * 1 / 4;
		//	var higherIndexToCheck = minIndex + (maxIndex - minIndex + 1) * 3 / 4;

		//	var leftValue = rotatedSortedArray[lowerIndexToCheck];
		//	var rightValue = rotatedSortedArray[higherIndexToCheck];
		//	if(leftValue > rightValue)
		//	{
		//		return this.FindMinimumValueInRotatedSortedArrayInternalInternal(
		//			rotatedSortedArray, Math.Min(lowerIndexToCheck + 1, higherIndexToCheck), higherIndexToCheck);
		//	}
		//	var minimumValueLeftSubarray = this.FindMinimumValueInRotatedSortedArrayInternalInternal(rotatedSortedArray, minIndex, lowerIndexToCheck);
		//	var minimumValueRightSubarray = this.FindMinimumValueInRotatedSortedArrayInternalInternal(
		//		rotatedSortedArray, Math.Min(higherIndexToCheck + 1, maxIndex), maxIndex);
		//	return Math.Min(minimumValueLeftSubarray, minimumValueRightSubarray);
		//}
	}
}
