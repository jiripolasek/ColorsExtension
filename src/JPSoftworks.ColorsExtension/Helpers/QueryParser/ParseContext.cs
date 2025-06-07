// ------------------------------------------------------------
// 
// Copyright (c) Jiří Polášek. All rights reserved.
// 
// ------------------------------------------------------------

namespace JPSoftworks.ColorsExtension.Helpers.QueryParser;

public class ParseContext
{
    public int CursorPosition { get; }
    public List<Suggestion> Suggestions { get; init; } = [];
    public List<ParseWarning> Warnings { get; init; } = [];
    public bool IsIncomplete { get; init; }

    public ParseContext(int cursorPosition)
    {
        this.CursorPosition = cursorPosition;
    }
}