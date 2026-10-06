namespace StarshipGame;

public class IoCRegGameInitializationCommand : ICommand
{
    public void Execute()
    {
        IoC.Resolve<ICommand>("IoC.Register", "Game.Initialize",
            (object[] args) =>
            {
                var gameScope = IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"));
                IoC.Resolve<ICommand>("Scopes.Current.Set", gameScope).Execute();

                IoC.Resolve<ICommand>("IoC.Register", "Game.CanContinue",
                    (object[] ar) => (object)(((Func<bool>)args[0])())).Execute();

                IoC.Resolve<ICommand>("IoC.Register", "Game.EventLoop",
                    (object[] ar) => (ICommand)args[1]).Execute();

                IoC.Resolve<ICommand>("IoC.Register", "Queue.Take",
                    (object[] ar) => ((Func<ICommand>)args[2])()).Execute();

                IoC.Resolve<ICommand>("IoC.Register", "Queue.Push",
                    (object[] ar) => ((Func<ICommand, ICommand>)args[3])((ICommand)ar[0])).Execute();

                var game = new GameCommand(gameScope);
                IoC.Resolve<ICommand>("IoC.Register", "Game", (object[] args) => game).Execute();

                return game;
            }).Execute();
    }
}
