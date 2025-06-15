// ------------------------------------------------------------
// 
// Copyright (c) Jiří Polášek. All rights reserved.
// 
// ------------------------------------------------------------

using System.Globalization;
using System.Text.RegularExpressions;
using Wacton.Unicolour;
using Wacton.Unicolour.Icc;

namespace JPSoftworks.ColorsExtension.Helpers.ColorParser;

public partial class CmykModernColorParser : IColorParser
{
    // Examples: cmyk(0% 69% 51% 92%) or cmyk(0% 69% 51% 92% / 0.5) or cmyk(0 0.69 0.51 0.92)
    //           device-cmyk(0% 69% 51% 92%)
    
    private static readonly Regex CmykPattern = CmykExpressionRegex();

    public ColorParseResult TryParse(string input)
    {
        var match = CmykPattern.Match(input);
        if (!match.Success)
        {
            return ColorParseResult.Fail($"Invalid CMYK format: {input}");
        }

        try
        {
            var c = ParseComponent(match.Groups["cyan"].Value);
            var m = ParseComponent(match.Groups["magenta"].Value);
            var y = ParseComponent(match.Groups["yellow"].Value);
            var k = ParseComponent(match.Groups["black"].Value);
            
            var config = new Configuration(iccConfig: IccConfiguration.None);
            return ColorParseResult.Ok(new Unicolour(config, new Channels(c, m, y, k)), ParsedColorFormat.CmykModern);
        }
        catch (Exception ex)
        {
            return ColorParseResult.Fail($"Error parsing CMYK color: {ex.Message}");
        }
    }

    private static double ParseComponent(string value)
    {
        var component = value.EndsWith('%')
            ? double.Parse(value.TrimEnd('%'), CultureInfo.InvariantCulture) / 100.0
            : double.Parse(value, CultureInfo.InvariantCulture);
        return Math.Clamp(component, 0, 1);
    }

    [GeneratedRegex("""
                      ^                                  # start of string
                      (?<prefix>device-)?cmyk            # optional prefix
                      \s*                                # optional whitespace
                      \( \s*                             # literal (
                        (?<cyan>\d+(?:\.\d+)?%?)         # C
                        (\s+|\s*,\s*)                    # separator: spaces OR comma-with-space
                        (?<magenta>\d+(?:\.\d+)?%?)      # M
                        (\s+|\s*,\s*)                    # separator: spaces OR comma-with-space
                        (?<yellow>\d+(?:\.\d+)?%?)       # Y
                        (\s+|\s*,\s*)                    # separator: spaces OR comma-with-space
                        (?<black>\d+(?:\.\d+)?%?)        # K
                        (                                # optional alpha channel
                          \s*\/\s*
                          (?<alpha>\d+(?:\.\d+)?%?)
                        )?
                        \s* \)                           # closing )
                      $                                  # end of string
                    """, RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture | RegexOptions.IgnorePatternWhitespace)]
    private static partial Regex CmykExpressionRegex();
}