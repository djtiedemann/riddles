using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Riddles.Caching
{
    public class CacheableDictionary<TKey, TValue>
    {
        public CacheableDictionary(Dictionary<TKey, TValue> dictionary)
        {
            this.Dictionary = dictionary;
        }

        public Dictionary<TKey, TValue> Dictionary { get; }

        public string GetStringRepresentation()
        {
            return this.Dictionary.Keys.OrderBy(k => k)
                .Select(k => $"{k}:{this.Dictionary[k]}")
                .Aggregate("", (score, i) => $"{score},{i}");
        }

        public override bool Equals(object obj)
        {
            if (!(obj is CacheableDictionary<TKey, TValue>))
            {
                return false;
            }
            return this.GetStringRepresentation()
                == ((CacheableDictionary<TKey, TValue>)obj).GetStringRepresentation();
        }

        public override int GetHashCode()
        {
            return this.GetStringRepresentation().GetHashCode();
        }

        public CacheableDictionary<TKey, TValue> Clone()
        {
            return new CacheableDictionary<TKey, TValue>(new Dictionary<TKey, TValue>(this.Dictionary));
        }
    }
}
