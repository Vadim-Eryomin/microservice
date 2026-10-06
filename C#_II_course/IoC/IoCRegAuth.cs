namespace StarshipGame;
using Hwdtech;

public class RegisterAuthCommand : ICommand
{
    public void Execute()
    {
        IoC.Resolve<ICommand>(
            "IoC.Register",
            "Commands.Auth",
            (object[] args) => new AuthCommand(
                (IObjectInfo)args[0],
                (IObjectInfo)args[1]
            )
        ).Execute();
    }
}
