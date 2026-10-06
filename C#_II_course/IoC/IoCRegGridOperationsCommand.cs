namespace StarshipGame;

public class IoCRegGridOperationsCommand : ICommand
{
    public void Execute()
    {
        var grids = new List<GridFieldStructure>();

        IoC.Resolve<ICommand>("IoC.Register", "GridField", (object[] args) => new GridFieldStructure((int)args[0]))
            .Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Collision.Grid.Grids", (object[] args) => grids.ToArray()).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Collision.Grid.Grids.Add",
            (object[] args) => new RunnableCommand(() => { grids.Add((GridFieldStructure)args[0]); })).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Collision.Grid.AddObject",
                (object[] args) => new RunnableCommand(() => { grids.ForEach(gr => gr.Add((IMovable)args[0])); }))
            .Execute();
    }
}