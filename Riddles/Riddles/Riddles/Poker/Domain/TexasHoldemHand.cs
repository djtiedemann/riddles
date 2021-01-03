using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Riddles.Poker.Domain
{
	public class TexasHoldemHand
	{
		public TexasHoldemHand(List<Card> individualCards, List<Card> communityCards)
		{
			this.IndividualCards = individualCards;
			this.CommunityCards = communityCards;
		}

		public List<Card> IndividualCards { get;  }
		public List<Card> CommunityCards { get; }

		public class TexasHoldemScore : IComparable
		{
			public int HighCardScore { get; }
			public int SinglePairScore { get; }
			public int TwoPairsScore { get; }
			public int ThreeOfAKindScore { get; }
			public int StraightScore { get; }
			public int FlushScore { get; }
			public int FullHouseScore { get; }
			public int FourOfAKindScore { get; }
			public int StraightFlushScore { get; }

			public HandType HandType { get; }

			public TexasHoldemScore(List<Card> hand)
			{
				if(hand.Count != 5)
				{
					throw new InvalidOperationException("Can't score a hand that doesn't contain exactly 5 cards");
				}
				this.StraightFlushScore = this.CalculateStraightFlushScore(hand);
				if(this.StraightFlushScore > 0)
				{
					this.HandType = HandType.StraightFlush;
					return;
				}
				this.FourOfAKindScore = this.CalculateFourOfAKindScore(hand);
				if(this.FourOfAKindScore > 0)
				{
					this.HandType = HandType.FourOfAKind;
					return;
				}
				this.FullHouseScore = this.CalculateFullHouseScore(hand);
				if(this.FullHouseScore > 0)
				{
					this.HandType = HandType.FullHouse;
					return;
				}
				this.FlushScore = this.CalculateFlushScore(hand);
				if(this.FlushScore > 0)
				{
					this.HandType = HandType.Flush;
					return;
				}
				this.StraightScore = this.CalculateStraightScore(hand);
				if(this.StraightScore > 0)
				{
					this.HandType = HandType.Straight;
					return;
				}
				this.ThreeOfAKindScore = this.CalculateThreeOfAKindScore(hand);
				if(this.ThreeOfAKindScore > 0)
				{
					this.HandType = HandType.ThreeOfAKind;
					return;
				}
				this.TwoPairsScore = this.CalculateTwoPairsScore(hand);
				if(this.TwoPairsScore > 0)
				{
					this.HandType = HandType.TwoPairs;
					return;
				}
				this.SinglePairScore = this.CalculateSinglePairScore(hand);
				if(this.SinglePairScore > 0)
				{
					this.HandType = HandType.Pair;
					return;
				}
				this.HandType = HandType.HighCard;
				this.HighCardScore = this.CalculateHighCardScore(hand);
			}

			public int CompareTo(object? obj)
			{
				if(obj == null || !(obj is TexasHoldemScore))
				{
					return 0;
				}
				var otherScore = (TexasHoldemScore)obj;
				// check if one hand has a higher straight flush, or they are the same straight flush
				var straightFlushCompairson = this.StraightFlushScore > otherScore.StraightFlushScore
					? 1
					: this.StraightFlushScore < otherScore.StraightFlushScore
						? -1
						: this.StraightFlushScore > 0
							? 0
							: (int?)null;
				if (straightFlushCompairson.HasValue) { return straightFlushCompairson.Value;  }

				// check if one hand has a higher four of a kind hand, or they are the same four of a kind hand
				var fourOfAKindComparison = this.FourOfAKindScore > otherScore.FourOfAKindScore
					? 1
					: this.FourOfAKindScore < otherScore.FourOfAKindScore
						? -1
						: this.FourOfAKindScore > 0
							? 0
							: (int?)null;
				if (fourOfAKindComparison.HasValue) { return fourOfAKindComparison.Value; }

				// check if one hand has a higher full house, or if they are the same full house
				var fullHouseComparison = this.FullHouseScore > otherScore.FullHouseScore
					? 1
					: this.FullHouseScore < otherScore.FullHouseScore
						? -1
						: this.FullHouseScore > 0
							? 0
							: (int?)null;
				if (fullHouseComparison.HasValue) { return fullHouseComparison.Value; }

				// check if one hand has a higher flush, or if they are the same flush
				var flushComparison = this.FlushScore > otherScore.FlushScore
					? 1
					: this.FlushScore < otherScore.FlushScore
						? -1
						: this.FlushScore > 0
							? 0
							: (int?)null;
				if (flushComparison.HasValue) { return flushComparison.Value; }

				// check if one hand has a higher straight, or if they are the same straight
				var straightComparison = this.StraightScore > otherScore.StraightScore
					? 1
					: this.StraightScore < otherScore.StraightScore
						? -1
						: this.StraightScore > 0
							? 0
							: (int?)null;
				if (straightComparison.HasValue) { return straightComparison.Value; }

				// check if one hand has a higher three of a kind, or if they are the same three of a kind
				var threeOfAKindComparison = this.ThreeOfAKindScore > otherScore.ThreeOfAKindScore
					? 1
					: this.ThreeOfAKindScore < otherScore.ThreeOfAKindScore
						? -1
						: this.ThreeOfAKindScore > 0
							? 0
							: (int?)null;
				if (threeOfAKindComparison.HasValue) { return threeOfAKindComparison.Value; }

				// check if one hand has a higher two pairs, or if they are the same two pairs
				var twoPairsComparison = this.TwoPairsScore > otherScore.TwoPairsScore
					? 1
					: this.TwoPairsScore < otherScore.TwoPairsScore
						? -1
						: this.TwoPairsScore > 0
							? 0
							: (int?)null;
				if (twoPairsComparison.HasValue) { return twoPairsComparison.Value; }

				// check if one hand has a pair, or if they are the same pair
				var singlePairComparison = this.SinglePairScore > otherScore.SinglePairScore
					? 1
					: this.SinglePairScore < otherScore.SinglePairScore
						? -1
						: this.SinglePairScore > 0
							? 0
							: (int?)null;
				if (singlePairComparison.HasValue) { return singlePairComparison.Value; }

				// check if one hand has a higher high card, or if they are the same high card
				var highCardComparison = this.HighCardScore > otherScore.HighCardScore
					? 1
					: this.HighCardScore < otherScore.HighCardScore
						? -1
						: this.HighCardScore > 0
							? 0
							: (int?)null;
				if (highCardComparison.HasValue) { return highCardComparison.Value; }
				throw new InvalidOperationException("All scores for a hand shouldn't be zero");
			}

			private int CalculateHighCardScore(List<Card> hand)
			{
				return int.Parse(hand.OrderByDescending(c => c.Value).Aggregate("", (agg, c) => $"{agg}{((int)c.Value).ToString().PadLeft(2, '0')}"));
			}

			private int CalculateSinglePairScore(List<Card> hand)
			{
				var pairs = hand.GroupBy(h => h.Value).Where(g => g.Count() == 2).ToList();
				var kickers = hand.GroupBy(h => h.Value).Where(g => g.Count() == 1).ToList();
				// if there isn't 1 pair and 3 other cards, then this is not a pair
				if(pairs.Count != 1 && kickers.Count != 3)
				{
					return 0;
				}

				var pairScore = ((int)pairs.Single().Key).ToString().PadLeft(2, '0');
				var kickerScore = kickers.OrderByDescending(k => k.Key).Aggregate("", (agg, c) => $"{agg}{((int)c.Key).ToString().PadLeft(2, '0')}");
				var score = $"{pairScore}{kickerScore}";
				return int.Parse(score);
			}

			private int CalculateTwoPairsScore(List<Card> hand)
			{
				var pairs = hand.GroupBy(h => h.Value).Where(g => g.Count() == 2).ToList();
				var kickers = hand.GroupBy(h => h.Value).Where(g => g.Count() == 1).ToList();
				// if there aren't 2 pairs and 1 other card, then this is not 2 pairs
				if (pairs.Count != 2 && kickers.Count != 1)
				{
					return 0;
				}
				var pairScore = pairs.OrderByDescending(p => p.Key).Aggregate("", (agg, v) => $"{agg}{v.ToString().PadLeft(2, '0')}");
				var kickerScore = kickers.Single().Key.ToString().PadLeft(2, '0');
				var score = $"{pairScore}{kickerScore}";
				return int.Parse(score);
			}

			private int CalculateThreeOfAKindScore(List<Card> hand)
			{
				var sets = hand.GroupBy(h => h.Value).Where(g => g.Count() == 3).ToList();
				var kickers = hand.GroupBy(h => h.Value).Where(g => g.Count() == 1).ToList();
				// if there isn't 1 set and 2 other cards, then this is not three of a kind
				if (sets.Count != 1 && kickers.Count != 2)
				{
					return 0;
				}
				var setScore = sets.Single().Key.ToString().PadLeft(2, '0');
				var kickerScore = kickers.OrderByDescending(k => k.Key).Aggregate("", (agg, v) => $"{agg}{v.ToString().PadLeft(2, '0')}");
				var score = $"{setScore}{kickerScore}";
				return int.Parse(score);
			}

			private int CalculateStraightScore(List<Card> hand)
			{
				var cards = hand.OrderBy(c => c.Value).ToArray();
				for(int i=1; i<hand.Count(); i++)
				{
					// if any 2 values aren't consecutive, this isn't a straight. The only exception is an A2345 straight which will be sorted as 2345A
					// so that is special cased here
					if(!(cards[i].Value - cards[i-1].Value == 1 || cards[i].Value == Value.Ace && cards[i-1].Value == Value.Five))
					{
						return 0;
					}
				}
				// if the straight is A2345, the score is 5 - otherwise the score is the highest card
				if(cards.Any(c => c.Value == Value.Five) && cards.Any(c => c.Value == Value.Ace))
				{
					return (int)Value.Five;
				}
				return (int)cards.Max(c => c.Value);
			}

			private int CalculateFlushScore(List<Card> hand)
			{
				// if not all cards have the same suit, this isn't a flush
				if(hand.GroupBy(c => c.Suit).Count() != 1)
				{
					return 0;
				}
				return int.Parse(hand.OrderByDescending(c => c.Value).Aggregate("", (agg, c) => $"{agg}{((int)c.Value).ToString().PadLeft(2, '0')}"));
			}

			private int CalculateFullHouseScore(List<Card> hand)
			{
				var sets = hand.GroupBy(h => h.Value).Where(g => g.Count() == 3).ToList();
				var pairs = hand.GroupBy(h => h.Value).Where(g => g.Count() == 2).ToList();
				// if there isn't 1 set and 1 pair, then this is not a full house
				if (sets.Count != 1 && pairs.Count != 1)
				{
					return 0;
				}
				var setScore = sets.Single().Key.ToString().PadLeft(2, '0');
				var pairsScore = pairs.Single().Key.ToString().PadLeft(2, '0');
				var score = $"{setScore}{pairsScore}";
				return int.Parse(score);
			}

			private int CalculateFourOfAKindScore(List<Card> hand)
			{
				var fourOfAKinds = hand.GroupBy(h => h.Value).Where(g => g.Count() == 4).ToList();
				var kickers = hand.GroupBy(h => h.Value).Where(g => g.Count() == 1).ToList();
				// if there isn't 1 set and 1 pair, then this is not a full house
				if (fourOfAKinds.Count != 1 && kickers.Count != 1)
				{
					return 0;
				}
				var fourOfAKindScore = fourOfAKinds.Single().Key.ToString().PadLeft(2, '0');
				var kickersScore = kickers.Single().Key.ToString().PadLeft(2, '0');
				var score = $"{fourOfAKindScore}{kickersScore}";
				return int.Parse(score);
			}

			private int CalculateStraightFlushScore(List<Card> hand)
			{
				// if not all cards have the same suit, this isn't a straight flush
				if (hand.GroupBy(c => c.Suit).Count() != 1)
				{
					return 0;
				}

				var cards = hand.OrderBy(c => c.Value).ToArray();
				for (int i = 1; i < hand.Count(); i++)
				{
					// if any 2 values aren't consecutive, this isn't a straight. The only exception is an A2345 straight which will be sorted as 2345A
					// so that is special cased here
					if (!(cards[i].Value - cards[i - 1].Value == 1 || cards[i].Value == Value.Ace && cards[i - 1].Value == Value.Five))
					{
						return 0;
					}
				}
				// if the straight is A2345, the score is 5 - otherwise the score is the highest card
				if (cards.Any(c => c.Value == Value.Five) && cards.Any(c => c.Value == Value.Ace))
				{
					return (int)Value.Five;
				}
				return (int)cards.Max(c => c.Value);
			}

			private int CalculateRoyalFlushScore(List<Card> hand)
			{
				// if not all cards have the same suit, this isn't a royal flush
				if (hand.GroupBy(c => c.Suit).Count() != 1)
				{
					return 0;
				}
				// if all cards have the same suit, and the lowest card is a 10 and the highest card is an A, this is a royal flush
				if (hand.Min(c => c.Value) == Value.Ten && hand.Max(c => c.Value) == Value.Ace)
				{
					return 1;
				}
				// otherwise it is not
				return 0;
			}
		}
	}
}
