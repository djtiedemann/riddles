using System;
using System.Collections.Generic;
using System.Text;

namespace Interviews.DataStructures
{
	public class Graph
	{
		private Func<Tuple<int, int>, List<Tuple<int, int>>> _graphFunc;
		public Graph(Func<Tuple<int, int>, List<Tuple<int, int>>> graphFunc)
		{
			this._graphFunc = graphFunc;
		}

		public List<Tuple<int, int>> GetShortestPath(Tuple<int, int> startingVertex, Tuple<int, int> finalVertex) { 
			var parentDictionary 
				= new Dictionary<Tuple<int, int>, Tuple<int, int>> { { startingVertex, null } };
			var verticesToProcess = new List<Tuple<int, int>> { startingVertex };
			while(verticesToProcess.Count > 0 && !parentDictionary.ContainsKey(finalVertex))
			{
				var vertex = verticesToProcess[0];
				verticesToProcess.RemoveAt(0);
				var adjacentVertices = this._graphFunc(vertex);
				foreach(var adjacentVertex in adjacentVertices)
				{
					if (!parentDictionary.ContainsKey(adjacentVertex))
					{
						parentDictionary[adjacentVertex] = vertex;
						if (adjacentVertex.Equals(finalVertex))
						{
							break;
						}
						verticesToProcess.Add(adjacentVertex);
					}
				}
			}

			var shortestPath = new List<Tuple<int, int>> { finalVertex };
			var currentNode = finalVertex;
			while(parentDictionary[currentNode] != null)
			{
				currentNode = parentDictionary[currentNode];
				shortestPath.Add(currentNode);
			}
			shortestPath.Reverse();
			return shortestPath;
		}

		public List<Tuple<int, int>> TopologicalSort(Tuple<int, int> startingVertex)
		{
			var orderFinished = new List<Tuple<int, int>>();
			var verticesExplored = new HashSet<Tuple<int, int>>();
			this.TopologicalSortInternal(startingVertex, orderFinished, verticesExplored);
			orderFinished.Reverse();
			return orderFinished;
		}

		private void TopologicalSortInternal(
			Tuple<int, int> currentVertex,
			List<Tuple<int, int>> orderFinished,
			HashSet<Tuple<int, int>> verticesExplored)
		{
			if (verticesExplored.Contains(currentVertex)) { return; }
			verticesExplored.Add(currentVertex);
			var adjacentVertices = this._graphFunc(currentVertex);
			foreach(var adjacentVertex in adjacentVertices)
			{
				this.TopologicalSortInternal(adjacentVertex, orderFinished, verticesExplored);
			}
			orderFinished.Add(currentVertex);
		}

		public List<Tuple<int, int>> FindCycle(List<Tuple<int, int>> verticesToExplore)
		{
			var verticesExplored = new Dictionary<Tuple<int, int>, Tuple<int, int>>();
			Tuple<int, int> endOfCycle = null;
			Tuple<int, int> startOfCycle = null;
			foreach(var vertex in verticesToExplore)
			{
				var verticesExploredThisCycle = new HashSet<Tuple<int, int>> { };
				var result = this.ExploreVertex(vertex, verticesExplored, verticesExploredThisCycle, null);
				if (result.Item1 != null) {
					endOfCycle = result.Item1;
					startOfCycle = result.Item2;
					break;
				}
			}
			var cycle = new List<Tuple<int, int>>();
			if(endOfCycle == null)
			{
				return cycle;
			}
			var lastVertex = endOfCycle;
			while(!lastVertex.Equals(startOfCycle))
			{
				cycle.Add(lastVertex);
				lastVertex = verticesExplored[lastVertex];
			}
			cycle.Add(lastVertex);
			return cycle;
		}

		private (Tuple<int, int>, Tuple<int, int>) ExploreVertex(Tuple<int, int> vertex,
			Dictionary<Tuple<int, int>, Tuple<int, int>> verticesExploredOverall,
			HashSet<Tuple<int, int>> verticesExploredThisCycle,
			Tuple<int, int> parent)
		{
			if (verticesExploredThisCycle.Contains(vertex))
			{
				return (parent, vertex);
			}
			if (verticesExploredOverall.ContainsKey(vertex))
			{
				return (null, null);
			}
			verticesExploredOverall[vertex] = parent;
			verticesExploredThisCycle.Add(vertex);

			var adjacentVertices = this._graphFunc(vertex);
			foreach (var adjacentVertex in adjacentVertices) {
				var result = this.ExploreVertex(adjacentVertex, verticesExploredOverall, verticesExploredThisCycle, vertex);
				if (result.Item1 != null) { return result; }
			}

			verticesExploredThisCycle.Remove(vertex);
			return (null, null);
		}
	}
}
