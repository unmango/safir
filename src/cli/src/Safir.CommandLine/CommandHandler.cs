using System;
using System.CommandLine.Invocation;
using System.Threading.Tasks;

namespace Safir.CommandLine;

public delegate ValueTask<int> CommandHandler(InvocationContext context, IServiceProvider services);
