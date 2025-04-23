using System.Threading.Tasks;
using MeshSharp.Abstractions;

namespace MeshSharp.Wallet;

public class MeshWalletFactory(MeshSdk sdk) : NodeObjectProxy(sdk)
{
    public async Task<string> BrewAsync()
    {
        return await RunAsync(async () =>
        {
            var meshModule = await ImportMeshSdkCoreModule();
            var typeOf = meshModule.GetProperty("resolvePrivateKey").TypeOf(); 

            var walletClass = meshModule.GetProperty("MeshWallet");
            var result = walletClass.CallMethod("brew", true);
            return (string)result;
        });
    }

    public async Task<MeshWalletProxy> CreateAsync(MeshWalletConfig config)
    {
        return await Runtime.RunAsync(async () =>
        {
            var meshModule = await ImportMeshSdkCoreModule();
            return MeshWalletProxy.Create(Sdk, meshModule, config);
        });
    }
}