namespace StarshipGame;
using Hwdtech;
public class AuthCommand : ICommand
{
    private readonly IObjectInfo _playerInfo;
    private readonly IObjectInfo _targetObjectInfo;

    public AuthCommand(IObjectInfo playerInfo, IObjectInfo targetObjectInfo)
    {
        _playerInfo = playerInfo;
        _targetObjectInfo = targetObjectInfo;
    }

    public void Execute()
    {
        var allowedObjects = IoC.Resolve<IEnumerable<IObjectInfo>>(
            "GameItem.GetByPlayerId",
            _playerInfo.PlayerId
        );

        if (!allowedObjects.Any(o => o.ObjectId == _targetObjectInfo.ObjectId))
        {
            throw new UnauthorizedAccessException(
                $"Игрок [ID:{_playerInfo.PlayerId}] " +
                $"не может взаимодействовать с объектом [ID:{_targetObjectInfo.ObjectId}]"
            );
        }
    }
}
