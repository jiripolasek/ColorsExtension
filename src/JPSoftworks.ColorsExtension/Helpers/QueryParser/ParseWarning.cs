namespace JPSoftworks.ColorsExtension.Helpers.QueryParser;

public record ParseWarning(
    string Message,
    int Position,
    int Length);