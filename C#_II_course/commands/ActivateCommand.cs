namespace StarshipGame;

public class ActivateCommand : ICommand
{
    private string _Code;

    public ActivateCommand(string code) => _Code = code;

    public void Execute()
    {
        DynamicCommandActivator.Activate(_Code);
    }
}
