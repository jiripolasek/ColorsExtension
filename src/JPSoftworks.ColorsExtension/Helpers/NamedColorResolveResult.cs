// ------------------------------------------------------------
// 
// Copyright (c) Jiří Polášek. All rights reserved.
// 
// ------------------------------------------------------------

using JPSoftworks.ColorsExtension.Helpers.ColorManager;

namespace JPSoftworks.ColorsExtension.Helpers;

internal class NamedColorResolveResult
{
    public bool HasResults { get; }
    public NamedColorResult? BestMatch { get; }
    public List<NamedColorResult> AllMatches { get; }
    public string OriginalQuery { get; }

    public NamedColorResolveResult(string query, List<NamedColorResult>? matches)
    {
        this.OriginalQuery = query;
        this.AllMatches = matches ?? [];
        this.HasResults = this.AllMatches.Count != 0;
        this.BestMatch = this.AllMatches.FirstOrDefault();
    }

    public static NamedColorResolveResult Empty(string query) => new(query, []);
}