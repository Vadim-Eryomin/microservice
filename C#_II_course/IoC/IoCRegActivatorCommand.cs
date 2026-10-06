namespace StarshipGame;

public class RegisterIoCDependencyActivatorCommand : ICommand
{
    public void Execute()
    {
        IoC.Resolve<ICommand>(
            "IoC.Register",
            "Commands.ActivateCommand",
            (object[] args) => new ActivateCommand((string)args[0])
        ).Execute();
    }
}
