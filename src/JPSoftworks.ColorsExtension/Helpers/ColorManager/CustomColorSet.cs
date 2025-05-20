namespace JPSoftworks.ColorsExtension.Helpers.ColorManager;

public class CustomColorSet : IColorSet
{
    public CustomColorSet(string name, Dictionary<string, (int r, int g, int b)> colors)
    {
        this.Name = name;
        this.Colors = colors;
    }

    public string Name { get; }
    public IReadOnlyDictionary<string, (int r, int g, int b)> Colors { get; }
}