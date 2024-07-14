using System;
using System.Collections.Generic;
using System.Text;
using static Riddles.MarkovChains.BottlesOfBeerSolver;

namespace Riddles.MarkovChains
{
    public class TennisGameSolver
    {
        private MarkovChainSolver _markovChainSolver;
        public TennisGameSolver() { 
            this._markovChainSolver = new MarkovChainSolver();
        }

        public Dictionary<int, double> CalculateOddsOfWinningGameByXPoints(
            int numPointsToWin,
            int numPointsToWinBy
        )
        {
            var odds = new Dictionary<int, double>();
            var args = new TennisGameSolverArgs(numPointsToWin, numPointsToWinBy);
            for(int i=numPointsToWinBy; i<=numPointsToWin; i++)
            {
                var oddsOfSpecificState =
                    this._markovChainSolver
                        .GetProbabilityOfArrivingAtSpecificTerminalStateLabel(
                            new TennisGameState(0, 0, numPointsToWin, numPointsToWinBy),
                            this.GetStateTransitions,
                            $"{i}",
                            args
                        );
                odds[i] = oddsOfSpecificState;
            }
            return odds;
        }

        public Dictionary<TennisGameState, double> GetStateTransitions(
           TennisGameState state,
           TennisGameSolverArgs args)
        {
            if (state.IsStateTerminalState())
            {
                return new Dictionary<TennisGameState, double>();
            }
            return new Dictionary<TennisGameState, double> {
                { new TennisGameState(
                    state.NumWinsTeamOne + 1,
                    state.NumWinsTeamTwo,
                    args.NumPointsToWin,
                    args.NumPointsToWinBy
                ), 0.5 },
                { new TennisGameState(
                    state.NumWinsTeamOne,
                    state.NumWinsTeamTwo + 1,
                    args.NumPointsToWin,
                    args.NumPointsToWinBy
                ), 0.5 },
            };
        }

        public class TennisGameState : IMarkovChainState
        {
            private int _numPointsToWin;
            private int _numPointsToWinBy;
            public TennisGameState(
                int numWinsTeamOne, 
                int numWinsTeamTwo,
                int numPointsToWin,
                int numPointsToWinBy
            )
            {
                this.NumWinsTeamOne = numWinsTeamOne;
                this.NumWinsTeamTwo = numWinsTeamTwo;
                this._numPointsToWin = numPointsToWin;
                this._numPointsToWinBy = numPointsToWinBy;
                this.StringRepresentation = this.GetStringRepresentation();
            }

            public int NumWinsTeamOne { get; }
            public int NumWinsTeamTwo { get; }
            public string StringRepresentation { get; } 
            
            private string GetStringRepresentation()
            {
                if (this.IsStateTerminalState())
                {
                    return $"Y{this.TerminalStateLabel()}";
                }
                return $"N{this.NumWinsTeamOne}:{this.NumWinsTeamTwo}";
            }

            public bool IsStateTerminalState()
            {
                return 
                    this.NumWinsTeamOne >= this._numPointsToWin
                    || this.NumWinsTeamTwo >= this._numPointsToWin
                    || (this.NumWinsTeamOne >= (this._numPointsToWin - this._numPointsToWinBy)
                    && (this.NumWinsTeamTwo >= (this._numPointsToWin - this._numPointsToWinBy)));
            }

            public string TerminalStateLabel()
            {
                var label = Math.Abs(this.NumWinsTeamOne - this.NumWinsTeamTwo);
                if(label < 2)
                {
                    label = 2;
                }
                return $"{label}";
            }

            public override bool Equals(object obj)
            {
                if (!(obj is TennisGameState))
                {
                    return false;
                }
                return ((TennisGameState)obj).StringRepresentation 
                    == this.StringRepresentation;
            }

            public override int GetHashCode()
            {
                return this.StringRepresentation.GetHashCode();
            }
        }
    }

    public class TennisGameSolverArgs
    {
        public TennisGameSolverArgs(
            int numPointsToWin,
            int numPointsToWinBy
        ) { 
            this.NumPointsToWin = numPointsToWin;
            this.NumPointsToWinBy = numPointsToWinBy;
        }
        public int NumPointsToWin { get; }
        public int NumPointsToWinBy { get; }

    }
}