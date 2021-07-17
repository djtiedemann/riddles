using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Interviews;

namespace Interviews.Tests
{
	public class KthLargestElementSolverTest
	{
		[TestCase(new int[] { 8, 12, 3, 5, 6, 6, 8, 2, 9, 11, 4, 3 })]
		public void TestKthLargestElementSolver(int[] array)
		{
			var kthLargestElementSolver = new KthLargestElementSolver();
			var results = new int[array.Length];
			for(int i=1; i<array.Length; i++)
			{
				results[i - 1] = kthLargestElementSolver.FindKthLargestElement(array, i);
			}
		}
	}
}
