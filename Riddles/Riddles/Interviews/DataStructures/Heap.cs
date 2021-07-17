using System;
using System.Collections.Generic;
using System.Text;

namespace Interviews.DataStructures
{
	public class Heap
	{
		private int[] _elements;
		private int _count;

		public Heap(int initialSize)
		{
			this._elements = new int[initialSize];
			this._count = 0;
		}

		public Heap(int[] elements)
		{
			this._elements = elements;
			this._count = elements.Length;
			this.BuildHeap();
		}

		public void Insert(int elements)
		{

		}

		public int[] HeapSort()
		{
			var returnArray = new int[this._count];
			var initialCount = this._count;
			for(int i=0; i< initialCount; i++)
			{
				returnArray[i] = this.ExtractMin().Value;
			}
			return returnArray;
		}
		
		public int? ExtractMin()
		{
			if(this._count == 0)
			{
				return (int?)null;
			}
			this._count--;
			return this.ExtractMinInternal(0);
		}

		public void ChangePriority(int value, int newValue) {
			var index = -1;
			for(int i=0; i<_count; i++)
			{
				if(this._elements[i] == value)
				{
					index = i;
					break;
				}
			}
			if(index < 0) { return; }
			this._elements[index] = newValue;
			if(newValue > value)
			{
				this.MinHeapify(index);
				return;
			}
			while(index > 0)
			{
				var parent = (index - 1) / 2;
				if(parent < 0) { return; }
				if (this._elements[parent] < this._elements[index]) {
					return;
				}
				else {
					var temp = this._elements[parent];
					this._elements[parent] = this._elements[index];
					this._elements[index] = temp;
					index = parent;
				}
			}
		}

		public int ExtractMinInternal(int index)
		{
			var value = this._elements[index];
			this._elements[index] = int.MaxValue;

			var leftChild = 2 * index + 1;
			var rightChild = 2 * index + 2;

			if(leftChild < _count && this._elements[leftChild] < this._elements[index]
				&& (rightChild >= this._count || this._elements[leftChild] < this._elements[rightChild]))
			{
				this._elements[index] = this._elements[leftChild];
				this.ExtractMinInternal(leftChild);
			} else if (rightChild < this._count && this._elements[rightChild] < this._elements[index])
			{
				this._elements[index] = this._elements[rightChild];
				this.ExtractMinInternal(rightChild);
			}
			return value;
		}

		public void BuildHeap()
		{
			for(int i=(this._count)/2; i>=0; i--)
			{
				this.MinHeapify(i);
			}
		}

		private void MinHeapify(int index)
		{
			var leftChild = 2*index + 1;
			var rightChild = 2 * index + 2;
			if(leftChild < this._count && this._elements[leftChild] < this._elements[index] 
				&& (rightChild >= this._count || this._elements[leftChild] < this._elements[rightChild]))
			{
				var temp = _elements[index];
				this._elements[index] = this._elements[leftChild];
				this._elements[leftChild] = temp;
				this.MinHeapify(leftChild);
				return;
			} if(rightChild < this._count && this._elements[rightChild] < this._elements[index])
			{
				var temp = this._elements[index];
				this._elements[index] = this._elements[rightChild];
				this._elements[rightChild] = temp;
				this.MinHeapify(rightChild);
				return;
			}
		}
	}
}
