namespace StarshipGame;
using Hwdtech.Ioc;
using System;

public class CollisionCheckCommand : ICommand
{
    private readonly ICollisionObject _Obj1;
    private readonly ICollisionObject _Obj2;

    public CollisionCheckCommand(ICollisionObject obj1, ICollisionObject obj2)
    {
        _Obj1 = obj1 ?? throw new ArgumentNullException(nameof(obj1));
        _Obj2 = obj2 ?? throw new ArgumentNullException(nameof(obj2));
    }

    public void Execute()
    {
        var collisionCriteria = (bool)IoC.Resolve<object>("Collision.Strategy", _Obj1, _Obj2);

        if (collisionCriteria)
        {
            throw IoC.Resolve<Exception>("Exceptions.Collision");
        }
    }
}
