using System.Threading.Tasks;
using MeshSharp.Abstractions;
using MeshSharp.Providers.Contracts;
using Microsoft.JavaScript.NodeApi;

namespace MeshSharp.Providers.Offline;

public class OfflineProvider : NodeObjectProxy, IFetcher
{
    private readonly JSReference _fetcher;

    public OfflineProvider(MeshSdk sdk, JSValue coreModule) : base(sdk)
    {
        var fetcherCtor = coreModule.GetProperty("OfflineFetcher");
        _fetcher = new(fetcherCtor.CallAsConstructor());
    }

    public static async Task<OfflineProvider> CreateAsync(MeshSdk sdk)
    {
        return await sdk.Runtime.RunAsync(async () =>
        {
            var coreModule = await sdk.ImportMeshSdkCoreModule();
            return new OfflineProvider(sdk, coreModule);
        });
    }

    JSReference IFetcher.GetReference()
    {
        return _fetcher;
    }
}