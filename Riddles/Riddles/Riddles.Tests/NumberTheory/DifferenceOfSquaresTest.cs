using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Riddles.NumberTheory.Application;
using System.Linq;

namespace Riddles.Tests.NumberTheory
{
	public class DifferenceOfSquaresTest
	{
		public Dictionary<int, List<PairOfSquares>> expectedPairings = new Dictionary<int, List<PairOfSquares>>
		{
			{ 17, new List<PairOfSquares> { new PairOfSquares(81, 64 )} },
			{ 40, new List<PairOfSquares> { new PairOfSquares(49, 9 ), new PairOfSquares(121, 81) } },
			{ 42, new List<PairOfSquares> {  } },
			{ 1400, new List<PairOfSquares> { new PairOfSquares(1521, 121), new PairOfSquares(2025 , 625 ), new PairOfSquares(3249, 1849),
				new PairOfSquares(5625, 4225), new PairOfSquares(31329, 29929), new PairOfSquares(123201, 121801)
			} }
		};

		[TestCase(17)]
		[TestCase(40)]
		[TestCase(42)]
		[TestCase(1400)]
		public void TestFindSquaresWhereDifferenceIs(int n)
		{
			var differenceOfSquaresUtility = new DifferenceOfSquares();
			var squares = differenceOfSquaresUtility.FindSquaresWhereDifferenceIs(n).OrderBy(n => n.LargerSquare).ToArray();
			var expectedSquares = expectedPairings[n].OrderBy(n => n.LargerSquare).ToArray();
			Assert.AreEqual(expectedSquares.Length, squares.Length);
			for(int i=0; i<squares.Length; i++)
			{
				Assert.AreEqual(expectedSquares[i].LargerSquare, squares[i].LargerSquare);
				Assert.AreEqual(expectedSquares[i].SmallerSquare, squares[i].SmallerSquare);
				Assert.AreEqual(n, squares[i].LargerSquare - squares[i].SmallerSquare);
			}
		}

		[TestCase(11, 5, true)]
		[TestCase(9, 3, true)]
		[TestCase(17, 1, true)]
		[TestCase(8, 2, true)]
		[TestCase(26, 6, true)]
		public void TestGetPairOfSquaresWithCriteria(int averageValue, int numSquaresTotal, bool shouldHaveValue)
		{
			var differenceOfSquaresUtility = new DifferenceOfSquares();
			var squares = differenceOfSquaresUtility.GetPairOfSquaresWithCriteria(averageValue, numSquaresTotal);
			if (!shouldHaveValue)
			{
				Assert.IsNull(squares);
				return;
			}
			Assert.AreEqual(averageValue * numSquaresTotal, squares.LargerSquare - squares.SmallerSquare);
		}
	}
}
