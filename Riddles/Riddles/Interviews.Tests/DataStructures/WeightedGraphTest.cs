using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Interviews.DataStructures;

namespace Interviews.Tests.DataStructures
{
	public class WeightedGraphTest
	{

		[Test]
		public void TestFindShortestPath()
		{
			var graph = new WeightedGraph(this.GetGridAdjacencyMatrix);
			var shortestPath = graph.FindShortestPath(Tuple.Create(0, 0), Tuple.Create(3, 3));
		}

		public List<WeightedGraph.WeightedEdge> GetGridAdjacencyMatrix(Tuple<int, int> currentVertex)
		{
			var max = 4;
			var weightedEdges = new List<WeightedGraph.WeightedEdge> { };
			if(currentVertex.Item1 < max - 1)
			{
				weightedEdges.Add(new WeightedGraph.WeightedEdge
				{
					StartingVertex = currentVertex,
					AdjacentVertex = Tuple.Create(currentVertex.Item1 + 1, currentVertex.Item2),
					Weight = 1
				});
			} if(currentVertex.Item2 < max - 1)
			{
				weightedEdges.Add(new WeightedGraph.WeightedEdge
				{
					StartingVertex = currentVertex,
					AdjacentVertex = Tuple.Create(currentVertex.Item1, currentVertex.Item2 + 1),
					Weight = 1
				});
			}
			if (currentVertex.Item1 < max - 1 && currentVertex.Item2 < max - 1)
			{
				weightedEdges.Add(new WeightedGraph.WeightedEdge
				{
					StartingVertex = currentVertex,
					AdjacentVertex = Tuple.Create(currentVertex.Item1 + 1, currentVertex.Item2 + 1),
					Weight = 3
				});
			}
			return weightedEdges;
		}
	}
}
