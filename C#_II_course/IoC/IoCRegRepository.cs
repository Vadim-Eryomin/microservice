namespace StarshipGame;

public class IoCRegRepository : ICommand
{
    Dictionary<object, IObjectInfo> _objectsByObjectIds = new Dictionary<object, IObjectInfo>();
    Dictionary<object, HashSet<IObjectInfo>> _objectsByPlayerIds = new Dictionary<object, HashSet<IObjectInfo>>();


    public void Execute()
    {
        IoC.Resolve<ICommand>("IoC.Register", "GameItem.AddByObjectId", (object[] args) => new RunnableCommand(
            () => _objectsByObjectIds.Add(args[0], (IObjectInfo)args[1]))).Execute();

        IoC.Resolve<ICommand>("IoC.Register", "GameItem.AddByPlayerId", (object[] args) => new RunnableCommand(
            // 0 - playerId, 1 - object
            () =>
            {
                if (!_objectsByPlayerIds.ContainsKey(args[0]))
                    _objectsByPlayerIds.Add(args[0], new HashSet<IObjectInfo>());
                _objectsByPlayerIds[args[0]].Add((IObjectInfo)args[1]);
            })).Execute();

        IoC.Resolve<ICommand>("IoC.Register", "GameItem.Add", (object[] args) => new RunnableCommand(
            () =>
            {
                IoC.Resolve<ICommand>("GameItem.AddByObjectId", ((IObjectInfo)args[0]).ObjectId, (IObjectInfo)args[0])
                    .Execute();
                IoC.Resolve<ICommand>("GameItem.AddByPlayerId", ((IObjectInfo)args[0]).PlayerId, (IObjectInfo)args[0])
                    .Execute();
            })).Execute();

        IoC.Resolve<ICommand>("IoC.Register", "GameItem.Remove", (object[] args) => new RunnableCommand(
            // 0 - object
            () =>
            {
                if (_objectsByObjectIds.ContainsKey(((IObjectInfo)args[0]).ObjectId))
                    _objectsByObjectIds.Remove(args[0]);

                if (_objectsByPlayerIds.ContainsKey(((IObjectInfo)args[0]).PlayerId) &&
                    _objectsByPlayerIds[args[0]].Contains(args[0]))
                    _objectsByPlayerIds[((IObjectInfo)args[0]).PlayerId].Remove((IObjectInfo)args[0]);
            })).Execute();

        IoC.Resolve<ICommand>("IoC.Register", "GameItem.GetByObjectId",
            (object[] args) => _objectsByObjectIds[args[0]]).Execute();

        IoC.Resolve<ICommand>("IoC.Register", "GameItem.GetByPlayerId",
            (object[] args) => _objectsByPlayerIds[args[0]].ToArray()).Execute();
    }
}
