namespace StarshipGame;

using Hwdtech;
using System.Reflection;

public class IoCRegCollectInterfaces : ICommand
{
    public void Execute()
    {
        IoC.Resolve<ICommand>(
            "IoC.Register",
            "Commands.CollectInterfaces",
            (object[] args) =>
            {
                var commandTypes = args.FirstOrDefault() as IEnumerable<Type> ?? Enumerable.Empty<Type>();

                return commandTypes
                    .Select(type => type.GetConstructors()[0].GetParameters()[0].ParameterType)
                    .Distinct()
                    .ToList();
            }
        ).Execute();
    }
}
