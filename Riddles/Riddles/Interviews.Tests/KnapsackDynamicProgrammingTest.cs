using System;
using System.Collections.Generic;
using System.Text;
using Interviews;
using NUnit.Framework;

namespace Interviews.Tests
{
	public class KnapsackDynamicProgrammingTest
	{
		[Test]
		public void TestGetMostValuableItemsThatFitInKnapsack()
		{
			var items = new KnapsackDynamicProgramming.Item[]
			{
				new KnapsackDynamicProgramming.Item { Size = 3, Value = 1},
				new KnapsackDynamicProgramming.Item { Size = 5, Value = 5},
				new KnapsackDynamicProgramming.Item { Size = 7, Value = 6},
				new KnapsackDynamicProgramming.Item { Size = 5, Value = 4},
				new KnapsackDynamicProgramming.Item { Size = 4, Value = 2},
				new KnapsackDynamicProgramming.Item { Size = 3, Value = 2},
			};
			var capacity = 11;

			var knapsackSolver = new KnapsackDynamicProgramming();
			var result = knapsackSolver.GetMostValuableItemsThatFitInKnapsack(items, capacity);
		}
	}
}
