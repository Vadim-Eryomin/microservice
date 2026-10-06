namespace StarshipGame;

public class IoCRegAdapterIsComputed : ICommand
{
    public void Execute()
    {
        IoC.Resolve<ICommand>(
            "IoC.Register",
            "Adapter.IsComputed",
            (object[] args) =>
            {
                var fieldName = (string)args[0];
                var isGet = (bool)args[1];

                var attributeMethods = IoC.Resolve<Dictionary<(string, bool), string>>("Commands.ParseAttributes");
                return (object)attributeMethods.ContainsKey((fieldName, isGet));
            }
        ).Execute();
    }
}
