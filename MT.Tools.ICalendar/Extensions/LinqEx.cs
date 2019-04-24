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

        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key)
        {
            return dict.ContainsKey(key) ? dict[key] : default(TValue);
        }

        #endregion Methods
    }
}
