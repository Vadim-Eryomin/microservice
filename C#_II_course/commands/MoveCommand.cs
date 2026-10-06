namespace StarshipGame;
public class MoveCommand : ICommand
{
    private IMovable _Obj;

    public MoveCommand(IMovable obj) => _Obj = obj;

    public void Execute() => _Obj.Position += _Obj.Velocity;
}
