﻿// ------------------------------------------------------------
// 
// Copyright (c) Jiří Polášek. All rights reserved.
// 
// ------------------------------------------------------------

namespace JPSoftworks.ColorsExtension.Helpers.ColorManager;

internal interface IColorSet
{
    string Id { get; }

    string Name { get; }

    IReadOnlyDictionary<string, RgbColor> Colors { get; }
}