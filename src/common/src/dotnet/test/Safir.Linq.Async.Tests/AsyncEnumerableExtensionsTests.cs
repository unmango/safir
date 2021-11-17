using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

// ReSharper disable AssignNullToNotNullAttribute
// ReSharper disable ConditionIsAlwaysTrueOrFalse

#nullable disable

namespace Safir.Linq.Async.Tests
{
    public class AsyncEnumerableExtensionsTests
    {
        [Fact]
        public void DistinctBy_SourceNull_ThrowsArgumentNullException()
        {
            IAsyncEnumerable<string> first = null;

            Assert.Throws<ArgumentNullException>("source", () => first.DistinctBy(x => x));
            Assert.Throws<ArgumentNullException>("source", () => first.DistinctBy(x => x, new AnagramEqualityComparer()));
        }

        [Fact]
        public void DistinctBy_KeySelectorNull_ThrowsArgumentNullException()
        {
            var source = new[] { "Bob", "Tim", "Robert", "Chris" }.ToAsyncEnumerable();
            Func<string, string> keySelector = null;

            Assert.Throws<ArgumentNullException>("keySelector", () => source.DistinctBy(keySelector));
            Assert.Throws<ArgumentNullException>("keySelector",
                () => source.DistinctBy(keySelector, new AnagramEqualityComparer()));
        }

        [Theory]
        [MemberData(nameof(DistinctBy_TestData))]
        public static async Task DistinctBy_HasExpectedOutput<TSource, TKey>(
            IAsyncEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            IEqualityComparer<TKey> comparer,
            IAsyncEnumerable<TSource> expected)
        {
            var result = source.DistinctBy(keySelector, comparer);
            
            Assert.True(await expected.SequenceEqualAsync(result));
        }

        [Theory]
        [MemberData(nameof(DistinctBy_TestData))]
        public static async Task DistinctBy_RunOnce_HasExpectedOutput<TSource, TKey>(
            IAsyncEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            IEqualityComparer<TKey> comparer,
            IAsyncEnumerable<TSource> expected)
        {
            var result = source.RunOnce().DistinctBy(keySelector, comparer);
            
            Assert.True(await expected.SequenceEqualAsync(result));
        }

        public static IEnumerable<object[]> DistinctBy_TestData()
        {
            yield return WrapArgs(
                source: AsyncEnumerable.Range(0, 10),
                keySelector: x => x,
                comparer: null,
                expected: AsyncEnumerable.Range(0, 10));

            yield return WrapArgs(
                source: AsyncEnumerable.Range(5, 10),
                keySelector: x => true,
                comparer: null,
                expected: new int[] { 5 }.ToAsyncEnumerable());

            yield return WrapArgs(
                source: AsyncEnumerable.Range(0, 20),
                keySelector: x => x % 5,
                comparer: null,
                expected: AsyncEnumerable.Range(0, 5));

            yield return WrapArgs(
                source: AsyncEnumerable.Repeat(5, 20),
                keySelector: x => x,
                comparer: null,
                expected: AsyncEnumerable.Repeat(5, 1));

            yield return WrapArgs(
                source: new string[] { "Bob", "bob", "tim", "Bob", "Tim" }.ToAsyncEnumerable(),
                keySelector: x => x,
                null,
                expected: new string[] { "Bob", "bob", "tim", "Tim" }.ToAsyncEnumerable());

            yield return WrapArgs(
                source: new string[] { "Bob", "bob", "tim", "Bob", "Tim" }.ToAsyncEnumerable(),
                keySelector: x => x,
                StringComparer.OrdinalIgnoreCase,
                expected: new string[] { "Bob", "tim" }.ToAsyncEnumerable());

            yield return WrapArgs(
                source: new (string Name, int Age)[] { ("Tom", 20), ("Dick", 30), ("Harry", 40) }.ToAsyncEnumerable(),
                keySelector: x => x.Age,
                comparer: null,
                expected: new (string Name, int Age)[] { ("Tom", 20), ("Dick", 30), ("Harry", 40) }.ToAsyncEnumerable());

            yield return WrapArgs(
                source: new (string Name, int Age)[] { ("Tom", 20), ("Dick", 20), ("Harry", 40) }.ToAsyncEnumerable(),
                keySelector: x => x.Age,
                comparer: null,
                expected: new (string Name, int Age)[] { ("Tom", 20), ("Harry", 40) }.ToAsyncEnumerable());

            yield return WrapArgs(
                source: new (string Name, int Age)[] { ("Bob", 20), ("bob", 30), ("Harry", 40) }.ToAsyncEnumerable(),
                keySelector: x => x.Name,
                comparer: null,
                expected: new (string Name, int Age)[] { ("Bob", 20), ("bob", 30), ("Harry", 40) }.ToAsyncEnumerable());

            yield return WrapArgs(
                source: new (string Name, int Age)[] { ("Bob", 20), ("bob", 30), ("Harry", 40) }.ToAsyncEnumerable(),
                keySelector: x => x.Name,
                comparer: StringComparer.OrdinalIgnoreCase,
                expected: new (string Name, int Age)[] { ("Bob", 20), ("Harry", 40) }.ToAsyncEnumerable());

            object[] WrapArgs<TSource, TKey>(
                IAsyncEnumerable<TSource> source,
                Func<TSource, TKey> keySelector,
                IEqualityComparer<TKey> comparer,
                IAsyncEnumerable<TSource> expected)
                => new object[] { source, keySelector, comparer, expected };
        }

        private class AnagramEqualityComparer : IEqualityComparer<string>
        {
            public bool Equals(string x, string y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (x == null | y == null) return false;

                int length = x.Length;
                if (length != y.Length) return false;

                using (var en = x.OrderBy(i => i).GetEnumerator())
                {
                    foreach (char c in y.OrderBy(i => i))
                    {
                        en.MoveNext();
                        if (c != en.Current) return false;
                    }
                }

                return true;
            }

            public int GetHashCode(string obj)
            {
                if (obj == null) return 0;

                int hash = obj.Length;
                foreach (char c in obj)
                    hash ^= c;
                return hash;
            }
        }
    }
}
