using System;
using System.Collections.Generic;
using System.Text;
using Interviews.DataStructures;
using NUnit.Framework;

namespace Interviews.Tests.DataStructures
{
	public class HeapTest
	{
		//[TestCase(new int[] { 5, 2, 4, 7, 6, 1, 3 }, new int[] { 1, 2, 3, 4, 5, 6, 7 })]
		[TestCase(new int[] { 7, 5, 11, 9, 4, 2, 3, 10, 1, 8, 4, 6 }, new int[] { 1, 2, 3, 4, 4, 5, 6, 7, 8, 9, 10, 11 })]
		public void TestHeapSort(int[] array, int[] expected)
		{
			var heap = new Heap(array);
			//var actual = heap.HeapSort();
			//Assert.AreEqual(expected, actual);
			heap.ChangePriority(8, 1);
			heap.ChangePriority(2, 8);
		}
	}
}
