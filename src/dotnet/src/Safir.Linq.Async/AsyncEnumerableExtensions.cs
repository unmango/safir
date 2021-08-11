using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Safir.Linq.Async
{
    // Named to avoid clashing with System.Linq.Async
    [PublicAPI]
    public static class AsyncEnumerableExtensions
    {
        /// <summary>Default initial capacity to use when creating sets for internal temporary storage.</summary>
        /// <remarks>
        /// This is based on the implicit size used in previous implementations, which used a custom Set type.
        /// https://github.com/dotnet/runtime/blob/57bfe474518ab5b7cfe6bf7424a79ce3af9d6657/src/libraries/System.Linq/src/System/Linq/ToCollection.cs#L184
        /// </remarks>
        private const int DefaultInternalSetCapacity = 7;

        public static IAsyncEnumerable<TSource> DistinctBy<TSource, TKey>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            IEqualityComparer<TKey>? comparer = null)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));

            return DistinctByIterator(source, keySelector, comparer);
        }

        public static async IAsyncEnumerable<TSource> DistinctByIterator<TSource, TKey>(
            IAsyncEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            IEqualityComparer<TKey>? comparer)
        {
            var enumerator = source.GetAsyncEnumerator(); // TODO: Token

            if (!await enumerator.MoveNextAsync()) yield break;

#if NET5_0 || NET6_0
            var set = new HashSet<TKey>(DefaultInternalSetCapacity, comparer);
#else
            var set = new HashSet<TKey>(comparer);
#endif
            do
            {
                var element = enumerator.Current;
                if (set.Add(keySelector(element)))
                {
                    yield return element;
                }
            } while (await enumerator.MoveNextAsync());
        }
    }
}
