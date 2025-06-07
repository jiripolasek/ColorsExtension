// ------------------------------------------------------------
// 
// Copyright (c) Jiří Polášek. All rights reserved.
// 
// ------------------------------------------------------------

namespace JPSoftworks.ColorsExtension.Helpers.QueryParser;

public record Suggestion(
    SuggestionType Type,
    string CompletionText,
    string? Description,
    int ReplaceStart,
    int ReplaceLength);