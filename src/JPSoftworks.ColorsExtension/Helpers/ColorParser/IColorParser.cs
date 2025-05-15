namespace JPSoftworks.ColorsExtension.Helpers.ColorParser;

public interface IColorParser
{
    ColorParseResult TryParse(string input);
}