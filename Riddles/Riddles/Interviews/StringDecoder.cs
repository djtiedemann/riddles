using System;
using System.Collections.Generic;
using System.Text;

namespace Interviews
{
	public class StringDecoder
	{
		public string DecodeString(string input)
		{
			var currentString = "";
			Stack<Tuple<string, int>> previousStateStack = new Stack<Tuple<string, int>>();
			int duplicationFactor = 0;
			for(int i=0; i<input.Length; i++)
			{
				if ((input[i] >= 'a' && input[i] <= 'z') || (input[i] >= 'A' && input[i] <= 'Z'))
				{
					currentString += input[i];
					continue;
				}
				else if (input[i] >= '0' && input[i] <= '9')
				{
					duplicationFactor = duplicationFactor * 10 + int.Parse($@"{input[i]}");
				}
				else if (input[i] == '{')
				{
					previousStateStack.Push(Tuple.Create(currentString, duplicationFactor));
					duplicationFactor = 0;
					currentString = "";
				}
				else if (input[i] == '}') {
					var previousState = previousStateStack.Pop();
					var stringToRepeat = currentString;
					currentString = "";
					for (int count = 0; count < previousState.Item2; count++) {
						currentString = currentString + stringToRepeat;
					}
					currentString = previousState.Item1 + currentString;					
				}
			}
			return currentString;
		}
	}
}
