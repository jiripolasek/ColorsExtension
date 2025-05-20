// ------------------------------------------------------------
// 
// Copyright (c) Jiří Polášek. All rights reserved.
// 
// ------------------------------------------------------------

using System.Runtime.InteropServices;

namespace JPSoftworks.ColorsExtension;

[Guid("adcce948-7b96-46ef-84b7-b884da5b1e92")]
public sealed partial class ColorsExtension : IExtension, IDisposable
{
    private readonly ManualResetEvent _extensionDisposedEvent;

    private readonly ColorsExtensionCommandsProvider _provider = new();

    public ColorsExtension(ManualResetEvent extensionDisposedEvent)
    {
        this._extensionDisposedEvent = extensionDisposedEvent;
    }

    public object? GetProvider(ProviderType providerType)
    {
        return providerType switch
        {
            ProviderType.Commands => this._provider,
            _ => null
        };
    }

    public void Dispose()
    {
        this._extensionDisposedEvent.Set();
    }
}