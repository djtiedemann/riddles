using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Interviews;

namespace Interviews.Tests
{
	public class MedianOfTwoSortedArraysTest
	{
		[Test]
		public void TestFindMedianOfTwoSortedArrays()
		{
			var sortedArray1 = new int[] { 1, 2, 3, 8, 10 };
			var sortedArray2 = new int[] { 3, 4, 6, 7 };

			var sortedArraySolver = new MedianOfTwoSortedArrays();
			var median = sortedArraySolver.FindMedianOfTwoSortedArrays(sortedArray1, sortedArray2);
		}
	}
}
