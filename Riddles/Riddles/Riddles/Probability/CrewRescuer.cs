using Riddles.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Riddles.Probability
{
    // https://fivethirtyeight.com/features/can-you-rescue-your-crew/
    public class CrewRescuer
    {
        private class DecisionTree
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
        private MutuallyExclusiveCollectivelyExhaustiveRangeCreator
            _meceRangeCreator;
        public CrewRescuer() { 
            this._multipleVariablesUniformDistributionProbabilityCalculator =
                new MultipleVariablesUniformDistributionProbabilityCalculator();
            this._meceRangeCreator = new MutuallyExclusiveCollectivelyExhaustiveRangeCreator();
        }

        public double CalculateProbabilityOfRescuingCrew(
            List<(double, double)> question1,
            List<(double, double)> question2True,
            List<(double, double)> question2False,
            List<(double, double)> question3TrueTrue,
            List<(double, double)> question3TrueFalse,
            List<(double, double)> question3FalseTrue,
            List<(double, double)> question3FalseFalse
        )
        {
            var tree = this.CreateDecisionTreeFromQuestions(
                question1,
                question2True,
                question2False,
                question3TrueTrue,
                question3TrueFalse,
                question3FalseTrue,
                question3FalseFalse
            );
            return this.CalculateProbabilityOfRescuingCrew(
                tree,
                new List<List<(double, double)>>(),
                1.0
            );
        }

        // this is hacky and terrible
        private DecisionTree CreateDecisionTreeFromQuestions(
            List<(double, double)> question1,
            List<(double, double)> question2True,
            List<(double, double)> question2False,
            List<(double, double)> question3TrueTrue,
            List<(double, double)> question3TrueFalse,
            List<(double, double)> question3FalseTrue,
            List<(double, double)> question3FalseFalse
        )
        {
            return new DecisionTree(
                question1,
                this._meceRangeCreator.CreateComplementaryRanges((0, 1), question1),
                new DecisionTree(
                    question2True,
                    this._meceRangeCreator.CreateComplementaryRanges((0, 1), question2True),
                    new DecisionTree(
                        question3TrueTrue,
                        this._meceRangeCreator.CreateComplementaryRanges((0, 1), question3TrueTrue),
                        new DecisionTree(),
                        new DecisionTree()
                    ),
                    new DecisionTree(
                        question3TrueFalse,
                        this._meceRangeCreator.CreateComplementaryRanges((0, 1), question3TrueFalse),
                        new DecisionTree(),
                        new DecisionTree()
                    )
                ),
                new DecisionTree(
                    question2False,
                    this._meceRangeCreator.CreateComplementaryRanges((0, 1), question2False),
                    new DecisionTree(
                        question3FalseTrue,
                        this._meceRangeCreator.CreateComplementaryRanges((0, 1), question3FalseTrue),
                        new DecisionTree(),
                        new DecisionTree()
                    ),
                    new DecisionTree(
                        question3FalseFalse,
                        this._meceRangeCreator.CreateComplementaryRanges((0, 1), question3FalseFalse),
                        new DecisionTree(),
                        new DecisionTree()
                    )
                )
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
                var maxProbabilityWinning = this._multipleVariablesUniformDistributionProbabilityCalculator
                        .CalculateProbabilityTeamHasHighestValue(ranges).Max();
                var rescueProbability = cumulativeProbabilityOfSeeingTree
                    * maxProbabilityWinning;
                return rescueProbability;
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

            var rescureProbability =  probabilityOfRescuingCrewTrue + probabilityOfRescuingCrewFalse;
            return rescureProbability;
        }
    }
}
