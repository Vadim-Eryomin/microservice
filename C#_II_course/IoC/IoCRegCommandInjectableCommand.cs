using Hwdtech;

namespace StarshipGame;

public class RegisterCommandInjectableCommand : Hwdtech.ICommand
{
    public void Execute()
    {
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.CommandInjectable",
            (object[] args) => new CommandInjectableCommand()).Execute();
    }
}
