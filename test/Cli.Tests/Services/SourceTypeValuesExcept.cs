using System;
using System.Linq;
using Cli.Services;
using Xunit;

namespace Cli.Tests.Services
{
    public class SourceTypeValuesExcept : TheoryData<SourceType>
    {
        public SourceTypeValuesExcept(params SourceType[] types)
        {
            var values = Enum.GetValues<SourceType>()
                .Except(types)
                .Concat(new[] { (SourceType)69 });

            foreach (var value in values) Add(value);
        }
    }
}
