namespace StarshipGame;
using System;
using System.Collections.Generic;

public class RegisterIoCDependencyCreateTorpedo : ICommand
{
    public void Execute()
    {
        IoC.Resolve<ICommand>("IoC.Register", "Commands.Create.Torpedo", (object[] args) =>
        {
            Dictionary<string, object> Torpedo = new Dictionary<string, object>
            {
                { "Position", (Vector)args[0] },
                { "Velocity", (Vector)args[1] },
                { "ObjectId", args[2] },
                { "PlayerId", args[3] }
            };
            return Torpedo;
        }).Execute();
    }
}
