using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Interviews;

namespace Interviews.Tests
{
	public class BinarySearchTest
	{
		[Test]
		public void TestBinarySearch()
		{
			var array = new BinarySearchItem[]
			{
				new BinarySearchItem{ Id = 1},
				new BinarySearchItem{ Id = 3},
				new BinarySearchItem{ Id = 5},
				new BinarySearchItem{ Id = 7},
				new BinarySearchItem{ Id = 9},
				new BinarySearchItem{ Id = 11},
				new BinarySearchItem{ Id = 15},
				new BinarySearchItem{ Id = 21},
				new BinarySearchItem{ Id = 29},
			};

			var binarySearch = new BinarySearch<BinarySearchItem>();
			var result1 = binarySearch.Find(array, new BinarySearchItem { Id = 21 });
			var result2 = binarySearch.Find(array, new BinarySearchItem { Id = 6 });
		}
	}
}
