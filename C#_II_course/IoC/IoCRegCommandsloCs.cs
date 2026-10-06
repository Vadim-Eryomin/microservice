using System.Reflection;
using Hwdtech;

namespace StarshipGame;

public class RegisterIoCDependencyCommandsReg : Hwdtech.ICommand
{
    public void Execute()
    {
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "CommandsRegister",
            (object[] args) =>
            {
                List<Type> commands = (List<Type>)args[0];

                commands.Select(cmdType =>
                        {
                            string commandName = cmdType.Name;
                            string className = cmdType.GetInterfaces()[0].Name.Substring(1);
                            return IoC.Resolve<string>("Adapters.Command.MakeLine", commandName, className);
                        })
                        .Select(code => IoC.Resolve<Hwdtech.ICommand>("Commands.ActivateCommand", code))
                        .ToList()
                        .ForEach(cmd => cmd.Execute());

                return (object)null;
            }).Execute();
    }
}
