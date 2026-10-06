namespace StarshipGame;

public class GridFieldStructure
{
    private IDictionary<int, IDictionary<int, List<IMovable>>> _fields =
        new Dictionary<int, IDictionary<int, List<IMovable>>>();

    private int _offset;
    private int _gridSize;

    public GridFieldStructure(int offset)
    {
        _gridSize = IoC.Resolve<int>("Rules.GridWidth") * 3;
        _offset = offset;
    }

    public void Add(IMovable movable)
    {
        int x = (movable.Position.GetCoordinate(0) + _offset) / _gridSize;
        int y = (movable.Position.GetCoordinate(1) + _offset) / _gridSize;

        if (!_fields.ContainsKey(x)) _fields[x] = new Dictionary<int, List<IMovable>>();
        if (!_fields[x].ContainsKey(y)) _fields[x][y] = new List<IMovable>();

        _fields[x][y].Add(movable);
    }

    public List<IMovable> Get(Vector position)
    {
        int x = (position.GetCoordinate(0) + _offset) / _gridSize;
        int y = (position.GetCoordinate(1) + _offset) / _gridSize;

        return _fields[x][y];
    }

    public void Remove(IMovable movable)
    {
        RemoveByPosition(movable.Position, movable);
    }

    public void RecountGridPosition(IMovable movable)
    {
        Vector prevPosition = movable.Position + movable.Velocity.Scale(-1);
        RemoveByPosition(prevPosition, movable);
        Add(movable);
    }

    private void RemoveByPosition(Vector position, IMovable movable)
    {
        int x = (position.GetCoordinate(0) + _offset) / _gridSize;
        int y = (position.GetCoordinate(1) + _offset) / _gridSize;

        if (!_fields.ContainsKey(x)) return;
        if (!_fields[x].ContainsKey(y)) return;

        _fields[x][y].Remove(movable);
    }
}
