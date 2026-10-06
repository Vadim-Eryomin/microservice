using System.Linq;

namespace StarshipGame;

public class MacroCommand : Hwdtech.ICommand
{

    private List<Hwdtech.ICommand> cmds;

    public MacroCommand(List<Hwdtech.ICommand> cmds)

    {
        this.cmds = cmds;
    }

    public void Execute()
    {

        cmds.ForEach(c => c.Execute());

    }
}
