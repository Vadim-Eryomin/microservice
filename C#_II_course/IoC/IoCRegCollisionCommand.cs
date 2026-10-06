namespace StarshipGame;

public class RegisterIoCDependencyCollisionCheckCommand : ICommand
{
    public void Execute()
    {
        IoC.Resolve<ICommand>(
            "IoC.Register",
            "Commands.CollisionCheck",
            (object[] args) => new CollisionCheckCommand((ICollisionObject)args[0], (ICollisionObject)args[1])
        ).Execute();
    }
}
