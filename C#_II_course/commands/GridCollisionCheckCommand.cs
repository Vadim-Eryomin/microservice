namespace StarshipGame;

public class GridCollisionCheckCommand(IMovable currentObject, GridFieldStructure grid) : ICommand
{
    public void Execute()
    {
        // если не двигается, то не может столкнуться
        if (currentObject.Velocity == currentObject.Velocity.Scale(0)) return;

        // если коллизия - то будет исключение, я его не трогаю 
        grid.Get(currentObject.Position)
            .FindAll(o => o != currentObject)
            .ForEach(o => IoC.Resolve<object>("Commands.CollisionCheck", currentObject, o));
    }
}
