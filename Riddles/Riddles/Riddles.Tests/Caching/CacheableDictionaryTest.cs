using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Riddles.Caching;
using System.Linq;

namespace Riddles.Tests.Caching
{
    internal class CacheableDictionaryTest
    {
        private Dictionary<int, (Dictionary<string, double>, string)> TestCaseDictionary =
            new Dictionary<int, (Dictionary<string, double>, string)>
        {
            { 1, (new Dictionary<string, double>
                {
                    { "val3", 3 },
                    { "val2", 2 },
                    { "val1", 1 }
                }, ",val1:1,val2:2,val3:3") }
        };

        [TestCase(1)]
        public void TestDictionaryEquality(int testCaseId)
        {
            var testCase = this.TestCaseDictionary[testCaseId];
            var dict = testCase.Item1;
            var dict2 = new Dictionary<string, double>();
            foreach (var key in dict.Keys)
            {
                dict2[key] = dict[key];
            }

            // underlying dictionaries for the objects should not be equal.
            // however the cacheable dictionaries should be equal.
            Assert.IsFalse(dict.Equals(dict2));
            var cacheableDictionary = new CacheableDictionary<string, double>(dict);
            var cacheableDictionary2 = new CacheableDictionary<string, double>(dict2);
            Assert.AreEqual(testCase.Item2, cacheableDictionary.GetStringRepresentation());
            Assert.AreEqual(testCase.Item2, cacheableDictionary2.GetStringRepresentation());

            Assert.AreEqual(cacheableDictionary, cacheableDictionary2);

            // verify that these dictionaries work properly as a cache key
            Dictionary<CacheableDictionary<string, double>, double> cache
                = new Dictionary<CacheableDictionary<string, double>, double>
                {
                    { cacheableDictionary, 15 }
                };

            Assert.AreEqual(cache[cacheableDictionary], 15);
            Assert.AreEqual(cache[cacheableDictionary2], 15);

            // create a new dictionary that is not of the same type
            var cacheableDictionary3 = new CacheableDictionary<bool, string>(
                new Dictionary<bool, string> { { true, "True" } }
                );

            Assert.AreEqual(cacheableDictionary3.GetStringRepresentation(), ",True:True");
            Assert.AreNotEqual(cacheableDictionary, cacheableDictionary3);
            Assert.AreNotEqual(cacheableDictionary2, cacheableDictionary3);

            var cacheableDictionary4 = new CacheableDictionary<string, double>(
                new Dictionary<string, double> { { "1", 1 } });

            Assert.AreNotEqual(cacheableDictionary, cacheableDictionary4);
            Assert.AreNotEqual(cacheableDictionary2, cacheableDictionary4);
            Assert.IsFalse(cache.ContainsKey(cacheableDictionary4));
        }

        [TestCase(1)]
        public void TestClone(int testCaseId)
        {
            var testCase = this.TestCaseDictionary[testCaseId];
            var dictionary = new CacheableDictionary<string, double>(testCase.Item1);
            var dictionary2 = dictionary.Clone();
            Assert.False(dictionary.Dictionary.Equals(dictionary2.Dictionary));
            Assert.True(dictionary.Equals(dictionary2));
            var cache = new Dictionary<CacheableDictionary<string, double>, double>();
            cache[dictionary] = 5;

            Assert.True(cache.ContainsKey(dictionary));
            Assert.True(cache.ContainsKey(dictionary2));

            var firstKey = dictionary2.Dictionary.Keys.First();
            dictionary2.Dictionary[firstKey] = dictionary2.Dictionary[firstKey] - 1;

            Assert.False(dictionary.Dictionary.Equals(dictionary2.Dictionary));
            Assert.False(dictionary.Equals(dictionary2));

            Assert.True(cache.ContainsKey(dictionary));
            Assert.False(cache.ContainsKey(dictionary2));
        }
    }
}
