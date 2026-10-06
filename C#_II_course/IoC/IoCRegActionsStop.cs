namespace StarshipGame;
using Hwdtech;

public class RegisterIoCDependencyActionsStop : ICommand
{
    public void Execute()
    {
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Actions.Stop",
            (object[] args) => (new EmptyCommand())).Execute();
    }
}
