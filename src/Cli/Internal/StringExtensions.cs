using System;

namespace Cli.Internal
{
    internal static class StringExtensions
    {
        private static readonly Func<string, string, double> _defaultDistanceAlgorithm =
            (x, y) => x.JaroWinklerDistance(y);

        public static double Distance(this string x, string y)
        {
            return _defaultDistanceAlgorithm(x, y);
        }
    }
}
