using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Interviews;

namespace Interviews.Tests
{
	public class StringDecoderTest
	{
		[TestCase("3{a}2{bc}", "aaabcbc")]
		[TestCase("3{a2{c}}", "accaccacc")]
		[TestCase("2{abc}3{cd}ef", "abcabccdcdcdef")]
		[TestCase("10{ab}", "abababababababababab")]
		public void TestDecodeString(string encodedString, string expected)
		{
			var stringDecoder = new StringDecoder();
			var actual = stringDecoder.DecodeString(encodedString);
			Assert.AreEqual(expected, actual);
		}
	}
}
