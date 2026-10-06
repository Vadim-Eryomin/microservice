namespace StarshipGame;

public interface ICollisionObject
{
    public object Form { get; }
    public Vector Position { get; }
    public Vector Velocity { get; }
}
