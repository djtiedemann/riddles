using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Riddles.Combinatorics.Core.Sets;

namespace Riddles.Strategy
{
    public class ConnectionsGameOptimalStrategySolver
    {
        private SubsetCalculator _subsetCalculator;
        public ConnectionsGameOptimalStrategySolver() { 
            this._subsetCalculator = new SubsetCalculator();
        }

        public SubsetCalculatorDecisionTree FindOptimalDecisionTree()
        {
            var allPossibleGroups = this.GetAllPossibleGroups();
            var equivalenceTags = Enumerable.Range(0, 8).ToDictionary(x => x,x => string.Empty);
            var optimalDecisionTree = new SubsetCalculatorDecisionTree(allPossibleGroups, equivalenceTags, allPossibleGroups, 0, int.MaxValue, int.MaxValue);
            return optimalDecisionTree;
        }

        public List<ConnectionGrouping> GetAllPossibleGroups() {
            return this._subsetCalculator.CalculateSubsets(
                new List<int> { 4, 4 }
            ).Select(g => new ConnectionGrouping(g[0], g[1])).ToList();
        }

        public class SubsetCalculatorDecisionTree
        {
            public SubsetCalculatorDecisionTree(
                IEnumerable<ConnectionGrouping> possibleGroupings, 
                Dictionary<int, string> equivalenceTags, 
                IEnumerable<ConnectionGrouping> allPossibleGroupings, 
                int currentDepth, 
                int maxAllowedDepth, 
                int bestNumNodes) 
            {
                if(currentDepth >= maxAllowedDepth)
                {
                    this.IsTerminatedForBeingTooDeep = true;
                    return;
                }
                this.IsTerminatedForBeingTooDeep = false;
                var (group, subtreeIfOneAway, subtreeIfTwoAway) = this.ConstructTree(possibleGroupings.ToList(), equivalenceTags, allPossibleGroupings.ToList(), currentDepth, maxAllowedDepth, bestNumNodes);
                if (group == null) {
                    this.IsTerminatedForBeingTooDeep = true;
                    return;
                }
                this.GuessAtNode = group;
                this.StringGuessAtNode = this.GuessAtNode.Group1.Aggregate(string.Empty, (agg, v) => $"{agg}{v}")
                    + " " + this.GuessAtNode.Group2.Aggregate(string.Empty, (agg, v) => $"{agg}{v}");
                this.GroupingsSolvedAtThisNode = possibleGroupings.Where(g => g.CalculateDistanceFromGroup(this.GuessAtNode.Group1) == ConnectionDistance.Correct).ToHashSet();
                this.DecisionTreeIfOneAway = subtreeIfOneAway;
                this.DecisionTreeIfTwoAway = subtreeIfTwoAway;
                this.TreeDepth = this.CalculateTreeDepth();
                this.NodeCount = this.CalculateNodeCount();
            }

            public bool IsTerminatedForBeingTooDeep { get; }

            public string StringGuessAtNode { get; }

            public int TreeDepth { get; }

            public int NodeCount { get; }

            private int CalculateTreeDepth()
            {
                var leftTreeDepth = this.DecisionTreeIfOneAway != null ? this.DecisionTreeIfOneAway.TreeDepth : 0;
                var rightTreeDepth = this.DecisionTreeIfTwoAway != null ? this.DecisionTreeIfTwoAway.TreeDepth : 0;
                return 1 + Math.Max(leftTreeDepth, rightTreeDepth);
            }

            private int CalculateNodeCount()
            {
                var leftNodeCount = this.DecisionTreeIfOneAway != null ? this.DecisionTreeIfOneAway.NodeCount : 0;
                var rightNodeCount = this.DecisionTreeIfTwoAway != null ? this.DecisionTreeIfTwoAway.NodeCount : 0;
                return 1 + leftNodeCount + rightNodeCount;
            }

            public ConnectionGrouping GuessAtNode { get; }
            public HashSet<ConnectionGrouping> GroupingsSolvedAtThisNode { get; }
            public SubsetCalculatorDecisionTree DecisionTreeIfOneAway { get; }
            public SubsetCalculatorDecisionTree DecisionTreeIfTwoAway { get; }

            private (ConnectionGrouping, SubsetCalculatorDecisionTree, SubsetCalculatorDecisionTree) ConstructTree(
                List<ConnectionGrouping> possibleGroupings, 
                Dictionary<int, string> equivalenceTags, 
                List<ConnectionGrouping> allPossibleGroupings, 
                int currentDepth, 
                int maxAllowedDepth, 
                int bestNumNodes
            )
            {
                var bestPossibleDepth = this.CalculateBestDepth(possibleGroupings);
                var groupingsToTry = new List<(ConnectionGrouping, List<ConnectionGrouping>, List<ConnectionGrouping>, Dictionary<int, string>)>();
                var relevantGroupingCache = new HashSet<string>();
                // we don't want to try all combinations of groupings because many of them are functionally the same. For instance, on the first try there's only one relevant grouping
                // two values are effectively the same if they've been grouped together in every past iteration
                foreach (var grouping in allPossibleGroupings)
                {
                    var equivalenceKey1 = grouping.Group1.Select(c => equivalenceTags[c]).OrderBy(x => x).Aggregate(string.Empty, (agg, x) => $"{agg}{x}") 
                        + grouping.Group2.Select(c => equivalenceTags[c]).OrderBy(x => x).Aggregate(string.Empty, (agg, x) => $"{agg}{x}");
                    var equivalenceKey2 = grouping.Group2.Select(c => equivalenceTags[c]).OrderBy(x => x).Aggregate(string.Empty, (agg, x) => $"{agg}{x}")
                        + grouping.Group1.Select(c => equivalenceTags[c]).OrderBy(x => x).Aggregate(string.Empty, (agg, x) => $"{agg}{x}");
                    if (!relevantGroupingCache.Contains(equivalenceKey1) && !relevantGroupingCache.Contains(equivalenceKey2))
                    {
                        List<ConnectionGrouping> groupingsOneAway = new List<ConnectionGrouping>();
                        List<ConnectionGrouping> groupingsTwoAway = new List<ConnectionGrouping>();
                        // split the possible groupings at this point into those that are one away from this grouping and one that are two away
                        foreach (var possibleGrouping in possibleGroupings)
                        {
                            var distance = possibleGrouping.CalculateDistanceFromGroup(grouping.Group1);
                            if (distance == ConnectionDistance.OneAway)
                            {
                                groupingsOneAway.Add(possibleGrouping);
                            }
                            else if (distance == ConnectionDistance.TwoAway)
                            {
                                groupingsTwoAway.Add(possibleGrouping);
                            }
                        }

                        // if all possible groupings are determined by this key, simply return it since it is the correct guess
                        if (groupingsOneAway.Count == 0 && groupingsTwoAway.Count == 0) {
                            return (grouping, null, null);
                        }

                        var newEquivalenceTags = equivalenceTags.ToDictionary(
                            k => k.Key,
                            k => grouping.Group1.Contains(k.Key) ? $"{k.Value}1" : $"{k.Value}2"
                        );
                        relevantGroupingCache.Add(equivalenceKey1);
                        groupingsToTry.Add((grouping, groupingsOneAway, groupingsTwoAway, newEquivalenceTags));
                    }
                }

                int bestDepth = maxAllowedDepth;
                int currentBestNumNodes = bestNumNodes;
                ConnectionGrouping bestGrouping = null;
                SubsetCalculatorDecisionTree bestDecisionTreeIfOneAway = null;
                SubsetCalculatorDecisionTree bestDecisionTreeIfTwoAway = null;
                // first try groupings that are most likely to split the trees evenly, as those are most likely to lead to shallow trees.
                var orderedGroupingsToTry = groupingsToTry.OrderBy(x => Math.Abs(x.Item2.Count - x.Item3.Count)).ToList();
                foreach (var (grouping, groupingsOneAway, groupingsTwoAway, newEquivalenceTags) in orderedGroupingsToTry)
                {
                    // don't try something that doesn't actually change the groupings
                    if (groupingsOneAway.Count() == possibleGroupings.Count() || groupingsTwoAway.Count() == possibleGroupings.Count()) {
                        continue;
                    }

                    SubsetCalculatorDecisionTree decisionTreeIfOneAway = groupingsOneAway.Any() 
                        ? new SubsetCalculatorDecisionTree(groupingsOneAway, newEquivalenceTags, allPossibleGroupings, currentDepth, bestDepth, currentBestNumNodes) 
                        : null;
                    SubsetCalculatorDecisionTree decisionTreeIfTwoAway = groupingsTwoAway.Any()
                        ? new SubsetCalculatorDecisionTree(groupingsTwoAway, newEquivalenceTags, allPossibleGroupings, currentDepth, bestDepth, currentBestNumNodes)
                        : null;

                    if(decisionTreeIfOneAway?.IsTerminatedForBeingTooDeep == true || decisionTreeIfTwoAway?.IsTerminatedForBeingTooDeep == true)
                    {
                        continue;
                    }
                    var leftTreeDepth = decisionTreeIfOneAway != null ? decisionTreeIfOneAway.TreeDepth : 0;
                    var rightTreeDepth = decisionTreeIfTwoAway != null ? decisionTreeIfTwoAway.TreeDepth : 0;
                    var depth = 1 + Math.Max(leftTreeDepth, rightTreeDepth);
                    var leftNumNodes = decisionTreeIfOneAway != null ? decisionTreeIfOneAway.NodeCount : 0;
                    var rightNumNodes = decisionTreeIfTwoAway != null ? decisionTreeIfTwoAway.NodeCount : 0;
                    var nodeCount = 1 + leftNumNodes + rightNumNodes; 
                    if (depth < bestDepth || (depth == bestDepth && nodeCount < currentBestNumNodes))
                    {
                        bestGrouping = grouping;
                        bestDecisionTreeIfOneAway = decisionTreeIfOneAway;
                        bestDecisionTreeIfTwoAway = decisionTreeIfTwoAway;
                        bestDepth = depth;
                        currentBestNumNodes = nodeCount;
                        if (bestDepth == bestPossibleDepth)
                        {
                            break;
                        }
                    }

                }
                return (bestGrouping, bestDecisionTreeIfOneAway, bestDecisionTreeIfTwoAway);
            }

            private int CalculateBestDepth(IEnumerable<ConnectionGrouping> possibleGroupings)
            {
                // due to the symmetry of the problem, each guess eliminates 2 pairs
                int currentNumGroupings = (possibleGroupings.Count() / 2);
                int bestDepth = 0;
                while(currentNumGroupings != 0)
                {
                    currentNumGroupings = currentNumGroupings >> 1;
                    bestDepth++;
                }
                return bestDepth;
            }
        }

        public enum ConnectionDistance
        {
            Correct = 1,
            OneAway = 2,
            TwoAway = 3
        }

        public class ConnectionGrouping { 
            public ConnectionGrouping(
                HashSet<int> group1,
                HashSet<int> group2
            )
            {
                this.Group1 = group1;
                this.Group2 = group2;
            }

            public HashSet<int> Group1 { get; }
            public HashSet<int> Group2 { get; }

            public ConnectionDistance CalculateDistanceFromGroup(HashSet<int> group)
            {
                int numMatches = 0;
                foreach(var member in group)
                {
                    if (this.Group1.Contains(member))
                    {
                        numMatches++;
                    }
                }
                switch (numMatches)
                {
                    case 0:
                    case 4:
                        return ConnectionDistance.Correct;
                    case 1:
                    case 3:
                        return ConnectionDistance.OneAway;
                    case 2:
                        return ConnectionDistance.TwoAway;
                    default:
                        throw new InvalidOperationException("Incorrect set sizes");
                }
            }
        }
    }
}
