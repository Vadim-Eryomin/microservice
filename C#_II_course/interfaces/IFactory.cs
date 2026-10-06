namespace StarshipGame;

public interface IFactory
{
    public object Adapt(IDictionary<object, object> startObject);
}
