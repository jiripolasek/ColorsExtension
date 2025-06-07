// ------------------------------------------------------------
// 
// Copyright (c) Jiří Polášek. All rights reserved.
// 
// ------------------------------------------------------------

namespace JPSoftworks.ColorsExtension.Helpers.QueryParser;

public record ParseResult<TOptions>(
    string Query,
    TOptions Options,
    IReadOnlyList<ParseError> Errors,
    ParseContext Context)
{
    public bool HasErrors => this.Errors.Count > 0;
    public bool HasWarnings => this.Context.Warnings.Count > 0;
    public bool HasSuggestions => this.Context.Suggestions.Count > 0;

    public override string ToString()
    {
        var result = $"Query: '{this.Query}', {this.Options}";
        if (this.HasErrors)
        {
            result += $", Errors: [{string.Join("; ", this.Errors.Select(e => e.Message))}]";
        }

        if (this.HasWarnings)
        {
            result += $", Warnings: [{string.Join("; ", this.Context.Warnings.Select(w => w.Message))}]";
        }

        if (this.HasSuggestions)
        {
            result += $", Suggestions: {this.Context.Suggestions.Count}";
        }

        return result;
    }
}