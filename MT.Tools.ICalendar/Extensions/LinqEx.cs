using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MT.Tools.ICalendar.Extensions
{
    public static class LinqEx
    {
        #region Methods

        public static bool ExactlyOne<T>(this IEnumerable<T> items, Func<T, bool> predicate)
        {
            return items.Where(predicate).Take(2).Count() == 1;
        }

        public static bool MaximumOne<T>(this IEnumerable<T> items, Func<T, bool> predicate)
        {
            return items.Where(predicate).Take(2).Count() <= 1;
        }

        #endregion Methods
    }

    public static class DictionaryEx
    {
        #region Methods

        //public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key)
        //{
        //    return dict.ContainsKey(key) ? dict[key] : default(TValue);
        //}

        public static Dictionary<TKey, TValue> ExceptKeys<TKey, TValue>(this Dictionary<TKey, TValue> dict, IEnumerable<TKey> exceptKeys)
        {
            // get the keys to be extracted in O(n)
            var keysDist = dict.Keys.Distinct().Except(exceptKeys.Distinct()).ToHashSet();

            // extract the given key-value-pairs from the dictionary in O(n) (HashSet.Contains() is O(1)!!!)
            var extractedPairs = dict.Where(x => keysDist.Contains(x.Key));

            // create a new dictionary with the extracted key-value-pairs in O(n)
            return new Dictionary<TKey, TValue>(extractedPairs);
        }

        public static void Update<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue value)
        {
            if (dict.ContainsKey(key))
            {
                // update value of an already added key-value-pair
                dict[key] = value;
            }
            else
            {
                // add new key-value-pair
                dict.Add(key, value);
            }
        }

        public static Dictionary<TValue, TKey> InversePairs<TKey, TValue>(this IDictionary<TKey, TValue> source)
        {
            var dictionary = new Dictionary<TValue, TKey>();

            // loop through all pairs of the input dictionary
            foreach (var entry in source)
            {
                // make sure that the dictionary is reversible
                if (dictionary.ContainsKey(entry.Value)) { throw new ArgumentException("Irreversible source dictionary detected! At least one value is duplicate!"); }

                // apply the key to the new dictionary
                dictionary.Add(entry.Value, entry.Key);
            }

            return dictionary;
        }

        #endregion Methods
    }
}
