using System;
using System.Collections.Generic;
using System.Text;
using Interviews;
using NUnit.Framework;

namespace Interviews.Tests
{
	public class LargestMissingPositiveIntegerSolverTest
	{
		[TestCase(new int[] { 1, 6, 3, 3, 9, 11, 2}, 4)]
		[TestCase(new int[] { 1, 2, 0}, 3)]
		[TestCase(new int[] { 3, 4, -1, 1}, 2)]
		[TestCase(new int[] { 7, 8, 9, 11, 12 }, 1)]
		public void TestFindLargestMissingPositiveInteger(int[] array, int expectedLargestMissingPositiveInteger)
		{
			var largestMissingPositiveIntegerSolver = new LargestMissingPositiveIntegerSolver();
			var actualLargestMissingPositiveInteger = largestMissingPositiveIntegerSolver.FindLargestMissingPositiveInteger(array);
			Assert.AreEqual(expectedLargestMissingPositiveInteger, actualLargestMissingPositiveInteger);
		}
	}
}
