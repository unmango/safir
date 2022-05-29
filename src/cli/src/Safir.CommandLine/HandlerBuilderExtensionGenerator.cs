using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Safir.CommandLine;

[Generator]
internal class HandlerBuilderExtensionGenerator : ISourceGenerator
{
    public void Initialize(GeneratorInitializationContext context)
    {
        context.RegisterForSyntaxNotifications(() => new AttributeReceiver());
    }

    public void Execute(GeneratorExecutionContext context)
    {
        var receiver = (AttributeReceiver)context.SyntaxReceiver!;

        var i = 0;
        foreach (var methodDeclaration in receiver.HandlerMethods) {
            context.AddSource($"found{i++}.g.cs", "ERROR");
        }
    }

    private class AttributeReceiver : ISyntaxReceiver
    {
        public List<MethodDeclarationSyntax> HandlerMethods { get; } = new();

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            // if (syntaxNode is not MethodDeclarationSyntax methodDeclaration)
            //     return;

            // if (methodDeclaration.AttributeLists.Any(x => x.Attributes.Any(MatchesName))) {
                HandlerMethods.Add(null!);
            // }
        }

        private static bool MatchesName(AttributeSyntax attribute)
            => $"{attribute.Name}Attribute" == nameof(CommandHandlerAttribute);
    }
}
