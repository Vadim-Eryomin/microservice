namespace StarshipGame;

public class IoCRegGridCollisionCheckCommand : ICommand
{
    public void Execute()
    {
        IoC.Resolve<ICommand>("IoC.Register", "Collision.Grid.Check", (object[] args) => new RunnableCommand(() =>
        {
            var grids = IoC.Resolve<GridFieldStructure[]>("Collision.Grid.Grids");
            if (grids.Length == 0)
            {
                Exception e = new Exception("No grids found");
                e.Data["Method"] = "Collision.Grid.Grids";
                throw e;
            }

            grids
                .ToList()
                .ForEach(grid => IoC.Resolve<ICommand>("Commands.Grid.Collision", args[0], grid).Execute());
        })).Execute();

        IoC.Resolve<ICommand>("IoC.Register", "Commands.Grid.Collision", (object[] args) =>
            new GridCollisionCheckCommand(
                (IMovable)args[0],
                (GridFieldStructure)args[1])).Execute();
    }
}
