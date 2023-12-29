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

        public int CalculateNumGuessesNeededToSolve()
        {
            var allPossibleGroups = this._subsetCalculator.CalculateSubsets(
                new List<int> { 4, 4 }
            ).Select(g => new ConnectionGrouping(g[0], g[1]));
            var equivalenceTags = Enumerable.Range(0, 8).ToDictionary(x => x,x => string.Empty);
            var optimalDecisionTree = new SubsetCalculatorDecisionTree(allPossibleGroups, equivalenceTags, allPossibleGroups);
            var debugMessages = optimalDecisionTree.CalculateInOrderTraversal(0, new List<string>(), "None");
            return int.MaxValue;
        }

        public class SubsetCalculatorDecisionTree
        {
            public SubsetCalculatorDecisionTree(IEnumerable<ConnectionGrouping> possibleGroupings, Dictionary<int, string> equivalenceTags, IEnumerable<ConnectionGrouping> allPossibleGroupings) 
            {
                var subtree = this.ConstructTree(possibleGroupings.ToList(), equivalenceTags, allPossibleGroupings.ToList());
                this.GuessAtNode = subtree.Item1;
                this.GroupingsSolvedAtThisNode = possibleGroupings.Where(g => g.CalculateDistanceFromGroup(this.GuessAtNode.Group1) == ConnectionDistance.Correct).ToHashSet();
                this.DecisionTreeIfOneAway = subtree.Item2;
                this.DecisionTreeIfTwoAway = subtree.Item3;
            }

            public int CalculateTreeDepth()
            {
                var leftTreeDepth = this.DecisionTreeIfOneAway != null ? this.DecisionTreeIfOneAway.CalculateTreeDepth() : 0;
                var rightTreeDepth = this.DecisionTreeIfTwoAway != null ? this.DecisionTreeIfTwoAway.CalculateTreeDepth() : 0;
                return 1 + Math.Max(leftTreeDepth, rightTreeDepth);
            }

            public List<string> CalculateInOrderTraversal(int depth, List<string> message, string lastGroup)
            {
                var group = this.GuessAtNode.Group1.Aggregate(string.Empty, (agg, v) => $"{agg}{v}")
                    + this.GuessAtNode.Group2.Aggregate(string.Empty, (agg, v) => $"{agg}{v}");
                message.Add($"Depth: {depth}, Parent: {lastGroup}, Group: {group}");
                if (this.DecisionTreeIfOneAway != null) {
                    this.DecisionTreeIfOneAway.CalculateInOrderTraversal(depth + 1, message, group);
                } if (this.DecisionTreeIfTwoAway != null) {
                    this.DecisionTreeIfTwoAway.CalculateInOrderTraversal(depth + 1, message, lastGroup);
                }
                return message;
            }

            public ConnectionGrouping GuessAtNode { get; }
            public HashSet<ConnectionGrouping> GroupingsSolvedAtThisNode { get; }
            public SubsetCalculatorDecisionTree DecisionTreeIfOneAway { get; }
            public SubsetCalculatorDecisionTree DecisionTreeIfTwoAway { get; }

            private (ConnectionGrouping, SubsetCalculatorDecisionTree, SubsetCalculatorDecisionTree) 
                ConstructTree(List<ConnectionGrouping> possibleGroupings, Dictionary<int, string> equivalenceTags, List<ConnectionGrouping> allPossibleGroupings)
            {
                var bestPossibleDepth = this.CalculateBestDepth(possibleGroupings);
                var groupingsToTry = new HashSet<ConnectionGrouping>();
                var relevantGroupingCache = new HashSet<string>();
                // we don't want to try all combinations of groupings because many of them are functionally the same. For instance, on the first try there's only one relevant grouping
                // two values are effectively the same if they've been grouped together in every past iteration
                foreach (var grouping in possibleGroupings)
                {
                    var equivalenceKey1 = grouping.Group1.Select(c => equivalenceTags[c]).OrderBy(x => x).Aggregate(string.Empty, (agg, x) => $"{agg}{x}") 
                        + grouping.Group2.Select(c => equivalenceTags[c]).OrderBy(x => x).Aggregate(string.Empty, (agg, x) => $"{agg}{x}");
                    var equivalenceKey2 = grouping.Group2.Select(c => equivalenceTags[c]).OrderBy(x => x).Aggregate(string.Empty, (agg, x) => $"{agg}{x}")
                        + grouping.Group1.Select(c => equivalenceTags[c]).OrderBy(x => x).Aggregate(string.Empty, (agg, x) => $"{agg}{x}");
                    if (!relevantGroupingCache.Contains(equivalenceKey1) && !relevantGroupingCache.Contains(equivalenceKey2))
                    {
                        relevantGroupingCache.Add(equivalenceKey1);
                        groupingsToTry.Add(grouping);
                    }
                }

                int bestDepth = int.MaxValue;
                ConnectionGrouping bestGrouping = null;
                SubsetCalculatorDecisionTree bestDecisionTreeIfOneAway = null;
                SubsetCalculatorDecisionTree bestDecisionTreeIfTwoAway = null;
                foreach (var grouping in groupingsToTry)
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
                        } else if (distance == ConnectionDistance.TwoAway)
                        {
                            groupingsTwoAway.Add(possibleGrouping);
                        }
                    }

                    // don't try something that doesn't actually change the groupings
                    if (groupingsOneAway.Count() == possibleGroupings.Count() || groupingsTwoAway.Count() == possibleGroupings.Count()) {
                        continue;
                    }

                    var newEquivalenceTags = equivalenceTags.ToDictionary(
                        k => k.Key,
                        k => grouping.Group1.Contains(k.Key) ? $"{k.Value}1" : $"{k.Value}2"
                    );
                    SubsetCalculatorDecisionTree decisionTreeIfOneAway = groupingsOneAway.Any() 
                        ? new SubsetCalculatorDecisionTree(groupingsOneAway, newEquivalenceTags, allPossibleGroupings) 
                        : null;
                    SubsetCalculatorDecisionTree decisionTreeIfTwoAway = groupingsTwoAway.Any()
                        ? new SubsetCalculatorDecisionTree(groupingsTwoAway, newEquivalenceTags, allPossibleGroupings)
                        : null;

                    var leftTreeDepth = decisionTreeIfOneAway != null ? decisionTreeIfOneAway.CalculateTreeDepth() : 0;
                    var rightTreeDepth = decisionTreeIfTwoAway != null ? decisionTreeIfTwoAway.CalculateTreeDepth() : 0;
                    var depth = 1 + Math.Max(leftTreeDepth, rightTreeDepth);
                    if(depth < bestDepth)
                    {
                        bestGrouping = grouping;
                        bestDecisionTreeIfOneAway = decisionTreeIfOneAway;
                        bestDecisionTreeIfTwoAway = decisionTreeIfTwoAway;
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
