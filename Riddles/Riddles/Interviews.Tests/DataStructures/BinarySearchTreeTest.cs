using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Interviews.DataStructures;

namespace Interviews.Tests.DataStructures
{
	public class BinarySearchTreeTest
	{
		[TestCase]
		public void TestBinarySearchTree()
		{
			var binarySearchTree = new BinarySearchTree<int>();
			//binarySearchTree.InsertAll(new List<int> { 9, 2, 25, 4, 11, 29, 31, 30, 3, 6 });
			//var inOrderTraversal = binarySearchTree.InOrderTraveral();
			//binarySearchTree.RebalanceTree();

			//keep deleting the root
			//binarySearchTree = new BinarySearchTree<int>();
			//binarySearchTree.InsertAll(new List<int> { 9, 2, 25, 4, 11, 29, 31, 30, 3, 6 });
			//binarySearchTree.Delete(9);
			//binarySearchTree.Delete(2);
			//binarySearchTree.Delete(25);
			//binarySearchTree.Delete(11);
			//binarySearchTree.Delete(4);
			//binarySearchTree.Delete(3);
			//binarySearchTree.Delete(29);
			//binarySearchTree.Delete(6);
			//binarySearchTree.Delete(31);
			//binarySearchTree.Delete(30);
			//binarySearchTree = new BinarySearchTree<int>();
			//binarySearchTree.InsertAll(new List<int> { 9, 2, 25, 4, 11, 29, 31, 30, 3, 6 });
			//var elementsByLevel = binarySearchTree.ReturnElementsByLevel();

			//binarySearchTree = new BinarySearchTree<int>();
			//binarySearchTree.InsertAll(new List<int> { 9, 2, 25, 4, 11, 29, 31, 30, 3, 6 });
			//var isBalanced = binarySearchTree.IsBalanced();
			//binarySearchTree.RebalanceTree();
			//isBalanced = binarySearchTree.IsBalanced();

			//binarySearchTree.InsertAll(new List<int> { 9, 2, 25, 4, 11, 29, 31, 30, 3, 6 });
			//var next = binarySearchTree.FindNext(2);
			//next = binarySearchTree.FindNext(3);
			//next = binarySearchTree.FindNext(4);
			//next = binarySearchTree.FindNext(6);
			//next = binarySearchTree.FindNext(9);
			//next = binarySearchTree.FindNext(11);
			//next = binarySearchTree.FindNext(25);
			//next = binarySearchTree.FindNext(29);
			//next = binarySearchTree.FindNext(30);
		}
	}
}
