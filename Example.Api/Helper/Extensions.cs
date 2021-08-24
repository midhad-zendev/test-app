using System;
using System.Collections.Generic;
using System.Linq;

namespace Example.Api.Helper
{
    public static class Extensions
    {
        public static IEnumerable<T> WherePrevious<T>(this IEnumerable<T> collection, Func<T, T, bool> predicate)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            if (!collection.Any())
                yield break;

            T previous = collection.First();

            yield return previous;

            foreach (var item in collection.Skip(1))
            {
                if (predicate(previous, item))
                {
                    yield return item;
                }
                else
                {
                    break;
                }
                previous = item;
            }
        }
        public static IEnumerable<T> WhereCustom<T>(this IEnumerable<T> collection, Func<T, T, bool> predicate, Func<T, bool> condition)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            if (!collection.Any())
                yield break;

            T previous = collection.First();
           
            if(condition(previous))
                yield return previous;

            foreach (var item in collection.Skip(1))
            {
                if (predicate(previous, item) && condition(item))
                {
                    yield return item;
                }
                else
                {
                    break;
                }
                previous = item;
            }
        }

    }
}
