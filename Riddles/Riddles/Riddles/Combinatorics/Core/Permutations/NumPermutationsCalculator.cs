using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Riddles.Combinatorics.Core.Permutations
{
    public class NumPermutationsCalculator
    {
        private FactorialCalculator _factorialCalculator;
        public NumPermutationsCalculator() { 
            this._factorialCalculator = new FactorialCalculator();
        }

        /// <summary>
        /// Calculates the number of permutations of the elements provided, taking into account duplicate items
        /// </summary>
        /// <returns></returns>
        public int CalculateNumPermutations(Dictionary<string, int> elements)
        {
            var totalNumElements = elements.Values.Sum();
            var duplicateOrderingCorrection = elements.Values.Where(v => v > 1).ToList().OrderByDescending(x => x).ToList();
            var factorial = this._factorialCalculator.FactorialDivision(totalNumElements, duplicateOrderingCorrection[0]);
            for(int i = 1; i<duplicateOrderingCorrection.Count; i++)
            {
                factorial = factorial / this._factorialCalculator.Factorial(duplicateOrderingCorrection[i]);
            }
            return (int)Math.Round(factorial);
        }
    }
}
