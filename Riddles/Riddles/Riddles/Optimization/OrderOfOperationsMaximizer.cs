using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Riddles.Combinatorics.Core.Sets;

namespace Riddles.Optimization
{
    public class OrderOfOperationsMaximizer
    {
        private SubsetCalculator _subsetCalculator;
        public OrderOfOperationsMaximizer() { 
            this._subsetCalculator = new SubsetCalculator();
        }

        public (double, Equation, double) CalculateMaximumValue(
            List<double> values, 
            List<Operation> operations,
            bool shouldPruneUnnecessaryEquations)
        {
            var allEquations = this.GenerateAllEquations(
                values, 
                operations,
                shouldPruneUnnecessaryEquations
            );
            this.DebugEquations(allEquations);
            Equation bestEquation = null;
            double bestValue = double.MinValue;

            foreach(var equation in allEquations)
            {
                if(equation.Value > bestValue && !double.IsInfinity(equation.Value))
                {
                    bestEquation = equation;
                    bestValue = equation.Value;
                }
            }
            return (bestValue, bestEquation, allEquations.Count());
        }

        private List<Equation> GenerateAllEquations(List<double> values, List<Operation> operations, bool shouldPruneUnnecessaryEquations) {
            
            var allEquations = new List<Equation>();
            if(values.Count() == 1)
            {
                allEquations.Add(new Equation(values.Single()));
                return allEquations;
            }
            foreach(var operation in operations)
            {
                var otherOperations = operations.Where(o => o != operation);
                // if the operation is symmetric, you can say the smallest
                // number is always on the left
                for(int numOperationsOnLeft=0; numOperationsOnLeft<=otherOperations.Count(); numOperationsOnLeft++)
                {
                    var numOperationsOnRight = otherOperations.Count() - numOperationsOnLeft;
                    var otherOperatorsSubsets = this._subsetCalculator
                        .CalculateSubsets(
                            new List<int> { numOperationsOnLeft, numOperationsOnRight }
                        );
                    foreach (var otherOperatorSubet in otherOperatorsSubsets) {
                        var leftOperatorSubsetIndicators = otherOperatorSubet[0];
                        var rightOperatorSubsetIndicators = otherOperatorSubet[1];
                        var leftOperatorSubset =
                            otherOperations.Where((x, i)
                            => leftOperatorSubsetIndicators.Contains(i));
                        var rightOperatorSubset =
                            otherOperations.Where((x, i)
                            => rightOperatorSubsetIndicators.Contains(i));
                        var numValuesOnLeft = numOperationsOnLeft + 1;
                        var numValuesOnRight = numOperationsOnRight + 1;
                        var valuesSubsets = this._subsetCalculator.
                            CalculateSubsets(
                                new List<int> { numValuesOnLeft, numValuesOnRight }
                            );
                        foreach (var valueSubset in valuesSubsets)
                        {
                            var leftValueSubsetIndicators = valueSubset[0];
                            var rightValueSubsetIndicators = valueSubset[1];
                            var leftValueSubset = values
                                .Where((x, i) => leftValueSubsetIndicators.Contains(i));
                            var rightValueSubset = values
                                .Where((x, i) => rightValueSubsetIndicators.Contains(i));

                            // for addition and multiplication, the results are symmetric
                            // so ignore duplicate equations
                            // (where the minimum value is on the right hand side)
                            if (shouldPruneUnnecessaryEquations &&
                                (operation == Operation.Addition || operation == Operation.Multiplication)) { 
                                if(leftValueSubset.Min() > rightValueSubset.Min())
                                {
                                    continue;
                                }
                            }

                            var leftEquations = this
                                .GenerateAllEquations(
                                    leftValueSubset.ToList(), 
                                    leftOperatorSubset.ToList(),
                                    shouldPruneUnnecessaryEquations
                                );
                            var rightEquations = this.GenerateAllEquations(
                                    rightValueSubset.ToList(),
                                    rightOperatorSubset.ToList(),
                                    shouldPruneUnnecessaryEquations
                                );
                            foreach (var leftEquation in leftEquations)
                            { 
                                foreach(var rightEquation in rightEquations)
                                {
                                    allEquations.Add(
                                        new Equation(
                                            operation, 
                                            leftEquation, 
                                            rightEquation
                                        )    
                                    );
                                }
                            }
                        }
                    }
                }
            }
            return allEquations;
        }

        // the number of equations is determined by
        // 1) the number of ways to structure the parentheses for the operators
        // 2) the number of ways to order the operators within a structure
        // 3) the number of ways to order the values within the structure
        // the product of these three values is the number of equations produced
        // by the most naive implementation
        private void DebugEquations(List<Equation> equations)
        {
            var numDistinctWaysToFormParenthesis = equations.Select(e => (e, e.ToString()
            .Select((x, i) => (x, i))
            .Where(x => x.x == '(' || x.x == ')')
            .Aggregate("", (agg, x) => $"{agg}_{x.i}")))
            .GroupBy(x => x.Item2).ToList();
        }

        public enum Operation
        {
            Addition = 0,
            Subtraction = 1,
            Multiplication = 2, 
            Division = 3
        }

        public class Equation
        {
            public double Value { get; }
            public string WrittenEquation { get; }
            public Equation(double value)
            {
                this.Value = value;
                this.WrittenEquation = $"{value}";
            }
            public Equation(
                Operation operation, 
                Equation leftEquation,
                Equation rightEquation
            ) {
                switch (operation) { 
                    case Operation.Addition: 
                        this.Value = leftEquation.Value + rightEquation.Value;
                        this.WrittenEquation = $"({leftEquation}+{rightEquation})";
                        break;
                    case Operation.Subtraction:
                        this.Value = leftEquation.Value - rightEquation.Value;
                        this.WrittenEquation = $"({leftEquation}-{rightEquation})";
                        break;
                    case Operation.Multiplication:
                        this.Value = leftEquation.Value * rightEquation.Value;
                        this.WrittenEquation = $"({leftEquation}*{rightEquation})";
                        break;
                    case Operation.Division:
                        this.Value = leftEquation.Value / rightEquation.Value;
                        this.WrittenEquation = $"({leftEquation}/{rightEquation})";
                        break;
                    default:
                        throw new ArgumentException("Operation is unexpected");
                }
            }

            public override string ToString()
            {
                return this.WrittenEquation;
            }
        }
    }
}