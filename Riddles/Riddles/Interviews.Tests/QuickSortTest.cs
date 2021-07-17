using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace Interviews.Tests
{
	public class QuickSortTest
	{
		[TestCase(new int[] { 8, 12, 3, 5, 6, 6, 8, 2, 9, 11, 4, 3})]
		public void TestQuickSort(int[] array)
		{
			var quickSort = new QuickSort();
			quickSort.Sort(array);
			for(int i=1; i<array.Length; i++)
			{
				Assert.LessOrEqual(array[i - 1], array[i]);
			}
		}
	}
}
