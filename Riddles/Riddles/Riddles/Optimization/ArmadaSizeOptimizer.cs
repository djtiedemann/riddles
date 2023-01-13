using System;
using System.Collections.Generic;
using System.Text;

namespace Riddles.Optimization
{
    // https://fivethirtyeight.com/features/can-you-fend-off-the-alien-armada/
    public class ArmadaSizeOptimizer
    {
        public int _numAssemblers;
        public int _numHoursToAssembleFighter;
        public int _numHoursToAssembleAssembler;
        public Dictionary<int, int> _optimalNumberOfFightersCache;
        public ArmadaSizeOptimizer(
            int numAssemblers,
            int numHoursToAssembleFighter,
            int numHoursToAssembleAssembler
        )
        {
            this._numAssemblers = numAssemblers;
            this._numHoursToAssembleFighter = numHoursToAssembleFighter;
            this._numHoursToAssembleAssembler = numHoursToAssembleAssembler;
            this._optimalNumberOfFightersCache = new Dictionary<int, int>();
        }

        public int GetMaximumNumberOfFighters(int numHoursToPrepare)
        {
            if(this._numAssemblers <= 0 || this._numHoursToAssembleFighter > numHoursToPrepare)
            {
                return 0;
            }
            if (this._numHoursToAssembleFighter <= 0 || this._numHoursToAssembleAssembler <= 0)
            {
                return Int32.MaxValue;
            }
            return this._numAssemblers *
                this.GetMaximumNumberOfFightersInternal(numHoursToPrepare);
        }

        private int GetMaximumNumberOfFightersInternal(int numHoursRemaining)
        {
            if (!this._optimalNumberOfFightersCache.ContainsKey(numHoursRemaining))
            {
                if (numHoursRemaining < this._numHoursToAssembleFighter)
                {
                    this._optimalNumberOfFightersCache.Add(numHoursRemaining, 0);
                }
                // in this case, you cannot assemble any more assemblers and must assemble fighters
                else if (numHoursRemaining <= this._numHoursToAssembleAssembler)
                {
                    this._optimalNumberOfFightersCache.Add(numHoursRemaining, numHoursRemaining / this._numHoursToAssembleFighter);
                } 
                else
                {
                    var numFightersAssembledIfOnlyAssemblingFighters = numHoursRemaining / this._numHoursToAssembleFighter;
                    var numFightersAssembledIfAssemblingAssemblerFirst =
                        2 * this.GetMaximumNumberOfFightersInternal(numHoursRemaining - this._numHoursToAssembleAssembler);
                    var numFightersAssembled = Math.Max(numFightersAssembledIfOnlyAssemblingFighters, numFightersAssembledIfAssemblingAssemblerFirst);
                    this._optimalNumberOfFightersCache.Add(numHoursRemaining, numFightersAssembled);
                }
            }
            return this._optimalNumberOfFightersCache[numHoursRemaining];
        }
    }
}
