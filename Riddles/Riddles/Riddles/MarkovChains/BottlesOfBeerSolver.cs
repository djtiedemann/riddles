using System;
using System.Collections.Generic;
using System.Text;
using static Riddles.MarkovChains.RandomBallPainter;

namespace Riddles.MarkovChains
{
    // https://fivethirtyeight.com/features/can-you-take-down-all-the-bottles-of-beer/
    public class BottlesOfBeerSolver
    {
        private MarkovChainSolver _markovChainSolver;
        public BottlesOfBeerSolver() { 
            this._markovChainSolver = new MarkovChainSolver();
        }
        public double? CalculateExpectedNumberOfVerses(int numVersesInSong, double probabilityOfForgetting)
        {
            if(numVersesInSong <= 0 || probabilityOfForgetting <= 0)
            {
                return null;
            }
            return this._markovChainSolver.GetExpectedValueOfNumTurnsToReachTerminalState(
                new BottlesOfBeerState(numVersesInSong),
                this.GetStateTransitions,
                new BottlesOfBeerGlobalArgs(numVersesInSong, probabilityOfForgetting)
            );
        }

        public Dictionary<BottlesOfBeerState, double> GetStateTransitions(
            BottlesOfBeerState state, 
            BottlesOfBeerGlobalArgs args)
        {
            if(state.NumBottles == 1)
            {
                // It's unclear if you can forget the last verse.
                // If you can, the dictionary should contain 1 element (the transition back)
                // Otherwise, it should contain nothing, representing no transitions from the state
                return new Dictionary<BottlesOfBeerState, double>
                {
                    { new BottlesOfBeerState(args.NumVersesInSong), args.ProbabilityOfForgetting}
                };
            }
            return new Dictionary<BottlesOfBeerState, double>
            {
                { new BottlesOfBeerState(state.NumBottles - 1), 1 - args.ProbabilityOfForgetting },
                { new BottlesOfBeerState(args.NumVersesInSong), args.ProbabilityOfForgetting }
            };
        }

        public class BottlesOfBeerGlobalArgs
        {
            public BottlesOfBeerGlobalArgs(int numVersesInSong, double probabilityOfForgetting) {
                this.NumVersesInSong = numVersesInSong;
                this.ProbabilityOfForgetting = probabilityOfForgetting;
            } 
            public int NumVersesInSong { get; }
            public double ProbabilityOfForgetting { get; }
        }

        public class BottlesOfBeerState : IMarkovChainState
        {
            public BottlesOfBeerState(int numBottles)
            {
                this.NumBottles = numBottles;
            }

            public int NumBottles { get; }

            public bool IsStateTerminalState() {
                return NumBottles == 0;
            }

            public string TerminalStateLabel()
            {
                return string.Empty;
            }

            public override bool Equals(object obj)
            {
                if (!(obj is BottlesOfBeerState))
                {
                    return false;
                }
                return ((BottlesOfBeerState)obj).NumBottles == this.NumBottles;
            }

            public override int GetHashCode()
            {
                return this.NumBottles.GetHashCode();
            }
        }
    }
}
