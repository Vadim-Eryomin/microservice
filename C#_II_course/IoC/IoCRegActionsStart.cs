namespace StarshipGame;
using Hwdtech;

public class RegisterIoCDependencyActionsStart : Hwdtech.ICommand
{
    public void Execute()
    {
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Actions.Start",
            (object[] args) => IoC.Resolve<Hwdtech.ICommand>("Commands.Macro", args[0])
        ).Execute();
    }
}
