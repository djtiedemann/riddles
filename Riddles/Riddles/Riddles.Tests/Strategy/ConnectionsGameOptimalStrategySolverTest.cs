using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Riddles.Strategy;

namespace Riddles.Tests.Strategy
{
    public class ConnectionsGameOptimalStrategySolverTest
    {
        //[TestCase(true)] - commented out, takes 10s. Returns a tree with 36 nodes instead of 63. depths are the same
        [TestCase(false)]
        public void TestCalculateNumGuessesNeededToSolve(bool findOptimallySmallTree)
        {
            var connectionsGameOptimalStrategySolver = new ConnectionsGameOptimalStrategySolver();
            var optimalDecisionTree = connectionsGameOptimalStrategySolver.FindOptimalDecisionTree(findOptimallySmallTree);
            var allGroups = connectionsGameOptimalStrategySolver.GetAllPossibleGroups();
            this.VerifySolution(allGroups, optimalDecisionTree);
            Assert.AreEqual(optimalDecisionTree.TreeDepth, 6);
        }

        private void VerifySolution(List<ConnectionsGameOptimalStrategySolver.ConnectionGrouping> allPossibleGroupings, ConnectionsGameOptimalStrategySolver.SubsetCalculatorDecisionTree decisionTree)
        {
            var answers = new List<(ConnectionsGameOptimalStrategySolver.ConnectionGrouping, List<ConnectionsGameOptimalStrategySolver.ConnectionGrouping>, HashSet<int>, HashSet<int>)> { };
            foreach (var grouping in allPossibleGroupings)
            {
                var currentDecisionTree = decisionTree;
                var answerForGrouping = new List<ConnectionsGameOptimalStrategySolver.ConnectionGrouping> { };
                bool foundAnswer = false;
                while (currentDecisionTree != null)
                {
                    var distance = currentDecisionTree.GuessAtNode.CalculateDistanceFromGroup(grouping.Group1);
                    answerForGrouping.Add(currentDecisionTree.GuessAtNode);
                    if (distance == ConnectionsGameOptimalStrategySolver.ConnectionDistance.Correct)
                    {
                        foundAnswer = true;
                        break;
                    }
                    else if (distance == ConnectionsGameOptimalStrategySolver.ConnectionDistance.OneAway)
                    {
                        currentDecisionTree = currentDecisionTree.DecisionTreeIfOneAway;
                    }
                    else if (distance == ConnectionsGameOptimalStrategySolver.ConnectionDistance.TwoAway)
                    {
                        currentDecisionTree = currentDecisionTree.DecisionTreeIfTwoAway;
                    }
                    else
                    {
                        throw new InvalidOperationException("Invalid distance");
                    }
                }
                if (!foundAnswer)
                {
                    throw new InvalidOperationException("Didn't find answer");
                }
                if (answerForGrouping.Count > decisionTree.TreeDepth)
                {
                    throw new InvalidOperationException("Required more guesses than anticipated");
                }
                var lastGuess = answerForGrouping.Last()!;
                var matchesGroup1LastGuess = true;
                var matchesGroup2LastGuess = true;
                if (lastGuess.Group1.Count != grouping.Group1.Count)
                {
                    matchesGroup1LastGuess = false;
                }
                if (lastGuess.Group2.Count != grouping.Group1.Count)
                {
                    matchesGroup2LastGuess = false;
                }
                foreach (var member in grouping.Group1)
                {
                    if (!lastGuess.Group1.Contains(member))
                    {
                        matchesGroup1LastGuess = false;
                    }
                    if (!lastGuess.Group2.Contains(member))
                    {
                        matchesGroup2LastGuess = false;
                    }
                }
                if (!matchesGroup1LastGuess && !matchesGroup2LastGuess)
                {
                    throw new InvalidOperationException("Last guess wouldn't work");
                }
                answers.Add((grouping, answerForGrouping, grouping.Group1, lastGuess.Group1));
            }
        }
    }
}
