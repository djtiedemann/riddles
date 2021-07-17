using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Interviews;

namespace Interviews.Tests
{
	public class TopKRecentElementsTest
	{
		[TestCase]
		public void TestTopKMostFrequentElements()
		{
			int[] array = new int[] { 1, 5, 7, 7, 8, 9, 10, 5, 5, 10, 10, 1, 1, 1, 7, 7, 7, 7, 4, 5, 5 };
			var topKSolver = new TopKRecentElements();
			var topK = topKSolver.TopKMostFrequentElements(array, 2);
		}
	}
}
