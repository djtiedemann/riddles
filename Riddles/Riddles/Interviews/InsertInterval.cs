using System;
using System.Collections.Generic;
using System.Text;

namespace Interviews
{
	public class InsertInterval
	{
		public List<Tuple<int, int>> InsertNewInterval(List<Tuple<int, int>> existingIntervals, Tuple<int, int> newInterval)
		{
			var insertIntervalStart = newInterval.Item1;
			var insertIntervalEnd = newInterval.Item2;
			bool hasInsertedNewInterval = false;
			var resultIntervals = new List<Tuple<int, int>>();
			foreach (var existingInterval in existingIntervals) { 
				if(existingInterval.Item2 < newInterval.Item1) {
					resultIntervals.Add(existingInterval);
					continue;
				}
				if (existingInterval.Item1 > newInterval.Item2) {
					if (!hasInsertedNewInterval) {
						resultIntervals.Add(Tuple.Create(insertIntervalStart, insertIntervalEnd));
						hasInsertedNewInterval = true;
					}
					resultIntervals.Add(existingInterval);
					continue;
				}
				if (existingInterval.Item1 < insertIntervalStart) { insertIntervalStart = existingInterval.Item1; }
				if (existingInterval.Item2 > insertIntervalEnd) { insertIntervalEnd = existingInterval.Item2; }
			}
			if (!hasInsertedNewInterval)
			{
				resultIntervals.Add(Tuple.Create(insertIntervalStart, insertIntervalEnd));
			}
			return resultIntervals;
		}
	}
}
