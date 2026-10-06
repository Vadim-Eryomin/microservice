namespace StarshipGame;

public class ShootCommand : ICommand
{
    private readonly IObjectInfo _playerInfo;
    private readonly IObjectInfo _shooterInfo;

    public ShootCommand(IObjectInfo playerInfo, IObjectInfo shooterInfo)
    {
        _playerInfo = playerInfo;
        _shooterInfo = shooterInfo;
    }

    public void Execute()
    {
        ICommand shootStrategy = IoC.Resolve<ICommand>("Commands.ShootStrategy", _playerInfo, _shooterInfo);
        shootStrategy.Execute();
    }
}
