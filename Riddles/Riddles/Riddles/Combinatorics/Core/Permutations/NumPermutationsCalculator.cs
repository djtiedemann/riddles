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

        public int CalculateNumPermutations(string element)
        {
            var characterCounts = 
                element.ToCharArray().GroupBy(g => $"{g}")
                    .ToDictionary(g => g.Key, g => g.Count());
            return this.CalculateNumPermutations(characterCounts);
        }

        /// <summary>
        /// Calculates the number of permutations of the elements provided, taking into account duplicate items
        /// </summary>
        /// <returns></returns>
        public int CalculateNumPermutations(Dictionary<string, int> elements)
        {
            var totalNumElements = elements.Values.Sum();
            if(totalNumElements == 0)
            {
                return 0;
            }
            var duplicateOrderingCorrection = elements.Values.Where(v => v > 1).ToList().OrderByDescending(x => x).ToList();
            var factorial = this._factorialCalculator.FactorialDivision(
                totalNumElements, 
                duplicateOrderingCorrection.Count > 0 
                    ? duplicateOrderingCorrection[0]
                    : 1
            );
            for(int i = 1; i<duplicateOrderingCorrection.Count; i++)
            {
                factorial = factorial / this._factorialCalculator.Factorial(duplicateOrderingCorrection[i]);
            }
            return (int)Math.Round(factorial);
        }
    }
}
