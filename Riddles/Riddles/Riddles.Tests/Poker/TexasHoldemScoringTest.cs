using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using NUnit.Framework;
using Riddles.Poker.Domain;

namespace Riddles.Tests.Poker
{
	public class TexasHoldemScoringTest
	{
		#region TestCases
		private const int royalFlushClubsId = 1;
		private static List<Card> royalFlushClubs = new List<Card> {
			new Card(Value.Ace, Suit.Clubs),
			new Card(Value.King, Suit.Clubs),
			new Card(Value.Queen, Suit.Clubs),
			new Card(Value.Jack, Suit.Clubs),
			new Card(Value.Ten, Suit.Clubs),
		};
		
		private const int royalFlushDiamondsId = 2;
		private static List<Card> royalFlushDiamonds = new List<Card> {
			new Card(Value.Queen, Suit.Diamonds),
			new Card(Value.Ten, Suit.Diamonds),
			new Card(Value.Jack, Suit.Diamonds),
			new Card(Value.Ace, Suit.Diamonds),
			new Card(Value.King, Suit.Diamonds),
		};		

		private const int straightFlushSpades_9HighId = 3;
		private static List<Card> straightFlushSpades_9High = new List<Card> {
			new Card(Value.Nine, Suit.Spades),
			new Card(Value.Five, Suit.Spades),
			new Card(Value.Six, Suit.Spades),
			new Card(Value.Seven, Suit.Spades),
			new Card(Value.Eight, Suit.Spades),
		};

		private const int straight_9HighId = 4;
		private static List<Card> straight_9High = new List<Card> {
			new Card(Value.Nine, Suit.Spades),
			new Card(Value.Five, Suit.Spades),
			new Card(Value.Six, Suit.Hearts),
			new Card(Value.Seven, Suit.Spades),
			new Card(Value.Eight, Suit.Spades),
		};

		private const int straightAHighId = 5;
		private static List<Card> straightAHigh = new List<Card> {
			new Card(Value.Ace, Suit.Clubs),
			new Card(Value.King, Suit.Spades),
			new Card(Value.Queen, Suit.Clubs),
			new Card(Value.Jack, Suit.Hearts),
			new Card(Value.Ten, Suit.Clubs),
		};

		private const int flushAHighId = 6;
		private static List<Card> flushAHigh = new List<Card> {
			new Card(Value.Ace, Suit.Clubs),
			new Card(Value.King, Suit.Clubs),
			new Card(Value.Three, Suit.Clubs),
			new Card(Value.Jack, Suit.Clubs),
			new Card(Value.Six, Suit.Clubs),
		};

		private const int flushJHighId = 7;
		private static List<Card> flushJHigh = new List<Card> {
			new Card(Value.Two, Suit.Hearts),
			new Card(Value.Eight, Suit.Hearts),
			new Card(Value.Three, Suit.Hearts),
			new Card(Value.Jack, Suit.Hearts),
			new Card(Value.Six, Suit.Hearts),
		};

		private Dictionary<int, List<Card>> testCaseDictionary = new Dictionary<int, List<Card>>
		{
			{ royalFlushClubsId, royalFlushClubs },
			{ royalFlushDiamondsId, royalFlushDiamonds },
			{ straightFlushSpades_9HighId, straightFlushSpades_9High },
			{ straight_9HighId, straight_9High },
			{ straightAHighId, straightAHigh },
			{ flushAHighId, flushAHigh },
			{ flushJHighId, flushJHigh }
		};
		#endregion

		[TestCase(royalFlushClubsId, royalFlushDiamondsId, 0)]
		[TestCase(royalFlushClubsId, straightFlushSpades_9HighId, 1)]
		[TestCase(straightFlushSpades_9HighId, royalFlushClubsId, -1)]
		public void TestScoring(int hand1, int hand2, int expectedComparison)
		{
			var score1 = new TexasHoldemHand.TexasHoldemScore(testCaseDictionary[hand1]);
			var score2 = new TexasHoldemHand.TexasHoldemScore(testCaseDictionary[hand2]);
			var comparison = score1.CompareTo(score2);
			Assert.AreEqual(expectedComparison, comparison);
		}

		[TestCase(royalFlushClubsId, HandType.StraightFlush, 14)]
		[TestCase(royalFlushDiamondsId, HandType.StraightFlush, 14)]
		[TestCase(straightFlushSpades_9HighId, HandType.StraightFlush, 9)]
		[TestCase(straight_9HighId, HandType.Straight, 9)]
		[TestCase(straightAHighId, HandType.Straight, 14)]
		[TestCase(flushAHighId, HandType.Flush, 1413110603)]
		[TestCase(flushJHighId, HandType.Flush, 1108060302)]
		public void TestHandType(int handId, HandType expectedHandType, int expectedScore)
		{
			var score = new TexasHoldemHand.TexasHoldemScore(testCaseDictionary[handId]);
			Assert.AreEqual(expectedHandType, score.HandType);
			Assert.AreEqual(expectedScore, this.GetScore(score));
		}

		private int GetScore(TexasHoldemHand.TexasHoldemScore score)
		{
			switch (score.HandType)
			{
				case HandType.StraightFlush:
					return score.StraightFlushScore;
				case HandType.FourOfAKind:
					return score.FourOfAKindScore;
				case HandType.FullHouse:
					return score.FullHouseScore;
				case HandType.Flush:
					return score.FlushScore;
				case HandType.Straight:
					return score.StraightScore;
				case HandType.ThreeOfAKind:
					return score.ThreeOfAKindScore;
				case HandType.TwoPairs:
					return score.TwoPairsScore;
				case HandType.Pair:
					return score.SinglePairScore;
				case HandType.HighCard:
					return score.HighCardScore;
			}
			throw new InvalidOperationException($"Unexpected handType {score.HandType}");
		}
	}

	public class TestHand
	{
		public int HandId { get; set; }
		public List<Card> Hand { get; set; }
	}
}
