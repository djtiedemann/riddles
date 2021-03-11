using System;
using System.Collections.Generic;
using System.Text;

namespace Interviews.DataStructures
{
	public class BinaryTree<T>
		where T : IComparable
	{
		BinaryTreeNode<T> _root;
		public BinaryTree()
		{
			this._root = null;
		}

		public void InsertAllInOrder(List<T> dataList) {
			foreach (var data in dataList) {
				this.InsertInOrder(data);
			}
		}

		public void InsertInOrder(T data) {
			var toInsert = new BinaryTreeNode<T>(data);
			if (this._root == null) {
				this._root = toInsert;
			} else
			{
				this.InsertInOrderInternal(toInsert, this._root);
			}
		}

		public bool IsBinarySearchTree() {
			if (this._root == null) {
				return true;
			}
			(var isBinarySearchTree, T min, T max) = this.IsBinarySearchTreeInternal(this._root);
			return isBinarySearchTree;
		}

		private (bool isBinarySearchTree, T min, T max) IsBinarySearchTreeInternal(BinaryTreeNode<T> currentNode) {
			T min = currentNode.Data;
			T max = currentNode.Data;
			bool isBinarySearchTree = true;
			if(currentNode.LeftNode != null)
			{
				(var isLeftBinarySearchTree, var leftMin, var leftMax) = this.IsBinarySearchTreeInternal(currentNode.LeftNode);
				isBinarySearchTree = isBinarySearchTree && isLeftBinarySearchTree && leftMax.CompareTo(currentNode.Data) < 0;
				min = min.CompareTo(leftMin) <= 0 ? min : leftMin;
				max = max.CompareTo(leftMax) >= 0 ? max : leftMax;
			} if (currentNode.RightNode != null) {
				(var isRightBinarySearchTree, var rightMin, var rightMax) = this.IsBinarySearchTreeInternal(currentNode.RightNode);
				isBinarySearchTree = isBinarySearchTree && isRightBinarySearchTree && rightMin.CompareTo(currentNode.Data) > 0;
				min = min.CompareTo(rightMin) <= 0 ? min : rightMin;
				max = max.CompareTo(rightMax) >= 0 ? max : rightMax;
			}
			return (isBinarySearchTree, min, max);
		}

		private void InsertInOrderInternal(BinaryTreeNode<T> data, BinaryTreeNode<T> currentNode)
		{
			if (currentNode.LeftNode == null)
			{
				currentNode.LeftNode = data;
				return;
			} if (currentNode.RightNode == null) {
				currentNode.RightNode = data;
				return;
			}
			var minHeightLeft = this.GetMinHeightInternal(currentNode.LeftNode);
			var minHeightRight = this.GetMinHeightInternal(currentNode.RightNode);
			if (minHeightLeft <= minHeightRight) {
				this.InsertInOrderInternal(data, currentNode.LeftNode);
			} else {
				this.InsertInOrderInternal(data, currentNode.RightNode);
			}
		}

		private int GetMinHeightInternal(BinaryTreeNode<T> currentNode) {
			if (currentNode == null) {
				return 0;
			}
			var leftHeight = this.GetMinHeightInternal(currentNode.LeftNode);
			var rightHeight = this.GetMinHeightInternal(currentNode.RightNode);
			return 1 + Math.Min(leftHeight, rightHeight);
		}
	}

	internal class BinaryTreeNode<T>
	{
		public BinaryTreeNode(T data)
		{
			this.Data = data;
		}
		public T Data { get; }
		public BinaryTreeNode<T> LeftNode { get; set; }
		public BinaryTreeNode<T> RightNode { get; set; }
	}
}
