using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Riddles.NumberTheory
{
	public class DifferenceOfSquares
	{
		private Factorization factorization;
		public DifferenceOfSquares()
		{
			factorization = new Factorization();
		}

		public List<PairOfSquares> FindSquaresWhereDifferenceIs(int n)
		{
			var pairsOfSquares = new List<PairOfSquares> { };
			return pairsOfSquares;
		}

		// https://fivethirtyeight.com/features/can-you-eat-all-the-chocolates/
		// Riddler Express. Basically if you have a number n, how many ways can you write it as a difference of squares
		// squares are made by summing odd numbers. So it makes sense that every odd number is the difference of two consecutive squares
		// for instance, take 7, which can be written as 2*n - 1 = 7, n = 4. n^2 = 16, n^2 - (n-1)^2 = 16 - 9 = 7
		//
		// now if the squares aren't consecutive but instead have a difference of 2, the result will be an even number
		// say we're taking n=4, n=6, 6^2 - 4^2 = 36 - 16 = 20
		// if we go back to the odd number idea, we're looking at 2 odd numbers being added to get to the difference, and the even number is the average
		// so 20 = 2*10, we have 9 and 11 between the odd numbers. solving for the roots that make up those numbers 7 = 2*4-1, 11=2*6-1, which gives
		// us the two squares
		//
		// for a number n. if n is odd, >=1, there will be two consecutive squares. if n is even >=2, and n/2 is even, there will be two squares separated
		// by 1. if n is odd >=3 and n/3 is even, then there will be two squares separated by 2, and so on.
		


		// so this takes in the average value, the total number of squares in the sequence (so the base case has numSquaresTotal = 1), and finds
		// the squares that give that value, if any
		// so for instance if averageValue = 7 and n=1, we're looking for 2*n-1 = 7, n=4, 4^2 - 3^2 = 7
		// if averageValue = 12, n = 4, we're looking for 9, 11, 13, 15. with the difference in squares being 12*4 = 48. 8^2 - 4^2 = 48
		public PairOfSquares GetPairOfSquaresWithCriteria(int averageValue, int numSquaresTotal)
		{
			// case where n is odd. numSquaresTotal must also be odd
			if(averageValue % 2 == 1 && numSquaresTotal % 2 == 1)
			{
				int rootJustGreaterThanAverage = (int)(averageValue + 1) / 2;
				int lowerRootInDifference = rootJustGreaterThanAverage - (int)Math.Ceiling(numSquaresTotal / 2.0);
				if(lowerRootInDifference < 0)
				{
					return null;
				}
				int greaterRootInDifference = rootJustGreaterThanAverage + (int)Math.Floor(numSquaresTotal / 2.0);
				return new PairOfSquares((int)Math.Pow(lowerRootInDifference, 2), (int)Math.Pow(greaterRootInDifference, 2));
			}
			// case where n is even. numSquaresTotal must also be even
			if(averageValue % 2 == 0 && numSquaresTotal % 2 == 0)
			{
				var oddNumberJustGreaterThanAverage = averageValue + 1;
				var oddNumberJustLessThanAverage = averageValue - 1;
				var rootJustGreaterThanAverage = (int)(oddNumberJustGreaterThanAverage + 1) / 2;
				var rootJustLessThanAverage = (int)(oddNumberJustLessThanAverage + 1) / 2;
				var differenceValuesNeedToShift = (numSquaresTotal - 2) / 2;
				var lowerRootInDifference = rootJustLessThanAverage - differenceValuesNeedToShift;
				if (lowerRootInDifference < 0)
				{
					return null;
				}
				var greaterRootInDifference = rootJustGreaterThanAverage + differenceValuesNeedToShift;
				return new PairOfSquares((int)Math.Pow(lowerRootInDifference, 2), (int)Math.Pow(greaterRootInDifference, 2));
			}
			// if averageValue and numSquares have a different value mod 1, there are no solutions
			return null;
		}
	}

	public class PairOfSquares{
		public long LargerSquare { get; private set; }
		public long SmallerSquare { get; private set; }

		public PairOfSquares(int a, int b)
		{
			SmallerSquare = Math.Min(a, b);
			LargerSquare = Math.Max(a, b);
		}
	}
}
