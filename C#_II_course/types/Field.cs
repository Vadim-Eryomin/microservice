namespace StarshipGame;

public class Field(
    string type,
    string name,
    bool isDefaultGetter,
    bool isDefaultSetter,
    string? customGetter,
    string? customSetter,
    bool needSetter)
{
    public string Type => type;
    public string Name => name;
    public bool IsDefaultGetter => isDefaultGetter;
    public bool IsDefaultSetter => isDefaultSetter;
    public string? CustomGetter => customGetter;
    public string? CustomSetter => customSetter;
    public bool NeedSetter => needSetter;
}
