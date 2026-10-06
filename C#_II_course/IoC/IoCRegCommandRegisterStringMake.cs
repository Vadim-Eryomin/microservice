using Scriban;

namespace StarshipGame;

public class IoCRegCommandRegisterStringMake : ICommand
{
    public void Execute()
    {
        IoC.Resolve<ICommand>("IoC.Register", "Adapters.Command.MakeLine", (object[] args) =>
        {
            if (args[0] is not string commandName || string.IsNullOrWhiteSpace(commandName))
                throw new ArgumentException("Invalid Command Name");

            if (args[1] is not string className || string.IsNullOrWhiteSpace(className))
                throw new ArgumentException("Invalid Class Name");

            var template = @"
using System;
using System.Collections.Generic;

public class {{command}}Factory : ICommand
{
    public void Execute()
    {
        IoC.Resolve<ICommand>(""IoC.Register"", ""Commands.{{command}}"",
            (object[] args) => new {{command}}(
                IoC.Resolve<I{{cls}}>(""Adapters.{{cls}}"", args[0])
            )
        ).Execute();
    }
}";

            return Template.ParseLiquid(template)
                .Render(new { command = commandName, cls = className });
        }).Execute();
    }
}
