using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Symbols;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.FindSymbols;
using Microsoft.CodeAnalysis.MSBuild;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeAnalysisApp1
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            // Attempt to set the version of MSBuild.
            var visualStudioInstances = MSBuildLocator.QueryVisualStudioInstances().ToArray();
            var instance = visualStudioInstances.Length == 1
                // If there is only one instance of MSBuild on this machine, set that as the one to use.
                ? visualStudioInstances[0]
                // Handle selecting the version of MSBuild you want to use.
                : SelectVisualStudioInstance(visualStudioInstances);

            Console.WriteLine($"Using MSBuild at '{instance.MSBuildPath}' to load projects.");

            // NOTE: Be sure to register an instance with the MSBuildLocator 
            //       before calling MSBuildWorkspace.Create()
            //       otherwise, MSBuildWorkspace won't MEF compose.
            MSBuildLocator.RegisterInstance(instance);

            using (var workspace = MSBuildWorkspace.Create())
            {
                // Print message for WorkspaceFailed event to help diagnosing project load failures.
                workspace.WorkspaceFailed += (o, e) => Console.WriteLine(e.Diagnostic.Message);

                var solutionPath = args[0];
                Console.WriteLine($"Loading solution '{solutionPath}'");

                // Attach progress reporter so we print projects as they are loaded.
                var solution = await workspace.OpenSolutionAsync(solutionPath, new ConsoleProgressReporter());
                Console.WriteLine($"Finished loading solution '{solutionPath}'");

                var dynamicMethods = new Dictionary<string, List<string>>();

                // TODO: Do analysis on the projects in the loaded solution
                foreach (var project in solution.Projects)
                {
                    if (project.Name != "Unigram")
                    {
                        continue;
                    }

                    foreach (var document in project.Documents)
                    {
                        if (!document.Name.EndsWith("UpdateManager.cs"))
                        {
                            continue;
                        }

                        var first = true;

                        var rootNode = await document.GetSyntaxRootAsync();
                        var semanticModel = await document.GetSemanticModelAsync();

                        var methods = rootNode
                            .DescendantNodes()
                            .OfType<MethodDeclarationSyntax>();

                        foreach (var method in methods)
                        {
                            if (method.Identifier.ValueText != "Subscribe")
                            {
                                continue;
                            }

                            var methodSymbol = semanticModel.GetDeclaredSymbol(method);
                            var callers = await SymbolFinder.FindCallersAsync(methodSymbol, solution);

                            foreach (var result in callers)
                            {
                                if (result.CallingSymbol.Name == "Subscribe")
                                {
                                    continue;
                                }

                                foreach (var definition in result.CallingSymbol.DeclaringSyntaxReferences)
                                {
                                    var callerDeclarationSyntax = await definition.GetSyntaxAsync();
                                    if (callerDeclarationSyntax != null)
                                    {
                                        var callerDocument = project.GetDocument(callerDeclarationSyntax.SyntaxTree);
                                        var callerSemanticModel = await callerDocument.GetSemanticModelAsync();

                                        var invocations = callerDeclarationSyntax
                                            .DescendantNodes()
                                            .OfType<InvocationExpressionSyntax>();

                                        foreach (var invocation in invocations)
                                        {
                                            var invocationSymbol = callerSemanticModel.GetSymbolInfo(invocation);
                                            if (SymbolEqualityComparer.Default.Equals(invocationSymbol.Symbol, methodSymbol))
                                            {
                                                foreach (var argument in invocation.ArgumentList.Arguments)
                                                {
                                                    var argumentSymbol = callerSemanticModel.GetSymbolInfo(argument.Expression);
                                                    if (argumentSymbol.Symbol is IMethodSymbol sourceMethodSymbol)
                                                    {
                                                        if (sourceMethodSymbol.MethodKind == MethodKind.AnonymousFunction)
                                                        {
                                                            var receiver = sourceMethodSymbol.ReceiverType.ToDisplayString();

                                                            Console.WriteLine(receiver);

                                                            dynamicMethods[receiver] = null;
                                                        }
                                                        else if (sourceMethodSymbol.MethodKind == MethodKind.Ordinary)
                                                        {
                                                            var receiver = sourceMethodSymbol.ReceiverType.ToDisplayString();
                                                            var name = string.Format("{0}.{1}", receiver, sourceMethodSymbol.Name);

                                                            Console.WriteLine(name);

                                                            dynamicMethods.TryGetValue(receiver, out var data);
                                                            if (data == null)
                                                            {
                                                                data = dynamicMethods[receiver] = new List<string>();
                                                            }

                                                            if (!data.Contains(sourceMethodSymbol.Name))
                                                            {
                                                                data.Add(sourceMethodSymbol.Name);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        var builder = new StringBuilder();

                        foreach (var type in dynamicMethods.OrderBy(x => x.Key))
                        {
                            if (type.Value != null)
                            {
                                builder.AppendFormat("    <Type Name=\"{0}\">\n", type.Key);
                                foreach (var method in type.Value)
                                {
                                    builder.AppendFormat("      <Method Name=\"{0}\" Dynamic=\"Required\"/>\n", method);
                                }
                                builder.AppendFormat("    </Type>\n\n");
                            }
                            else
                            {
                                builder.AppendFormat("    <Type Name=\"{0}\" Dynamic=\"Required All\"/>\n\n", type.Key);
                            }
                        }

                        var a = builder.ToString();
                        Console.ReadLine();
                    }
                }
            }
        }

        private static VisualStudioInstance SelectVisualStudioInstance(VisualStudioInstance[] visualStudioInstances)
        {
            Console.WriteLine("Multiple installs of MSBuild detected please select one:");
            for (int i = 0; i < visualStudioInstances.Length; i++)
            {
                Console.WriteLine($"Instance {i + 1}");
                Console.WriteLine($"    Name: {visualStudioInstances[i].Name}");
                Console.WriteLine($"    Version: {visualStudioInstances[i].Version}");
                Console.WriteLine($"    MSBuild Path: {visualStudioInstances[i].MSBuildPath}");
            }

            while (true)
            {
                var userResponse = Console.ReadLine();
                if (int.TryParse(userResponse, out int instanceNumber) &&
                    instanceNumber > 0 &&
                    instanceNumber <= visualStudioInstances.Length)
                {
                    return visualStudioInstances[instanceNumber - 1];
                }
                Console.WriteLine("Input not accepted, try again.");
            }
        }

        private class ConsoleProgressReporter : IProgress<ProjectLoadProgress>
        {
            public void Report(ProjectLoadProgress loadProgress)
            {
                var projectDisplay = Path.GetFileName(loadProgress.FilePath);
                if (loadProgress.TargetFramework != null)
                {
                    projectDisplay += $" ({loadProgress.TargetFramework})";
                }

                Console.WriteLine($"{loadProgress.Operation,-15} {loadProgress.ElapsedTime,-15:m\\:ss\\.fffffff} {projectDisplay}");
            }
        }
    }
}
