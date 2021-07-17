using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Interviews.DataStructures
{
	public class BinarySearchTree<T>
		where T : IComparable
	{
		private BinarySearchTreeNode<T> _root;
		public BinarySearchTree()
		{
			this._root = null;
		}

		public void Insert(T data)
		{
			var toInsert = new BinarySearchTreeNode<T>(data);
			if (this._root == null)
			{
				this._root = toInsert;
			}
			else {
				this.InsertInternal(toInsert, this._root);
			}
		}

		public void InsertAll(List<T> dataToInsert) {
			foreach (var data in dataToInsert) {
				this.Insert(data);
			}
		}

		public void Delete(T data) { 
			if(this._root != null)
			{
				this.DeleteInternal(data, this._root);
			}
		}

		public List<T> InOrderTraveral()
		{
			var traversal = new List<T>();
			if(this._root != null)
			{
				this.InOrderTraversalInternal(this._root, traversal);
			}
			return traversal;
		}

		public T FindLeastCommonAncestor(T child1, T child2)
		{
			return this.FindLeastCommonAncestorInternal(child1, child2, this._root);
		}

		private T FindLeastCommonAncestorInternal(T child1, T child2, BinarySearchTreeNode<T> currentNode){
			if (currentNode.Data.Equals(child1) || currentNode.Data.Equals(child2)) { return currentNode.Data; }
			if (currentNode.Data.CompareTo(child1) < 0 && currentNode.Data.CompareTo(child2) < 0) {
				return this.FindLeastCommonAncestorInternal(child1, child2, currentNode.RightChild);
			}
			if(currentNode.Data.CompareTo(child1) > 0 && currentNode.Data.CompareTo(child2) > 0)
			{
				return this.FindLeastCommonAncestorInternal(child1, child2, currentNode.LeftChild);
			}
			return currentNode.Data;
		}

		public void RebalanceTree()
		{
			var inOrderTraversal = this.InOrderTraveral();
			List<List<T>> listsToProcess = new List<List<T>> { inOrderTraversal };
			List<T> correctOrdering = new List<T>();
			while (listsToProcess.Count > 0)
			{
				var listToProcess = listsToProcess.First();
				listsToProcess.RemoveAt(0);

				if (listToProcess.Count() == 0)
				{
					continue;
				}

				var medianIndex = listToProcess.Count() / 2;
				correctOrdering.Add(listToProcess[medianIndex]);

				var leftList = listToProcess.Where((element, index) => index < medianIndex).ToList();
				var rightList = listToProcess.Where((element, index) => index > medianIndex).ToList();
				listsToProcess.Add(leftList);
				listsToProcess.Add(rightList);
			}

			this._root = null;
			this.InsertAll(correctOrdering);
		}

		public List<List<T>> ReturnElementsByLevel()
		{
			List<List<T>> nodesByLevel = new List<List<T>> { };

			List<BinarySearchTreeNode<T>> nodesToProcess = new List<BinarySearchTreeNode<T>> { this._root };
			List<BinarySearchTreeNode<T>> nextLevelToProcess = new List<BinarySearchTreeNode<T>> { };
			while (nodesToProcess.Count() > 0)
			{
				List<T> dataAtCurrentLevel = new List<T> { };
				foreach (var node in nodesToProcess)
				{
					dataAtCurrentLevel.Add(node.Data);
					if (node.LeftChild != null)
					{
						nextLevelToProcess.Add(node.LeftChild);
					}
					if (node.RightChild != null)
					{
						nextLevelToProcess.Add(node.RightChild);
					}
				}
				nodesByLevel.Add(dataAtCurrentLevel);
				nodesToProcess = nextLevelToProcess;
				nextLevelToProcess = new List<BinarySearchTreeNode<T>> { };
			}
			return nodesByLevel;
		}

		public bool IsBalanced()
		{
			return this.IsBalancedInternal(this._root);
		}

		public T FindNext(T data)
		{
			var node = this.FindInternal(data, this._root);
			if(node == null)
			{
				throw new InvalidOperationException("unexpected input");
			}
			BinarySearchTreeNode<T> prevNode = null;
			while(node.RightChild == null || (node.RightChild == prevNode && prevNode != null))
			{
				if(node.Parent == null)
				{
					throw new InvalidOperationException("unexpected input");
				}
				if(node.Parent.LeftChild == node)
				{
					return node.Parent.Data;
				}
				prevNode = node;
				node = node.Parent;
			}
			node = node.RightChild;
			while(node.LeftChild != null)
			{
				node = node.LeftChild;
			}
			return node.Data;
		}

		private bool IsBalancedInternal(BinarySearchTreeNode<T> currentNode) { 
			if(currentNode == null)
			{
				return true;
			}
			if(!this.IsBalancedInternal(currentNode.LeftChild) || !this.IsBalancedInternal(currentNode.RightChild))
			{
				return false;
			}
			var leftHeight = this.GetMaxHeightInternal(currentNode.LeftChild);
			var rightHeight = this.GetMaxHeightInternal(currentNode.RightChild);
			return Math.Abs(leftHeight - rightHeight) <= 1;
		}

		private int GetMaxHeightInternal(BinarySearchTreeNode<T> currentNode)
		{
			if(currentNode == null)
			{
				return 0;
			}
			var leftHeight = this.GetMaxHeightInternal(currentNode.LeftChild);
			var rightHeight = this.GetMaxHeightInternal(currentNode.RightChild);
			return 1 + Math.Max(leftHeight, rightHeight);
		}

		private void InsertInternal(BinarySearchTreeNode<T> toInsert, BinarySearchTreeNode<T> currentNode)
		{
			if (toInsert.Data.CompareTo(currentNode.Data) <= 0)
			{
				if (currentNode.LeftChild == null)
				{
					currentNode.LeftChild = toInsert;
					toInsert.Parent = currentNode;
				}
				else
				{
					this.InsertInternal(toInsert, currentNode.LeftChild);
				}
			}
			else
			{
				if (currentNode.RightChild == null)
				{
					currentNode.RightChild = toInsert;
					toInsert.Parent = currentNode;
				}
				else
				{
					this.InsertInternal(toInsert, currentNode.RightChild);
				}
			}
		}	

		private bool DeleteInternal(T data, BinarySearchTreeNode<T> currentNode) {
			if (currentNode.Data.Equals(data))
			{
				// if there is no parent or children, then there's only 1 node in the BST, and we can simply delete it
				if (currentNode.LeftChild == null && currentNode.RightChild == null && currentNode.Parent == null)
				{
					this._root = null;
				}
				if(currentNode.Parent == null)
				{
					if(currentNode.LeftChild != null)
					{						
						this._root = currentNode.LeftChild;
						currentNode.LeftChild.Parent = null;

						if(currentNode.RightChild != null){
							this.InsertInternal(currentNode.RightChild, currentNode.LeftChild);
						}
					}
					else if(currentNode.RightChild != null)
					{
						this._root = currentNode.RightChild;
						currentNode.RightChild.Parent = null;
					}
					return true;
				}
				bool currentNodeIsLeftChild = currentNode.Parent.LeftChild == currentNode;
				if (currentNode.LeftChild != null) {
					if (currentNodeIsLeftChild)
					{
						currentNode.Parent.LeftChild = currentNode.LeftChild;
					}
					else
					{
						currentNode.Parent.RightChild = currentNode.LeftChild;
					}
					currentNode.LeftChild.Parent = currentNode.Parent;
					
					if(currentNode.RightChild != null)
					{
						this.InsertInternal(currentNode.RightChild, this._root);
					}
				}
				else if (currentNode.RightChild != null)
				{
					if (currentNodeIsLeftChild)
					{
						currentNode.Parent.LeftChild = currentNode.RightChild;
					}
					else
					{
						currentNode.Parent.RightChild = currentNode.RightChild;
					}
					currentNode.RightChild.Parent = currentNode.Parent;
				}
				else
				{
					// this is a leaf node
					if (currentNodeIsLeftChild) {
						currentNode.Parent.LeftChild = null;
					} else
					{
						currentNode.Parent.RightChild = null;
					}
				}
				return true;
			}
			else if (data.CompareTo(currentNode.Data) < 1) { 
				if(currentNode.LeftChild != null)
				{
					return this.DeleteInternal(data, currentNode.LeftChild);
				}
				return false;
			}
			else
			{
				if (currentNode.RightChild != null) {
					return this.DeleteInternal(data, currentNode.RightChild);
				}
				return false;
			}
		}

		private void InOrderTraversalInternal(BinarySearchTreeNode<T> currentNode, List<T> traversal)
		{
			if (currentNode.LeftChild != null)
			{
				this.InOrderTraversalInternal(currentNode.LeftChild, traversal);
			}
			traversal.Add(currentNode.Data);
			if (currentNode.RightChild != null)
			{
				this.InOrderTraversalInternal(currentNode.RightChild, traversal);
			}
		}

		private BinarySearchTreeNode<T> FindInternal(T data, BinarySearchTreeNode<T> currentNode) { 
			if(currentNode == null)
			{
				return null;
			}
			if (currentNode.Data.CompareTo(data) == 0) {
				return currentNode;
			}
			if(data.CompareTo(currentNode.Data) < 0)
			{
				return this.FindInternal(data, currentNode.LeftChild);
			}
			return this.FindInternal(data, currentNode.RightChild);
		}
	}

	internal class BinarySearchTreeNode<T>
		where T : IComparable
	{
		public BinarySearchTreeNode(T data) {
			this.Data = data;
		}

		public BinarySearchTreeNode<T> LeftChild { get; set; }

		public BinarySearchTreeNode<T> RightChild { get; set; }

		public BinarySearchTreeNode<T> Parent { get; set; }

		public T Data { get; private set; }
	}
}
