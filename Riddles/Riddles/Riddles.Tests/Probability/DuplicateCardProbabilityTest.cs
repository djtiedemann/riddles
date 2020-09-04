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
		[TestCase(2, 1, 0.0, Description = "1 copy of 2 distinct cards")]
		[TestCase(1, 2, 1.0, Description = "2 copies of 1 distinct card")]
		[TestCase(3, 2, 0.46666666666, Description = "2 copies of 3 distinct cards")]
		[TestCase(2, 4, 0.771428571, Description = "4 copies of 2 distinct cards")]
		[TestCase(3, 4, 0.8337662, Description = "4 copies of 3 distinct cards")]
		[TestCase(4, 4, 0.818421578, Description = "4 copies of 4 distinct cards")]
		[TestCase(5, 4, 0.810353361, Description = "4 copies of 5 distinct cards")]
		[TestCase(6, 4, 0.804829211, Description = "4 copies of 6 distinct cards")]
		[TestCase(7, 4, 0.8008554124763422, Description = "4 copies of 7 distinct cards")]
		[TestCase(8, 4, 0.7978641190233514, Description = "4 copies of 8 distinct cards")]
		[TestCase(9, 4, 0.7955330225964994, Description = "4 copies of 9 distinct cards")]
		[TestCase(10, 4, 0.7936661626182595, Description = "4 copies of 10 distinct cards")]
		[TestCase(11, 4, 0.792137851189068, Description = "4 copies of 11 distinct cards")]
		[TestCase(12, 4, 0.7908638810507843, Description = "4 copies of 12 distinct cards")]
		[TestCase(13, 4, 0.7897857667887249, Description = "4 copies of 13 distinct cards")]
		public void TestDuplicateCardProbability(int numDistinctCards, int numCopiesOfEachCard, double expectedPobability)
		{
			double epsilon = 0.0001;
			var duplicateCardProbabilityUtility = new DuplicateCardProbability();
			var actualProbability = duplicateCardProbabilityUtility.FindProbabilityOfDrawingDuplicateCardAfterDrawing2CardsUntilEmpty(
				numDistinctCards, numCopiesOfEachCard);
			Console.WriteLine($"Num distinct cards: {numDistinctCards}, Probability: {actualProbability}");
			Assert.LessOrEqual(Math.Abs(expectedPobability - actualProbability), epsilon);
		}
	}
}
