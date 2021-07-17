using System;
using System.Collections.Generic;
using System.Text;

namespace Interviews.DataStructures
{
	public class WeightedGraph
	{
		private Func<Tuple<int, int>, List<WeightedEdge>> _adjacencyFunction;
		public WeightedGraph(Func<Tuple<int, int>, List<WeightedEdge>> adjacencyFunction)
		{
			this._adjacencyFunction = adjacencyFunction;
		}

		public (List<Tuple<int, int>> path, int cost) FindShortestPath(Tuple<int, int> startingVertex, Tuple<int, int> finalVertex)
		{
			var costDictionary = new Dictionary<Tuple<int, int>, int> { { startingVertex, 0 } };
			var previousVertexDictionary = new Dictionary<Tuple<int, int>, Tuple<int, int>> { { startingVertex, null } };
			var verticesToExplore = new List<Tuple<int, int>> { startingVertex };
			while(verticesToExplore.Count != 0)
			{
				var currentVertex = verticesToExplore[0];
				verticesToExplore.RemoveAt(0);
				var weightedEdges = this._adjacencyFunction(currentVertex);
				foreach(var weightedEdge in weightedEdges)
				{
					var cost = costDictionary[currentVertex] + weightedEdge.Weight;
					if(!costDictionary.ContainsKey(weightedEdge.AdjacentVertex) || cost < costDictionary[weightedEdge.AdjacentVertex])
					{
						costDictionary[weightedEdge.AdjacentVertex] = cost;
						previousVertexDictionary[weightedEdge.AdjacentVertex] = currentVertex;
						verticesToExplore.Add(weightedEdge.AdjacentVertex);
					}
				}
			}

			var finalCost = costDictionary[finalVertex];
			var path = new List<Tuple<int, int>> { finalVertex };
			var vertexInPath = finalVertex;
			while(previousVertexDictionary[vertexInPath] != null)
			{
				vertexInPath = previousVertexDictionary[vertexInPath];
				path.Add(vertexInPath);
			}
			path.Reverse();
			return (path, finalCost);
		}

		public class WeightedEdge { 
			public Tuple<int, int> StartingVertex { get; set; }
			public Tuple<int, int> AdjacentVertex { get; set; }
			public int Weight { get; set; }
		}
	}
}
