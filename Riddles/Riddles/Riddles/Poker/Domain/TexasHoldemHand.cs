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

		public class TexasHoldemScore
		{
			public TexasHoldemScore(List<Card> hand)
			{
				if(hand.Count != 5)
				{
					throw new InvalidOperationException("Can't score a hand that doesn't contain exactly 5 cards");
				}
			}

			public int CalculateHighCardScore(List<Card> hand)
			{
				return int.Parse(hand.OrderByDescending(c => c.Value).Aggregate("", (agg, c) => $"{agg}{c.Value.ToString().PadLeft(2)}"));
			}

			public int CalculateSinglePairScore(List<Card> hand)
			{
				var pairs = hand.GroupBy(h => h.Value).Where(g => g.Count() == 2).ToList();
				var kickers = hand.GroupBy(h => h.Value).Where(g => g.Count() == 1).ToList();
				// if there isn't 1 pair and 3 other cards, then this is not a pair
				if(pairs.Count != 1 && kickers.Count != 3)
				{
					return 0;
				}

				var pairScore = pairs.Single().Key.ToString().PadLeft(2);
				var kickerScore = kickers.OrderByDescending(k => k.Key).Aggregate("", (agg, v) => $"{agg}{v.ToString().PadLeft(2)}");
				var score = $"{pairScore}{kickerScore}";
				return int.Parse(score);
			}

			public int CalculateTwoPairsScore(List<Card> hand)
			{
				var pairs = hand.GroupBy(h => h.Value).Where(g => g.Count() == 2).ToList();
				var kickers = hand.GroupBy(h => h.Value).Where(g => g.Count() == 1).ToList();
				// if there aren't 2 pairs and 1 other card, then this is not 2 pairs
				if (pairs.Count != 2 && kickers.Count != 1)
				{
					return 0;
				}
				var pairScore = pairs.OrderByDescending(p => p.Key).Aggregate("", (agg, v) => $"{agg}{v.ToString().PadLeft(2)}");
				var kickerScore = kickers.Single().Key.ToString().PadLeft(2);
				var score = $"{pairScore}{kickerScore}";
				return int.Parse(score);
			}

			public int CalculateThreeOfAKindScore(List<Card> hand)
			{
				var sets = hand.GroupBy(h => h.Value).Where(g => g.Count() == 3).ToList();
				var kickers = hand.GroupBy(h => h.Value).Where(g => g.Count() == 1).ToList();
				// if there isn't 1 set and 2 other cards, then this is not three of a kind
				if (sets.Count != 1 && kickers.Count != 2)
				{
					return 0;
				}
				var setScore = sets.Single().Key.ToString().PadLeft(2);
				var kickerScore = kickers.OrderByDescending(k => k.Key).Aggregate("", (agg, v) => $"{agg}{v.ToString().PadLeft(2)}");
				var score = $"{setScore}{kickerScore}";
				return int.Parse(score);
			}

			public int CalculateStraightScore(List<Card> hand)
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

			public int CalculateFlushScore(List<Card> hand)
			{
				// if not all cards have the same suit, this isn't a flush
				if(hand.GroupBy(c => c.Suit).Count() != 1)
				{
					return 0;
				}
				return int.Parse(hand.OrderByDescending(c => c.Value).Aggregate("", (agg, c) => $"{agg}{c.Value.ToString().PadLeft(2)}"));
			}

			public int CalculateFullHouseScore(List<Card> hand)
			{
				var sets = hand.GroupBy(h => h.Value).Where(g => g.Count() == 3).ToList();
				var pairs = hand.GroupBy(h => h.Value).Where(g => g.Count() == 2).ToList();
				// if there isn't 1 set and 1 pair, then this is not a full house
				if (sets.Count != 1 && pairs.Count != 1)
				{
					return 0;
				}
				var setScore = sets.Single().Key.ToString().PadLeft(2);
				var pairsScore = pairs.Single().Key.ToString().PadLeft(2);
				var score = $"{setScore}{pairsScore}";
				return int.Parse(score);
			}

			public int CalculateFourOfAKindScore(List<Card> hand)
			{
				var fourOfAKinds = hand.GroupBy(h => h.Value).Where(g => g.Count() == 4).ToList();
				var kickers = hand.GroupBy(h => h.Value).Where(g => g.Count() == 1).ToList();
				// if there isn't 1 set and 1 pair, then this is not a full house
				if (fourOfAKinds.Count != 1 && kickers.Count != 1)
				{
					return 0;
				}
				var fourOfAKindScore = fourOfAKinds.Single().Key.ToString().PadLeft(2);
				var kickersScore = kickers.Single().Key.ToString().PadLeft(2);
				var score = $"{fourOfAKindScore}{kickersScore}";
				return int.Parse(score);
			}

			public int CalculateStraightFlushScore(List<Card> hand)
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

			public int CalculateRoyalFlushScore(List<Card> hand)
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
