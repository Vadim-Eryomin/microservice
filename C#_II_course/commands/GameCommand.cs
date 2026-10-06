using Hwdtech.Ioc;

namespace StarshipGame;

public class GameCommand(object scope) : ICommand
{
    public void Execute()
    {
        IoC.Resolve<ICommand>("Scopes.Current.Set", scope).Execute();
        while (IoC.Resolve<bool>("Game.CanContinue"))
        {
            IoC.Resolve<ICommand>("Game.EventLoop").Execute();
        }
    }
}
