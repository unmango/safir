namespace Safir.CommandLine;

public delegate ValueTask<int> CommandHandler(HandlerContext context);
