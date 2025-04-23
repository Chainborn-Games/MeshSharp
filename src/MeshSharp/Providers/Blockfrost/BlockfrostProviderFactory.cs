using System.Threading.Tasks;
using MeshSharp.Providers.Blockfrost;

// ReSharper disable once CheckNamespace
namespace MeshSharp.Providers;

public partial class ProvidersFactory
{
    public async Task<BlockfrostProvider> CreateBlockfrostProviderAsync(string projectId)
    {
        return await BlockfrostProvider.CreateAsync(Sdk, projectId);
    }
}