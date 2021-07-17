using System;
using System.Collections.Generic;
using System.Text;

namespace Interviews
{
	public class LargestMissingPositiveIntegerSolver
	{
		public int FindLargestMissingPositiveInteger(int[] array)
		{
			var largestPossibleMissingInteger = array.Length + 1;
			bool hasSeenOne = false;
			for(int i=0; i<array.Length; i++)
			{
				if(array[i] == 1)
				{
					hasSeenOne = true;
					continue;
				}
				if(array[i] <= 0 || array[i] >= largestPossibleMissingInteger)
				{
					array[i] = 1;
				}
			}
			if (!hasSeenOne) { return 1; }
			for(int i=0; i<array.Length; i++)
			{
				var indexToNegate = Math.Abs(array[i]) - 1;
				if (array[indexToNegate] > 0) { array[indexToNegate] *= -1; }
			}
			for(int i=0; i<array.Length; i++)
			{
				if(array[i] > 0) { return i + 1; }
			}
			return largestPossibleMissingInteger;
		}
	}
}
