namespace StarshipGame;

public class CollisionDetectedException : Exception
{
    public ICollisionObject Object1 { get; }
    public ICollisionObject Object2 { get; }

    public CollisionDetectedException(ICollisionObject obj1, ICollisionObject obj2)
        : base($"Collision detected between objects at positions " +
               $"({obj1.Position.GetCoordinate(0)}, {obj1.Position.GetCoordinate(1)}) and " +
               $"({obj2.Position.GetCoordinate(0)}, {obj2.Position.GetCoordinate(1)})")
    {
        Object1 = obj1;
        Object2 = obj2;
    }
}
