using Microsoft.CodeAnalysis;

namespace Safir.Protos.Generator;

public class ClientInterfaceGenerator : ISourceGenerator
{
    public void Initialize(GeneratorInitializationContext context)
    {
        context.RegisterForSyntaxNotifications(() => new GrpcClientSyntaxReceiver());
    }

    public void Execute(GeneratorExecutionContext context)
    {
        if (context.SyntaxReceiver is not GrpcClientSyntaxReceiver receiver)
            return;

        throw new NotImplementedException();
    }
}
