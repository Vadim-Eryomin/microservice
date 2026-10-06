using Hwdtech;
namespace StarshipGame;

public class RegisterIoCDependencyMoveCommand : ICommand
{
    public void Execute()
    {
        IoC.Resolve<ICommand>("IoC.Register", "Commands.Move", (object[] args) => (new MoveCommand(IoC.Resolve<IMovable>("Adapters.IMovingObject", args[0])))).Execute();
    }
}
