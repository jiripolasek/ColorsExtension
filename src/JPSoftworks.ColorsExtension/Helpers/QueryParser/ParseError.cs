// ------------------------------------------------------------
// 
// Copyright (c) Jiří Polášek. All rights reserved.
// 
// ------------------------------------------------------------

namespace JPSoftworks.ColorsExtension.Helpers.QueryParser;

public record ParseError(
    ParseErrorType Type,
    string Message,
    int Position,
    int Length)
{
    public override string ToString() => $"{this.Type}: {this.Message} (at position {this.Position})";
}