namespace StarshipGame;

public class RunnableCommand(Action act) : ICommand
{
    public void Execute()
    {
        act();
    }
}
