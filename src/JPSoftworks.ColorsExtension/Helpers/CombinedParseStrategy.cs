// ------------------------------------------------------------
// 
// Copyright (c) Jiří Polášek. All rights reserved.
// 
// ------------------------------------------------------------

namespace JPSoftworks.ColorsExtension.Helpers;

internal enum CombinedParseStrategy
{
    /// <summary>
    /// Use exact result immediately
    /// </summary>
    ExactMatch,

    /// <summary>
    /// Use best named match automatically
    /// </summary>
    AutoSelectNamed,

    /// <summary>
    /// Show user a list to choose from
    /// </summary>
    ShowNamedOptions,

    /// <summary>
    /// No results found
    /// </summary>
    NoMatch
}