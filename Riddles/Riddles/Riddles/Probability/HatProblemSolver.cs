using System;
using System.Collections.Generic;
using System.Text;

namespace Riddles.Probability
{
	public class HatProblemSolver
	{
		private Dictionary<int, double> _cache;
		Dictionary<State, double> _memoizationDictionary;

		public HatProblemSolver()
		{
			this._cache = new Dictionary<int, double>();
			this._memoizationDictionary = new Dictionary<State, double>();
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
			var initialState = new State(n, n);
			return this.CalculateProbabilityAtLeastOnePersonGetsTheirOwnHatInternal(initialState, this._memoizationDictionary);
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
			if(state.NumHatsInPileThatCouldBeMatched == state.NumRemainingPeople)
			{
				var probabilityThisPersonDrawsOwnHat = 1.0 / (double)state.NumRemainingPeople;
				var probabilityThisPersonDrawsAnotherPersonsHat = 1 - probabilityThisPersonDrawsOwnHat;
				var probabilityOwnHatIsDrawnEventually = probabilityThisPersonDrawsOwnHat;
				if(probabilityThisPersonDrawsAnotherPersonsHat > 0)
				{
					// in this case, the person could have drawn their hat and instead drew someone else's hat. this means the number of people
					// drawing hats is reduced by 1.
					// 
					// it also means the number of hats that could be matched is reduced by 2, because both the hat drawn and this person's hat
					// are thrown out of the pool
					var nextStateIfPersonDrawsAnotherPersonsHat = new State(state.NumRemainingPeople - 1, state.NumHatsInPileThatCouldBeMatched - 2);
					probabilityOwnHatIsDrawnEventually += probabilityThisPersonDrawsAnotherPersonsHat
						* this.CalculateProbabilityAtLeastOnePersonGetsTheirOwnHatInternal(
							nextStateIfPersonDrawsAnotherPersonsHat, 
							memoizationDictionary
						);					
				}
				memoizationDictionary[state] = probabilityOwnHatIsDrawnEventually;
				return memoizationDictionary[state];

			}
			if(state.NumHatsInPileThatCouldBeMatched < state.NumRemainingPeople)
			{
				// WLOG, assume that the next person to pick a hat is a person whose hat is not in the pile
				var oddsPersonDrawsHatOfSomeoneElseWhoHasRemainingHat = (double)state.NumHatsInPileThatCouldBeMatched / (double)state.NumRemainingPeople;
				var oddsPersonDrawsHatOfSomeoneOutsideGroup = 1 - oddsPersonDrawsHatOfSomeoneElseWhoHasRemainingHat;
				var probabilitySomeoneDrawsOwnHat = 0.0;
				if(oddsPersonDrawsHatOfSomeoneElseWhoHasRemainingHat > 0)
				{
					// in this case, 1 fewer person remains and there is 1 less hat that could be matched (the one drawn)
					var nextState = new State(state.NumRemainingPeople - 1, state.NumHatsInPileThatCouldBeMatched - 1);
					var incrementalProbabilityOfSomeoneDrawingOwnHat = oddsPersonDrawsHatOfSomeoneElseWhoHasRemainingHat
						* this.CalculateProbabilityAtLeastOnePersonGetsTheirOwnHatInternal(nextState, memoizationDictionary);
					probabilitySomeoneDrawsOwnHat += incrementalProbabilityOfSomeoneDrawingOwnHat;
				}
				if(oddsPersonDrawsHatOfSomeoneOutsideGroup > 0)
				{
					// in this case, 1 fewer person remains, but the same number of hats can be matched
					var nextState = new State(state.NumRemainingPeople - 1, state.NumHatsInPileThatCouldBeMatched);
					var incrementalProbabilityOfSomeoneDrawingOwnHat = oddsPersonDrawsHatOfSomeoneOutsideGroup
						* this.CalculateProbabilityAtLeastOnePersonGetsTheirOwnHatInternal(nextState, memoizationDictionary);
					probabilitySomeoneDrawsOwnHat += incrementalProbabilityOfSomeoneDrawingOwnHat;
				}
				memoizationDictionary[state] = probabilitySomeoneDrawsOwnHat;
				return memoizationDictionary[state];
			}
			throw new InvalidOperationException("all cases should have been exhausted here. should be impossible to reach");
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
