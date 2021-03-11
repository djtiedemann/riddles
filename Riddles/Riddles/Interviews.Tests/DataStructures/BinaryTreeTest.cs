using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Interviews.DataStructures;

namespace Interviews.Tests.DataStructures
{
	public class BinaryTreeTest
	{
		[TestCase]
		public void TestBinaryTree()
		{
			var binaryTree = new BinaryTree<int>();
			binaryTree.InsertAllInOrder(new List<int> { 11, 4, 30, 3, 9, 29, 31 });
			var isBinarySearchTree = binaryTree.IsBinarySearchTree();

			binaryTree = new BinaryTree<int>();
			binaryTree.InsertAllInOrder(new List<int> { 11, 4, 30, 9, 3, 29, 31 });
			isBinarySearchTree = binaryTree.IsBinarySearchTree();
		}
	}
}
