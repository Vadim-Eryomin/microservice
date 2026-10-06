using System.Linq;
using Hwdtech;

namespace StarshipGame;

public class RegisterIoCDependencyMacroMoveRotate : Hwdtech.ICommand
{
    public void Execute()
    {
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Macro.Move",
            (object[] args) =>
                IoC.Resolve<Hwdtech.ICommand>("Commands.Macro", IoC.Resolve<string[]>("Specs.Move").ToList()
                    .Select(commandName => IoC.Resolve<Hwdtech.ICommand>(commandName, args)).ToArray())
        ).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Macro.Rotate",
            (object[] args) =>
                IoC.Resolve<Hwdtech.ICommand>("Commands.Macro", IoC.Resolve<string[]>("Specs.Rotate").ToList()
                    .Select(commandName => IoC.Resolve<Hwdtech.ICommand>(commandName, args)).ToArray())
        ).Execute();
    }
}
