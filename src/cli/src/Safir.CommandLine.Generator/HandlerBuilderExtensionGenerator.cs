using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Safir.CommandLine;

// [Generator]
internal class HandlerBuilderExtensionGenerator : ISourceGenerator
{
    private const string UsingFormat = @"using {0};";

    private const string HeaderFormat = @"// Auto-generated code

using Safir.CommandLine;
{0}

namespace Safir.CommandLine
{{
    internal static class GeneratedHandlerBuilderExtensions
    {{";

    private const string MethodFormat = @"
        public static IHandlerBuilder ConfigureHandler<T>(this IHandlerBuilder builder)
            {0}
        {{
            builder.ConfigureHandler<T>({1});
            return builder;
        }}";

    private const string Footer = @"
    }
}";

    private const string ConstraintFormat = "where T : {0}";

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
        var receiver = (AttributeReceiver)context.SyntaxReceiver!;

        if (receiver.HandlerMethods.Count <= 0) return;

        var methodSources = new List<string>();
        var usingSources = new HashSet<string>();
        var asyncConstraints = new HashSet<string>();
        var syncConstraints = new HashSet<string>();

        foreach (var methodDeclaration in receiver.HandlerMethods) {
            var classes = methodDeclaration.Ancestors()
                .OfType<ClassDeclarationSyntax>()
                .Reverse()
                .ToList();

            if (classes.Count <= 0) continue;

            var namespaceName = methodDeclaration.Ancestors()
                .OfType<NamespaceDeclarationSyntax>()
                .Select(x => x.Name.ToString())
                .FirstOrDefault();

            if (namespaceName is null) continue;

            var className = string.Join('.', classes.Select(x => x.Identifier.Text));
            var methodName = methodDeclaration.Identifier.Text;

            var returnType = methodDeclaration.ReturnType.ToString();
            string? delegateFormat = null;
            switch (returnType) {
                case "Task<int>":
                case "Task":
                    delegateFormat = AsyncDelegateFormat;
                    asyncConstraints.Add(string.Format(ConstraintFormat, className));
                    break;
                case "int":
                case "void":
                    delegateFormat = SyncDelegateFormat;
                    syncConstraints.Add(string.Format(ConstraintFormat, className));
                    break;
            }

            if (delegateFormat is null) continue;

            var delegateSource = string.Format(delegateFormat, methodName);
            var methodSource = string.Format(MethodFormat, className, delegateSource);

            usingSources.Add(string.Format(UsingFormat, namespaceName));
            methodSources.Add(methodSource);
        }

        var usings = string.Join('\n', usingSources);
        var header = string.Format(HeaderFormat, usings);
        var source = new StringBuilder(header);
        source.Append(string.Join('\n', methodSources));
        source.Append(Footer);

        context.AddSource("HandlerBuilderExtensions.g.cs", source.ToString());
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
