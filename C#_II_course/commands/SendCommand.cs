namespace StarshipGame;
using Hwdtech;

public class SendCommand : Hwdtech.ICommand
{
    private Hwdtech.ICommand _Cmd;
    private IMessageReceiver _Reciever;

    public SendCommand(Hwdtech.ICommand cmd, IMessageReceiver reciever)
    {
        _Cmd = cmd;
        _Reciever = reciever;
    }

    public void Execute()
    {
        _Reciever.Receive(_Cmd);
    }
}
