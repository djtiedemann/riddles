using System;
using System.Collections.Generic;
using System.Text;
using Interviews;
using NUnit.Framework;
using System.Linq;

namespace Interviews.Tests
{
	public class TextJustificationTest
	{
		[TestCase("this is a fairly long sentence that i would like to break into separate lines. is that okay?", 15)]
		public void TestTextJustification(string sentence, int maximumLineLength)
		{
			var words = sentence.Split(" ").ToList();
			var textJustificationSolver = new TextJustification();
			var optimalLines = textJustificationSolver.GetOptimalLineSpacing(words, maximumLineLength);
		}
	}
}
