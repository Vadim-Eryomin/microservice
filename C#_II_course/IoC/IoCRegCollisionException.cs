namespace StarshipGame;

public class RegisterIoCDependencyCollisionException : ICommand
{
    public void Execute()
    {
        IoC.Resolve<ICommand>("IoC.Register", "Exceptions.Collision",
        (object[] args) => new CollisionDetectedException((ICollisionObject)args[0], (ICollisionObject)args[1])).Execute();
    }
}
