// ------------------------------------------------------------
// 
// Copyright (c) Jiří Polášek. All rights reserved.
// 
// ------------------------------------------------------------

namespace JPSoftworks.ColorsExtension.Helpers.QueryParser;

internal record SwitchDefinition<TOptions>(
    string Name,
    bool HasArgument,
    Action<TOptions, string?> Handler,
    Func<string?, (bool isValid, string? errorMessage)>? Validator,
    Dictionary<string, string>? Aliases,
    string? Description,
    string[]? ValueSuggestions);