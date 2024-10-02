using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Riddles.Graphs.Core
{
    public class EulerianCircuitFinder
    {
        public EulerianCircuitFinder() { }

        public List<List<string>> FindCircuits(Dictionary<string, HashSet<string>> adjacencies)
        {
            var numEdges = adjacencies.Sum(
                g => g.Value.Count
            );

            var circuits = new List<List<string>>();
            foreach(var startingNode in adjacencies.Keys)
            {
                var newCircuits = this.GetCircuits(
                    startingNode,
                    adjacencies,
                    new List<string> { startingNode },
                    new List<List<string>>(),
                    numEdges
                );
                circuits.AddRange( newCircuits );
            }
            return circuits;
        }

        private List<List<string>> GetCircuits(
            string currentNode,
            Dictionary<string, HashSet<string>> adjacencies,
            List<string> currentCircuit,
            List<List<string>> allCircuits,
            int numEdges
        )
        {
            // if we've travelled across all edges, add the circuit to the list
            if(currentCircuit.Count() == numEdges + 1)
            {
                allCircuits.Add(currentCircuit);
            }
            // if there are no remaining edges from here, return
            if (adjacencies[currentNode].Count == 0)
            {
                return allCircuits;
            }

            var nextNodes = adjacencies[currentNode].ToList();
            foreach(var nextNode in nextNodes)
            {
                var clonedCircuit = currentCircuit.ToList();
                clonedCircuit.Add(nextNode);
                adjacencies[currentNode].Remove(nextNode);
                this.GetCircuits(
                    nextNode,
                    adjacencies,
                    clonedCircuit,
                    allCircuits,
                    numEdges
                );
                // add the edge back to the potential list of edges
                adjacencies[currentNode].Add(nextNode);
            }
            return allCircuits;
        }
    }
}
