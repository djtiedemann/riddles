using Riddles.Algebra.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Riddles.Combinatorics.Application
{
    public class DigitGameNumDistinctEquationsCalculator
    {
        public (int, Dictionary<int, HashSet<Expression>>) 
            CalculateNumDistinctEquations(int numVariables)
        {
            var variables = Enumerable.Range(0, numVariables)
                .Select(i => ((char)('A' + i)).ToString())
                .ToList();

            var expressionsByNumVariables = new Dictionary<int, HashSet<Expression>>();
            var expressionsWithSingleVariable = variables
                .Select(v => (Expression)new Term(v))
                .ToHashSet();
            expressionsByNumVariables[1] = expressionsWithSingleVariable;

            for (int numVarsInIteration=2; 
                numVarsInIteration<=numVariables; 
                numVarsInIteration++)
            {
                HashSet<Expression> expressionsForNumVars = new HashSet<Expression>();
                for(int numVarsInPastIteration=1;
                    numVarsInPastIteration < numVarsInIteration;
                    numVarsInPastIteration++)
                {
                    var numVarsInPastIteration2 = numVarsInIteration - numVarsInPastIteration;
                    foreach(var expression1 in expressionsByNumVariables[numVarsInPastIteration])
                    {
                        foreach(var expression2 in expressionsByNumVariables[numVarsInPastIteration2])
                        {
                            // if the expression share any variables, they cannot be combined
                            if (expression1.Terms.Intersect(expression2.Terms).ToList().Count > 0)
                            {
                                continue;
                            }
                            var addition =
                                new Sum(
                                    new List<Expression> { expression1.Copy(), expression2.Copy() },
                                    new List<Expression>(),
                                    simplifyExpression: true
                                );
                            var subtraction =
                                new Sum(
                                    new List<Expression> { expression1.Copy() },
                                    new List<Expression> { expression2.Copy() },
                                    simplifyExpression: true
                                );
                            var multiplication =
                                new Product(
                                    new List<Expression> { expression1.Copy(), expression2.Copy() },
                                    new List<Expression>(),
                                    simplifyExpression: true
                                );
                            var division =
                                new Product(
                                    new List<Expression> { expression1.Copy() },
                                    new List<Expression> { expression2.Copy() },
                                    simplifyExpression: true
                                );
                            expressionsForNumVars.Add(addition);
                            expressionsForNumVars.Add(subtraction);
                            expressionsForNumVars.Add(multiplication);
                            expressionsForNumVars.Add(division);
                        }
                    }
                }
                expressionsByNumVariables[numVarsInIteration] = expressionsForNumVars;
            }

            var totalNumExpressions = 0;
            foreach(var key in expressionsByNumVariables.Keys)
            {
                totalNumExpressions += expressionsByNumVariables[key].Count;
            }
            return (totalNumExpressions, expressionsByNumVariables);
        }
    }
}
