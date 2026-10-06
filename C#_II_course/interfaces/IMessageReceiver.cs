namespace StarshipGame;
using Hwdtech;

public interface IMessageReceiver
{
    public void Receive(Hwdtech.ICommand cmd);
}
