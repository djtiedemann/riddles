using System;
using System.Collections.Generic;
using System.Text;

namespace Interviews
{
	public class MinimumWindowSubstring
	{
		public string FindMinimumWindowSubstring(string searchString, string targetString) {
			var numCharactersNeededToSeeFromTargetString = targetString.Length;
			Dictionary<char, int> numCharactersNeededToCompleteSearchString = new Dictionary<char, int> { };
			foreach (var targetChar in targetString) {
				numCharactersNeededToCompleteSearchString[targetChar] =
					numCharactersNeededToCompleteSearchString.ContainsKey(targetChar)
						? numCharactersNeededToCompleteSearchString[targetChar] + 1
						: 1;
			}

			int leadingPointer = 0;
			int laggingPointer = 0;
			int minWindowSize = int.MaxValue;
			int minWindowStart = 0;

			for(leadingPointer=0; leadingPointer< searchString.Length; leadingPointer++)
			{
				if (numCharactersNeededToCompleteSearchString.ContainsKey(searchString[leadingPointer]))
				{
					numCharactersNeededToCompleteSearchString[searchString[leadingPointer]]--;
					if (numCharactersNeededToCompleteSearchString[searchString[leadingPointer]] >= 0) {
						numCharactersNeededToSeeFromTargetString--;
					}
					if (numCharactersNeededToSeeFromTargetString == 0 && numCharactersNeededToCompleteSearchString[searchString[leadingPointer]] <= 0) {
						while (!numCharactersNeededToCompleteSearchString.ContainsKey(searchString[laggingPointer])
						|| numCharactersNeededToCompleteSearchString[searchString[laggingPointer]] < 0)
						{
							if (numCharactersNeededToCompleteSearchString.ContainsKey(searchString[laggingPointer]))
							{
								numCharactersNeededToCompleteSearchString[searchString[laggingPointer]]++;
							}
							laggingPointer++;
						}
					}
					if(numCharactersNeededToSeeFromTargetString == 0 && (leadingPointer - laggingPointer) + 1 < minWindowSize)					
					{
						minWindowSize = (leadingPointer - laggingPointer) + 1;
						minWindowStart = laggingPointer;
					}
				}
			}

			return searchString.Substring(minWindowStart, minWindowSize);
		}
	}
}
