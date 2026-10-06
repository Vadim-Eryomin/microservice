using Hwdtech;
using Hwdtech.Ioc;

namespace StarshipGame;

public class RegisterIoCDependencyRotateCommand : ICommand
{
    public void Execute()
    {

        IoC.Resolve<ICommand>
        (
            "IoC.Register",
            "Commands.Rotate",
            (object[] args) => (new RotateCommand(IoC.Resolve<IRotatable>("Adapters.IRotatable", args[0])))).Execute();
    }
}
