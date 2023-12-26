using System;
using System.Collections.Generic;
using System.Text;
using Riddles.Combinatorics.Core;

namespace Riddles.Probability.Application
{
    public class ConnectionGameSolver
    {
        private BinomialTheoremCalculator _binomialTheoremCalculator;
        public ConnectionGameSolver() { 
            this._binomialTheoremCalculator = new BinomialTheoremCalculator();
        }

        public double CalculateOddsOfGettingOneAwayOnFirstTry(int numGroups, int sizeOfGroup)
        {
            // in the case of the problem, where there are 4 groups of
            // size 4. The result is 4*(4 choose 3)*(12 choose 1)/(16 choose 4)
            return this._binomialTheoremCalculator.CalculateBinomialCoefficient(
                sizeOfGroup,
                sizeOfGroup - 1
            ) * this._binomialTheoremCalculator.CalculateBinomialCoefficient(
                sizeOfGroup * (numGroups - 1),
                1
            ) * numGroups / this._binomialTheoremCalculator.CalculateBinomialCoefficient(
                numGroups * sizeOfGroup,
                sizeOfGroup
            );
        }
    }
}
