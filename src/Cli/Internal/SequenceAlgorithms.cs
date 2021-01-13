using System;
using System.Collections.Generic;
using System.Linq;

namespace Cli.Internal
{
    internal static class SequenceAlgorithms
    {
        public static double JaroDistance<T>(
            this IEnumerable<T> source,
            IEnumerable<T> compare,
            IEqualityComparer<T>? equalityComparer = null)
        {
            equalityComparer ??= EqualityComparer<T>.Default;
            var sourceList = source.ToList();
            var compareList = compare.ToList();
            var sLen = sourceList.Count;
            var cLen = compareList.Count;

            if (sLen == 0 && cLen == 0) return 1;

            var matchDistance = Math.Max(sLen, cLen) / 2 - 1;

            var sMatches = new bool[sLen];
            var cMatches = new bool[cLen];

            var matches = 0;
            var transpositions = 0;

            for (int i = 0; i < sLen; i++)
            {
                var start = Math.Max(0, i - matchDistance);
                var end = Math.Min(i + matchDistance, cLen);

                for (int j = start; j < end; j++)
                {
                    if (cMatches[j]) continue;
                    if (!equalityComparer.Equals(sourceList[i], compareList[j])) continue;
                    sMatches[i] = true;
                    cMatches[j] = true;
                    matches++;
                    break;
                }
            }

            if (matches == 0) return 0;

            var k = 0;
            for (int i = 0; i < sLen; i++)
            {
                if (!sMatches[i]) continue;
                while (!cMatches[k]) k++;
                if (!equalityComparer.Equals(sourceList[i], compareList[k])) transpositions++;
                k++;
            }

            return ((double)matches / sLen +
                    (double)matches / cLen +
                    (matches - transpositions / 2.0) / matches) / 3.0;
        }

        public static double JaroWinklerSimilarity<T>(
            this IEnumerable<T> source,
            IEnumerable<T> compare,
            int commonPrefixLength = 4,
            double scalingFactor = 0.1,
            IEqualityComparer<T>? equalityComparer = null)
        {
            // TODO: Better requirements based on https://en.wikipedia.org/wiki/Jaro%E2%80%93Winkler_distance
            if (commonPrefixLength > 4) throw new InvalidOperationException("Prefix length should not exceed 4");
            if (scalingFactor > 0.25) throw new InvalidOperationException("Scaling factor should not exceed 0.25");

            var jaro = source.JaroDistance(compare, equalityComparer);
            return jaro + commonPrefixLength * scalingFactor * (1 - jaro);
        }

        public static double JaroWinklerDistance<T>(
            this IEnumerable<T> source,
            IEnumerable<T> compare,
            int commonPrefixLength = 4,
            double scalingFactor = 0.1,
            IEqualityComparer<T>? equalityComparer = null)
        {
            var similarity = source.JaroWinklerSimilarity(compare, commonPrefixLength, scalingFactor, equalityComparer);
            return 1 - similarity;
        }
    }
}
