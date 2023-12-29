using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Riddles.Strategy;

namespace Riddles.Tests.Strategy
{
    public class ConnectionsGameOptimalStrategySolverTest
    {
        [Test]
        public void TestCalculateNumGuessesNeededToSolve()
        {
            var connectionsGameOptimalStrategySolver = new ConnectionsGameOptimalStrategySolver();
            var actualNumGuessesNeededToSolve = connectionsGameOptimalStrategySolver.CalculateNumGuessesNeededToSolve();
            Assert.AreEqual(actualNumGuessesNeededToSolve, 6);
        }
    }
}
