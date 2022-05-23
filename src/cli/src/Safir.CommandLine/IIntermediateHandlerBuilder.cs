using System;
using System.CommandLine.Invocation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Safir.CommandLine;

public interface IIntermediateHandlerBuilder : IHandlerBuilder<IIntermediateHandlerBuilder>
{
}
