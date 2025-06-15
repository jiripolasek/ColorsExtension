// ------------------------------------------------------------
// 
// Copyright (c) Jiří Polášek. All rights reserved.
// 
// ------------------------------------------------------------

using JPSoftworks.ColorsExtension.Helpers.ColorParser;

namespace JPSoftworks.ColorsExtension.Helpers;

internal sealed class CombinedParseResult
{
    public bool HasExactMatch { get; }
    public ColorParseResult? ExactResult { get; }
    public NamedColorResolveResult? NamedResult { get; }
    public string OriginalQuery { get; }
    public CombinedParseStrategy Strategy { get; }

    public CombinedParseResult(string query, ColorParseResult? exactResult, NamedColorResolveResult? namedResult)
    {
        this.OriginalQuery = query;
        this.ExactResult = exactResult;
        this.HasExactMatch = exactResult?.Success == true;
        this.NamedResult = namedResult;

        this.Strategy = this.HasExactMatch ? CombinedParseStrategy.ExactMatch :
            namedResult!.AllMatches.Count == 1 ? CombinedParseStrategy.AutoSelectNamed :
            namedResult.HasResults ? CombinedParseStrategy.ShowNamedOptions :
            CombinedParseStrategy.NoMatch;
    }
}