using System;
using System.Collections.Generic;
using System.Text;

namespace Interviews
{
	public class LongestSubstringWithoutRepeatingCharacters
	{
		public int FindLongestSubstringWithoutRepeatingCharacters(string searchString) {
			Dictionary<char, int> mostRecentIndexFound = new Dictionary<char, int> { };
			int maxLongestString = 0;
			int currentLongestString = 0;
			for(int i=0; i<searchString.Length; i++)
			{
				if (!mostRecentIndexFound.ContainsKey(searchString[i]) || i - mostRecentIndexFound[searchString[i]] > currentLongestString)
				{
					currentLongestString++;					
				}
				else {
					currentLongestString = i - mostRecentIndexFound[searchString[i]];
				}
				mostRecentIndexFound[searchString[i]] = i;
				if (currentLongestString > maxLongestString) {
					maxLongestString = currentLongestString;
				}
			}
			return maxLongestString;
		}
	}
}
