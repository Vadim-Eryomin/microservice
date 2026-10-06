namespace StarshipGame;

[AttributeUsage(AttributeTargets.Method)]
public class CustomMethodAttribute : Attribute
{
    public string Name { get; }
    public bool IsGet { get; }

    public CustomMethodAttribute(string name, bool isGet)
    {
        Name = name;
        IsGet = isGet;
    }
}
