using System.Reflection;

namespace StarshipGame;

public class RegisterIoCDependencyFactoryReg : ICommand
{
    public void Execute()
    {
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "FactoriesRegister",
            (object[] args) =>
            {
                List<Type> interfaces = (List<Type>)args[0];

                var adapterStrings = interfaces.Select((Type i, int index) =>
                {
                    var fields = i.GetProperties().Select((PropertyInfo pi, int piIndex) =>
                    {
                        var defaultGetter = IoC.Resolve<bool>("Adapter.IsComputed", pi.Name, true);
                        var defaultSetter = IoC.Resolve<bool>("Adapter.IsComputed", pi.Name, false);

                        return new Field(
                            pi.PropertyType.Name,
                            pi.Name,
                            defaultGetter,
                            defaultSetter,
                            defaultGetter ? null : IoC.Resolve<string>("Adapter.ComputeMethod", pi.Name, true),
                            defaultSetter ? null : IoC.Resolve<string>("Adapter.ComputeMethod", pi.Name, false),
                            pi.CanWrite
                        );
                    }).ToArray();

                    return IoC.Resolve<string>("Adapters.CreateAdapter", i.Name, fields);
                }).ToArray();

                adapterStrings
                    .Select<string, object>((string adapterString, int index) =>
                    {
                        IoC.Resolve<Hwdtech.ICommand>("Commands.ActivateCommand", adapterString).Execute();
                        return (object)null;
                    })
                    .ToArray();

                interfaces
                    .Select<Type, object>((Type interfaceType, int index) =>
                    {
                        var factoryCommand = IoC.Resolve<Hwdtech.ICommand>("Factories.Create", interfaceType);
                        factoryCommand.Execute();
                        return (object)null;
                    })
                    .ToArray();

                return (object)null;
            }
        ).Execute();
    }
}
