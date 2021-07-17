using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Interviews;

namespace Interviews.Tests
{
	public class InsertIntervalTest
	{
		[Test]
		public void TestInsertNewInterval()
		{			
			var insertIntervalSolver = new InsertInterval();

			var intervals = new List<Tuple<int, int>> { Tuple.Create(1, 3), Tuple.Create(6, 9) };
			var interval = Tuple.Create(2, 5);
			var result = insertIntervalSolver.InsertNewInterval(intervals, interval);

			intervals = new List<Tuple<int, int>> { Tuple.Create(1, 2), Tuple.Create(3, 5), Tuple.Create(6, 7),
				Tuple.Create(8, 10), Tuple.Create(12, 16)};
			interval = Tuple.Create(4, 8);
			result = insertIntervalSolver.InsertNewInterval(intervals, interval);

			intervals = new List<Tuple<int, int>> { };
			interval = Tuple.Create(5, 7);
			result = insertIntervalSolver.InsertNewInterval(intervals, interval);

			intervals = new List<Tuple<int, int>> { Tuple.Create(1, 5) };
			interval = Tuple.Create(2, 3);
			result = insertIntervalSolver.InsertNewInterval(intervals, interval);

			intervals = new List<Tuple<int, int>> { Tuple.Create(1, 5) };
			interval = Tuple.Create(2, 7);
			result = insertIntervalSolver.InsertNewInterval(intervals, interval);
		}
	}
}
