using System.Threading.Tasks;
using MeshSharp.Providers.Offline;

// ReSharper disable once CheckNamespace
namespace MeshSharp.Providers;

public partial class ProvidersFactory
{
    public async Task<OfflineProvider> CreateOfflineProviderAsync()
    {
        return await OfflineProvider.CreateAsync(Sdk);
    }
}