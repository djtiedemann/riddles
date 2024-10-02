using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Riddles.Graphs.Core
{
    public class EulerianPathFinder
    {
        public EulerianPathFinder() { }

        public List<List<string>> FindPaths(Dictionary<string, HashSet<string>> adjacencies)
        {
            var numEdges = adjacencies.Sum(
                g => g.Value.Count
            );

            var paths = new List<List<string>>();
            foreach(var startingNode in adjacencies.Keys)
            {
                var newPaths = this.GetPaths(
                    startingNode,
                    adjacencies,
                    new List<string> { startingNode },
                    new List<List<string>>(),
                    numEdges
                );
                paths.AddRange( newPaths );
            }
            return paths;
        }

        private List<List<string>> GetPaths(
            string currentNode,
            Dictionary<string, HashSet<string>> adjacencies,
            List<string> currentPath,
            List<List<string>> allPaths,
            int numEdges
        )
        {
            // if we've travelled across all edges, add the path to the list
            if(currentPath.Count() == numEdges + 1)
            {
                allPaths.Add(currentPath);
            }
            // if there are no remaining paths from here, return
            if (adjacencies[currentNode].Count == 0)
            {
                return allPaths;
            }

            var nextNodes = adjacencies[currentNode].ToList();
            foreach(var nextNode in nextNodes)
            {
                var clonedPath = currentPath.ToList();
                clonedPath.Add(nextNode);
                adjacencies[currentNode].Remove(nextNode);
                this.GetPaths(
                    nextNode,
                    adjacencies,
                    clonedPath,
                    allPaths,
                    numEdges
                );
                // add the edge back to the potential list of edges
                adjacencies[currentNode].Add(nextNode);
            }
            return allPaths;
        }
    }
}
