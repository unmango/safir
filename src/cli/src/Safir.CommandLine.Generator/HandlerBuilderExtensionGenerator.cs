using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Safir.CommandLine.Generator;

[Generator]
internal class HandlerBuilderExtensionGenerator : ISourceGenerator
{
    private const string HandlerExtensionFormat = @"// <auto-generated>
//   Generated by Safir.CommandLine.Generator
// </auto-generated>

using Safir.CommandLine;

namespace {0}
{{
    internal static class Generated{1}HandlerBuilderExtensions
    {{
        /// <summary>
        /// Generated extension to configure <paramref name=""builder"" />
        /// to use <see cref=""{2}.{3}"" /> as the handler.
        /// </summary>
        /// <param name=""builder"">The <see cref=""IHandlerBuilder"" /> to configure.</param>
        /// <returns>The original <see cref=""IHandlerBuilder"" /> so calls can be chained.</returns>
        public static IHandlerBuilder Use{4}(this IHandlerBuilder builder)
        {{
            builder.ConfigureHandler<{5}>(
                {6});
            return builder;
        }}
    }}
}}";

    private const string AsyncDelegateFormat =
        "(handler, parseResult, cancellationToken) => handler.{0}(parseResult, cancellationToken)";

    private const string SyncDelegateFormat =
        "(handler, parseResult) => handler.{0}(parseResult)";

    public void Initialize(GeneratorInitializationContext context)
    {
        context.RegisterForSyntaxNotifications(() => new AttributeReceiver());
    }

    public void Execute(GeneratorExecutionContext context)
    {
        if (context.SyntaxReceiver is not AttributeReceiver receiver)
            return;

        if (receiver.HandlerMethods.Count <= 0) return;

        foreach (var methodDeclaration in receiver.HandlerMethods) {
            var ancestors = methodDeclaration.Ancestors().ToList();
            var classes = ancestors
                .OfType<ClassDeclarationSyntax>()
                .Reverse()
                .ToList();

            if (classes.Count <= 0) continue;

            var namespaceName = ancestors
                .OfType<BaseNamespaceDeclarationSyntax>()
                .Select(x => x.Name.ToString())
                .FirstOrDefault();

            if (namespaceName is null) continue;

            var allClassNames = classes.Select(x => x.Identifier.Text).ToList();
            var displayClassName = string.Join(string.Empty, allClassNames);
            var className = string.Join(".", allClassNames);
            var methodName = methodDeclaration.Identifier.Text;

            var returnType = methodDeclaration.ReturnType.ToString();
            var delegateFormat = returnType switch {
                "Task<int>" => AsyncDelegateFormat,
                "Task" => AsyncDelegateFormat,
                "int" => SyncDelegateFormat,
                "void" => SyncDelegateFormat,
                _ => null,
            };

            if (delegateFormat is null) continue;

            var delegateSource = string.Format(delegateFormat, methodName);

            var source = string.Format(
                HandlerExtensionFormat,
                namespaceName,
                displayClassName,
                className,
                methodName,
                displayClassName,
                className,
                delegateSource);

            context.AddSource($"{displayClassName}HandlerBuilderExtensions.g.cs", SourceText.From(source, Encoding.UTF8));
        }
    }

    private class AttributeReceiver : ISyntaxReceiver
    {
        public List<MethodDeclarationSyntax> HandlerMethods { get; } = new();

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            if (syntaxNode is not MethodDeclarationSyntax methodDeclaration)
                return;

            if (methodDeclaration.AttributeLists.Any(x => x.Attributes.Any(MatchesName))) {
                HandlerMethods.Add(methodDeclaration);
            }
        }

        private static bool MatchesName(AttributeSyntax attribute)
            => $"{attribute.Name}Attribute" == nameof(CommandHandlerAttribute);
    }
}
