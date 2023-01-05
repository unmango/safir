using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Safir.Protos.Generator;

internal sealed record GrpcClient(string Name);

internal sealed class GrpcClientSyntaxReceiver : ISyntaxReceiver
{
    public List<GrpcClient> Clients { get; } = new();

    public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
    {
        if (syntaxNode is not ClassDeclarationSyntax classDeclaration)
            return;

        // TODO: Something with inspecting the base types for `grpc::ClientBase<T>`
        // if (classDeclaration.BaseList.Types.Any(x => x.Type.))

        throw new NotImplementedException();
    }
}
