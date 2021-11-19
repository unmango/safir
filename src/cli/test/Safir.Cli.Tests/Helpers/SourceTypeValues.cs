using System;
using System.Collections.Generic;
using System.Linq;
using Safir.Cli.Services.Configuration;
using Xunit;

namespace Safir.Cli.Tests.Helpers
{
    public class SourceTypeValues : TheoryData<SourceType>
    {
        private static readonly IEnumerable<SourceType> _values =
            Enum.GetValues<SourceType>().Concat(new[] { (SourceType)69 });

        private SourceTypeValues(IEnumerable<SourceType>? values = null)
        {
            foreach (var value in values ?? _values) Add(value);
        }

        public static SourceTypeValues Except(params SourceType[] types)
            => new(_values.Except(types));
    }
}
