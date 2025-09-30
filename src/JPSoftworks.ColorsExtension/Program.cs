// ------------------------------------------------------------
// 
// Copyright (c) Jiří Polášek. All rights reserved.
// 
// ------------------------------------------------------------

using JPSoftworks.CommandPalette.Extensions.Toolkit;

namespace JPSoftworks.ColorsExtension;

public static class Program
{
    [MTAThread]
    public static async Task Main(string[] args)
    {
        await ExtensionHostRunner.RunAsync(
            args,
            new ExtensionHostRunnerParameters
            {
                PublisherMoniker = "JPSoftworks",
                ProductMoniker = "ColorsExtension",
#if DEBUG
                IsDebug = true,
#endif
                EnableEfficiencyMode = true,
                ExtensionFactories = [
                    new DelegateExtensionFactory(manualResetEvent => new ColorsExtension(manualResetEvent))
                ]
            });
    }
}