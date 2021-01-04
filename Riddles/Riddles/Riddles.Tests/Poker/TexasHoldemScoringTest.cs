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

		private const int fourSixesId = 8;
		private static List<Card> fourSixes = new List<Card> {
			new Card(Value.Six, Suit.Hearts),
			new Card(Value.Six, Suit.Spades),
			new Card(Value.Six, Suit.Diamonds),
			new Card(Value.Jack, Suit.Hearts),
			new Card(Value.Six, Suit.Clubs),
		};

		private const int fourJacksId = 9;
		private static List<Card> fourJacks = new List<Card> {
			new Card(Value.Jack, Suit.Hearts),
			new Card(Value.Jack, Suit.Spades),
			new Card(Value.Jack, Suit.Diamonds),
			new Card(Value.Six, Suit.Hearts),
			new Card(Value.Jack, Suit.Clubs),
		};

		private const int fullHouseQueensFoursId = 10;
		private static List<Card> fullHouseQueensFours = new List<Card> {
			new Card(Value.Queen, Suit.Hearts),
			new Card(Value.Four, Suit.Spades),
			new Card(Value.Four, Suit.Diamonds),
			new Card(Value.Queen, Suit.Spades),
			new Card(Value.Queen, Suit.Diamonds),
		};

		private const int fullHouseFoursQueensId = 11;
		private static List<Card> fullHouseFoursQueens = new List<Card> {
			new Card(Value.Queen, Suit.Hearts),
			new Card(Value.Queen, Suit.Clubs),
			new Card(Value.Four, Suit.Diamonds),
			new Card(Value.Four, Suit.Spades),
			new Card(Value.Four, Suit.Clubs),
		};

		private const int threeOfAKind8s2KId = 12;
		private static List<Card> threeOfAKind8s2K = new List<Card> {
			new Card(Value.Eight, Suit.Hearts),
			new Card(Value.King, Suit.Hearts),
			new Card(Value.Eight, Suit.Diamonds),
			new Card(Value.Two, Suit.Spades),
			new Card(Value.Eight, Suit.Clubs),
		};

		private const int threeOfAKind8s79Id = 13;
		private static List<Card> threeOfAKind8s79 = new List<Card> {
			new Card(Value.Eight, Suit.Hearts),
			new Card(Value.Eight, Suit.Clubs),
			new Card(Value.Seven, Suit.Diamonds),
			new Card(Value.Eight, Suit.Spades),
			new Card(Value.Nine, Suit.Diamonds),
		};

		private const int twoPairsJKId = 14;
		private static List<Card> twoPairsJK = new List<Card> {
			new Card(Value.Jack, Suit.Hearts),
			new Card(Value.King, Suit.Clubs),
			new Card(Value.King, Suit.Diamonds),
			new Card(Value.Jack, Suit.Spades),
			new Card(Value.Two, Suit.Diamonds),
		};

		private const int twoPairsJ2Id = 15;
		private static List<Card> twoPairsJ2 = new List<Card> {
			new Card(Value.Jack, Suit.Hearts),
			new Card(Value.Two, Suit.Clubs),
			new Card(Value.Two, Suit.Diamonds),
			new Card(Value.Jack, Suit.Spades),
			new Card(Value.King, Suit.Diamonds),
		};

		private const int pair5sWith29AId = 16;
		private static List<Card> pair5sWith29A = new List<Card> {
			new Card(Value.Five, Suit.Hearts),
			new Card(Value.Five, Suit.Clubs),
			new Card(Value.Two, Suit.Diamonds),
			new Card(Value.Ace, Suit.Spades),
			new Card(Value.Nine, Suit.Diamonds),
		};

		private const int pair4sWith37KId = 17;
		private static List<Card> pair4sWith37K = new List<Card> {
			new Card(Value.King, Suit.Hearts),
			new Card(Value.Seven, Suit.Clubs),
			new Card(Value.Three, Suit.Diamonds),
			new Card(Value.Four, Suit.Spades),
			new Card(Value.Four, Suit.Diamonds),
		};

		private const int highCard79JQAhandId = 18;
		private static List<Card> highCard79JQAhand = new List<Card> {
			new Card(Value.Jack, Suit.Hearts),
			new Card(Value.Ace, Suit.Clubs),
			new Card(Value.Seven, Suit.Clubs),
			new Card(Value.Nine, Suit.Spades),
			new Card(Value.Queen, Suit.Clubs),
		};

		private const int highCard48JKAhandId = 19;
		private static List<Card> highCard48JKAhand = new List<Card> {
			new Card(Value.King, Suit.Spades),
			new Card(Value.Ace, Suit.Spades),
			new Card(Value.Eight, Suit.Spades),
			new Card(Value.Four, Suit.Spades),
			new Card(Value.Jack, Suit.Diamonds),
		};

		private Dictionary<int, List<Card>> testCaseDictionary = new Dictionary<int, List<Card>>
		{
			{ royalFlushClubsId, royalFlushClubs },
			{ royalFlushDiamondsId, royalFlushDiamonds },
			{ straightFlushSpades_9HighId, straightFlushSpades_9High },
			{ straight_9HighId, straight_9High },
			{ straightAHighId, straightAHigh },
			{ flushAHighId, flushAHigh },
			{ flushJHighId, flushJHigh },
			{ fourSixesId, fourSixes },
			{ fourJacksId, fourJacks },
			{ fullHouseQueensFoursId, fullHouseQueensFours },
			{ fullHouseFoursQueensId, fullHouseFoursQueens },
			{ threeOfAKind8s2KId, threeOfAKind8s2K },
			{ threeOfAKind8s79Id, threeOfAKind8s79 },
			{ twoPairsJKId, twoPairsJK },
			{ twoPairsJ2Id, twoPairsJ2 },
			{ pair5sWith29AId, pair5sWith29A },
			{ pair4sWith37KId, pair4sWith37K },
			{ highCard79JQAhandId, highCard79JQAhand },
			{ highCard48JKAhandId, highCard48JKAhand },
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
		[TestCase(fourSixesId, HandType.FourOfAKind, 611)]
		[TestCase(fourJacksId, HandType.FourOfAKind, 1106)]
		[TestCase(fullHouseQueensFoursId, HandType.FullHouse, 1204)]
		[TestCase(fullHouseFoursQueensId, HandType.FullHouse, 412)]
		[TestCase(threeOfAKind8s2KId, HandType.ThreeOfAKind, 81302)]
		[TestCase(threeOfAKind8s79Id, HandType.ThreeOfAKind, 80907)]
		[TestCase(twoPairsJKId, HandType.TwoPairs, 131102)]
		[TestCase(twoPairsJ2Id, HandType.TwoPairs, 110213)]
		[TestCase(pair5sWith29AId, HandType.Pair, 5140902)]
		[TestCase(pair4sWith37KId, HandType.Pair, 4130703)]
		[TestCase(highCard79JQAhandId, HandType.HighCard, 1412110907)]
		[TestCase(highCard48JKAhandId, HandType.HighCard, 1413110804)]
		public void TestHandType(int handId, HandType expectedHandType, int expectedScore)
		{
			var score = new TexasHoldemHand.TexasHoldemScore(testCaseDictionary[handId]);
			Assert.AreEqual(expectedHandType, score.HandType);
			Assert.AreEqual(expectedScore, score.GetScore());
		}		
	}

	public class TestHand
	{
		public int HandId { get; set; }
		public List<Card> Hand { get; set; }
	}
}
