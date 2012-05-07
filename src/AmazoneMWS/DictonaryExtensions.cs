using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Amazone.MWS
{
    public static class DictonaryExtensions
    {
        public static void Update<TKey,TValue>(this IDictionary<TKey,TValue> dist, IDictionary<TKey,TValue> src)
        {
            foreach (var pair in src)
                if (dist.ContainsKey(pair.Key))
                    dist[pair.Key] = pair.Value;
                else
                    dist.Add(pair.Key, pair.Value);
        }
    }
}
