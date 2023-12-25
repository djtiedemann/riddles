using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Riddles.Combinatorics.Core.Permutations;
using Riddles.Combinatorics.Core.SetGeneration;

namespace Riddles.MarkovChains
{
    // https://thefiddler.substack.com/p/dont-flip-out
    // Kyle and Julien are playing a game in which they each toss their own fair coins.
    // On each turn of the game, both players flip their own coin once.
    // If, at any point, Kyle’s most recent three flips are Tails, Tails, and Heads(i.e., TTH),
    // then he wins.
    // If, at any point, Julien’s most recent three flips are Tails, Tails, and Tails(i.e, TTT),
    // then he wins.
    // However, both players can’t win at the same time.
    // If Kyle gets TTH at the same time Julien gets TTT, then no one wins, and they
    // continue flipping.They don’t start over completely or erase their history,
    // mind you—they merely continue flipping, so that one of them could conceivably win
    // in the next flip or two. What is the probability that Kyle wins this game?
    public class CoinFlippingRaceSolver
    {
        private MarkovChainSolver _markovChainSolver;
        private PermutationWithRepetitionGenerator _outcomeGenerator;
        private PermutationWithoutRepetitionGenerator _permutationGenerator;
        public CoinFlippingRaceSolver() { 
            this._markovChainSolver = new MarkovChainSolver();
            this._outcomeGenerator = new PermutationWithRepetitionGenerator();
            this._permutationGenerator = new PermutationWithoutRepetitionGenerator();
        }

        public double CalculateProbabilityOfCertainPlayerWinningCoinFlipRace(
            string terminalLabel,
            Dictionary<char, double> outcomeProbabilities,
            List<List<string>> winningSequences)
        {
            var initialEmptyArrays = new List<List<string>>();
            for(int i=0; i<winningSequences.Count; i++)
            {
                initialEmptyArrays.Add(new List<string>());
            }
            var initialState = new CoinFlippingRaceState(initialEmptyArrays, winningSequences);
            var collectiveOutcomes = this._outcomeGenerator.GenerateAllOutcomes(
                winningSequences.Count,
                outcomeProbabilities.Keys.ToList()
            );
            var collectiveOutcomeProbabilities =
                collectiveOutcomes.ToDictionary(
                    c => c,
                    c => c.ToCharArray()
                            .Aggregate(1.0, (agg, v) => agg * outcomeProbabilities[v])
            );
            var globalCoinFlippingRaceState = new CoinFlippingRaceGlobalState(
                collectiveOutcomes,
                collectiveOutcomeProbabilities
            );
            return this._markovChainSolver.GetProbabilityOfArrivingAtSpecificTerminalStateLabel(
                initialState,
                this.GetStateTransitions,
                terminalLabel,
                globalCoinFlippingRaceState
            );
        }

        public Dictionary<string, double> FindWinningLikelihoodBasedOnStartingPick(
            int numPlayers, 
            Dictionary<char, double> outcomeDictionary,
            int numRolls)
        {
            var possibleOutcomes = this._outcomeGenerator.GenerateAllOutcomes(
                numRolls,
                outcomeDictionary.Keys.ToList()
            );
            var possibleSequences = this._permutationGenerator
                .GenerateAllPermutations(possibleOutcomes.Count, numPlayers)
                .Select(g => g.GetPermutation()).ToList();
            var selectionsGroupedByFirstPerson = possibleSequences.GroupBy(o => o[0]);
            var likelihoodOfWinningByHand = new Dictionary<string, double>();
            foreach(var outcomeDrawnByFirstPerson in selectionsGroupedByFirstPerson)
            {
                var probabilityOfWinning = 0.0;
                foreach(var fullOutcome in outcomeDrawnByFirstPerson)
                {
                    var winningOutcomeList = new List<List<string>>();
                    for(int i=0; i<numPlayers; i++)
                    {
                        var playerOutcome = possibleOutcomes[fullOutcome[i]-1];
                        winningOutcomeList.Add(
                            playerOutcome.ToCharArray().Select(i => i.ToString()).ToList()
                        );
                    }
                    var probabilityOfWinningThisRound = this.
                        CalculateProbabilityOfCertainPlayerWinningCoinFlipRace(
                            "0",
                            outcomeDictionary,
                            winningOutcomeList
                        );
                    probabilityOfWinning +=
                        probabilityOfWinningThisRound * (1.0 / outcomeDrawnByFirstPerson.Count());
                }
                var firstPersonWinningKey = possibleOutcomes[outcomeDrawnByFirstPerson.Key - 1];
                likelihoodOfWinningByHand[firstPersonWinningKey]
                    = probabilityOfWinning;
            }
            return likelihoodOfWinningByHand;
        }

        public Dictionary<CoinFlippingRaceState, double> GetStateTransitions(
            CoinFlippingRaceState state,
            CoinFlippingRaceGlobalState globalState)
        {
            var stateTransitions = new Dictionary<CoinFlippingRaceState, double>();
            // probably not necessary, but prevent terminal state from infinitely continuing
            if (state.IsStateTerminalState())
            {
                stateTransitions[state] = 1;
            }
            else
            {
                foreach (var outcome in globalState.Outcomes) 
                {
                    var clonedCurrentFlips = state.DeepCloneCurrentFlips();
                    for (int playerNum = 0; playerNum < outcome.Length; playerNum++)
                    {
                        var flip = outcome[playerNum];
                        if (clonedCurrentFlips[playerNum].Count 
                            == state.WinningFlips[playerNum].Count)
                        {
                            clonedCurrentFlips[playerNum].RemoveAt(0);
                        }
                        clonedCurrentFlips[playerNum].Add(flip.ToString());
                    }
                    var newState = new CoinFlippingRaceState(
                        clonedCurrentFlips,
                        state.WinningFlips
                    );
                    stateTransitions[newState] = globalState.OutcomeProbabilities[outcome];
                }
            }
            return stateTransitions;
        }

        public class CoinFlippingRaceGlobalState
        {
            public CoinFlippingRaceGlobalState(
                List<string> outcomes,
                Dictionary<string, double> outcomeProbabilities
            )
            {
                this.Outcomes = outcomes;
                this.OutcomeProbabilities = outcomeProbabilities;
            }
            public List<string> Outcomes { get; }
            public Dictionary<string, double> OutcomeProbabilities { get; }  
        }

        public class CoinFlippingRaceState : IMarkovChainState
        {
            private bool _isTerminalState;
            private string _terminalStateLabel;
            public CoinFlippingRaceState(
                List<List<string>> currentFlips,
                List<List<string>> winningFlips)
            {
                this.CurrentFlips = currentFlips;
                this.WinningFlips = winningFlips;
                this.StringRepresentation = currentFlips.Select(
                    i => i.Aggregate("", (agg, i) => $"{agg}{i}"))
                    .Aggregate("", (agg, i) => $"{agg}{i}");

                var winningIndicators = new List<bool>();
                for(int playerNum = 0; playerNum<currentFlips.Count; playerNum++)
                {
                    var currentFlipsForPlayer = currentFlips[playerNum];
                    var winningFlipsForPlayer = winningFlips[playerNum];
                    if(currentFlipsForPlayer.Count == winningFlipsForPlayer.Count)
                    {
                        var foundMismatch = false;
                        for(int flipNum = 0; flipNum<currentFlipsForPlayer.Count; flipNum++)
                        {
                            if (currentFlipsForPlayer[flipNum] != winningFlipsForPlayer[flipNum])
                            {
                                foundMismatch = true;
                                break;
                            }
                        }
                        winningIndicators.Add(!foundMismatch);
                    }
                    else
                    {
                        winningIndicators.Add(false);
                    }
                }
                this._isTerminalState = winningIndicators.Where(x => x).Count() == 1;
                this._terminalStateLabel =
                    this._isTerminalState ?
                        winningIndicators
                            .Select((hasWon, playerNum) => (hasWon, playerNum))
                            .Where(x => x.hasWon)
                            .Select(x => $"{x.playerNum}")
                            .Single() : null;
            }

            public bool IsStateTerminalState()
            {
                return this._isTerminalState;
            }
            public string? TerminalStateLabel()
            {
                return this._terminalStateLabel;
            }

            public virtual List<List<string>> CurrentFlips { get; }

            public List<List<string>> WinningFlips { get; }

            public string StringRepresentation { get; }

            public List<List<string>> DeepCloneCurrentFlips()
            {
                var listClone = new List<List<string>>();
                foreach(var playerList in this.CurrentFlips)
                {
                    var newPlayerList = new List<string>();
                    foreach(var flip in playerList)
                    {
                        newPlayerList.Add(flip.ToString());
                    }
                    listClone.Add(newPlayerList);
                }
                return listClone;
            }

            public override bool Equals(object obj)
            {
                if (!(obj is CoinFlippingRaceState))
                {
                    return false;
                }
                var otherCoinFlippingRaceState = ((CoinFlippingRaceState)obj);
                if(this.CurrentFlips.Count != otherCoinFlippingRaceState.CurrentFlips.Count)
                {
                    return false;
                }
                for(int i=0; i < this.CurrentFlips.Count; i++)
                {
                    var thisPlayerFlip = this.CurrentFlips[i];
                    var otherPlayerFlip = otherCoinFlippingRaceState.CurrentFlips[i];
                    if(thisPlayerFlip.Count != otherPlayerFlip.Count)
                    {
                        return false;
                    }
                    for(int j=0; j<thisPlayerFlip.Count; j++)
                    {
                        if (thisPlayerFlip[j] != otherPlayerFlip[j])
                        {
                            return false;
                        }
                    }
                }
                return true;
            }

            public override int GetHashCode()
            {
                return this.StringRepresentation.GetHashCode();
            }
        }
    }
}