using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace StarshipGame;

public static class DynamicCommandActivator
{
    public static void Activate(string generatedCode)
    {
        var assembly = Compile(generatedCode);
        ExecuteCommand(assembly);
    }

    private static Assembly Compile(string code)
    {
        var syntaxTree = CSharpSyntaxTree.ParseText(code);

        var references = AppDomain.CurrentDomain
            .GetAssemblies()
            .Where(a => !a.IsDynamic && !string.IsNullOrEmpty(a.Location))
            .Select(a => MetadataReference.CreateFromFile(a.Location));

        var compilation = CSharpCompilation.Create(
            Path.GetRandomFileName(),
            new[] { syntaxTree },
            references,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
        );

        using var ms = new MemoryStream();
        var result = compilation.Emit(ms);

        if (!result.Success)
        {
            var failures = result.Diagnostics
                .Where(d => d.Severity == DiagnosticSeverity.Error);
            var message = string.Join("\n", failures.Select(f => f.ToString()));
            throw new InvalidOperationException("Compilation failed:\n" + message);
        }

        ms.Seek(0, SeekOrigin.Begin);
        return Assembly.Load(ms.ToArray());
    }

    private static void ExecuteCommand(Assembly assembly)
    {
        var commandType = assembly.GetTypes()
            .FirstOrDefault(t => typeof(ICommand).IsAssignableFrom(t));

        if (commandType == null) return;

        var commandInstance = Activator.CreateInstance(commandType) as ICommand;
        commandInstance?.Execute();
    }
}
