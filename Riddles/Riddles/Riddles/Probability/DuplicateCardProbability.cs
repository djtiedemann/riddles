using System;
using System.Collections.Generic;
using System.Text;

namespace Riddles.Probability
{
	public class DuplicateCardProbability
	{
		public DuplicateCardProbability()
		{

		}

		public double FindProbabilityOfDrawingDuplicateCardAfterDrawing2CardsUntilEmpty(int numDistinctCards, int numCopiesOfEachCard)
		{
			// build a dictionary determining how many copies of each card remain. this will determine the odds of drawing a duplicate
			// say for instance, we have a 52 card deck. before any cards are drawn the dictionary will look like
			// {1 : 0}, {2:, 0}, {3: 0}, {4: 13}
			// then, say the first cards drawn are a 7 and a J. those cards will only have 3 copies, leading to the dictionary
			// {1 : 0}, {2:, 0}, {3: 2}, {4: 11}
			// the probability of drawing duplicate cards are different with each dictionary
			Dictionary<int, int> mappingFromNumRemainingCopiesToNumCardsWithNCopies = new Dictionary<int, int>();
			for(var i=1; i < numCopiesOfEachCard; i++)
			{
				mappingFromNumRemainingCopiesToNumCardsWithNCopies[i] = 0;
			}
			mappingFromNumRemainingCopiesToNumCardsWithNCopies[numCopiesOfEachCard] = numDistinctCards;
			var probabilitiesForSetsOfRemainingCards = new Dictionary<string, double>();
			return 1 - this.FindProbabilityOfNotDrawingDuplicateCardAfterDrawing2CardsUntilEmpty(
				mappingFromNumRemainingCopiesToNumCardsWithNCopies,
				probabilitiesForSetsOfRemainingCards,
				numDistinctCards *numCopiesOfEachCard,
				numCopiesOfEachCard);
		}

		public double FindProbabilityOfNotDrawingDuplicateCardAfterDrawing2CardsUntilEmpty(
			Dictionary<int, int> numCopiesForCardDictionary,
			Dictionary<string, double> memoizedProbabilities,
			int numCardsRemaining,
			int initialNumCopiesOfEachCard)
		{
			var memoizationKey = this.ComputeMemoizationKey(numCopiesForCardDictionary, initialNumCopiesOfEachCard);
			if (memoizedProbabilities.ContainsKey(memoizationKey))
			{
				return memoizedProbabilities[memoizationKey];
			}
			if(numCardsRemaining == 2)
			{
				var probabilityOfNotDrawingADuplicateCard = 0.0;
				if(numCopiesForCardDictionary[1] == 2)
				{
					probabilityOfNotDrawingADuplicateCard = 1.0;
				}
				memoizedProbabilities[memoizationKey] = probabilityOfNotDrawingADuplicateCard;
				return probabilityOfNotDrawingADuplicateCard;
			}
			if(numCardsRemaining == 3)
			{
				var probabilityOfNotDrawingADuplicateCard = 0.0;
				if (numCopiesForCardDictionary[1] == 3)
				{
					probabilityOfNotDrawingADuplicateCard = 1.0;
				}
				if(numCopiesForCardDictionary[2] == 1)
				{
					probabilityOfNotDrawingADuplicateCard = 2.0 / 3.0;
				}
				memoizedProbabilities[memoizationKey] = probabilityOfNotDrawingADuplicateCard;
				return probabilityOfNotDrawingADuplicateCard;
			}
			double oddsOfNotDrawingADuplicate = 0.0;
			for (int n=1; n<=initialNumCopiesOfEachCard; n++)
			{
				// for instance, if we're looking at cards where there are 2 copies (n = 2), 
				// and there are 4 sets of those cards remaining (say 7, 8, K, A), then there are 8 total cards, and the odds of drawing 
				// a card with 2 copies is 8/numCardsRemaining
				int numberOfCardsWithNCopiesRemaining = numCopiesForCardDictionary[n] * n;
				if(numberOfCardsWithNCopiesRemaining == 0)
				{
					continue;
				}
				double oddsOfDrawingACardWithNCopiesRemaining = (double)numberOfCardsWithNCopiesRemaining / numCardsRemaining;
				// if you drew a card with n copies, there will be n-1 copies remaining. 
				// there will be numCardsRemaining - 1 cards remaining (since you drew one), putting the odds of drawing an immediate duplicate at
				// (n-1)/(numCardsRemaining-1)
				// so your odds of continuing given the draw are (1 - ((n-1)/(numCardsRemaining-1))) and multiplying that by the odds of getting the draw
				// gives you the odds of going down this path
				double oddsOfGettingFirstDrawAndNotDrawingADuplicateCard = oddsOfDrawingACardWithNCopiesRemaining *
					(1 - ((double)(n - 1) / (numCardsRemaining - 1)));
				// update dictionary to reflect we've drawn a card
				numCopiesForCardDictionary[n] = numCopiesForCardDictionary[n] - 1;
				if (n > 1)
				{
					numCopiesForCardDictionary[n - 1] = numCopiesForCardDictionary[n - 1] + 1;
				}
				for (int n2=1; n2<=initialNumCopiesOfEachCard; n2++)
				{
					int numberOfCardsWithNCopiesRemainingForSecondDraw = (n2 == n - 1)
						? (numCopiesForCardDictionary[n2] - 1) * n2 // we know we didn't draw a duplicate so subtract 1 from the case where n2 == n - 1
						: numCopiesForCardDictionary[n2] * n2;
					if (numberOfCardsWithNCopiesRemainingForSecondDraw == 0)
					{
						continue;
					}
					double oddsForPairOfDraws = oddsOfDrawingACardWithNCopiesRemaining * 
						((double)numberOfCardsWithNCopiesRemainingForSecondDraw / (numCardsRemaining - 1));
					// update dictionay to reflect second draw
					numCopiesForCardDictionary[n2] = numCopiesForCardDictionary[n2] - 1;
					if(n2 > 1)
					{
						numCopiesForCardDictionary[n2 - 1] = numCopiesForCardDictionary[n2 - 1] + 1;
					}
					var probabilityOfNotDrawingDuplicateCardAfterDrawing2CardsUntilEmpty = this.FindProbabilityOfNotDrawingDuplicateCardAfterDrawing2CardsUntilEmpty(
						numCopiesForCardDictionary,
						memoizedProbabilities,
						numCardsRemaining - 2,
						initialNumCopiesOfEachCard);
					var incrementalProbability = oddsForPairOfDraws * probabilityOfNotDrawingDuplicateCardAfterDrawing2CardsUntilEmpty;
					// calculate the odds of not drawing a duplicate in the next scenario
					oddsOfNotDrawingADuplicate += incrementalProbability;

					// add back card from second draw
					numCopiesForCardDictionary[n2] = numCopiesForCardDictionary[n2] + 1;
					if (n2 > 1)
					{
						numCopiesForCardDictionary[n2 - 1] = numCopiesForCardDictionary[n2 - 1] - 1;
					}
				}
				// add card back for next permutation
				numCopiesForCardDictionary[n] = numCopiesForCardDictionary[n] + 1;
				if (n > 1)
				{
					numCopiesForCardDictionary[n - 1] = numCopiesForCardDictionary[n - 1] - 1;
				}
			}
			memoizedProbabilities[memoizationKey] = oddsOfNotDrawingADuplicate;
			return oddsOfNotDrawingADuplicate;
		}

		public string ComputeMemoizationKey(Dictionary<int, int> numCopiesForCardDictionary, int initialNumCopiesOfEachCard)
		{
			string key = "";
			for (int n = 1; n <= initialNumCopiesOfEachCard; n++)
			{
				key = $"{key}({n}: {numCopiesForCardDictionary[n]})";
			}
			return key;
		}
	}
}
