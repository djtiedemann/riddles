using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Riddles.Probability
{
    // https://fivethirtyeight.com/features/can-you-rescue-your-crew/
    public class CrewRescuer
    {
        public class DecisionTree
        {
            public DecisionTree() {
                this.TrueRange = null;
                this.FalseRange = null;
                this.TrueTree = null;
                this.FalseTree = null;
            }

            public DecisionTree(
                List<(double, double)> trueRange,
                List<(double, double)> falseRange,
                DecisionTree trueTree,
                DecisionTree falseTree
            )
            {
                this.TrueTree = trueTree; 
                this.FalseTree = falseTree;
                this.TrueRange = trueRange;
                this.FalseRange = falseRange;
            }

            public List<(double, double)> TrueRange { get; }
            public List<(double, double)> FalseRange { get; }
            public DecisionTree TrueTree { get; }
            public DecisionTree FalseTree { get; }
        }

        private MultipleVariablesUniformDistributionProbabilityCalculator
            _multipleVariablesUniformDistributionProbabilityCalculator;
        public CrewRescuer() { 
            this._multipleVariablesUniformDistributionProbabilityCalculator =
                new MultipleVariablesUniformDistributionProbabilityCalculator();
        }

        public double CalculateProbabilityOfRescuingCrew(DecisionTree tree)
        {
            return this.CalculateProbabilityOfRescuingCrew(
                tree,
                new List<List<(double, double)>>(),
                1.0
            );
        }

        private double CalculateProbabilityOfRescuingCrew(
            DecisionTree tree,
            List<List<(double, double)>> ranges,
            double cumulativeProbabilityOfSeeingTree
        )
        {
            if(tree.TrueTree == null)
            {
                return cumulativeProbabilityOfSeeingTree
                    * this._multipleVariablesUniformDistributionProbabilityCalculator
                        .CalculateProbabilityTeamHasHighestValue(ranges).Max();
            }
            var trueTreeProbability =
                tree.TrueRange.Sum(d => d.Item2 - d.Item1);
            var falseTreeProbability =
                tree.FalseRange.Sum(d => d.Item2 - d.Item1);
            var trueTreeRanges =
                ranges.ToList();
            trueTreeRanges.Add(tree.TrueRange);
            var falseTreeRanges =
                ranges.ToList();
            falseTreeRanges.Add(tree.FalseRange);
            var probabilityOfRescuingCrewTrue =
                this.CalculateProbabilityOfRescuingCrew(
                    tree.TrueTree,
                    trueTreeRanges,
                    cumulativeProbabilityOfSeeingTree * trueTreeProbability
                );
            var probabilityOfRescuingCrewFalse =
                this.CalculateProbabilityOfRescuingCrew(
                    tree.FalseTree,
                    falseTreeRanges,
                    cumulativeProbabilityOfSeeingTree * falseTreeProbability
                );

            return probabilityOfRescuingCrewTrue + probabilityOfRescuingCrewFalse;
        }
    }
}
