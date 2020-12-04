using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Riddles.Probability
{
	/// <summary>
	/// The idea here is that you have n balls of different colors. Each turn you draw 2 balls. You paint the second ball to be the same color as the first
	/// one. How many turns does it take to paint all of the balls
	/// </summary>
	public class RandomBallPainter
	{
		private Dictionary<int, double> _numBallsToProbabilityDictionary;
		// this tracks which terms are assigned to each column in the matrix equation that we form
		Dictionary<string, int> _termDictionary;
		public RandomBallPainter()
		{
			this._numBallsToProbabilityDictionary = new Dictionary<int, double>();
		}

		public double? GetExpectedNumberOfTurnsToPaintBalls(int numBalls)
		{
			if (this._numBallsToProbabilityDictionary.ContainsKey(numBalls))
			{
				return this._numBallsToProbabilityDictionary[numBalls];
			}
			if(numBalls <= 0)
			{
				return null;
			}
			if(numBalls == 1)
			{
				return 0;
			}
			this._termDictionary = new Dictionary<string, int>();
			return null;
		}

		public class State {
			/// <summary>
			/// This keeps track of the current state by tracking how many balls there are of similar colors.
			/// The key to this dictionary is the number of balls of the same color, and the value is the number of colors who have that many balls
			/// 
			/// So for instance if there were 4 green balls, 2 red balls, 2 blue balls, 1 yellow ball, 1 purple ball, and 1 black ball, you'd have the
			/// following dictionary:
			/// { 4: 1 /* green */, 2: 2 /* red, blue */, 1: 3 /* yellow, purple, black */ }
			/// 
			/// The initial state for n balls is { 1: n }, and the final state for n balls is { n: 1 }
			/// </summary>
			private readonly Dictionary<int, int> _ballDistributionForState;
			private string _stateKey;
			private int _numBalls;
			public State(Dictionary<int, int> ballDistributionForState)
			{
				this._ballDistributionForState = ballDistributionForState;
				this._numBalls = this._ballDistributionForState.Keys.Aggregate((k1, k2) => k1 + k2*this._ballDistributionForState[k2]);
			}

			/// <summary>
			/// returns a tuple of (state, probability transition to this state)
			/// </summary>
			/// <returns></returns>
			public List<Tuple<State, double>> GetStateTransitions()
			{
				return null;
			}

			public string StringRepresentation()
			{
				if(this._stateKey == null)
				{
					this._stateKey = 
						$"({this._ballDistributionForState.Keys.OrderBy(k => k).Select(k => $"{k}:{this._ballDistributionForState[k]}").Aggregate((i, j) => $"{i},{j}")})";
				}
				return this._stateKey;
			}

			public override bool Equals(object obj)
			{
				if(!(obj is State))
				{
					return false;
				}
				return ((State)obj).StringRepresentation() == this.StringRepresentation();
			}

			public override int GetHashCode()
			{
				return this.StringRepresentation().GetHashCode();
			}
		}
	}	
}
