using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Interviews
{
	public class TextJustification
	{
		public List<List<string>> GetOptimalLineSpacing(List<string> words, int maximumLineLength)
		{
			Tuple<List<int>, int>[] memo = new Tuple<List<int>, int>[words.Count()];
			(var optimalLines, var optimalCost) = this.GetOptimalLineSpacingInternal(words, 0, memo, maximumLineLength);
			return this.ExpandOutOptimalLines(words, optimalLines);
		}

		private (List<int> optimalLineBreaks, int optimalCost) GetOptimalLineSpacingInternal(
			List<string> words,
			int startingAtIndex,
			Tuple<List<int>, int>[] memo,
			int optimalLineLength)
		{
			if (memo[startingAtIndex] != null)
			{
				var result = memo[startingAtIndex];
				return (result.Item1, result.Item2);
			}
			if (startingAtIndex == words.Count() - 1)
			{
				(int lineLength, int cost) = this.CalculateCost(words, startingAtIndex, words.Count() - 1, optimalLineLength);
				var lineBreaks = new List<int> { startingAtIndex };
				memo[startingAtIndex] = Tuple.Create(lineBreaks, cost);
				return (lineBreaks, cost);
			}
			List<int> optimalLineBreaks = new List<int> { startingAtIndex };
			(var _, var optimalCost) = this.CalculateCost(words, startingAtIndex, words.Count() - 1, optimalLineLength);
			for (int i = startingAtIndex + 1; i < words.Count(); i++)
			{
				(int firstLineLength, int firstLineCost) = this.CalculateCost(words, startingAtIndex, i - 1, optimalLineLength);
				if (firstLineLength > optimalLineLength) { continue; }
				(List<int> lineBreaksForLaterLines, int costForLaterLines) = this.GetOptimalLineSpacingInternal(
					words, i, memo, optimalLineLength);
				if (firstLineCost + costForLaterLines < optimalCost)
				{
					optimalCost = firstLineCost + costForLaterLines;
					optimalLineBreaks = (new List<int> { startingAtIndex }).Concat(lineBreaksForLaterLines).ToList();
				}
			}
			memo[startingAtIndex] = Tuple.Create(optimalLineBreaks, optimalCost);
			return (optimalLineBreaks, optimalCost);
		}

		private (int lineLength, int cost) CalculateCost(List<string> words, int startingIndex, int lastIndex, int optimalLineLength)
		{
			var lineLength = words.Where((w, i) => i >= startingIndex && i <= lastIndex).Sum(w => w.Length) + (lastIndex - startingIndex);
			var cost = lineLength <= optimalLineLength ? (int)Math.Pow((optimalLineLength - lineLength), 3) : int.MaxValue;
			return (lineLength, cost);
		}
		private List<List<string>> ExpandOutOptimalLines(List<string> words, List<int> lineBreaks)
		{
			List<List<string>> lines = new List<List<string>> { };
			List<string> currentLine = new List<string> { };
			var lineIndex = 0;
			if (lineBreaks.First() == 0)
			{
				lineIndex++;
			}
			for (int i = 0; i < words.Count(); i++)
			{
				if (lineIndex < lineBreaks.Count() && i == lineBreaks[lineIndex])
				{
					lines.Add(currentLine);
					currentLine = new List<string> { };
					lineIndex++;
				}
				currentLine.Add(words[i]);
			}
			lines.Add(currentLine);
			return lines;
		}

		//	public List<List<string>> GetOptimalLineSpacing(List<string> words, int lineLength)
		//	{
		//		Tuple<List<int>, int>[,] memo = new Tuple<List<int>, int>[words.Count(), words.Count()];
		//		 (var optimalLines, var optimalCost) = this.GetOptimalLineSpacingInternal(words, 0, words.Count - 1, memo, lineLength);

		//		return this.ExpandOutOptimalLines(words, optimalLines);
		//	}

		//	private List<List<string>> ExpandOutOptimalLines(List<string> words, List<int> lineBreaks) {
		//		List<List<string>> lines = new List<List<string>> { };
		//		List<string> currentLine = new List<string> { };
		//		if (lineBreaks.First() == 0)
		//		{
		//			lineBreaks.RemoveAt(0);
		//		}
		//		for (int i = 0; i < words.Count(); i++)
		//		{
		//			if (i == lineBreaks.First())
		//			{
		//				lines.Add(currentLine);
		//				currentLine = new List<string> { };
		//			}
		//			currentLine.Add(words[i]);
		//		}
		//		lines.Add(currentLine);
		//		return lines;
		//	}

		//	private (List<int> lines, int cost) GetOptimalLineSpacingInternal(List<string> words, int indexOfFirstWord, int indexOfLastWord,
		//		Tuple<List<int>, int> [,] memo, int maximumLineLength) 
		//	{ 
		//		if(memo[indexOfFirstWord, indexOfLastWord] != null) {
		//			var solution = memo[indexOfFirstWord, indexOfLastWord];
		//			return (solution.Item1, solution.Item2);
		//		}
		//		var lineLength = words.Sum(w => w.Length) + words.Count - 1;
		//		if (lineLength <= maximumLineLength) {
		//			var optimalLines = new List<int> { indexOfFirstWord };
		//			var optimalLineLength = (int)Math.Pow(maximumLineLength - lineLength, 3);
		//			memo[indexOfFirstWord, indexOfLastWord] = Tuple.Create(optimalLines, optimalLineLength);
		//			return (optimalLines, optimalLineLength);
		//		} if (indexOfFirstWord == indexOfLastWord) {
		//			var optimalLines = new List<int> { indexOfFirstWord };
		//			var optimalLineLength = 0;
		//			memo[indexOfFirstWord, indexOfLastWord] = Tuple.Create(optimalLines, optimalLineLength);
		//			return (optimalLines, optimalLineLength);
		//		}
		//		int minimumCost = int.MaxValue;
		//		IEnumerable<int> bestLineBreaks = new List<int> { };
		//		for(int i=indexOfFirstWord; i<indexOfLastWord; i++)	{
		//			var firstLine = words.Where((w, index) => index <= i - indexOfFirstWord).ToList();
		//			var firstLineLength = words.Sum(w => w.Length) + words.Count - 1;
		//			if(firstLineLength > maximumLineLength)	{
		//				continue;
		//			}

		//			var (linesForFirstSection, costForFirstSection) = this.GetOptimalLineSpacingInternal(
		//				firstLine, 
		//				indexOfFirstWord, 
		//				indexOfFirstWord + i, 
		//				memo, 
		//				maximumLineLength);
		//			var (linesForSecondSection, costForSecondSection) = this.GetOptimalLineSpacingInternal(
		//				words.Where((w, index) => index > i - indexOfFirstWord).ToList(),
		//				indexOfFirstWord + i + 1,
		//				indexOfLastWord,
		//				memo,
		//				maximumLineLength);
		//			if(costForFirstSection + costForSecondSection < minimumCost)
		//			{
		//				minimumCost = costForFirstSection + costForSecondSection;
		//				bestLineBreaks = linesForFirstSection.Concat(linesForSecondSection);
		//			}
		//		}
		//		var bestLineBreaksList = bestLineBreaks.ToList();
		//		memo[indexOfFirstWord, indexOfLastWord] = Tuple.Create(bestLineBreaksList, minimumCost);
		//		return (bestLineBreaksList, minimumCost);
		//	}
		//}
	}
}
