namespace StarshipGame;

public class RegisterIoCDependencyShootStrategy : ICommand
{
    public void Execute()
    {
        IoC.Resolve<ICommand>("IoC.Register", "Commands.ShootStrategy", (object[] args) =>
        {
            IObjectInfo playerInfo = (IObjectInfo)args[0];
            IObjectInfo shooterInfo = (IObjectInfo)args[1];
            ICommand authCommand = IoC.Resolve<ICommand>("Commands.Auth", playerInfo);
            ICommand createProjectileCommand = IoC.Resolve<ICommand>("Commands.CreateProjectile", shooterInfo);
            ICommand macroCommand = IoC.Resolve<ICommand>("Commands.Macro", new object[] { authCommand, createProjectileCommand });
            return IoC.Resolve<ICommand>("Game.EnqueueCommand", macroCommand);
        }).Execute();
    }
}
