using System;
using System.Collections.Generic;
using System.Text;

namespace Riddles.Probability
{
	public class HatProblemSolver
	{
		private Dictionary<int, double> _cache;

		public HatProblemSolver()
		{
			this._cache = new Dictionary<int, double>();
		}

		/// <summary>
		/// There are n people with n hats. They are randomly assigned to people. What's the probability that someone gets their hat back
		/// </summary>
		/// <returns></returns>
		public double CalculateProbabilityAtLeastOnePersonGetsTheirOwnHat(int n)
		{
			if (this._cache.ContainsKey(n))
			{
				return this._cache[n];
			}
			Dictionary<State, double> memoizationDictionary = new Dictionary<State, double>();
			var initialState = new State(n, n);
			return this.CalculateProbabilityAtLeastOnePersonGetsTheirOwnHatInternal(initialState, memoizationDictionary);
		}

		private double CalculateProbabilityAtLeastOnePersonGetsTheirOwnHatInternal(State state, Dictionary<State, double> memoizationDictionary)
		{
			if (memoizationDictionary.ContainsKey(state))
			{
				return memoizationDictionary[state];
			}
			// if there is only 1 person left and their hat is in the pile, return 1
			if(state.NumHatsInPileThatCouldBeMatched == 1 && state.NumRemainingPeople == 1)
			{
				memoizationDictionary[state] = 1.0;
				return memoizationDictionary[state];
			}
			// if only hats from people who have already selected hats remain in the pile, return 0
			if(state.NumHatsInPileThatCouldBeMatched == 0)
			{
				memoizationDictionary[state] = 0.0;
				return memoizationDictionary[state];
			}
			var probabilityPersonDrawsTheirOwnHat /* probability this person's hat is still in pile * probability that they draw that hat */
				= ((double)state.NumHatsInPileThatCouldBeMatched / (double)state.NumRemainingPeople) * (1 / state.NumRemainingPeople);
			var probabilityPersonDrawsHatOfSomeoneWhoHasAlreadyDrawnHat 
				/* we know that there are (state.NumRemainingPeople - state.NumHatsInPileThatCouldBeMatched) hats from people who have already drawn hats*/
				= (state.NumRemainingPeople - state.NumHatsInPileThatCouldBeMatched) / (double)state.NumRemainingPeople;
			var probabilityPersonDrawsHatOfSomeoneElseWhoNeedsToDrawHat 
				/* need to consider both the case where this person's hat has not been drawn and the probability that this person's hat has been drawn*/
				= ((double)state.NumHatsInPileThatCouldBeMatched / (double)state.NumRemainingPeople)
					* (state.NumHatsInPileThatCouldBeMatched - 1) / (double)state.NumRemainingPeople
					+
					/* this person's hat has already been drawn in the following value*/
					((double)(state.NumRemainingPeople - state.NumHatsInPileThatCouldBeMatched) / (double)state.NumRemainingPeople)
					* (state.NumHatsInPileThatCouldBeMatched) / (double)state.NumRemainingPeople;

			var probabilityThatSomeoneDrawsTheirOwnHat = probabilityPersonDrawsTheirOwnHat;
			if(probabilityPersonDrawsHatOfSomeoneWhoHasAlreadyDrawnHat > 0)
			{
				probabilityThatSomeoneDrawsTheirOwnHat += this.CalculateProbabilityAtLeastOnePersonGetsTheirOwnHatInternal(
					new State(state.NumRemainingPeople - 1, state.NumHatsInPileThatCouldBeMatched), memoizationDictionary);
			}
			if(probabilityPersonDrawsHatOfSomeoneElseWhoNeedsToDrawHat > 0)
			{
				probabilityThatSomeoneDrawsTheirOwnHat += this.CalculateProbabilityAtLeastOnePersonGetsTheirOwnHatInternal(
					new State(state.NumRemainingPeople - 1, state.NumHatsInPileThatCouldBeMatched - 1), memoizationDictionary);
			}
			memoizationDictionary[state] = probabilityPersonDrawsTheirOwnHat;
			return memoizationDictionary[state];
		}

		private class State {
			private readonly int _numRemainingPeople;
			private readonly int _numHatsInPileThatCouldBeMatched;

			public State(int numRemainingPeople, int numHatsInPileThatCouldBeMatched)
			{
				this._numRemainingPeople = numRemainingPeople;
				this._numHatsInPileThatCouldBeMatched = numHatsInPileThatCouldBeMatched;
			}

			public int NumRemainingPeople { get { return this._numRemainingPeople;  } }
			public int NumHatsInPileThatCouldBeMatched { get { return this._numHatsInPileThatCouldBeMatched;  } }

			public override bool Equals(object obj)
			{
				if (!(obj is State))
				{
					return false;
				}
				return ((State)obj).NumRemainingPeople == NumRemainingPeople
					&& ((State)obj).NumHatsInPileThatCouldBeMatched == NumHatsInPileThatCouldBeMatched;
			}

			public override int GetHashCode()
			{
				int hash = 17;
				hash = hash * 23 + _numRemainingPeople.GetHashCode();
				hash = hash * 23 + _numHatsInPileThatCouldBeMatched.GetHashCode();
				return hash;
			}
		}
	}
}
