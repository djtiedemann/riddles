using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Interviews
{
	public class KnapsackDynamicProgramming
	{
		public List<Item> GetMostValuableItemsThatFitInKnapsack(Item[] items, int initialCapacity)
		{
			var memo = new Tuple<double, bool, int, int>[items.Length, initialCapacity + 1];
			var totalValue = this.GetMostValueInKnapsackInternal(items, 0, initialCapacity, memo);
			var itemsInKnapsack = new List<Item> { };
			this.BacktrackSolution(items, memo, 0, initialCapacity, itemsInKnapsack);
			return itemsInKnapsack;
		}

		private void BacktrackSolution(Item[] items, Tuple<double, bool, int, int>[,] memo, int i, int capacity, List<Item> itemsInKnapsack)
		{
			if(i >= items.Length || capacity <=0)
			{
				return;
			}
			if(memo[i, capacity].Item2)
			{
				itemsInKnapsack.Add(items[i]);
			}
			BacktrackSolution(items, memo, memo[i, capacity].Item3, memo[i, capacity].Item4, itemsInKnapsack);
		}

		private double GetMostValueInKnapsackInternal(Item[] items, int i, int remainingCapacity, Tuple<double, bool, int, int>[,] memo)
		{
			if (i >= items.Length || remainingCapacity <= 0)
			{
				return 0;
			}
			if(memo[i, remainingCapacity] != null)
			{
				return memo[i, remainingCapacity].Item1;
			}
			var valueIfItemNotIncluded = this.GetMostValueInKnapsackInternal(items, i + 1, remainingCapacity, memo);
			var valueIfItemIncluded = -1.0;
			if(items[i].Size <= remainingCapacity)
			{
				valueIfItemIncluded = this.GetMostValueInKnapsackInternal(items, i + 1, remainingCapacity - items[i].Size, memo) + items[i].Value;
			}
			if(valueIfItemNotIncluded > valueIfItemIncluded)
			{
				memo[i, remainingCapacity] = Tuple.Create(valueIfItemNotIncluded, false, i + 1, remainingCapacity);				
			}else
			{
				memo[i, remainingCapacity] = Tuple.Create(valueIfItemIncluded, true, i + 1, remainingCapacity - items[i].Size);
			}
			return memo[i, remainingCapacity].Item1;
		}

	
		public class Item
		{
			public int Size { get; set; }
			public double Value { get; set; }
		}
	}
}
