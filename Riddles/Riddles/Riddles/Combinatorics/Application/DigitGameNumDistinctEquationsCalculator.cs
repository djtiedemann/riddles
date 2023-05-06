using Riddles.Combinatorics.Core.SetGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Riddles.Combinatorics.Application
{
    public class DigitGameNumDistinctEquationsCalculator
    {
        private double epsilon = 0.00000000001;
        
        public DigitGameNumDistinctEquationsCalculator() { 
        }

        
        public int CalculateNumDistinctEquations(double[] variables)
        {
            var outputValues = new HashSet<double>();
            foreach(var variable in variables)
            {
                outputValues.Add(variable);
            }
            this.CalculateNumDistinctEquations(variables, outputValues);
            var outputList = outputValues.ToList().OrderBy(x => x).ToList();
            var finalList = new List<double>();
            for(int i=0; i<outputList.Count() - 1; i++)
            {
                if (
                    (Math.Abs(outputList[i] - outputList[i + 1]) 
                    / Math.Abs(outputList[i+1])) > this.epsilon )
                {
                    finalList.Add(outputList[i]);
                }
            }
            finalList.Add(outputList.Last());
            return finalList.Count;
        }

        private void CalculateNumDistinctEquations(
            double[] variables, 
            HashSet<double> outputValues)
        {
            if(variables.Length == 1)
            {
                foreach(var value in variables)
                {
                    outputValues.Add(value);
                }
                return;
            }
            for(int i=0; i<variables.Length; i++)
            {
                for (int j=0; j<variables.Length; j++)
                {
                    if(j == i)
                    {
                        continue;
                    }
                    var newVariables = variables.Where((x, index)
                        => index != i && index != j);
                    var sum = variables[i] + variables[j];
                    var difference = variables[i] - variables[j];
                    var product = variables[i] * variables[j];
                    var quotient = variables[i] / variables[j];
                    outputValues.Add(difference);
                    outputValues.Add(quotient);
                    this.CalculateNumDistinctEquations(
                        newVariables.Concat(new List<double> { difference }
                    ).ToArray(), outputValues);
                    this.CalculateNumDistinctEquations(
                        newVariables.Concat(new List<double> { quotient }
                    ).ToArray(), outputValues);
                    if (j > i)
                    {
                        outputValues.Add(sum);
                        outputValues.Add(product);
                        this.CalculateNumDistinctEquations(
                            newVariables.Concat(new List<double> { sum }
                        ).ToArray(), outputValues);
                        this.CalculateNumDistinctEquations(
                            newVariables.Concat(new List<double> { product }
                        ).ToArray(), outputValues);
                    }
                }
            }
        }
    }
}
