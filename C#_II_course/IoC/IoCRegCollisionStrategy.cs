namespace StarshipGame;
using CollisionTree = Dictionary<int, Dictionary<int, Dictionary<int, HashSet<int>>>>;

public class RegisterIoCDependencyCollisionStrategy : ICommand
{
    public void Execute()
    {
        IoC.Resolve<ICommand>("IoC.Register", "Collision.Strategy", (object[] args) =>
        {
            var collisionTree = IoC.Resolve<CollisionTree>("Game.Collision.Tree");

            var obj1 = (ICollisionObject)args[0];
            var obj2 = (ICollisionObject)args[1];

            var deltaPosX = obj1.Position.GetCoordinate(0) - obj2.Position.GetCoordinate(0);
            var deltaPosY = obj1.Position.GetCoordinate(1) - obj2.Position.GetCoordinate(1);
            var deltaVelX = obj1.Velocity.GetCoordinate(0) - obj2.Velocity.GetCoordinate(0);
            var deltaVelY = obj1.Velocity.GetCoordinate(1) - obj2.Velocity.GetCoordinate(1);

            var result = (object)(collisionTree.TryGetValue(deltaPosX, out var dyDict)
                && dyDict.TryGetValue(deltaPosY, out var dvxDict)
                && dvxDict.TryGetValue(deltaVelX, out var dvySet)
                && dvySet.Contains(deltaVelY));

            return result;
        }).Execute();
    }
}
