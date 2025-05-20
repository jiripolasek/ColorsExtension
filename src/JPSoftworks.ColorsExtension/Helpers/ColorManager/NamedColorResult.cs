namespace JPSoftworks.ColorsExtension.Helpers.ColorManager;

public class NamedColorResult
{
    public bool Success { get; }
    public string? ColorName { get; }
    public string? ColorSet { get; }
    public (int r, int g, int b)? Rgb { get; }

    private NamedColorResult(bool success, string? colorName, string? colorSet, (int r, int g, int b)? rgb)
    {
        this.Success = success;
        this.ColorName = colorName;
        this.ColorSet = colorSet;
        this.Rgb = rgb;
    }

    public static NamedColorResult Ok(string colorName, string colorSet, (int r, int g, int b) rgb)
    {
        return new NamedColorResult(true, colorName, colorSet, rgb);
    }

    public static NamedColorResult Fail()
    {
        return new NamedColorResult(false, null, null, null);
    }

    public string GetQualifiedName()
    {
        if (!this.Success)
        {
            return "";
        }

        return $"{this.ColorName} ({this.ColorSet})";
    }
}