using NUnit.Framework;
using Riddles.Probability;
using System.Collections.Generic;
using System;

namespace Riddles.Tests.Probability
{
	public class DuplicateCardProbabilityTest
	{
		[SetUp]
		public void Setup()
		{

		}

		[TestCase(8, 1, 0.0, Description = "1 copy of 8 distinct cards")]
		////[TestCase(8, 2, 0.066666, Description = "2 copies of 8 distinct cards")]
		////[TestCase(8, 3, 0.0869565, Description = "3 copies of 8 distinct cards")]
		////[TestCase(13, 4, 0.05882352941, Description = "4 copies of 8 distinct cards")]
		[TestCase(2, 1, 0.0, Description = "1 copy of 2 distinct cards")]
		[TestCase(1, 2, 1.0, Description = "2 copies of 1 distinct card")]
		[TestCase(3, 2, 0.46666666666, Description = "2 copies of 3 distinct cards")]
		[TestCase(2, 4, 0.771428571, Description = "4 copies of 2 distinct cards")]
		[TestCase(3, 4, 0.8337662, Description = "4 copies of 3 distinct cards")]
		public void TestDuplicateCardProbability(int numDistinctCards, int numCopiesOfEachCard, double expectedPobability)
		{
			double epsilon = 0.0001;
			var duplicateCardProbabilityUtility = new DuplicateCardProbability();
			var actualProbability = duplicateCardProbabilityUtility.FindProbabilityOfDrawingDuplicateCardAfterDrawing2CardsUntilEmpty(
				numDistinctCards, numCopiesOfEachCard);
			Assert.LessOrEqual(Math.Abs(expectedPobability - actualProbability), epsilon);
		}

		[Test]
		public void TestDuplicateCardProbability2()
		{
			var dict = new Dictionary<int, int>();
			dict[1] = 0;
			dict[2] = 0;
			dict[3] = 0;
			dict[4] = 3;
			var duplicateCardProbabilityUtility = new DuplicateCardProbability();
			var actualProbability = duplicateCardProbabilityUtility.FindProbabilityOfNotDrawingDuplicateCardAfterDrawing2CardsUntilEmpty(
				dict, 12, 4);
		}
	}
}
