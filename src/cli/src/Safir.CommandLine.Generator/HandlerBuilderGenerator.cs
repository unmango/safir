using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Safir.CommandLine;

[Generator]
internal class HandlerBuilderGenerator : ISourceGenerator
{
    public void Initialize(GeneratorInitializationContext context)
    {
        context.RegisterForSyntaxNotifications(() => new AttributeReceiver());
    }

    public void Execute(GeneratorExecutionContext context)
    {
        var receiver = (AttributeReceiver)context.SyntaxReceiver!;

        if (receiver.HandlerMethods.Count <= 0) return;
    }

    private class AttributeReceiver : ISyntaxReceiver
    {
        public List<MethodDeclarationSyntax> HandlerMethods { get; } = new();

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            if (syntaxNode is not MethodDeclarationSyntax methodDeclaration)
                return;

            if (methodDeclaration.AttributeLists.Any(x => x.Attributes.Any(MatchesName)))
                HandlerMethods.Add(methodDeclaration);
        }

        private static bool MatchesName(AttributeSyntax attribute)
            => $"{attribute.Name}Attribute" == nameof(CommandHandlerAttribute);
    }
}
