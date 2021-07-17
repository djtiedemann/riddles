using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Interviews;

namespace Interviews.Tests
{
	public class MergeSortTest
	{
		[TestCase(new int[] { 5, 2, 4, 7, 6, 1, 3}, new int[] { 1, 2, 3, 4, 5, 6, 7 })]
		[TestCase(new int[] { 7, 5, 11, 9, 4, 2, 3, 10, 1, 8, 4, 6 }, new int[] { 1, 2, 3, 4, 4, 5, 6, 7, 8, 9, 10, 11 })]

		public void TestMergeSort(int[] array, int[] expectedResults)
		{
			var mergeSorter = new MergeSort();
			mergeSorter.Sort(array);
			Assert.AreEqual(array.Length, expectedResults.Length);
			for(int i=0; i<array.Length; i++)
			{
				Assert.AreEqual(expectedResults[i], array[i]);
			}
		}
	}
}
