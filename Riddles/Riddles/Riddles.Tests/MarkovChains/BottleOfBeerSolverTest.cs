using NUnit.Framework;
using Riddles.MarkovChains;
using System;
using System.Collections.Generic;
using System.Text;

namespace Riddles.Tests.MarkovChains
{
    public class BottleOfBeerSolverTest
    {
        private double Epsilon = 0.00000001;

        [TestCase(1, 0.01, 1.0101010101010102)]
        [TestCase(2, 0.01, 2.0304050607080915)]
        [TestCase(3, 0.01, 3.0610152128364554)]
        [TestCase(5, 0.01, 5.1535712813350232)]
        [TestCase(99, 0.01, 170.46790361647365)]
        public void TestCalculateExpectedNumberOfVerses(
            int numVerses, 
            double probabilityOfForgetting, 
            double expectedNumTerms
        )
        {
            var bottlesOfBeerSolver = new BottlesOfBeerSolver();
            var actualNumTerms = bottlesOfBeerSolver
                .CalculateExpectedNumberOfVerses(numVerses, probabilityOfForgetting)
                .Value;
            Assert.LessOrEqual(Math.Abs(expectedNumTerms - actualNumTerms), Epsilon);
        }
    }
}
