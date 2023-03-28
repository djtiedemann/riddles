using System;
using System.Collections.Generic;
using System.Text;

namespace Riddles.NumberTheory.Core
{
    public class PowerOfTwoCalculator
    {
        public double FindNextPowerOfTwoInclusive(int n)
        {
            if(n <= 1)
            {
                return 1;
            }
            int numPowersOfTwo = 1;
            n -= 1;
            while(n > 1)
            {
                n = n >> 1;
                numPowersOfTwo++;
            }
            return 1 << numPowersOfTwo;
        }
    }
}
