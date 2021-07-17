using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Interviews.DataStructures;

namespace Interviews.Tests.DataStructures
{
	public class GraphTest
	{
		[Test]
		public void TestCycleDetection()
		{
			//var graph = new Graph(this.GridAdjacencyMatrix);
			//var cycle = graph.FindCycle(new List<Tuple<int, int>> { Tuple.Create(0, 0) });

			var graph = new Graph(this.SnakeFlowAdjacencyMatrix);
			var cycle = graph.FindCycle(new List<Tuple<int, int>> { Tuple.Create(0, 0) });
		}

		[Test]
		public void TestTopologicalSort()
		{
			var graph = new Graph(this.GridAdjacencyMatrix);
			var topologicalSort = graph.TopologicalSort(Tuple.Create(0,0));
		}

		[Test]
		public void TestFindShortestPath()
		{
			var graph = new Graph(this.GridAdjacencyMatrix);
			var shortestPath = graph.GetShortestPath(Tuple.Create(1, 0), Tuple.Create(3, 3));
		}

		public List<Tuple<int, int>> SnakeFlowAdjacencyMatrix(Tuple<int, int> location)
		{
			var maxSize = 4;
			var adjacentLocations = new List<Tuple<int, int>>();
			if(location.Item1 % 2 == 0)
			{
				if (location.Item2 + 1 < maxSize) {
					adjacentLocations.Add(Tuple.Create(location.Item1, location.Item2 + 1));
				}
			}
			else
			{
				if(location.Item2 - 1 >= 0)
				{
					adjacentLocations.Add(Tuple.Create(location.Item1, location.Item2 - 1));
				}
			}
			if((location.Item2 == 0 || location.Item2 == maxSize - 1) && location.Item1 < maxSize - 1)
			{
				adjacentLocations.Add(Tuple.Create(location.Item1 + 1, location.Item2));				
			}
			if (location.Item2 == 2 && location.Item1 > 0) {
				adjacentLocations.Add(Tuple.Create(location.Item1 - 1, location.Item2));
			}
			return adjacentLocations;
		}

		public List<Tuple<int, int>> GridAdjacencyMatrix(Tuple<int, int> location)
		{
			var maxSize = 4;
			var adjacentLocations = new List<Tuple<int, int>>();
			if (location.Item1 < maxSize - 1)
			{
				adjacentLocations.Add(Tuple.Create(location.Item1 + 1, location.Item2));
			} if(location.Item2 < maxSize - 1)
			{
				adjacentLocations.Add(Tuple.Create(location.Item1, location.Item2 + 1));
			} if(location.Item1 < maxSize - 1 && location.Item2 < maxSize - 1)
			{
				adjacentLocations.Add(Tuple.Create(location.Item1 + 1, location.Item2 + 1));
			}
			return adjacentLocations;
		}
	}
}
