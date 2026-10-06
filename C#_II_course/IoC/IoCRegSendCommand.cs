using Hwdtech;
using Hwdtech.Ioc;

namespace StarshipGame;

public class RegisterIoCDependencySendCommand : Hwdtech.ICommand
{
    public void Execute()
    {
        IoC.Resolve<Hwdtech.ICommand>(
        "IoC.Register",
        "Commands.Send",
        (object[] args) => new SendCommand((Hwdtech.ICommand)args[0], (IMessageReceiver)args[1]))
        .Execute();
    }
}
