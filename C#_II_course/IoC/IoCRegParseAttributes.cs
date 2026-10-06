namespace StarshipGame;

using System.Reflection;

public class IoCRegParseAttributes : ICommand
{
    public void Execute()
    {
        var attributesDictionary = AppDomain.CurrentDomain
            .GetAssemblies()
            .Where(assembly =>
                !assembly.IsDynamic &&
                !string.IsNullOrWhiteSpace(assembly.Location) &&
                assembly.ExportedTypes.Any())
            .SelectMany(assembly => assembly.ExportedTypes)
            .SelectMany(type => type.GetMethods(
                BindingFlags.Public |
                BindingFlags.NonPublic |
                BindingFlags.Instance |
                BindingFlags.Static)
                .Select(method => new { Type = type, Method = method }))
            .SelectMany(m => m.Method
                .GetCustomAttributes(typeof(CustomMethodAttribute), inherit: false)
                .Cast<CustomMethodAttribute>()
                .Select(attr => new
                {
                    m.Type,
                    m.Method,
                    Attr = attr
                }))
            .Where(x => x.Attr != null)
            .GroupBy(x => (x.Attr.Name, x.Attr.IsGet))
            .ToDictionary(
                g => g.Key,
                g => $"{g.First().Type.Name}.{g.First().Method.Name}"
            );

        IoC.Resolve<ICommand>(
            "IoC.Register",
            "Commands.ParseAttributes",
            (object[] _) => attributesDictionary
        ).Execute();
    }
}
