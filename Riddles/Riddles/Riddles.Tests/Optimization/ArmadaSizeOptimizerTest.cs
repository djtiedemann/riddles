using NUnit.Framework;
using Riddles.Optimization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Riddles.Tests.Optimization
{
    public class ArmadaSizeOptimizerTest
    {
        [TestCase(0, 1, 0, 0, 0, Description = "No initial assemblers")]
        [TestCase(1, 1, 1, 0, Int32.MaxValue, Description = "Free assemblers")]
        [TestCase(1, 1, 0, 1, Int32.MaxValue, Description = "Free fighters")]
        [TestCase(1, 1, 1, 1, 1, Description = "Only time to assemble one fighter")]
        [TestCase(3, 1, 1, 1, 3, Description = "Only time to assemble one per assembler")]
        [TestCase(1, 10, 1, 2, 32, Description = "Assemble assemblers early on, then fighters")]
        [TestCase(1, 10, 1, 3, 16, Description = "Assemble assemblers early on, then fighters")]
        [TestCase(1, 10, 1, 4, 12, Description = "Assemble assemblers early on, then fighters")]
        [TestCase(1, 20, 2, 4, 32, Description = "Double previous time left and time to build")]
        [TestCase(1, 20, 2, 6, 16, Description = "Double previous time left and time to build")]
        [TestCase(1, 20, 2, 8, 12, Description = "Double previous time left and time to build")]
        [TestCase(1, 11, 2, 11, 5, Description = "Number of fighters doesn't evenly divide")]
        [TestCase(1, 10, 1, 1, 512, Description = "Equally easy to make fighters and assemblers")]
        [TestCase(1, 10, 2, 1, 256, Description = "Easier to make assemblers")]
        [TestCase(1, 2400, 1, 6*24, 7864320, Description = "Actual problem statement")]
        public void TestGetMaximumNumberOfFighters(int numAssemblers, int numHoursRemaining, int timeToAssembleFighter, int timeToAssembleAssembler, int expectedNumFighters)
        {
            var armadaSizeOptimizer = new ArmadaSizeOptimizer(numAssemblers: numAssemblers, numHoursToAssembleFighter: timeToAssembleFighter, numHoursToAssembleAssembler: timeToAssembleAssembler);
            var armadaSize = armadaSizeOptimizer.GetMaximumNumberOfFighters(numHoursRemaining);
            Assert.AreEqual(expectedNumFighters, armadaSize);
        }
    }
}
