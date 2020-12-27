using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Riddles.Probability.SetGeneration
{
	public class PasscodeGenerator
	{
		public PasscodeGenerator()
		{

		}

		public List<string> GenerateAllPasscodes(int lengthOfPasscode, int sizeOfCharacterSet, char firstCharacter)
		{
			if (lengthOfPasscode <= 0 || sizeOfCharacterSet <= 0)
			{
				return null;
			}

			List<string> passcodes = new List<string>();
			var initialPasscode = new String(firstCharacter, lengthOfPasscode);
			var lastCharacter = (char)(firstCharacter + (sizeOfCharacterSet - 1));

			var currentPasscode = initialPasscode;
			passcodes.Add(currentPasscode);
			while (currentPasscode != null)
			{
				currentPasscode = this.GenerateNextPasscode(currentPasscode, firstCharacter, lastCharacter);
				if (currentPasscode != null)
				{
					passcodes.Add(currentPasscode);
				}
			}
			return passcodes;
		}

		private string GenerateNextPasscode(string currentPasscode, char firstCharacter, char lastCharacter)
		{
			var currentPasscodeAsCharArray = currentPasscode.ToCharArray();

			// this is the last passcode if every character is the last character
			if (currentPasscodeAsCharArray.All(c => c == lastCharacter))
			{
				return null;
			}

			for (int i = currentPasscodeAsCharArray.Length - 1; i >= 0; i--)
			{
				if (currentPasscodeAsCharArray[i] != lastCharacter)
				{
					currentPasscodeAsCharArray[i]++;
					for (int j = i + 1; j < currentPasscodeAsCharArray.Length; j++)
					{
						currentPasscodeAsCharArray[j] = firstCharacter;
					}
					break;
				}
			}
			var nextPasscode = currentPasscodeAsCharArray.Aggregate("", (agg, c) => $@"{agg}{c}");
			return nextPasscode;
		}
	}
}
