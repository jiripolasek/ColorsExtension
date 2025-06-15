// ------------------------------------------------------------
// 
// Copyright (c) Jiří Polášek. All rights reserved.
// 
// ------------------------------------------------------------

using System.Text.Json.Nodes;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace JPSoftworks.ColorsExtension.Services.Settings;

public sealed class SettingsManager : JsonSettingsManager
{
    public SettingsManager()
    {
        this.Settings.Add(new TextBlockSetting
        {
            Value = """
                    Created by Jiri Polasek [https://jiripolasek.com](https://jiripolasek.com)
                    _Licensed under the Apache 2.0 license_
                    _Copyright © Jiri Polasek_
                    """,
            IsSubtle = true
        });
        this.Settings.Add(new TextBlockSetting
        {
            Value = """
                    Unicolour library is used for color conversion [https://unicolour.wacton.xyz/](https://unicolour.wacton.xyz/)
                    _Licensed under the MIT license_
                    _Copyright © 2022-2025 William Acton_
                    """,
            IsSubtle = true
        });

        this.FilePath = SettingsJsonPath();
        this.LoadSettings();
        this.Settings.SettingsChanged += (_, _) => this.SaveSettings();
    }

    private static string SettingsJsonPath()
    {
        var directory = Utilities.BaseSettingsPath("Microsoft.CmdPal");
        Directory.CreateDirectory(directory);
        return Path.Combine(directory, "settings.json");
    }
}

public sealed class TextBlockSetting : Setting<string>
{
    public bool IsSubtle { get; init; }

    public TextBlockSetting() : base(Guid.NewGuid().ToString(), "", "", "")
    {

    }

    public override void Update(JsonObject payload)
    {
        // no-op
    }

    public override string ToState()
    {
        return string.Empty;
    }

    public override Dictionary<string, object> ToDictionary()
    {
        return new Dictionary<string, object>()
        {
            { "type", "TextBlock" },
            { "text", this.Value ?? "" },
            { "isSubtle", this.IsSubtle }
        };
    }
}