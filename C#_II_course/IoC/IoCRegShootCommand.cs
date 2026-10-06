namespace StarshipGame;

public class RegisterIoCDependencyShootCommand : ICommand
{
    public void Execute()
    {
        IoC.Resolve<ICommand>(
            "IoC.Register",
            "Commands.Shoot",
            (object[] args) => new ShootCommand((IObjectInfo)args[0], (IObjectInfo)args[1])
        ).Execute();
    }
}
