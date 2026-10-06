using Hwdtech;

namespace StarshipGame;

public class CreateMacroCommandStrategy
{
    private string spec;

    public CreateMacroCommandStrategy(string commandSpec)
    {
        spec = commandSpec;
    }

    public Hwdtech.ICommand Resolve(object[] args)
    {
        string[] commandsNames = IoC.Resolve<string[]>(spec);
        Hwdtech.ICommand[] cmds = commandsNames.Select(name => IoC.Resolve<Hwdtech.ICommand>(name, args)).ToArray();
        return IoC.Resolve<Hwdtech.ICommand>("Commands.Macro", cmds);
    }
}
