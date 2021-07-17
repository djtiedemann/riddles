using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Interviews;

namespace Interviews.Tests
{
	public class MinimumValueInRotatedSortedArrayTest
	{
		[TestCase(new int[] { 3, 4, 5, 1, 2}, 1)]
		[TestCase(new int[] { 4, 5, 6, 7, 0, 1, 2}, 0)]
		[TestCase(new int[] { 11, 13, 15, 17}, 11)]
		[TestCase(new int[] { 7, 0}, 0)]
		public void TestFindMinimumValueInArray(int[] rotatedSortedArray, int expected)
		{
			var rotatedSortedArraySolver = new MiniumumValueInRotatedArray();
			var actual = rotatedSortedArraySolver.FindMinimumValueInArray(rotatedSortedArray);
			Assert.AreEqual(expected, actual);
		}
	}
}
