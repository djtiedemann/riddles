using System;
using System.Collections.Generic;
using System.Text;

namespace Riddles.Poker.Domain
{
	public class Card
	{
		private Suit _suit;
		private Value _value;

		public Card(Value value, Suit suit)
		{
			this._suit = suit;
			this._value = value;
		}

		public Suit Suit { get { return this._suit;  }  }
		public Value Value { get { return this._value;  } }
	}
}
