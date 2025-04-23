using System;
using System.Threading.Tasks;
using MeshSharp.Abstractions;
using MeshSharp.Providers.Contracts;
using Microsoft.JavaScript.NodeApi;

namespace MeshSharp.Providers.Blockfrost;

public class BlockfrostProvider : NodeObjectProxy, IFetcher, ISubmitter
{
    private readonly JSReference _provider;

    public BlockfrostProvider(MeshSdk sdk, JSValue meshModule, string projectId) : base(sdk)
    {
        var providerCtor = meshModule.GetProperty("BlockfrostProvider");
        ProjectId = projectId;
        _provider = new(providerCtor.CallAsConstructor(projectId));
    }

    public static async Task<BlockfrostProvider> CreateAsync(MeshSdk sdk, string projectId)
    {
        return await sdk.Runtime.RunAsync(async () =>
        {
            var meshModule = await sdk.ImportMeshSdkCoreModule();
            return new BlockfrostProvider(sdk, meshModule, projectId);
        });
    }

    public string ProjectId { get; set; }

    JSReference IFetcher.GetReference()
    {
        return _provider;
    }

    JSReference ISubmitter.GetReference()
    {
        return _provider;
    }
}