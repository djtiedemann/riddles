using System;
using System.Collections.Generic;
using System.Text;

namespace Interviews
{
	public class BinarySearch<T>
		where T : IComparable
	{
		public T Find(T[] array, T itemToFind)
		{
			return this.FindInternal(array, itemToFind, 0, array.Length - 1);
		}

		public T FindInternal(T[] array, T itemToFind, int min, int max)
		{
			if(min > max)
			{
				return default(T);
			}
			if(min == max)
			{
				if(array[min].CompareTo(itemToFind) == 0)
				{
					return array[min];
				}
				return default(T);
			}
			var median = (min + max) / 2;
			var medianElement = array[median];
			var comparison = itemToFind.CompareTo(medianElement);
			if (comparison == 0)
			{
				return medianElement;
			}
			if(comparison < 1)
			{
				return this.FindInternal(array, itemToFind, min, median - 1);
			} else
			{
				return this.FindInternal(array, itemToFind, median + 1, max);
			}
		}		
	}

	public class BinarySearchItem : IComparable
	{
		public int Id { get; set; }

		public int CompareTo(object other)
		{
			if (!(other is BinarySearchItem))
			{
				throw new InvalidOperationException("not allowed to compare an item to a non-item");
			}
			var otherItem = (BinarySearchItem)other;
			if (Id < otherItem.Id) { return -1; }
			else if (Id == otherItem.Id) { return 0; }
			else { return 1; }
		}
	}
}
