namespace StarshipGame;

using Hwdtech;

public class EventLoopCommand : ICommand
{
    public void Execute()
    {
        IoC.Resolve<ICommand>("Queue.Take").Execute();
    }
}
