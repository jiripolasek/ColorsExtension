// ------------------------------------------------------------
// 
// Copyright (c) Jiří Polášek. All rights reserved.
// 
// ------------------------------------------------------------

namespace JPSoftworks.ColorsExtension.Helpers.QueryParser;

internal record IncompleteSwitchInfo(
    string PartialName,
    bool HasSeparator,
    string? PartialValue,
    int Position);